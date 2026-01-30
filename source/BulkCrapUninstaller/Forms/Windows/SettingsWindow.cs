/*
    Copyright (c) 2017 Marcin Szeniak (https://github.com/Klocman/)
    Apache License Version 2.0
*/

using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using BulkCrapUninstaller.Functions;
using BulkCrapUninstaller.Properties;
using Klocman;
using Klocman.Binding.Settings;
using Klocman.Forms.Tools;
using Klocman.Localising;

namespace BulkCrapUninstaller.Forms
{
    public partial class SettingsWindow : AntdUI.Window
    {
        private readonly SettingBinder<Settings> _settings = Settings.Default.SettingBinder;
        private bool _restartNeeded;

        public int OpenedTab { get { return tabControl.SelectedIndex; } set { tabControl.SelectedIndex = value; } }

        public SettingsWindow()
        {
            InitializeComponent();
            AntdUI.Config.Font = new Font("Segoe UI", 9F);
        }

        // private ThemeController _themeController; // Removed

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode) return;
            
            // AntdUI handles theming automatically
            /*
            _themeController = new ThemeController(this);
            _settings.Subscribe((x, y) =>
            {
                if (Enum.TryParse<ThemeController.Theme>(y.NewValue, true, out var theme))
                    _themeController.ApplyTheme(theme);
            }, x => x.MiscTheme, this);

            // Apply initial theme
            if (Enum.TryParse<ThemeController.Theme>(_settings.Settings.MiscTheme, true, out var initialTheme))
                _themeController.ApplyTheme(initialTheme);
            */

            Icon = Resources.Icon_Logo;

            BindAntdCheck(checkBoxLoud, x => x.MessagesAskRemoveLoudItems);
            BindAntdCheck(checkBoxShowAllBadJunk, x => x.MessagesShowAllBadJunk);
            BindAntdCheck(checkBoxNeverFeedback, x => x.MiscFeedbackNagNeverShow);
            BindAntdCheck(checkBoxUpdateSearch, x => x.MiscCheckForUpdates);
            BindAntdCheck(checkBoxSendStats, x => x.MiscSendStatistics);
            BindAntdCheck(checkBoxAutoLoad, x => x.MiscAutoLoadDefaultList);
            BindAntdCheck(checkBoxRatings, x => x.MiscUserRatings);
            BindAntdCheck(checkBoxColorblind, x => x.MiscColorblind);
            BindAntdCheck(checkBoxDpiaware, x => x.WindowDpiAware);

            BindAntdCheck(checkBoxEnableExternal, x => x.ExternalEnable);
            BindAntdInput(textBoxPreUninstall, x => x.ExternalPreCommands);
            BindAntdInput(textBoxPostUninstall, x => x.ExternalPostCommands);

            BindAntdInput(textBoxProgramFolders, x => x.FoldersCustomProgramDirs);
            BindAntdCheck(checkBoxAutoInstallFolderDetect, x => x.FoldersAutoDetect);
            BindAntdCheck(checkBoxRemovable, x => x.FoldersScanRemovable);
            _settings.Subscribe((x, y) => checkBoxRemovable.Enabled = y.NewValue, x => x.FoldersAutoDetect, this);

            BindAntdCheck(checkBoxChoco, x => x.ScanChocolatey);
            BindAntdCheck(checkBoxScoop, x => x.ScanScoop);
            BindAntdCheck(checkBoxScanSteam, x => x.ScanSteam);
            BindAntdCheck(checkBoxScanStoreApps, x => x.ScanStoreApps);
            BindAntdCheck(checkBoxOculus, x => x.ScanOculus);
            BindAntdCheck(checkBoxScanWinFeatures, x => x.ScanWinFeatures);
            BindAntdCheck(checkBoxScanWinUpdates, x => x.ScanWinUpdates);

            BindAntdCheck(checkBoxScanDrives, x => x.ScanDrives);
            BindAntdCheck(checkBoxScanRegistry, x => x.ScanRegistry);
            BindAntdCheck(checkBoxPreDefined, x => x.ScanPreDefined);

            foreach (YesNoAsk value in Enum.GetValues(typeof(YesNoAsk)))
            {
                var wrapper = new LocalisedEnumWrapper(value);
                comboBoxJunk.Items.Add(wrapper);
                comboBoxRestore.Items.Add(wrapper);
            }
            _settings.Subscribe(JunkSettingChanged, x => x.MessagesRemoveJunk, this);
            _settings.Subscribe(RestoreSettingChanged, x => x.MessagesRestorePoints, this);

            foreach (UninstallerListDoubleClickAction value in Enum.GetValues(typeof(UninstallerListDoubleClickAction)))
            {
                var wrapper = new LocalisedEnumWrapper(value);
                comboBoxDoubleClick.Items.Add(wrapper);
            }
            _settings.Subscribe(DoubleClickSettingChanged, x => x.UninstallerListDoubleClickAction, this);

            comboBoxLanguage.Items.Add(Localisable.DefaultLanguage);
            foreach (var languageCode in CultureConfigurator.SupportedLanguages.OrderBy(x => x.DisplayName))
            {
                comboBoxLanguage.Items.Add(new ComboBoxWrapper<CultureInfo>(languageCode, x => x.DisplayName));
            }
            _settings.Subscribe(LanguageSettingChanged, x => x.Language, this);

            _settings.Subscribe(BackupSettingChanged, x => x.BackupLeftovers, this);
            _settings.BindProperty(directorySelectBoxBackup,
                box => box.DirectoryPath, nameof(directorySelectBoxBackup.DirectoryPathChanged),
                settings => settings.BackupLeftoversDirectory, this);

            _settings.SendUpdates(this);

            _restartNeeded = false;
        }

        private void BindAntdCheck(AntdUI.Checkbox box, Expression<Func<Settings, bool>> settingSelector)
        {
            var compiled = settingSelector.Compile();
            box.Checked = compiled(_settings.Settings);

            var memberExpr = (MemberExpression)settingSelector.Body;
            var property = (System.Reflection.PropertyInfo)memberExpr.Member;

            box.CheckedChanged += (s, e) => 
            {
                property.SetValue(_settings.Settings, box.Checked);
            };

            _settings.Subscribe((sender, args) => 
            {
                if (box.Checked != args.NewValue)
                    box.Checked = args.NewValue;
            }, settingSelector, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();

            if (_restartNeeded && MessageBoxes.RestartNeededForSettingChangeQuestion())
            {
                EntryPoint.Restart();
            }
        }

        private void checkBoxEnableExternal_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            splitContainer1.Enabled = checkBoxEnableExternal.Checked;
            //textBoxPreUninstall.Enabled = checkBoxEnableExternal.Checked;
            //textBoxPostUninstall.Enabled = checkBoxEnableExternal.Checked;
        }

        private void comboBoxJunk_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (comboBoxJunk.SelectedValue is LocalisedEnumWrapper wrapper)
            {
                _settings.Settings.MessagesRemoveJunk = (YesNoAsk)wrapper.TargetEnum;
            }
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (comboBoxLanguage.SelectedValue is ComboBoxWrapper<CultureInfo> wrapper)
            {
                _settings.Settings.Language = wrapper.WrappedObject.Name;
                _restartNeeded = true;
            }
            else if (comboBoxLanguage.SelectedValue is string)
            {
                _settings.Settings.Language = string.Empty;
                _restartNeeded = true;
            }
        }

        private void comboBoxRestore_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (comboBoxRestore.SelectedValue is LocalisedEnumWrapper wrapper)
            {
                _settings.Settings.MessagesRestorePoints = (YesNoAsk)wrapper.TargetEnum;
            }
        }

        private void JunkSettingChanged(object sender, SettingChangedEventArgs<YesNoAsk> args)
        {
            var newSelection =
                comboBoxJunk.Items.Cast<LocalisedEnumWrapper>().FirstOrDefault(x => x.TargetEnum.Equals(args.NewValue));
            if (newSelection == null || newSelection.Equals(comboBoxJunk.SelectedValue))
                return;

            comboBoxJunk.SelectedValue = newSelection;
        }

        private void LanguageSettingChanged(object sender, SettingChangedEventArgs<string> args)
        {
            if (!string.IsNullOrEmpty(args.NewValue))
            {
                var selectedItem = comboBoxLanguage.Items.OfType<ComboBoxWrapper<CultureInfo>>()
                    .FirstOrDefault(x => x.WrappedObject.Name.Equals(args.NewValue));
                if (selectedItem != null)
                {
                    comboBoxLanguage.SelectedValue = selectedItem;
                    return;
                }
            }
            comboBoxLanguage.SelectedIndex = 0;
        }

        private void RestoreSettingChanged(object sender, SettingChangedEventArgs<YesNoAsk> args)
        {
            var newSelection =
                comboBoxRestore.Items.Cast<LocalisedEnumWrapper>()
                    .FirstOrDefault(x => x.TargetEnum.Equals(args.NewValue));
            if (newSelection == null || newSelection.Equals(comboBoxRestore.SelectedValue))
                return;

            comboBoxRestore.SelectedValue = newSelection;
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _settings.RemoveHandlers(this);
        }

        private void tabControl_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
        }

        private void radioButtonBackup_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            directorySelectBoxBackup.Enabled = false;

            if (radioButtonBackupAsk.Checked)
                _settings.Settings.BackupLeftovers = YesNoAsk.Ask;
            else if (radioButtonBackupAuto.Checked)
            {
                _settings.Settings.BackupLeftovers = YesNoAsk.Yes;
                directorySelectBoxBackup.Enabled = true;
            }
            else if (radioButtonBackupNever.Checked)
                _settings.Settings.BackupLeftovers = YesNoAsk.No;
            else
                throw new InvalidOperationException();
        }

        private void BackupSettingChanged(object sender, SettingChangedEventArgs<YesNoAsk> args)
        {
            switch (args.NewValue)
            {
                case YesNoAsk.Ask:
                    radioButtonBackupAsk.Checked = true;
                    break;
                case YesNoAsk.Yes:
                    radioButtonBackupAuto.Checked = true;
                    break;
                case YesNoAsk.No:
                    radioButtonBackupNever.Checked = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(args), args.NewValue, "Unknown value");
            }
        }

        private void comboBoxDoubleClick_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (comboBoxDoubleClick.SelectedValue is LocalisedEnumWrapper wrapper)
            {
                _settings.Settings.UninstallerListDoubleClickAction = (UninstallerListDoubleClickAction)wrapper.TargetEnum;
            }
        }

        private void DoubleClickSettingChanged(object sender, SettingChangedEventArgs<UninstallerListDoubleClickAction> args)
        {
            var newSelection = comboBoxDoubleClick.Items.Cast<LocalisedEnumWrapper>().FirstOrDefault(x => x.TargetEnum.Equals(args.NewValue));
            if (newSelection == null || newSelection.Equals(comboBoxDoubleClick.SelectedValue))
                return;

            comboBoxDoubleClick.SelectedValue = newSelection;
        }
        private void BindAntdInput(AntdUI.Input control, System.Linq.Expressions.Expression<Func<Settings, string>> settingSelector)
        {
            var compiled = settingSelector.Compile();
            control.Text = compiled(_settings.Settings);

            var memberExpr = (MemberExpression)settingSelector.Body;
            var property = (System.Reflection.PropertyInfo)memberExpr.Member;

            control.TextChanged += (s, e) => 
            {
                property.SetValue(_settings.Settings, control.Text);
            };

            _settings.Subscribe((x, y) => 
            {
                if (control.Text != y.NewValue) control.Text = y.NewValue;
            }, settingSelector, this);
        }

        private void BindAntdInput(AntdUI.Input control, System.Linq.Expressions.Expression<Func<Settings, decimal>> settingSelector)
        {
             var compiled = settingSelector.Compile();
             control.Text = compiled(_settings.Settings).ToString();

             var memberExpr = (MemberExpression)settingSelector.Body;
             var property = (System.Reflection.PropertyInfo)memberExpr.Member;

             control.TextChanged += (s, e) => {
                 if(decimal.TryParse(control.Text, out var val)) 
                    property.SetValue(_settings.Settings, val);
             };

             _settings.Subscribe((x, y) => 
             {
                 var newVal = y.NewValue.ToString();
                 if (control.Text != newVal) control.Text = newVal;
             }, settingSelector, this);
        }
    }
}