using System;

namespace UninstallTools.Controls
{
    partial class FilterEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterEditor));
            this.labelText = new AntdUI.Label();
            this.textBoxFilterText = new System.Windows.Forms.TextBox();
            this.label2 = new AntdUI.Label();
            this.comboBoxCompareMethod = new AntdUI.Select();
            this.comboBox1 = new AntdUI.Select();
            this.label1 = new AntdUI.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxInvert = new AntdUI.Checkbox();
            this.searchBox1 = new Klocman.Controls.SearchBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelText
            // 
            this.labelText.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
            resources.ApplyResources(this.labelText, "labelText");
            this.labelText.Name = "labelText";
            // 
            // textBoxFilterText
            // 
            resources.ApplyResources(this.textBoxFilterText, "textBoxFilterText");
            this.textBoxFilterText.Name = "textBoxFilterText";
            this.textBoxFilterText.TextChanged += new System.EventHandler(this.textBoxFilterText_TextChanged);
            // 
            // label2
            // 
            this.label2.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboBoxCompareMethod
            // 
            resources.ApplyResources(this.comboBoxCompareMethod, "comboBoxCompareMethod");
            this.comboBoxCompareMethod.Name = "comboBoxCompareMethod";
            // this.comboBoxCompareMethod.Sorted = true;
            this.comboBoxCompareMethod.SelectedIndexChanged += new AntdUI.IntEventHandler(this.comboBoxCompareMethod_SelectedIndexChanged);
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new AntdUI.IntEventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.checkBoxInvert);
            this.panel1.Name = "panel1";
            // 
            // checkBoxInvert
            // 
            resources.ApplyResources(this.checkBoxInvert, "checkBoxInvert");
            this.checkBoxInvert.Name = "checkBoxInvert";
            // this.checkBoxInvert.UseVisualStyleBackColor = true;
            this.checkBoxInvert.CheckedChanged += new AntdUI.BoolEventHandler(this.checkBoxInvert_CheckedChanged);
            // 
            // searchBox1
            // 
            resources.ApplyResources(this.searchBox1, "searchBox1");
            this.searchBox1.BackColor = System.Drawing.SystemColors.Window;
            this.searchBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.searchBox1.InactiveSearchColor = System.Drawing.SystemColors.GrayText;
            this.searchBox1.Name = "searchBox1";
            this.searchBox1.NormalSearchColor = System.Drawing.SystemColors.WindowText;
            this.searchBox1.SearchBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBox1.SearchTextChanged += new System.EventHandler<Klocman.Controls.SearchBox.SearchEventArgs>(this.searchBox1_SearchTextChanged);
            // 
            // FilterEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxCompareMethod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxFilterText);
            this.Controls.Add(this.labelText);
            this.Controls.Add(this.searchBox1);
            this.Name = "FilterEditor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AntdUI.Label labelText;
        private System.Windows.Forms.TextBox textBoxFilterText;
        private AntdUI.Label label2;
        private AntdUI.Select comboBoxCompareMethod;
        private Klocman.Controls.SearchBox searchBox1;
        private AntdUI.Select comboBox1;
        private AntdUI.Label label1;
        private System.Windows.Forms.Panel panel1;
        private AntdUI.Checkbox checkBoxInvert;
    }
}
