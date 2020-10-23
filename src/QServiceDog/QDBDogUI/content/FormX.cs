using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using QCommon.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QDBDogUI
{
    public partial class FormX : FormMain3
    {
        public FormX()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(1024, 600);
            this.MaximumSize = new Size(1024, 600);
            this.Size = new Size(1024, 600);
            this.Icon = Properties.Resources.database;
            var x1 = new UI.UISystem() { BackColor = Color.Transparent, Dock = DockStyle.Fill }.LoadConfig();
            sideNavPanelX1.Controls.Add(x1);
            var x2 = new UI.UIDB() { BackColor = Color.Transparent, Dock = DockStyle.Fill }.LoadConfig();
            sideNavPanelX2.Controls.Add(x2);
            Shown += Form1_Shown;
        }

        protected SideNavItem AddSideNavItem()
        {
            SideNavItem item = new SideNavItem();
            SideNavPanel panel = new SideNavPanel();
            panel.Dock = DockStyle.Fill;
            item.Panel = panel;
            mySideNav1.Controls.Add(panel);
            mySideNav1.Items.Add(item);
            return item;
        }

        protected MySideNav GetNav()
        {
            return this.mySideNav1;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.mySideNav1.SetItemsTheme();
            this.sideNavItemExit.Click += (s, e2) =>
            {
                TaskDialogInfo info = new TaskDialogInfo("警告", eTaskDialogIcon.Stop, "是否退出？", "", eTaskDialogButton.Yes | eTaskDialogButton.No,
                        eTaskDialogBackgroundColor.Default, null, null, null, "", null);
                eTaskDialogResult result = TaskDialog.Show(info);
                if (result == eTaskDialogResult.Yes)
                    this.Close();
            };

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.Manual;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
        }

        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanelX2;
        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanelX3;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItem1;
        private Separator separatorX1;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItem2;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItem3;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItem4;
        


        #region 设计器代码

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormX));
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.mySideNav1 = new QCommon.UI.MySideNav();
            this.sideNavPanelX1 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.sideNavPanelX3 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.sideNavPanelX2 = new DevComponents.DotNetBar.Controls.SideNavPanel();
            this.sideNavItem1 = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.separatorX1 = new DevComponents.DotNetBar.Separator();
            this.sideNavItem2 = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.sideNavItem3 = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.sideNavItem4 = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.sideNavItemExit = new DevComponents.DotNetBar.Controls.SideNavItem();
            this.panelBottom.SuspendLayout();
            this.panelFill.SuspendLayout();
            this.mySideNav1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(843, 70);
            this.panelTop.Style.BackColor1.Alpha = ((byte)(220));
            this.panelTop.Style.BackColor1.Color = System.Drawing.Color.White;
            this.panelTop.Style.BackColor2.Alpha = ((byte)(100));
            this.panelTop.Style.BackColor2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(227)))), ((int)(((byte)(217)))));
            this.panelTop.Style.GradientAngle = 90;
            this.panelTop.TabIndex = 3;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelUser);
            this.panelBottom.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 544);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(843, 35);
            this.panelBottom.Style.BackColor1.Alpha = ((byte)(50));
            this.panelBottom.Style.BackColor1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(227)))), ((int)(((byte)(217)))));
            this.panelBottom.Style.BackColor2.Color = System.Drawing.Color.White;
            this.panelBottom.Style.BackgroundImageAlpha = ((byte)(200));
            this.panelBottom.Style.GradientAngle = 90;
            this.panelBottom.TabIndex = 4;
            // 
            // panelFill
            // 
            this.panelFill.Controls.Add(this.mySideNav1);
            this.panelFill.Size = new System.Drawing.Size(843, 474);
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(227)))), ((int)(((byte)(217))))));
            // 
            // mySideNav1
            // 
            this.mySideNav1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.mySideNav1.Controls.Add(this.sideNavPanelX1);
            this.mySideNav1.Controls.Add(this.sideNavPanelX3);
            this.mySideNav1.Controls.Add(this.sideNavPanelX2);
            this.mySideNav1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mySideNav1.EnableClose = false;
            this.mySideNav1.EnableMaximize = false;
            this.mySideNav1.EnableSplitter = false;
            this.mySideNav1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mySideNav1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.sideNavItem1,
            this.separatorX1,
            this.sideNavItem2,
            this.sideNavItem3,
            this.sideNavItem4,
            this.sideNavItemExit});
            this.mySideNav1.Location = new System.Drawing.Point(0, 0);
            this.mySideNav1.Name = "mySideNav1";
            this.mySideNav1.Padding = new System.Windows.Forms.Padding(1);
            this.mySideNav1.Size = new System.Drawing.Size(843, 474);
            this.mySideNav1.TabIndex = 2;
            this.mySideNav1.Text = "mySideNav1";
            // 
            // sideNavPanelX1
            // 
            this.sideNavPanelX1.BackColor = System.Drawing.Color.AliceBlue;
            this.sideNavPanelX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanelX1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sideNavPanelX1.Location = new System.Drawing.Point(129, 32);
            this.sideNavPanelX1.Name = "sideNavPanelX1";
            this.sideNavPanelX1.Size = new System.Drawing.Size(713, 441);
            this.sideNavPanelX1.TabIndex = 2;
            // 
            // sideNavPanelX3
            // 
            this.sideNavPanelX3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanelX3.Location = new System.Drawing.Point(128, 41);
            this.sideNavPanelX3.Name = "sideNavPanelX3";
            this.sideNavPanelX3.Size = new System.Drawing.Size(714, 432);
            this.sideNavPanelX3.TabIndex = 10;
            this.sideNavPanelX3.Visible = false;
            // 
            // sideNavPanelX2
            // 
            this.sideNavPanelX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideNavPanelX2.Location = new System.Drawing.Point(128, 41);
            this.sideNavPanelX2.Name = "sideNavPanelX2";
            this.sideNavPanelX2.Size = new System.Drawing.Size(714, 432);
            this.sideNavPanelX2.TabIndex = 6;
            this.sideNavPanelX2.Visible = false;
            // 
            // sideNavItem1
            // 
            this.sideNavItem1.ColorTable = DevComponents.DotNetBar.eButtonColor.Magenta;
            this.sideNavItem1.IsSystemMenu = true;
            this.sideNavItem1.Name = "sideNavItem1";
            this.sideNavItem1.Symbol = "";
            this.sideNavItem1.Text = "Menu";
            // 
            // separatorX1
            // 
            this.separatorX1.FixedSize = new System.Drawing.Size(3, 1);
            this.separatorX1.Name = "separatorX1";
            this.separatorX1.Padding.Bottom = 2;
            this.separatorX1.Padding.Left = 6;
            this.separatorX1.Padding.Right = 6;
            this.separatorX1.Padding.Top = 2;
            this.separatorX1.SeparatorOrientation = DevComponents.DotNetBar.eDesignMarkerOrientation.Vertical;
            // 
            // sideNavItem2
            // 
            this.sideNavItem2.Checked = true;
            this.sideNavItem2.ColorTable = DevComponents.DotNetBar.eButtonColor.Magenta;
            this.sideNavItem2.Name = "sideNavItem2";
            this.sideNavItem2.Panel = this.sideNavPanelX1;
            this.sideNavItem2.Symbol = "";
            this.sideNavItem2.Text = "系统配置";
            // 
            // sideNavItem3
            // 
            this.sideNavItem3.Name = "sideNavItem3";
            this.sideNavItem3.Panel = this.sideNavPanelX2;
            this.sideNavItem3.Symbol = "";
            this.sideNavItem3.Text = "数据库";
            // 
            // sideNavItem4
            // 
            this.sideNavItem4.Name = "sideNavItem4";
            this.sideNavItem4.Panel = this.sideNavPanelX3;
            this.sideNavItem4.Symbol = "57346";
            this.sideNavItem4.SymbolSet = DevComponents.DotNetBar.eSymbolSet.Material;
            this.sideNavItem4.Text = "文件或文件夹";
            // 
            // sideNavItemExit
            // 
            this.sideNavItemExit.Name = "sideNavItemExit";
            this.sideNavItemExit.Symbol = "";
            this.sideNavItemExit.Text = "退出";
            // 
            // reflectionLabel1
            // 
            // 
            // 
            // 
            this.reflectionLabel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.reflectionLabel1.Location = new System.Drawing.Point(0, 0);
            this.reflectionLabel1.Name = "reflectionLabel1";
            this.reflectionLabel1.Size = new System.Drawing.Size(448, 70);
            this.reflectionLabel1.TabIndex = 0;
            this.reflectionLabel1.Text = "    <b><font size=\"+6\" color=\"#5C4A3D\"><i>数据</i><font color=\"#B02B2C\"> 备份上传配置</font></font></b>";

            // 
            // FormX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 579);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormX";
            this.Text = "Form1";
             
            this.panelBottom.ResumeLayout(false);
            this.panelFill.ResumeLayout(false);
            this.mySideNav1.ResumeLayout(false);
            this.mySideNav1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private MySideNav mySideNav1;
        private DevComponents.DotNetBar.Controls.SideNavPanel sideNavPanelX1;
        private DevComponents.DotNetBar.Controls.SideNavItem sideNavItemExit;


        #endregion
    }
}
