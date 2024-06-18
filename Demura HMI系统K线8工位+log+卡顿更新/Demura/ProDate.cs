using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demura
{
    public class ProDate
    {
        public string AxisName { get; set; }
        public bool Busy { get; set; }//轴运动状态
        public int CurrentPosition { get; set; }//当前位置      
        public string CurrentMenulSpeed { get; set; }//当前手动速度
        public string CurrentAutoSpeed { get; set; }//手动自动速度
        public string RunSpeed { get; set; }//转速
        public string Load { get; set; }//负载
        public string SetMenulSpeed { get; set; }//设定手动速度
        public string SetAutoSpeed { get; set; }//设定自动速度
        public string SetAccTime { get; set; }//设定加速时间
        public string SetDecTime { get; set; }//设定减速时间
        public string AwaitPosition { get; set; }//待机位位置
        public string PhotoPosition { get; set; }//拍照位位置
        public string NeedlePosition { get; set; }//扎针位位置
        public string NeedleAddPosition { get; set; }//扎针补正位位置
        public bool TakeAwaitPosition { get; set; }//待机位定位
        public bool TakePhotoPosition { get; set; }//拍照位定位
        public bool TakeNeedlePosition { get; set; }//扎针位定位
        public bool TakeNeedleAddPosition { get; set; }//扎针补正位定位
        public bool AwaitPFinish { get; set; }//待机位到位
        public bool PhotoPFinish { get; set; }//拍照位到位
        public bool NeedlePFinish { get; set; }//扎针位到位
        public bool NeedleADDFinish { get; set; }//扎针补正位到位
        public bool LimitLeft { get; set; } //左限位
        public bool LimitRight { get; set; } //右限位
        public bool LimitOriginal { get; set; } //原点
        public bool OriginLocation { get; set; }//手动回原
        public bool LeftJOG { get; set; }//正点动
        public bool RightJOG { get; set; }//负点动
        public bool STOP { get; set; }//停止
        public bool JOGControl { get; set; } = false;//寸动控制
        public int JOGDistance { get; set; }//寸动距离
        public bool AutoStart1 { get; set; }//工位1自动启动
        public bool AutoStop1 { get; set; }//工位1自动停止
        public bool Auto1 { get; set; }//工位1自动
        public bool Menul1 { get; set; }//工位1手动
        public bool Reset1 { get; set; }//工位1一键复位
        public bool AutoMFlag1 { get; set; }//工位1手自动标志位
        public bool StartSFlag1 { get; set; }//工位1启动停止标志位
        public bool AutoStart2 { get; set; }//工位2自动启动
        public bool AutoStop2 { get; set; }//工位2自动停止
        public bool Auto2 { get; set; }//工位2自动
        public bool Menul2 { get; set; }//工位2手动
        public bool Reset2 { get; set; }//工位2一键复位
        public bool AutoMFlag2 { get; set; }//工位2手自动标志位
        public bool StartSFlag2 { get; set; }//工位2启动停止标志位
        public bool AutoStart3 { get; set; }//工位3自动启动
        public bool AutoStop3 { get; set; }//工位3自动停止
        public bool Auto3 { get; set; }//工位3自动
        public bool Menul3 { get; set; }//工位3手动
        public bool Reset3 { get; set; }//工位3一键复位
        public bool AutoMFlag3 { get; set; }//工位3手自动标志位
        public bool StartSFlag3 { get; set; }//工位3启动停止标志位
        public bool AutoStart4 { get; set; }//工位4
        public bool AutoStop4 { get; set; }
        public bool Auto4 { get; set; }
        public bool Menul4 { get; set; }
        public bool Reset4 { get; set; }
        public bool AutoMFlag4 { get; set; }
        public bool StartSFlag4 { get; set; }
        public bool AutoStart5 { get; set; }//工位五
        public bool AutoStop5 { get; set; }
        public bool Auto5 { get; set; }
        public bool Menul5 { get; set; }
        public bool Reset5 { get; set; }
        public bool AutoMFlag5 { get; set; }
        public bool StartSFlag5 { get; set; }
        public bool AutoStart6 { get; set; }//工位六
        public bool AutoStop6 { get; set; }
        public bool Auto6 { get; set; }
        public bool Menul6 { get; set; }
        public bool Reset6 { get; set; }
        public bool AutoMFlag6 { get; set; }
        public bool StartSFlag6 { get; set; }
        public bool AutoStart7 { get; set; }//工位七
        public bool AutoStop7 { get; set; }
        public bool Auto7 { get; set; }
        public bool Menul7 { get; set; }
        public bool Reset7 { get; set; }
        public bool AutoMFlag7 { get; set; }
        public bool StartSFlag7 { get; set; }
        public bool AutoStart8 { get; set; }//工位八
        public bool AutoStop8 { get; set; }
        public bool Auto8 { get; set; }
        public bool Menul8 { get; set; }
        public bool Reset8 { get; set; }
        public bool AutoMFlag8 { get; set; }
        public bool StartSFlag8 { get; set; }


        public string CurrentPositionY1R { get; set; }//一工位Y1右当前位置
        public string PhotoPositionY1R { get; set; }//拍照位位置
        public string NeedlePositionY1R { get; set; }//扎针位位置
        public string NeedleAddPositionY1R { get; set; }//扎针补正位位置
        public string CurrentPositionY1L { get; set; }//一工位Y2左当前位置
        public string PhotoPositionY1L { get; set; }//拍照位位置
        public string NeedlePositionY1L { get; set; }//扎针位位置
        public string NeedleAddPositionY1L { get; set; }//扎针补正位位置
        public string CurrentPositionX1R { get; set; }//一工位X1右当前位置
        public string PhotoPositionX1R { get; set; }//拍照位位置
        public string NeedlePositionX1R { get; set; }//扎针位位置
        public string NeedleAddPositionX1R { get; set; }//扎针补正位位置
        public string CurrentPositionX1L { get; set; }//一工位X2左当前位置
        public string PhotoPositionX1L { get; set; }//拍照位位置
        public string NeedlePositionX1L { get; set; }//扎针位位置
        public string NeedleAddPositionX1L { get; set; }//扎针补正位位置
        public string CurrentPositionθ1R { get; set; }//一工位θ1当前位置
        public string PhotoPositionθ1R { get; set; }//拍照位位置
        public string NeedlePositionθ1R { get; set; }//扎针位位置
        public string NeedleAddPositionθ1R { get; set; }//扎针补正位位置
        public string CurrentPositionθ1L { get; set; }//一工位θ2当前位置
        public string PhotoPositionθ1L { get; set; }//拍照位位置
        public string NeedlePositionθ1L { get; set; }//扎针位位置
        public string NeedleAddPositionθ1L { get; set; }//扎针补正位位置


        public string CurrentPositionY2R { get; set; }//二工位Y1当前位置
        public string PhotoPositionY2R { get; set; }//拍照位位置
        public string NeedlePositionY2R { get; set; }//扎针位位置
        public string NeedleAddPositionY2R { get; set; }//扎针补正位位置
        public string CurrentPositionY2L { get; set; }//二工位Y2当前位置
        public string PhotoPositionY2L { get; set; }//拍照位位置
        public string NeedlePositionY2L { get; set; }//扎针位位置
        public string NeedleAddPositionY2L { get; set; }//扎针补正位位置
        public string CurrentPositionX2R { get; set; }//二工位X1当前位置
        public string PhotoPositionX2R { get; set; }//拍照位位置
        public string NeedlePositionX2R { get; set; }//扎针位位置
        public string NeedleAddPositionX2R { get; set; }//扎针补正位位置
        public string CurrentPositionX2L { get; set; }//二工位X2当前位置
        public string PhotoPositionX2L { get; set; }//拍照位位置
        public string NeedlePositionX2L { get; set; }//扎针位位置
        public string NeedleAddPositionX2L { get; set; }//扎针补正位位置
        public string CurrentPositionθ2R { get; set; }//二工位θ1当前位置
        public string PhotoPositionθ2R { get; set; }//拍照位位置
        public string NeedlePositionθ2R { get; set; }//扎针位位置
        public string NeedleAddPositionθ2R { get; set; }//扎针补正位位置
        public string CurrentPositionθ2L { get; set; }//二工位θ2当前位置
        public string PhotoPositionθ2L { get; set; }//拍照位位置
        public string NeedlePositionθ2L { get; set; }//扎针位位置
        public string NeedleAddPositionθ2L { get; set; }//扎针补正位位置

        public bool MenulPhoto1L { get; set; }//一工位手动拍照左
        public bool MenulPhoto1R { get; set; }//一工位手动拍照右
        public bool MemulPhoto2L { get; set; }//二工位手动拍照左
        public bool MemulPhoto2R { get; set; }//二工位手动拍照右
        public bool TakePicturePosition1L { get; set; }//一工位一键拍照位左
        public bool TakePicturePosition1R { get; set; }//一工位一键拍照位右
        public bool TakePicturePosition2L { get; set; }//二工位一键拍照位左
        public bool TakePicturePosition2R { get; set; }//二工位一键拍照位右
        public bool MarkPosition1L { get; set; }//一工位一键标定位左
        public bool MarkPosition1R { get; set; }//一工位一键标定位右
        public bool MarkPosition2L { get; set; }//二工位一键标定位左
        public bool MarkPosition2R { get; set; }//二工位一键标定位右
        public bool MarkStop { get; set; }//流程停止
        public bool StartSFlag11 { get; set; }//启动界面工位按钮显示
        public bool StartSFlag21 { get; set; }
        public bool StartSFlag31 { get; set; }
        public bool StartSFlag41 { get; set; }
        public bool StartSFlag51 { get; set; }
        public bool StartSFlag61 { get; set; }
        public bool StartSFlag71 { get; set; }
        public bool StartSFlag81 { get; set; }
        public bool StartSFlag91 { get; set; }
        public bool StartSFlag101 { get; set; }

    }
}
