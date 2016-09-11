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
                    Step_Recheck = 210
                Else
                    '否则跳过这一颗料
                    ListBoxAddMessage("复检站跳过第" & index_InRecheck + 1 & "颗料！")
                    Step_Recheck = 7000
                End If

            Case 210

            Case 7000

            Case 8000
                '工位正常工作完成
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
