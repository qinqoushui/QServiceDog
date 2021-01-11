using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using QCommon.UI;
using QDBDog;
using QDBDog.Share;

namespace QDBDogUI.UI
{
    public partial class UIBase : UserControl
    {
        protected static readonly Q.Helper.ILogHelper logger = Q.Helper.LogHelper.Default.Get(nameof(UIBase));
        public UIBase()
        {
            InitializeComponent();
        }
        protected TableLayoutPanel theTableLayoutPanel;

        protected string defaultFileName = System.IO.Path.Combine(Application.StartupPath, "config\\appsettings.json");
        public QDBDog.Config loadConfig(bool allowNew = true)
        {
            return Config.LoadConfig(defaultFileName, allowNew);
        }

        public UIBase LoadConfig()
        {
            render(loadConfig());
            return this;
        }

        protected void loadConfigFromFile()
        {
            //从指定文件加载 
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "配置文件|*.json;*.json.bak";
                dlg.FilterIndex = 0;
                dlg.CheckFileExists = true;
                dlg.Multiselect = false;
                dlg.Title = "选择配置文件";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string s = System.IO.File.ReadAllText(dlg.FileName, Encoding.UTF8);
                    var config = s.De<Config>();
                    if (config == null)
                    {
                        MessageBox.Show("无效的配置文件");
                        return;
                    }
                    render(config);
                }

            }
        }
        protected void loadDefault()
        {
            if (MessageBox.Show("恢复默认配置？当前的修改将全部丢失", "备份配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            render(new QDBDog.Config());
        }

        protected void save()
        {
            if (MessageBox.Show("保存当前配置？", "备份配置", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            if (collect(out var c))
            {
                string fileName = defaultFileName;
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
                if (System.IO.File.Exists(fileName))
                    System.IO.File.Copy(fileName, $"{fileName}.{DateTime.Now.Minute % 10 }.json.bak", true);
                System.IO.File.WriteAllText(fileName, c.Serialize(), Encoding.UTF8);
                MessageBox.Show("OK");
            }
        }


        protected virtual void render(QDBDog.Config config)
        {
            var data = config.ToDict();
            render(theTableLayoutPanel.Controls, data);
        }
        void render(ControlCollection controls, KeyValuePair<string, string>[] data)
        {
            foreach (var control in controls)
            {
                var ap = control as Panel;
                if (ap != null)
                    render(ap.Controls, data);
                else
                {
                    var ac = control as TextBox;
                    if (ac != null && ac.Tag != null)
                    {
                        var a = data.FirstOrDefault(r => r.Key.Equals(ac.Tag.ToString(), StringComparison.OrdinalIgnoreCase));
                        ac.Text = a.Value;
                    }
                }
            }
        }
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[a-zA-Z0-9]+[a-zA-Z0-9/\\]{0,50}");
        protected bool collect(out QDBDog.Config config)
        {
            //先用从当文件中读取配置？
            config = loadConfig();
            Dictionary<string, string> data = new Dictionary<string, string>();
            collect(theTableLayoutPanel.Controls, data);
            collectSub(data);
            config = data.ToArray().CopyNameValuePairs<Config>(config);
            //强制节点名为纯字母，取最后一个符合要求的
            if (reg.IsMatch(config.ServerPath))
            {
                var m = reg.Matches(config.ServerPath);
                config.ServerPath = m[m.Count - 1].Value.Replace("\\", "/").ToUpper();
            }
            else
            {
                MessageBox.Show("备份上传节点代码无效，只能使用字母和数字，如sz\\m2");
            }
            return true;
        }
        void collect(ControlCollection controls, Dictionary<string, string> data)
        {
            foreach (var control in controls)
            {
                var ap = control as Panel;
                if (ap != null)
                    collect(ap.Controls, data);
                else
                {
                    var ac = control as TextBox;
                    if (ac != null && ac.Tag != null)
                    {
                        data[ac.Tag.ToString()] = ac.Text;
                    }
                }
            }
        }
        protected virtual void collectSub(Dictionary<string, string> data)
        {

        }

        protected void showResult(string result, string title = "提示")
        {
            Form frm = new Form();
            frm.Size = new Size(600, 600);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.TopMost = true;
            TextBox tb = new TextBox();
            frm.Controls.Add(tb);
            tb.Dock = DockStyle.Fill;
            tb.Multiline = true;
            tb.ScrollBars = ScrollBars.Vertical;
            tb.Text = result;
            frm.MinimizeBox = false;
            frm.Text = title;
            frm.ShowDialog();
        }

        

        protected void lockButton(int second, Button btn)
        {
            System.Windows.Forms.Timer timer = new Timer();
            timer.Interval = second * 1000;
            timer.Tick += (s, e) =>
            {
                btn.Enabled = true;
                timer.Stop();
                timer.Dispose();
                timer = null;
            };
            timer.Start();
            btn.Enabled = false;
        }
    }
}
