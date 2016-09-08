Module L1Run

    '1段流水线用于接收载具，点胶
    Public Sub L1_AutoRun()
        'Line_Sta(1).workState = 0 '接收载具完成，等待点胶
        'Line_Sta(1).workState = 1 '接收载具完成，点胶完成
        Static timeStart As Long    '记录开始时间

        Select Case Step_Line(1)
            Case 10
                '0 段流水线工作完成，1段流水线无载具，可以接收载具
                If Flag_MachineStop = False And Line_Sta(0).isWorking = False And Line_Sta(0).isHaveTray And Line_Sta(1).isHaveTray = False Then
                    Call setMotorRun(1)  '第一段流水线接收载具
                    ListBoxAddMessage(">> 1 段流水线开始接收载具")
                    Step_Line(1) = 100
                End If

            Case 100
                If isHaveTray(1) Or par.chkFn(4) Then
                    Tray_Pallet(1) = Tray_Pallet(0) '数据传递到本工位
                    Line_Sta(1).isHaveTray = True
                    Line_Sta(1).isWorking = True
                    ListBoxAddMessage(">> 1 段流水线已经接收到载具")
                    Step_Line(1) = 200
                End If

            Case 200
                timeStart = GetTickCount
                Step_Line(1) = 300

            Case 300
                If Tray_Pallet(1).isTrayOK = False Then
                    Step_Line(1) = 800 '载具不良直接流走
                ElseIf isTimeout(timeStart, 300) Then
                    Call setCylinderRise(1, True)
                    SetEMO(1, 8, True)  'L1真空吸载具
                    Call setMotorStop(1)
                    timeStart = GetTickCount
                    Step_Line(1) = 400
                End If

            Case 400
                '判断顶升气缸是否升起
                If isCylinderRised(1) Then
                    If par.chkFn(4) Then
                        timeStart = GetTickCount
                        Step_Line(1) = 500
                    Else
                        If EMI(1, 8) Then
                            timeStart = GetTickCount
                            Step_Line(1) = 500
                        ElseIf isTimeout(timeStart, 4000) Then
                            SetEMO(1, 8, False)  'L1真空吸载具
                            Call setCylinderRise(1, False)
                            Frm_DialogAddMessage("1段流水线真空吸载具负压异常！")
                            Tray_Pallet(1).isTrayOK = False
                            Step_Line(1) = 800 '载具不良直接流走
                        End If
                    End If
                ElseIf isTimeout(timeStart, 2000) Then
                    SetEMO(1, 8, False)  'L1真空吸载具
                    Call setCylinderRise(1, False)
                    Frm_DialogAddMessage("1段流水线升降气缸升起超时！")
                    Tray_Pallet(1).isTrayOK = False
                    Step_Line(1) = 800 '载具不良直接流走
                End If

            Case 500
                Line_Sta(1).workState = 0 '接收载具完成，等待点胶
                GLue_Sta.workState = 0  '点胶进行中
                Step_Line(1) = 600

            Case 600
                '等待点胶工站工作完成
                If GLue_Sta.isWorking = False And GLue_Sta.workState = 1 Then
                    Step_Line(1) = 700
                End If

            Case 700
                SetEMO(1, 8, False)  'L1真空吸载具
                Call setCylinderRise(1, False)
                Step_Line(1) = 800

            Case 800
                '1 段流水线工作完成，等待第2段流水线接收载具
                Line_Sta(1).isWorking = False
                Line_Sta(1).workState = 1 '1 段流水线工作完成(点胶工作OK)
                If Flag_MachineStop = False And Line_Sta(2).isWorking = False And Line_Sta(2).isHaveTray = False Then
                    Call setBlock(1, False)
                    Call setMotorRun(1)
                    ListBoxAddMessage(">> 1 段流水线开始发送载具")
                    Step_Line(1) = 900
                End If

            Case 900
                If isHaveTray(2) Or Line_Sta(2).isHaveTray Then  '2段流水线已经接收到载具
                    Step_Line(1) = 1000
                End If

            Case 1000
                If isHaveTray(2) Or Line_Sta(2).isHaveTray Then  '2段流水线已经接收到载具
                    Call setBlock(1, True)
                    Call setMotorStop(1)
                    timeStart = GetTickCount
                    Step_Line(1) = 1100
                End If

            Case 1100
                If isBlocked(1) Then
                    Line_Sta(1).isHaveTray = False
                    Line_Sta(1).isNormal = True
                    Step_Line(1) = 10   '开始下一循环
                ElseIf isTimeout(timeStart, 2000) Then
                    Step_Line(1) = 9000
                End If

            Case 9000
                '异常，急停处理
                Call Frm_Main.Machine_Stop()

        End Select
    End Sub
End Module
