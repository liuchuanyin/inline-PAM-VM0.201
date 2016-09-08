﻿Imports System.Math

Module Motion

    Public HomeTime(4, 8) As Long
    Public AxisHome(2, 8) As sFlag3
    Public HomeStep(2, 8) As Integer                         '回原点
    Public AxisTime(2, 8) As Double '各卡上轴运动到位判断时间计数

    Public HomeSearchDist(2, 8) As Double     '原点搜索距离
    Public HomeOffsetDist(2, 8) As Double     '第2次原点搜索偏移距离
    Public LimToHomeDist(2, 8) As Double      '到极限走过原点的距离
    Public HomeCapture(2, 8) As Integer       '回原点捕获的临时状态
    Public HomeTempPos(2, 8) As Long          '回原点捕获的临时位置
    Public HomeCounter(2, 8) As Long          '回原点次数计数

#Region "固高板卡 变量定义"

    ''' <summary>
    ''' 运动控制卡的数量
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GTS_CardNum As Integer = 3
    ''' <summary>
    ''' 扩展模块的数量
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GTS_ModNum As Integer = 2
    '''<summary>
    '''运动控制卡是否打开
    ''' </summary>
    Public GTS_Opened_Card As Boolean
    '''<summary>
    ''' 扩展模块IO是否打开 
    ''' </summary>
    Public GTS_Opened_EM As Boolean
    '''<summary>
    ''' 运动控制卡IO是否打开 
    ''' </summary>
    Public GTS_Opened_EX As Boolean
    '''<summary>
    ''' 扩展ADDA模块是否打开 
    ''' </summary>
    Public GTS_Opened_ADDA As Boolean

    ''' <summary>
    ''' 卡所对应的IO数 {16, 16, 16, 16} 
    ''' </summary>
    ''' <remarks>##AABBCC</remarks>
    Public GTS_CardIONum() As Integer = {16, 16, 16, 16}
    ''' <summary>
    ''' 卡所对应的轴数{8, 8, 4} 
    ''' </summary>
    ''' <remarks>##112233</remarks>
    Public GTS_AxisNum() As Short = {8, 8, 4, 0}

    ''' <summary>
    ''' 扩展卡IO数 {16, 16, 16, 16} 
    ''' </summary>
    ''' <remarks>##AABBCC</remarks>
    Public GTS_ModIONum() As Integer = {16, 16, 16, 16, 16, 16}
    ''' <summary>
    ''' 输入信号（card,index）
    ''' </summary>
    ''' <remarks></remarks>
    Public EXI(6, 16) As Boolean '输入信号
    ''' <summary>
    ''' 输出信号（card,index）
    ''' </summary>
    ''' <remarks></remarks>
    Public EXO(6, 16) As Boolean '输出信号
    ''' <summary>
    ''' 扩展输卡入信号（card,index）
    ''' </summary>
    ''' <remarks></remarks>
    Public EMI(10, 16) As Boolean '扩展输卡入信号
    ''' <summary>
    ''' 扩展卡输出信号（card,index）
    ''' </summary>
    ''' <remarks></remarks>
    Public EMO(10, 16) As Boolean '扩展卡输出信号
    ''' <summary>
    ''' 原点信号(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public Home(6, 8) As Boolean '原点信号
    ''' <summary>
    ''' Postive Limit(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public LimitP(6, 8) As Boolean
    ''' <summary>
    ''' Negative Limit(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public LimitN(6, 8) As Boolean
    ''' <summary>
    ''' 伺服报警 Servo Alarm(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public ServoErr(6, 8) As Boolean
    ''' <summary>
    ''' 紧急停止 Emergency
    ''' </summary>
    ''' <remarks></remarks>
    Public EMGN(6, 8) As Boolean

    Public EMGN_ALL As Boolean
    ''' <summary>
    ''' 伺服使能 ServoOn(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public ServoOn(6, 8) As Boolean
    ''' <summary>
    ''' 电机到位信号，Motor Arrive(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public MotorArrive(6, 8) As Boolean

#End Region

#Region "功能：运动控制卡初始化函数"
    Public Sub MotionCardInit()
        Dim n, rtn As Short
        GTS_Opened_Card = False
        GTS_Opened_EX = False
        GTS_Opened_EM = False

        Try
            For n = 0 To GTS_CardNum - 1
                rtn = GT_Open(n, 0, 1)                                                         '打开控制卡0
                If rtn <> 0 Then
                    Frm_DialogAddMessage("运动控制板卡" & n & "打开失败，请检查！")
                    Exit Sub
                Else
                    rtn = GT_Reset(n)
                    If n = 0 Then
                        '复位运动控制卡0
                        rtn = GT_LoadConfig(n, Application.StartupPath & "\GTS_Config\GTS800_0.cfg")          '下载控制卡0配置参数
                    ElseIf n = 1 Then
                        '复位运动控制卡1
                        rtn = GT_LoadConfig(n, Application.StartupPath & "\GTS_Config\GTS800_1.cfg")          '下载控制卡1配置参数
                    ElseIf n = 2 Then
                        '复位运动控制卡1
                        rtn = GT_LoadConfig(n, Application.StartupPath & "\GTS_Config\GTS400_VB.cfg")          '下载控制卡2配置参数
                    End If
                   
                    If rtn <> 0 Then
                        Frm_DialogAddMessage("运动控制板卡" & n & "配置参数失败，请检查！")
                        Exit Sub
                    Else
                        ListBoxAddMessage("运动控制板卡" & n & "打开成功")
                    End If
                End If
            Next n

            GTS_Opened_Card = True
            GTS_Opened_EX = True

            If GTS_ModNum > 0 Then
                rtn = GT_OpenExtMdlGts(0, "gts.dll")                         '打开扩展IO模块
                If rtn <> 0 Then
                    Frm_DialogAddMessage("扩展IO模块打开失败，请检查！")
                    GTS_Opened_EM = False
                    Exit Sub
                Else
                    rtn = GT_ResetExtMdlGts(0)                                           '复位运动控制卡IO扩展模块
                    rtn = GT_LoadExtConfigGts(0, Application.StartupPath & "\GTS_Config\ExtModule.cfg") '载入运动控制卡IO扩展模块配置文件
                    If rtn <> 0 Then
                        Frm_DialogAddMessage("扩展IO模块配置参数失败，请检查！")
                        GTS_Opened_EM = False
                        Exit Sub
                    Else
                        ListBoxAddMessage("扩展IO模块配置参数成功！")
                        rtn = GT_ResetExtMdlGts(0)                                      '复位运动控制卡IO扩展模块
                        GTS_Opened_EM = True
                    End If
                End If


                rtn = GT_OpenExtMdlGts(1, "gts.dll")                         '打开扩展IO模块
                If rtn <> 0 Then
                    Frm_DialogAddMessage("扩展AD-DA模块打开失败，请检查！")
                    GTS_Opened_ADDA = False
                    Exit Sub
                Else
                    rtn = GT_ResetExtMdlGts(1)                                           '复位运动控制卡IO扩展模块
                    rtn = GT_LoadExtConfigGts(1, Application.StartupPath & "\GTS_Config\ExtModuleADDA.cfg") '载入运动控制卡IO扩展模块配置文件
                    If rtn <> 0 Then
                        Frm_DialogAddMessage("扩展AD-DA模块配置参数失败，请检查！")
                        GTS_Opened_ADDA = False
                        Exit Sub
                    Else
                        ListBoxAddMessage("扩展IO模块配置参数成功！")
                        rtn = GT_ResetExtMdlGts(1)                                      '复位运动控制卡IO扩展模块
                        GTS_Opened_ADDA = True
                    End If
                End If

            End If

            '//如下使能  位置清零
            For n = 0 To GTS_CardNum - 1
                For i = 1 To GTS_AxisNum(n)
                    Call GT_ClrSts(n, i, 1)
                    Call GT_SetAxisBand(n, i, 500, 20)
                    Call GT_AxisOn(n, i)
                Next i
                Delay(200)
                For i = 1 To GTS_AxisNum(n)
                    rtn = GT_SetPrfPos(n, i, 0)
                    rtn = GT_SetEncPos(n, i, 0)
                    Call GT_SynchAxisPos(n, 2 ^ (i - 1))
                Next i
            Next n

        Catch
            GTS_Opened_Card = False
            GTS_Opened_EX = False
            GTS_Opened_EM = False
            GTS_Opened_ADDA = False
        End Try
    End Sub
#End Region

    Public Sub HomeValue()
        'Station 2 Axis X
        HomeSearchDist(0, 1) = -1000
        HomeOffsetDist(0, 1) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 1) = -40      '到极限走过原点的距离
        'Station 2 Axis Y
        HomeSearchDist(0, 2) = -1000
        HomeOffsetDist(0, 2) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 2) = -40      '到极限走过原点的距离
        'Station 2 Axis Z
        HomeSearchDist(0, 3) = -1000
        HomeOffsetDist(0, 3) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(0, 3) = -25      '到极限走过原点的距离
        'Station 3 Axis X
        HomeSearchDist(0, 4) = -1000
        HomeOffsetDist(0, 4) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 5) = -40      '到极限走过原点的距离
        'Station 3 Axis Y
        HomeSearchDist(0, 5) = -1000
        HomeOffsetDist(0, 5) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 5) = -40      '到极限走过原点的距离
        'Station 3 Axis Z
        HomeSearchDist(0, 6) = -1000
        HomeOffsetDist(0, 6) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(0, 6) = -40     '到极限走过原点的距离
        'Station 3 Axis R
        HomeSearchDist(0, 7) = 200
        HomeOffsetDist(0, 7) = 5     '第2次原点搜索偏移距离
        LimToHomeDist(0, 7) = 130      '到极限走过原点的距离
        'Y1
        HomeSearchDist(0, 8) = -1000
        HomeOffsetDist(0, 8) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 8) = -40      '到极限走过原点的距离

        'Station 4 Axis X
        HomeSearchDist(1, 1) = -1000
        HomeOffsetDist(1, 1) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 1) = -40      '到极限走过原点的距离
        'Station 4 Axis Y
        HomeSearchDist(1, 2) = -1000
        HomeOffsetDist(1, 2) = -50     '第2次原点搜索偏移距离
        LimToHomeDist(1, 2) = -40      '到极限走过原点的距离
        'Station 4 Axis Z
        HomeSearchDist(1, 3) = -200
        HomeOffsetDist(1, 3) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 3) = -25     '到极限走过原点的距离
        'SM 转盘
        HomeSearchDist(1, 4) = 400
        HomeOffsetDist(1, 4) = 20     '第2次原点搜索偏移距离
        LimToHomeDist(1, 4) = 30      '到极限走过原点的距离
        'Y2
        HomeSearchDist(1, 5) = -1000
        HomeOffsetDist(1, 5) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 5) = -40      '到极限走过原点的距离
        'Z1
        HomeSearchDist(1, 6) = -1000
        HomeOffsetDist(1, 6) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 6) = -60      '到极限走过原点的距离
        'Z2
        HomeSearchDist(1, 7) = -1000
        HomeOffsetDist(1, 7) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 7) = -60      '到极限走过原点的距离

    End Sub

#Region "功能：轴回原点"

    ''' <summary>
    ''' 轴回原点函数
    ''' </summary>
    ''' <param name="Card">卡号：从0开始</param>
    ''' <param name="Axis">轴号：从1开始</param>
    ''' <remarks></remarks>
    Public Sub Motor_Home(ByVal Card As Integer, ByVal Axis As Integer, Optional isRotateAxis As Boolean = False)
        Dim CurrentPos As Double
        Dim CurrentPosEnc As Double
        Dim Status As Long
        Dim TrapPrm As TTrapPrm
        Dim Tpos, Tvel As Double
        Dim rtn As Short

        If AxisHome(Card, Axis).Enable Then
            Select Case HomeStep(Card, Axis)
                Case 0
                    AxisHome(Card, Axis).State = True

                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                    rtn = GT_SetPrfPos(Card, Axis, 0) '将当前轴规划器位置修改为零点
                    rtn = GT_SetEncPos(Card, Axis, 0) '将当前轴编码器位置修改为零点
                    rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1))  '将当前轴进行位置同步

                    rtn = GT_PrfTrap(Card, Axis) '设置当前轴的运动模式为点位模式
                    rtn = GT_GetTrapPrm(Card, Axis, TrapPrm) '获取当前轴点位模式运动参数
                    TrapPrm.acc = AxisPar.acc(Card, Axis)       '载入当前轴的加速度
                    TrapPrm.dec = AxisPar.dec(Card, Axis)      '载入当前轴的减速度
                    rtn = GT_SetTrapPrm(Card, Axis, TrapPrm) '设置当前轴点位模式运动参数
                    rtn = GT_SetCaptureMode(Card, Axis, CAPTURE_HOME) '启动当前轴的原点捕获
                    If isRotateAxis Then
                        Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)     '计算当前轴目标速度脉冲频率（原点搜索速度）
                        Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)  '计算当前轴目标位置脉冲数量（即原点搜索距离）
                    Else
                        Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000)     '计算当前轴目标速度脉冲频率（原点搜索速度）
                        Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))  '计算当前轴目标位置脉冲数量（即原点搜索距离）
                    End If
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点搜索速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点搜索距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                    HomeStep(Card, Axis) = 1

                Case 1
                    rtn = GT_GetCaptureStatus(Card, Axis, HomeCapture(Card, Axis), HomeTempPos(Card, Axis), 1, 0) '获取当前轴原点捕获的状态及捕获的当前位置
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If HomeCapture(Card, Axis) Then   '判断当前轴是否原点捕获触发
                        HomeCapture(Card, Axis) = 0   '当前轴原点捕获触发标志清零
                        HomeStep(Card, Axis) = 2
                    ElseIf CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点搜索距离太小或触发极限）
                        HomeStep(Card, Axis) = 16 '跳转到第16步（移过一段极限到原点的距离再重新搜索）
                    End If

                Case 2
                    rtn = GT_Stop(Card, 2 ^ (Axis - 1), 2 ^ (Axis - 1))  '捕获到原点则当前轴紧急停止
                    HomeStep(Card, Axis) = 3

                Case 3
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                        HomeStep(Card, Axis) = 4
                    End If

                Case 4
                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '获取当前轴当前位置
                    If isRotateAxis Then
                        Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)     '计算当前轴目标速度脉冲频率（第二次搜索原点的偏移速度）
                        Tpos = CurrentPos - (CLng(HomeOffsetDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)) '计算当前轴目标位置脉冲数量（第二次搜索原点的偏移距离）
                    Else
                        Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000)     '计算当前轴目标速度脉冲频率（第二次搜索原点的偏移速度）
                        Tpos = CurrentPos - (CLng(HomeOffsetDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))) '计算当前轴目标位置脉冲数量（第二次搜索原点的偏移距离）
                    End If
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即第二次搜索原点的偏移速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即第二次搜索原点的偏移距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动（运动中进行原点位置修正）
                    HomeStep(Card, Axis) = 5

                Case 5
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 6
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''第二次搜索原点
                Case 6
                    If GetTickCount - HomeTime(Card, Axis) > 50 Then
                        rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                        rtn = GT_SetCaptureMode(Card, Axis, CAPTURE_HOME) '启动当前轴的原点捕获
                        If isRotateAxis Then
                            Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000)) / 10   '计算当前轴目标速度脉冲频率（原点搜索速度）
                            Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))  '计算当前轴目标位置脉冲数量（即原点搜索距离）
                        Else
                            Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000)) / 10   '计算当前轴目标速度脉冲频率（原点搜索速度）
                            Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))  '计算当前轴目标位置脉冲数量（即原点搜索距离）
                        End If
                        rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点搜索速度）
                        rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点搜索距离）
                        rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                        HomeStep(Card, Axis) = 7
                    End If

                Case 7
                    rtn = GT_GetCaptureStatus(Card, Axis, HomeCapture(Card, Axis), HomeTempPos(Card, Axis), 1, 0) '获取当前轴原点捕获的状态及捕获的当前位置
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If HomeCapture(Card, Axis) Then   '判断当前轴是否原点捕获触发
                        HomeCapture(Card, Axis) = 0   '当前轴原点捕获触发标志清零
                        HomeStep(Card, Axis) = 8
                    ElseIf CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点搜索距离太小或触发极限）
                        AxisHome(Card, Axis).Result = False  '当前轴回原点失败(返回结果为FALSE)
                        HomeStep(Card, Axis) = 18 '跳转到第16步（当前轴回原点完成，回原点失败）
                    End If

                Case 8
                    rtn = GT_Stop(Card, 2 ^ (Axis - 1), 0)  '捕获到原点则当前轴平滑停止
                    HomeStep(Card, Axis) = 9

                Case 9
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 10
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''对第二次搜索到的原点信号进行修正（高速硬件捕获锁存位置）
                Case 10
                    If GetTickCount - HomeTime(Card, Axis) > 50 Then
                        rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                        If isRotateAxis Then
                            Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360) / 2   '计算当前轴目标速度脉冲频率（即原点修正速度）
                        Else
                            Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000)) / 2   '计算当前轴目标速度脉冲频率（即原点修正速度）
                        End If
                        Tpos = HomeTempPos(Card, Axis)    '计算当前轴目标位置脉冲数量，即原点修正距离（高速硬件捕获到的原点位置）
                        rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点修正速度）
                        rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点修正距离）
                        rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动（运动中进行原点位置修正）
                        HomeStep(Card, Axis) = 11
                    End If

                Case 11
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点修正完成）
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 12
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''移动固定的原点偏移量
                Case 12
                    If GetTickCount - HomeTime(Card, Axis) > 50 Then
                        If isRotateAxis Then
                            Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360)  '计算目标速度脉冲频率（原点偏移速度）
                            Tpos = HomeTempPos(Card, Axis) + CLng(AxisPar.OrgOffset(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)  '计算目标位置脉冲数量（即原点偏移距离）
                        Else
                            Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000))  '计算目标速度脉冲频率（原点偏移速度）
                            Tpos = HomeTempPos(Card, Axis) + CLng(AxisPar.OrgOffset(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))  '计算目标位置脉冲数量（即原点偏移距离）
                        End If

                        rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点偏移速度）
                        rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点偏移距离）
                        rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                        HomeStep(Card, Axis) = 13
                    End If

                Case 13
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点偏移完成）
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 14
                    End If

                Case 14
                    If GetTickCount - HomeTime(Card, Axis) > 50 Then
                        rtn = GT_SetPrfPos(Card, Axis, 0) '将当前轴规划器位置修改为零点
                        rtn = GT_SetEncPos(Card, Axis, 0) '将当前轴编码器位置修改为零点
                        rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1))  '将当前轴进行位置同步
                        Delay(50)
                        HomeStep(Card, Axis) = 15
                    End If

                Case 15
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '读取0号卡当前轴规划位置
                    rtn = GT_GetEncPos(Card, Axis, CurrentPosEnc, 1, 0) '读取0号卡当前轴实际位置
                    If CurrentPos = 0 And CurrentPosEnc = 0 Then
                        AxisHome(Card, Axis).Result = True  '当前轴回原点成功(返回结果为TRUE)
                        HomeStep(Card, Axis) = 18 '跳转到第18步（当前轴原点复归完成，原点复归成功）
                    Else
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 14 '跳转到14,重新进行位置清0和同步
                    End If

                Case 16
                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志（极限触发停止）
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '获取当前轴当前位置
                    If isRotateAxis Then
                        Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360)  '计算目标速度脉冲频率（极限过原点走过的速度）
                        Tpos = CurrentPos - CLng(LimToHomeDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)   '计算目标位置脉冲数量（极限过原点走过的距离）
                    Else
                        Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000))  '计算目标速度脉冲频率（极限过原点走过的速度）
                        Tpos = CurrentPos - CLng(LimToHomeDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))   '计算目标位置脉冲数量（极限过原点走过的距离）
                    End If
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（极限过原点走过的速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（极限过原点走过的距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                    HomeStep(Card, Axis) = 17

                Case 17
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止（极限过原点走过的距离完成）                                                                          '判断当前轴是否运动停止
                        Delay(50) '极限过原点走过的距离完成延时50ms等待马达停稳
                        If HomeCounter(Card, Axis) = 0 Then   '判断当前轴回原点计数是否等于零
                            HomeCounter(Card, Axis) = HomeCounter(Card, Axis) + 1   '当前轴回原点计数加1
                            HomeStep(Card, Axis) = 6 '回原点计数等于零则回到第5步直接进行第二次回原点搜索
                        Else
                            AxisHome(Card, Axis).Result = False  '当前轴回原点失败(返回结果为FALSE)
                            HomeStep(Card, Axis) = 18   '跳转到下一步（当前轴回原点完成，回原点失败）
                        End If
                    End If
                Case 18
                    AxisHome(Card, Axis).Enable = False       '当前轴回原点循环完成退出
                    AxisHome(Card, Axis).State = False        '回原点完成
                    HomeStep(Card, Axis) = 0
                    HomeCounter(Card, Axis) = 0
                    HomeCapture(Card, Axis) = 0
                    HomeTempPos(Card, Axis) = 0
                Case Else
            End Select
        End If
    End Sub

    ''' <summary>
    ''' R轴回原点
    ''' </summary>
    ''' <param name="Card"></param>
    ''' <param name="Axis"></param>
    ''' <remarks></remarks>
    Public Sub MotorR_Home(ByVal Card As Integer, ByVal Axis As Integer)
        Dim CurrentPos As Double
        Dim CurrentPosEnc As Double
        Dim Status As Long
        Dim TrapPrm As TTrapPrm
        Dim Tpos, Tvel As Double
        Dim rtn As Short

        If AxisHome(Card, Axis).Enable Then
            Select Case HomeStep(Card, Axis)
                Case 0
                    AxisHome(Card, Axis).State = True

                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                    rtn = GT_SetPrfPos(Card, Axis, 0) '将当前轴规划器位置修改为零点
                    rtn = GT_SetEncPos(Card, Axis, 0) '将当前轴编码器位置修改为零点
                    rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1))  '将当前轴进行位置同步

                    rtn = GT_PrfTrap(Card, Axis) '设置当前轴的运动模式为点位模式
                    rtn = GT_GetTrapPrm(Card, Axis, TrapPrm) '获取当前轴点位模式运动参数
                    TrapPrm.acc = AxisPar.acc(Card, Axis)       '载入当前轴的加速度
                    TrapPrm.dec = AxisPar.dec(Card, Axis)      '载入当前轴的减速度
                    rtn = GT_SetTrapPrm(Card, Axis, TrapPrm) '设置当前轴点位模式运动参数
                    rtn = GT_SetCaptureMode(Card, Axis, CAPTURE_HOME) '启动当前轴的原点捕获
                    Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)     '计算当前轴目标速度脉冲频率（原点搜索速度）
                    Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)  '计算当前轴目标位置脉冲数量（即原点搜索距离）
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点搜索速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点搜索距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                    HomeStep(Card, Axis) = 1

                Case 1
                    rtn = GT_GetCaptureStatus(Card, Axis, HomeCapture(Card, Axis), HomeTempPos(Card, Axis), 1, 0) '获取当前轴原点捕获的状态及捕获的当前位置
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If HomeCapture(Card, Axis) Then   '判断当前轴是否原点捕获触发
                        HomeCapture(Card, Axis) = 0   '当前轴原点捕获触发标志清零
                        'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                        HomeStep(Card, Axis) = 2
                    ElseIf CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点搜索距离太小或触发极限）
                        HomeStep(Card, Axis) = 16 '跳转到第16步（移过一段极限到原点的距离再重新搜索）
                    End If
                Case 2
                    rtn = GT_Stop(Card, 2 ^ (Axis - 1), 2 ^ (Axis - 1))  '捕获到原点则当前轴紧急停止
                    'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                    HomeStep(Card, Axis) = 3
                Case 3
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                        'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                        HomeStep(Card, Axis) = 4
                    End If
                Case 4
                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                    rtn = GT_GetEncPos(Card, Axis, CurrentPos, 1, 0) '获取当前轴当前位置
                    Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)     '计算当前轴目标速度脉冲频率（第二次搜索原点的偏移速度）
                    Tpos = CurrentPos - (CLng(HomeOffsetDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)) '计算当前轴目标位置脉冲数量（第二次搜索原点的偏移距离）
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即第二次搜索原点的偏移速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即第二次搜索原点的偏移距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动（运动中进行原点位置修正）
                    'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                    HomeStep(Card, Axis) = 5
                Case 5
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                        Delay(50) '延时50ms等待马达停稳
                        'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                        HomeStep(Card, Axis) = 6
                    End If
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''第二次搜索原点
                Case 6
                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                    rtn = GT_SetCaptureMode(Card, Axis, CAPTURE_HOME) '启动当前轴的原点捕获
                    Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360) / 10   '计算当前轴目标速度脉冲频率（原点搜索速度）
                    Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)  '计算当前轴目标位置脉冲数量（即原点搜索距离）
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点搜索速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点搜索距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                    'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                    HomeStep(Card, Axis) = 7
                Case 7
                    rtn = GT_GetCaptureStatus(Card, Axis, HomeCapture(Card, Axis), HomeTempPos(Card, Axis), 1, 0) '获取当前轴原点捕获的状态及捕获的当前位置
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If HomeCapture(Card, Axis) Then   '判断当前轴是否原点捕获触发
                        HomeCapture(Card, Axis) = 0   '当前轴原点捕获触发标志清零
                        'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                        HomeStep(Card, Axis) = 8
                    ElseIf CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点搜索距离太小或触发极限）
                        AxisHome(Card, Axis).Result = False  '当前轴回原点失败(返回结果为FALSE)
                        HomeStep(Card, Axis) = 18 '跳转到第16步（当前轴回原点完成，回原点失败）
                    End If
                Case 8
                    rtn = GT_Stop(Card, 2 ^ (Axis - 1), 0)  '捕获到原点则当前轴平滑停止
                    'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                    HomeStep(Card, Axis) = 9
                Case 9
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                        HomeTime(Card, Axis) = GetTickCount '延时50ms等待马达停稳
                        HomeStep(Card, Axis) = 10
                    End If


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''对第二次搜索到的原点信号进行修正（高速硬件捕获锁存位置）
                Case 10
                    If GetTickCount - HomeTime(Card, Axis) > 200 Then
                        rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志
                        Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360) / 10   '计算当前轴目标速度脉冲频率（即原点修正速度）
                        Tpos = HomeTempPos(Card, Axis)    '计算当前轴目标位置脉冲数量，即原点修正距离（高速硬件捕获到的原点位置）
                        rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点修正速度）
                        rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点修正距离）
                        rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动（运动中进行原点位置修正）
                        HomeStep(Card, Axis) = 11
                    End If

                Case 11
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点修正完成）
                        HomeTime(Card, Axis) = GetTickCount '延时50ms等待马达停稳
                        HomeStep(Card, Axis) = 12
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''移动固定的原点偏移量
                Case 12
                    If GetTickCount - HomeTime(Card, Axis) > 200 Then
                        Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360)  '计算目标速度脉冲频率（原点偏移速度）
                        Tpos = HomeTempPos(Card, Axis) + CLng(AxisPar.OrgOffset(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)  '计算目标位置脉冲数量（即原点偏移距离）
                        rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（即原点偏移速度）
                        rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（即原点偏移距离）
                        rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                        HomeStep(Card, Axis) = 13
                    End If

                Case 13
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止（原点偏移完成）
                        HomeTime(Card, Axis) = GetTickCount '延时50ms等待马达停稳
                        HomeStep(Card, Axis) = 14
                    End If
                Case 14
                    If GetTickCount - HomeTime(Card, Axis) > 500 Then
                        rtn = GT_SetPrfPos(Card, Axis, 0) '将当前轴规划器位置修改为零点
                        rtn = GT_SetEncPos(Card, Axis, 0) '将当前轴编码器位置修改为零点
                        rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1))  '将当前轴进行位置同步
                        Delay(50)
                        HomeStep(Card, Axis) = 15
                    End If

                Case 15
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '读取0号卡当前轴规划位置
                    rtn = GT_GetEncPos(Card, Axis, CurrentPosEnc, 1, 0) '读取0号卡当前轴实际位置
                    If CurrentPos = 0 And CurrentPosEnc = 0 Then
                        AxisHome(Card, Axis).Result = True  '当前轴回原点成功(返回结果为TRUE)
                        HomeStep(Card, Axis) = 18 '跳转到第18步（当前轴原点复归完成，原点复归成功）
                    Else
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 14 '跳转到14,重新进行位置清0和同步
                    End If
                Case 16
                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志（极限触发停止）
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '获取当前轴当前位置
                    Tvel = (CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) / 360)  '计算目标速度脉冲频率（极限过原点走过的速度）
                    Tpos = CurrentPos - CLng(LimToHomeDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)   '计算目标位置脉冲数量（极限过原点走过的距离）
                    rtn = GT_SetVel(Card, Axis, Tvel) '设置当前轴的目标速度（极限过原点走过的速度）
                    rtn = GT_SetPos(Card, Axis, Tpos) '设置当前轴的目标位置（极限过原点走过的距离）
                    rtn = GT_Update(Card, 2 ^ (Axis - 1)) '启动当前轴点位运动
                    'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步
                    HomeStep(Card, Axis) = 17
                Case 17
                    rtn = GT_GetSts(Card, Axis, Status, 1, 0) '获取当前轴的状态
                    If CBool(Status And &H400) = False Then '判断当前轴是否运动停止（极限过原点走过的距离完成）                                                                          '判断当前轴是否运动停止
                        Delay(50) '极限过原点走过的距离完成延时50ms等待马达停稳
                        If HomeCounter(Card, Axis) = 0 Then   '判断当前轴回原点计数是否等于零
                            HomeCounter(Card, Axis) = HomeCounter(Card, Axis) + 1   '当前轴回原点计数加1
                            HomeStep(Card, Axis) = 6 '回原点计数等于零则回到第5步直接进行第二次回原点搜索
                        Else
                            AxisHome(Card, Axis).Result = False  '当前轴回原点失败(返回结果为FALSE)
                            'HomeStep(Card, Axis) = HomeStep(Card, Axis) + 1 '跳转到下一步（当前轴回原点完成，回原点失败）
                            HomeStep(Card, Axis) = 18
                        End If
                    End If
                Case 18
                    AxisHome(Card, Axis).Enable = False       '当前轴回原点循环完成退出
                    AxisHome(Card, Axis).State = False        '回原点完成
                    HomeStep(Card, Axis) = 0
                    HomeCounter(Card, Axis) = 0
                    HomeCapture(Card, Axis) = 0
                    HomeTempPos(Card, Axis) = 0
                Case Else
            End Select
        End If
    End Sub

#End Region

#Region "功能：读编码器值"
    Public Function GetEncPos(CardNum As Integer, Axis As Integer) As Long
        Dim rtn As Short
        Dim dEncPosTmpl As Double
        rtn = GT_GetEncPos(CardNum, Axis, dEncPosTmpl, 1, 0)
        GetEncPos = CLng(dEncPosTmpl)
    End Function
#End Region

#Region "功能：获取原点状态"
    Public Function HomeBit(CardNum As Integer, Axis As Integer) As Integer
        Dim lTmplData As Long
        Dim rtn As Short

        rtn = GT_GetDi(CardNum, MC_HOME, lTmplData)
        If lTmplData And 2 ^ (Axis - 1) Then
            Return 1
        Else
            Return 0
        End If
    End Function
#End Region


    ' ''' <summary>
    ' ''' 获取0号卡各轴运动状态   轴号从1开始
    ' ''' </summary>
    ' ''' <param name="Axis"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function isAxisMoving(0,ByVal Axis As Integer) As Boolean
    '    Dim Status As Long
    '    Dim TempPos(1) As Double
    '    Dim i As Long
    '    Dim rtn As Short
    '    Dim flag_Moving As Boolean

    '    rtn = GT_GetSts(0, Axis, Status, 1, 0)

    '    If Flag_MachineInitOngoing Then
    '        i = 2000
    '    Else
    '        i = 200
    '    End If

    '    flag_Moving = True
    '    If CBool(Status And &H400) = False Then
    '        rtn = GT_GetEncPos(0, Axis, TempPos(0), 1, 0)
    '        rtn = GT_GetPrfPos(0, Axis, TempPos(1), 1, 0)
    '        If Abs(TempPos(0) - TempPos(1)) < i Then
    '            flag_Moving = False
    '        Else
    '            If GetTickCount() - AxisTime(0, Axis) >= 5000 Then
    '                'Call Form_Main.MacStop()
    '                'Form_Main.List1(0).AddItem AxisName0_2(Axis) & "运动异常"
    '                'Call WriteErrLog(0, AxisName0_2(Axis) & "伺服电机运动目标位置与编码位置相差过大", "A0001")
    '                'Call WriteDowntime_Data(0, "Red", "A0001", "Motor move to set lucation and encode lucation abnormal", "AE:Contact BZ")
    '                'Form_Main.List1(0).AddItem "重新初始，复位卡"
    '                'Form_Main.List1(0).AddItem "消息时间 " & Format(TIME, "HH：MM：SS")
    '                flag_Moving = True
    '            End If
    '        End If
    '    Else
    '        AxisTime(0, Axis) = GetTickCount()
    '        flag_Moving = True
    '    End If
    '    Return flag_Moving
    'End Function

    ' ''' <summary>
    ' ''' 获取1号卡各轴运动状态   轴号从1开始
    ' ''' </summary>
    ' ''' <param name="Axis"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function isAxisMoving(1,(ByVal Axis As Integer) As Boolean
    '    Dim Status As Long
    '    Dim TempPos(1) As Double
    '    Dim i As Long
    '    Dim rtn As Short
    '    Dim flag_Moving As Boolean

    '    rtn = GT_GetSts(1, Axis, Status, 1, 0)

    '    If Flag_MachineInitOngoing Then
    '        i = 2000
    '    Else
    '        i = 200
    '    End If

    '    flag_Moving = True
    '    If CBool(Status And &H400) = False Then
    '        rtn = GT_GetEncPos(1, Axis, TempPos(0), 1, 0)
    '        rtn = GT_GetPrfPos(1, Axis, TempPos(1), 1, 0)
    '        If Abs(TempPos(0) - TempPos(1)) < i Then
    '            flag_Moving = False
    '        Else
    '            If GetTickCount() - AxisTime(1, Axis) >= 5000 Then
    '                'Call Form_Main.MacStop()
    '                'Form_Main.List1(0).AddItem AxisName0_2(Axis) & "运动异常"
    '                'Call WriteErrLog(0, AxisName0_2(Axis) & "伺服电机运动目标位置与编码位置相差过大", "A0001")
    '                'Call WriteDowntime_Data(0, "Red", "A0001", "Motor move to set lucation and encode lucation abnormal", "AE:Contact BZ")
    '                'Form_Main.List1(0).AddItem "重新初始，复位卡"
    '                'Form_Main.List1(0).AddItem "消息时间 " & Format(TIME, "HH：MM：SS")
    '                flag_Moving = True
    '            End If
    '        End If
    '    Else
    '        AxisTime(1, Axis) = GetTickCount()
    '        flag_Moving = True
    '    End If
    '    Return flag_Moving

    'End Function

    Public Function GetDi(ByRef CardNum As Short, ByRef i As Short) As Short ''''''''''''''''''''''''''''
        Dim CardExI As Integer
        Dim rtn As Short
        rtn = GT_GetDi(CardNum, MC_GPI, CardExI)
        If CardExI And 2 ^ i Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Public Function GetDo(ByRef CardNum As Short, ByRef i As Short) As Short '''''''''''''''''''''''''''''
        Dim CardExO As Integer
        Dim rtn As Short
        rtn = GT_GetDo(CardNum, MC_GPO, CardExO)
        If CardExO And 2 ^ i Then
            Return 0
        Else
            Return 1
        End If
    End Function

    ''' <summary>
    ''' 判断轴是否在运动中
    ''' </summary>
    ''' <param name="card">卡号：从0开始</param>
    ''' <param name="axis">轴号：从1开始</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isAxisMoving(ByVal card As Short, ByVal axis As Integer) As Boolean
        Dim Status As Long
        Dim TempPos(1) As Double
        Dim i As Long
        Dim rtn As Short
        Dim flag_Moving As Boolean

        i = 200
        rtn = GT_GetSts(card, Axis, Status, 1, 0)
        flag_Moving = True
        If CBool(Status And &H400) = False Then
            rtn = GT_GetEncPos(card, Axis, TempPos(0), 1, 0)
            rtn = GT_GetPrfPos(card, Axis, TempPos(1), 1, 0)
            If Abs(TempPos(0) - TempPos(1)) < i Then
                flag_Moving = False
            Else
                If GetTickCount() - AxisTime(card, Axis) >= 5000 Then
                    'Call Form_Main.MacStop()
                    'Form_Main.List1(0).AddItem AxisName0_2(Axis) & "运动异常"
                    'Call WriteErrLog(0, AxisName0_2(Axis) & "伺服电机运动目标位置与编码位置相差过大", "A0001")
                    'Call WriteDowntime_Data(0, "Red", "A0001", "Motor move to set lucation and encode lucation abnormal", "AE:Contact BZ")
                    'Form_Main.List1(0).AddItem "重新初始，复位卡"
                    'Form_Main.List1(0).AddItem "消息时间 " & Format(TIME, "HH：MM：SS")
                    flag_Moving = True
                End If
            End If
        Else
            AxisTime(card, Axis) = GetTickCount()
            flag_Moving = True
        End If
        Return flag_Moving
    End Function

    ''' <summary>
    ''' JOG连续运动模式子程序
    ''' </summary>
    ''' <param name="Card">卡号从0开始</param>
    ''' <param name="Axis">轴号从1开始</param>
    ''' <param name="Direction">方向：1正向，-1负方向</param>
    ''' <param name="JOG_VEL">速度</param>
    ''' <remarks></remarks>
    Public Sub JogMotion(ByVal Card As Integer, ByVal Axis As Integer, ByVal Direction As Long, ByVal JOG_VEL As Short)
        Dim Vel As Double
        Dim JogPrm As TJogPrm
        Dim rtn As Short

        rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
        rtn = GT_PrfJog(Card, Axis)               '将当前轴设置为JOG运动模式
        rtn = GT_GetJogPrm(Card, Axis, JogPrm)    '读取当前轴点位模式运动参数
        JogPrm.acc = AxisPar.acc(Card, Axis)        '载入当前轴的加速度
        JogPrm.dec = AxisPar.dec(Card, Axis)          '载入当前轴的减速度
        rtn = GT_SetJogPrm(Card, Axis, JogPrm)    '设置当前轴加速度、减速度、起跳速度、平滑时间
        Select Case Card
            Case 0
                Select Case Axis
                    Case 1, 2, 3, 4, 5, 7, 8
                        Vel = CDbl(JOG_VEL * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                    Case Else
                        Vel = CDbl(JOG_VEL * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360) '(*计算目标速度脉冲频率*)
                End Select
            Case 1
                Select Case Axis
                    Case 2, 3, 4, 5, 6, 7, 8
                        Vel = CDbl(JOG_VEL * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                    Case Else
                        Vel = CDbl(JOG_VEL * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360) '(*计算目标速度脉冲频率*)
                End Select
            Case 2
                Vel = CDbl(JOG_VEL * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
        End Select

        rtn = GT_SetVel(Card, Axis, Vel * Direction)  '设置当前轴的目标速度
        rtn = GT_Update(Card, 2 ^ (Axis - 1))   '启动当前轴运动
    End Sub

    ''' <summary>
    ''' STEP单步运动模式子程序
    ''' </summary>
    ''' <param name="Card">卡号从0开始</param>
    ''' <param name="Axis">轴号从1开始</param>
    ''' <param name="Direction">方向：1正向，-1负方向</param>
    ''' <param name="Tempvel">速度</param>
    ''' <param name="TempPos">移动距离</param>
    ''' <remarks></remarks>
    Public Sub StepMotion(ByVal Card As Integer, ByVal Axis As Integer, ByVal Direction As Short, ByVal Tempvel As Double, ByVal TempPos As Double)
        Dim Pos, Vel As Double
        Dim TrapPrm As TTrapPrm
        Dim CurrentPos As Long
        Dim rtn As Short

        rtn = GT_ClrSts(Card, Axis)                 '清除当前轴的错误标志
        rtn = GT_PrfTrap(Card, Axis)                '将当前轴设置为点位运动模式
        rtn = GT_GetTrapPrm(Card, Axis, TrapPrm)    '读取当前轴点位模式运动参数
        TrapPrm.acc = AxisPar.acc(Card, Axis)            '载入当前轴的加速度
        TrapPrm.dec = AxisPar.dec(Card, Axis)            '载入当前轴的减速度
        rtn = GT_SetTrapPrm(Card, Axis, TrapPrm)    '设置当前轴加速度、减速度、起跳速度、平滑时间
        rtn = GT_GetPos(Card, Axis, CurrentPos)     '获取当前轴当前位置

        Select Case Card
            Case 0
                Select Case Axis
                    Case 1, 2, 3, 4, 5, 7, 8
                        Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis)) * Direction     '(*计算目标位置脉冲数量*)
                    Case 7
                        Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360) * Direction        '(*计算目标位置脉冲数量*)
                End Select
            Case 1
                Select Case Axis
                    Case 2, 3, 4, 5, 6, 7, 8
                        Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis)) * Direction     '(*计算目标位置脉冲数量*)
                    Case 4
                        Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360) * Direction        '(*计算目标位置脉冲数量*)
                End Select
            Case 2
                Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis)) * Direction     '(*计算目标位置脉冲数量*)
        End Select
        'Delay(500)
        rtn = GT_SetVel(Card, Axis, Vel)    '设置当前轴的目标速度
        rtn = GT_SetPos(Card, Axis, Pos)    '设置当前轴的目标位置
        'Delay(20)
        rtn = GT_Update(Card, 2 ^ (Axis - 1))         '启动当前轴运动
    End Sub

    'ABS绝对运动模式子程序，参数1：卡号；参数2：轴号；参数3：速度；参数4：相对于原点的位移距离
    ''' <summary>
    ''' 轴走绝对运动
    ''' </summary>
    ''' <param name="Card">卡号:从0开始</param>
    ''' <param name="Axis">轴号：从0开始</param>
    ''' <param name="Speed">速度:mm/s</param>
    ''' <param name="Dist">相对于原点的位移距离</param>
    ''' <remarks></remarks>
    Public Sub AbsMotion(ByVal Card As Integer, ByVal Axis As Integer, ByVal Speed As Double, ByVal Dist As Double)
        Dim TempPos, TempVel As Double
        Dim Pos, Vel As Double
        Dim TrapPrm As TTrapPrm
        Dim rtn As Short

        TempVel = Speed
        TempPos = Dist
        rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
        rtn = GT_PrfTrap(Card, Axis)              '将当前轴设置为点位运动模式
        rtn = GT_GetTrapPrm(Card, Axis, TrapPrm)  '读取当前轴点位模式运动参数

        TrapPrm.acc = AxisPar.acc(Card, Axis)       '载入当前轴的加速度
        TrapPrm.dec = AxisPar.dec(Card, Axis)       '载入当前轴的减速度

        rtn = GT_SetTrapPrm(Card, Axis, TrapPrm)    '设置当前轴加速度、减速度、起跳速度、平滑时间

        Select Case Card
            Case 0
                Select Case Axis
                    Case 1, 2, 3, 4, 5, 7, 8
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
                    Case 6
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)          '(*计算目标位置脉冲数量*)
                End Select
            Case 1
                Select Case Axis
                    Case 2, 3, 4, 5, 6, 7, 8
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
                    Case 1
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)          '(*计算目标位置脉冲数量*)
                End Select
            Case 2
                Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
        End Select
        rtn = GT_SetVel(Card, Axis, Vel)      '设置当前轴的目标速度
        rtn = GT_SetPos(Card, Axis, Pos)      '设置当前轴的目标位置
        rtn = GT_Update(Card, 2 ^ (Axis - 1))   '启动当前轴运动

        AxisTime(Card, Axis) = GetTickCount()
    End Sub

    'REL相对运动模式子程序，参数1：轴号；参数2：速度；参数3：相对于当前点的位移距离
    Public Sub RelMotion(ByVal Card As Integer, ByVal Axis As Integer, ByVal Speed As Double, ByVal Dist As Double)
        Dim TempPos, TempVel As Double
        Dim Pos, Vel As Double
        Dim TrapPrm As TTrapPrm
        Dim CurrentPos As Long
        Dim rtn As Short

        TempVel = Speed
        TempPos = Dist
        rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
        rtn = GT_PrfTrap(Card, Axis)              '将当前轴设置为点位运动模式
        rtn = GT_GetTrapPrm(Card, Axis, TrapPrm)  '读取当前轴点位模式运动参数

        TrapPrm.acc = AxisPar.acc(Card, Axis)       '载入当前轴的加速度
        TrapPrm.dec = AxisPar.dec(Card, Axis)       '载入当前轴的减速度

        rtn = GT_SetTrapPrm(Card, Axis, TrapPrm)    '设置当前轴加速度、减速度、起跳速度、平滑时间
        rtn = GT_GetPos(Card, Axis, CurrentPos)   '获取当前轴当前位置
        Select Case Card
            Case 0
                Select Case Axis
                    Case 1, 2, 3, 4, 5, 7, 8
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))      '(*计算目标位置脉冲数量*)
                    Case Else
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)          '(*计算目标位置脉冲数量*)
                End Select
            Case 1
                Select Case Axis
                    Case 2, 3, 4, 5, 6, 7, 8
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))        '(*计算目标位置脉冲数量*)
                    Case Else
                        Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)          '(*计算目标位置脉冲数量*)
                End Select
            Case 2
                Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))        '(*计算目标位置脉冲数量*)
        End Select
        rtn = GT_SetVel(Card, Axis, Vel)      '设置当前轴的目标速度
        rtn = GT_SetPos(Card, Axis, Pos)      '设置当前轴的目标位置
        rtn = GT_Update(Card, 2 ^ (Axis - 1))   '启动当前轴运动

        AxisTime(Card, Axis) = GetTickCount()
    End Sub

    ''' <summary>
    ''' 紧急停止
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub EMC_Stop()
        Static Once As Boolean
        Dim rtn As Short

        '判断是否触发急停
        If EXI(0, 15) = False Or EXI(1, 15) = False Or EXI(2, 15) = False Then
            IsSysEmcStop = True
            If Once = False Then
                Once = True

                For n = 0 To GTS_CardNum - 1
                    rtn = GT_Stop(n, 255, 255)  '紧急停止所有轴
                Next n

                '//轴伺服OFF
                For n = 0 To GTS_CardNum - 1
                    For i = 1 To GTS_AxisNum(n)
                        rtn = GT_ClrSts(n, i, 1)  '清除当前轴报警标志
                        rtn = GT_AxisOff(n, i) '当前轴伺服OFF
                    Next i
                Next n
                Frm_DialogAddMessage("紧急停止被触发，请检查!")
                Call Frm_Main.Machine_Stop()
                Frm_Main.Timer_MacInit.Enabled = False
                Frm_Main.Timer_AutoRun.Enabled = False
                'Flag_MachineInit = False
                Flag_MachineAutoRun = False
                Frm_Main.SetMachine(0)
            End If
        Else
            IsSysEmcStop = False
            Once = False
        End If
    End Sub


End Module
