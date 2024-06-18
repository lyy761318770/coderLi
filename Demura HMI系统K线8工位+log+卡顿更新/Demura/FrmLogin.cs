using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demura

{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        public void btn_login_Click(object sender, EventArgs e)
        {           
            if (this.txt_UserName.Text.Trim().Length == 0)
            {
                AppLog.WriteInfo("登陆账号未填写", "log日志", true);
                MessageBox.Show("请输入登录账号！", "提示信息");
                this.txt_UserName.Focus();
                return;
            }
         
            if (this.txt_Password.Text.Trim().Length == 0)
            {
                AppLog.WriteInfo("登陆密码未填写", "log日志", true);
                MessageBox.Show("请输入登录密码！", "提示信息");
                this.txt_Password.Focus();
                return;
            }
            if(Convert.ToInt32(this.txt_UserName.Text.Trim())==10000&& this.txt_Password.Text.Trim()=="123456")
            {
                AppLog.WriteInfo("账号密码正确，登陆成功", "log日志", true);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                AppLog.WriteInfo("登陆失败，账号或密码错误", "log日志", true);
                MessageBox.Show("账号密码错误！", "提示信息");
                this.txt_UserName.Focus();
            }


            
        }

        private void txt_UserName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyValue == 13)
            {
                this.txt_UserName.Focus();
                this.txt_UserName.SelectAll();
            }
        }

        private void txt_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                AppLog.WriteInfo("点击了ENTER键盘", "log日志", true);
                btn_login_Click(null, null);
            }
        }
    }
}
