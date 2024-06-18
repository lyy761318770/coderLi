using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Demura
{
    public partial class FrmUnitOperation : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();//创建一个页面更新的定时器
        //private System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
        private bool temp = false;
        private object objLock = new object();//创建锁，使得读写不能同时使用
        private PLCService1Y1 plcServiceCurrent = null;
        DataTable dtDisplay;
        public FrmUnitOperation(PLCService1Y1 plcService)
        {
            InitializeComponent();
            plcServiceCurrent=plcService;
            plcService.ConnectPLCUnitOperation();
            this.timer.Interval = 300;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();//定时器开始
            //this.timer1.Interval = 1500;
            //this.timer1.Tick += Timer_Tick1;
            //this.timer1.Start();//定时器开始
            if (plcService.connect.IsSuccess)
            {
                tsl_ConnectState.Text = "连接成功";
                tsl_ConnectState.ForeColor = Color.Green;
                AppLog.WriteInfo("连接PLC自动报警界面成功", "log日志", true);
            }
            else
            {
                tsl_ConnectState.Text = "连接失败";
                tsl_ConnectState.ForeColor = Color.Red;
                AppLog.WriteInfo("连接PLC自动报警界面失败", "log日志", true);
            }

            //实例化一个xmlDoucument()的对象
            XmlDocument xDoc = new XmlDocument();
            //加载根目录这个路径的xml
            xDoc.Load("PLC数据.xml");
            //获取根节点PLCDate，取跟节点下的第一个
            XmlNode node = xDoc.SelectSingleNode("PLCDate");
            XmlNodeList nodeList = node.ChildNodes;

            btn_AutoStart1.Tag = ((XmlElement)nodeList.Item(0))["btn_AutoStart"].InnerText;
            btn_AutoStop1.Tag = ((XmlElement)nodeList.Item(0))["btn_AutoStop"].InnerText;
            btn_Auto1.Tag = ((XmlElement)nodeList.Item(0))["btn_Auto"].InnerText;
            btn_Menul1.Tag = ((XmlElement)nodeList.Item(0))["btn_Menul"].InnerText;
            btn_Reset1.Tag = ((XmlElement)nodeList.Item(0))["btn_Reset"].InnerText;

            btn_AutoStart2.Tag = ((XmlElement)nodeList.Item(8))["btn_AutoStart"].InnerText;
            btn_AutoStop2.Tag = ((XmlElement)nodeList.Item(8))["btn_AutoStop"].InnerText;
            btn_Auto2.Tag = ((XmlElement)nodeList.Item(8))["btn_Auto"].InnerText;
            btn_Menul2.Tag = ((XmlElement)nodeList.Item(8))["btn_Menul"].InnerText;
            btn_Reset2.Tag = ((XmlElement)nodeList.Item(8))["btn_Reset"].InnerText;

            btn_AutoStart3.Tag = ((XmlElement)nodeList.Item(16))["btn_AutoStart"].InnerText;
            btn_AutoStop3.Tag = ((XmlElement)nodeList.Item(16))["btn_AutoStop"].InnerText;
            btn_Auto3.Tag = ((XmlElement)nodeList.Item(16))["btn_Auto"].InnerText;
            btn_Menul3.Tag = ((XmlElement)nodeList.Item(16))["btn_Menul"].InnerText;
            btn_Reset3.Tag = ((XmlElement)nodeList.Item(16))["btn_Reset"].InnerText;

            btn_AutoStart4.Tag = ((XmlElement)nodeList.Item(24))["btn_AutoStart"].InnerText;
            btn_AutoStop4.Tag = ((XmlElement)nodeList.Item(24))["btn_AutoStop"].InnerText;
            btn_Auto4.Tag = ((XmlElement)nodeList.Item(24))["btn_Auto"].InnerText;
            btn_Menul4.Tag = ((XmlElement)nodeList.Item(24))["btn_Menul"].InnerText;
            btn_Reset4.Tag = ((XmlElement)nodeList.Item(24))["btn_Reset"].InnerText;

            btn_AutoStart5.Tag = ((XmlElement)nodeList.Item(32))["btn_AutoStart"].InnerText;
            btn_AutoStop5.Tag = ((XmlElement)nodeList.Item(32))["btn_AutoStop"].InnerText;
            btn_Auto5.Tag = ((XmlElement)nodeList.Item(32))["btn_Auto"].InnerText;
            btn_Menul5.Tag = ((XmlElement)nodeList.Item(32))["btn_Menul"].InnerText;
            btn_Reset5.Tag = ((XmlElement)nodeList.Item(32))["btn_Reset"].InnerText;

            btn_AutoStart6.Tag = ((XmlElement)nodeList.Item(40))["btn_AutoStart"].InnerText;
            btn_AutoStop6.Tag = ((XmlElement)nodeList.Item(40))["btn_AutoStop"].InnerText;
            btn_Auto6.Tag = ((XmlElement)nodeList.Item(40))["btn_Auto"].InnerText;
            btn_Menul6.Tag = ((XmlElement)nodeList.Item(40))["btn_Menul"].InnerText;
            btn_Reset6.Tag = ((XmlElement)nodeList.Item(40))["btn_Reset"].InnerText;

            btn_AutoStart7.Tag = ((XmlElement)nodeList.Item(48))["btn_AutoStart"].InnerText;
            btn_AutoStop7.Tag = ((XmlElement)nodeList.Item(48))["btn_AutoStop"].InnerText;
            btn_Auto7.Tag = ((XmlElement)nodeList.Item(48))["btn_Auto"].InnerText;
            btn_Menul7.Tag = ((XmlElement)nodeList.Item(48))["btn_Menul"].InnerText;
            btn_Reset7.Tag = ((XmlElement)nodeList.Item(48))["btn_Reset"].InnerText;

            btn_AutoStart8.Tag = ((XmlElement)nodeList.Item(56))["btn_AutoStart"].InnerText;
            btn_AutoStop8.Tag = ((XmlElement)nodeList.Item(56))["btn_AutoStop"].InnerText;
            btn_Auto8.Tag = ((XmlElement)nodeList.Item(56))["btn_Auto"].InnerText;
            btn_Menul8.Tag = ((XmlElement)nodeList.Item(56))["btn_Menul"].InnerText;
            btn_Reset8.Tag = ((XmlElement)nodeList.Item(56))["btn_Reset"].InnerText;

            btn_removealarm.Tag = ((XmlElement)nodeList.Item(0))["btn_removealarm"].InnerText;

            
            //dt.Columns.Add("报警类型");
            //dt.Columns.Add("报警状态");
            //dt.Columns.Add("报警产生时间");
            //dt.Columns.Add("报警恢复时间");
        }

        //private void Timer_Tick1(object sender, EventArgs e)//在这里刷新报警界面状态
        //{           
            //AdjustDataTable();
            //Control.CheckForIllegalCrossThreadCalls = false;
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    if (dataGridView1.Rows[i].Cells[1].Value.ToString() == "已产生")
            //    {
            //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
            //    }
            //    else if (dataGridView1.Rows[i].Cells[1].Value.ToString() == "已恢复")
            //    {
            //        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
            //    }
            //}
        //}

        private void Timer_Tick(object sender, EventArgs e)//在这里刷新界面状态
        {

            LoadLogFile($"SysLog//AppLog_Info_log日志_{DateTime.Now.ToString("yyyyMMdd")}.log");
            
            //自动启动按钮与自动停止按钮显示工位1
            if (!plcServiceCurrent.CurrentState.StartSFlag1)
            {
                btn_AutoStart1.BackColor = Color.Transparent;
                btn_AutoStop1.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart1.BackColor = Color.Lime;
                btn_AutoStop1.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位1
            if (!plcServiceCurrent.CurrentState.AutoMFlag1)
            {
                btn_Auto1.BackColor = Color.Transparent;
                btn_Menul1.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto1.BackColor = Color.Lime;
                btn_Menul1.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位2
            if (!plcServiceCurrent.CurrentState.StartSFlag2)
            {
                btn_AutoStart2.BackColor = Color.Transparent;
                btn_AutoStop2.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart2.BackColor = Color.Lime;
                btn_AutoStop2.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位2
            if (!plcServiceCurrent.CurrentState.AutoMFlag2)
            {
                btn_Auto2.BackColor = Color.Transparent;
                btn_Menul2.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto2.BackColor = Color.Lime;
                btn_Menul2.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位3
            if (!plcServiceCurrent.CurrentState.StartSFlag3)
            {
                btn_AutoStart3.BackColor = Color.Transparent;
                btn_AutoStop3.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart3.BackColor = Color.Lime;
                btn_AutoStop3.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位3
            if (!plcServiceCurrent.CurrentState.AutoMFlag3)
            {
                btn_Auto3.BackColor = Color.Transparent;
                btn_Menul3.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto3.BackColor = Color.Lime;
                btn_Menul3.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位4
            if (!plcServiceCurrent.CurrentState.StartSFlag4)
            {
                btn_AutoStart4.BackColor = Color.Transparent;
                btn_AutoStop4.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart4.BackColor = Color.Lime;
                btn_AutoStop4.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位4
            if (!plcServiceCurrent.CurrentState.AutoMFlag4)
            {
                btn_Auto4.BackColor = Color.Transparent;
                btn_Menul4.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto4.BackColor = Color.Lime;
                btn_Menul4.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位5
            if (!plcServiceCurrent.CurrentState.StartSFlag5)
            {
                btn_AutoStart5.BackColor = Color.Transparent;
                btn_AutoStop5.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart5.BackColor = Color.Lime;
                btn_AutoStop5.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位5
            if (!plcServiceCurrent.CurrentState.AutoMFlag5)
            {
                btn_Auto5.BackColor = Color.Transparent;
                btn_Menul5.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto5.BackColor = Color.Lime;
                btn_Menul5.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位6
            if (!plcServiceCurrent.CurrentState.StartSFlag6)
            {
                btn_AutoStart6.BackColor = Color.Transparent;
                btn_AutoStop6.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart6.BackColor = Color.Lime;
                btn_AutoStop6.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位6
            if (!plcServiceCurrent.CurrentState.AutoMFlag6)
            {
                btn_Auto6.BackColor = Color.Transparent;
                btn_Menul6.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto6.BackColor = Color.Lime;
                btn_Menul6.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位7
            if (!plcServiceCurrent.CurrentState.StartSFlag7)
            {
                btn_AutoStart7.BackColor = Color.Transparent;
                btn_AutoStop7.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart7.BackColor = Color.Lime;
                btn_AutoStop7.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位7
            if (!plcServiceCurrent.CurrentState.AutoMFlag7)
            {
                btn_Auto7.BackColor = Color.Transparent;
                btn_Menul7.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto7.BackColor = Color.Lime;
                btn_Menul7.BackColor = Color.Transparent;
            }

            //自动启动按钮与自动停止按钮显示工位8
            if (!plcServiceCurrent.CurrentState.StartSFlag8)
            {
                btn_AutoStart8.BackColor = Color.Transparent;
                btn_AutoStop8.BackColor = Color.Red;
            }
            else
            {
                btn_AutoStart8.BackColor = Color.Lime;
                btn_AutoStop8.BackColor = Color.Transparent;
            }
            //自动按钮与手动按钮显示工位8
            if (!plcServiceCurrent.CurrentState.AutoMFlag8)
            {
                btn_Auto8.BackColor = Color.Transparent;
                btn_Menul8.BackColor = Color.Yellow;
            }
            else
            {
                btn_Auto8.BackColor = Color.Lime;
                btn_Menul8.BackColor = Color.Transparent;
            }
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

        private void FrmUnitOperation_FormClosing(object sender, FormClosingEventArgs e)
        {
            //plcServiceCurrent.DisConnectPLC();
            plcServiceCurrent.cts?.Cancel();
            timer.Stop();
            //timer1.Stop();
            AppLog.WriteInfo("自动报警页面窗体关闭", "log日志", true);
        }

        //void AdjustDataTable()
        //{
        //    try
        //    {
        //        //实例化一个xmlDoucument()的对象
        //        XmlDocument xDoc = new XmlDocument();
        //        //加载根目录这个路径的xml
        //        xDoc.Load("报警信息.xml");
        //        //获取根节点PLCDate，取跟节点下的第一个
        //        XmlNode node = xDoc.SelectSingleNode("PLCAlarm");
        //        XmlNodeList nodeList = node.ChildNodes;
        //        for (int i = 0; i < plcService.boolArray.Length; i++)
        //        {
        //            if (plcService.boolArray[i] == true)
        //            {
        //                for (int j = 0; j < dt.Rows.Count; j++)
        //                {
        //                    //判断当前报警表格里是否有该报警，如果没有就添加，有就跳出循环，判断下一个报警类型
        //                    if (dt.Rows[j]["报警类型"].ToString() == ((XmlElement)nodeList.Item(i)).GetAttribute("AlarmName") && dt.Rows[j]["报警状态"].ToString() == "已产生")
        //                    {
        //                        goto LabelA;
        //                    }
        //                }
        //                dt.Rows.Add(((XmlElement)nodeList.Item(i)).GetAttribute("AlarmName"), "已产生", DateTime.Now.ToString(), "");//"报警类型" + i.ToString()
        //            }
        //            else if (plcService.boolArray[i] == false)
        //            {
        //                for (int j = 0; j < dt.Rows.Count; j++)
        //                {
        //                    //判断当前报警表格了是否有该报警恢复，如果有该报警类型同时报警状态是已产生，那么改成已恢复，报警恢复时间为当前时间
        //                    if (dt.Rows[j]["报警类型"].ToString() == ((XmlElement)nodeList.Item(i)).GetAttribute("AlarmName") && dt.Rows[j]["报警状态"].ToString() == "已产生")
        //                    {
        //                        dt.Rows[j]["报警状态"] = "已恢复";
        //                        dt.Rows[j]["报警恢复时间"] = DateTime.Now.ToString();
        //                    }
        //                }
        //            }
        //        LabelA: continue;
        //        }
        //        this.Invoke(new Action(() => { dataGridView1.DataSource = dt; }));
        //    }
        //    catch (Exception)
        //    {
        //        AppLog.WriteInfo("报警读取失败", "log日志", true);
        //        //MessageBox.Show(ex.Message);
        //    }
        //}


        //private void MainForm_Load(object sender, EventArgs e)
        //{
        //    // 载入日志文件内容到TextBox
        //    LoadLogFile($"AppLog_Info_log日志_{DateTime.Now.ToString("yyyyMMdd")}.log"); 

        //    // 每次有新日志内容时，滚动到最后一行
        //    textBoxLog.TextChanged += TextBoxLog_TextChanged;
        //}

        private void LoadLogFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath, System.Text.Encoding.GetEncoding("gb2312"));
                textBoxLog.Lines = lines;
            }
            else
            {
                MessageBox.Show("日志文件不存在。");
            }
            //每次有新日志内容时，滚动到最后一行
                textBoxLog.TextChanged += TextBoxLog_TextChanged;
        }

        private void TextBoxLog_TextChanged(object sender, EventArgs e)
        {
            // 滚动到TextBox的最后一行
            textBoxLog.SelectionStart = textBoxLog.Text.Length;
            textBoxLog.ScrollToCaret();
        }




    }
}
