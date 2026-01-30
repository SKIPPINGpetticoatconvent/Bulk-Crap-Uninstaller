/*
    Copyright (c) 2017 Marcin Szeniak (https://github.com/Klocman/)
    Apache License Version 2.0
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq.Expressions;
using Klocman;
using Klocman.Binding.Settings;
using Klocman.IO;

namespace BulkCrapUninstaller.Controls
{
    public partial class UninstallationSettings : UserControl
    {
        public UninstallationSettings()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        
            if (DesignMode) return;

            var sb = Properties.Settings.Default.SettingBinder;
        
            // Shutdown blocking not available below Windows Vista
            if (Environment.OSVersion.Version < new Version(6, 0))
                checkBoxShutdown.Enabled = false;
            else
                BindAntdCheck(checkBoxShutdown, settings => settings.UninstallPreventShutdown);
        
            checkBoxRestorePoint.Enabled = SysRestore.SysRestoreAvailable();
            // Manual binding for complex RestorePoint logic
            sb.Bind(x => checkBoxRestorePoint.Checked = x != YesNoAsk.No, () => checkBoxRestorePoint.Checked &&  sb.Settings.MessagesRestorePoints != YesNoAsk.No ? sb.Settings.MessagesRestorePoints : checkBoxRestorePoint.Checked ? YesNoAsk.Ask : YesNoAsk.No,
                    eh => checkBoxRestorePoint.CheckedChanged += (s, e) => eh(s, e), eh => checkBoxRestorePoint.CheckedChanged -= (s, e) => eh(s, e),
                    settings => settings.MessagesRestorePoints, this);
        
            BindAntdCheck(checkBoxConcurrent, settings => settings.UninstallConcurrency);
        
            BindAntdCheck(checkBoxConcurrentOneLoud, settings => settings.UninstallConcurrentOneLoud);
            BindAntdCheck(checkBoxManualNoCollisionProtection, settings => settings.UninstallConcurrentDisableManualCollisionProtection);
        
            sb.Subscribe(OnMaxCountChanged, settings => settings.UninstallConcurrentMaxCount, this);
            numericUpDownMaxConcurrent.ValueChanged += NumericUpDownMaxConcurrentOnValueChanged;
        
            BindAntdCheck(checkBoxBatchSortQuiet, x => x.AdvancedIntelligentUninstallerSorting);
            BindAntdCheck(checkBoxDiisableProtection, x => x.AdvancedDisableProtection);
            BindAntdCheck(checkBoxSimulate, x => x.AdvancedSimulate);
        
            BindAntdCheck(checkBoxAutoKillQuiet, x => x.QuietAutoKillStuck);
            BindAntdCheck(checkBoxRetryQuiet, x => x.QuietRetryFailedOnce);
            BindAntdCheck(checkBoxGenerate, x => x.QuietAutomatization);
            BindAntdCheck(checkBoxGenerateStuck, x => x.QuietAutomatizationKillStuck);
            BindAntdCheck(checkBoxAutoDaemon, x => x.QuietUseDaemon);
        
            sb.Subscribe((sender, args) => checkBoxGenerateStuck.Enabled = args.NewValue, settings => settings.QuietAutomatization, this);
        
            sb.Subscribe(
                (x, y) => {
                    var isDark = AntdUI.Config.Mode == AntdUI.TMode.Dark;
                    var defaultColor = isDark ? Klocman.Forms.Tools.ThemeController.Palette.DarkForeground : SystemColors.ControlText;
                    checkBoxSimulate.ForeColor = y.NewValue ? Color.OrangeRed : defaultColor;
                },
                x => x.AdvancedSimulate, this);
        
            sb.SendUpdates(this);
            Disposed += (x, y) => sb.RemoveHandlers(this);
        }

        private void NumericUpDownMaxConcurrentOnValueChanged(object sender, AntdUI.DecimalEventArgs e)
        {
            Properties.Settings.Default.UninstallConcurrentMaxCount = (int)e.Value;
        }

        private void OnMaxCountChanged(object sender, SettingChangedEventArgs<int> args)
        {
            numericUpDownMaxConcurrent.Value = args.NewValue;
        }

        private void BindAntdCheck(AntdUI.Checkbox box, System.Linq.Expressions.Expression<Func<Properties.Settings, bool>> settingSelector)
        {
            var compiled = settingSelector.Compile();
            var sb = Properties.Settings.Default.SettingBinder;
            box.Checked = compiled(sb.Settings);

            var memberExpr = (MemberExpression)settingSelector.Body;
            var property = (System.Reflection.PropertyInfo)memberExpr.Member;

            box.CheckedChanged += (s, e) => 
            {
                property.SetValue(sb.Settings, box.Checked);
            };

            sb.Subscribe((sender, args) => 
            {
                if (box.Checked != args.NewValue)
                    box.Checked = args.NewValue;
            }, settingSelector, this);
        }

        private void BindAntdNumber(AntdUI.InputNumber box, System.Linq.Expressions.Expression<Func<Properties.Settings, int>> settingSelector)
        {
             var compiled = settingSelector.Compile();
             var sb = Properties.Settings.Default.SettingBinder;
             box.Value = compiled(sb.Settings);

             var memberExpr = (MemberExpression)settingSelector.Body;
             var property = (System.Reflection.PropertyInfo)memberExpr.Member;

             box.ValueChanged += (s, e) => {
                 property.SetValue(sb.Settings, (int)e.Value);
             };

             sb.Subscribe((x, y) => 
             {
                 if (box.Value != y.NewValue) box.Value = y.NewValue;
             }, settingSelector, this);
        }
    }
}
