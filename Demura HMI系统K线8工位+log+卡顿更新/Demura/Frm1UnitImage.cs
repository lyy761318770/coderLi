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
    public partial class Frm1UnitImage : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//创建一个页面更新的定时器
        private PLCService1Y1 plcServiceCurrent = null;

        public Frm1UnitImage(int Y1R,int Y2R,int TH1L,int TH2L, int i, PLCService1Y1 plcService)
        {
            InitializeComponent();
            plcServiceCurrent=plcService;
            try
            {
                plcService.ConnectPLCImage12(Y1R, Y2R, TH1L, TH2L, i);
                if (plcService.connect.IsSuccess)
                {
                    tsl_ConnectState.Text = "连接成功";
                    tsl_ConnectState.ForeColor = Color.Green;
                    AppLog.WriteInfo("连接PLC手动拍照界面成功", "log日志", true);
                }
                else
                {
                    tsl_ConnectState.Text = "连接失败";
                    tsl_ConnectState.ForeColor = Color.Red;
                    AppLog.WriteInfo("连接PLC手动拍照界面失败", "log日志", true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("PLC连接异常，请检查网络"+ex.Message);
            }         
            grb_1R.Text=((i+4)/2-1).ToString()+ "工位RAM1拍照（右）";
            grb_1L.Text = ((i+4)/2-1).ToString() + "工位RAM1拍照（左）";
            grb_2R.Text = ((i+4)/2).ToString() + "工位RAM1拍照（右）";
            grb_2L.Text = ((i+4)/2).ToString() + "工位RAM1拍照（左）";

            btn_MenulPhoto1R.Tag = "M" + (820 + i).ToString();
            btn_MenulPhoto1L.Tag = "M" + (821 + i).ToString();
            btn_MemulPhoto2R.Tag = "M" + (822 + i).ToString();
            btn_MemulPhoto2L.Tag = "M" + (823 + i).ToString();

            btn_MarkPosition1R.Tag = "M" + (840 + i).ToString();
            btn_MarkPosition1L.Tag = "M" + (841 + i).ToString();
            btn_MarkPosition2R.Tag = "M" + (844 + i).ToString();
            btn_MarkPosition2L.Tag = "M" + (845 + i).ToString();    


            btn_TakePicturePosition1R.Tag = "M" + (842 + i).ToString();
            btn_TakePicturePosition1L.Tag = "M" + (843 + i).ToString();
            btn_TakePicturePosition2R.Tag = "M" + (846 + i).ToString();
            btn_TakePicturePosition2L.Tag = "M" + (847 + i).ToString();
            

            this.timer.Interval = 300;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();//定时器开始
        }

        private void Timer_Tick(object sender, EventArgs e)//在这里刷新界面状态
        {
            //一工位三个轴的当前位置
            txt_CurrentPX1R.Text = plcServiceCurrent.CurrentState.CurrentPositionX1R;
            txt_CurrentPX1L.Text = plcServiceCurrent.CurrentState.CurrentPositionX1L;
            txt_CurrentPY1R.Text = plcServiceCurrent.CurrentState.CurrentPositionY1R;
            txt_CurrentPY1L.Text = plcServiceCurrent.CurrentState.CurrentPositionY1L;
            txt_CurrentPθ1R.Text = plcServiceCurrent.CurrentState.CurrentPositionθ1R;
            txt_CurrentPθ1L.Text= plcServiceCurrent.CurrentState.CurrentPositionθ1L;
            //二工位三个轴的当前位置
            txt_CurrentPX2R.Text = plcServiceCurrent.CurrentState.CurrentPositionX2R;
            txt_CurrentPX2L.Text = plcServiceCurrent.CurrentState.CurrentPositionX2L;
            txt_CurrentPY2R.Text = plcServiceCurrent.CurrentState.CurrentPositionY2R;
            txt_CurrentPY2L.Text = plcServiceCurrent.CurrentState.CurrentPositionY2L;
            txt_CurrentPθ2R.Text = plcServiceCurrent.CurrentState.CurrentPositionθ2R;
            txt_CurrentPθ2L.Text = plcServiceCurrent.CurrentState.CurrentPositionθ2L;
            //一工位三个轴的拍照位置
            txt_photoPX1R.Text = plcServiceCurrent.CurrentState.PhotoPositionX1R;
            txt_photoPX1L.Text = plcServiceCurrent.CurrentState.PhotoPositionX1L;
            txt_photoPY1R.Text = plcServiceCurrent.CurrentState.PhotoPositionY1R;
            txt_photoPY1L.Text = plcServiceCurrent.CurrentState.PhotoPositionY1L;
            txt_photoPθ1R.Text = plcServiceCurrent.CurrentState.PhotoPositionθ1R;
            txt_photoPθ1L.Text = plcServiceCurrent.CurrentState.PhotoPositionθ1L;
            //二工位三个轴的拍照位置
            txt_photoPX2R.Text = plcServiceCurrent.CurrentState.PhotoPositionX2R;
            txt_photoPX2L.Text = plcServiceCurrent.CurrentState.PhotoPositionX2L;
            txt_photoPY2R.Text = plcServiceCurrent.CurrentState.PhotoPositionY2R;
            txt_photoPY2L.Text = plcServiceCurrent.CurrentState.PhotoPositionY2L;
            txt_photoPθ2R.Text = plcServiceCurrent.CurrentState.PhotoPositionθ2R;
            txt_photoPθ2L.Text = plcServiceCurrent.CurrentState.PhotoPositionθ2L;
            //一工位三个轴的扎针基准位置
            txt_needlePX1R.Text = plcServiceCurrent.CurrentState.NeedlePositionX1R;
            txt_needlePX1L.Text = plcServiceCurrent.CurrentState.NeedlePositionX1L;
            txt_needlePY1R.Text = plcServiceCurrent.CurrentState.NeedlePositionY1R;
            txt_needlePY1L.Text = plcServiceCurrent.CurrentState.NeedlePositionY1L;
            txt_needlePθ1R.Text = plcServiceCurrent.CurrentState.NeedlePositionθ1R;
            txt_needlePθ1L.Text = plcServiceCurrent.CurrentState.NeedlePositionθ1L;
            //二工位三个轴的扎针基准位置
            txt_needlePX2R.Text = plcServiceCurrent.CurrentState.NeedlePositionX2R;
            txt_needlePX2L.Text = plcServiceCurrent.CurrentState.NeedlePositionX2L;
            txt_needlePY2R.Text = plcServiceCurrent.CurrentState.NeedlePositionY2R;
            txt_needlePY2L.Text = plcServiceCurrent.CurrentState.NeedlePositionY2L;
            txt_needlePθ2R.Text = plcServiceCurrent.CurrentState.NeedlePositionθ2R;
            txt_needlePθ2L.Text = plcServiceCurrent.CurrentState.NeedlePositionθ2L;
            //一工位三个轴的扎针补正位置
            txt_needleADDPX1R.Text = plcServiceCurrent.CurrentState.NeedleAddPositionX1R;
            txt_needleADDPX1L.Text = plcServiceCurrent.CurrentState.NeedleAddPositionX1L;
            txt_needleADDPY1R.Text = plcServiceCurrent.CurrentState.NeedleAddPositionY1R;
            txt_needleADDPY1L.Text = plcServiceCurrent.CurrentState.NeedleAddPositionY1L;
            txt_needleADDPθ1R.Text = plcServiceCurrent.CurrentState.NeedleAddPositionθ1R;
            txt_needleADDPθ1L.Text = plcServiceCurrent.CurrentState.NeedleAddPositionθ1L;
            //二工位三个轴的扎针补正位置
            txt_needleADDPX2R.Text = plcServiceCurrent.CurrentState.NeedleAddPositionX2R;
            txt_needleADDPX2L.Text = plcServiceCurrent.CurrentState.NeedleAddPositionX2L;
            txt_needleADDPY2R.Text = plcServiceCurrent.CurrentState.NeedleAddPositionY2R;
            txt_needleADDPY2L.Text = plcServiceCurrent.CurrentState.NeedleAddPositionY2L;
            txt_needleADDPθ2R.Text = plcServiceCurrent.CurrentState.NeedleAddPositionθ2R;
            txt_needleADDPθ2L.Text = plcServiceCurrent.CurrentState.NeedleAddPositionθ2L;
        }

        private void btn_Click(object sender, EventArgs e)//sender为事件本身
        {
            if (sender is Button btn)//判断sender是否为button按钮
            {
                //Button btn = sender as Button;

                if (btn.Tag != null)
                {
                    try
                    {
                        plcServiceCurrent.ExcuteTask1(btn.Tag.ToString());//调用写入方法，参数为Tag值
                        AppLog.WriteInfo($"{btn.Tag.ToString()}按钮单击", "log日志", true);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteInfo($"{btn.Tag.ToString()}按钮单击失败", "log日志", true);
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        private void Frm1UnitImage_FormClosing(object sender, FormClosingEventArgs e)
        {
            //plcServiceCurrent.DisConnectPLC();
            plcServiceCurrent.cts?.Cancel();
            timer.Stop();
            AppLog.WriteInfo("手动拍照页面窗体关闭", "log日志", true);
        }
    }
}
