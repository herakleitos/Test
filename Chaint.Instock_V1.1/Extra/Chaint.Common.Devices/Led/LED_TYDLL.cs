using System;
using System.Runtime.InteropServices;

namespace Chaint.Common.Devices.LED
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LEDDEV
    {
        public int iID;			//LED设备连接号.
        public int iWidth;	    //屏宽度(象素点)
        public int iHeight;		//屏宽度(象素点)
        public byte cScrType;	//1=双基色屏(4色); 2=单色屏
        public byte cScrNo;		//屏号
        public byte cCard;		//LED型号和状态
        public byte cRev1;
        public uint dwRev1;
        public uint dwRev2;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct DATE_ST
    {
        public ushort wYear;	//年
        public byte cMonth;		//月
        public byte cDay;		//日
    };

    //颜色
    public enum Color
    {
        BLACK = 0x0,
        RED = 0x0000FF,
        GREEN = 0x00FF00,
        YELLOW = 0x00FFFF
    }

    //波特率号
    public enum BaudNo
    {
        NO_9600,    //0
        NO_19200,   //1
        NO_38400,   //2
        NO_57600,   //3
        NO_115200   //4
    };

    //窗口边框
    public enum WndEdge
    {
        NONE,		//0		//无边框
        SOLID,		//1		//实线
        DOTTED 	    //2		//虚线
    }

    //LED亮度
    public enum LedBright : byte
    {
        MIN_BRIGHT,		    //0		//亮度最小(关电)
        MAX_BRIGHT = 255	    //255	//最大亮度
    }

    //即时信息播放控制
    public enum PlayCon : byte
    {
        ALWAYS,			//0		//播放不自动返回
        BYTIME,			//1		//按时间
        BYLOOP			//2		//按轮数
    }

    //通信接口
    public enum LedPort { RS232, NET };		//0,1

    //进入方式(适合所有文件)：
    public enum InMode : byte
    {
        RAND,			//0		//随机
        DIRECT,		//1		//直接显示
        BLINK,		//2		//闪动
        I_3, I_4, I_5, I_6, I_7, I_8, I_9, I_10, I_11, I_12, I_13, I_14, I_15, I_16, I_17, I_18, I_19,
        I_20, I_21, I_22, I_23, I_24, I_25, I_26, I_27, I_28, I_29, I_30, I_31,

        //仅文本可用的进入方式: (退出方式和停留时间无效)
        TEXTUP = 100,		//文本连续上移
        TEXTWSLEFT = 101,		//逐字连续左移(连成一行)
        TEXTWSUP = 102,		//逐字连续上移(连成一列)
        TEXTWSRIGHT = 103		//逐字连续右移(连成一行)
    }

    //0随机选择,1直接显示,2闪动,3水平百叶[8],4水平百叶[16],5垂直百叶[8],6垂直百叶[16],7左移,8右移,9上移,10下移,
    //11从右下角移入,12从右上角移入,13从左下角移入,14从左上角移入,15从左到右,16从右到左,17从上到下,18从下到上,
    //19中间到左右,20左右到中间,21中间到上下,22上下到中间,23从左上角(方),24从左下角(方),25从右上角(方),26从右下角(方),
    //27左对角(方),28右对角(方),29中间到四周(方),30四周到中间(方),31中间到四周(十字)。

    //退出方式：
    public enum OutMode : byte
    {
        RAND,			//0		//随机
        KEEP,			//1		//保持不动
        CLEAR,		//2		//直接清
        O_3, O_4, O_5, O_6, O_7, O_8, O_9, O_10, O_11, O_12, O_13, O_14, O_15, O_16, O_17, O_18, O_19,
        O_20, O_21, O_22, O_23, O_24, O_25, O_26, O_27, O_28, O_29, O_30, O_31
    }

    //0随机选择,1保持/与下屏连动,2直接清屏,3水平百叶[8],4水平百叶[16],5垂直百叶[8],6垂直百叶[16],7左移,8右移,9上移,10下移,
    //11从左上角移出,12从左下角移出,13从右上角移出,14从右下角移出,15从左到右,16从右到左,17从上到下,18从下到上,
    //19中间到左右,20左右到中间,21中间到上下,22上下到中间,23从左上角(方),24从左下角(方),25从右上角(方),26从右下角(方),
    //27左对角(方),28右对角(方),29中间到四周(方),30四周到中间(方),31中间到四周(十字)

    //进入、退出速度: 0-32. 0最慢,16中速, 32最快.
    public enum Speed
    {
        SLOWEST,		//0		//最慢
        S_1, S_2, S_3, S_4, S_5, S_6, S_7, S_8, S_9, S_10, S_11, S_12, S_13, S_14, S_15,
        MID,			//16	//中速
        S_17, S_18, S_19, S_20, S_21, S_22, S_23, S_24, S_25, S_26, S_27, S_28, S_29, S_30, S_31,
        FASTEST		    //32	//最快
    }

    //LED屏类型
    public enum ScrType : byte
    {
        SCRTYPE_3C,			//0 三基色
        SCRTYPE_2C,			//1 双基色
        SCRTYPE_1C			//2 单色
    }

    //窗口类型
    public enum WndType
    {
        FILE,			//0		//文件播放窗
        TIMEDATE,		//1		//时间日期窗
        COUNTUP,		//2		//正日期计数窗
        COUNTDOWN,		//3		//倒日期计数窗
        TEMPHUMI,		//4		//温度湿度窗
        RESV,
        ROLLEDGE,		//6		//滚动花边窗
    }

    //窗口格式设置对象选择------
    //时间日期窗
    public enum TDWObj
    {
        TIME,			//0		//设置时间格式
        DATE,			//1		//设置日期格式
        WEEK			//2		//设置星期格式
    }

    //温度湿度窗
    public enum THObj
    {
        TEMP,			//0		//设置温度格式
        HUMI			//1		//设置湿度格式
    }

    //旋转花边窗
    public enum Direction
    {
        CLOCKWISE,		    //0		//顺时针
        ANTICLOCK			//1		//逆时针
    }

    //--------------------------

    //函数返回值
    public enum RetVal
    {
        ERR_OK,             //0		//OK
        ERR_OPEN,			//1		//创建Socket或打开串口失败!
        ERR_PARA,			//2		//不能设置Socket或串口参数!
        ERR_SEND,			//3		//发送包时出错!
        ERR_READ,			//4		//帧接收时出错!
        ERR_TIMEOUT,		//5		//帧响应超时!
        ERR_CHKSUM,			//6		//响应帧校验和错!

        ERR_CONNECT,		//7		//不能与LED屏建立Socket连接!
        ERR_IP,				//8		//无效的IP地址!

        ERR_CONNECT_NUM,	//9		//建立的连接数目超界!
        ERR_NO_CONNECT,		//10	//没有与LED屏建立连接或连接ID不存在!
        ERR_NO_MEMIMAGE,	//11	//没建立内存画面!
        ERR_CMD_EXEC,		//12	//显示屏当前不能执行指定的操作!
        ERR_MEM_ALLOC,		//13	//存贮器定位错误!
        ERR_FILE_OPEN,		//14	//文件不能打开或创建!				
        ERR_FILE_FORMAT,	//15	//文件格式不对或不识别!
        ERR_RAM_LACK,		//16	//LED屏的RAM存贮器容量不足!
        ERR_PLAYFILE_EMPTY,	//17	//LED播放文件没有加载或无内容!
        ERR_PLAYFILE_OPEN,	//18	//LED播放文件不能打开!
        ERR_PLAYFILE_FORMAT,//19	//LED播放文件格式不对或不识别!
        ERR_PROGRAM_WIN,	//20	//节目下无窗口或窗口数量超界!
        ERR_WIN_SIZE,		//21	//窗口大小超出LED屏或者太小无显示区!
        ERR_WIN_POS,		//22	//窗口完全位于LED屏显示区之外!
        ERR_WIN_EMPTY,		//23	//窗口无内容或文档窗下没有包含文件!
        ERR_WIN_UNKNOWN,	//24	//播放文件包含不识别的窗口类型!
        ERR_FILE_PROCESS,	//25	//播放文件包含不识别、不存在或不能处理的文件!
        ERR_RES_LACK,		//26	//资源不足(字体太多或太大)!
        ERR_UNSUPPORT,		//27	//播放文件包含不支持的功能!
        ERR_FLASH_LACK,		//28	//播放内容超出LED屏的FLASH存贮器容量!
        ERR_SEND_ABORT,		//29	//发送线程被中止!
        ERR_ITEM_POS,		//30	//节目、窗口或文件项目位置不存在或不适合进行指定的操作!
        ERR_START_OFFICE,	//31	//不能启动Word或Excel!
        ERR_APP_OFFICE		//32	//操作Word或Excel时出错!
    };

    public class TYLedDll
    {
        //连接和关闭函数
        [DllImport("TYLed.dll")]
        public static extern int ConnectLED_TCPIP(ref byte cIP, ushort wPortNo, uint iConnectTimeOut, uint iRvTimeOut, ref LEDDEV Dev);
        [DllImport("TYLed.dll")]
        public static extern int ConnectLED_RS232(byte cScrNo, uint iCOMSNo, uint iBaudNo, ref LEDDEV Dev);
        [DllImport("TYLed.dll")]
        public static extern bool CloseLEDConnect(int iID);

        //内存映象操作函数
        [DllImport("TYLed.dll")]
        public static extern bool Image_Clear(int iID);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern bool Image_SetFontW(int iID, string szFontName, int iCharWidth, int iCharHeight, bool bBold, bool bItalic);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern bool Image_WriteTextW(int iID, string szText, int iX, int iY, int iColor, int iCharSpacing, bool bTransparent, bool bRightToLeft);
        [DllImport("TYLed.dll")]
        public static extern bool Image_DrawLine(int iID, int iStartX, int iStartY, int iEndX, int iEndY, int iColor, bool bDashed);
        [DllImport("TYLed.dll")]
        public static extern bool Image_DrawRect(int iID, int iLeft, int iTop, int iRight, int iBottom, int iColor, bool bDashed);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern bool Image_DrawPicFileW(int iID, string szPicFile, int iX, int iY, byte cRGBMin);
        [DllImport("TYLed.dll")]
        public static extern bool Image_DrawMemPicFile(int iID, ref byte pMemPicFile, uint iFileSize, int iPicType, int iX, int iY, byte cRGBMin);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern bool Image_SaveAsBitmapFileW(int iID, string szBitmapFileName);
        [DllImport("TYLed.dll")]
        public static extern bool Image_DrawOnWindow(int iID, IntPtr hWnd);

        //发送函数
        [DllImport("TYLed.dll")]
        public static extern int SendImageToLED(int iID, byte cInMode, byte cInSpeed, ushort wStay, byte cOutMode, byte cOutSpeed, byte cPlayCon, ushort wTimeOrLoops, bool bWaitSend);
        [DllImport("TYLed.dll")]
        public static extern int SendToLED(int iID, byte cPlayCon, ushort wTimeOrLoops, bool bWaitSend);
        [DllImport("TYLed.dll")]
        public static extern int SendToFLASH(int iID, bool bWaitSend);

        //播放控制及状态
        [DllImport("TYLed.dll")]
        public static extern int ReturnToFlashPlay(int iID);
        [DllImport("TYLed.dll")]
        public static extern int GetSendStatus(int iID);
        [DllImport("TYLed.dll")]
        public static extern int StopSending(int iID);

        //LED播放文件操作
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int LoadPlayFileW(int iID, string szPlayFile);
        [DllImport("TYLed.dll")]
        public static extern int CreateNewPlayFile(int iID);
        [DllImport("TYLed.dll")]
        public static extern int InsertProgram(int iID, int iProPos, bool bLimitTime, ushort wProTime);
        [DllImport("TYLed.dll")]
        public static extern int InsertWindow(int iID, int iProPos, int iWndPos, uint iWndType, int iX, int iY, int iWndWd, int iWndHi, int iEdge, int iEdgeColor, bool bTransparent);
        [DllImport("TYLed.dll")]
        public static extern int InsertImageItem(int iID, int iProPos, int iWndPos, int iItemPos, byte cInMode, byte cInSpeed, ushort wStay, byte cOutMode, byte cOutSpeed);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int InsertTextItemW(int iID, int iProPos, int iWndPos, int iItemPos, string szText, byte cInMode, byte cInSpeed, ushort wStay, byte cOutMode, byte cOutSpeed);
        [DllImport("TYLed.dll")]
        public static extern int DeletePlayItem(int iID, int iProPos, int iWndPos, int iItemPos);
        [DllImport("TYLed.dll")]
        public static extern int DeleteWindow(int iID, int iProPos, int iWndPos);
        [DllImport("TYLed.dll")]
        public static extern int DeleteProgram(int iID, int iProPos);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int SetTimeDateWndFormatW(int iID, int iProPos, int iWndPos, int iSetObj, int iFormat, int iX, int iY, string szPrefix, int iColor, int iCharSpacing, string szFontName, int iCharWidth, int iCharHeight, bool bBold);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int SetDayCountWndFormatW(int iID, int iProPos, int iWndPos, ref DATE_ST Date, int iX, int iY, string szPrefix, int iPreColor, int iDigits, int iColor, string szSuffix, int iSuffColor, int iCharSpacing, string szFontName, int iCharWidth, int iCharHeight, bool bBold);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int SetTempHumiWndFormatW(int iID, int iProPos, int iWndPos, int iSetObj, bool bShow, int iX, int iY, string szPrefix, int iPreColor, int iTempOrHumiColor, string szSuffix, int iSuffColor, int iCharSpacing, string szFontName, int iCharWidth, int iCharHeight, bool bBold);
        [DllImport("TYLed.dll")]
        public static extern int SetRollEdgeWndFormat(int iID, int iProPos, int iWndPos, byte cEdgeStyle, int iColor, byte cDirection, byte cSpeed);
        [DllImport("TYLed.dll")]
        public static extern int ReplacePlayItem(int iID, int iProPos, int iWndPos, int iItemPos);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int ReplaceItemTextW(int iID, int iProPos, int iWndPos, int iItemPos, string szNewText);
        [DllImport("TYLed.dll", CharSet = CharSet.Unicode)]
        public static extern int SetTextItemFormatW(int iID, int iProPos, int iWndPos, int iItemPos, int iColor, int iCharSp, int iRowSp, int iLeftSp, int iTopSp, bool bRightToLeft, string szFontName, int iCharWidth, int iCharHeight, bool bBold, bool bItalic);
        [DllImport("TYLed.dll")]
        public static extern int SetPlayItemShowMode(int iID, int iProPos, int iWndPos, int iItemPos, byte cInMode, byte cInSpeed, ushort wStay, byte cOutMode, byte cOutSpeed);

        //其它
        [DllImport("TYLed.dll")]
        public static extern int SetLEDBright(int iID, byte cBrightVal);
        [DllImport("TYLed.dll")]
        public static extern int UpdateLEDTime(int iID);
    }
}


