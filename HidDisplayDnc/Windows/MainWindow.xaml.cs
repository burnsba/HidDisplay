﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using BurnsBac.WinApi.Error;
using BurnsBac.WinApi.Hid;
using BurnsBac.WinApi.User32;
using BurnsBac.WindowsAppToolkit;
using BurnsBac.WindowsAppToolkit.Mvvm;
using BurnsBac.WindowsAppToolkit.ViewModels;
using BurnsBac.WindowsAppToolkit.Windows;
using BurnsBac.WindowsHardware.HardwareWatch;
using HidDisplay.PluginDefinition;
using HidDisplay.SkinModel.Core;
using HidDisplay.SkinModel.Core.Display;
using HidDisplay.SkinModel.InputSourceDescription;
using HidDisplayDnc.ViewModels;

namespace HidDisplayDnc.Windows
{
    using Expr = System.Linq.Expressions.Expression;

    /// <summary>
    /// Interaction logic for MainWindow.xaml .
    /// </summary>
    public partial class MainWindow : Window, ICloseable
    {
        /// <summary>
        /// Timers for flash events.
        /// </summary>
        public Dictionary<Type, Dictionary<UInt64, Timer>> _hwFlashTimers;

        private List<IPassiveTranslate<HidResult>> _passiveHidPlugins = new List<IPassiveTranslate<HidResult>>();
        private List<IPassiveTranslate<RawMouse>> _passiveRawMousePlugins = new List<IPassiveTranslate<RawMouse>>();
        private RawInputHandler _rih = new RawInputHandler();
        private Dictionary<Type, System.Reflection.MethodInfo> _simpleToStringMethodCache = new Dictionary<Type, System.Reflection.MethodInfo>();

        /// <summary>
        /// Main view model.
        /// </summary>
        private MainViewModel _vm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;

            _vm = new MainViewModel();

            DataContext = _vm;

            DisplayGrid.Children.Clear();
        }

        /// <summary>
        /// Window closing handler. Tries to stop event listeners before exiting.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            UnloadSelectedSkin();

            _rih.Dispose();
        }

        /// <summary>
        /// Windows start event to hook raw input events.
        /// </summary>
        /// <param name="e">Args.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            var win = source.Handle;

            RawInputDevice[] rid = new RawInputDevice[2];

            rid[0].UsagePage = HidUsagePages.GenericDesktop;
            rid[0].Usage = (ushort)BurnsBac.WinApi.Hid.Usage.GenericDesktop.GamePad;
            rid[0].Flags = RawInputDeviceFlags.InputSink;
            rid[0].WindowHandle = win;

            rid[1].UsagePage = HidUsagePages.GenericDesktop;
            rid[1].Usage = (ushort)BurnsBac.WinApi.Hid.Usage.GenericDesktop.Joystick;
            rid[1].Flags = RawInputDeviceFlags.InputSink;
            rid[1].WindowHandle = win;

            rid[1].UsagePage = HidUsagePages.GenericDesktop;
            rid[1].Usage = (ushort)BurnsBac.WinApi.Hid.Usage.GenericDesktop.Mouse;
            rid[1].Flags = RawInputDeviceFlags.InputSink;
            rid[1].WindowHandle = win;

            if (BurnsBac.WinApi.User32.Api.RegisterRawInputDevices(rid, (uint)rid.Length, (uint)Marshal.SizeOf(rid[0])) == false)
            {
                var err = Marshal.GetLastWin32Error();
                if (err > 0)
                {
                    throw new Win32ErrorCode($"GetLastWin32Error: {err}");
                }
                else
                {
                    throw new Win32ErrorCode("RegisterRawInputDevices failed with error code 0. Parameter count mis-match?");
                }
            }
        }

        private bool AnyWndProcListeners()
        {
            return _passiveHidPlugins.Any() || _passiveRawMousePlugins.Any();
        }

        private FrameworkElement BuildFrameworkElement(InputHandlerItem descriptionItem)
        {
            FrameworkElement wpfObject;

            if (HidDisplay.SkinModel.Core.UiHelper.IsImage(descriptionItem.Ui))
            {
                wpfObject = ImageHelper($"DYN_{descriptionItem.HwTypeName}_{descriptionItem.Name}_{descriptionItem.Hw.Id}_{Guid.NewGuid().ToString("n")}", ((IUiImageItem)descriptionItem.Ui).Image);
            }
            else if (HidDisplay.SkinModel.Core.UiHelper.IsText(descriptionItem.Ui))
            {
                wpfObject = TextHelper($"DYN_{descriptionItem.HwTypeName}_{descriptionItem.Name}_{descriptionItem.Hw.Id}_{Guid.NewGuid().ToString("n")}", ((IUiTextItem)descriptionItem.Ui).TextInfo);
            }
            else
            {
                throw new NotSupportedException();
            }

            return wpfObject;
        }

        /// <summary>
        /// Config button handler.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">Args.</param>
        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            var p = BurnsBac.HotConfig.TypeResolver.ConfigDataProvidersDirectory;
            var skinSettingsPath = System.IO.Path.Combine(_vm.SelectedSkin.SkinDirectoryPath, HidDisplay.SkinModel.Constants.SkinSettingsFilename);
            Workspace.CreateSingletonWindow<ConfigWindow>(skinSettingsPath);
        }

        /// <summary>
        /// Load button handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            UnloadSelectedSkin();
            LoadSelectedSkin();
        }

        /// <summary>
        /// Unload button handler.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Args.</param>
        private void ButtonUnload_Click(object sender, RoutedEventArgs e)
        {
            UnloadSelectedSkin();
        }

        /// <summary>
        /// Stops a timer for an event.
        /// </summary>
        /// <param name="type">Concrete type of <see cref="IInputSource" />.</param>
        /// <param name="eventSourceId">Event source id.</param>
        private void CancelFlashTimer(Type type, UInt64 eventSourceId)
        {
            // throw if cancelling something that doesn't exist.
            _hwFlashTimers[type][eventSourceId].Stop();
        }

        /// <summary>
        /// Stops all timers associated with flash buttons.
        /// Clears references to existing timers.
        /// </summary>
        private void DisposeAllFlashTimers()
        {
            if (object.ReferenceEquals(null, _hwFlashTimers))
            {
                return;
            }

            foreach (var kvp in _hwFlashTimers)
            {
                if (!object.ReferenceEquals(null, kvp.Value))
                {
                    foreach (var kvp2 in kvp.Value)
                    {
                        kvp2.Value.Stop();
                    }

                    kvp.Value.Clear();
                }
            }

            _hwFlashTimers.Clear();
        }

        /// <summary>
        /// Creates a new <see cref="System.Windows.Controls.Image"/>. Loads the image from disk, and adds to DisplayGrid.
        /// </summary>
        /// <param name="name">WPF control name.</param>
        /// <param name="image">Image info.</param>
        private FrameworkElement ImageHelper(string name, ImageInfo image)
        {
            var wpfImage = new Image();
            wpfImage.Name = name;

            image.LoadImageFromDisk();

            wpfImage.Source = image.ImageData;
            wpfImage.Width = image.Width;
            wpfImage.Height = image.Height;
            wpfImage.Margin = new Thickness()
            {
                Left = image.XOffset,
                Top = image.YOffset,
                Right = 0,
                Bottom = 0,
            };
            wpfImage.HorizontalAlignment = HorizontalAlignment.Left;
            wpfImage.VerticalAlignment = VerticalAlignment.Top;
            wpfImage.Visibility = Visibility.Hidden;

            DisplayGrid.Children.Add(wpfImage);

            return wpfImage;
        }

        /// <summary>
        /// Loads skin definition from disk. Sets up everything and starts event handlers.
        /// </summary>
        /// <remarks>
        /// Should be able to change xml file while the app is running, unload, then load
        /// and pickup new changes.
        /// </remarks>
        private void LoadSelectedSkin()
        {
            Skin skin = null;

            try
            {
                skin = Skin.FromXmlFile(_vm.SelectedSkin.SkinXmlPath);
            }
            catch (Exception ex)
            {
                Workspace.RecreateSingletonWindow<ErrorWindow>(new ErrorWindowViewModel(ex)
                {
                    HeaderMessage = "Error parsing skin definition file",
                });

                return;
            }

            _vm.ActiveSkin = skin;

            DisplayGrid.Children.Clear();

            var background = ImageHelper("SkinBackground", _vm.ActiveSkin.BackgroundImage);
            background.Visibility = Visibility.Visible;

            _vm.ActiveSkin.SetupCallback += SetupUpdateHandler;

            try
            {
                _vm.ActiveSkin.Activate();
            }
            catch (Exception ex)
            {
                Workspace.RecreateSingletonWindow<ErrorWindow>(new ErrorWindowViewModel(ex)
                {
                    HeaderMessage = "Error when activating skin",
                });

                UnloadSelectedSkin();

                return;
            }

            foreach (var handler in skin.InputHandlers)
            {
                if (typeof(IPassiveTranslate<HidResult>).IsAssignableFrom(handler.Handler.GetType()))
                {
                    _passiveHidPlugins.Add((IPassiveTranslate<HidResult>)handler.Handler);
                }
                else if (typeof(IPassiveTranslate<RawMouse>).IsAssignableFrom(handler.Handler.GetType()))
                {
                    _passiveRawMousePlugins.Add((IPassiveTranslate<RawMouse>)handler.Handler);
                }
            }
        }

        /// <summary>
        /// Stops a timer for an existing event and restarts it. If the timer
        /// does not exist, it will be created.
        /// </summary>
        /// <param name="type">Concrete type of <see cref="IInputSource" />.</param>
        /// <param name="eventSourceId">Event source id.</param>
        /// <param name="interval">Timer interval in ms if timer is created.</param>
        /// <param name="act">Elapsed action to be performed if timer is created.</param>
        private void RefreshFlashTimer(Type type, UInt64 eventSourceId, int interval, Action act)
        {
            if (object.ReferenceEquals(null, _hwFlashTimers))
            {
                _hwFlashTimers = new Dictionary<Type, Dictionary<ulong, Timer>>();
            }

            Dictionary<UInt64, Timer> hwtimers = null;

            if (!_hwFlashTimers.TryGetValue(type, out hwtimers))
            {
                hwtimers = new Dictionary<ulong, Timer>();

                // don't forget to save a reference to the new collection
                _hwFlashTimers[type] = hwtimers;
            }

            Timer timer = null;

            if (!hwtimers.TryGetValue(eventSourceId, out timer))
            {
                timer = new Timer();
                timer.AutoReset = false;
                timer.Interval = interval;
                timer.Elapsed += (s, e) => act();

                // don't forget to save a reference to the new timer
                _hwFlashTimers[type][eventSourceId] = timer;

                //////System.Diagnostics.Debug.WriteLine($"new timer: {type.Name} {eventSourceId}");
            }
            else
            {
                timer.Stop();

                //////System.Diagnostics.Debug.WriteLine($"restart timer: {type.Name} {eventSourceId}");
            }

            timer.Start();
        }

        /// <summary>
        /// Core functionality of app. Maps hardware events to ui stuff.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="inputHandler">InputHandler to process.</param>
        private void SetupUpdateHandler(object sender, InputHandler inputHandler)
        {
            var b2DescriptionItems = inputHandler.Items.Where(x => x.HwType.Name == nameof(Button2));
            var b3DescriptionItems = inputHandler.Items.Where(x => x.HwType.Name == nameof(Button3));
            var rangeableDescriptionItems = inputHandler.Items.Where(x => x.HwType.Name == nameof(IRangeableInput));
            var rangeable2DescriptionItems = inputHandler.Items.Where(x => x.HwType.Name == nameof(IRangeableInput2));
            var rangeable3DescriptionItems = inputHandler.Items.Where(x => x.HwType.Name == nameof(IRangeableInput3));

            // The plan is to break down InputHandlerItems by hardware type.
            // For each hardware type, build a method block that will match incoming events by hw ids.
            // Something like
            //     inputHandler.Handler.InputEvent += (s, e) =>
            //     {
            //         processButton2s(e);
            //         processButton3s(e);
            //         ...
            //     };

            // Trying to build a for-each loop in expression trees is just so very terrible, I'm
            // using intermediate actions to wrap the loops.
            //     Action<List<Button2>> processButton2s = (col) => {
            //         foreach (var item in col)
            //         {
            //             b2BlockMethod(item);
            //         }
            //     };

            // This hardware specific block will accept a parameter which will be the iteratation
            // of the foreach above. This will check the specific hardware ids defined in the skin xml.

            // The actual event handler method is another expression built based on the skin contents.
            // So it's not
            //     inputHandler.Handler.InputEvent += (s, e) =>
            //     {
            //         if (e.Button2s.Any())
            //             processButton2s(e);
            //         if (e.Button3s.Any())
            //             processButton3s(e);
            //     };
            // the method handler for each input type is either there or it's not.
            ///////////////

            var inputEventHandlerExpressions = new List<Expr>();
            var s = Expr.Parameter(typeof(System.Object), "sender");
            var e = Expr.Parameter(typeof(GenericInputEventArgs), "e");

            if (b2DescriptionItems.Any())
            {
                var b2IdMatchIfExpressions = new List<Expr>();
                var b2 = Expr.Parameter(typeof(Button2), "b2");

                foreach (var descriptionItem in b2DescriptionItems)
                {
                    // also adds FrameworkElement to DisplayGrid UI grid.
                    FrameworkElement wpfObject = BuildFrameworkElement(descriptionItem);

                    Action fshow = EventWpfUtility.MakeShowAction(wpfObject);
                    Action fhide = EventWpfUtility.MakeHideAction(wpfObject);

                    MethodCallExpression singleIdActionExpression = null;

                    if (descriptionItem.UiType == UiType.ToggleButton)
                    {
                        Action<Button2> action = arg =>
                        {
                            if (arg.Id == descriptionItem.Hw.Id)
                            {
                                if (arg.State == ((Button2Description)descriptionItem.Hw).StateMatch)
                                {
                                    Dispatcher.Invoke(fshow);
                                }
                                else
                                {
                                    Dispatcher.Invoke(fhide);
                                }
                            }
                        };

                        singleIdActionExpression = Expr.Call(Expr.Constant(action.Target), action.Method, b2);
                    }
                    else if (descriptionItem.UiType == UiType.FlashButton)
                    {
                        Action<Button2> action = arg =>
                        {
                            if (arg.Id == descriptionItem.Hw.Id)
                            {
                                if (arg.State == ((Button2Description)descriptionItem.Hw).StateMatch)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        wpfObject.Visibility = Visibility.Visible;
                                        RefreshFlashTimer(
                                            typeof(Button2),
                                            arg.EventSourceId,
                                            ((FlashButton)descriptionItem.Ui).DisplayDurationMs,
                                            () => Dispatcher.Invoke(fhide));
                                    });
                                }
                            }
                        };

                        singleIdActionExpression = Expr.Call(Expr.Constant(action.Target), action.Method, b2);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    b2IdMatchIfExpressions.Add(singleIdActionExpression);
                }

                var b2Block = Expr.Block(b2IdMatchIfExpressions);
                var b2BlockMethod = Expr.Lambda<Action<Button2>>(b2Block, b2).Compile();

                Action<GenericInputEventArgs> processButton2s = (e) =>
                {
                    foreach (var item in e.Button2s)
                    {
                        b2BlockMethod(item);
                    }
                };

                inputEventHandlerExpressions.Add(Expr.Call(Expr.Constant(processButton2s.Target), processButton2s.Method, e));
            }

            if (b3DescriptionItems.Any())
            {
                var b3IdMatchIfExpressions = new List<Expr>();
                var b3 = Expr.Parameter(typeof(Button3), "b3");

                foreach (var descriptionItem in b3DescriptionItems)
                {
                    // also adds FrameworkElement to DisplayGrid UI grid.
                    FrameworkElement wpfObject = BuildFrameworkElement(descriptionItem);

                    Action fshow = EventWpfUtility.MakeShowAction(wpfObject);
                    Action fhide = EventWpfUtility.MakeHideAction(wpfObject);

                    MethodCallExpression singleIdActionExpression = null;

                    if (descriptionItem.UiType == UiType.ToggleButton)
                    {
                        Action<Button3> action = arg =>
                        {
                            if (arg.Id == descriptionItem.Hw.Id)
                            {
                                if (arg.State == ((Button3Description)descriptionItem.Hw).StateMatch)
                                {
                                    Dispatcher.Invoke(fshow);
                                }
                                else
                                {
                                    Dispatcher.Invoke(fhide);
                                }
                            }
                        };

                        singleIdActionExpression = Expr.Call(Expr.Constant(action.Target), action.Method, b3);
                    }
                    else if (descriptionItem.UiType == UiType.FlashButton)
                    {
                        Action<Button3> action = arg =>
                        {
                            if (arg.Id == descriptionItem.Hw.Id)
                            {
                                if (arg.State == ((Button3Description)descriptionItem.Hw).StateMatch)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        wpfObject.Visibility = Visibility.Visible;
                                        RefreshFlashTimer(
                                            typeof(Button3),
                                            arg.EventSourceId,
                                            ((FlashButton)descriptionItem.Ui).DisplayDurationMs,
                                            () => Dispatcher.Invoke(fhide));
                                    });
                                }
                            }
                        };

                        singleIdActionExpression = Expr.Call(Expr.Constant(action.Target), action.Method, b3);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    b3IdMatchIfExpressions.Add(singleIdActionExpression);
                }

                var b3Block = Expr.Block(b3IdMatchIfExpressions);
                var b3BlockMethod = Expr.Lambda<Action<Button3>>(b3Block, b3).Compile();

                Action<GenericInputEventArgs> processButton3s = (e) =>
                {
                    foreach (var item in e.Button3s)
                    {
                        b3BlockMethod(item);
                    }
                };

                inputEventHandlerExpressions.Add(Expr.Call(Expr.Constant(processButton3s.Target), processButton3s.Method, e));
            }

            if (rangeableDescriptionItems.Any())
            {
                var r1IdMatchIfExpressions = new List<Expr>();
                var r1 = Expr.Parameter(typeof(IRangeableInput), "r1");

                foreach (var descriptionItem in rangeableDescriptionItems)
                {
                    // also adds FrameworkElement to DisplayGrid UI grid.
                    FrameworkElement wpfObject = BuildFrameworkElement(descriptionItem);

                    Action fshow = EventWpfUtility.MakeShowAction(wpfObject);
                    Action fhide = EventWpfUtility.MakeHideAction(wpfObject);

                    MethodCallExpression singleIdActionExpression = null;

                    if (descriptionItem.UiType == UiType.SimpleToString)
                    {
                        Action<IRangeableInput> action = arg =>
                        {
                            if (arg.Id == descriptionItem.Hw.Id)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    var textItem = (IUiTextItem)descriptionItem.Ui;
                                    var toStringParams = textItem.TextInfo.ToStringFormatParameters;

                                    wpfObject.Visibility = Visibility.Visible;

                                    if (!string.IsNullOrEmpty(toStringParams))
                                    {
                                        System.Reflection.MethodInfo toStringMethod;

                                        if (!_simpleToStringMethodCache.TryGetValue(arg.RangeInfo.BaseType, out toStringMethod))
                                        {
                                            toStringMethod = arg.RangeInfo.BaseType.GetMethod(nameof(object.ToString), new Type[] { typeof(string) });
                                            _simpleToStringMethodCache.Add(arg.RangeInfo.BaseType, toStringMethod);
                                        }

                                        ((TextBlock)wpfObject).Text = (string)toStringMethod.Invoke(arg.Value, new object[] { toStringParams });
                                    }
                                    else
                                    {
                                        ((TextBlock)wpfObject).Text = arg.Value.ToString();
                                    }
                                });
                            }
                        };

                        singleIdActionExpression = Expr.Call(Expr.Constant(action.Target), action.Method, r1);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    r1IdMatchIfExpressions.Add(singleIdActionExpression);
                }

                var r1Block = Expr.Block(r1IdMatchIfExpressions);
                var r1BlockMethod = Expr.Lambda<Action<IRangeableInput>>(r1Block, r1).Compile();

                Action<GenericInputEventArgs> processRangeable1s = (e) =>
                {
                    foreach (var item in e.RangeableInputs)
                    {
                        r1BlockMethod(item);
                    }
                };

                inputEventHandlerExpressions.Add(Expr.Call(Expr.Constant(processRangeable1s.Target), processRangeable1s.Method, e));
            }

            if (rangeable2DescriptionItems.Any())
            {
                var r2IdMatchIfExpressions = new List<Expr>();
                var r2 = Expr.Parameter(typeof(IRangeableInput2), "r2");

                foreach (var descriptionItem in rangeable2DescriptionItems)
                {
                    if (descriptionItem.UiType == UiType.RadialVector)
                    {
                        // supported!
                        // also adds FrameworkElement to DisplayGrid UI grid.
                        FrameworkElement wpfObject = BuildFrameworkElement(descriptionItem);

                        wpfObject.RenderTransformOrigin = new Point(0.5, 0.5);

                        Action<IRangeableInput2> action = arg =>
                        {
                            try
                            {
                                if (arg.Id == descriptionItem.Hw.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        if (arg.IsEmpty)
                                        {
                                            wpfObject.Visibility = Visibility.Hidden;
                                        }
                                        else
                                        {
                                            var transform = EventWpfUtility.RadialVector2Transform(arg.Value1.ValueDouble, arg.Value2.ValueDouble, descriptionItem);
                                            wpfObject.RenderTransform = transform;
                                            wpfObject.Visibility = Visibility.Visible;
                                        }
                                    });
                                }
                            }
                            catch (System.Threading.Tasks.TaskCanceledException)
                            {
                            }
                        };

                        var process = Expr.Call(Expr.Constant(action.Target), action.Method, r2);

                        r2IdMatchIfExpressions.Add(process);
                    }
                    else if (descriptionItem.UiType == UiType.SimpleToString)
                    {
                        // also adds FrameworkElement to DisplayGrid UI grid.
                        FrameworkElement wpfObject = BuildFrameworkElement(descriptionItem);

                        MethodCallExpression singleIdActionExpression = null;

                        Action<IRangeableInput2> action = arg =>
                        {
                            if (arg.Id == descriptionItem.Hw.Id)
                            {
                                Dispatcher.Invoke(() =>
                                {
                                    var textItem = (IUiTextItem)descriptionItem.Ui;
                                    var toStringParams = textItem.TextInfo.ToStringFormatParameters;

                                    wpfObject.Visibility = Visibility.Visible;

                                    if (!string.IsNullOrEmpty(toStringParams))
                                    {
                                        System.Reflection.MethodInfo toStringMethod;
                                        var argType = arg.GetType();

                                        if (!_simpleToStringMethodCache.TryGetValue(argType, out toStringMethod))
                                        {
                                            toStringMethod = argType.GetMethod(nameof(object.ToString), new Type[] { typeof(string) });
                                            _simpleToStringMethodCache.Add(argType, toStringMethod);
                                        }

                                        ((TextBlock)wpfObject).Text = (string)toStringMethod.Invoke(arg, new object[] { toStringParams });
                                    }
                                    else
                                    {
                                        ((TextBlock)wpfObject).Text = arg.ToString();
                                    }
                                });
                            }
                        };

                        var process = Expr.Call(Expr.Constant(action.Target), action.Method, r2);

                        r2IdMatchIfExpressions.Add(process);
                    }
                    else
                    {
                        throw new NotSupportedException($"Only {nameof(UiType.RadialVector)} is allowed for {nameof(IRangeableInput2)}");
                    }
                }

                var r2Block = Expr.Block(r2IdMatchIfExpressions);
                var r2BlockMethod = Expr.Lambda<Action<IRangeableInput2>>(r2Block, r2).Compile();

                Action<GenericInputEventArgs> processIRangeableInput2s = (e) =>
                {
                    foreach (var item in e.RangeableInput2s)
                    {
                        r2BlockMethod(item);
                    }
                };

                inputEventHandlerExpressions.Add(Expr.Call(Expr.Constant(processIRangeableInput2s.Target), processIRangeableInput2s.Method, e));
            }

            if (rangeable3DescriptionItems.Any())
            {
                throw new NotSupportedException();
            }

            if (inputEventHandlerExpressions.Any())
            {
                var inputEventHandlerBody = Expr.Block(inputEventHandlerExpressions);
                var x = Expr.Lambda<EventHandler<GenericInputEventArgs>>(inputEventHandlerBody, s, e).Compile();
                inputHandler.Handler.UpdateEvent += x;
            }
        }

        private FrameworkElement TextHelper(string name, TextInfo textThing)
        {
            var wpfTextblock = new TextBlock();

            wpfTextblock.Name = name;

            wpfTextblock.FontFamily = new FontFamily(textThing.Font);
            wpfTextblock.FontSize = textThing.FontSize;

            wpfTextblock.Margin = new Thickness()
            {
                Left = textThing.XOffset,
                Top = textThing.YOffset,
                Right = 0,
                Bottom = 0,
            };
            wpfTextblock.HorizontalAlignment = HorizontalAlignment.Left;
            wpfTextblock.VerticalAlignment = VerticalAlignment.Top;
            wpfTextblock.Visibility = Visibility.Hidden;

            DisplayGrid.Children.Add(wpfTextblock);

            return wpfTextblock;
        }

        /// <summary>
        /// Unloads skin. Stops all timers. Calls dispose, which should stop all event handlers.
        /// Should clear and free up everything to be able to reload the skin at runtime.
        /// </summary>
        private void UnloadSelectedSkin()
        {
            if (object.ReferenceEquals(null, _vm.ActiveSkin))
            {
                return;
            }

            DisposeAllFlashTimers();
            _hwFlashTimers = null;

            _vm.ActiveSkin.Dispose();
            _vm.ActiveSkin = null;

            DisplayGrid.Children.Clear();

            _passiveHidPlugins.Clear();
            _passiveRawMousePlugins.Clear();

            _simpleToStringMethodCache.Clear();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (!AnyWndProcListeners())
            {
                return hwnd;
            }

            switch (msg)
            {
                case (int)BurnsBac.WinApi.Windows.WindowsMessages.INPUT:
                    {
                        //////System.Diagnostics.Debug.WriteLine("Received WndProc.WM_INPUT");

                        var ri = _rih.WndProcToRawInput(lParam);

                        if (ri.Header.dwType == RawInputDeviceType.Mouse)
                        {
                            if (_passiveRawMousePlugins.Any())
                            {
                                foreach (var plugin in _passiveRawMousePlugins)
                                {
                                    plugin.AcceptMessage(this, ri.Data.Mouse);
                                }
                            }
                        }
                        else if (ri.Header.dwType == RawInputDeviceType.Hid)
                        {
                            if (_passiveHidPlugins.Any())
                            {
                                var hidResult = _rih.RawInputHidDetail(ri);

                                foreach (var plugin in _passiveHidPlugins)
                                {
                                    plugin.AcceptMessage(this, hidResult);
                                }
                            }
                        }
                    }

                    break;
            }

            return hwnd;
        }
    }
}
