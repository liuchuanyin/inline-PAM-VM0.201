Module L2Run
    '2段流水线用于接收载具，贴合
    Public Sub L2_AutoRun()
        'Line_Sta(2).workState = 1 '接收载具完成，贴合完成
        'Line_Sta(2).workState = 2 '接收载具完成，等待贴合完成
        Static timeStart As Long    '记录开始时间

        Select Case Step_Line(2)
            Case 10
                '1 段流水线工作完成，2段流水线无载具，可以接收载具
                If Flag_MachineStop = False And Line_Sta(1).isWorking = False And Line_Sta(1).isHaveTray And Line_Sta(2).isHaveTray = False Then
                    Call setMotorRun(2)  '第2段流水线接收载具
                    ListBoxAddMessage(">> 2 段流水线开始接收载具")
                    Step_Line(2) = 100
                End If

            Case 100
                If isHaveTray(2) Or par.chkFn(4) Then
                    Tray_Pallet(2) = Tray_Pallet(1) '数据传递到本工位
                    Line_Sta(2).isHaveTray = True
                    Line_Sta(2).isWorking = True
                    ListBoxAddMessage(">> 2 段流水线已经接收到载具")
                    Step_Line(2) = 200
                End If

            Case 200
                timeStart = GetTickCount
                Step_Line(2) = 300

            Case 300
                If Tray_Pallet(2).isTrayOK = False Then
                    Step_Line(2) = 800 '载具不良直接流走
                ElseIf isTimeout(timeStart, 300) Then
                    Call setCylinderRise(2, True)
                    SetEMO(1, 9, True)  'L2真空吸载具
                    Call setMotorStop(2)
                    timeStart = GetTickCount
                    Step_Line(2) = 400
                End If

            Case 400
                '判断顶升气缸是否升起
                If isCylinderRised(2) Then
                    If par.chkFn(4) Then
                        timeStart = GetTickCount
                        Step_Line(2) = 500
                    Else
                        If EMI(1, 9) Then
                            timeStart = GetTickCount
                            Step_Line(2) = 500
                        ElseIf isTimeout(timeStart, 4000) Then
                            SetEMO(1, 9, False)  'L2真空吸载具
                            Call setCylinderRise(2, False)
                            Frm_DialogAddMessage("2段流水线真空吸载具负压异常！")
                            Tray_Pallet(2).isTrayOK = False
                            Step_Line(2) = 800 '载具不良直接流走
                        End If
                    End If
                ElseIf isTimeout(timeStart, 2000) Then
                    SetEMO(1, 9, False)  'L2真空吸载具
                    Call setCylinderRise(2, False)
                    Frm_DialogAddMessage("2段流水线升降气缸升起超时！")
                    Tray_Pallet(2).isTrayOK = False
                    Step_Line(2) = 800 '载具不良直接流走
                End If

            Case 500
                Line_Sta(2).workState = 2 '接收载具完成，等待贴合
                Paste_Sta.workState = 0  '贴合进行中
                Step_Line(2) = 600

            Case 600
                '等待组装工站工作完成
                If Paste_Sta.isWorking = False And Paste_Sta.workState = 1 Then
                    Step_Line(2) = 700
                End If

            Case 700
                SetEMO(1, 9, False)  'L2真空吸载具
                Call setCylinderRise(2, False)
                Step_Line(2) = 800

            Case 800
                '2 段流水线工作完成，等待第3段流水线接收载具
                Line_Sta(2).isWorking = False
                Line_Sta(2).workState = 1 '2 段流水线工作完成(组装工作OK)
                If Flag_MachineStop = False And Line_Sta(3).isWorking = False And Line_Sta(3).isHaveTray = False Then
                    Call setBlock(2, False)
                    Call setMotorRun(2)
                    ListBoxAddMessage(">> 2 段流水线开始发送载具")
                    Step_Line(2) = 900
                End If

            Case 900
                If isHaveTray(3) Or Line_Sta(3).isHaveTray Then  '3段流水线已经接收到载具
                    Step_Line(2) = 1000
                End If

            Case 1000
                If isHaveTray(3) Or Line_Sta(3).isHaveTray Then  '3段流水线已经接收到载具
                    Call setBlock(2, True)
                    Call setMotorStop(2)
                    timeStart = GetTickCount
                    Step_Line(2) = 1100
                End If

            Case 1100
                If isBlocked(2) Then
                    Line_Sta(2).isHaveTray = False
                    Line_Sta(2).isNormal = True
                    Step_Line(2) = 10   '开始下一循环
                ElseIf isTimeout(timeStart, 2000) Then
                    Step_Line(2) = 9000
                End If

            Case 9000
                '异常，急停处理
                Call Frm_Main.Machine_Stop()

        End Select
    End Sub
End Module
