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

        Select Case Step_Recheck
            Case 10
                If Flag_MachineStop = False And Line_Sta(3).workState = 2 And _
              Line_Sta(3).isHaveTray = True And isTimeout(cmd_SendTime, 2) Then
                    Recheck_Sta.isNormal = True : Recheck_Sta.isWorking = True : Recheck_Sta.workState = 0    '组装模组工作进行中
                    Step_Recheck = 100
                End If

            Case 100
                '流水线上有载具，且载具可用
                If Tray_Pallet(3).isHaveTray And Tray_Pallet(3).isTrayOK Then
                    index_InRecheck = 0
                    Step_Recheck = 200
                End If

            Case 200
                If Tray_Pallet(3).Hole(index_InRecheck).isHaveProduct And Tray_Pallet(3).Hole(index_InRecheck).isProductOk And Frm_Engineering.chk_Brc(index_InRecheck).Checked Then
                    '当前穴位有料，且是OK的，并且选中要做这个
                    Step_Recheck = 220
                Else
                    '否则跳过这一颗料
                    ListBoxAddMessage("复检站跳过第" & index_InRecheck + 1 & "颗料！")
                    Step_Recheck = 7000
                End If

            Case 220
                AbsMotion(1, RecheckX, AxisPar.MoveVel(1, RecheckX), TrayMatrix.TrayRecheck(index_InRecheck).X)
                AbsMotion(1, RecheckX, AxisPar.MoveVel(1, RecheckX), TrayMatrix.TrayRecheck(index_InRecheck).Y)
                Step_Recheck = 240

            Case 240
                If isAxisMoving(1, RecheckX) = False And isAxisMoving(1, RecheckY) = False Then
                    Step_Recheck = 260
                End If

            Case 260
                If TriggerCCD("T5,1", index_InRecheck, Tray_Pallet(3).Tray_Barcode, Tray_Pallet(3).Hole(index_InRecheck).ProductBarcode) Then
                    timeStart = GetTickCount
                    Step_Recheck = 280
                End If

            Case 280
                If Winsock1_Data(0) = "T5" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(5) = 1 Then
                        Step_Recheck = 300
                    Else
                        Frm_DialogAddMessage("复检站CCD5拍第" & index_InRecheck + 1 & "颗料物料异常，请检查有无产品") 
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("复检站CCD5拍第" & index_InRecheck + 1 & "颗料物料超时")
                End If

            Case 300
                If MACTYPE = "PAM-B" Then
                    Step_Recheck = 320
                Else
                    Step_Recheck = 500
                End If

            Case 320
                If TriggerCCD("T6,1", index_InRecheck, Tray_Pallet(3).Tray_Barcode, Tray_Pallet(3).Hole(index_InRecheck).ProductBarcode) Then
                    timeStart = GetTickCount
                    Step_Recheck = 340
                End If

            Case 340
                If Winsock1_Data(0) = "T6" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(6) = 1 Then
                        Step_Recheck = 360
                    Else
                        Frm_DialogAddMessage("复检站CCD6拍第" & index_InRecheck + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("复检站CCD6拍第" & index_InRecheck + 1 & "颗料物料超时")
                End If

            Case 360
                Step_Recheck = 500

            Case 500 '处理复检的拍照数据
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
