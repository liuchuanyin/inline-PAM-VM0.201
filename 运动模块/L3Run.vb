Module L3Run
    '3段流水线用于接收载具，贴合
    Public Sub L3_AutoRun()
        'Line_Sta(3).workState = 1 '接收载具完成，贴合完成
        'Line_Sta(3).workState = 2 '接收载具完成，等待复检完成
        Static timeStart As Long    '记录开始时间

        Select Case Step_Line(3)
            Case 10
                '2 段流水线工作完成，3段流水线无载具，可以接收载具
                If Flag_MachineStop = False And Line_Sta(2).isWorking = False And Line_Sta(2).isHaveTray And Line_Sta(3).isHaveTray = False Then
                    Call setMotorRun(3)  '第3段流水线接收载具
                    ListBoxAddMessage(">> 3 段流水线开始接收载具")
                    Step_Line(3) = 100
                End If

            Case 100
                If isHaveTray(3) Or par.chkFn(4) Then
                    Tray_Pallet(3) = Tray_Pallet(2) '数据传递到本工位
                    Line_Sta(3).isHaveTray = True
                    Line_Sta(3).isWorking = True
                    ListBoxAddMessage(">> 3 段流水线已经接收到载具")
                    Step_Line(3) = 200
                End If

            Case 200
                timeStart = GetTickCount
                Step_Line(3) = 300

            Case 300
                If Tray_Pallet(3).isTrayOK = False Then
                    Step_Line(3) = 800 '载具不良直接流走
                ElseIf isTimeout(timeStart, 300) Then
                    Call setCylinderRise(3, True)
                    SetEMO(1, 10, True)  'L3真空吸载具
                    Call setMotorStop(3)
                    timeStart = GetTickCount
                    Step_Line(3) = 400
                End If

            Case 400
                '判断顶升气缸是否升起
                If isCylinderRised(3) Then
                    If par.chkFn(4) Then
                        timeStart = GetTickCount
                        Step_Line(3) = 500
                    Else
                        If EMI(1, 10) Then
                            timeStart = GetTickCount
                            Step_Line(3) = 500
                        ElseIf isTimeout(timeStart, 4000) Then
                            SetEMO(1, 10, False)  'L3真空吸载具
                            Call setCylinderRise(3, False)
                            Frm_DialogAddMessage("3段流水线真空吸载具负压异常！")
                            Tray_Pallet(3).isTrayOK = False
                            Step_Line(3) = 800 '载具不良直接流走
                        End If
                    End If
                ElseIf isTimeout(timeStart, 2000) Then
                    SetEMO(1, 10, False)  'L3真空吸载具
                    Call setCylinderRise(3, False)
                    Frm_DialogAddMessage("3段流水线升降气缸升起超时！")
                    Tray_Pallet(3).isTrayOK = False
                    Step_Line(3) = 800 '载具不良直接流走
                End If

            Case 500
                Line_Sta(3).workState = 2 '接收载具完成，等待复检
                Recheck_Sta.workState = 0  '复检进行中
                Step_Line(3) = 600

            Case 600
                '等待组装工站工作完成
                If Recheck_Sta.isWorking = False And Recheck_Sta.workState = 1 Then
                    Step_Line(3) = 700
                End If

            Case 700
                SetEMO(1, 10, False)  'L3真空吸载具
                Call setCylinderRise(3, False)
                Step_Line(3) = 800

                'Case 800
                '    '3 段流水线工作完成，等待第3段流水线接收载具
                '    Line_Sta(3).isWorking = False
                '    Line_Sta(3).workState = 1 '3 段流水线工作完成(组装工作OK)
                '    If Flag_MachineStop = False And Line_Sta(3).isWorking = False And Line_Sta(3).isHaveTray = False Then
                '        Call setBlock(2, False)
                '        Call setMotorRun(2)
                '        ListBoxAddMessage(">> 2 段流水线开始发送载具")
                '        Step_Line(3) = 900
                '    End If

                'Case 900
                '    If isHaveTray(3) Or Line_Sta(3).isHaveTray Then  '2段流水线已经接收到载具
                '        Step_Line(3) = 1000
                '    End If

                'Case 1000
                '    If isHaveTray(3) Or Line_Sta(3).isHaveTray Then  '2段流水线已经接收到载具
                '        Call setBlock(2, True)
                '        Call setMotorStop(2)
                '        timeStart = GetTickCount
                '        Step_Line(3) = 1100
                '    End If

            Case 1100
                If isBlocked(3) Then
                    Line_Sta(3).isHaveTray = False
                    Line_Sta(3).isNormal = True
                    Step_Line(3) = 10   '开始下一循环
                ElseIf isTimeout(timeStart, 2000) Then
                    Step_Line(3) = 9000
                End If

            Case 9000
                '异常，急停处理
                Call Frm_Main.Machine_Stop()

        End Select
    End Sub
End Module
