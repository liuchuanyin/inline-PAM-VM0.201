Module Station_Paste

    Public Step_Paste As Integer

    '产品索引号
    Public index_InPaste As Short

    ''' <summary>
    ''' 精确补偿次数
    ''' </summary>
    ''' <remarks></remarks>
    Private Cnt_Buchang As Short

    ''' <summary>
    ''' 贴合贴压力传感器返回值
    ''' </summary>
    ''' <remarks></remarks>
    Private RstPastePress As Integer

    ''' <summary>
    ''' 当前吸嘴上这颗物料的数据
    ''' </summary>
    ''' <remarks></remarks>
    Private CurNozTransferPlate As TransferPlate

    ''' <summary>
    ''' 用来控制预固化索引
    ''' </summary>
    ''' <remarks></remarks>
    Private Index_CurePoint As Short

    '组装XYZR去指定位置
    Public Sub GoPos_Paste(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(0, PasteX) And ServoOn(2, PasteY1) And ServoOn(0, PasteZ) And ServoOn(0, PasteR) Then
        Else
            List_DebugAddMessage("请先打开组装工位所有轴伺服ON")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(0, PasteX) Or isAxisMoving(0, PasteZ) Or isAxisMoving(0, PasteR) Or isAxisMoving(2, PasteY1) Then
            List_DebugAddMessage("组装工位有轴正在运动中，请等待")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(1, CureX) - Par_Pos.St_Cure(0).X) > 2 Then
            List_DebugAddMessage("请检查预固化轴是否在安全位置！")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(2, PreTakerY1) - Par_Pos.St_PreTaker(0).Y) > 2 Then
            List_DebugAddMessage("请检查取料模组是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(2) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(2)
                Case 0
                    Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                    Step_Gopos(2) = 10

                Case 10
                    If isAxisMoving(0, PasteZ) = False Then
                        Frm_DialogAddMessage("组装站Z轴运动到待机位置完成")
                        Step_Gopos(2) = 20
                    End If

                Case 20
                    Call AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(index).X)
                    Call AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(index).R)
                    If AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(index).Y) = True Then
                        Step_Gopos(2) = 30
                    End If
                Case 30
                    If isAxisMoving(0, PasteR) = False And isAxisMoving(0, PasteX) = False And isAxisMoving(2, PasteY1) = False Then
                        Frm_DialogAddMessage("组装站X,Y,R轴运动到" & Par_Pos.St_Paste(index).Name & "完成")
                        Step_Gopos(2) = 40
                    End If

                Case 40
                    Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(index).Z)
                    Step_Gopos(2) = 50

                Case 50
                    If isAxisMoving(0, PasteZ) = False Then
                        Frm_DialogAddMessage("组装站Z轴运动到" & Par_Pos.St_Paste(index).Name & "完成")
                        Step_Gopos(2) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub

    '精补XY去指定位置
    Public Sub GoPos_FineCompensation(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, FineX) And ServoOn(1, FineY) Then
        Else
            List_DebugAddMessage("请先打开精补工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, FineX) Or isAxisMoving(1, FineY) Then
            List_DebugAddMessage("精补工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Step_Gopos(4) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(4)
                Case 0
                    Call AbsMotion(1, FineX, AxisPar.MoveVel(1, FineX), Par_Pos.St_FineCompensation(index).X)
                    Call AbsMotion(1, FineY, AxisPar.MoveVel(1, FineY), Par_Pos.St_FineCompensation(index).Y)
                    Step_Gopos(4) = 10

                Case 10
                    If isAxisMoving(1, FineX) = False And isAxisMoving(1, FineY) = False Then
                        Frm_DialogAddMessage("精补模组运动到" & Par_Pos.St_FineCompensation(index).Name & "完成！")
                        Step_Gopos(4) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub

    '预固化去指定位置
    Public Sub GoPos_Cure(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, CureX) Then
        Else
            List_DebugAddMessage("请先打开CureX轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, CureX) Then
            List_DebugAddMessage("CureX轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(2, PasteY1) - Par_Pos.St_Paste(0).Y) > 2 Then
            List_DebugAddMessage("请检组装站模组是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(6) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(6)
                Case 0
                    Call AbsMotion(1, CureX, AxisPar.MoveVel(1, CureX), Par_Pos.St_Cure(index).X)
                    Step_Gopos(6) = 10

                Case 10
                    If isAxisMoving(1, CureX) = False Then
                        Frm_DialogAddMessage("预固化模组运动到" & Par_Pos.St_Cure(index).Name & "完成")
                        Step_Gopos(6) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub

    ''' <summary>
    ''' 判断是否还有物料，没有贴合Cam
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isHaveBracketNotPasteCamera() As Boolean
        Dim mValue As Boolean
        mValue = False
        Try
            For i = index_InPaste To Tray_Pallet(2).Hole.Count - 1
                If Tray_Pallet(2).Hole(i).isProductOk And Tray_Pallet(2).Hole(i).isHaveProduct And Frm_Engineering.chk_Brc(i).Checked Then
                    mValue = True
                End If
            Next
        Catch ex As Exception
            Frm_DialogAddMessage("获取2段流水线上是否有产品没有贴合异常" & ex.ToString)
        End Try

        Return mValue
    End Function

    ''' <summary>
    ''' 组装站自动运行
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub AutoRun_PasteStation()
        ' Paste_Sta.workState =0 工作进行中
        ' Paste_Sta.workState =1 工作完成
        ' Paste_Sta.workState =2 工作进行中:取料
        ' Paste_Sta.workState =3 工作进行中:定位拍照
        ' Paste_Sta.workState =4 工作进行中:精补贴合
        ' Paste_Sta.workState =5 工作进行中:抛料
        ' Paste_Sta.workState =6 工作进行中:等待取料机构向中转机构上放料
        Static timeStart As Long    '记录开始时间

        Select Case Step_Paste
            Case 10
                If Flag_MachineStop = False And Line_Sta(2).workState = 2 And _
                    Line_Sta(2).isHaveTray = True And isTimeout(cmd_SendTime, 2) Then
                    Paste_Sta.isNormal = True : Paste_Sta.isWorking = True : Paste_Sta.workState = 0    '组装模组工作进行中
                    Step_Paste = 100
                End If

            Case 100
                '流水线上有载具，且载具可用
                If Tray_Pallet(2).isHaveTray And Tray_Pallet(2).isTrayOK Then
                    index_InPaste = 0
                    Step_Paste = 200
                End If

            Case 200
                If Tray_Pallet(2).Hole(index_InPaste).isHaveProduct And Tray_Pallet(2).Hole(index_InPaste).isProductOk And Frm_Engineering.chk_Brc(index_InPaste).Checked Then
                    '当前穴位有料，且是OK的，并且选中要做这个

                    ListBoxAddMessage("组装站开始组装第" & index_InPaste + 1 & "颗料！")

                    '判断完要做哪个穴位后，精补轴直接过去对应点位
                    AbsMotion(1, FineX, AxisPar.MoveVel(1, FineX), TrayMatrix.TrayFineCompensation(index_InPaste).X)
                    AbsMotion(1, FineY, AxisPar.MoveVel(1, FineY), TrayMatrix.TrayFineCompensation(index_InPaste).Y)

                    Step_Paste = 202
                Else
                    '否则跳过这一颗料
                    ListBoxAddMessage("组装站组装跳过第" & index_InPaste + 1 & "颗料！")
                    Step_Paste = 4000
                End If

            Case 202 '判断精补轴到位
                If (Not isAxisMoving(1, FineX)) And (Not isAxisMoving(1, FineY)) Then
                    timeStart = GetTickCount
                    Step_Paste = 204
                End If

            Case 204  '延时0.5S
                If GetTickCount - timeStart > 500 Then
                    If TriggerCCD("T2,1", index_InPaste, Tray_Pallet(2).Tray_Barcode, Tray_Pallet(2).Hole(index_InPaste).ProductBarcode) = True Then
                        timeStart = GetTickCount
                        Step_Paste = 206
                    End If
                End If

            Case 206  '等待精补CCD2拍照完成
                If Winsock1_Data(0) = "T2" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(2) = 1 Then
                        Step_Paste = 210
                    Else
                        Frm_DialogAddMessage("组装站CCD2拍第" & index_InPaste + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("组装站CCD2拍第" & index_InPaste + 1 & "颗料物料超时")
                End If

            Case 210
                '中转机构上有料且，取料模组不在放料的过程中
                If Cam_OnTransferPlate.isHaveCam And PreTaker_Sta.workState <> 4 Then
                    Paste_Sta.workState = 2    '工作进行中:取料
                    Step_Paste = 300    '直接去取料
                Else
                    Step_Paste = 220    '运动到待机位置，待料
                End If

            Case 220
                Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                Step_Paste = 230

            Case 230
                If isAxisMoving(0, PasteZ) = False Then
                    '运动到待机位置
                    Call AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(0).X)
                    Call AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(0).R)
                    If AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(0).Y) = True Then
                        Step_Paste = 230
                    End If
                End If

            Case 240
                If isAxisMoving(0, PasteR) = False And isAxisMoving(0, PasteX) = False And isAxisMoving(2, PasteY1) = False Then
                    Paste_Sta.workState = 6     '工作进行中:等待取料机构向中转机构上放料
                    ListBoxAddMessage("组装站运动到待机位置，等待取料模组放料")
                    Step_Paste = 250
                End If

            Case 250
                '中转机构上有料且，取料模组不在放料的过程中
                If Cam_OnTransferPlate.isHaveCam And PreTaker_Sta.workState <> 4 Then
                    Paste_Sta.workState = 2    '工作进行中:取料
                    Step_Paste = 300    '去取料
                End If

            Case 300  '回到待机位置
                Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                Step_Paste = 310

            Case 310
                '组装模组去取料位置取料
                If isAxisMoving(0, PasteZ) = False Then
                    '运动到取料位置
                    Call AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(1).X)
                    Call AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(1).R)
                    If AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(1).Y) = True Then
                        Step_Paste = 330
                    End If
                End If

            Case 330
                If isAxisMoving(0, PasteR) = False And isAxisMoving(0, PasteX) = False And isAxisMoving(2, PasteY1) = False Then
                    ListBoxAddMessage("组装站X、Y、R轴运动到取料位置")
                    Step_Paste = 350
                End If

            Case 350
                Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(1).Z - 3)
                Step_Paste = 370

            Case 370
                If isAxisMoving(0, PasteZ) = False Then
                    '吸料高度最后3mm降速运行
                    Call AbsMotion(0, PasteZ, 3, Par_Pos.St_Paste(1).Z)
                    Step_Paste = 390
                End If

            Case 390
                If isAxisMoving(0, PasteZ) = False Then
                    ListBoxAddMessage("组装站Z轴运动到取料位置高度")
                    Step_Paste = 400
                End If

            Case 400
                SetEXO(0, 12, True)     '打开组装站取料吸嘴真空吸
                SetEXO(0, 8, True)      '打开组装站排线吸嘴真空吸
                timeStart = GetTickCount
                Step_Paste = 420

            Case 420
                If EXI(0, 12) And EXI(0, 8) Then
                    'Z轴抬升到待机位置
                    AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                    timeStart = GetTickCount
                    Step_Paste = 440
                ElseIf GetTickCount - timeStart > 3 * 1000 Then
                    '如果真空3S内还是达不到，还是上升到Z待机位置
                    AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                    ListBoxAddMessage("组装站取料吸嘴负压或吸排线负压过小")
                    timeStart = GetTickCount
                    Step_Paste = 440
                End If

            Case 440
                If Not isAxisMoving(0, PasteZ) Then
                    If EXI(0, 12) And EXI(0, 8) Then
                        Step_Paste = 460
                    ElseIf GetTickCount - timeStart > 2 * 1000 Then
                        If EXI(0, 12) = False Then
                            Frm_DialogAddMessage("组装站取料吸嘴负压太小，请检查！")
                        ElseIf EXI(0, 8) = False Then
                            Frm_DialogAddMessage("组装站排线吸嘴负压太小，请检查！")
                        End If
                        '进行到抛料处理程序
                        Step_Paste = 4500
                    End If
                End If

            Case 460  '组装站X,Y,R到定位拍照位置
                AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(2).X)
                AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(2).R)
                If AbsMotion(2, PasteY1, AxisPar.MoveVel(0, PasteY1), Par_Pos.St_Paste(2).Y) = True Then
                    Step_Paste = 480
                End If

            Case 480 'X,Y,R轴到位后就下降Z轴
                If (Not isAxisMoving(0, PasteX)) And (Not isAxisMoving(2, PasteY1)) And (Not isAxisMoving(0, PasteR)) Then
                    AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(2).Z)

                    '处理Cam_OnTransferPlate上面的标志位和数据
                    CurNozTransferPlate = Cam_OnTransferPlate

                    '清除原有吸料中转台的数据
                    Cam_OnTransferPlate.Init()

                    Paste_Sta.workState = 3  '定位拍照中

                    Step_Paste = 500
                End If

            Case 500
                If Not isAxisMoving(0, PasteZ) Then
                    timeStart = GetTickCount
                    Step_Paste = 520
                End If

            Case 520 '停稳后进行定位拍照T3,1
                If GetTickCount - timeStart > 500 Then
                    If TriggerCCD("T3,1", index_InPaste, Tray_Pallet(2).Tray_Barcode, Tray_Pallet(2).Hole(index_InPaste).ProductBarcode) = True Then
                        timeStart = GetTickCount
                        Step_Paste = 540
                    End If
                End If

            Case 540 '进行module定位拍照
                If Winsock1_Data(0) = "T3" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(3) = 1 Then
                        '判定CCD返回值没有超限



                        Step_Paste = 550
                    Else
                        Frm_DialogAddMessage("组装站定位CCD3拍第" & index_InPaste + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("组装站定位CCD3拍第" & index_InPaste + 1 & "颗料超时")
                End If

            Case 550
                '定位拍照完成后，Z轴抬升到安全高度
                AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                Step_Paste = 560

            Case 560
                If Not isAxisMoving(0, PasteZ) Then
                    '判断预固化轴是否在待机位置
                    If Math.Abs(CurrEncPos(BarcodeStrS1, CureX) - Par_Pos.St_Cure(0).X) < 5 Then
                        timeStart = GetTickCount
                        Step_Paste = 600
                    End If
                End If

            Case 600  '运动到精补位置的X,Y,R位置
                AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Cam3Data(1, 0))
                AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Cam3Data(1, 2))
                If AbsMotion(2, PasteY1, AxisPar.MoveVel(0, PasteY1), Cam3Data(1, 1)) = True Then
                    Step_Paste = 620
                End If

            Case 620   '运动到精补位置的Z位置
                If (Not isAxisMoving(0, PasteX)) And (Not isAxisMoving(2, PasteY1)) And (Not isAxisMoving(0, PasteR)) Then
                    Paste_Sta.workState = 4  '精补中
                    'Z轴运行到贴第1颗料精补位置的Z位置
                    AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(3).Z)
                    Step_Paste = 640
                End If

            Case 640  '判断Z轴停止
                If Not isAxisMoving(0, PasteZ) Then
                    timeStart = GetTickCount
                    '清理一次精确补偿的次数
                    Cnt_Buchang = 0
                    Step_Paste = 660
                End If

            Case 660 '精确补正拍照
                If GetTickCount - timeStart > 500 Then
                    If TriggerCCD("T2,2", index_InPaste, Tray_Pallet(2).Tray_Barcode, Tray_Pallet(2).Hole(index_InPaste).ProductBarcode) = True Then
                        timeStart = GetTickCount
                        Step_Paste = 680
                    End If
                End If

            Case 680
                If Winsock1_Data(0) = "T2" And Winsock1_Data(1) = 2 Then
                    If Cam_Status(2) = 1 Then
                        If Cam2Data(2, 3) < CType(par.num(26), Double) Then
                            '精确补偿OK
                            Step_Paste = 800
                        Else
                            Cnt_Buchang = Cnt_Buchang + 1
                            If Cnt_Buchang > par.num(27) Then
                                '精确补偿超次数处理，进行抛料 
                                Step_Paste = 4500
                            Else
                                '再次精确补偿
                                Step_Paste = 700
                            End If
                        End If

                    Else
                        Frm_DialogAddMessage("组装站精补CCD2拍第" & index_InPaste + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("组装站精补CCD2拍第" & index_InPaste + 1 & "颗料物料超时")
                End If

            Case 700
                AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Cam2Data(2, 0))
                AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Cam2Data(2, 2))
                If AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Cam2Data(2, 1)) Then
                    Step_Paste = 720
                End If

            Case 720
                If (Not isAxisMoving(0, PasteX)) And (Not isAxisMoving(2, PasteY1)) And (Not isAxisMoving(0, PasteR)) Then
                    timeStart = GetTickCount
                    Step_Paste = 660
                End If

            Case 800 '精确补偿OK
                If Com1_Send(":O000000o" & vbCrLf) = False Then    'COM1发送打开压力监视
                    Call Frm_Main.COM1_Init(par.CCD(2))
                    Delay(50)
                    Com1_Send(":O000000o" & vbCrLf)
                End If
                Step_Paste = 820

            Case 820
                RstPastePress = Com1_Return() '等待压力传感器打开结束
                If RstPastePress = 0 Then     '表示压力传感器打开成功
                    Step_Paste = 840
                Else
                    ListBoxAddMessage("组装站吸嘴压力传感器打开失败，请查检！")
                End If

            Case 840
                '以3mm/s的速度进行贴合，在COM1接收数据事件中判断压力是否到达，如果到达则立即停止Z轴
                AbsMotion(0, PasteZ, 3, Par_Pos.St_Paste(4).Z)
                Step_Paste = 860

            Case 860  '关闭压力传感器
                If isAxisMoving(0, PasteZ) Then
                    Com1_Send(":Q000000q" & vbCrLf) ' 串口接收数据关闭
                    Step_Paste = 880
                End If

            Case 880
                '判断组装吸嘴头上的两个点UV灯是否连接上
                If Flag_UVConnect(6) = True Then
                    UV_Open(ControllerHandle(6), 1, 255)
                    UV_Open(ControllerHandle(6), 2, 255)
                    timeStart = GetTickCount
                    Step_Paste = 900
                Else
                    ListBoxAddMessage("UV灯控制器6连接失败，请检查！")
                End If

            Case 900
                '关闭UV灯控制器6所有的通道,关闭真空，破真空
                If GetTickCount - timeStart > par.num(18) * 1000 Then
                    UV_Close(ControllerHandle(6), 0)

                    '关闭吸嘴真空和吸排线的真空
                    SetEXO(0, 8, False)
                    SetEXO(0, 12, False)

                    '打开吸嘴破真空和吸排线的破真空
                    SetEXO(0, 9, True)
                    SetEXO(0, 13, True)

                    timeStart = GetTickCount
                    Step_Paste = 920
                End If

            Case 920 'Z轴慢速上抬3mm
                If GetTickCount - timeStart > 500 Then
                    '以3mm/s的速度Z轴上抬3mm
                    AbsMotion(0, PasteZ, 3, Par_Pos.St_Paste(4).Z - 3)
                    Step_Paste = 940
                End If

            Case 940 'Z轴以正常速度上抬到初始位置
                If Not isAxisMoving(0, PasteZ) Then
                    AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                    Step_Paste = 960
                End If

            Case 960 '记录数据
                If Not isAxisMoving(0, PasteZ) Then

                    Tray_Pallet(2).Hole(index_InPaste).Press_Paste = Press(0)
                    Tray_Pallet(2).Hole(index_InPaste).ProductBarcode = CurNozTransferPlate.Barcode
                    Tray_Pallet(2).Hole(index_InPaste).Press_Taker = CurNozTransferPlate.TakerPress
                    Tray_Pallet(2).Hole(index_InPaste).isProductOk = True

                    CurNozTransferPlate.Init()

                    Step_Paste = 4000
                End If
                 
            Case 4000
                '共计12颗料，index_InPaste从0开始
                If index_InPaste < 11 Then
                    index_InPaste += 1
                    Step_Paste = 200 '去贴合下一颗料
                Else
                    Step_Paste = 4020 '工作完成
                End If



                '****************************************************************************************************
                '*****************************************组装站预固化段开始*****************************************
            Case 4020  'X,Y,R回待机位置
                AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(0).X)
                AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(0).R)
                If AbsMotion(2, PasteY1, AxisPar.MoveVel(0, PasteY1), Par_Pos.St_Paste(0).Y) = True Then
                    Step_Paste = 4040
                End If

            Case 4040 '将Index_CurePoint 预固化牵引次数清零
                If (Not isAxisMoving(0, PasteX)) And (Not isAxisMoving(2, PasteY1)) And (Not isAxisMoving(0, PasteR)) Then
                    Index_CurePoint = 0
                    Step_Paste = 4060
                End If

            Case 4060   '用来判断预固化需要走哪些点位
                If Tray_Pallet(2).Hole(2 * Index_CurePoint).isProductOk Or Tray_Pallet(2).Hole(2 * Index_CurePoint + 1).isProductOk Or Tray_Pallet(2).Hole(2 * Index_CurePoint + 6).isProductOk Or Tray_Pallet(2).Hole(2 * Index_CurePoint + 7).isProductOk Then
                    AbsMotion(1, CureX, AxisPar.MoveVel(1, CureX), Par_Pos.St_Cure(Index_CurePoint).X)
                    Step_Paste = 4070
                Else
                    Step_Paste = 4120
                End If

            Case 4070
                If Not isAxisMoving(1, CureX) Then
                    Step_Paste = 4080
                End If

            Case 4080 '打开预固化UV灯
                If Flag_UVConnect(1) And Flag_UVConnect(2) Then
                    If Tray_Pallet(2).Hole(2 * Index_CurePoint).isProductOk Then
                        UV_Open(ControllerHandle(1), 1, 255)
                        UV_Open(ControllerHandle(1), 2, 255)
                    End If

                    If Tray_Pallet(2).Hole(2 * Index_CurePoint + 1).isProductOk Then
                        UV_Open(ControllerHandle(1), 3, 255)
                        UV_Open(ControllerHandle(1), 4, 255)
                    End If

                    If Tray_Pallet(2).Hole(2 * Index_CurePoint + 6).isProductOk Then
                        UV_Open(ControllerHandle(2), 1, 255)
                        UV_Open(ControllerHandle(2), 2, 255)
                    End If

                    If Tray_Pallet(2).Hole(2 * Index_CurePoint + 7).isProductOk Then
                        UV_Open(ControllerHandle(2), 3, 255)
                        UV_Open(ControllerHandle(2), 4, 255)
                    End If

                    timeStart = GetTickCount
                    Step_Paste = 4100
                End If

            Case 4100 '关闭预固化UV灯
                If GetTickCount - timeStart > par.num(19) * 1000 Then
                    UV_Close(ControllerHandle(1), 0)
                    UV_Close(ControllerHandle(2), 0)
                    Step_Paste = 4120
                End If

            Case 4120
                Index_CurePoint = Index_CurePoint + 1
                If Index_CurePoint < 3 Then
                    Step_Paste = 4060
                Else
                    Step_Paste = 4140
                End If

            Case 4140  '预固化轴回待机位置
                AbsMotion(1, CureX, AxisPar.MoveVel(1, CureX), Par_Pos.St_Cure(0).X)
                Step_Paste = 4160

            Case 4160
                If Not isAxisMoving(1, CureX) Then
                    Step_Paste = 4180
                End If

            Case 4180   '整个预固化完成
                Step_Paste = 8000 
                '*****************************************组装站预固化段结束*****************************************
                '****************************************************************************************************




                '****************************************************************************************************
                '*****************************************组装站抛料程序段开始*****************************************
            Case 4500
                Paste_Sta.workState = 5 '工作进行中:抛料
                AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                Step_Paste = 4550

            Case 4550
                If isAxisMoving(0, PasteZ) = False Then
                    Step_Paste = 4570
                End If

            Case 4570
                AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(5).X)
                AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(5).R)
                If AbsMotion(2, PasteY1, AxisPar.MoveVel(0, PasteY1), Par_Pos.St_Paste(5).Y) = True Then
                    Step_Paste = 4590
                End If

            Case 4590
                If isAxisMoving(0, PasteX) = False And isAxisMoving(0, PasteR) = False And isAxisMoving(2, PasteY1) = False Then
                    Step_Paste = 4600
                End If

            Case 4600
                AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(5).Z)
                Step_Paste = 4620

            Case 4620
                If isAxisMoving(0, PasteZ) = False Then
                    '关闭吸嘴真空和吸排线的真空
                    SetEXO(0, 8, False)
                    SetEXO(0, 12, False)

                    '打开吸嘴破真空和吸排线的破真空
                    SetEXO(0, 9, True)
                    SetEXO(0, 13, True)

                    timeStart = GetTickCount
                    Step_Paste = 4640
                End If

            Case 4640
                If GetTickCount - timeStart > 1000 Then
                    AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                    Step_Paste = 4660
                End If

            Case 4660
                If isAxisMoving(0, PasteZ) = False Then
                    Step_Paste = 4680
                End If

            Case 4680
                Step_Paste = 210
                '*****************************************组装站抛料程序段结束*****************************************
                '****************************************************************************************************


            Case 8000
                '组装工站工作完成
                Paste_Sta.isWorking = False    '组装模组工作完成
                Paste_Sta.isNormal = True
                Paste_Sta.workState = 1  '工作完成
                Step_Paste = 10  '开始下一个循环

            Case 9000
                '工作异常需要急停处理
                Paste_Sta.isNormal = False   '组装工站工作异常
                Call Frm_Main.Machine_Stop()
                Step_Paste = 0

        End Select



    End Sub

    ''' <summary>
    ''' 手动单站自动运行贴合工作
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ManualRun_Paste()
        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If
         
        '判断组装站所有轴的状态
        If ServoOn(0, PasteX) And ServoOn(2, PasteY1) And ServoOn(0, PasteZ) And ServoOn(0, PasteR) Then
        Else
            List_DebugAddMessage("请先打开组装工位所有轴伺服ON")
            Exit Sub
        End If
          
        If isAxisMoving(0, PasteX) Or isAxisMoving(0, PasteZ) Or isAxisMoving(0, PasteR) Or isAxisMoving(2, PasteY1) Then
            List_DebugAddMessage("组装工位有轴正在运动中，请等待")
            Exit Sub
        End If
         
        '判断精补站所有轴的状态
        If ServoOn(1, FineX) And ServoOn(1, FineY) Then
        Else
            List_DebugAddMessage("请先打开精补工位所有轴伺服ON")
            Exit Sub
        End If
         
        If isAxisMoving(1, FineX) Or isAxisMoving(1, FineY) Then
            List_DebugAddMessage("精补工位有轴正在运动中，请等待")
            Exit Sub
        End If
         
        '判断预固化轴的状态
        If ServoOn(1, CureX) Then
        Else
            List_DebugAddMessage("请先打开CureX轴伺服ON")
            Exit Sub
        End If
         
        If isAxisMoving(1, CureX) Then
            List_DebugAddMessage("CureX轴正在运动中，请等待")
            Exit Sub
        End If
         
        '检查取料站的Y轴是否在安全位置
        If Math.Abs(CurrEncPos(2, PreTakerY1) - Par_Pos.St_PreTaker(0).Y) > 2 Then
            List_DebugAddMessage("请检查取料模组是否在安全位置！")
            Exit Sub
        End If

        '检察预固化轴是否在安全位置
        If Math.Abs(CurrEncPos(1, CureX) - Par_Pos.St_Cure(0).X) > 2 Then
            List_DebugAddMessage("请检查预固化轴是否在安全位置！")
            Exit Sub
        End If








        If isHaveTray(2) = False And par.chkFn(4) = False Then
            List_DebugAddMessage("流水线上无载具，且不是演示装配，已退出单站自动运行！")
            Exit Sub
        End If

        If (EMI(2, 9) = False Or isCylinderRised(2) = False) And par.chkFn(4) = False Then
            List_DebugAddMessage("流水线上吸载具或顶升气缸异常，且不是演示装配，已退出单站自动运行！")
            Exit Sub
        End If

        Line_Sta(2).workState = 2   '接收载具完成，等待点胶
        Line_Sta(2).isHaveTray = True

        Tray_Pallet(1).isHaveTray = True : Tray_Pallet(1).isTrayOK = True : Tray_Pallet(1).Tray_Barcode = Format(Now, "yyyyMMddHHmmss")

        For index = 0 To Tray_Pallet(2).Hole.Count - 1
            If index = Val(Frm_Engineering.txt_MaterialSelected.Text) Then
                Tray_Pallet(2).Hole(index).isHaveProduct = True : Tray_Pallet(2).Hole(index).isProductOk = True
                Tray_Pallet(2).Hole(index).ProductBarcode = "Hole" & index
            Else
                Tray_Pallet(2).Hole(index).isHaveProduct = False : Tray_Pallet(2).Hole(index).isProductOk = False
                Tray_Pallet(2).Hole(index).ProductBarcode = "Hole" & index
            End If
        Next

        Step_Paste = 10

        Do While True
            My.Application.DoEvents()
            Delay(10)

            '自动配合程序
            Call AutoRun_PasteStation()

            If Paste_Sta.isWorking = False And Paste_Sta.workState = 1 Then
                List_DebugAddMessage("2段流水线单站自动运行完成！")
                Exit Do
            End If

            If IsSysEmcStop Or Paste_Sta.isNormal = False Then
                List_DebugAddMessage("2段流水线单站自动运行异常，停止自动运行！")
                Exit Do
            End If
        Loop
          
    End Sub
End Module
