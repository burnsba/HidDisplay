using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WinApi.Windows;

namespace WinApi.User32
{
    /// <summary>
    /// Function definitions for user32.dll.
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// Passes the hook information to the next hook procedure in the current
        /// hook chain. A hook procedure can call this function either before or after
        /// processing the hook information.
        /// </summary>
        /// <param name="hhk">
        /// This parameter is ignored.
        /// </param>
        /// <param name="nCode">
        /// The hook code passed to the current hook procedure. The next hook procedure
        /// uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        /// The wParam value passed to the current hook procedure. The meaning of this
        /// parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        /// The lParam value passed to the current hook procedure. The meaning of this
        /// parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        /// This value is returned by the next hook procedure in the chain. The current
        /// hook procedure must also return this value. The meaning of the return value
        /// depends on the hook type. For more information, see the descriptions of the
        /// individual hook procedures.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-callnexthookex
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(
            IntPtr hhk,
            int nCode,
            IntPtr wParam,
            IntPtr lParam
        );

        /// <summary>
        /// Dispatches a message to a window procedure. It is typically used to
        /// dispatch a message retrieved by the GetMessage function.
        /// </summary>
        /// <param name="lpmsg">A pointer to a structure that contains the message.</param>
        /// <returns>The return value specifies the value returned by the window procedure.
        /// Although its meaning depends on the message being dispatched, the return value
        /// generally is ignored.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-dispatchmessage
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage(
            [In] ref WinUserMessage lpmsg
        );

        /// <summary>
        /// Retrieves a handle to the foreground window (the window with which
        /// the user is currently working). The system assigns a slightly higher
        /// priority to the thread that creates the foreground window than it
        /// does to other threads.
        /// </summary>
        /// <returns>
        /// The return value is a handle to the foreground window. The foreground
        /// window can be NULL in certain circumstances, such as when a
        /// window is losing activation.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Retrieves a message from the calling thread's message queue. The function dispatches
        /// incoming sent messages until a posted message is available for retrieval.
        /// Unlike GetMessage, the PeekMessage function does not wait for a message to be posted before returning.
        /// </summary>
        /// <param name="lpMsg">A pointer to an MSG structure that receives message information from the thread's message queue.</param>
        /// <param name="hWnd">
        /// A handle to the window whose messages are to be retrieved. The window must belong to the current thread.
        /// If hWnd is NULL, GetMessage retrieves messages for any window that belongs to the current thread, and
        /// any messages on the current thread's message queue whose hwnd value is NULL (see the MSG structure).
        /// Therefore if hWnd is NULL, both window messages and thread messages are processed. If hWnd is -1,
        /// GetMessage retrieves only messages on the current thread's message queue whose hwnd value is NULL,
        /// that is, thread messages as posted by PostMessage (when the hWnd parameter is NULL) or PostThreadMessage.
        /// </param>
        /// <param name="wMsgFilterMin">
        /// The integer value of the lowest message value to be retrieved. Use WM_KEYFIRST (0x0100) to specify the
        /// first keyboard message or WM_MOUSEFIRST (0x0200) to specify the first mouse message.
        /// Use WM_INPUT here and in wMsgFilterMax to specify only the WM_INPUT messages.
        /// If wMsgFilterMin and wMsgFilterMax are both zero, GetMessage returns all
        /// available messages(that is, no range filtering is performed).
        /// </param>
        /// <param name="wMsgFilterMax">
        /// The integer value of the highest message value to be retrieved. Use WM_KEYLAST to specify the
        /// last keyboard message or WM_MOUSELAST to specify the last mouse message.
        /// Use WM_INPUT here and in wMsgFilterMin to specify only the WM_INPUT messages.
        /// If wMsgFilterMin and wMsgFilterMax are both zero, GetMessage returns all
        /// available messages(that is, no range filtering is performed).</param>
        /// <returns>
        /// If the function retrieves a message other than WM_QUIT, the return value is nonzero.
        /// If the function retrieves the WM_QUIT message, the return value is zero.
        /// If there is an error, the return value is -1. For example, the function
        /// fails if hWnd is an invalid window handle or lpMsg is an invalid pointer.
        /// To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getmessage
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern int GetMessage(
            out WinUserMessage lpMsg,
            IntPtr hWnd,
            uint wMsgFilterMin,
            uint wMsgFilterMax
        );

        /// <summary>
        /// Performs a buffered read of the raw input data.
        /// </summary>
        /// <param name="pData">A pointer to a buffer of RAWINPUT structures that contain the raw
        /// input data. If NULL, the minimum required buffer, in bytes, is returned in *pcbSize.</param>
        /// <param name="size">The size, in bytes, of a RAWINPUT structure.</param>
        /// <param name="sizeHeader">The size, in bytes, of the RAWINPUTHEADER structure.</param>
        /// <returns>If pData is NULL and the function is successful, the return value is zero.
        /// If pData is not NULL and the function is successful, the return value is the number of
        /// RAWINPUT structures written to pData.
        /// If an error occurs, the return value is (UINT)-1. Call GetLastError for the error code.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputbuffer
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern uint GetRawInputBuffer(
            [Out] IntPtr pData,
            ref uint size,
            int sizeHeader
        );

        /// <summary>
        /// Retrieves the raw input from the specified device.
        /// </summary>
        /// <param name="hRawInput">A handle to the RAWINPUT structure. This comes from the lParam in WM_INPUT.</param>
        /// <param name="command">The command flag. This parameter can be RID_HEADER, RID_INPUT.</param>
        /// <param name="pData">A pointer to the data that comes from the RAWINPUT structure. This depends on
        /// the value of uiCommand. If pData is NULL, the required size of the buffer is returned in *pcbSize.</param>
        /// <param name="size">The size, in bytes, of the data in pData.</param>
        /// <param name="sizeHeader">The size, in bytes, of the RAWINPUTHEADER structure.</param>
        /// <returns>If pData is NULL and the function is successful, the return value is 0. If pData is not
        /// NULL and the function is successful, the return value is the number of bytes copied into pData.
        /// If there is an error, the return value is (UINT)-1.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdata
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern int GetRawInputData(
            IntPtr hRawInput,
            RawInputCommand command,
            [Out] IntPtr pData,
            ref uint size,
            int sizeHeader
        );

        /// <summary>
        /// Retrieves information about the raw input device.
        /// </summary>
        /// <param name="deviceHandle">A handle to the raw input device. This comes from the hDevice member of
        /// RAWINPUTHEADER or from GetRawInputDeviceList.</param>
        /// <param name="command">Specifies what data will be returned in pData: RIDI_DEVICENAME,
        /// RIDI_DEVICEINFO, RIDI_PREPARSEDDATA.</param>
        /// <param name="data">
        /// A pointer to a buffer that contains the information specified by uiCommand. If uiCommand is
        /// RIDI_DEVICEINFO, set the cbSize member of RID_DEVICE_INFO to sizeof(RID_DEVICE_INFO) before
        /// calling GetRawInputDeviceInfo.
        /// </param>
        /// <param name="dataSize">The size, in bytes, of the data in pData.</param>
        /// <returns>
        /// If successful, this function returns a non-negative number indicating the number of bytes
        /// copied to pData.
        /// If pData is not large enough for the data, the function returns -1. If pData is NULL, the
        /// function returns a value of zero.In both of these cases, pcbSize is set to the minimum size
        /// required for the pData buffer.
        /// Call GetLastError to identify any other errors.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdeviceinfoa
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern uint GetRawInputDeviceInfo(
            IntPtr deviceHandle,
            GetRawInputDeviceInfoCommand command,
            ref RidDeviceInfo data,
            ref uint dataSize
        );

        /// <summary>
        /// Retrieves information about the raw input device.
        /// </summary>
        /// <param name="deviceHandle">A handle to the raw input device. This comes from the hDevice member of
        /// RAWINPUTHEADER or from GetRawInputDeviceList.</param>
        /// <param name="command">Specifies what data will be returned in pData: RIDI_DEVICENAME,
        /// RIDI_DEVICEINFO, RIDI_PREPARSEDDATA.</param>
        /// <param name="pdata">
        /// A pointer to a buffer that contains the information specified by uiCommand. If uiCommand is
        /// RIDI_DEVICEINFO, set the cbSize member of RID_DEVICE_INFO to sizeof(RID_DEVICE_INFO) before
        /// calling GetRawInputDeviceInfo.
        /// </param>
        /// <param name="dataSize">The size, in bytes, of the data in pData.</param>
        /// <returns>
        /// If successful, this function returns a non-negative number indicating the number of bytes
        /// copied to pData.
        /// If pData is not large enough for the data, the function returns -1. If pData is NULL, the
        /// function returns a value of zero.In both of these cases, pcbSize is set to the minimum size
        /// required for the pData buffer.
        /// Call GetLastError to identify any other errors.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdeviceinfoa
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern int GetRawInputDeviceInfo(
            IntPtr deviceHandle,
            GetRawInputDeviceInfoCommand command,
            IntPtr pdata,
            ref uint dataSize
        );

        /// <summary>
        /// Enumerates the raw input devices attached to the system.
        /// </summary>
        /// <param name="RawInputDeviceList">
        /// An array of RAWINPUTDEVICELIST structures for the devices attached to the system.
        /// If NULL, the number of devices are returned in *puiNumDevices.
        /// </param>
        /// <param name="NumDevices">
        /// If pRawInputDeviceList is NULL, the function populates this variable with the number
        /// of devices attached to the system; otherwise, this variable specifies the number of
        /// RAWINPUTDEVICELIST structures that can be contained in the buffer to which 
        /// pRawInputDeviceList points. If this value is less than the number of devices attached
        /// to the system, the function returns the actual number of devices in this variable and
        /// fails with ERROR_INSUFFICIENT_BUFFER.
        /// </param>
        /// <param name="cbSize">The size of a RAWINPUTDEVICELIST structure, in bytes.</param>
        /// <returns>
        /// If the function is successful, the return value is the number of devices stored
        /// in the buffer pointed to by pRawInputDeviceList.
        /// On any other error, the function returns(UINT) -1 and GetLastError returns the
        /// error indication.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getrawinputdevicelist
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetRawInputDeviceList
        (
            [In, Out] RawInputDeviceList[] RawInputDeviceList,
            ref uint NumDevices,
            uint cbSize /* = (uint)Marshal.SizeOf(typeof(RawInputDeviceList)) */
        );

        /// <summary>
        /// Copies the text of the specified window's title bar (if it has one)
        /// into a buffer. If the specified window is a control, the text of the
        /// control is copied. However, GetWindowText cannot retrieve the text
        /// of a control in another application.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window or control containing the text.
        /// </param>
        /// <param name="text">
        /// The buffer that will receive the text. If the string is as long or
        /// longer than the buffer, the string is truncated and terminated with a
        /// null character.
        /// </param>
        /// <param name="count">
        /// The maximum number of characters to copy to the buffer, including the
        /// null character. If the text exceeds this limit, it is truncated.
        /// </param>
        /// <returns>
        /// <para>
        /// If the function succeeds, the return value is the length, in characters, of the
        /// copied string, not including the terminating null character. If the window has
        /// no title bar or text, if the title bar is empty, or if the window or control handle
        /// is invalid, the return value is zero. To get extended error information,
        /// call GetLastError.
        /// </para>
        /// <para>This function cannot retrieve the text of an edit control in another application.
        /// </para></returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowtextw
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder text,
            int count
        );

        /// <summary>
        /// Destroys the specified timer.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window associated with the specified timer. This value must be
        /// the same as the hWnd value passed to the SetTimer function that created the timer.
        /// </param>
        /// <param name="uIDEvent">
        /// The timer to be destroyed. If the window handle passed to SetTimer is valid,
        /// this parameter must be the same as the nIDEvent
        /// value passed to SetTimer.If the application calls SetTimer with hWnd set to
        /// NULL, this parameter must be the timer identifier returned by SetTimer.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-killtimer
        /// </remarks>
        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(
            IntPtr hWnd,
            IntPtr uIDEvent
        );

        /// <summary>
        /// Registers the devices that supply the raw input data.
        /// </summary>
        /// <param name="pRawInputDevice">An array of RAWINPUTDEVICE structures that represent the devices that supply the raw input.</param>
        /// <param name="uiNumDevices">The number of RAWINPUTDEVICE structures pointed to by pRawInputDevices.</param>
        /// <param name="cbSize">The size, in bytes, of a RAWINPUTDEVICE structure.</param>
        /// <returns>TRUE if the function succeeds; otherwise, FALSE. If the function fails, call GetLastError for more information.</returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-registerrawinputdevices
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern bool RegisterRawInputDevices(
            RawInputDevice[] pRawInputDevice,
            uint uiNumDevices,
            uint cbSize
        );

        /// <summary>
        /// Creates a timer with the specified time-out value.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window to be associated with the timer. This window must be
        /// owned by the calling thread. If a NULL value for hWnd is passed in along with
        /// an nIDEvent of an existing timer, that timer will be replaced in the same way
        /// that an existing non-NULL hWnd timer will be.
        /// </param>
        /// <param name="nIDEvent">
        /// A nonzero timer identifier. If the hWnd parameter is NULL, and the nIDEvent does
        /// not match an existing timer then it is ignored and a new timer ID is generated.
        /// If the hWnd parameter is not NULL and the window specified by hWnd already has a
        /// timer with the value nIDEvent, then the existing timer is replaced by the new
        /// timer. When SetTimer replaces a timer, the timer is reset. Therefore, a message
        /// will be sent after the current time-out value elapses, but the previously set time-out
        /// value is ignored. If the call is not intended to replace an existing timer, nIDEvent
        /// should be 0 if the hWnd is NULL.
        /// </param>
        /// <param name="uElapse">
        /// <para>
        /// The time-out value, in milliseconds.
        /// </para>
        /// <para>If uElapse is less than USER_TIMER_MINIMUM(0x0000000A), the timeout is set to
        /// USER_TIMER_MINIMUM.If uElapse is greater than USER_TIMER_MAXIMUM(0x7FFFFFFF),
        /// the timeout is set to USER_TIMER_MAXIMUM.
        /// </para></param>
        /// <param name="lpTimerFunc">
        /// A pointer to the function to be notified when the time-out value elapses. For more
        /// information about the function, see TimerProc. If lpTimerFunc is NULL, the system
        /// posts a WM_TIMER message to the application queue. The hwnd member of the message's
        /// MSG structure contains the value of the hWnd parameter.
        /// </param>
        /// <returns><para>If the function succeeds and the hWnd parameter is NULL, the return
        /// value is an integer identifying the new timer. An application can pass this value to
        /// the KillTimer function to destroy the timer.
        /// </para>
        /// <para>If the function succeeds and
        /// the hWnd parameter is not NULL, then the return value is a nonzero integer. An
        /// application can pass the value of the nIDEvent parameter to the KillTimer function
        /// to destroy the timer.
        /// </para>
        /// <para>If the function fails to create a timer, the
        /// return value is zero. To get extended error information, call GetLastError.
        /// </para></returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-settimer
        /// </remarks>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(
            IntPtr hWnd,
            IntPtr nIDEvent,
            uint uElapse,
            IntPtr lpTimerFunc
        );

        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain. You would install a
        /// hook procedure to monitor the system for certain types of events. These events are
        /// associated either with a specific thread or with all threads in the same desktop as
        /// the calling thread.
        /// </summary>
        /// <param name="hookType">The type of hook procedure to be installed.</param>
        /// <param name="lpfn">
        /// A pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the
        /// identifier of a thread created by a different process, the lpfn parameter must point
        /// to a hook procedure in a DLL. Otherwise, lpfn can point to a hook procedure in the
        /// code associated with the current process.</param>
        /// <param name="hMod">
        /// A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. The
        /// hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread
        /// created by the current process and if the hook procedure is within the code associated
        /// with the current process.</param>
        /// <param name="dwThreadId">
        /// The identifier of the thread with which the hook procedure is to be associated. For
        /// desktop apps, if this parameter is zero, the hook procedure is associated with all
        /// existing threads running in the same desktop as the calling thread. For Windows Store
        /// apps, see the Remarks section.
        /// </param>
        /// <returns>
        /// <para>If the function succeeds, the return value is the handle to the hook procedure.
        /// </para>
        /// <para>If the function fails, the return value is NULL.To get extended error
        /// information, call GetLastError.
        /// </para></returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowshookexa
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(SetWindowsHookExType hookType, Delegate lpfn, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// Translates virtual-key messages into character messages. The character messages are posted to the calling thread's message
        /// queue, to be read the next time the thread calls the GetMessage or PeekMessage function.
        /// </summary>
        /// <param name="lpMsg">
        /// A pointer to an MSG structure that contains message information retrieved from the calling thread's
        /// message queue by using the GetMessage or PeekMessage function.
        /// </param>
        /// <returns>
        /// If the message is translated (that is, a character message is posted to the thread's message
        /// queue), the return value is nonzero.
        /// If the message is WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or WM_SYSKEYUP, the return value is
        /// nonzero, regardless of the translation.
        /// If the message is not translated (that is, a character message is not posted to the
        /// thread's message queue), the return value is zero.
        /// </returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-translatemessage
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern bool TranslateMessage(
            [In] ref WinUserMessage lpMsg
        );

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">
        /// A handle to the hook to be removed. This parameter is a hook handle
        /// obtained by a previous call to SetWindowsHookEx.
        /// </param>
        /// <returns>
        /// <para>
        /// If the function succeeds, the return value is nonzero.
        /// </para>
        /// <para>If the function fails, the return value is zero. To get extended
        /// error information, call GetLastError.
        /// </para></returns>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-unhookwindowshookex
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(
            IntPtr hhk
        );
    }
}
