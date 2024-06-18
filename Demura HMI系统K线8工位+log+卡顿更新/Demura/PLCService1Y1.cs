using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using thinger.DataConvertLib;

namespace Demura
{

    public class PLCService1Y1
    {
        public MelsecMcNet melsec_net = null;
        public HslCommunication.OperateResult connect = null;
        public CancellationTokenSource cts = null;//用于停止线程
        //private CancellationTokenSource cts1 = null;//用于停止线程
        private static readonly object objLock = new object();//创建锁，使得读写不能同时使用
        public ProDate CurrentState { get; set; } = new ProDate();//保存读取到的PLC数据
        public bool[] boolArray = null;

        static HashSet<int> generated = new HashSet<int>();
        static Random random = new Random();

        /// <summary>
        /// 在8000-8015内产生一个随机数
        /// </summary>
        /// <returns></returns>
        static int GenerateUniqueRandom()
        {
            int randomNumber;
            do
            {
                randomNumber = random.Next(8000, 8015);
            } while (generated.Contains(randomNumber));

            generated.Add(randomNumber);
            return randomNumber;
        }

        /// <summary>
        /// 在规定时间里循环连接plc
        /// </summary>
        public void Connection()
        {
            // 创建一个 Stopwatch 对象来计时
            Stopwatch stopwatch = new Stopwatch();

            // 设置目标时间（毫秒）
            int targetTimeMilliseconds = 6000; // 6秒 = 6000毫秒
                                               
            stopwatch.Start();// 开始计时
            while (true)
            {
                int uniqueRandom = GenerateUniqueRandom();
                melsec_net = new MelsecMcNet("192.168.66.6", uniqueRandom);
                connect = melsec_net.ConnectServer();
                if (connect.IsSuccess == true)
                {
                    //AppLog.WriteInfo("PLC连接成功", "log日志", true);
                    break;
                }
                // 检查经过的时间是否达到目标时间
                if (stopwatch.ElapsedMilliseconds >= targetTimeMilliseconds)
                {
                    AppLog.WriteInfo("PLC循环连接失败，检查网线或者PLC是否有故障", "log日志", true);
                    break;
                }
            }

            // 停止计时器
            stopwatch.Stop();
        }

        /// <summary>
        /// PLC登陆页面连接
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ConnectPLCEnter()
        {        
            try
            {
                //Connection();

                //开启线程
                this.cts = new CancellationTokenSource();
                Task.Run(() => { ReadToPLCEnter(); }, cts.Token);
            }
            catch (Exception ex)
            {
                //AppLog.WriteInfo("执行PLC登陆页面连接方法时发生异常", "log日志", true);
                throw new Exception("执行PLC连接方法时发生异常" + ex.Message);
            }

        }


        /// <summary>
        /// PLC手动页面连接
        /// </summary>
        /// <param name="m">界面传入过来的plc M值</param>
        /// <param name="d">界面传入过来的plc D值</param>
        /// <exception cref="Exception"></exception>
        public void ConnectPLC(string m, string d)
        {
            try
            {
                //Connection();
                //开启线程
                this.cts = new CancellationTokenSource();
                Task.Run(() => { ReadToPLC(m, d); }, cts.Token);
                //this.cts1 = new CancellationTokenSource();
                Task.Run(() => { ReadToPLC(); }, cts.Token);
            }
            catch (Exception ex)
            {
                //AppLog.WriteInfo($"执行PLC{m}单轴手动页面连接方法时发生异常", "log日志", true);
                throw new Exception("执行PLC连接方法时发生异常" + ex.Message);
            }
        }

        /// <summary>
        /// PLC自动报警页面连接
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ConnectPLCUnitOperation()
        {
            try
            {
                //Connection();
                //开启线程
                this.cts = new CancellationTokenSource();
                Task.Run(() => { ReadToPLCUnitOperation(); }, cts.Token);
            }
            catch (Exception ex)
            {
                //AppLog.WriteInfo("执行PLC自动报警页面连接方法时发生异常", "log日志", true);
                throw new Exception("执行PLC连接方法时发生异常" + ex.Message);
            }
        }

        /// <summary>
        /// PLC一键拍照页面连接
        /// </summary>
        /// <param name="Y1R">一工位Y1右轴</param>
        /// <param name="Y2R">二工位Y1右轴</param>
        /// <param name="TH1L">一工位TH1右轴</param>
        /// <param name="TH2L">二工位TH1右轴</param>
        /// <param name="i">规律算法号0 4 8 12</param>
        /// <exception cref="Exception"></exception>
        public void ConnectPLCImage12(int Y1R, int Y2R, int TH1L, int TH2L, int i)
        {
            try
            {
                //Connection();
                //开启线程
                this.cts = new CancellationTokenSource();
                Task.Run(() => { ReadToPLCImage12(Y1R, Y2R, TH1L, TH2L, i); }, cts.Token);
            }
            catch (Exception ex)
            {
                ///AppLog.WriteInfo("执行PLC一键拍照页面连接方法时发生异常", "log日志", true);
                throw new Exception("执行PLC连接方法时发生异常" + ex.Message);
            }
        }



        /// <summary>
        /// PLC断开连接
        /// </summary>
        public void DisConnectPLC()
        {
            //停止线程
            this.cts?.Cancel();

            //关闭连接
            melsec_net.Dispose();
            melsec_net.ConnectClose();

            //AppLog.WriteInfo("PLC断开连接", "log日志", true);
        }

        private void ReadToPLCEnter()
        {
            while (!this.cts.IsCancellationRequested)//请求取消此停止线程为true，没有取消为false
            {
                //读数据。。。
                lock (objLock)
                {
                    try
                    {
                        byte[] resultbool1 = melsec_net.Read("m0", 20).Content;

                        CurrentState.StartSFlag11 = BitLib.GetBitFromByteArray(resultbool1, 0, 0);//m0  启动停止标志
                        CurrentState.StartSFlag21 = BitLib.GetBitFromByteArray(resultbool1, 2, 4);//m20
                        CurrentState.StartSFlag31 = BitLib.GetBitFromByteArray(resultbool1, 5, 0);//m40
                        CurrentState.StartSFlag41 = BitLib.GetBitFromByteArray(resultbool1, 7, 4);//m60
                        CurrentState.StartSFlag51 = BitLib.GetBitFromByteArray(resultbool1, 10, 0);//m80
                        CurrentState.StartSFlag61 = BitLib.GetBitFromByteArray(resultbool1, 12, 4);//m100
                        CurrentState.StartSFlag71 = BitLib.GetBitFromByteArray(resultbool1, 15, 0);//m120
                        CurrentState.StartSFlag81 = BitLib.GetBitFromByteArray(resultbool1, 17, 4);//m140
                        CurrentState.StartSFlag91 = BitLib.GetBitFromByteArray(resultbool1, 20, 0);//m160
                        CurrentState.StartSFlag101 = BitLib.GetBitFromByteArray(resultbool1, 22, 4);//m180
                        //AppLog.WriteInfo("读取设备手自动状态成功", "log日志", true);
                    }
                    catch (Exception)
                    {
                        //AppLog.WriteInfo("读取设备手自动状态失败", "log日志", true);
                        break;
                    }
                }
            }
        }

        private void ReadToPLC(string m, string d)
        {
            while (!this.cts.IsCancellationRequested)//请求取消此停止线程为true，没有取消为false
            {
                //读数据。。。
                lock (objLock)
                {
                    try
                    {
                        //byte[] resultbool1 = melsec_net.Read("M390", 10).Content;
                        byte[] resultbool = melsec_net.Read(m, 10).Content;
                        //byte[] resultint1 = melsec_net.Read("D490", 10).Content;
                        byte[] resultint = melsec_net.Read(d, 100).Content;
                        //CurrentState.AxisName = name;
                        CurrentState.Busy = BitLib.GetBitFromByteArray(resultbool, 9, 0);//melsec_net.ReadBool("M12072").Content;//轴运行

                        CurrentState.CurrentPosition = BitConverter.ToInt32(resultint, 80);//当前位置D12040
                        CurrentState.CurrentMenulSpeed = (BitConverter.ToInt32(resultint, 88) * 0.001).ToString("f3");//当前手动速度D12044
                        CurrentState.CurrentAutoSpeed = (BitConverter.ToInt32(resultint, 84) * 0.001).ToString("f3");//当前自动速度D12042
                        CurrentState.RunSpeed = BitConverter.ToInt32(resultint, 112).ToString("f3");//转速D12056
                        CurrentState.Load = BitConverter.ToInt32(resultint, 116).ToString("f3");//负载D12058

                        CurrentState.SetMenulSpeed = (BitConverter.ToInt32(resultint, 88) * 0.001).ToString("f3");//设定手动速度D12044
                        CurrentState.SetAutoSpeed = (BitConverter.ToInt32(resultint, 84) * 0.001).ToString("f3");//设定自动速度D12042
                        CurrentState.SetAccTime = (BitConverter.ToInt32(resultint, 156) * 0.001).ToString("f3");//设定加速度D12078
                        CurrentState.SetDecTime = (BitConverter.ToInt32(resultint, 160) * 0.001).ToString("f3");//设定减速度D12080

                        CurrentState.AwaitPosition = (BitConverter.ToInt32(resultint, 0) * 0.001).ToString("f3");//待机位D12000
                        CurrentState.PhotoPosition = (BitConverter.ToInt32(resultint, 8) * 0.001).ToString("f3");//拍照位D12004
                        CurrentState.NeedlePosition = (BitConverter.ToInt32(resultint, 12) * 0.001).ToString("f3");//扎针位D12006
                        CurrentState.NeedleAddPosition = (BitConverter.ToInt32(resultint, 16) * 0.001).ToString("f3");//扎针补正位D12008

                        CurrentState.TakeAwaitPosition = BitLib.GetBitFromByteArray(resultbool, 0, 0);//定位待机位M12000
                        CurrentState.TakePhotoPosition = BitLib.GetBitFromByteArray(resultbool, 0, 2);//定位拍照位M12002
                        CurrentState.TakeNeedlePosition = BitLib.GetBitFromByteArray(resultbool, 0, 3);//定位扎针位M12003
                        CurrentState.TakeNeedleAddPosition = BitLib.GetBitFromByteArray(resultbool, 0, 4);//定位扎针补正位M12004

                        CurrentState.AwaitPFinish = BitLib.GetBitFromByteArray(resultbool, 2, 4);//待机位到位M12020
                        CurrentState.PhotoPFinish = BitLib.GetBitFromByteArray(resultbool, 2, 6);//拍照位到位M12022
                        CurrentState.NeedlePFinish = BitLib.GetBitFromByteArray(resultbool, 2, 7);//扎针位到位M12023
                        CurrentState.NeedleADDFinish = BitLib.GetBitFromByteArray(resultbool, 3, 0);//扎针补正位到位M12024

                        CurrentState.LimitLeft = BitLib.GetBitFromByteArray(resultbool, 5, 0);//负极限M12040
                        CurrentState.LimitRight = BitLib.GetBitFromByteArray(resultbool, 5, 1);//正极限M12041
                        CurrentState.LimitOriginal = BitLib.GetBitFromByteArray(resultbool, 5, 6);//原点M12046
                        CurrentState.OriginLocation = BitLib.GetBitFromByteArray(resultbool, 9, 3);//回原M12075
                        CurrentState.LeftJOG = BitLib.GetBitFromByteArray(resultbool, 9, 5);//负点动M12077
                        CurrentState.RightJOG = BitLib.GetBitFromByteArray(resultbool, 9, 4);//正点动M12076
                        CurrentState.STOP = BitLib.GetBitFromByteArray(resultbool, 9, 2);//停止M12074
                        CurrentState.JOGControl = melsec_net.ReadBool("M392").Content;//寸动控制M392
                        //CurrentState.JOGDistance = BitConverter.ToInt32(resultint1, 16);//寸动距离D498
                        //AppLog.WriteInfo($"读取设备单轴{m}{d}值成功", "log日志", true);
                    }
                    catch (Exception)
                    {
                        //AppLog.WriteInfo($"读取设备单轴{m}{d}值失败", "log日志", true);
                        break;
                    }
                }
            }
        }
        private void ReadToPLC()
        {
            while (!this.cts.IsCancellationRequested)
            {
                lock (objLock)
                {
                    try
                    {
                        byte[] resultint1 = melsec_net.Read("D490", 10).Content;
                        CurrentState.JOGDistance = BitConverter.ToInt32(resultint1, 16);//寸动距离D498
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }
        }
        private void ReadToPLCUnitOperation()
        {
            while (!this.cts.IsCancellationRequested)//请求取消此停止线程为true，没有取消为false
            {
                //读数据。。。
                lock (objLock)
                {
                    try
                    {

                        //boolArray = melsec_net.ReadBool("F0", 3520).Content;              

                        byte[] resultbool = melsec_net.Read("m0", 20).Content;
                        CurrentState.AutoStart1 = BitLib.GetBitFromByteArray(resultbool, 1, 2);//m10
                        CurrentState.AutoStop1 = BitLib.GetBitFromByteArray(resultbool, 1, 3);//m11
                        CurrentState.Auto1 = BitLib.GetBitFromByteArray(resultbool, 1, 4);//m12
                        CurrentState.Menul1 = BitLib.GetBitFromByteArray(resultbool, 1, 5);//m13
                        CurrentState.Reset1 = BitLib.GetBitFromByteArray(resultbool, 1, 6);//m14
                        CurrentState.StartSFlag1 = BitLib.GetBitFromByteArray(resultbool, 0, 0);//m0
                        CurrentState.AutoMFlag1 = BitLib.GetBitFromByteArray(resultbool, 0, 1);//m1

                        CurrentState.AutoStart2 = BitLib.GetBitFromByteArray(resultbool, 3, 6);//m30
                        CurrentState.AutoStop2 = BitLib.GetBitFromByteArray(resultbool, 3, 7);
                        CurrentState.Auto2 = BitLib.GetBitFromByteArray(resultbool, 4, 0);
                        CurrentState.Menul2 = BitLib.GetBitFromByteArray(resultbool, 4, 1);
                        CurrentState.Reset2 = BitLib.GetBitFromByteArray(resultbool, 4, 2);
                        CurrentState.StartSFlag2 = BitLib.GetBitFromByteArray(resultbool, 2, 4);//m20
                        CurrentState.AutoMFlag2 = BitLib.GetBitFromByteArray(resultbool, 2, 5);

                        CurrentState.AutoStart3 = BitLib.GetBitFromByteArray(resultbool, 6, 2);//m50
                        CurrentState.AutoStop3 = BitLib.GetBitFromByteArray(resultbool, 6, 3);
                        CurrentState.Auto3 = BitLib.GetBitFromByteArray(resultbool, 6, 4);
                        CurrentState.Menul3 = BitLib.GetBitFromByteArray(resultbool, 6, 5);
                        CurrentState.Reset3 = BitLib.GetBitFromByteArray(resultbool, 6, 6);
                        CurrentState.StartSFlag3 = BitLib.GetBitFromByteArray(resultbool, 5, 0);//m40
                        CurrentState.AutoMFlag3 = BitLib.GetBitFromByteArray(resultbool, 5, 1);

                        CurrentState.AutoStart4 = BitLib.GetBitFromByteArray(resultbool, 8, 6);//m70
                        CurrentState.AutoStop4 = BitLib.GetBitFromByteArray(resultbool, 8, 7);
                        CurrentState.Auto4 = BitLib.GetBitFromByteArray(resultbool, 9, 0);
                        CurrentState.Menul4 = BitLib.GetBitFromByteArray(resultbool, 9, 1);
                        CurrentState.Reset4 = BitLib.GetBitFromByteArray(resultbool, 9, 2);
                        CurrentState.StartSFlag4 = BitLib.GetBitFromByteArray(resultbool, 7, 4);//m60
                        CurrentState.AutoMFlag4 = BitLib.GetBitFromByteArray(resultbool, 7, 5);

                        CurrentState.AutoStart5 = BitLib.GetBitFromByteArray(resultbool, 11, 2);//m90
                        CurrentState.AutoStop5 = BitLib.GetBitFromByteArray(resultbool, 11, 3);
                        CurrentState.Auto5 = BitLib.GetBitFromByteArray(resultbool, 11, 4);
                        CurrentState.Menul5 = BitLib.GetBitFromByteArray(resultbool, 11, 5);
                        CurrentState.Reset5 = BitLib.GetBitFromByteArray(resultbool, 11, 6);
                        CurrentState.StartSFlag5 = BitLib.GetBitFromByteArray(resultbool, 10, 0);//m80
                        CurrentState.AutoMFlag5 = BitLib.GetBitFromByteArray(resultbool, 10, 1);

                        CurrentState.AutoStart6 = BitLib.GetBitFromByteArray(resultbool, 13, 6);//m110
                        CurrentState.AutoStop6 = BitLib.GetBitFromByteArray(resultbool, 13, 7);
                        CurrentState.Auto6 = BitLib.GetBitFromByteArray(resultbool, 14, 0);
                        CurrentState.Menul6 = BitLib.GetBitFromByteArray(resultbool, 14, 1);
                        CurrentState.Reset6 = BitLib.GetBitFromByteArray(resultbool, 14, 2);
                        CurrentState.StartSFlag6 = BitLib.GetBitFromByteArray(resultbool, 12, 4);//m100
                        CurrentState.AutoMFlag6 = BitLib.GetBitFromByteArray(resultbool, 12, 5);

                        CurrentState.AutoStart7 = BitLib.GetBitFromByteArray(resultbool, 16, 2);//m130
                        CurrentState.AutoStop7 = BitLib.GetBitFromByteArray(resultbool, 16, 3);
                        CurrentState.Auto7 = BitLib.GetBitFromByteArray(resultbool, 16, 4);
                        CurrentState.Menul7 = BitLib.GetBitFromByteArray(resultbool, 16, 5);
                        CurrentState.Reset7 = BitLib.GetBitFromByteArray(resultbool, 16, 6);
                        CurrentState.StartSFlag7 = BitLib.GetBitFromByteArray(resultbool, 15, 0);//m120
                        CurrentState.AutoMFlag7 = BitLib.GetBitFromByteArray(resultbool, 15, 1);

                        CurrentState.AutoStart8 = BitLib.GetBitFromByteArray(resultbool, 18, 6);//m150
                        CurrentState.AutoStop8 = BitLib.GetBitFromByteArray(resultbool, 18, 7);
                        CurrentState.Auto8 = BitLib.GetBitFromByteArray(resultbool, 19, 0);
                        CurrentState.Menul8 = BitLib.GetBitFromByteArray(resultbool, 19, 1);
                        CurrentState.Reset8 = BitLib.GetBitFromByteArray(resultbool, 19, 2);
                        CurrentState.StartSFlag8 = BitLib.GetBitFromByteArray(resultbool, 17, 4);//m140
                        CurrentState.AutoMFlag8 = BitLib.GetBitFromByteArray(resultbool, 17, 5);
                        //AppLog.WriteInfo("读取设备手自动启动、自动停止、手自动、复位、开始标志位、自动标志位成功", "log日志", true);
                    }
                    catch (Exception)
                    {
                        //AppLog.WriteInfo("读取设备手自动启动、自动停止、手自动、复位、开始标志位、自动标志位失败", "log日志", true);
                        break;
                    }
                }
            }
        }

        private void ReadToPLCImage12(int Y1R, int Y2R, int TH1L, int TH2L, int i)
        {
            while (!this.cts.IsCancellationRequested)//请求取消此停止线程为true，没有取消为false
            {
                //读数据。。。
                lock (objLock)
                {
                    try
                    {
                        bool[] resultbool = melsec_net.ReadBool("m800", 100).Content;
                        byte[] resultint = melsec_net.Read("d" + Y1R.ToString(), 7500).Content;

                        CurrentState.MarkStop = resultbool[0];//流程停止
                        CurrentState.MenulPhoto1R = resultbool[20 + i];//一工位手动拍照位右
                        CurrentState.MenulPhoto1L = resultbool[21 + i];//一工位手动拍照位左
                        CurrentState.MemulPhoto2R = resultbool[22 + i];//二工位手动拍照位右
                        CurrentState.MemulPhoto2L = resultbool[23 + i];//二工位手动拍照位左

                        CurrentState.MarkPosition1R = resultbool[40 + 2 * i];//一工位一键标定位右
                        CurrentState.MarkPosition1L = resultbool[41 + 2 * i];
                        CurrentState.TakePicturePosition1R = resultbool[42 + 2 * i];//一工位一键拍照位右
                        CurrentState.TakePicturePosition1L = resultbool[43 + 2 * i];

                        CurrentState.MarkPosition2R = resultbool[44 + 2 * i];//二工位一键标定位右
                        CurrentState.MarkPosition2L = resultbool[45 + 2 * i];
                        CurrentState.TakePicturePosition2R = resultbool[46 + 2 * i];//二工位一键拍照位右
                        CurrentState.TakePicturePosition2L = resultbool[47 + 2 * i];

                        CurrentState.CurrentPositionY1R = (BitConverter.ToInt32(resultint, 80) * 0.001).ToString("f3");//一工位右Y当前位置D12040
                        CurrentState.PhotoPositionY1R = (BitConverter.ToInt32(resultint, 8) * 0.001).ToString("f3");//拍照位D12004
                        CurrentState.NeedlePositionY1R = (BitConverter.ToInt32(resultint, 12) * 0.001).ToString("f3");//扎针位D12006
                        CurrentState.NeedleAddPositionY1R = (BitConverter.ToInt32(resultint, 16) * 0.001).ToString("f3");//扎针补正位D12008

                        CurrentState.CurrentPositionY1L = (BitConverter.ToInt32(resultint, 280) * 0.001).ToString("f3");//一工位左Y当前位置D12140 280
                        CurrentState.PhotoPositionY1L = (BitConverter.ToInt32(resultint, 208) * 0.001).ToString("f3");//拍照位D12104
                        CurrentState.NeedlePositionY1L = (BitConverter.ToInt32(resultint, 212) * 0.001).ToString("f3");//扎针位D12106
                        CurrentState.NeedleAddPositionY1L = (BitConverter.ToInt32(resultint, 216) * 0.001).ToString("f3");//扎针补正位D12108

                        CurrentState.CurrentPositionθ1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 40) * 2) * 0.001).ToString("f3");//一工位右θ当前位置D13640 3280
                        CurrentState.PhotoPositionθ1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 4) * 2) * 0.001).ToString("f3");//拍照位D13604 3208
                        CurrentState.NeedlePositionθ1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 6) * 2) * 0.001).ToString("f3");//扎针位D13606 3212
                        CurrentState.NeedleAddPositionθ1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 8) * 2) * 0.001).ToString("f3");//扎针补正位D13608 3216

                        CurrentState.CurrentPositionθ1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 140) * 2) * 0.001).ToString("f3");//一工位左θ当前位置D13740 3480
                        CurrentState.PhotoPositionθ1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 104) * 2) * 0.001).ToString("f3");//拍照位D13704
                        CurrentState.NeedlePositionθ1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 106) * 2) * 0.001).ToString("f3");//扎针位D13706
                        CurrentState.NeedleAddPositionθ1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 108) * 2) * 0.001).ToString("f3");//扎针补正位D13708

                        CurrentState.CurrentPositionX1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 240) * 2) * 0.001).ToString("f3");//一工位右X当前位置D13840 3680
                        CurrentState.PhotoPositionX1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 204) * 2) * 0.001).ToString("f3");//拍照位D13804
                        CurrentState.NeedlePositionX1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 206) * 2) * 0.001).ToString("f3");//扎针位D13806
                        CurrentState.NeedleAddPositionX1R = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 208) * 2) * 0.001).ToString("f3");//扎针补正位D13808

                        CurrentState.CurrentPositionX1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 340) * 2) * 0.001).ToString("f3");//一工位左X当前位置D13940 3880
                        CurrentState.PhotoPositionX1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 304) * 2) * 0.001).ToString("f3");//拍照位D13904
                        CurrentState.NeedlePositionX1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 306) * 2) * 0.001).ToString("f3");//扎针位D13906
                        CurrentState.NeedleAddPositionX1L = (BitConverter.ToInt32(resultint, (TH1L - Y1R + 308) * 2) * 0.001).ToString("f3");//扎针补正位D13908


                        CurrentState.CurrentPositionY2R = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 40) * 2) * 0.001).ToString("f3");//二工位右Y当前位置D12440 880 
                        CurrentState.PhotoPositionY2R = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 4) * 2) * 0.001).ToString("f3");//拍照位D12404 808
                        CurrentState.NeedlePositionY2R = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 6) * 2) * 0.001).ToString("f3");//扎针位D12406 812
                        CurrentState.NeedleAddPositionY2R = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 8) * 2) * 0.001).ToString("f3");//扎针补正位D12408 816

                        CurrentState.CurrentPositionY2L = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 140) * 2) * 0.001).ToString("f3");//二工位左Y当前位置D12540 1080
                        CurrentState.PhotoPositionY2L = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 104) * 2) * 0.001).ToString("f3");//拍照位D12504
                        CurrentState.NeedlePositionY2L = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 106) * 2) * 0.001).ToString("f3");//扎针位D12506
                        CurrentState.NeedleAddPositionY2L = (BitConverter.ToInt32(resultint, (Y2R - Y1R + 108) * 2) * 0.001).ToString("f3");//扎针补正位D12508

                        CurrentState.CurrentPositionθ2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 40) * 2) * 0.001).ToString("f3");//二工位右θ当前位置D15240  6480
                        CurrentState.PhotoPositionθ2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 4) * 2) * 0.001).ToString("f3");//拍照位D15204
                        CurrentState.NeedlePositionθ2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 6) * 2) * 0.001).ToString("f3");//扎针位D15206
                        CurrentState.NeedleAddPositionθ2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 8) * 2) * 0.001).ToString("f3");//扎针补正位D15208

                        CurrentState.CurrentPositionθ2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 140) * 2) * 0.001).ToString("f3");//二工位左θ当前位置D15340  6680
                        CurrentState.PhotoPositionθ2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 104) * 2) * 0.001).ToString("f3");//拍照位D15304
                        CurrentState.NeedlePositionθ2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 106) * 2) * 0.001).ToString("f3");//扎针位D15306
                        CurrentState.NeedleAddPositionθ2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 108) * 2) * 0.001).ToString("f3");//扎针补正位D15308

                        CurrentState.CurrentPositionX2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 240) * 2) * 0.001).ToString("f3");//二工位右X当前位置D15440 6880
                        CurrentState.PhotoPositionX2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 204) * 2) * 0.001).ToString("f3");//拍照位D15404
                        CurrentState.NeedlePositionX2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 206) * 2) * 0.001).ToString("f3");//扎针位D15406
                        CurrentState.NeedleAddPositionX2R = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 208) * 2) * 0.001).ToString("f3");//扎针补正位D15408

                        CurrentState.CurrentPositionX2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 340) * 2) * 0.001).ToString("f3");//二工位左X当前位置D15540  7080
                        CurrentState.PhotoPositionX2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 304) * 2) * 0.001).ToString("f3");//拍照位D15504
                        CurrentState.NeedlePositionX2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 306) * 2) * 0.001).ToString("f3");//扎针位D15506
                        CurrentState.NeedleAddPositionX2L = (BitConverter.ToInt32(resultint, (TH2L - Y1R + 308) * 2) * 0.001).ToString("f3");//扎针补正位D15508
                        //AppLog.WriteInfo($"读取设备手动拍照位、一键拍照位、标定位成功{Y1R}、{Y2R}、{TH1L}、{TH2L}、{i}", "log日志", true);
                    }
                    catch (Exception)
                    {
                        //AppLog.WriteInfo($"读取设备手动拍照位、一键拍照位、标定位失败{Y1R}、{Y2R}、{TH1L}、{TH2L}、{i}", "log日志", true);
                        break;//throw new Exception("plc连接异常" + ex.Message);
                    }
                }
            }
        }




        /// <summary>
        /// bool量的写入
        /// </summary>
        /// <param name="varAddress">变量地址</param>
        public void ExcuteTask1(string varAddress)
        {
            lock (objLock)
            {
                try
                {
                    melsec_net.Write(varAddress, true);
                    Thread.Sleep(1000);
                    melsec_net.Write(varAddress, false);
                }
                catch (Exception ex)
                {
                    AppLog.WriteInfo("写入自复位bool量异常", "log日志", true);
                    throw new Exception("bool量写入异常" + ex.Message);
                }
            }
        }

        public void ExcuteTask2(string varAddress)
        {
            lock (objLock)
            {
                try
                {
                    melsec_net.Write(varAddress, true);
                }
                catch (Exception ex)
                {
                    AppLog.WriteInfo("写入True bool量异常", "log日志", true);
                    throw new Exception("bool量写入异常" + ex.Message);
                }
            }
        }

        public void ExcuteTask3(string varAddress)
        {
            lock (objLock)
            {
                try
                {
                    melsec_net.Write(varAddress, false);
                }
                catch (Exception ex)
                {
                    AppLog.WriteInfo("写入False bool量异常", "log日志", true);
                    throw new Exception("bool量写入异常" + ex.Message);
                }
            }
        }

        public void WriteInt(string varAddress, int number)
        {
            //lock (objLock)
            //{
                try
                {
                    melsec_net.Write(varAddress, number);
                }
                catch (Exception ex)
                {
                    AppLog.WriteInfo("写入D值量异常", "log日志", true);
                    throw new Exception("双字写入异常" + ex.Message);
                }
            //}
        }
    }
}
