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
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//����һ��ҳ����µĶ�ʱ��
        private PLCService1Y1 plcService = new PLCService1Y1();
        public FrmMain()
        {
            InitializeComponent();
            plcService.Connection();
            plcService.ConnectPLCEnter();
            this.timer.Interval = 300;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();//��ʱ����ʼ

            if (plcService.connect.IsSuccess)
            {
                tsl_ConnectState.Text = "   ��½�ɹ�";
                tsl_ConnectState.ForeColor = Color.Green;
                AppLog.WriteInfo("����PLC������ɹ�", "log��־", true);
            }
            else
            {
                tsl_ConnectState.Text = "   ��½ʧ��";
                tsl_ConnectState.ForeColor = Color.Red;
                AppLog.WriteInfo("����PLC������ʧ��", "log��־", true);
            }

            btn_unitOperation_Click(null, null);
        }

        private void Timer_Tick(object sender, EventArgs e)//������ˢ�½���״̬
        {
            //�������
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

        #region ����Ƕ��
        //�ر�Ƕ��Ĵ���
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
            ClosePreForm();//�ر�ǰ��Ĵ���
            objForm.TopLevel = false;//���Ӵ������óɷǶ����ؼ�      
            objForm.FormBorderStyle = FormBorderStyle.None;//ȥ���Ӵ���ı߿�
            objForm.Parent = this.spContainer.Panel2;//ָ���Ӵ�����ʾ������  
            objForm.Dock = DockStyle.Fill;//����������С�Զ����������С
            objForm.Show();
        }
        #endregion

        private void tsm_1Y1R_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem tsm)//�ж�sender�Ƿ�Ϊbutton��ť
            {
                string[] varArry = ((ToolStripMenuItem)sender).Tag.ToString().Split('|');
                if (tsm.Tag != null)
                {
                    try
                    {
                        Frm1Y1 frmOneY1Right = new Frm1Y1(varArry[0], varArry[1], Convert.ToInt32( varArry[2]), plcService);
                        OpenForm(frmOneY1Right);
                        //AppLog.WriteInfo($"�򿪵���{varArry[0]}�ֶ�����ɹ�", "log��־", true);
                    }
                    catch (Exception ex)
                    {
                        //AppLog.WriteInfo($"�򿪵���{varArry[0]}�ֶ�����ʧ��", "log��־", true);
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }
        private void btn_unitOperation_Click(object sender, EventArgs e)
        {
            FrmUnitOperation frmUnitOperation = new FrmUnitOperation(plcService);
            OpenForm(frmUnitOperation);
            //AppLog.WriteInfo("�����Զ���������ɹ�", "log��־", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)//�ж�sender�Ƿ�Ϊbutton��ť
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
            AppLog.WriteInfo("������ر�", "log��־", true);
        }
    }
}