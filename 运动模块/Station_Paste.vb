Module Station_Paste

    Public Step_Paste As Integer
    '产品索引号
    Public index_InPaste As Short
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
                    Call AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(index).Y)
                    Step_Gopos(2) = 30

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

    Public Sub AutoRun_PasteStation()
        ' Paste_Sta.workState =0 工作进行中
        ' Paste_Sta.workState =1 工作完成
        ' Paste_Sta.workState =2 工作进行中:取料
        ' Paste_Sta.workState =3 工作进行中:定位拍照
        ' Paste_Sta.workState =4 工作进行中:精补贴合
        ' Paste_Sta.workState =5 工作进行中:抛料
        ' Paste_Sta.workState =6 工作进行中:等待取料机构向中转机构上放料
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
                    Step_Paste = 210
                Else
                    '否则跳过这一颗料
                    ListBoxAddMessage("组装站组装跳过第" & index_InPaste + 1 & "颗料！")
                    Step_Paste = 2000
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
                    Call AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(0).Y)
                    Step_Paste = 230
                End If

            Case 230
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

            Case 300
                Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                Step_Paste = 310

            Case 310
                '组装模组去取料位置取料
                If isAxisMoving(0, PasteZ) = False Then
                    '运动到取料位置
                    Call AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(1).X)
                    Call AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(1).R)
                    Call AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(1).Y)
                    Step_Paste = 330
                End If

            Case 330
                If isAxisMoving(0, PasteR) = False And isAxisMoving(0, PasteX) = False And isAxisMoving(2, PasteY1) = False Then
                    ListBoxAddMessage("组装站X、Y、R轴运动到取料位置")
                    Step_Paste = 350
                End If

            Case 350
                Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(1).Z)
                Step_Paste = 360

            Case 360
                If isAxisMoving(0, PasteZ) = False Then
                    ListBoxAddMessage("组装站Z轴运动到取料位置")
                    Step_Paste = 400
                End If

            Case 400
                '取料，条码处理？？？？？？？？？？？？？


            Case 2000
                '共计12颗料，index_InPaste从0开始
                If index_InPaste < 11 Then
                    index_InPaste += 1
                    Step_Paste = 200 '去贴合下一颗料
                Else
                    Step_Paste = 8000 '工作完成
                End If

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

End Module
