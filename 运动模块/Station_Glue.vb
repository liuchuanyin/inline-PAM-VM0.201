Public Module Station_Glue

    Public Step_Glue As Integer
    '产品索引号
    Public index_InGlue As Short
    ''' <summary>
    ''' 镭射点索引号(1个产品有N个镭射点)
    ''' </summary>
    ''' <remarks></remarks>
    Public index_Laser As Short
    Public Step_Gopos(7) As Integer
    Public Probe_Result As Boolean
    Public ClrGlue_Work As sFlag3
    Public step_clrGlue As Integer
    Public flgGlue As sFlag3

    ''' <summary>
    ''' 记录Laser测量的高度原始值
    ''' </summary>
    ''' <remarks></remarks>
    Public LaserGlueH() As Double

    ''' <summary>
    ''' 手动运行到某个调试点位
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
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

    ''' <summary>
    ''' 点胶工位自动擦胶
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GlueAuto_Clr(ByRef status As sFlag3)
        Static timeStart As Long

        Select Case step_clrGlue
            Case 10
                status.State = True
                step_clrGlue = 20

            Case 20
                If isHaveTray(1) Then
                    Frm_DialogAddMessage("1段流水线上有载具，已退出自动擦胶")
                    step_clrGlue = 9000
                Else
                    step_clrGlue = 30
                End If

            Case 30
                If Flag_MachineInit = False Then
                    Frm_DialogAddMessage("设备未初始化，已退出自动擦胶")
                    step_clrGlue = 9000
                Else
                    step_clrGlue = 40
                End If

            Case 40
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                ListBoxAddMessage("GlueZ 轴运动到待机位置")
                step_clrGlue = 50

            Case 50
                If isAxisMoving(0, GlueZ) = False Then
                    step_clrGlue = 60
                End If

            Case 60
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(5).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(5).Y)
                ListBoxAddMessage("GlueX、GlueY轴运动到擦胶位置")
                step_clrGlue = 70

            Case 70
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    ListBoxAddMessage("GlueX,GlueY 轴已运动到擦胶位置")
                    step_clrGlue = 80
                End If

            Case 80
                SetEMO(0, 13, True) '擦胶电机使能
                SetEMO(0, 14, True) '擦胶电机工作转动
                SetEMO(0, 15, False)    '擦胶加紧气缸松开
                timeStart = GetTickCount
                step_clrGlue = 100

            Case 100
                If isTimeout(timeStart, 2000) Then
                    SetEMO(0, 14, False) '擦胶电机工作转动
                    step_clrGlue = 110
                End If

            Case 110
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(5).Z)
                ListBoxAddMessage("GlueZ 轴运动到擦胶位置")
                step_clrGlue = 120

            Case 120
                If isAxisMoving(0, GlueZ) = False Then
                    step_clrGlue = 130
                End If

            Case 130
                If EMI(0, 15) Then
                    SetEXO(2, 9, True)  '点胶1气缸降下
                    timeStart = GetTickCount
                    step_clrGlue = 150
                Else
                    Frm_DialogAddMessage("擦胶加紧气缸张开磁簧信号异常")
                    step_clrGlue = 9000
                End If

            Case 150
                If EXI(2, 10) Then
                    step_clrGlue = 160
                ElseIf isTimeout(timeStart, 2000) Then
                    Frm_DialogAddMessage("点胶1气缸下降到位信号异常")
                    step_clrGlue = 9000
                End If

            Case 160
                SetEMO(0, 15, True) '擦胶加紧气缸加紧
                timeStart = GetTickCount
                step_clrGlue = 170

            Case 170
                If isTimeout(timeStart, 1000) Then
                    SetEXO(2, 9, False) '点胶1气缸降上升
                    timeStart = GetTickCount
                    step_clrGlue = 180
                End If

            Case 180
                If EXI(2, 9) Then
                    step_clrGlue = 200
                ElseIf isTimeout(timeStart, 2000) Then
                    Frm_DialogAddMessage("点胶1气缸上升到位信号异常")
                    step_clrGlue = 9000
                End If

            Case 200
                If MACTYPE = "PAM-B" Then
                    step_clrGlue = 210
                Else
                    step_clrGlue = 8000
                End If

            Case 210
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(10).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(10).Y)
                ListBoxAddMessage("GlueX、GlueY轴运动到擦胶位置")
                step_clrGlue = 220

            Case 220
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    ListBoxAddMessage("GlueX,GlueY 轴已运动到擦胶位置")
                    step_clrGlue = 230
                End If

            Case 230
                SetEMO(0, 14, True) '擦胶电机工作转动
                SetEMO(0, 15, False)    '擦胶加紧气缸松开
                timeStart = GetTickCount
                step_clrGlue = 240

            Case 240
                If isTimeout(timeStart, 2000) Then
                    SetEMO(0, 14, False) '擦胶电机工作转动
                    step_clrGlue = 250
                End If

            Case 250
                If EMI(0, 15) Then
                    SetEXO(2, 11, True)  '点胶2气缸降下
                    timeStart = GetTickCount
                    step_clrGlue = 260
                Else
                    Frm_DialogAddMessage("擦胶加紧气缸张开磁簧信号异常")
                    step_clrGlue = 9000
                End If

            Case 260
                If EXI(2, 12) Then
                    step_clrGlue = 270
                ElseIf isTimeout(timeStart, 2000) Then
                    Frm_DialogAddMessage("点胶2气缸下降到位信号异常")
                    step_clrGlue = 9000
                End If

            Case 270
                SetEMO(0, 15, True) '擦胶加紧气缸加紧
                timeStart = GetTickCount
                step_clrGlue = 280

            Case 280
                If isTimeout(timeStart, 1000) Then
                    SetEXO(2, 11, False) '点胶2气缸降上升
                    timeStart = GetTickCount
                    step_clrGlue = 290
                End If

            Case 290
                If EXI(2, 11) Then
                    step_clrGlue = 300
                ElseIf isTimeout(timeStart, 2000) Then
                    Frm_DialogAddMessage("点胶2气缸上升到位信号异常")
                    step_clrGlue = 9000
                End If

            Case 300
                step_clrGlue = 8000

            Case 8000
                '擦胶成功
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                ListBoxAddMessage("GlueZ 轴运动到待机位置")
                step_clrGlue = 8010

            Case 8010
                If isAxisMoving(0, GlueZ) = False Then
                    step_clrGlue = 8020
                End If

            Case 8030
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(0).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(0).Y)
                ListBoxAddMessage("GlueX、GlueY轴运动到待机位置")
                step_clrGlue = 8040

            Case 8040
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    ListBoxAddMessage("GlueX,GlueY 轴已运动到擦胶位置")
                    step_clrGlue = 8050
                End If

            Case 8050
                SetEMO(0, 13, False) '擦胶电机失能
                SetEMO(0, 15, False)    '擦胶加紧气缸松开
                status.State = False
                status.Result = True
                step_clrGlue = 0

            Case 9000
                '擦胶失败
                SetEMO(0, 13, False) '擦胶电机失能
                SetEMO(0, 15, False)    '擦胶加紧气缸松开
                SetEXO(2, 9, False) '点胶1气缸降上升
                SetEXO(2, 11, False) '点胶2气缸降上升
                status.State = False
                status.Result = False
                step_clrGlue = 0
        End Select

    End Sub

    ''' <summary>
    ''' 点胶单站自动运行
    ''' </summary>
    ''' <remarks></remarks>
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
            If index = Val(Frm_Engineering.txt_MaterialSelected.Text) Then
                Tray_Pallet(1).Hole(index).isHaveProduct = True : Tray_Pallet(1).Hole(index).isProductOk = True
                Tray_Pallet(1).Hole(index).ProductBarcode = "Hole" & index
            Else
                Tray_Pallet(1).Hole(index).isHaveProduct = False : Tray_Pallet(1).Hole(index).isProductOk = False
                Tray_Pallet(1).Hole(index).ProductBarcode = "Hole" & index
            End If
        Next

        Step_Glue = 10

        Do While True
            My.Application.DoEvents()
            Delay(10)

            Call Autorun_GlueStation()

            If GLue_Sta.isWorking = False And GLue_Sta.workState = 1 Then
                List_DebugAddMessage("1段流水线单站自动点胶运行完成！")
                Exit Do
            End If

            If IsSysEmcStop Or GLue_Sta.isNormal = False Then
                List_DebugAddMessage("1段流水线单站自动点胶运行异常，停止自动运行！")
                Exit Do
            End If
        Loop

    End Sub

    ''' <summary>
    ''' 点胶站拍照定位，镭射，点胶
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Autorun_GlueStation()
        ' GLue_Sta.workState =0 工作进行中
        ' GLue_Sta.workState =1 工作完成
        ' GLue_Sta.workState =2 工作进行中:点胶进行中
        Static timeStart As Long    '记录开始时间
        Dim Result, TempPosZ As Short

        '点胶站暂停功能控制，但是点胶阀开启时不允许暂停，关闭点胶阀后再自动暂停
        If Flag_MachinePause = True Then
            If Not (EXO(1, 13) Or EXO(1, 14)) Then
                Exit Sub
            End If
        End If
         
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
                    index_InGlue = 0
                    index_Laser = 0
                    Step_Glue = 200
                End If

            Case 200
                If Tray_Pallet(1).Hole(index_InGlue).isHaveProduct And Tray_Pallet(1).Hole(index_InGlue).isProductOk And Frm_Engineering.chk_Brc(index_InGlue).Checked Then
                    '当前穴位有料，且是OK的，并且选中要做这个

                    ListBoxAddMessage("点胶站第" & index_InGlue + 1 & "颗料开始生产！")
                    Step_Glue = 220
                Else
                    '否则跳过这一颗料
                    ListBoxAddMessage("点胶站跳过第" & index_InGlue + 1 & "颗料！")
                    Step_Glue = 7000
                End If

            Case 220   'Z轴去安全位置
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                Step_Glue = 240

            Case 240
                If Not isAxisMoving(0, GlueZ) Then
                    Step_Glue = 260
                End If

            Case 300   'XY轴去第n个点胶拍照位置
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), TrayMatrix.TrayGlue(index_InGlue).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), TrayMatrix.TrayGlue(index_InGlue).Y)
                Step_Glue = 320

            Case 320   'Z轴去点胶站CCD拍第1颗料位置的高度
                If (Not isAxisMoving(0, GlueX)) And (Not isAxisMoving(0, GlueY)) Then
                    Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(14).Z)
                    Step_Glue = 340
                End If

            Case 340   '判断Z轴到位
                If Not isAxisMoving(0, GlueZ) Then
                    timeStart = GetTickCount
                    Step_Glue = 360
                End If

            Case 360 
                'Z轴停止后延时0.5S开始拍照
                If GetTickCount - timeStart > 500 Then
                    Step_Glue = 380
                End If 

            Case 380 '判断CCD是否在锁定中
                If TriggerCCD("T1,1", index_InGlue, Tray_Pallet(1).Tray_Barcode, Tray_Pallet(1).Hole(index_InGlue).ProductBarcode) Then
                    timeStart = GetTickCount
                    Step_Glue = 400
                ElseIf par.chkFn(4) Then   '装配演示中
                    timeStart = GetTickCount
                    Step_Glue = 400
                End If

            Case 400  '判断CCD1返回值OK
                If Winsock1_Data(0) = "T1" And Winsock1_Data(1) = 1 Then
                    If Cam_Status(1) = 1 Then
                        '重新定义此数组大小
                        ReDim LaserGlueH(Cam1LaserPoint.Length - 1)
                        Step_Glue = 410
                    Else
                        Frm_DialogAddMessage("点胶站CCD1拍第" & index_InGlue + 1 & "颗料物料异常，请检查有无产品")
                    End If
                ElseIf GetTickCount - timeStart > 5000 Then
                    Frm_DialogAddMessage("点胶站CCD1拍第" & index_InGlue + 1 & "颗料超时")
                End If

            Case 410 'Z轴回待机位置
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                Step_Glue = 420

            Case 420
                If Not isAxisMoving(0, GlueZ) Then
                    Step_Glue = 440
                End If

                ''step440--580进行连续镭射量测 

            Case 440 'XY轴去第n个镭射点位置 
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Cam1LaserPoint(index_Laser).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Cam1LaserPoint(index_Laser).Y)
                Step_Glue = 460

            Case 460 '去镭射点Z轴高度
                If (Not isAxisMoving(0, GlueX)) And (Not isAxisMoving(0, GlueY)) Then
                    Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(1).Z)
                    Step_Glue = 480
                End If

            Case 480 '判断Z轴停止
                If (Not isAxisMoving(0, GlueZ)) Then
                    timeStart = GetTickCount
                    Step_Glue = 500
                End If

            Case 500
                If par.chkFn(4) Then  '装配演示
                    '演示装配
                ElseIf GetTickCount - timeStart > 500 Then
                    Step_Glue = 520
                End If

            Case 520
                If Com2_Send("M1" & vbCr) = False Then    'COM2发送镭射触发测距命令
                    'COM2口发送数据异常
                    ListBoxAddMessage("COM2端口未打开，请检查!")
                Else
                    Step_Glue = 540
                End If

            Case 540
                Result = Com2_Return() '等待COM2镭射触发结束
                If Result = 0 Then
                    Step_Glue = 560
                ElseIf Result = 1 Then
                    '镭射异常
                    If Flag_FrmEngineeringOpned Then
                        ListBoxAddMessage("点胶站镭射COM2端口未响应，请检查")
                    End If
                    Call Write_Log(2, "点胶站镭射COM2端口未响应，请检查", Path_Log)
                End If

            Case 560
                TempPosZ = COM2_Data(0)
                If Math.Abs(TempPosZ) < Math.Abs(par.num(16)) And COM2_String <> "M1,-FFFFFFF" And TempPosZ <> 0 Then
                    LaserGlueH(index_Laser) = -TempPosZ
                    ListBoxAddMessage("点胶站第" & index_InGlue + 1 & "个产品的第" & index_Laser + 1 & "个镭射点值:" & LaserGlueH(index_Laser))
                    Step_Glue = 580
                Else
                    '镭射超出范围
                    Frm_DialogAddMessage("点胶站第" & index_InGlue + 1 & "个产品的第" & index_Laser + 1 & "个镭射点超出范围")
                End If

            Case 580  '判断镭射点是否全部工作完成
                index_Laser = index_Laser + 1
                If index_Laser < Cam1LaserPoint.Length Then
                    Step_Glue = 440
                Else
                    Step_Glue = 600
                End If

            Case 600  'Z轴抬升到待机位置
                AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                Step_Glue = 620

            Case 620  '判断Z轴停止
                If Not isAxisMoving(0, GlueZ) Then
                    Step_Glue = 640
                End If

            Case 640  '将之前得到的点位数据转化成点胶最终用的XYZ的值
                Call SouceToTarget(0)  '计算1号胶针

                PAMGlueStep = 10

                Step_Glue = 660

            Case 660  '开始点胶
                GLue_Sta.workState = 2 '工作进行中:点胶进行中
                flgGlue.State = True

                If MACTYPE = "PAM-1" Then
                    Call PAM1Glue(flgGlue)
                ElseIf MACTYPE = "PAM-2" Then

                ElseIf MACTYPE = "PAM-3" Then

                ElseIf MACTYPE = "PAM-B" Then

                End If

                If flgGlue.State = False Then
                    GLue_Sta.workState = 0 '状态由点胶中改成工作进行中
                    If flgGlue.Result = True Then
                        Step_Glue = 680
                    Else
                        Step_Glue = 9000
                    End If
                End If

            Case 680
                '单个穴位点胶完成
                Step_Glue = 7000

            Case 7000
                '共计12颗料，index从0开始
                If index_InGlue < 11 Then
                    index_InGlue += 1
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
                Step_Glue = 0

        End Select

    End Sub

    ''' <summary>
    ''' 将CCD1给的点胶点位加上较针偏移值
    ''' </summary>
    ''' <param name="index">0代表一号胶针，1代表二号胶针</param>
    ''' <remarks></remarks>
    Private Sub SouceToTarget(index As Integer)

        Select Case index
            Case 0
                '计算点胶点XY的最终值，保存到Cam1GluePoint()中
                For i = 0 To Cam1GluePoint.Length - 1
                    Cam1GluePoint(i).X = Cam1GluePoint(i).X + Par_Pos.Probe1_Diff(0).X
                    Cam1GluePoint(i).Y = Cam1GluePoint(i).X + Par_Pos.Probe1_Diff(0).Y
                Next
                '计算点胶点Z的最终值，保存到LaserGlueH()中
                For i = 0 To LaserGlueH.Length - 1
                    '               镭射打点Z轴高度      '镭射零点与针头间距离 '镭射量出来的高度  '针头较什Z向差值           '镭射偏移值
                    LaserGlueH(i) = Par_Pos.St_Glue(1).Z + Par_Pos.Dis_LN(0) + LaserGlueH(i) + Par_Pos.Probe1_Diff(0).Z + Par_Pos.Laser_offset
                Next
            Case 1
                For i = 0 To Cam1GluePoint.Length - 1
                    Cam1GluePoint(i).X = Cam1GluePoint(i).X + Par_Pos.Probe1_Diff(1).X
                    Cam1GluePoint(i).Y = Cam1GluePoint(i).X + Par_Pos.Probe1_Diff(1).Y
                Next

                For i = 0 To LaserGlueH.Length - 1
                    '               镭射打点Z轴高度      '镭射零点与针头间距离 '镭射量出来的高度  '针头较什Z向差值           '镭射偏移值
                    LaserGlueH(i) = Par_Pos.St_Glue(1).Z + Par_Pos.Dis_LN(1) + LaserGlueH(i) + Par_Pos.Probe1_Diff(1).Z + Par_Pos.Laser_offset
                Next
        End Select
    End Sub

    ''' <summary>
    ''' PAM1点胶步序号
    ''' </summary>
    ''' <remarks></remarks>
    Public PAMGlueStep As Integer

    ''' <summary>
    ''' 圆弧插补运动半径3.69mm
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RADIUS As Double = 3690

    ''' <summary>
    ''' PAM1点胶路径索引号
    ''' </summary>
    ''' <remarks></remarks>
    Public index_PAM1Glue As Integer

    ''' <summary>
    ''' PAM1点胶路径
    ''' </summary>
    ''' <param name="Status"></param>
    ''' <remarks></remarks>
    Private Sub PAM1Glue(ByRef Status As sFlag3)
        '''''''''''''''''需要CCD给出4个镭射点LaserGlueH（3）'''''''''''''''''''
        '''''''''''''''''需要CCD给出16个点胶点Cam1GluePoint(15)''''''''''''''''
        '''''''''''''''''一共有8段圆弧，需要Par_Glue.Segment(7)''''''''''''''''

        Static PAM1TimWatch As Long
        Dim Result As Short
        Dim Z_Glue As Double
        Dim sPoint() As Dist_XY

        On Error Resume Next
        Select Case PAMGlueStep
            Case 10
                If GLue_Sta.workState = 2 Then
                    index_PAM1Glue = 0
                    Status.State = True
                    PAMGlueStep = 100
                End If

                '***************************圆弧开始***************************
            Case 100
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)  'Z轴回待机位置
                PAMGlueStep = 120

            Case 120
                If Not isAxisMoving(0, GlueZ) Then
                    PAMGlueStep = 140
                End If

            Case 140
                'X,Y去点胶位置1
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Cam1GluePoint(0).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Cam1GluePoint(0).Y)
                PAMGlueStep = 160

            Case 160
                If (Not isAxisMoving(0, GlueX)) And (Not isAxisMoving(0, GlueY)) = False Then
                    SetEXO(2, 9, True)  '点胶升降气缸1打下
                    PAM1TimWatch = GetTickCount
                    PAMGlueStep = 180
                End If

            Case 180
                If GetTickCount - PAM1TimWatch > 500 Then '气缸到位延时
                    If EXI(2, 10) = True And EXI(2, 9) = False Then
                        PAMGlueStep = 190
                    ElseIf GetTickCount - PAM1TimWatch > 2000 Then
                        Frm_DialogAddMessage("点胶气缸下降异常,请检查")
                        PAMGlueStep = 9000
                    End If
                End If

            Case 190 '走到点胶点上方5mm
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), LaserGlueH(index_PAM1Glue / 4) - 5) 'LaserGlueH(index_PAM1Glue / 4)因为4个点胶点共用一个Laser点
                PAMGlueStep = 200

            Case 200
                If Not isAxisMoving(0, GlueZ) Then
                    PAMGlueStep = 205
                End If

            Case 205
                'X,Y去点胶位置1
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Cam1GluePoint(index_PAM1Glue).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Cam1GluePoint(index_PAM1Glue).Y)
                PAMGlueStep = 210

            Case 215
                If (Not isAxisMoving(0, GlueX)) And (Not isAxisMoving(0, GlueY)) = False Then
                    PAM1TimWatch = GetTickCount
                    PAMGlueStep = 220
                End If

            Case 220  'Z轴下降第二段低速
                If par.chkFn(4) Then '判断是否开启演示装配
                    Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ) / 5, LaserGlueH(index_PAM1Glue / 4) - 2) 'Z轴移到点胶位置1
                Else
                    Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ) / 5, LaserGlueH(index_PAM1Glue / 4))
                End If
                PAMGlueStep = 240

            Case 240
                If Not isAxisMoving(0, GlueZ) Then
                    PAM1TimWatch = GetTickCount
                    PAMGlueStep = 260
                End If

            Case 260
                If EXI(2, 10) = True And EXI(2, 9) = False Then
                    If par.chkFn(4) = False Then '判断是否开启演示装配
                        SetEXO(1, 13, True)      '打开点胶阀1
                    End If
                    PAM1TimWatch = GetTickCount
                    PAMGlueStep = 280
                End If

            Case 280
                '圆弧点胶第一段开始时延时
                If GetTickCount - PAM1TimWatch >= (Par_Glue.Segment(index_PAM1Glue / 2).startDelay * 1000) Then 'Par_Glue.Segment(index_PAM1Glue / 2)是因为2个点胶点共用这1个Segment参数
                    ReDim sPoint(2)
                    PAMGlueStep = 300
                End If

            Case 300
                Call ArcXY_Motion(0, Cam1GluePoint(index_PAM1Glue + 1), RADIUS, Par_Glue.Segment(index_PAM1Glue / 2).vel, 0)
                PAM1TimWatch = GetTickCount
                PAMGlueStep = 320

            Case 320
                '因为ArcXY_Motion设置的坐标系号为1
                If ZSPD_Line(0, 1) Then
                    PAM1TimWatch = GetTickCount
                    PAMGlueStep = 340
                ElseIf GetTickCount - PAM1TimWatch > 10000 Then
                    PAMGlueStep = 9000
                End If

            Case 340
                If GetTickCount - PAM1TimWatch > Par_Glue.Segment(index_PAM1Glue / 2).endDelay * 1000 Then
                    SetEXO(1, 13, False)      '关闭点胶阀1
                    PAMGlueStep = 360
                End If

            Case 360
                index_PAM1Glue = index_PAM1Glue + 2
                If index_PAM1Glue >= 16 Then
                    index_PAM1Glue = 0
                    PAMGlueStep = 380
                Else
                    PAMGlueStep = 190
                End If

            Case 380
                SetEXO(2, 9, False)  '点胶升降气缸1缩回
                PAM1TimWatch = GetTickCount
                PAMGlueStep = 400

            Case 400
                '气缸上升到位
                If EXI(2, 9) = True And EXI(2, 10) = False Then
                    PAMGlueStep = 420
                Else
                    Frm_DialogAddMessage("点胶气缸上升异常,请检查")
                End If

            Case 420
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)  'Z轴回待机位置 
                PAMGlueStep = 440

            Case 440
                If Not isAxisMoving(0, GlueZ) Then
                    PAMGlueStep = 460
                End If

            Case 460
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(0).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(0).Y)
                PAMGlueStep = 480

            Case 480
                If (Not isAxisMoving(0, GlueX)) And (Not isAxisMoving(0, GlueY)) = False Then
                    PAMGlueStep = 8000
                End If

                '点胶成功
            Case 8000
                Status.Result = True
                Status.State = False
                PAMGlueStep = 0
                Exit Sub

                '点胶失败
            Case 9000
                SetEXO(1, 13, False)      '关闭点胶阀1
                SetEXO(2, 9, False)      '点胶升降气缸1缩回
                PAM1TimWatch = GetTickCount
                PAMGlueStep = 9010

            Case 9010
                '气缸上升到位
                '气缸上升到位
                If EXI(2, 9) = True And EXI(2, 10) = False Then
                    PAMGlueStep = 9020
                Else
                    Frm_DialogAddMessage("点胶气缸上升异常,请检查")
                End If

            Case 9020
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)  'Z轴回待机位置 
                PAMGlueStep = 9030

            Case 9030
                If Not isAxisMoving(0, GlueZ) Then
                    PAMGlueStep = 9040
                End If

            Case 9040
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(0).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(0).Y)
                PAMGlueStep = 9050

            Case 9050
                If (Not isAxisMoving(0, GlueX)) And (Not isAxisMoving(0, GlueY)) = False Then
                    PAMGlueStep = 9400
                End If

            Case 9400
                SetEXO(1, 13, False)      '关闭点胶阀1
                Status.Result = False
                Status.State = False
                PAMGlueStep = 0
                Exit Sub
        End Select
    End Sub

End Module
