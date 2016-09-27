Imports System.Math

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
                        If MACTYPE = "PAM-B" Then
                            rtn = GT_LoadConfig(n, Application.StartupPath & "\GTS_Config\GTS800_B.cfg")          '下载控制卡1配置参数
                        Else
                            rtn = GT_LoadConfig(n, Application.StartupPath & "\GTS_Config\GTS800_1.cfg")          '下载控制卡1配置参数
                        End If 
                    ElseIf n = 2 Then
                        '复位运动控制卡1
                        rtn = GT_LoadConfig(n, Application.StartupPath & "\GTS_Config\GTS400.cfg")          '下载控制卡2配置参数
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
        'Glue X
        HomeSearchDist(0, 1) = -1000
        HomeOffsetDist(0, 1) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 1) = -40      '到极限走过原点的距离
        'Glue Y
        HomeSearchDist(0, 2) = -1000
        HomeOffsetDist(0, 2) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 2) = -40      '到极限走过原点的距离
        'Glue Z
        HomeSearchDist(0, 3) = -1000
        HomeOffsetDist(0, 3) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(0, 3) = -30      '到极限走过原点的距离
        'Paste X
        HomeSearchDist(0, 4) = -1000
        HomeOffsetDist(0, 4) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(0, 4) = -80      '到极限走过原点的距离
        'Paste Z
        HomeSearchDist(0, 5) = -1000
        HomeOffsetDist(0, 5) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(0, 5) = -30      '到极限走过原点的距离
        'Paste R
        HomeSearchDist(0, 6) = -360
        HomeOffsetDist(0, 6) = -20     '第2次原点搜索偏移距离
        LimToHomeDist(0, 6) = -100    '到极限走过原点的距离
        'PreTaker X
        HomeSearchDist(0, 7) = 1000
        HomeOffsetDist(0, 7) = 4     '第2次原点搜索偏移距离
        LimToHomeDist(0, 7) = 40      '到极限走过原点的距离
        'PreTaker Z
        HomeSearchDist(0, 8) = -1000
        HomeOffsetDist(0, 8) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(0, 8) = -30      '到极限走过原点的距离

        'PreTaker R
        HomeSearchDist(1, 1) = -360
        HomeOffsetDist(1, 1) = -20     '第2次原点搜索偏移距离
        LimToHomeDist(1, 1) = -100      '到极限走过原点的距离

        'Cure X
        HomeSearchDist(1, 2) = -1000
        HomeOffsetDist(1, 2) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(1, 2) = -40      '到极限走过原点的距离

        'Fine X
        HomeSearchDist(1, 3) = -1000
        HomeOffsetDist(1, 3) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(1, 3) = -25     '到极限走过原点的距离

        'Fine Y
        HomeSearchDist(1, 4) = -1000
        HomeOffsetDist(1, 4) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 4) = -40     '到极限走过原点的距离

        'Reheck X
        HomeSearchDist(1, 5) = -1000
        HomeOffsetDist(1, 5) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(1, 5) = -40      '到极限走过原点的距离

        'Reheck Y
        HomeSearchDist(1, 6) = -1000
        HomeOffsetDist(1, 6) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(1, 6) = -40     '到极限走过原点的距离

        'Z1
        HomeSearchDist(1, 7) = -1000
        HomeOffsetDist(1, 7) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 7) = -60      '到极限走过原点的距离

        'Z2
        HomeSearchDist(1, 8) = -1000
        HomeOffsetDist(1, 8) = -10     '第2次原点搜索偏移距离
        LimToHomeDist(1, 8) = -60      '到极限走过原点的距离

        'Paste Y1
        HomeSearchDist(2, 1) = -1000
        HomeOffsetDist(2, 1) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(2, 1) = -40     '到极限走过原点的距离

        'Paste Y2
        HomeSearchDist(2, 2) = -1000
        HomeOffsetDist(2, 2) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(2, 2) = -40      '到极限走过原点的距离

        'PreTaker Y1
        HomeSearchDist(2, 3) = -1000
        HomeOffsetDist(2, 3) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(2, 3) = -30     '到极限走过原点的距离

        'PreTaker Y2
        HomeSearchDist(2, 4) = -1000
        HomeOffsetDist(2, 4) = -2     '第2次原点搜索偏移距离
        LimToHomeDist(2, 4) = -40     '到极限走过原点的距离

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
                            Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)     '计算当前轴目标速度脉冲频率（原点搜索速度）
                            Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360) '计算当前轴目标位置脉冲数量（即原点搜索距离）
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
                    If GetTickCount - HomeTime(Card, Axis) > 2500 Then
                        rtn = GT_SetPrfPos(Card, Axis, 0) '将当前轴规划器位置修改为零点
                        rtn = GT_SetEncPos(Card, Axis, 0) '将当前轴编码器位置修改为零点
                        rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1))  '将当前轴进行位置同步
                        Delay(100)
                        HomeStep(Card, Axis) = 15
                    End If

                Case 15
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '读取0号卡当前轴规划位置
                    rtn = GT_GetEncPos(Card, Axis, CurrentPosEnc, 1, 0) '读取0号卡当前轴实际位置
                    If Math.Abs(CurrentPos) < 10 And Math.Abs(CurrentPosEnc) < 10 Then
                        AxisHome(Card, Axis).Result = True  '当前轴回原点成功(返回结果为TRUE)
                        HomeStep(Card, Axis) = 18 '跳转到第18步（当前轴原点复归完成，原点复归成功）
                    Else
                        HomeTime(Card, Axis) = GetTickCount
                        HomeStep(Card, Axis) = 14 '跳转到14,重新进行位置清0和同步
                    End If

                Case 16
                    rtn = GT_ClrSts(Card, Axis, 1) '清除当前轴驱动器报警标志（极限触发停止）
                    rtn = GT_GetEncPos(Card, Axis, CurrentPos, 1, 0) '获取当前轴当前位置
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
        rtn = GT_GetSts(card, axis, Status, 1, 0)
        flag_Moving = True
        If CBool(Status And &H400) = False Then

            rtn = GT_GetEncPos(card, axis, TempPos(0), 1, 0)
            rtn = GT_GetPrfPos(card, axis, TempPos(1), 1, 0)


            If Abs(TempPos(0) - TempPos(1)) < i Then
                flag_Moving = False
            Else
                If GetTickCount() - AxisTime(card, axis) >= 5000 Then
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
            AxisTime(card, axis) = GetTickCount()
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
                    Case 6
                        Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360) * Direction        '(*计算目标位置脉冲数量*)
                End Select
            Case 1
                Select Case Axis
                    Case 2, 3, 4, 5, 6, 7, 8

                        Vel = CDbl(Tempvel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
                        Pos = CurrentPos + CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis)) * Direction     '(*计算目标位置脉冲数量*)
                  
                    Case Else
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
    'Public Sub AbsMotion(ByVal Card As Integer, ByVal Axis As Integer, ByVal Speed As Double, ByVal Dist As Double)
    '    Dim TempPos, TempVel As Double
    '    Dim Pos, Vel As Double
    '    Dim TrapPrm As TTrapPrm
    '    Dim rtn As Short

    '    TempVel = Speed
    '    TempPos = Dist
    '    rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
    '    rtn = GT_PrfTrap(Card, Axis)              '将当前轴设置为点位运动模式
    '    rtn = GT_GetTrapPrm(Card, Axis, TrapPrm)  '读取当前轴点位模式运动参数

    '    TrapPrm.acc = AxisPar.acc(Card, Axis)       '载入当前轴的加速度
    '    TrapPrm.dec = AxisPar.dec(Card, Axis)       '载入当前轴的减速度

    '    rtn = GT_SetTrapPrm(Card, Axis, TrapPrm)    '设置当前轴加速度、减速度、起跳速度、平滑时间

    '    Select Case Card
    '        Case 0
    '            Select Case Axis
    '                Case 1, 2, 3, 4, 5, 7, 8
    '                    Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
    '                    Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
    '                Case 6
    '                    Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
    '                    Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)          '(*计算目标位置脉冲数量*)
    '            End Select
    '        Case 1
    '            Select Case Axis
    '                Case 2, 3, 4, 5, 6, 7, 8
    '                    Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
    '                    Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
    '                Case 1
    '                    Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)   '(*计算目标速度脉冲频率*)
    '                    Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360)          '(*计算目标位置脉冲数量*)
    '            End Select
    '        Case 2
    '            Vel = CDbl(TempVel * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000) '(*计算目标速度脉冲频率*)
    '            Pos = CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
    '    End Select
    '    rtn = GT_SetVel(Card, Axis, Vel)      '设置当前轴的目标速度
    '    rtn = GT_SetPos(Card, Axis, Pos)      '设置当前轴的目标位置
    '    rtn = GT_Update(Card, 2 ^ (Axis - 1))   '启动当前轴运动

    '    AxisTime(Card, Axis) = GetTickCount()
    'End Sub


    Private LockedPaste As Boolean
    Private LockedTake As Boolean
    Public Function AbsMotion(ByVal Card As Integer, ByVal Axis As Integer, ByVal Speed As Double, ByVal Dist As Double) As Boolean
        Dim TempPos, TempVel As Double
        Dim Pos, Vel As Double
        Dim TrapPrm As TTrapPrm
        Dim rtn As Short

        Dim PasetY_SafeLimet As Double
        Dim TakeY_SafeLimet As Double

        AbsMotion = False

        ''龙门Y轴会有撞机风险
        'If Card = 2 Then
        '    PasetY_SafeLimet = Par_Pos.St_Paste(14).Y
        '    TakeY_SafeLimet = Par_Pos.St_PreTaker(9).Y

        '    Select Case Axis
        '        Case 1, 2
        '            '判断安全锁
        '            If LockedTake = True Then Return False

        '            '先判断运行方向，如果朝+方向运行，则继续向下判断
        '            If Dist - CurrEncPos(2, PasteY1) > 0 Or Dist - CurrEncPos(2, PasteY2) > 0 Then
        '                '再判断运动的目的地，如果超过安全区域，则往下继续判断
        '                If Dist > PasetY_SafeLimet Then
        '                    '最后判断对方轴是在安全区域以内并且停止中
        '                    If (Not isAxisMoving(2, PreTakerY1)) And (CurrEncPos(2, PreTakerY1) <= TakeY_SafeLimet) And (CurrEncPos(2, PreTakerY2) <= TakeY_SafeLimet) Then
        '                        LockedPaste = True
        '                    Else
        '                        Return False
        '                    End If
        '                End If
        '            End If

        '        Case 3, 4
        '            '判断安全锁
        '            If LockedPaste = True Then Return False

        '            '先判断运行方向，如果朝+方向运行，则继续向下判断
        '            If Dist - CurrEncPos(2, PreTakerY1) > 0 Or Dist - CurrEncPos(2, PreTakerY2) > 0 Then
        '                '再判断运动的目的地，如果超过安全区域，则往下继续判断
        '                If Dist > TakeY_SafeLimet Then
        '                    '最后判断对方轴是在安全区域以内并且停止中
        '                    If (Not isAxisMoving(2, PasteY1)) And (CurrEncPos(2, PasteY1) <= PasetY_SafeLimet) And (CurrEncPos(2, PasteY2) <= PasetY_SafeLimet) Then
        '                        LockedTake = True
        '                    Else
        '                        Return False
        '                    End If
        '                End If
        '            End If

        '        Case Else
        '            Return False
        '    End Select
        'End If

        TempVel = Speed
        TempPos = Dist

        If Card = 2 And (Axis = 1 Or Axis = 3) Then
            rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
            rtn = GT_ClrSts(Card, Axis + 1)               '清除当前轴的错误标志
        Else
            rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
        End If
         
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

        'If Card = 2 And Axis = 1 And rtn = 0 Then MessageBox.Show(Pos & "," & Vel & "," & TrapPrm.acc)

        AxisTime(Card, Axis) = GetTickCount()

        '解锁
        If Card = 2 Then
            Select Case Axis
                Case 1, 2
                    LockedPaste = False
                Case 3, 4
                    LockedTake = False
            End Select
        End If

        Return True
    End Function


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
                Frm_Engineering.Btn_initialize.Text = "初始化"
                'Btn_Start.BZ_Color = Color_Unselected
                'Btn_Pause.BZ_Color = Color_Unselected
                'Btn_Stop.BZ_Color = Color_Red

                Frm_Engineering.Btn_initialize.Enabled = True
                Frm_Engineering.Btn_initialize.BZ_Color = Color_Unselected
                Frm_Main.Timer_MacInit.Enabled = False
                Frm_Main.Timer_AutoRun.Enabled = False
                Flag_MachineInit = False
                Flag_MachineAutoRun = False
                Frm_Main.SetMachine(0)
            End If
        Else
            IsSysEmcStop = False
            Once = False
        End If
    End Sub

    ''' <summary>
    ''' 直线插补用到的结构体
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure sLnXYZR
        Dim cardNo As Short
        Dim crd As Short
        Dim X As Integer
        Dim Y As Integer
        Dim Z As Integer
        Dim R As Integer
        Dim synVel As Double
        Dim synAcc As Double
        Dim velEnd As Double
        Dim fifo As Short
    End Structure

    ''' <summary>
    ''' 直线插补运动
    ''' </summary>
    ''' <param name="cardNum">卡号</param>
    ''' <param name="ArrayXY">走的直线的坐标数组</param>
    ''' <param name="Speed">速度</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LineXY_Motion(ByVal cardNum As Short, ArrayXY() As Dist_XY, Speed As Double) As Boolean
        Dim i As Integer
        Dim n As Integer
        Dim crdprm As TCrdPrm   '声明坐标系结构体参数
        Dim cLnXY As sLnXYZR
        Dim rtn As Short
        n = ArrayXY.Count
        With crdprm             '设置坐标系参数
            .dimension = 2     '指定坐标系维数为2(X、Y)
            .synVelMax = 1000   '该坐标系最大合成速度(Pulse/ms)
            .synAccMax = 3      '该坐标系最大合成加速度(Pulse/ms^2)
            .evenTime = 50      '每个插补段的最小匀速段时间(ms)
            .profile1 = 1       '坐标系X维映射相应的规划轴
            .profile2 = 2       '坐标系Y维映射相应的规划轴
            .setOriginFlag = 1  '需要明确指定坐标系原点的位置(为0则用当前规划位置作为坐标系的原点)
            .originPos1 = 0     '指定X维坐标系原点(为0则和机械原点重合)
            .originPos2 = 0     '指定Y维坐标系原点(为0则和机械原点重合)
        End With
        rtn = GT_SetCrdPrm(cardNum, 1, crdprm)  '建立坐标系1
        If rtn <> 0 Then    '判断坐标系建立是否成功
            Return False
        End If

        With cLnXY
            .cardNo = cardNum      '指定坐标系的卡号
            .crd = 1                '指定坐标系号
            .synVel = Speed         '合成目标速度
            .synAcc = 0.5      '合成加速度
            .velEnd = Speed    '终点速度
            .fifo = 0               '缓冲区选择
        End With

        rtn = GT_CrdClear(cLnXY.cardNo, cLnXY.crd, cLnXY.fifo)    '清除FIFO

        For i = 0 To n - 2 '向FIFO循环压入插补运动数据
            rtn = GT_LnXY(cLnXY.cardNo, cLnXY.crd, ArrayXY(i).X * 1000, ArrayXY(i).Y * 1000, cLnXY.synVel, cLnXY.synAcc, cLnXY.velEnd, cLnXY.fifo)
            If rtn <> 0 Then
                Return False
            End If
        Next i
        rtn = GT_LnXY(cLnXY.cardNo, cLnXY.crd, ArrayXY(n - 1).X * 1000, ArrayXY(n - 1).Y * 1000, Speed, cLnXY.synAcc, 0, cLnXY.fifo)
        If rtn <> 0 Then
            Return False
        End If

        rtn = GT_CrdStart(cLnXY.cardNo, cLnXY.crd, cLnXY.fifo)   '启动坐标系1的2维直线插补运动
        If rtn <> 0 Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' 平面二维圆弧插补运动(以圆弧的终点坐标及半径描述的圆弧)
    ''' </summary>
    ''' <param name="cardNum">卡号</param>
    ''' <param name="ArrayXY">圆弧终点坐标</param>
    ''' <param name="Radius">半径</param>
    ''' <param name="Speed">合成速度</param>
    ''' <param name="Dir">方向：0-顺时针圆弧；1-逆时针圆弧</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ArcXY_Motion(ByVal cardNum As Short, ByVal ArrayXY As Dist_XY, ByVal Radius As Double, ByVal Speed As Double, ByVal Dir As Short) As Boolean
        Dim crdprm As TCrdPrm   '声明坐标系结构体参数
        Dim cLnXY As sLnXYZR
        Dim rtn As Short

        With crdprm             '设置坐标系参数
            .dimension = 2     '指定坐标系维数为2
            .synVelMax = 1000   '该坐标系最大合成速度(Pulse/ms)
            .synAccMax = 0.5      '该坐标系最大合成加速度(Pulse/ms^2)
            .evenTime = 50      '每个插补段的最小匀速段时间(ms)
            .profile1 = 1       '坐标系X维映射相应的规划轴
            .profile2 = 2       '坐标系Y维映射相应的规划轴
            .setOriginFlag = 1  '需要明确指定坐标系原点的位置(为0则用当前规划位置作为坐标系的原点)
            .originPos1 = 0     '指定X维坐标系原点(为0则和机械原点重合)
            .originPos2 = 0     '指定Y维坐标系原点(为0则和机械原点重合)
        End With
        rtn = GT_SetCrdPrm(cardNum, 1, crdprm)  '建立坐标系1
        If rtn <> 0 Then    '判断坐标系建立是否成功
            Return False
        End If

        With cLnXY
            .cardNo = cardNum      '指定坐标系的卡号
            .crd = 1                '指定坐标系号
            .synVel = Speed         '合成目标速度
            .synAcc = 0.5     '合成加速度
            .velEnd = 0    '终点速度
            .fifo = 0               '缓冲区选择
        End With

        rtn = GT_CrdClear(cLnXY.cardNo, cLnXY.crd, cLnXY.fifo)    '清除FIFO
        If rtn <> 0 Then
            Return False
        End If
        rtn = GT_ArcXYR(cLnXY.cardNo, cLnXY.crd, ArrayXY.X * 1000, ArrayXY.Y * 1000, Radius, Dir, Speed, cLnXY.synAcc, cLnXY.velEnd, cLnXY.fifo)
        If rtn <> 0 Then
            Return False
        End If
        rtn = GT_CrdStart(cLnXY.cardNo, cLnXY.crd, cLnXY.fifo)   '启动坐标系1插补运动
        If rtn <> 0 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 判断插补运动是否完成
    ''' </summary>
    ''' <param name="cardNum"></param>
    ''' <param name="crd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ZSPD_Line(ByVal cardNum As Short, ByVal crd As Short) As Boolean
        Dim sRtn As Short, state As Short, Segment As Integer

        sRtn = GT_CrdStatus(cardNum, crd, state, Segment, 0)
        If sRtn = 0 Then
            If state <> 0 Then
                Return False
            End If
        Else
            Return False
        End If
        Return True
    End Function


    ''' <summary>
    ''' 轴回原点函数
    ''' </summary>
    ''' <param name="Card">卡号：从0开始</param>
    ''' <param name="Axis">轴号：从1开始</param>
    ''' <remarks></remarks>
    Public Sub Motor_Homelongmen(ByVal Card As Integer, ByVal Axis As Integer, Optional isRotateAxis As Boolean = False)
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
                            Tvel = CDbl(AxisPar.HomeVel(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 1000 / 360)     '计算当前轴目标速度脉冲频率（原点搜索速度）
                            Tpos = CLng(HomeSearchDist(Card, Axis) * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) / 360) '计算当前轴目标位置脉冲数量（即原点搜索距离）
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
                    If GetTickCount - HomeTime(Card, Axis) > 500 Then
                        rtn = GT_SetPrfPos(Card, Axis, 0) '将当前轴规划器位置修改为零点
                        rtn = GT_SetEncPos(Card, Axis, 0) '将当前轴编码器位置修改为零点
                        rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1))  '将当前轴进行位置同步

                        If (Card = 2 And Axis = 1) Or (Card = 2 And Axis = 3) Then
                            rtn = GT_SetPrfPos(Card, Axis + 1, 0) '将当前轴规划器位置修改为零点
                            rtn = GT_SetEncPos(Card, Axis + 1, 0) '将当前轴编码器位置修改为零点
                            rtn = GT_SynchAxisPos(Card, 2 ^ (Axis - 1 + 1))  '将当前轴进行位置同步 
                        End If

                        Delay(100)
                        HomeStep(Card, Axis) = 15
                    End If

                Case 15
                    rtn = GT_GetPrfPos(Card, Axis, CurrentPos, 1, 0) '读取0号卡当前轴规划位置
                    rtn = GT_GetEncPos(Card, Axis, CurrentPosEnc, 1, 0) '读取0号卡当前轴实际位置
                    If Math.Abs(CurrentPos) < 10 And Math.Abs(CurrentPosEnc) < 10 Then
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


    Private LockedPaste1 As Boolean
    Private LockedTake1 As Boolean
    Public Function AbsMotionlongmen(ByVal Card As Integer, ByVal Axis As Integer, ByVal Speed As Double, ByVal Dist As Double) As Boolean
        Dim TempPos, TempVel As Double
        Dim Pos, Vel As Double
        Dim TrapPrm As TTrapPrm
        Dim rtn As Short

        Dim TempPos1, TempVel1 As Double
        Dim Pos1, Vel1 As Double
        Dim TrapPrm1 As TTrapPrm
        Dim rtn1 As Short

        Dim K As Double

        Dim prfPos(1) As Double

        Dim d(1) As Double

        Dim PasetY_SafeLimet As Double
        Dim TakeY_SafeLimet As Double

        '龙门Y轴会有撞机风险
        If Card = 2 Then
            PasetY_SafeLimet = Par_Pos.St_Paste(14).Y
            TakeY_SafeLimet = Par_Pos.St_PreTaker(9).Y

            Select Case Axis
                Case 1, 2
                    '判断安全锁
                    If LockedTake1 = True Then Return False

                    '先判断运行方向，如果朝+方向运行，则继续向下判断
                    If Dist - CurrEncPos(2, PasteY1) > 0 Or Dist - CurrEncPos(2, PasteY2) > 0 Then
                        '再判断运动的目的地，如果超过安全区域，则往下继续判断
                        If Dist > PasetY_SafeLimet Then
                            '最后判断对方轴是在安全区域以内并且停止中
                            If (Not isAxisMoving(2, PreTakerY1)) And (Not isAxisMoving(2, PreTakerY2)) And (CurrEncPos(2, PreTakerY1) <= TakeY_SafeLimet) And (CurrEncPos(2, PreTakerY2) <= TakeY_SafeLimet) Then
                                LockedPaste1 = True
                            Else
                                Return False
                            End If
                        End If
                    End If

                Case 3, 4
                    '判断安全锁
                    If LockedPaste1 = True Then Return False

                    '先判断运行方向，如果朝+方向运行，则继续向下判断
                    If Dist - CurrEncPos(2, PreTakerY1) > 0 Or Dist - CurrEncPos(2, PreTakerY2) > 0 Then
                        '再判断运动的目的地，如果超过安全区域，则往下继续判断
                        If Dist > TakeY_SafeLimet Then
                            '最后判断对方轴是在安全区域以内并且停止中
                            If (Not isAxisMoving(2, PasteY1)) And (Not isAxisMoving(2, PasteY2)) And (CurrEncPos(2, PasteY1) <= PasetY_SafeLimet) And (CurrEncPos(2, PasteY2) <= PasetY_SafeLimet) Then
                                LockedTake1 = True
                            Else
                                Return False
                            End If
                        End If
                    End If

                Case Else
                    Return False
            End Select
        End If

        TempVel = Speed
        TempPos = Dist
        rtn = GT_ClrSts(Card, Axis)               '清除当前轴的错误标志
        rtn = GT_PrfTrap(Card, Axis)              '将当前轴设置为点位运动模式
        rtn = GT_GetTrapPrm(Card, Axis, TrapPrm)  '读取当前轴点位模式运动参数

        TrapPrm.acc = AxisPar.acc(Card, Axis)       '载入当前轴的加速度
        TrapPrm.dec = AxisPar.dec(Card, Axis)       '载入当前轴的减速度

        rtn = GT_SetTrapPrm(Card, Axis, TrapPrm)    '设置当前轴加速度、减速度、起跳速度、平滑时间

        If Card = 2 And (Axis = 1 Or Axis = 3) Then
            rtn = GT_GetPrfPos(Card, Axis + 1, prfPos(1))
            TempPos1 = CalculateOffset(Dist)
            d(1) = Math.Abs(CDbl(TempPos1 * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis)) - prfPos(1))

            rtn = GT_GetPrfPos(Card, Axis, prfPos(0))
            d(0) = Math.Abs(CDbl(TempPos * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis)) - prfPos(0))

            If d(1) <> 0 And d(0) <> 0 Then
                K = d(1) / d(0)
            Else
                K = 1
            End If


            rtn1 = GT_ClrSts(Card, Axis + 1)               '清除当前轴的错误标志
            rtn1 = GT_PrfTrap(Card, Axis + 1)              '将当前轴设置为点位运动模式
            rtn1 = GT_GetTrapPrm(Card, Axis + 1, TrapPrm1)  '读取当前轴点位模式运动参数

            TrapPrm1.acc = TrapPrm.acc * K     '载入当前轴的加速度
            TrapPrm1.dec = TrapPrm.dec * K     '载入当前轴的减速度

            rtn1 = GT_SetTrapPrm(Card, Axis + 1, TrapPrm1)    '设置当前轴加速度、减速度、起跳速度、平滑时间
        End If


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

                If Card = 2 And (Axis = 1 Or Axis = 3) Then
                    Vel1 = Vel * K
                    Pos1 = CDbl(TempPos1 * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))    '(*计算目标位置脉冲数量*)
                End If

        End Select
        rtn = GT_SetVel(Card, Axis, Vel)      '设置当前轴的目标速度
        rtn = GT_SetPos(Card, Axis, Pos)      '设置当前轴的目标位置

        If Card = 2 And (Axis = 1 Or Axis = 3) Then
            rtn1 = GT_SetVel(Card, Axis + 1, Vel1)      '设置当前轴的目标速度
            rtn1 = GT_SetPos(Card, Axis + 1, Pos1)      '设置当前轴的目标位置
        End If

        If Card = 2 And (Axis = 1 Or Axis = 3) Then
            rtn = GT_Update(Card, (2 ^ (Axis - 1)) + (2 ^ Axis))   '启动当前轴运动
        Else
            rtn = GT_Update(Card, 2 ^ (Axis - 1))   '启动当前轴运动
        End If 

        AxisTime(Card, Axis) = GetTickCount()

        '解锁
        If Card = 2 Then
            Select Case Axis
                Case 1, 2
                    LockedPaste1 = False
                Case 3, 4
                    LockedTake1 = False
            End Select
        End If

        Return True
    End Function
End Module
