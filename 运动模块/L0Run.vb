Module L0Run

    '0段流水线用于接收载具，扫载具条码，并从服务器抓取数据
    Public Sub L0_AutoRun()
        Static timeStart As Long    '记录开始时间

        Select Case Step_Line(0)
            Case 10
                '判断是否开启联机模式
                If par.chkFn(21) Then
                    If EMI(1, 14) And isHaveTray(0) = False And Line_Sta(0).isWorking = False Then
                        SetEMO(1, 13, True) '给上一台设备可以接受载具信号
                        Step_Line(0) = 100
                    End If
                Else
                    If isHaveTray(0) = True And Line_Sta(0).isWorking = False Then
                        Step_Line(0) = 100
                    End If
                End If

            Case 100
                setBlock(0, True)   '第0段流水线阻挡气缸阻挡
                setMotorRun(0)
                Line_Sta(0).isWorking = True
                timeStart = GetTickCount
                ListBoxAddMessage(">> 0 段流水线开始接收载具")
                Step_Line(0) = 200

            Case 200
                If par.chkFn(4) = True And Flag_MachineStop = False Then
                    Step_Line(0) = 210
                Else
                    If isHaveTray(0) And Flag_MachineStop = False Then
                        SetEXO(1, 15, True)   '打开真空泵
                        Step_Line(0) = 210
                    ElseIf isTimeout(timeStart, 5000) Then
                        ListBoxAddMessage(">> 0 段流水线接收载具超时")
                        Line_Sta(0).isWorking = False
                        Step_Line(0) = 10   '超时去下一个循环
                    End If
                End If

            Case 210
                Tray_Pallet(0).isHaveTray = True
                Line_Sta(0).isHaveTray = True
                SetEMO(1, 13, False) '给上一台设备可以接受载具信号False
                ListBoxAddMessage(">> 0 段流水线已经接收到载具")
                Step_Line(0) = 300

            Case 300
                If par.chkFn(4) Or par.chkFn(8) = False Then
                    '演示装配或关闭扫码功能 跳过扫条码过程
                    Tray_Pallet(0).Tray_Barcode = Format(Now, "yyyyMMddHHmmss")
                    Step_Line(0) = 500
                Else
                    Call TriggerBarcodeScanner(0)
                    Step_Line(0) = 400
                End If

            Case 400
                '处理扫得到的条码
                Tray_Pallet(0).Tray_Barcode = ""
                Call setMotorStop(0) '扫到条码，停止流水线工作
                Step_Line(0) = 500


            Case 500
                If par.chkFn(8) And par.chkFn(10) And par.chkFn(4) = False Then
                    '条码功能，从服务器下载数据功能开启，且不是演示装配
                    Step_Line(0) = 600
                Else
                    For i = 0 To Tray_Pallet(0).Hole.Count - 1
                        Tray_Pallet(0).isTrayOK = True
                        Tray_Pallet(0).Hole(i).isHaveProduct = True
                        Tray_Pallet(0).Hole(i).isProductOk = True
                    Next
                    Step_Line(0) = 800
                End If

            Case 600
                '与服务器通信获取需要的数据
                Step_Line(0) = 800


            Case 800
                '0 段流水线工作完成，等待第1段流水线接收载具
                Line_Sta(0).isWorking = False
                If Flag_MachineStop = False And Line_Sta(1).isWorking = False And Line_Sta(1).isHaveTray = False Then
                    Call setBlock(0, False)
                    Call setMotorRun(0)
                    ListBoxAddMessage(">> 0 段流水线开始发送载具")
                    Step_Line(0) = 900
                End If

            Case 900
                setMotorRun(0)
                If isHaveTray(1) Or Line_Sta(1).isHaveTray Then  '1段流水线已经接收到载具
                    Step_Line(0) = 1000
                End If

            Case 1000
                If isHaveTray(1) Or Line_Sta(1).isHaveTray Then  '1段流水线已经接收到载具
                    Call setBlock(0, True)
                    Call setMotorStop(0)
                    Step_Line(0) = 1100
                End If

            Case 1100
                If isBlocked(0) Then
                    Line_Sta(0).isHaveTray = False
                    Line_Sta(0).isNormal = True
                    Step_Line(0) = 10   '开始下一循环
                End If

            Case 9000
                Call Frm_Main.Machine_Stop()

        End Select
    End Sub


End Module
