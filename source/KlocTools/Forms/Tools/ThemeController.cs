using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Klocman.Forms.Tools
{
    public class ThemeController
    {
        public static Theme CurrentTheme { get; set; } = Theme.System;

        public enum Theme
        {
            Light,
            Dark,
            System
        }

        public static Color DarkenColor(Color color, float ratio)
        {
            var r = (int)(color.R * ratio);
            var g = (int)(color.G * ratio);
            var b = (int)(color.B * ratio);
            return Color.FromArgb(color.A, r, g, b);
        }

        private static readonly Color LightBackground = SystemColors.Control;
        private static readonly Color LightForeground = SystemColors.ControlText;
        private static readonly Color DarkBackground = Color.FromArgb(30, 30, 30);
        private static readonly Color DarkForeground = Color.FromArgb(212, 212, 212);
        private static readonly Color DarkControlBackground = Color.FromArgb(45, 45, 48);
        private static readonly Color DarkControlForeground = Color.White;

        private readonly Form _referenceForm;
        private Theme _currentTheme;

        public event EventHandler ThemeChanged;
        public bool IsDark { get; private set; }

        public ThemeController(Form referenceForm)
        {
            _referenceForm = referenceForm;
            _currentTheme = Theme.System;
            
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General && _currentTheme == Theme.System)
            {
                ApplyTheme(_currentTheme);
            }
        }

        public void ApplyTheme(Theme theme)
        {
            _currentTheme = theme;
            bool isDark = false;

            if (theme == Theme.System)
            {
                isDark = IsSystemInDarkMode();
            }
            else
            {
                isDark = theme == Theme.Dark;
            }
            IsDark = isDark;

            ApplyThemeToControl(_referenceForm, isDark);
            
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ApplyThemeToControl(Control control, bool isDark)
        {
            if (control == null) return;

            // Apply colors based on control type
            if (isDark)
            {
                control.BackColor = DarkBackground;
                control.ForeColor = DarkForeground;

                if (control is TextBox || control is ListBox || control is ComboBox || control is TreeView)
                {
                    control.BackColor = DarkControlBackground;
                    control.ForeColor = DarkControlForeground;
                }
                // Add more specific control handling here
            }
            else
            {
                control.BackColor = LightBackground;
                control.ForeColor = LightForeground;
                
                 if (control is TextBox || control is ListBox || control is ComboBox || control is TreeView)
                {
                     control.BackColor = SystemColors.Window;
                     control.ForeColor = SystemColors.WindowText;
                }
            }
            
            // Recursively apply to children
            foreach (Control child in control.Controls)
            {
                ApplyThemeToControl(child, isDark);
            }
        }

        public static bool IsSystemInDarkMode()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("AppsUseLightTheme");
                        if (value != null && value is int intValue)
                        {
                            return intValue == 0;
                        }
                    }
                }
            }
            catch
            {
                // Fallback to light mode if check fails
            }
            return false;
        }
    }
}
