using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Demura;
using System.Configuration;

namespace Demura
{
    public partial class FrmMain : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//创建一个页面更新的定时器
        private PLCService1Y1 plcService = new PLCService1Y1();
        public FrmMain()
        {
            InitializeComponent();
            plcService.Connection();
            plcService.ConnectPLCEnter();
            this.timer.Interval = 300;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();//定时器开始

            if (plcService.connect.IsSuccess)
            {
                tsl_ConnectState.Text = "   登陆成功";
                tsl_ConnectState.ForeColor = Color.Green;
                AppLog.WriteInfo("连接PLC主界面成功", "log日志", true);
            }
            else
            {
                tsl_ConnectState.Text = "   登陆失败";
                tsl_ConnectState.ForeColor = Color.Red;
                AppLog.WriteInfo("连接PLC主界面失败", "log日志", true);
            }

            btn_unitOperation_Click(null, null);
        }

        private void Timer_Tick(object sender, EventArgs e)//在这里刷新界面状态
        {
            //界面防呆
            if (plcService.CurrentState.StartSFlag11)
                tsm_1.Visible = false;
            else
                tsm_1.Visible = true;
            if (plcService.CurrentState.StartSFlag21)
                tsm_2.Visible = false;
            else
                tsm_2.Visible = true;
            if (plcService.CurrentState.StartSFlag31)
                tsm_3.Visible = false;
            else
                tsm_3.Visible = true;
            if (plcService.CurrentState.StartSFlag41)
                tsm_4.Visible = false;
            else
                tsm_4.Visible = true;
            if (plcService.CurrentState.StartSFlag51)
                tsm_5.Visible = false;
            else
                tsm_5.Visible = true;
            if (plcService.CurrentState.StartSFlag61)
                tsm_6.Visible = false;
            else
                tsm_6.Visible = true;
            if (plcService.CurrentState.StartSFlag71)
                tsm_7.Visible = false;
            else
                tsm_7.Visible = true;
            if (plcService.CurrentState.StartSFlag81)
                tsm_8.Visible = false;
            else
                tsm_8.Visible = true;
        }

        #region 窗体嵌入
        //关闭嵌入的窗体
        private void ClosePreForm()
        {
            foreach (Control item in this.spContainer.Panel2.Controls)
            {
                if (item is Form)
                {
                    Form objControl = (Form)item;
                    objControl.Close();
                }
            }
        }
        private void OpenForm(Form objForm)
        {
            ClosePreForm();//关闭前面的窗体
            objForm.TopLevel = false;//将子窗体设置成非顶级控件      
            objForm.FormBorderStyle = FormBorderStyle.None;//去掉子窗体的边框
            objForm.Parent = this.spContainer.Panel2;//指定子窗体显示的容器  
            objForm.Dock = DockStyle.Fill;//随着容器大小自动调整窗体大小
            objForm.Show();
        }
        #endregion

        private void tsm_1Y1R_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem tsm)//判断sender是否为button按钮
            {
                string[] varArry = ((ToolStripMenuItem)sender).Tag.ToString().Split('|');
                if (tsm.Tag != null)
                {
                    try
                    {
                        Frm1Y1 frmOneY1Right = new Frm1Y1(varArry[0], varArry[1], Convert.ToInt32( varArry[2]), plcService);
                        OpenForm(frmOneY1Right);
                        //AppLog.WriteInfo($"打开单轴{varArry[0]}手动界面成功", "log日志", true);
                    }
                    catch (Exception ex)
                    {
                        //AppLog.WriteInfo($"打开单轴{varArry[0]}手动界面失败", "log日志", true);
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }
        private void btn_unitOperation_Click(object sender, EventArgs e)
        {
            FrmUnitOperation frmUnitOperation = new FrmUnitOperation(plcService);
            OpenForm(frmUnitOperation);
            //AppLog.WriteInfo("打开手自动报警界面成功", "log日志", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)//判断sender是否为button按钮
            {
                string[] varArry = ((Button)sender).Tag.ToString().Split('|');
                if (btn.Tag != null)
                {
                    try
                    {
                        Frm1UnitImage frm1UnitImage = new Frm1UnitImage(Convert.ToInt32(varArry[0]), Convert.ToInt32(varArry[1]), Convert.ToInt32(varArry[2]), Convert.ToInt32(varArry[3]), Convert.ToInt32(varArry[4]), plcService);
                        OpenForm(frm1UnitImage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            plcService.DisConnectPLC();
            timer.Stop();
            AppLog.WriteInfo("主窗体关闭", "log日志", true);
        }
    }
}