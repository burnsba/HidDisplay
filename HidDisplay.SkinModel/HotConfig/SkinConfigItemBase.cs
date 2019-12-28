using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplay.SkinModel.HotConfig
{
    /// <summary>
    /// Better typed skin config item.
    /// </summary>
    public abstract class SkinConfigItemBase : ISkinConfigItem
    {
        protected SettingsItem _settingsItem = null;

        /// <inheritdoc />
        public string Key
        {
            get
            {
                return _settingsItem.Key;
            }

            set
            {
                _settingsItem.Key = value;
            }
        }

        /// <inheritdoc />
        public string Display
        {
            get
            {
                return _settingsItem.Display;
            }

            set
            {
                _settingsItem.Display = value;
            }
        }

        /// <inheritdoc />
        public InputTypes InputType
        {
            get
            {
                return InputTypesConverter.StringToInputTypes(_settingsItem.Input);
            }

            set
            {
                _settingsItem.Input = InputTypesConverter.InputTypeToString(value);
            }
        }

        /// <inheritdoc />
        public string CurrentValue
        {
            get
            {
                return _settingsItem.CurrentValue;
            }

            set
            {
                _settingsItem.CurrentValue = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinConfigItemBase"/> class.
        /// </summary>
        /// <param name="item">Item source.</param>
        public SkinConfigItemBase(SettingsItem item)
        {
            _settingsItem = item.Clone();
        }

        /// <summary>
        /// Converts back to a json setttings item.
        /// </summary>
        /// <returns>Underlying setting object.</returns>
        public virtual SettingsItem ToSettingsItem()
        {
            return _settingsItem;
        }
    }
}
