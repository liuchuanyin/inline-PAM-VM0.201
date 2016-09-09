Public Module Station_Glue

    Public Step_Glue As Integer
    Public Step_Gopos(7) As Integer
    Public Probe_Result As Boolean

    Public Sub GoPos_Glue(ByVal index As Short)
        'Static timeStart As Long
        '判断是否所有轴伺服ON
        If ServoOn(0, GlueX) And ServoOn(0, GlueY) And ServoOn(0, GlueZ) Then
        Else
            List_DebugAddMessage("请先打开点胶工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中()
        If isAxisMoving(0, GlueX) Or isAxisMoving(0, GlueY) Or isAxisMoving(0, GlueZ) Then
            List_DebugAddMessage("点胶工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        '气缸上升到位
        If EXI(2, 9) = False And IIf(MACTYPE = "PAM-B", EXI(2, 11) = False, True) Then
            List_DebugAddMessage("请检查点胶工位点胶气缸是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(1) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(1)
                Case 0
                    Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                    Step_Gopos(1) = 10

                Case 10
                    If isAxisMoving(0, GlueZ) = False Then
                        List_DebugAddMessage("GlueZ 轴已运动到待机位置")
                        Step_Gopos(1) = 20
                    End If

                Case 20
                    Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(index).X)
                    Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(index).Y)
                    Step_Gopos(1) = 30

                Case 30
                    If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                        List_DebugAddMessage("GlueX,GlueY 轴已运动到指定位置")
                        Step_Gopos(1) = 40
                    End If

                Case 40
                    Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(index).Z)
                    Step_Gopos(1) = 50

                Case 50
                    If isAxisMoving(0, GlueZ) = False Then
                        List_DebugAddMessage("GlueZ 轴已运动到指定位置")
                        Step_Gopos(1) = 80
                    End If

                Case 80
                    List_DebugAddMessage("点胶工位运动到指定位置完成")
                    Step_Gopos(1) = 0
                    Exit Do
            End Select
        Loop

    End Sub

    ''' <summary>
    ''' 点胶工站自动校针
    ''' </summary>
    ''' <param name="index">胶针编号：0：胶针1；1：胶针2</param>
    ''' <remarks></remarks>
    Public Sub Auto_NeedleCalibration(ByVal index As Short)
        If MACTYPE <> "PAM-B" And index = 1 Then Exit Sub
        If Flag_MachineInit = False Then
            Frm_DialogAddMessage("设备未初始化,请在初始化完成后再自动校针！")
            Exit Sub
        End If
        If isHaveTray(1) Then
            Frm_DialogAddMessage("请检查第一段流水线上是否有载具！")
            Exit Sub
        End If
        If ServoOn(0, GlueX) And ServoOn(0, GlueY) And ServoOn(0, GlueZ) Then
        Else
            List_DebugAddMessage("请先打开点胶工位所有轴伺服ON")
            Exit Sub
        End If

        If isAxisMoving(0, GlueX) Or isAxisMoving(0, GlueY) Or isAxisMoving(0, GlueZ) Then    '判断工位是否有某个轴在运动中
            List_DebugAddMessage("点胶工位有轴正在运动中，请稍后再自动校针")
            Exit Sub
        End If

        Do While True
            Call Needle_AutoCalibration(Probe_Result, index) '自动校针
            Delay(10)
            If Probe_Result = False Then         '等待校针结束
                Step_NeedleCalibration = 0
                List_DebugAddMessage("自动校针结束")
                Exit Do
            End If

            If IsSysEmcStop Then    '判断急停按钮是否按下
                Step_NeedleCalibration = 0
                Frm_DialogAddMessage("自动校针急停中断")
                Exit Do
            End If
            Application.DoEvents()
        Loop
    End Sub

    Public Sub ManualRun_Glue()
        'Static timeStart As Long
        '判断是否所有轴伺服ON
        If ServoOn(0, GlueX) And ServoOn(0, GlueY) And ServoOn(0, GlueZ) Then
        Else
            List_DebugAddMessage("请先打开点胶工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中()
        If isAxisMoving(0, GlueX) Or isAxisMoving(0, GlueY) Or isAxisMoving(0, GlueZ) Then
            List_DebugAddMessage("点胶工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        If EXI(2, 9) = False And IIf(MACTYPE = "PAM-B", EXI(2, 11) = False, True) Then
            List_DebugAddMessage("请检查点胶工位点胶气缸是否在安全位置！")
            Exit Sub
        End If

        If isHaveTray(1) = False And par.chkFn(4) = False Then
            List_DebugAddMessage("流水线上无载具，且不是演示装配，已退出单站自动运行！")
            Exit Sub
        End If

        If (EMI(2, 8) = False Or isCylinderRised(1) = False) And par.chkFn(4) = False Then
            List_DebugAddMessage("流水线上吸载具或顶升气缸异常，且不是演示装配，已退出单站自动运行！")
            Exit Sub
        End If

        Line_Sta(1).workState = 2   '接收载具完成，等待点胶
        Line_Sta(1).isHaveTray = True
        Tray_Pallet(1).isHaveTray = True : Tray_Pallet(1).isTrayOK = True : Tray_Pallet(1).Tray_Barcode = Format(Now, "yyyyMMddHHmmss")
        For index = 0 To Tray_Pallet(1).Hole.Count - 1
            Tray_Pallet(1).Hole(index).isHaveProduct = True : Tray_Pallet(1).Hole(index).isProductOk = True
            Tray_Pallet(1).Hole(index).ProductBarcode = "Hole" & index
        Next
        Step_Glue = 10

        Do While True
            My.Application.DoEvents()
            Delay(10)
            Call Autorun_GlueStation()
            If GLue_Sta.isWorking = False And GLue_Sta.workState = 1 Then
                List_DebugAddMessage("1段流水线单站自动运行完成！")
                Exit Do
            End If
            If IsSysEmcStop Or GLue_Sta.isNormal = False Then
                List_DebugAddMessage("1段流水线单站自动运行异常，停止自动运行！")
                Exit Do
            End If
        Loop

    End Sub



    Public Sub Autorun_GlueStation()
        '产品索引号
        Static index As Short
        ' GLue_Sta.workState =0 工作进行中
        ' GLue_Sta.workState =1 工作完成
        ' GLue_Sta.workState =2 工作进行中:点胶进行中

        Select Case Step_Glue
            Case 10
                If Flag_MachineStop = False And Line_Sta(1).workState = 2 And _
                    Line_Sta(1).isHaveTray = True And isTimeout(cmd_SendTime, 2) Then
                    GLue_Sta.isNormal = True : GLue_Sta.isWorking = True : GLue_Sta.workState = 0    '点胶模组工作进行中
                    Step_Glue = 100
                End If

            Case 100
                '流水线上有载具，且载具可用
                If Tray_Pallet(1).isHaveTray And Tray_Pallet(1).isTrayOK Then
                    index = 0
                    Step_Glue = 200
                End If

            Case 200
                If Tray_Pallet(1).Hole(index).isHaveProduct And Tray_Pallet(1).Hole(index).isProductOk Then

                End If



            Case 2000
                '共计12颗料，index从0开始
                If index < 11 Then
                    index += 1
                    Step_Glue = 200 '去下一颗料点胶
                Else
                    Step_Glue = 8000 '点胶完成
                End If

            Case 8000
                '点胶工站工作完成
                GLue_Sta.isWorking = False    '点胶模组工作完成
                GLue_Sta.isNormal = True
                GLue_Sta.workState = 1  '工作完成
                Step_Glue = 10  '开始下一个循环

            Case 9000
                '工作异常需要急停处理
                GLue_Sta.isNormal = False   '点胶工站工作异常
                Call Frm_Main.Machine_Stop()

        End Select

    End Sub


End Module
