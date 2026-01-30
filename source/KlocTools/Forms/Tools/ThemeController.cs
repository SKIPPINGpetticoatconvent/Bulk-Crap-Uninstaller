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

        public static class Palette
        {
            public static readonly Color LightBackground = SystemColors.Control;
            public static readonly Color LightForeground = SystemColors.ControlText;

            // Updated to match AntdUI/VS Code style (Flatter, #1F1F1F)
            public static readonly Color DarkBackground = Color.FromArgb(31, 31, 31); 
            public static readonly Color DarkForeground = Color.FromArgb(220, 220, 220);
            
            // Panels/Containers
            public static readonly Color DarkPanelBackground = Color.FromArgb(45, 45, 48); // Slightly lighter for contrast or Sidebar

            // Controls (Input, List)
            public static readonly Color DarkControlBackground = Color.FromArgb(40, 40, 40);
            public static readonly Color DarkControlForeground = Color.FromArgb(240, 240, 240);
            
            // Accents
            public static readonly Color DarkAccent = Color.FromArgb(24, 144, 255); // Ant Design Blue
            public static readonly Color DarkBorder = Color.FromArgb(60, 60, 60);
        }

        private static readonly Color LightBackground = Palette.LightBackground;
        private static readonly Color LightForeground = Palette.LightForeground;
        private static readonly Color DarkBackground = Palette.DarkBackground;
        private static readonly Color DarkForeground = Palette.DarkForeground;
        private static readonly Color DarkControlBackground = Palette.DarkControlBackground;
        private static readonly Color DarkControlForeground = Palette.DarkControlForeground;

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

                if (control is TextBox || control is ListBox || control is ComboBox || 
                    control is TreeView || control is ListView)
                {
                    control.BackColor = DarkControlBackground;
                    control.ForeColor = DarkControlForeground;

                    if (control is ListView || control is TreeView)
                    {
                         // Force re-apply theme to ensure headers catch it
                         NativeMethods.SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
                    }
                }
            }
            else
            {
                control.BackColor = LightBackground;
                control.ForeColor = LightForeground;
                
                 if (control is TextBox || control is ListBox || control is ComboBox || 
                     control is TreeView || control is ListView)
                {
                     control.BackColor = SystemColors.Window;
                     control.ForeColor = SystemColors.WindowText;
                     
                     if (control is ListView || control is TreeView)
                     {
                         NativeMethods.SetWindowTheme(control.Handle, "Explorer", null);
                     }
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

        private static class NativeMethods
        {
            [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
            public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        }
    }
}
