/*
    Copyright (c) 2017 Marcin Szeniak (https://github.com/Klocman/)
    Apache License Version 2.0
*/

using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Windows.Forms;
using Klocman.Binding.Settings;

namespace BulkCrapUninstaller.Controls
{
    public partial class PropertiesSidebar : UserControl
    {
        private readonly SettingBinder<Properties.Settings> _settings = Properties.Settings.Default.SettingBinder;

        public PropertiesSidebar()
        {
            InitializeComponent();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode) return;

            BindAntdCheck(checkBoxViewCheckboxes, x => x.UninstallerListUseCheckboxes);
            BindAntdCheck(checkBoxViewGroups, x => x.UninstallerListUseGroups);

            BindAntdCheck(checkBoxListHideMicrosoft, x => x.FilterHideMicrosoft);
            BindAntdCheck(checkBoxShowUpdates, x => x.FilterShowUpdates);
            BindAntdCheck(checkBoxListSysComp, x => x.FilterShowSystemComponents);
            BindAntdCheck(checkBoxListProtected, x => x.FilterShowProtected);
            BindAntdCheck(checkBoxShowStoreApps, x => x.FilterShowStoreApps);
            BindAntdCheck(checkBoxWinFeature, x => x.FilterShowWinFeatures);
            BindAntdCheck(checkBoxTweaks, x => x.FilterShowTweaks);

            BindAntdCheck(checkBoxInvalidTest, x => x.AdvancedTestInvalid);
            BindAntdCheck(checkBoxCertTest, x => x.AdvancedTestCertificates);
            BindAntdCheck(checkBoxOrphans, x => x.AdvancedDisplayOrphans);
            BindAntdCheck(checkBoxHighlightSpecial, x => x.AdvancedHighlightSpecial);

            _settings.SendUpdates(this);
            Disposed += (x, y) => _settings.RemoveHandlers(this);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public int GetSuggestedWidth()
        {
            var maxWidth = typeof(PropertiesSidebar)
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(x => x.FieldType == typeof(AntdUI.Checkbox))
                .Select(x => x.GetValue(this))
                .Cast<AntdUI.Checkbox>()
                .Max(c => c.Width);

            return maxWidth + (groupBox1.Width - groupBox1.DisplayRectangle.Width) + Padding.Left + Padding.Right;
        }

        private void BindAntdCheck(AntdUI.Checkbox box, System.Linq.Expressions.Expression<System.Func<Properties.Settings, bool>> settingSelector)
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool SysCompEnabled
        {
            get { return checkBoxListSysComp.Enabled; }
            set { checkBoxListSysComp.Enabled = value; checkBoxListSysComp.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ProtectedEnabled
        {
            get { return checkBoxListProtected.Enabled; }
            set { checkBoxListProtected.Enabled = value; checkBoxListProtected.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool UpdatesEnabled
        {
            get { return checkBoxShowUpdates.Enabled; }
            set { checkBoxShowUpdates.Enabled = value; checkBoxShowUpdates.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool OrphansEnabled
        {
            get { return checkBoxOrphans.Enabled; }
            set { checkBoxOrphans.Enabled = value; checkBoxOrphans.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool StoreAppsEnabled
        {
            get { return checkBoxShowStoreApps.Enabled; }
            set { checkBoxShowStoreApps.Enabled = value; checkBoxShowStoreApps.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool InvalidEnabled
        {
            get { return checkBoxInvalidTest.Enabled; }
            set { checkBoxInvalidTest.Enabled = value; checkBoxInvalidTest.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool WinFeaturesEnabled
        {
            get { return checkBoxWinFeature.Enabled; }
            set { checkBoxWinFeature.Enabled = value; checkBoxWinFeature.Visible = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShowTweaksEnabled
        {
            get { return checkBoxTweaks.Enabled; }
            set { checkBoxTweaks.Enabled = value; checkBoxTweaks.Visible = value; }
        }
    }
}