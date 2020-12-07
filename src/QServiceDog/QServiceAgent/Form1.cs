using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QServiceAgent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
#if DEBUG
            textBox2.Text = "Yczn83658622";
#endif
            this.Shown += Form1_Shown;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox1.Text = System.Environment.UserName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show(this,"用户名称密码不能为空");
                return;
            }
            AutoLoginHelper.Instance.SetAutoLogin(textBox1.Text, textBox2.Text);
            //检查一遍
            string userName;
            bool isAutoLogin = AutoLoginHelper.Instance.IsAutoLogin(out userName);
            if (isAutoLogin)
            {
                MessageBox.Show($"已启用{userName}自动登录操作系统\r\n请在适当的时候重启计算机观察自动登录是否生效");
            }
        }
    }
}
