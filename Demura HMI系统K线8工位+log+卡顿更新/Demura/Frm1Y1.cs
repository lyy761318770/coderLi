using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using HslCommunication;
using HslCommunication.Profinet;
using HslCommunication.Profinet.Melsec;

namespace Demura
{
    public partial class Frm1Y1 : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//创建一个页面更新的定时器
        private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
        private bool temp = false;
        private object objLock = new object();//创建锁，使得读写不能同时使用
        private PLCService1Y1 plcServiceCurrent = null;
        private int[] txtbox = new int[8];
        public Frm1Y1(string m, string d, int i, PLCService1Y1 plcService)
        {
            InitializeComponent();
            plcServiceCurrent=plcService;
            plcService.ConnectPLC(m, d);
            if (plcService.connect.IsSuccess)
            {
                tsl_ConnectState.Text = "连接成功";
                tsl_ConnectState.ForeColor = Color.Green;

                AppLog.WriteInfo($"连接PLC单轴{m}界面成功", "log日志", true);
            }
            else
            {
                tsl_ConnectState.Text = "连接失败";
                tsl_ConnectState.ForeColor = Color.Red;
                AppLog.WriteInfo($"连接PLC单轴{m}界面失败", "log日志", true);
            }
            this.timer.Interval = 300;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();//定时器开始

            this.timer1.Interval = 2000;
            this.timer1.Tick += Timer1_Tick;
            this.timer1.Start();//定时器开始

            //实例化一个xmlDoucument()的对象
            XmlDocument xDoc = new XmlDocument();
            //加载根目录这个路径的xml
            xDoc.Load("PLC数据.xml");
            //获取根节点PLCDate，取跟节点下的第一个
            XmlNode node = xDoc.SelectSingleNode("PLCDate");
            XmlNodeList nodeList = node.ChildNodes;
            txt_setAccT.Tag = ((XmlElement)nodeList.Item(i))["txt_setAccT"].InnerText;
            txt_setDecT.Tag = ((XmlElement)nodeList.Item(i))["txt_setDecT"].InnerText;
            txt_setMenulV.Tag = ((XmlElement)nodeList.Item(i))["txt_setMenulV"].InnerText;
            txt_setAutoV.Tag = ((XmlElement)nodeList.Item(i))["txt_setAutoV"].InnerText;
            //btn_AutoStart.Tag = ((XmlElement)nodeList.Item(i))["btn_AutoStart"].InnerText;
            //btn_AutoStop.Tag = ((XmlElement)nodeList.Item(i))["btn_AutoStop"].InnerText;
            //btn_Auto.Tag = ((XmlElement)nodeList.Item(i))["btn_Auto"].InnerText;
            //btn_Menul.Tag = ((XmlElement)nodeList.Item(i))["btn_Menul"].InnerText;
            //btn_Reset.Tag = ((XmlElement)nodeList.Item(i))["btn_Reset"].InnerText;
            lab_awaitP.Tag = ((XmlElement)nodeList.Item(i))["lab_awaitP"].InnerText;
            lab_photoP.Tag = ((XmlElement)nodeList.Item(i))["lab_photoP"].InnerText;
            lab_needleP.Tag = ((XmlElement)nodeList.Item(i))["lab_needleP"].InnerText;
            lab_needleADDP.Tag = ((XmlElement)nodeList.Item(i))["lab_needleADDP"].InnerText;
            btn_awaitTakeP.Tag = ((XmlElement)nodeList.Item(i))["btn_awaitTakeP"].InnerText;
            btn_photoTakeP.Tag = ((XmlElement)nodeList.Item(i))["btn_photoTakeP"].InnerText;
            btn_needleTakeP.Tag = ((XmlElement)nodeList.Item(i))["btn_needleTakeP"].InnerText;
            btn_needleADDTakeP.Tag = ((XmlElement)nodeList.Item(i))["btn_needleADDTakeP"].InnerText;
            btn_ORP.Tag = ((XmlElement)nodeList.Item(i))["btn_ORP"].InnerText;
            btn_JOGR.Tag = ((XmlElement)nodeList.Item(i))["btn_JOGR"].InnerText;
            btn_JOGL.Tag = ((XmlElement)nodeList.Item(i))["btn_JOGL"].InnerText;
            btn_stop.Tag = ((XmlElement)nodeList.Item(i))["btn_stop"].InnerText;
            btn_jogControl.Tag = ((XmlElement)nodeList.Item(i))["btn_jogControl"].InnerText;
            btn_1mm.Tag = ((XmlElement)nodeList.Item(i))["btn_1mm"].InnerText;
            btn_500um.Tag = ((XmlElement)nodeList.Item(i))["btn_500um"].InnerText;
            btn_100um.Tag = ((XmlElement)nodeList.Item(i))["btn_100um"].InnerText;
            but_save.Tag = ((XmlElement)nodeList.Item(i))["but_save"].InnerText;
            btn_AxisMoveY.Tag = ((XmlElement)nodeList.Item(i))["btn_AxisMoveY"].InnerText;
            btn_removealarm.Tag = ((XmlElement)nodeList.Item(i))["btn_removealarm"].InnerText;
            this.lab_axisName.Text = ((XmlElement)nodeList.Item(i)).GetAttribute("Name");
           
        }

        /// <summary>
        /// 上升沿的捕获方法
        /// </summary>
        /// <param name="value"></param>
        private void PtrigChk(bool value)
        {
            if (value == true && temp == false)//判断上升沿，上一次的值由false到true时
            {
                this.pb_Motor.Image = Properties.Resources.冷却泵ON;
                this.temp = value;//更新缓存值（上一次的值）
            }
        }

        /// <summary>
        /// 捕获下降沿的方法
        /// </summary>
        /// <param name="value"></param>
        private void NtrigChk(bool value)
        {
            if (value == false && temp == true)
            {
                this.pb_Motor.Image = Properties.Resources.冷却泵OFF;
                this.temp = value;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)//在这里刷新界面状态
        {
            //目前参数显示
            txt_CurrentP.Text = ((plcServiceCurrent.CurrentState.CurrentPosition) * 0.001).ToString();
            txt_CurrentMenulV.Text = plcServiceCurrent.CurrentState.CurrentMenulSpeed;
            txt_CurrentAutoV.Text = plcServiceCurrent.CurrentState.CurrentAutoSpeed;
            lab_runV1Y1.Text = plcServiceCurrent.CurrentState.RunSpeed;
            lab_load1Y1.Text = plcServiceCurrent.CurrentState.Load;

            bool value = plcServiceCurrent.CurrentState.Busy;//控制状态，为手自动状态
            PtrigChk(value);
            NtrigChk(value);
            //负极限显示
            if (plcServiceCurrent.CurrentState.LimitLeft)
            {
                lab_lLimit.BackColor = Color.Red;
            }
            else
            {
                lab_lLimit.BackColor = Color.Transparent;
            }
            //正极限显示
            if (plcServiceCurrent.CurrentState.LimitRight)
            {
                lab_rLimit.BackColor = Color.Red;
            }
            else
            {
                lab_rLimit.BackColor = Color.Transparent;
            }
            //原点信号、回原按钮显示
            if (plcServiceCurrent.CurrentState.LimitOriginal)
            {
                lab_origin.BackColor = Color.Red;
                btn_ORP.BackColor = Color.Lime;
            }
            else
            {
                lab_origin.BackColor = Color.Transparent;
                btn_ORP.BackColor = Color.Green;
            }
            //待机位显示
            if (plcServiceCurrent.CurrentState.AwaitPFinish)
            {
                btn_awaitTakeP.BackColor = Color.Lime;
            }
            else
            {
                btn_awaitTakeP.BackColor = Color.Green;
            }
            //拍照位显示
            if (plcServiceCurrent.CurrentState.PhotoPFinish)
            {
                btn_photoTakeP.BackColor = Color.Lime;
            }
            else
            {
                btn_photoTakeP.BackColor = Color.Green;
            }
            //扎针位显示
            if (plcServiceCurrent.CurrentState.NeedlePFinish)
            {
                btn_needleTakeP.BackColor = Color.Lime;
            }
            else
            {
                btn_needleTakeP.BackColor = Color.Green;
            }
            //扎针补正位显示
            if (plcServiceCurrent.CurrentState.NeedleADDFinish)
            {
                btn_needleADDTakeP.BackColor = Color.Lime;
            }
            else
            {
                btn_needleADDTakeP.BackColor = Color.Green;
            }
            //寸动控制显示
            if (plcServiceCurrent.CurrentState.JOGControl)
            {
                btn_jogControl.BackColor = Color.Lime;
                if (plcServiceCurrent.CurrentState.JOGDistance == 1)
                {

                    btn_1mm.BackColor = Color.Lime;
                    btn_500um.BackColor = Color.Green;
                    btn_100um.BackColor = Color.Green;

                }
                //寸动0.5mm显示
                if (plcServiceCurrent.CurrentState.JOGDistance == 2)
                {
                    btn_1mm.BackColor = Color.Green;
                    btn_500um.BackColor = Color.Lime;
                    btn_100um.BackColor = Color.Green;
                }
                //寸动0.1mm显示
                if (plcServiceCurrent.CurrentState.JOGDistance == 3)
                {
                    btn_1mm.BackColor = Color.Green;
                    btn_500um.BackColor = Color.Green;
                    btn_100um.BackColor = Color.Lime;
                }
            }
            else
            {
                btn_jogControl.BackColor = Color.Green;
                btn_1mm.BackColor = Color.Green;
                btn_500um.BackColor = Color.Green;
                btn_100um.BackColor = Color.Green;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)//在这里刷新界面状态
        {         
            //定点位位置显示
            txt_awaitP.Text = plcServiceCurrent.CurrentState.AwaitPosition;
            txt_photoP.Text = plcServiceCurrent.CurrentState.PhotoPosition;
            txt_needleP.Text = plcServiceCurrent.CurrentState.NeedlePosition;
            txt_needleADDP.Text = plcServiceCurrent.CurrentState.NeedleAddPosition;
            //参数修改显示
            txt_setMenulV.Text = plcServiceCurrent.CurrentState.SetMenulSpeed;
            txt_setAutoV.Text = plcServiceCurrent.CurrentState.SetAutoSpeed;
            txt_setAccT.Text = plcServiceCurrent.CurrentState.SetAccTime;
            txt_setDecT.Text = plcServiceCurrent.CurrentState.SetDecTime;
            timer1.Stop();
        }

        #region 核心功能实现，点击各个按钮功能
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

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)//判断sender是否为button按钮
            {
                //Button btn = sender as Button;

                if (btn.Tag != null)
                {
                    try
                    {
                        plcServiceCurrent.ExcuteTask2(btn.Tag.ToString());//调用写入方法，参数为Tag值
                        AppLog.WriteInfo($"{btn.Tag.ToString()}按钮按下", "log日志", true);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteInfo($"{btn.Tag.ToString()}按钮按下失败", "log日志", true);
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)//判断sender是否为button按钮
            {
                //Button btn = sender as Button;

                if (btn.Tag != null)
                {
                    try
                    {
                        plcServiceCurrent.ExcuteTask3(btn.Tag.ToString());//调用写入方法，参数为Tag值
                        AppLog.WriteInfo($"{btn.Tag.ToString()}按钮抬起", "log日志", true);
                    }
                    catch (Exception ex)
                    {
                        AppLog.WriteInfo($"{btn.Tag.ToString()}按钮抬起失败", "log日志", true);
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }


        #endregion

        private void but_save_Click(object sender, EventArgs e)
        {
            
            //lock (objLock)
            //{
                try
                {
                    txtbox[0] = Convert.ToInt32(Convert.ToDouble(txt_setMenulV.Text) * 1000);
                    txtbox[1] = Convert.ToInt32(Convert.ToDouble(txt_setAutoV.Text) * 1000);
                    txtbox[2] = Convert.ToInt32(Convert.ToDouble(txt_setAccT.Text) * 1000);
                    txtbox[3] = Convert.ToInt32(Convert.ToDouble(txt_setDecT.Text) * 1000);
                    txtbox[4] = Convert.ToInt32(Convert.ToDouble(txt_awaitP.Text) * 1000);
                    txtbox[5] = Convert.ToInt32(Convert.ToDouble(txt_photoP.Text) * 1000);
                    txtbox[6] = Convert.ToInt32(Convert.ToDouble(txt_needleP.Text) * 1000);
                    txtbox[7] = Convert.ToInt32(Convert.ToDouble(txt_needleADDP.Text) * 1000);
                //保存手动速度、自动速度、加速度、减速带                   
                plcServiceCurrent.WriteInt(txt_setMenulV.Tag.ToString(), txtbox[0]);
                plcServiceCurrent.WriteInt(txt_setAutoV.Tag.ToString(), txtbox[1]);
                plcServiceCurrent.WriteInt(txt_setAccT.Tag.ToString(), txtbox[2]);
                plcServiceCurrent.WriteInt(txt_setDecT.Tag.ToString(), txtbox[3]);
                //保存待机位、拍照位、扎针基准位、扎针补正位
                plcServiceCurrent.WriteInt(lab_awaitP.Tag.ToString(), txtbox[4]);//Convert.ToInt32(txt_awaitP.Text) * 1000
                plcServiceCurrent.WriteInt(lab_photoP.Tag.ToString(), txtbox[5]);
                plcServiceCurrent.WriteInt(lab_needleP.Tag.ToString(), txtbox[6]);
                plcServiceCurrent.WriteInt(lab_needleADDP.Tag.ToString(), txtbox[7]);


                    ////保存手动速度、自动速度、加速度、减速带
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(txt_setMenulV.Tag.ToString(), Convert.ToInt32(txt_setMenulV.Text) * 1000);
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(txt_setAutoV.Tag.ToString(), Convert.ToInt32(txt_setAutoV.Text) * 1000);
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(txt_setAccT.Tag.ToString(), Convert.ToInt32(txt_setAccT.Text) * 1000);
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(txt_setDecT.Tag.ToString(), Convert.ToInt32(txt_setDecT.Text) * 1000);
                    ////保存待机位、拍照位、扎针基准位、扎针补正位
                    //plcService.WriteInt(lab_awaitP.Tag.ToString(), Convert.ToInt32(txt_awaitP.Text) * 1000);//Convert.ToInt32(txt_awaitP.Text) * 1000
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(lab_photoP.Tag.ToString(), Convert.ToInt32(txt_photoP.Text) * 1000);
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(lab_needleP.Tag.ToString(), Convert.ToInt32(txt_needleP.Text) * 1000);
                    //Thread.Sleep(1000);
                    //plcService.WriteInt(lab_needleADDP.Tag.ToString(), Convert.ToInt32(txt_needleADDP.Text) * 1000);


                    //melsec_net.Write("M4125", true);
                    //plcService.ExcuteTask1(but_save.Tag.ToString());
                    AppLog.WriteInfo($"{but_save.Tag.ToString()}保存按钮单击", "log日志", true);
                }
                catch (Exception)
                {
                    AppLog.WriteInfo($"{but_save.Tag.ToString()}保存按钮单击失败", "log日志", true);
                    //MessageBox.Show("bool量写入异常" + ex.Message);
                }
            //}

        }

        private void btn_jogControl_Click(object sender, EventArgs e)
        {
            if (plcServiceCurrent.CurrentState.JOGControl)
            {
                plcServiceCurrent.ExcuteTask3(btn_jogControl.Tag.ToString());
                AppLog.WriteInfo("寸动控制按钮单击", "log日志", true);
            }
            else
            {
                plcServiceCurrent.ExcuteTask2(btn_jogControl.Tag.ToString());
                AppLog.WriteInfo("寸动控制按钮单击", "log日志", true);
            }
        }

        private void btn_1mm_Click(object sender, EventArgs e)
        {
            plcServiceCurrent.WriteInt(btn_1mm.Tag.ToString(), 1);
            AppLog.WriteInfo("1mm按钮单击", "log日志", true);
        }

        private void btn_500um_Click(object sender, EventArgs e)
        {
            plcServiceCurrent.WriteInt(btn_1mm.Tag.ToString(), 2);
            AppLog.WriteInfo("500um按钮单击", "log日志", true);
        }

        private void btn_100um_Click(object sender, EventArgs e)
        {
            plcServiceCurrent.WriteInt(btn_1mm.Tag.ToString(), 3);
            AppLog.WriteInfo("100um按钮单击", "log日志", true);
        }

        private void FrmOneY1Right_FormClosing(object sender, FormClosingEventArgs e)
        {
            //plcServiceCurrent.DisConnectPLC();
            plcServiceCurrent.cts?.Cancel();
            timer.Stop();
            //timer1.Stop();
            AppLog.WriteInfo("单轴控制页面窗体关闭", "log日志", true);
        }

        private void lab_awaitP_DoubleClick(object sender, EventArgs e)
        {
            txt_awaitP.Text = txt_CurrentP.Text;
        }

        private void lab_photoP_DoubleClick(object sender, EventArgs e)
        {
            txt_photoP.Text = txt_CurrentP.Text;
        }

        private void lab_needleP_DoubleClick(object sender, EventArgs e)
        {
            txt_needleP.Text = txt_CurrentP.Text;
        }

        private void lab_needleADDP_DoubleClick(object sender, EventArgs e)
        {
            txt_needleADDP.Text = txt_CurrentP.Text;
        }

        //private void lab_DoubleClick(object sender, EventArgs e)
        //{
        //    txt_awaitP.Text = txt_CurrentP.Text;
        //    if (sender is Label btn)//判断sender是否为button按钮Convert.ToInt32(plcService.CurrentState.CurrentPosition)*1000
        //    {
        //        if (btn.Tag != null)
        //        {
        //            try
        //            {
        //                plcService.WriteInt(btn.Tag.ToString(), plcService.CurrentState.CurrentPosition);                       
        //                AppLog.WriteInfo($"{btn.Tag.ToString()}按钮被双击", "log日志", true);
        //            }
        //            catch (Exception ex)
        //            {
        //                AppLog.WriteInfo($"{btn.Tag.ToString()}按钮双击失败", "log日志", true);
        //                MessageBox.Show(ex.Message);
        //            }

        //        }
        //    }

        //}




    }
}
