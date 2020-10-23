using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QDBDog;

namespace QDBDogUI.UI
{
    public partial class UISystem : UIBase
    {
        public UISystem()
        {
            InitializeComponent();
            theTableLayoutPanel = tableLayoutPanel1;
            btnDefault.Click += (s, e) => loadDefault();
            btnSave.Click += (s, e) => save();
            btnLoad.Click += (s, e) => loadConfigFromFile();
        }

        protected override void render(Config config)
        {
            base.render(config);
            var data = config.ToDict();
            foreach (var control in panel1.Controls)
            {
                var ac = control as RadioButton;
                if (ac != null && ac.Tag != null)
                {
                    var a = data.FirstOrDefault(r => r.Key.Equals(panel1.Tag.ToString(), StringComparison.OrdinalIgnoreCase));
                    ac.Checked = a.Value == ac.Tag.ToString();
                }
            }
            tbName.Text = config.Name;
            numExpire.Value = config.ExpireValue;
        }

        protected override void collectSub(Dictionary<string, string> data)
        {
            base.collectSub(data);
            data["Name"] = tbName.Text;
            data["ExpireValue"] = numExpire.Value.ToString("0");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.ShowNewFolderButton = true;
                dlg.Description = "指定保存备份的本地文件夹";
                dlg.SelectedPath = "d:\\data\\sql\\autobackup";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    tbPath.Text = dlg.SelectedPath;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://cron.qqe2.com/");
        }

    }
}
