Module Station_PreTaker

    Public Step_PreTaker As Integer
    Public index_TakerMaterial As Integer

    ''' <summary>
    ''' 中转平台上的物料信息
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure TransferPlate
        Dim isHaveCam As Boolean
        Dim Barcode As String
        Dim TakerPress As Double

        Public Sub Init()
            isHaveCam = False
            Barcode = ""
            TakerPress = 999
        End Sub
    End Structure

    Public Cam_OnTransferPlate As TransferPlate

    ''' <summary>
    ''' 用来保存从料盘取料得到的压力，条码等信息
    ''' </summary>
    ''' <remarks></remarks>
    Private PreTakerTransferPlate As TransferPlate

    ''' <summary>
    ''' 预取料站压力传感器返回值
    ''' </summary>
    ''' <remarks></remarks>
    Private RetPreTakerPress As Integer

    Public Sub GoPos_PreTaker(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(0, PreTakerX) And ServoOn(2, PreTakerY1) And ServoOn(0, PreTakerZ) And ServoOn(1, PreTakerR) Then
        Else
            List_DebugAddMessage("请先打开取料工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(0, PreTakerX) Or isAxisMoving(0, PreTakerZ) Or isAxisMoving(1, PreTakerR) Or isAxisMoving(2, PreTakerY1) Then
            List_DebugAddMessage("取料工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(2, PasteY1) - Par_Pos.St_Paste(0).Y) > 2 Then
            List_DebugAddMessage("请检查组装模组是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(3) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(3)
                Case 0
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                    Step_Gopos(3) = 10

                Case 10
                    If isAxisMoving(0, PreTakerZ) = False Then
                        List_DebugAddMessage("取料站Z轴运动到待机位置完成")
                        Step_Gopos(3) = 20
                    End If

                Case 20
                    Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), Par_Pos.St_PreTaker(index).X)
                    Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Par_Pos.St_PreTaker(index).R)
                    Step_Gopos(3) = 25

                Case 25
                    If AbsMotion(2, PreTakerY1, AxisPar.MoveVel(0, PreTakerY1), Par_Pos.St_PreTaker(index).Y) = True Then
                        Step_Gopos(3) = 30
                    End If

                Case 30
                    If isAxisMoving(0, PreTakerX) = False And isAxisMoving(1, PreTakerR) = False And isAxisMoving(2, PreTakerY1) = False Then
                        List_DebugAddMessage("取料站XYR运动到" & Par_Pos.St_PreTaker(index).Name & "完成")
                        Step_Gopos(3) = 40
                    End If

                Case 40
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(index).Z)
                    Step_Gopos(3) = 50

                Case 50
                    If isAxisMoving(0, PreTakerZ) = False Then
                        List_DebugAddMessage("取料站Z运动到" & Par_Pos.St_PreTaker(index).Name & "完成")
                        Step_Gopos(3) = 0
                        Exit Do
                    End If

            End Select
        Loop
    End Sub

    Public Sub GoPos_Feed(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, FeedZ) And ServoOn(1, RecycleZ) Then
        Else
            List_DebugAddMessage("请先打开供料工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, FeedZ) Or isAxisMoving(1, RecycleZ) Then
            List_DebugAddMessage("供料工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Select Case index
            Case 0
                Call AbsMotion(1, FeedZ, AxisPar.MoveVel(1, FeedZ), Par_Pos.St_Feed(0).Z)
                Call AbsMotion(1, RecycleZ, AxisPar.MoveVel(1, RecycleZ), Par_Pos.St_Recycle(0).Z)

            Case 1, 2
                Call AbsMotion(1, FeedZ, AxisPar.MoveVel(1, FeedZ), Par_Pos.St_Feed(index).Z)

            Case 3, 4
                Call AbsMotion(1, RecycleZ, AxisPar.MoveVel(1, RecycleZ), Par_Pos.St_Recycle(index).Z)
        End Select

        Do While True
            My.Application.DoEvents()
            If isAxisMoving(1, FeedZ) = False And isAxisMoving(1, RecycleZ) = False Then
                List_DebugAddMessage("供料Z轴运动到指定位置OK")
            End If
        Loop
    End Sub


    Public Sub AutoRun_PreTakerStation()
        'PreTaker_Sta.workState = 0   '工作进行中
        'PreTaker_Sta.workState = 1   '工作完成
        'PreTaker_Sta.workState = 2   '工作进行中：拍物料
        'PreTaker_Sta.workState = 3   '工作进行中：取料
        'PreTaker_Sta.workState = 4   '工作进行中：去放料，夹镜头保护盖
        Static timeStart As Long    '记录开始时间


        '预取料站暂停功能
        If Flag_MachinePause = True Then
            Exit Sub
        End If


        Select Case Step_PreTaker
            Case 10
                If Flag_MachineStop = False And Cam_OnTransferPlate.isHaveCam = False And Feed_Sta.isWorking = False And Feed_Sta.workState = 1 Then
                    PreTaker_Sta.isNormal = True : PreTaker_Sta.isWorking = True : PreTaker_Sta.workState = 0    '取料模组工作进行中
                    PreTakerTransferPlate.Init()
                    Step_PreTaker = 100
                End If

            Case 100
                '取料站Z轴回初始位置
                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                Step_PreTaker = 110

            Case 110
                If isAxisMoving(0, PreTakerZ) = False Then
                    Step_PreTaker = 120
                End If

            Case 120
                'XY去料盘拍照位置 R轴去拍料盘第一颗料的R位置
                Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), TrayMatrix.TrayPreTaker(index_TakerMaterial).X) 
                Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Par_Pos.St_PreTaker(1).R)
                Step_PreTaker = 125

            Case 125
                If AbsMotion(2, PreTakerY1, AxisPar.MoveVel(2, PreTakerY1), TrayMatrix.TrayPreTaker(index_TakerMaterial).Y) = True Then
                    Step_PreTaker = 130
                End If

            Case 130
                'XYR轴到位
                If isAxisMoving(0, PreTakerX) = False And isAxisMoving(2, PreTakerY1) = False And isAxisMoving(1, PreTakerR) = False Then
                    PreTaker_Sta.workState = 2
                    Step_PreTaker = 150
                End If

            Case 150
                'Z去拍料盘第一颗料的Z位置
                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(1).Z)
                Step_PreTaker = 160

            Case 160
                If isAxisMoving(0, PreTakerZ) = False Then
                    timeStart = GetTickCount
                    Step_PreTaker = 200
                End If

            Case 200
                If isTimeout(timeStart, 500) Then
                    '触发CCD拍照 ,其中载具条码与module条码用时间代替
                    If TriggerCCD("T4,1", index_TakerMaterial, Now.ToString, Now.ToString) Then
                        timeStart = GetTickCount
                        Step_PreTaker = 300
                    End If
                End If

            Case 300
                '等待获取取料位置和产品条码
                If Winsock1_Data(0) = "T4" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(4) = 1 Then
                        PreTakerTransferPlate.Barcode = Cam4Data(1, 4)
                        Step_Paste = 400
                    Else 
                        Frm_DialogAddMessage("预取料站CCD1料盘拍第" & index_TakerMaterial + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("预取料站CCD1料盘拍第" & index_TakerMaterial + 1 & "颗料物料超时")
                End If

            Case 400
                '取料站Z轴回初始位置
                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                Step_PreTaker = 420

            Case 420
                If isAxisMoving(0, PreTakerZ) = False Then
                    Step_PreTaker = 460
                End If

            Case 460
                'XY去取料盘中物料的位置
                Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), Cam4Data(1, 0))
                Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Cam4Data(1, 2))
                Step_PreTaker = 465

            Case 465
                If AbsMotion(2, PreTakerY1, AxisPar.MoveVel(2, PreTakerY1), Cam4Data(1, 1)) = True Then
                    Step_PreTaker = 480
                End If

            Case 480
                If isAxisMoving(0, PreTakerX) = False And isAxisMoving(2, PreTakerY1) = False And isAxisMoving(1, PreTakerR) = False Then
                    PreTaker_Sta.workState = 3
                    Step_PreTaker = 500
                End If

            Case 500
                'Z轴下降到取料Z上方3mm处
                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(5).Z - 3)
                Step_PreTaker = 505

            Case 505
                '打开预取料站的压力传感器
                If Com3_Send(":O000000o" & vbCrLf) = False Then
                    Call Frm_Main.COM1_Init(par.CCD(2))
                    Delay(50)
                    Com3_Send(":O000000o" & vbCrLf)
                    Step_PreTaker = 510
                End If

            Case 510
                RetPreTakerPress = Com1_Return() '等待压力传感器打开结束
                If RetPreTakerPress = 0 Then     '表示压力传感器打开成功
                    Step_PreTaker = 520
                Else
                    ListBoxAddMessage("预取料站吸嘴压力传感器打开失败，请查检！") 
                End If 
                 
            Case 520
                If isAxisMoving(0, PreTakerZ) = False Then
                    Call AbsMotion(0, PreTakerZ, 5, Par_Pos.St_PreTaker(5).Z)
                    Step_PreTaker = 540
                End If

            Case 540
                If isAxisMoving(0, PreTakerZ) = False Then
                    Com1_Send(":Q000000q" & vbCrLf) ' 串口接收数据关闭
                    PreTakerTransferPlate.TakerPress = Press(1)
                    Step_PreTaker = 560
                End If

            Case 560
                SetEXO(0, 14, True)     '打开取料站取料吸嘴真空吸
                SetEXO(0, 10, True)      '打开取料站排线吸嘴真空吸
                timeStart = GetTickCount
                Step_PreTaker = 580

            Case 580
                If EXI(0, 14) And EXI(0, 10) Then
                    'Z轴抬升到待机位置
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                    timeStart = GetTickCount
                    Step_PreTaker = 600
                ElseIf GetTickCount - timeStart > 3 * 1000 Then
                    '如果真空3S内还是达不到，还是上升
                    ListBoxAddMessage("预取料站取料吸嘴负压或吸排线负压过小")
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                    timeStart = GetTickCount
                    Step_PreTaker = 600
                End If

            Case 600
                If isAxisMoving(0, PreTakerZ) = False Then
                    If EXI(0, 14) And EXI(0, 10) Then
                        '从料盘中取料成功
                        Step_PreTaker = 620
                    ElseIf GetTickCount - timeStart > 2 * 1000 Then
                        If EXI(0, 14) = False Then
                            Frm_DialogAddMessage("预取料站取料吸嘴负压太小，请检查！")
                        ElseIf EXI(0, 10) Then
                            Frm_DialogAddMessage("预取料站排线吸嘴负压太小，请检查！")
                        End If
                         
                        '从料盘中取料失败，进入抛料流程
                        Step_PreTaker = 602
                    End If 
                End If

            Case 602  '开始抛料，直接抛在当前料盘位
                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(5).Z - 5)
                Step_PreTaker = 604

            Case 604
                If isAxisMoving(0, PreTakerZ) = False Then
                    SetEXO(0, 14, False)     '关闭取料站取料吸嘴真空吸
                    SetEXO(0, 10, False)     '关闭取料站排线吸嘴真空吸

                    SetEXO(0, 15, True)     '打开取料站取料吸嘴破真空
                    SetEXO(0, 11, True)     '打开取料站排线吸嘴破真空

                    timeStart = GetTickCount
                    Step_PreTaker = 606
                End If

            Case 606
                If GetTickCount - timeStart > 500 Then
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                    Step_PreTaker = 608
                End If

            Case 608
                If isAxisMoving(0, PreTakerZ) = False Then
                    Step_PreTaker = 980
                End If


            Case 620  '去物料中转平台的位置
                If Cam_OnTransferPlate.isHaveCam = False And Paste_Sta.workState <> 2 And EMI(1, 11) And EMI(1, 12) Then
                    PreTaker_Sta.workState = 4 
                    Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), Par_Pos.St_PreTaker(6).X)
                    Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Par_Pos.St_PreTaker(6).R)
                    Step_PreTaker = 625
                   
                ElseIf Cam_OnTransferPlate.isHaveCam Then
                    '中转平台上有物料
                ElseIf Paste_Sta.workState <> 2 Then
                    '组装站正在中转平台上取料，请等待

                ElseIf EMI(1, 11) = False Then
                    '夹镜头保护盖夹紧气缸未处于松开状态'
                    Frm_DialogAddMessage("夹镜头保护盖夹紧气缸未处于松开状态,请检查！")
                ElseIf EMI(1, 12) = False Then
                    '夹镜头保护盖升降气缸未处于上升状态
                    Frm_DialogAddMessage("夹镜头保护盖升降气缸未处于上升状态,请检查！")
                End If

            Case 625
                If AbsMotion(2, PreTakerY1, AxisPar.MoveVel(2, PreTakerY1), Par_Pos.St_PreTaker(6).Y) = True Then
                    Step_PreTaker = 700
                End If

            Case 700
                If isAxisMoving(0, PreTakerX) = False And isAxisMoving(2, PreTakerY1) = False And isAxisMoving(1, PreTakerR) = False Then
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(6).Z)
                    Step_PreTaker = 720
                End If

            Case 720
                If isAxisMoving(0, PreTakerZ) = False Then
                    SetEXO(0, 14, False)     '关闭取料站取料吸嘴真空吸
                    SetEXO(0, 10, False)     '关闭取料站排线吸嘴真空吸

                    SetEXO(0, 15, True)     '打开取料站取料吸嘴破真空
                    SetEXO(0, 11, True)     '打开取料站排线吸嘴破真空

                    timeStart = GetTickCount
                    Step_PreTaker = 740
                End If

            Case 740
                If GetTickCount - timeStart > 500 Then
                    SetEXO(0, 15, False)     '关闭取料站取料吸嘴破真空
                    SetEXO(0, 11, False)     '关闭取料站排线吸嘴破真空
                    timeStart = GetTickCount
                    Step_PreTaker = 760
                End If

            Case 760
                If EMI(1, 11) And EMI(1, 12) Then
                    SetEMO(1, 11, True) '夹抓气缸夹紧
                    timeStart = GetTickCount
                    Step_PreTaker = 780
                ElseIf EMI(1, 11) = False Then 
                    Frm_DialogAddMessage("夹镜头保护盖夹紧气缸未处于松开状态,请检查！")
                ElseIf EMI(1, 12) = False Then 
                    Frm_DialogAddMessage("夹镜头保护盖升降气缸未处于上升状态,请检查！")
                End If
                 
            Case 780
                If EMI(1, 11) = False Then
                    timeStart = GetTickCount
                    Step_PreTaker = 800
                ElseIf GetTickCount - timeStart > 3000 Then 
                    Frm_DialogAddMessage("夹镜头保护盖夹紧气缸夹紧动作异常,请检查！")
                End If

            Case 800
                If GetTickCount - timeStart > 500 Then
                    SetEMO(0, 12, True)
                    timeStart = GetTickCount
                    Step_PreTaker = 820
                End If

            Case 820 
                If EMI(1, 12) = False Then
                    timeStart = GetTickCount
                    Step_PreTaker = 840
                ElseIf GetTickCount - timeStart > 3000 Then 
                    Frm_DialogAddMessage("夹镜头保护盖升降气缸下拉异常,请检查！")
                End If

            Case 840
                If GetTickCount - timeStart > 500 Then
                    timeStart = GetTickCount
                    SetEMO(1, 11, False) '夹镜头保护盖气缸夹爪松开
                    Step_PreTaker = 860
                End If

            Case 860
                If EMI(1, 11) Then
                    timeStart = GetTickCount
                    SetEMO(1, 12, False) '夹镜头保护盖气缸夹爪松开
                    Step_PreTaker = 880
                ElseIf GetTickCount - timeStart > 3000 Then 
                    Frm_DialogAddMessage("夹镜头保护盖夹紧气缸夹松开动作异常,请检查！")
                End If

            Case 880
                If EMI(1, 12) Then
                    Step_PreTaker = 900
                ElseIf GetTickCount - timeStart > 3000 Then 
                    Frm_DialogAddMessage("夹镜头保护盖升降气缸回位上升异常,请检查！")
                End If
                 
            Case 900

                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                Step_PreTaker = 920


            Case 920
                If isAxisMoving(0, PreTakerZ) = False Then
                    Step_PreTaker = 940
                End If

            Case 940
                Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), Par_Pos.St_PreTaker(0).X)
                Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Par_Pos.St_PreTaker(0).R)
                Step_PreTaker = 945

            Case 945
                If AbsMotion(2, PreTakerY1, AxisPar.MoveVel(2, PreTakerY1), Par_Pos.St_PreTaker(0).Y) = True Then
                    Step_PreTaker = 960
                End If

            Case 960  '将数据传递给物料中转平台
                If isAxisMoving(0, PreTakerX) = False And isAxisMoving(2, PreTakerY1) = False And isAxisMoving(1, PreTakerR) = False Then
                    PreTakerTransferPlate.isHaveCam = True
                    Cam_OnTransferPlate = PreTakerTransferPlate
                    PreTaker_Sta.workState = 0   '工作完成

                    Step_PreTaker = 980
                End If

            Case 980
                index_TakerMaterial = index_TakerMaterial + 1

                If index_TakerMaterial > (par.num(33) * par.num(34) - 1) Then
                    '物料已经用完
                    PreTaker_Sta.workState = 1   '工作完成
                Else
                    Step_PreTaker = 1000
                End If

            Case 1000
                If Cam_OnTransferPlate.isHaveCam = True Then
                    Step_PreTaker = 8000
                Else
                    '抛料完成后，
                    Step_PreTaker = 10
                End If

            Case 8000
                '预取料站工作完成
                PreTaker_Sta.isWorking = False    '组装模组工作完成
                PreTaker_Sta.isNormal = True
                PreTaker_Sta.workState = 1  '工作完成
                Step_PreTaker = 10  '开始下一个循环

            Case 9000
                '工作异常需要急停处理
                PreTaker_Sta.isNormal = False   '组装工站工作异常
                Call Frm_Main.Machine_Stop()
                Step_PreTaker = 0
                 
        End Select
    End Sub


    Public Sub ManualRun_PreTaker()
        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        '判断取料站是否所有轴伺服ON
        If ServoOn(0, PreTakerX) And ServoOn(2, PreTakerY1) And ServoOn(0, PreTakerZ) And ServoOn(1, PreTakerR) Then
        Else
            List_DebugAddMessage("请先打开取料工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断取料站是否有某个轴在运动中
        If isAxisMoving(0, PreTakerX) Or isAxisMoving(0, PreTakerZ) Or isAxisMoving(1, PreTakerR) Or isAxisMoving(2, PreTakerY1) Then
            List_DebugAddMessage("取料工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断供料站是否所有轴伺服ON
        If ServoOn(1, FeedZ) And ServoOn(1, RecycleZ) Then
        Else
            List_DebugAddMessage("请先打开供料工位所有轴伺服ON")
            Exit Sub
        End If

        '判断供料站是否有某个轴在运动中
        If isAxisMoving(1, FeedZ) Or isAxisMoving(1, RecycleZ) Then
            List_DebugAddMessage("供料工位有轴正在运动中，请等待")
            Exit Sub
        End If
         
        '判断组装站Y轴是否在初始位置
        If Math.Abs(CurrEncPos(2, PasteY1) - Par_Pos.St_Paste(0).Y) > 2 Then
            List_DebugAddMessage("请检查组装模组是否在安全位置！")
            Exit Sub
        End If

        '检测物料中转平台上面的两个磁簧是否工作正常
        If EMI(1, 11) And EMI(1, 12) Then
        Else
            If Not EMI(1, 11) Then
                MessageBox.Show("夹镜头保护盖夹紧气缸未处于松开状态")
            ElseIf Not EMI(1, 12) Then
                MessageBox.Show("夹镜头保护盖升降气缸未处于上升状态")
            End If
            Exit Sub
        End If

        '提示物料中转平台上的物料是否已经清除干净
        If MessageBox.Show("请确认物料中转平台上物料是否已经被清除", "", MessageBoxButtons.OKCancel) <> DialogResult.OK Then
            Exit Sub
        End If

        '清除物料中转平台的信息
        Cam_OnTransferPlate.Init()

        Feed_Sta.isWorking = False
        Feed_Sta.workState = 1

        Step_PreTaker = 10

        Do While True
            My.Application.DoEvents()
            Delay(10)

            '自动配合程序
            Call AutoRun_PreTakerStation()

            If PreTaker_Sta.isWorking = False And PreTaker_Sta.workState = 1 Then
                List_DebugAddMessage("取料站自动运行完成！")
                Exit Do
            End If

            If IsSysEmcStop Or PreTaker_Sta.isNormal = False Then
                List_DebugAddMessage("预取料单站自动运行异常，停止自动运行！")
                Exit Do
            End If
        Loop

    End Sub

End Module
