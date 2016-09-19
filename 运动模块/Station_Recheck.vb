Module Station_Recheck
    Public Step_Recheck As Integer
    Public index_InRecheck As Short

    Public Sub GoPos_Recheck(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, RecheckX) And ServoOn(1, RecheckY) Then
        Else
            List_DebugAddMessage("请先打开复检工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, RecheckX) Or isAxisMoving(1, RecheckY) Then
            List_DebugAddMessage("复检工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Step_Gopos(5) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(5)
                Case 0
                    Call AbsMotion(1, RecheckX, AxisPar.MoveVel(1, RecheckX), Par_Pos.St_Recheck(index).X)
                    Call AbsMotion(1, RecheckY, AxisPar.MoveVel(1, RecheckY), Par_Pos.St_Recheck(index).Y)
                    Step_Gopos(5) = 10

                Case 10
                    If isAxisMoving(1, RecheckX) = False And isAxisMoving(1, RecheckY) = False Then
                        Frm_DialogAddMessage("精补模组运动到" & Par_Pos.St_Recheck(index).Name & "完成！")
                        Step_Gopos(5) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub


    Public Sub AutoRun_Recheck()
        'Recheck_Sta.workState = 0  '工作进行中
        'Recheck_Sta.workState = 1  '工作完成
        'Recheck_Sta.workState = 2  '工作进行中：复检
        'Recheck_Sta.workState = 3  '工作进行中：传数据等
        Static timeStart As Long    '记录开始时间

        '复检UV固化分为气缸的两个位置，用来判断这两个位置是否需要固化
        Static UV_Area(1) As Boolean

        '复检UV固化每个位置有6个点，用来判断这6个点中哪些是需要固化的
        Static UV_Point(1, 5) As Boolean


        '复检站暂停功能控制，但是UV灯有开启时不允许暂停，关闭UV后再自动暂停
        If Flag_MachinePause = True Then
            If Not (Flag_UVIsOpened(3) Or Flag_UVIsOpened(4) Or Flag_UVIsOpened(5) Or Flag_UVIsOpened(7)) Then
                Exit Sub
            End If
        End If

        Select Case Step_Recheck
            Case 10
                If Flag_MachineStop = False And Line_Sta(3).workState = 2 And _
                Line_Sta(3).isHaveTray = True And isTimeout(cmd_SendTime, 2) Then
                    Recheck_Sta.isNormal = True : Recheck_Sta.isWorking = True : Recheck_Sta.workState = 0    '组装模组工作进行中
                    Step_Recheck = 100
                End If

            Case 50
                '流水线上有载具，且载具可用
                If Tray_Pallet(3).isHaveTray And Tray_Pallet(3).isTrayOK Then
                    index_InRecheck = 0

                    '清除UV固化相关标志位
                    UV_Area(0) = False
                    UV_Area(1) = False

                    For i = 0 To 1
                        For j = 0 To 5
                            UV_Point(i, j) = False
                        Next
                    Next

                    Step_Recheck = 70
                End If

            Case 70 '用来判断哪些位置需要固化哪些位置不需要固化
                For i = 0 To Tray_Pallet(3).Hole.Length - 1
                    If Tray_Pallet(3).Hole(i).isHaveProduct And Tray_Pallet(3).Hole(i).isProductOk Then
                        If i <= 5 Then
                            UV_Area(0) = True
                            UV_Point(0, i) = True
                        Else
                            UV_Area(1) = True
                            UV_Point(1, i) = True
                        End If
                    End If
                Next

                Step_Recheck = 80

            Case 80
                If UV_Area(0) = True Then
                    SetEMO(0, 11, False) '防呆先自动关闭一次
                    Step_Recheck = 90
                Else
                    Step_Recheck = 130
                End If

            Case 90
                If EMI(0, 12) Then  '判断UV固化移动气缸磁簧处于缩回位置
                    Step_Recheck = 100
                Else
                    ListBoxAddMessage("UV固化平移气缸不在缩回位置，请确认！")
                End If

            Case 100    '用来判断UV灯控制器是否已经连接上
                If Flag_UVConnect(3) And Flag_UVConnect(4) And Flag_UVConnect(5) Then
                    Step_Recheck = 110
                ElseIf Flag_UVConnect(3) = False Then
                    ListBoxAddMessage("复检站3号UV灯控制器未连接上")
                ElseIf Flag_UVConnect(4) = False Then
                    ListBoxAddMessage("复检站4号UV灯控制器未连接上")
                ElseIf Flag_UVConnect(5) = False Then
                    ListBoxAddMessage("复检站5号UV灯控制器未连接上")
                End If

            Case 110
                If UV_Point(0, 0) Then
                    UV_Open(ControllerHandle(3), 1, 255)
                    UV_Open(ControllerHandle(3), 2, 255)
                ElseIf UV_Point(0, 1) Then
                    UV_Open(ControllerHandle(3), 3, 255)
                    UV_Open(ControllerHandle(3), 4, 255)
                ElseIf UV_Point(0, 2) Then
                    UV_Open(ControllerHandle(4), 1, 255)
                    UV_Open(ControllerHandle(4), 2, 255)
                ElseIf UV_Point(0, 3) Then
                    UV_Open(ControllerHandle(4), 3, 255)
                    UV_Open(ControllerHandle(4), 4, 255)
                ElseIf UV_Point(0, 4) Then
                    UV_Open(ControllerHandle(5), 1, 255)
                    UV_Open(ControllerHandle(5), 2, 255)
                ElseIf UV_Point(0, 5) Then
                    UV_Open(ControllerHandle(5), 3, 255)
                    UV_Open(ControllerHandle(5), 4, 255)
                End If
                timeStart = GetTickCount
                Step_Recheck = 120


            Case 120  '关闭UV灯控制器3，4，5的所有通道
                If GetTickCount - timeStart > par.num(20) * 1000 Then
                    UV_Close(ControllerHandle(3), 0)
                    UV_Close(ControllerHandle(4), 0)
                    UV_Close(ControllerHandle(5), 0)
                    Step_Recheck = 130
                End If

            Case 130
                If UV_Area(1) = True Then
                    SetEMO(0, 11, True)   '置位UV灯平移气缸
                    timeStart = GetTickCount
                    Step_Recheck = 140
                Else
                    Step_Recheck = 200
                End If

            Case 140
                If EMI(0, 11) Then
                    Step_Recheck = 150
                ElseIf GetTickCount - timeStart > 5000 Then
                    ListBoxAddMessage("复检站UV灯平移气缸伸出动作超时")
                End If

            Case 150     '用来判断UV灯控制器是否已经连接上
                If Flag_UVConnect(3) And Flag_UVConnect(4) And Flag_UVConnect(5) Then
                    Step_Recheck = 160
                ElseIf Flag_UVConnect(3) = False Then
                    ListBoxAddMessage("3号UV灯控制器未连接上")
                ElseIf Flag_UVConnect(4) = False Then
                    ListBoxAddMessage("4号UV灯控制器未连接上")
                ElseIf Flag_UVConnect(5) = False Then
                    ListBoxAddMessage("5号UV灯控制器未连接上")
                End If

            Case 160
                If UV_Point(1, 0) Then
                    UV_Open(ControllerHandle(3), 1, 255)
                    UV_Open(ControllerHandle(3), 2, 255)
                ElseIf UV_Point(1, 1) Then
                    UV_Open(ControllerHandle(3), 3, 255)
                    UV_Open(ControllerHandle(3), 4, 255)
                ElseIf UV_Point(1, 2) Then
                    UV_Open(ControllerHandle(4), 1, 255)
                    UV_Open(ControllerHandle(4), 2, 255)
                ElseIf UV_Point(1, 3) Then
                    UV_Open(ControllerHandle(4), 3, 255)
                    UV_Open(ControllerHandle(4), 4, 255)
                ElseIf UV_Point(1, 4) Then
                    UV_Open(ControllerHandle(5), 1, 255)
                    UV_Open(ControllerHandle(5), 2, 255)
                ElseIf UV_Point(1, 5) Then
                    UV_Open(ControllerHandle(5), 3, 255)
                    UV_Open(ControllerHandle(5), 4, 255)
                End If
                timeStart = GetTickCount
                Step_Recheck = 170

            Case 170  '关闭UV灯控制器3，4，5的所有通道
                If GetTickCount - timeStart > par.num(20) * 1000 Then
                    UV_Close(ControllerHandle(3), 0)
                    UV_Close(ControllerHandle(4), 0)
                    UV_Close(ControllerHandle(5), 0)
                    Step_Recheck = 180
                End If

            Case 180
                SetEMO(0, 11, False)   '复位UV灯平移气缸
                timeStart = GetTickCount
                Step_Recheck = 190

            Case 190
                If EMI(0, 12) Then
                    Step_Recheck = 200
                ElseIf GetTickCount - timeStart > 5000 Then
                    ListBoxAddMessage("复检站UV灯平移气缸缩回动作超时")
                End If

            Case 200
                If Tray_Pallet(3).Hole(index_InRecheck).isHaveProduct And Tray_Pallet(3).Hole(index_InRecheck).isProductOk And Frm_Engineering.chk_Brc(index_InRecheck).Checked Then
                    '当前穴位有料，且是OK的，并且选中要做这个
                    Step_Recheck = 420
                Else
                    '否则跳过这一颗料
                    ListBoxAddMessage("复检站跳过第" & index_InRecheck + 1 & "颗料！")
                    Step_Recheck = 7000
                End If

            Case 420
                AbsMotion(1, RecheckX, AxisPar.MoveVel(1, RecheckX), TrayMatrix.TrayRecheck(index_InRecheck).X)
                AbsMotion(1, RecheckX, AxisPar.MoveVel(1, RecheckX), TrayMatrix.TrayRecheck(index_InRecheck).Y)
                Step_Recheck = 440

            Case 440
                If isAxisMoving(1, RecheckX) = False And isAxisMoving(1, RecheckY) = False Then
                    If Flag_UVConnect(7) Then
                        UV_Open(ControllerHandle(7), 1, 255)
                        UV_Open(ControllerHandle(7), 2, 255)
                        timeStart = GetTickCount
                        Step_Recheck = 450
                    Else
                        ListBoxAddMessage("复检站UV灯控制器7未连接上")
                    End If
                End If

            Case 450  'UV灯打开后0.1S再拍照 
                If GetTickCount - timeStart > 100 Then
                    Step_Recheck = 460
                End If

            Case 460
                If TriggerCCD("T5,1", index_InRecheck, Tray_Pallet(3).Tray_Barcode, Tray_Pallet(3).Hole(index_InRecheck).ProductBarcode) Then
                    timeStart = GetTickCount
                    Step_Recheck = 480
                End If

            Case 480
                If Winsock1_Data(0) = "T5" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(5) = 1 Then
                        Step_Recheck = 500
                    Else
                        Frm_DialogAddMessage("复检站CCD5拍第" & index_InRecheck + 1 & "颗料物料异常，请检查有无产品")
                    End If
                    '关闭UV灯控制器7
                    UV_Close(ControllerHandle(7), 0)
                ElseIf GetTickCount - timeStart > 3000 Then
                    '关闭UV灯控制器7
                    UV_Close(ControllerHandle(7), 0)
                    Frm_DialogAddMessage("复检站CCD5拍第" & index_InRecheck + 1 & "颗料物料超时")
                End If

            Case 500
                If MACTYPE = "PAM-B" Then
                    Step_Recheck = 520
                Else
                    Step_Recheck = 700
                End If

            Case 520
                If TriggerCCD("T6,1", index_InRecheck, Tray_Pallet(3).Tray_Barcode, Tray_Pallet(3).Hole(index_InRecheck).ProductBarcode) Then
                    timeStart = GetTickCount
                    Step_Recheck = 540
                End If

            Case 540
                If Winsock1_Data(0) = "T6" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(6) = 1 Then
                        Step_Recheck = 560
                    Else
                        Frm_DialogAddMessage("复检站CCD6拍第" & index_InRecheck + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("复检站CCD6拍第" & index_InRecheck + 1 & "颗料物料超时")
                End If

            Case 560
                Step_Recheck = 700

            Case 700 '处理复检的拍照数据
                If MACTYPE = "PAM-1" Then

                ElseIf MACTYPE = "PAM-2" Then

                ElseIf MACTYPE = "PAM-3" Then

                ElseIf MACTYPE = "PAM-B" Then

                End If

                '处理完数据后开始下一颗料的复检
                Step_Recheck = 7000

            Case 7000
                '共计12颗料，index从0开始
                If index_InRecheck < 11 Then
                    index_InRecheck += 1
                    Step_Recheck = 200 '去下一颗料点胶
                Else
                    Step_Recheck = 8000 '点胶完成
                End If

            Case 8000
                '复检工位正常工作完成
                Recheck_Sta.isWorking = False    '点胶模组工作完成
                Recheck_Sta.isNormal = True
                Recheck_Sta.workState = 1  '工作完成
                Step_Recheck = 10  '开始下一个循环

            Case 9000
                '工作异常需要急停处理
                Recheck_Sta.isNormal = False   '点胶工站工作异常
                Call Frm_Main.Machine_Stop()
                Step_Recheck = 0
        End Select
    End Sub

End Module
