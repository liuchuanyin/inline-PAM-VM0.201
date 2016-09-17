Imports System.Windows.Forms.DataVisualization.Charting

Public Module Variables

    ''' <summary>
    ''' 设备类型
    ''' </summary>
    ''' <remarks></remarks>
    Public MACTYPE As String

    Public CCD_Lock_Flag As Boolean        'CCD拍照锁变量

#Region "Axis 定义"

    '0号卡
    Public Const GlueX = 1      '点胶站X轴
    Public Const GlueY = 2      '点胶站Y轴
    Public Const GlueZ = 3      '点胶站Z轴
    Public Const PasteX = 4     '组装站X轴
    Public Const PasteZ = 5     '组装站Z轴
    Public Const PasteR = 6     '组装站R轴
    Public Const PreTakerX = 7     '取料站X轴
    Public Const PreTakerZ = 8     '取料站Z轴

    '1号卡
    Public Const PreTakerR = 1     '取料站R轴
    Public Const CureX = 2     '预固化站X轴
    Public Const FineX = 3     '精确补偿站X轴
    Public Const FineY = 4     '精确补偿站Y轴
    Public Const RecheckX = 5     '复检站X轴
    Public Const RecheckY = 6     '复检站Y轴
    Public Const FeedZ = 7     '供料Z轴
    Public Const RecycleZ = 8     '料盘回收Z轴

    '2号卡
    Public Const PasteY1 = 1    '组装站Y1轴
    Public Const PasteY2 = 2    '组装站Y2轴
    Public Const PreTakerY1 = 3    '取料站Y1轴
    Public Const PreTakerY2 = 4    '取料站Y2轴

#End Region

    Public Const CW As Long = 1
    Public Const CCW As Long = -1
    ''' <summary>
    ''' 点胶气压
    ''' </summary>
    ''' <remarks></remarks>
    Public Pressure(2) As Double

    ''' <summary>
    ''' 是否排过胶，用于判断是否需要擦胶
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_Purged As Boolean

#Region "颜色变量区域"
    '********************* 颜色变量区域 *****************************
    Public Color_Unselected As Color = Color.FromArgb(234, 234, 235)   '16进制颜色EAEAEB，Button未选中
    Public Color_Selected As Color = Color.FromArgb(174, 218, 151)  '16进制颜色AEDA97 = Argb(174, 218, 151)，Button选中
    Public Color_Yellow As Color = Color.FromArgb(253, 253, 191)    'UPH,Material 使用 FDFDBF
    Public Color_Red As Color = Color.FromArgb(200, 37, 6)   'C82506
    Public Color_Blue As Color = Color.FromArgb(179, 202, 255)  'B3CAFF
    Public Color_LightBlue As Color = Color.FromArgb(196, 204, 234)     'C4CCEA
    Public Color_Purple As Color = Color.FromArgb(222, 188, 228)     '紫色DEBCE4
    Public Color_LightPink As Color = Color.FromArgb(228, 117, 43)       '浅粉色E4752B
    Public Color_DarkPink As Color = Color.FromArgb(252, 223, 222)       '深粉色FCDFDE
    Public Color_LightGreen As Color = Color.FromArgb(203, 255, 243)  '蓝绿之间的颜色CBFFF3
#End Region

#Region "窗体是否被打开标志"
    ''' <summary>
    ''' 窗体打开标志位
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_FrmEngineeringOpned As Boolean

#End Region

#Region "文件路径"
    ''' <summary>
    ''' "E:\BZ-Data\Log\"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_Log As String = "E:\BZ-Data\Log\"
    Public Path_Errlog As String = "E:\BZ-Data\ErrLog\"
    Public Path_Image As String = "E:\BZ-Data\Images\"
    Public Path_Data As String = "E:\BZ-Data\Data\"
    ''' <summary>
    ''' 设备信息保存路径 "D:\BZ-Parameter\Setting.ini"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_IniFile As String = "D:\BZ-Parameter\Setting.ini"
    ''' <summary>
    ''' 设置参数保存路径 "D:\BZ-Parameter\Par.xml"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_Par As String = "D:\BZ-Parameter\Par.xml"
    Public Path_Par_Glue As String = "D:\BZ-Parameter\Par_Glue.xml"
    ''' <summary>
    ''' 密码保存 "D:\BZ-Parameter\User.dat"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_UserCode As String = "D:\BZ-Parameter\User.dat"
    ''' <summary>
    ''' "D:\BZ-Parameter\MachineType\MachineeType.dat"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_MacType As String = "D:\BZ-Parameter\MachineType\MachineeType.dat"
    ''' <summary>
    ''' 良率信息"D:\BZ-Parameter\Production.xml"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_YieldData As String = "D:\BZ-Parameter\Production.xml"
    ''' <summary>
    ''' 轴位置保存"D:\BZ-Parameter\Par_Position.xml"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_Par_Pos As String = "D:\BZ-Parameter\Par_Position.xml"
    ''' <summary>
    ''' "D:\BZ-Parameter\Par_CCD.xml"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_Par_CCD As String = "D:\BZ-Parameter\Par_CCD.xml"
    ''' <summary>
    ''' "D:\BZ-Parameter\Alarm List.xml"
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_AlarmFile As String = "D:\BZ-Parameter\Alarm List.xml"

    ''' <summary>
    ''' Tray盘矩阵点位保存文件
    ''' </summary>
    ''' <remarks></remarks>
    Public Path_TrayMatrix As String = "D:\BZ-Parameter\TrayMatrix.xml"

#End Region

#Region "时间变量"
    ''' <summary>
    ''' 软件打开时间
    ''' </summary>
    ''' <remarks></remarks>
    Public Time_SWOpened As Date
    ''' <summary>
    ''' 软件关闭时间
    ''' </summary>
    ''' <remarks></remarks>
    Public Time_SWClosed As Date
#End Region

#Region "网络连接 变量定义"

    Public Winsock1State As Boolean
    Public Winsock1_String As String
    Public Winsock1Message As String
    Public Winsock1_TimmingWatch As Long
    Public Winsock1_Data(200) As String

    Public Winsock2State As Boolean
    Public Winsock2_String As String
    Public Winsock2Message As String
    Public Winsock2_TimmingWatch As Long
    Public Winsock2_Data(100) As String

    Public Winsock3State As Boolean
    Public Winsock3_String As String
    Public Winsock3Message As String
    Public Winsock3_TimmingWatch As Long
    Public Winsock3_Data(100) As String

#End Region

#Region "参数设置结构体"

    ''' <summary>
    ''' 软件版本
    ''' </summary>
    ''' <remarks></remarks>
    Public Const VERSION_SOFTWARE As String = "SV1.2.5"
    ''' <summary>
    ''' 软体更新日期
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UPDATE_SOFTWARE As String = "2016/07/29"

    ''' <summary>
    ''' DownTime
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure DownTime_Info
        Public DownTime As Date
        Public Project As String
        Public BU As String
        Public Floor As String
        Public Line As String
        Public AE_ID As String
        Public AE_SubID As String
        Public AE_Vendor As String
        Public Machine_SN As String
        Public SW_rev As String
        Public HW_rev As String
        Public Time_Mode As Integer
        Public Error_Code As String
        Public Error_Description As String
        Public Color As String
        Public Notes As String
        Public Station_ID As String
        Public StandBy1 As String
        Public StandBy2 As String
        Public StandBy3 As String
        Public StandBy4 As String
    End Structure

    Public Structure Parameter
        Public num() As Double
        ''' <summary>
        ''' 功能勾选项
        ''' </summary>
        ''' <remarks></remarks>
        Public chkFn() As Boolean
        ''' <summary>
        ''' DownTime信息结构体
        ''' </summary>
        ''' <remarks></remarks>
        Public Machine_Info As DownTime_Info
        Public CCD() As String
        Sub Ini()
            ReDim num(60)
            ReDim chkFn(40)
            ReDim CCD(30)
        End Sub
    End Structure
    ''' <summary>
    ''' 参数结构体
    ''' </summary>
    ''' <remarks></remarks>
    Public par As Parameter
#End Region

#Region "点胶参数结构体"

    Public Structure Glue_Vel_Delay
        Dim vel As Double
        Dim startDelay As Double
        Dim endDelay As Double
    End Structure

    Public Structure Glue_Segment
        Dim Segment() As Glue_Vel_Delay
        Sub Ini()
            ReDim Segment(20)
        End Sub
    End Structure

    Public Par_Glue As Glue_Segment
#End Region

#Region "账户名密码结构体"
    ''' <summary>
    ''' 用户结构体，保存账户名和密码
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure NamePassword
        Dim Name As String
        Dim Code As String
    End Structure
    ''' <summary>
    ''' 用户
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure UserCollect
        Dim User1 As NamePassword
        Dim User2 As NamePassword
        Dim User3 As NamePassword
    End Structure
    Public BOZHON As UserCollect

    ''' <summary>
    ''' =1：Par Setting；=2：Manual;=3：CCD Settings;=4 Machine Type
    ''' </summary>
    ''' <remarks></remarks>
    Public Login_Mode As Short

#End Region

    '三标志
    Public Structure sFlag3
        ''' <summary>
        ''' 使能
        ''' </summary>
        ''' <remarks></remarks>
        Dim Enable As Boolean   '使能
        ''' <summary>
        ''' 状态
        ''' </summary>
        ''' <remarks></remarks>
        Dim State As Boolean    '状态
        ''' <summary>
        ''' 结果
        ''' </summary>
        ''' <remarks></remarks>
        Dim Result As Boolean   '结果
    End Structure

#Region "设备运行状态标志位"
    ''' <summary>
    ''' Machine Pause Flag
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_MachinePause As Boolean
    ''' <summary>
    ''' Machine AutoRun Flag
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_MachineAutoRun As Boolean
    ''' <summary>
    ''' Machine Stop Flag
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_MachineStop As Boolean
    ''' <summary>
    ''' 急停标志位
    ''' </summary>
    ''' <remarks></remarks>
    Public IsSysEmcStop As Boolean

    Public CycelTime As Long        '机器周期时间
    Public CycelTimeEn As Boolean   '
    Public TotalResult As Integer   '产品检测总结果(0:未检测；1:OK；2:NG；)
    Public CT As Double

#End Region

    Public Structure Hole
        Dim isHaveProduct As Boolean
        Dim isProductOk As Boolean
        Dim ProductBarcode As String
        Dim Press_Taker As Double
        Dim Press_Paste As Double
        Dim CT As Double
        Dim time As String
        ''' <summary>
        ''' 抛料数量统计
        ''' </summary>
        ''' <remarks></remarks>
        Dim Cnt_Pao As UShort
        Dim offset_X As Double
        Dim offset_Y As Double
        Dim offset_A As Double
        Dim CC As Double
        Dim CC1 As Double

    End Structure

    Public Structure Tray
        ''' <summary>
        ''' 载具的条码
        ''' </summary>
        ''' <remarks></remarks>
        Dim Tray_Barcode As String
        ''' <summary>
        ''' 载具是否可用
        ''' </summary>
        ''' <remarks></remarks>
        Dim isTrayOK As Boolean
        Dim isHaveTray As Boolean
        Dim Hole() As Hole
        Sub init()
            ReDim Hole(12)
        End Sub
    End Structure

    Public Tray_Pallet(4) As Tray

    ''' <summary>
    ''' 记录当前的Tray_Pallet(2).Tray_Barcode
    ''' </summary>
    ''' <remarks></remarks>
    Public tempTray2_Barcode As String

    ''' <summary>
    ''' 临时记录各站的步序号
    ''' </summary>
    ''' <remarks></remarks>
    Public TempStep(10) As Integer

    Public Const Field0 As String = "设备" & vbCr & "编号"
    Public Const Field1 As String = "治具" & vbCr & "编号"
    Public Const Field2 As String = "装配时间"
    Public Const Field3 As String = "产品编号"
    Public Const Field4 As String = "用户" & vbCr & "名称"

    Public Const Field5 As String = "OK/NG"
    Public Const Field6 As String = "Brc_X" & vbCr & "±1.00"
    Public Const Field7 As String = "Brc_Y" & vbCr & "±1.00"
    Public Const Field8 As String = "Mod_X" & vbCr & "±1.00"
    Public Const Field9 As String = "Mod_Y" & vbCr & "±1.00"

    Public Const Field16 As String = "CC" & vbCr & "0.05"
    Public Const Field17 As String = "Tossing"
    Public Const Field18 As String = "Press" & vbCr & "kg"
    Public Const Field19 As String = "CT" & vbCr & "s"

End Module
