Imports System.Math
Module Needle_Calibration
    '圆弧插补，圆心描述方式，结构体参数
    Public Structure sArcXYC
        Dim CardNum As Integer
        Dim crd As Integer
        Dim X As Long
        Dim Y As Long
        Dim xCenter As Long
        Dim yCenter As Long
        Dim circleDir As Integer
        Dim synVel As Double
        Dim synAcc As Double
        Dim velEnd As Double
        Dim fifo As Integer
    End Structure

    '2维直线插补，结构体参数
    Public Structure sLnXY
        Dim CardNum As Integer
        Dim crd As Integer
        Dim X As Long
        Dim Y As Long
        Dim synVel As Double
        Dim synAcc As Double
        Dim velEnd As Double
        Dim fifo As Integer
    End Structure

    Private ValueX(3) As Long  '工位探针捕获的X轴位置值(X方向4个位置)
    Private ValueY(3) As Long  '工位探针捕获的Y轴位置值(Y方向4个位置)
    Private ValueZ(1) As Long  '工位探针捕获的Z轴位置值(Z方向2个位置)

    Private GetProbe_Work As sFlag3          '工位获取针点位置状态
    Public GetProbe_Step As Long    '工位获取针点位置步序号
    Public Const S2R = 2.5    '工位校针画圆半径
    Public Step_NeedleCalibration As Integer     '工位校针运动步序号
    Public Status_Calibration As Boolean   '工位自动校针运行结果
    Public Probe_Pos(1) As Pos_XYZ   '工位自动校针位置前后值
    Public Probe_PosAve As Pos_XYZ  '工位自动校针位置前后两次平均值
    Public AutoNeedle_Over As Boolean  '工位点胶完成标志

    '自动校针
    Public Sub Needle_AutoCalibration(ByRef workState As Boolean, ByVal index As Short)
        Static TempPos(1) As Double
        Static Probe_Count As Integer   '校针次数
        Dim Probe_Status As Integer    '探针捕获状态(捕获到探针时值为1，未捕获到探针时值为0)
        Static Timecount As Long
        Dim rtn As Short

        Select Case Step_NeedleCalibration
            Case 0
                workState = True
                Probe_Count = 0
                Step_NeedleCalibration = 10

            Case 10
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)   'Z轴到待机位置
                Step_NeedleCalibration = 20

            Case 20
                If isAxisMoving(0, GlueZ) = False Then
                    Step_NeedleCalibration = 30
                End If

            Case 30
                If index = 1 Then
                    '胶针2校针位置
                    Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(11).X) 'XY针头校正位置
                    Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(11).Y)
                Else
                    '胶针1校针位置
                    Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(6).X) 'XY针头校正位置
                    Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(6).Y)
                End If
                Step_NeedleCalibration = 40

            Case 40
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    Step_NeedleCalibration = 50
                End If


            Case 50
                If index = 1 Then
                    SetEXO(2, 11, True) '点胶2气缸下降
                Else
                    SetEXO(2, 9, True)  '点胶1气缸下降
                End If
                Timecount = GetTickCount
                Step_NeedleCalibration = 60

            Case 60
                If index = 1 Then
                    If EXI(2, 12) = True And EXI(2, 11) = False Then
                        Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(11).Z)   'Z1轴下降'胶针2校针位置Z
                        Step_NeedleCalibration = 70
                    End If
                Else
                    If EXI(2, 10) = True And EXI(2, 9) = False Then
                        Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(6).Z)   'Z1轴下降'胶针1校针位置Z
                        Step_NeedleCalibration = 70
                    End If
                End If
                If isTimeout(Timecount, 3000) Then
                    Frm_DialogAddMessage("校针时气缸下降到位异常！")
                    Step_NeedleCalibration = 9000
                End If

            Case 70
                If isAxisMoving(0, GlueZ) = False Then
                    Step_NeedleCalibration = 80
                End If


            Case 80
                '2mm/s 移动
                If index = 1 Then
                    Call AbsMotion(0, GlueX, 2, Par_Pos.St_Glue(11).X - ((S2R / 2) / Sin(45)))
                    Call AbsMotion(0, GlueY, 2, Par_Pos.St_Glue(11).Y - ((S2R / 2) / Sin(45)))
                Else
                    Call AbsMotion(0, GlueX, 2, Par_Pos.St_Glue(6).X - ((S2R / 2) / Sin(45)))
                    Call AbsMotion(0, GlueY, 2, Par_Pos.St_Glue(6).Y - ((S2R / 2) / Sin(45)))
                End If
                Step_NeedleCalibration = 90

            Case 90
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    Step_NeedleCalibration = 100
                End If

            Case 100
                GetProbe_Step = 0    '获取XY交点坐标之前，先将XY交点坐标获取步序号清0
                Step_NeedleCalibration = 200

            Case 200
                '2mm/s 移动
                If index = 1 Then
                    Call S2_GetProbePos(0, GlueX, GlueY, Par_Pos.St_Glue(11).X, Par_Pos.St_Glue(11).Y, 2, TempPos, GetProbe_Work)
                Else
                    Call S2_GetProbePos(0, GlueX, GlueY, Par_Pos.St_Glue(6).X, Par_Pos.St_Glue(6).Y, 2, TempPos, GetProbe_Work)
                End If


                If GetProbe_Work.State = False Then   '等待获取XY交点坐标结束
                    If GetProbe_Work.Result Then
                        Probe_Pos(Probe_Count).X = TempPos(0)
                        Probe_Pos(Probe_Count).Y = TempPos(1)
                        If Flag_FrmEngineeringOpned Then
                            List_DebugAddMessage("第" & CStr(Probe_Count + 1) & "次X交点坐标:" & Format(Probe_Pos(Probe_Count).X, "0.0000"))
                            List_DebugAddMessage("第" & CStr(Probe_Count + 1) & "次Y交点坐标:" & Format(Probe_Pos(Probe_Count).Y, "0.0000"))
                        End If
                        Step_NeedleCalibration = 210
                    Else
                        Step_NeedleCalibration = 9000
                    End If
                End If

            Case 210
                Step_NeedleCalibration = 300

            Case 300
                Call AbsMotion(0, GlueX, 2, Probe_Pos(Probe_Count).X)  '到X交点坐标
                Call AbsMotion(0, GlueY, 2, Probe_Pos(Probe_Count).Y)  '到Y交点坐标
                Call AbsMotion(0, GlueZ, 2, CurrEncPos(0, GlueZ) - 5) 'Z轴抬升5mm
                Step_NeedleCalibration = 310

            Case 310
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False And isAxisMoving(0, GlueZ) = False Then
                    Step_NeedleCalibration = 320
                End If

            Case 320
                rtn = GT_SetCaptureMode(0, GlueZ, CAPTURE_PROBE) '设定Z轴探针捕获
                Step_NeedleCalibration = 330

            Case 330
                Call AbsMotion(0, GlueZ, 2, CurrEncPos(0, GlueZ) + 5) 'Z轴校针位置
                Step_NeedleCalibration = 340

            Case 340
                If isAxisMoving(0, GlueZ) = False Then
                    Step_NeedleCalibration = 350
                End If

            Case 350
                rtn = GT_GetCaptureStatus(0, GlueZ, Probe_Status, ValueZ(0)) '获收Z轴探针捕获的状态和位置
                If Probe_Status = 1 Then    '判断探针捕获是否触发
                    Probe_Status = 0        '探针捕获触发清零
                    Probe_Pos(Probe_Count).Z = CDbl(ValueZ(0) * AxisPar.Lead(0, GlueZ) / AxisPar.pulse(0, GlueZ) / AxisPar.Gear(0, GlueZ))  '交点位置Z(mm)
                    List_DebugAddMessage("第" & CStr(Probe_Count + 1) & "次Z交点坐标:" & Format(Probe_Pos(Probe_Count).Z, "0.0000"))
                    Step_NeedleCalibration = 360
                Else
                    Step_NeedleCalibration = 9000
                End If

            Case 360
                If Probe_Count = 0 Then
                    Probe_Count = Probe_Count + 1
                    Step_NeedleCalibration = 80
                Else
                    Step_NeedleCalibration = 400
                End If

                '
            Case 400
                If Abs(Probe_Pos(0).X - Probe_Pos(1).X) > 0.05 Then '判断前后两次校针的X位置差值是否在规定以内
                    Step_NeedleCalibration = 9000
                ElseIf Abs(Probe_Pos(0).Y - Probe_Pos(1).Y) > 0.05 Then '判断前后两次校针的X位置差值是否在规定以内
                    Step_NeedleCalibration = 9000
                ElseIf Abs(Probe_Pos(0).Z - Probe_Pos(1).Z) > 0.05 Then '判断前后两次校针的X位置差值是否在规定以内
                    Step_NeedleCalibration = 9000
                Else
                    Probe_PosAve.X = (Probe_Pos(0).X + Probe_Pos(1).X) / 2
                    Probe_PosAve.Y = (Probe_Pos(0).Y + Probe_Pos(1).Y) / 2
                    Probe_PosAve.Z = (Probe_Pos(0).Z + Probe_Pos(1).Z) / 2
                    If Flag_FrmEngineeringOpned Then
                        List_DebugAddMessage("X交点坐标平均值:" & Format(Probe_PosAve.X, "0.0000"))
                        List_DebugAddMessage("Y交点坐标平均值:" & Format(Probe_PosAve.Y, "0.0000"))
                        List_DebugAddMessage("Z交点坐标平均值:" & Format(Probe_PosAve.Z, "0.0000"))
                    End If
                    Step_NeedleCalibration = 410
                End If

            Case 410
                Step_NeedleCalibration = 500
                '
            Case 500
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)   'Z轴到待机位置
                '点胶气缸上升
                SetEXO(2, 9, False)
                SetEXO(2, 11, False)
                Step_NeedleCalibration = 510

            Case 510
                If isAxisMoving(0, GlueZ) = False Then
                    Step_NeedleCalibration = 520
                End If

            Case 520
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(0).X) 'XY待机位置
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(0).Y)
                Step_NeedleCalibration = 530

            Case 530
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    Step_NeedleCalibration = 540
                End If

                ' 求出XYZ补偿值()
            Case 540
                'X，Y判断基准值是否存在
                If Par_Pos.Probe1_Base(index).X = 0 And Par_Pos.Probe1_Base(index).Y = 0 Then
                    Par_Pos.Probe1_Base(index).X = Probe_PosAve.X '当前校针位置平均值作为基准值
                    Par_Pos.Probe1_Base(index).Y = Probe_PosAve.Y
                    Par_Pos.Probe1_Diff(index).X = 0    '基准差值清0
                    Par_Pos.Probe1_Diff(index).Y = 0
                Else
                    Par_Pos.Probe1_Diff(index).X = Probe_PosAve.X - Par_Pos.Probe1_Base(index).X  '当前校针X平均值减去基准X值
                    Par_Pos.Probe1_Diff(index).Y = Probe_PosAve.Y - Par_Pos.Probe1_Base(index).Y  '当前校针Y平均值减去基准Y值
                End If

                'Z判断基准值是否存在
                If Par_Pos.Probe1_Base(index).Z = 0 Then
                    Par_Pos.Probe1_Base(index).Z = Probe_PosAve.Z
                    Par_Pos.Probe1_Diff(index).Z = 0    '基准差值清0
                Else
                    Par_Pos.Probe1_Diff(index).Z = Probe_PosAve.Z - Par_Pos.Probe1_Base(index).Z  '当前校针Z平均值减去基准Z值
                End If

                If Flag_FrmEngineeringOpned Then
                    ListBoxAddMessage("胶针" & index + 1 & "X补偿值:" & Format(Par_Pos.Probe1_Diff(index).X, "0.0000"))
                    ListBoxAddMessage("胶针" & index + 1 & "Y补偿值:" & Format(Par_Pos.Probe1_Diff(index).Y, "0.0000"))
                    ListBoxAddMessage("胶针" & index + 1 & "Z补偿值:" & Format(Par_Pos.Probe1_Diff(index).Z, "0.0000"))
                End If
                '校针成功后校针完成标志位置为True
                Par_Pos.Needle_NeedCalibration(index) = False
                Call Write_Par_Pos(Path_Par_Pos, Par_Pos)   '参数保存
                Step_NeedleCalibration = 550

            Case 550
                Frm_DialogAddMessage("自动校针成功")
                AutoNeedle_Over = True
                Step_NeedleCalibration = 0
                workState = False
                Exit Sub

            Case 9000
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)   'Z轴到待机位置
                '点胶气缸上升
                SetEXO(2, 9, False)
                SetEXO(2, 11, False)
                Step_NeedleCalibration = 9100

            Case 9100
                If isAxisMoving(0, GlueZ) = False Then
                    Step_NeedleCalibration = 9200
                End If

            Case 9200
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(0).X) 'XY待机位置
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(0).Y)
                Step_NeedleCalibration = 9300

            Case 9300
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False Then
                    Step_NeedleCalibration = 9400
                End If

            Case 9400
                Frm_DialogAddMessage("自动校针失败")
                Step_NeedleCalibration = 0
                workState = False

        End Select
    End Sub

    '------------------------------------------------------------------------
    '参数1：卡号，参数2:X轴号,参数3：Y轴号,参数4：X0圆点坐标值，参数6：Y0圆点坐标值，参数7：X,Y轴的合成目标速度，参数7：X,Y轴的交点坐标值，
    Public Sub S2_GetProbePos(CardNum As Integer, AxisX As Integer, AxisY As Integer, X0 As Double, Y0 As Double, Speed As Double, ByRef Intersection() As Double, ByRef State As sFlag3)
        Dim rtn As Short
        Dim CrdPrm As TCrdPrm   '声明坐标系结构体参数
        Dim cArc As sArcXYC     '声明圆弧插补结构体参数
        Dim Probe_Status As Integer    '探针捕获状态(捕获到探针时值为1，未捕获到探针时值为0)
        Dim Run_Status As Integer       '插补运动状态(0：该坐标系的该FIFO没有在运动；1：该坐标系的该FIFO正在进行插补运动
        Dim Segment As Long             '存储当前已经完成的插补段数。当重新建立坐标系或者调用GT_CrdClear指令后，该值会被清零。
        Dim TempPosX As Double
        Dim TempPosY As Double

        Dim VelXY As Double     '两轴合成速度
        Dim PosXStart As Long   'X轴圆弧起点位置(脉冲数)
        Dim PosYStart As Long   'Y轴圆弧起点位置(脉冲数)
        Dim PosXEnd As Long     'X轴圆弧终点位置(脉冲数)
        Dim PosYEnd As Long     'Y轴圆弧终点位置(脉冲数)
        Dim PosXCenter As Long  'X轴圆弧圆点位置(脉冲数)
        Dim PosYCenter As Long  'Y轴圆弧圆点位置(脉冲数)

        Select Case GetProbe_Step
            Case 0
                With CrdPrm '设置坐标系参数
                    .dimension = 2      '指定坐标系维数为2(X、Y的2维插补)
                    .synVelMax = 1000   '该坐标系最大合成速度(Pulse/ms)
                    .synAccMax = 0.2    '该坐标系最大合成加速度(Pulse/ms^2)
                    .evenTime = 50      '每个插补段的最小匀速段时间(ms)
                    .profile1 = 1       '坐标系X维映射相应的规划轴
                    .profile2 = 2       '坐标系Y维映射相应的规划轴
                    .setOriginFlag = 1  '需要明确指定坐标系原点的位置(为0则用当前规划位置作为坐标系的原点)
                    .originPos1 = 0     '指定X维坐标系原点(为0则和机械原点重合)
                    .originPos2 = 0     '指定Y维坐标系原点(为0则和机械原点重合)
                End With
                rtn = GT_SetCrdPrm(CardNum, 1, CrdPrm)  '建立坐标系1
                If rtn <> 0 Then
                    GetProbe_Step = 1000 '坐标系建立失败则退出
                Else
                    State.State = True
                    GetProbe_Step = GetProbe_Step + 1
                End If
            Case 1
                GetProbe_Step = 10

                '获取第1个校针点XY位置
            Case 10
                rtn = GT_SetCaptureMode(CardNum, AxisX, CAPTURE_PROBE) '设定X轴探针捕获
                rtn = GT_SetCaptureMode(CardNum, AxisY, CAPTURE_PROBE) '设定Y轴探针捕获
                VelXY = CDbl(Speed * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX) / 1000) '计算合成目标速度(Pluse/ms)
                TempPosX = X0 + (S2R / 2) / Sin(45) '计算X终点坐标(mm)
                TempPosY = Y0 - (S2R / 2) / Sin(45) '计算Y终点坐标(mm)
                PosXEnd = CLng(TempPosX * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX))  'X圆弧终点坐标(Pluse)
                PosYEnd = CLng(TempPosY * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY))  'Y圆弧终点坐标(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisX, TempPosX, 1, 0) '读取0号卡X轴当前规划位置(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisY, TempPosY, 1, 0) '读取0号卡Y轴当前规划位置(Pluse)
                PosXStart = X0 * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX)    'X圆弧圆心坐标(Pluse)
                PosYStart = Y0 * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY)    'Y圆弧圆心坐标(Pluse)
                PosXCenter = PosXStart - CLng(TempPosX) 'X圆弧相对于起的圆心坐标(Pluse)
                PosYCenter = PosYStart - CLng(TempPosY) 'Y圆弧相对于起的圆心坐标(Pluse)

                With cArc
                    .CardNum = CardNum      '指定坐标系的卡号
                    .crd = 1                '指定坐标系号
                    .X = PosXEnd            '圆弧X终点坐标
                    .Y = PosYEnd            '圆弧Y终点坐标
                    .xCenter = PosXCenter   '圆弧X原心坐标
                    .yCenter = PosYCenter   '圆弧Y原心坐标
                    '.circleDir = 0          '顺时针圆弧
                    .circleDir = 1          '逆时针圆弧
                    .synVel = VelXY         '合成目标速度
                    .synAcc = 0.1           '合成加速度
                    .velEnd = 0             '终点速度
                    .fifo = 0               '缓冲区选择
                End With
                rtn = GT_CrdClear(cArc.CardNum, cArc.crd, cArc.fifo) '清除坐标系1的FIFO0
                rtn = GT_ArcXYC(cArc.CardNum, cArc.crd, cArc.X, cArc.Y, cArc.xCenter, cArc.yCenter, cArc.circleDir, cArc.synVel, cArc.synAcc)     'X轴正向
                rtn = GT_CrdStart(cArc.CardNum, cArc.crd, cArc.fifo) '启动坐标系1的FIFO0的插补运动
                GetProbe_Step = GetProbe_Step + 1
            Case 11
                rtn = GT_CrdStatus(CardNum, 1, Run_Status, Segment) '判断插补运动是否完成
                If Run_Status = 0 Then
                    GetProbe_Step = GetProbe_Step + 1
                End If
            Case 12
                rtn = GT_GetCaptureStatus(CardNum, AxisX, Probe_Status, ValueX(0)) '获收X轴探针捕获的状态和位置
                rtn = GT_GetCaptureStatus(CardNum, AxisY, Probe_Status, ValueY(0)) '获收Y轴探针捕获的状态和位置
                If Probe_Status = 1 Then    '判断探针捕获是否触发
                    Probe_Status = 0        '探针捕获触发清零
                    GetProbe_Step = 20
                    '                GetProbe_Step = 50   '直接获取4点坐标
                Else
                    GetProbe_Step = 1000
                End If

                '获取第2个校针点XY位置
            Case 20
                rtn = GT_SetCaptureMode(CardNum, AxisX, CAPTURE_PROBE) '设定X轴探针捕获
                rtn = GT_SetCaptureMode(CardNum, AxisY, CAPTURE_PROBE) '设定Y轴探针捕获
                VelXY = CDbl(Speed * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX) / 1000) '计算合成目标速度(Pluse/ms)
                TempPosX = X0 + (S2R / 2) / Sin(45) '计算X终点坐标(mm)
                TempPosY = Y0 + (S2R / 2) / Sin(45) '计算Y终点坐标(mm)
                PosXEnd = CLng(TempPosX * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX))  'X圆弧终点坐标(Pluse)
                PosYEnd = CLng(TempPosY * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY))  'Y圆弧终点坐标(Pluse)

                rtn = GT_GetPrfPos(CardNum, AxisX, TempPosX, 1, 0) '读取0号卡X轴当前规划位置(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisY, TempPosY, 1, 0) '读取0号卡Y轴当前规划位置(Pluse)
                PosXStart = X0 * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX)    'X圆弧圆心坐标(Pluse)
                PosYStart = Y0 * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY)    'Y圆弧圆心坐标(Pluse)
                PosXCenter = PosXStart - CLng(TempPosX) 'X圆弧相对于起的圆心坐标(Pluse)
                PosYCenter = PosYStart - CLng(TempPosY) 'Y圆弧相对于起的圆心坐标(Pluse)
                With cArc
                    .CardNum = CardNum      '指定坐标系的卡号
                    .crd = 1                '指定坐标系号
                    .X = PosXEnd            '圆弧X终点坐标
                    .Y = PosYEnd            '圆弧Y终点坐标
                    .xCenter = PosXCenter   '圆弧X原心坐标
                    .yCenter = PosYCenter   '圆弧Y原心坐标
                    '.circleDir = 0          '顺时针圆弧
                    .circleDir = 1          '逆时针圆弧
                    .synVel = VelXY         '合成目标速度
                    .synAcc = 0.1           '合成加速度
                    .velEnd = 0             '终点速度
                    .fifo = 0               '缓冲区选择
                End With
                rtn = GT_CrdClear(cArc.CardNum, cArc.crd, cArc.fifo) '清除坐标系1的FIFO0
                rtn = GT_ArcXYC(cArc.CardNum, cArc.crd, cArc.X, cArc.Y, cArc.xCenter, cArc.yCenter, cArc.circleDir, cArc.synVel, cArc.synAcc)     'X轴正向
                rtn = GT_CrdStart(cArc.CardNum, cArc.crd, cArc.fifo) '启动坐标系1的FIFO0的插补运动
                GetProbe_Step = GetProbe_Step + 1
            Case 21
                rtn = GT_CrdStatus(CardNum, 1, Run_Status, Segment) '判断插补运动是否完成
                If Run_Status = 0 Then
                    GetProbe_Step = GetProbe_Step + 1
                End If
            Case 22
                rtn = GT_GetCaptureStatus(CardNum, AxisX, Probe_Status, ValueX(1)) '获收X轴探针捕获的状态和位置
                rtn = GT_GetCaptureStatus(CardNum, AxisY, Probe_Status, ValueY(1)) '获收Y轴探针捕获的状态和位置
                If Probe_Status = 1 Then    '判断探针捕获是否触发
                    Probe_Status = 0        '探针捕获触发清零
                    GetProbe_Step = 30
                Else
                    GetProbe_Step = 1000
                End If

                '获取第3个校针点XY位置
            Case 30
                rtn = GT_SetCaptureMode(CardNum, AxisX, CAPTURE_PROBE) '设定X轴探针捕获
                rtn = GT_SetCaptureMode(CardNum, AxisY, CAPTURE_PROBE) '设定Y轴探针捕获
                VelXY = CDbl(Speed * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX) / 1000) '计算合成目标速度(Pluse/ms)
                TempPosX = X0 - (S2R / 2) / Sin(45) '计算X终点坐标(mm)
                TempPosY = Y0 + (S2R / 2) / Sin(45) '计算Y终点坐标(mm)
                PosXEnd = CLng(TempPosX * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX))  'X圆弧终点坐标(Pluse)
                PosYEnd = CLng(TempPosY * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY))  'Y圆弧终点坐标(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisX, TempPosX, 1, 0) '读取0号卡X轴当前规划位置(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisY, TempPosY, 1, 0) '读取0号卡Y轴当前规划位置(Pluse)
                PosXStart = X0 * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX)    'X圆弧圆心坐标(Pluse)
                PosYStart = Y0 * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY)    'Y圆弧圆心坐标(Pluse)
                PosXCenter = PosXStart - CLng(TempPosX) 'X圆弧相对于起的圆心坐标(Pluse)
                PosYCenter = PosYStart - CLng(TempPosY) 'Y圆弧相对于起的圆心坐标(Pluse)
                With cArc
                    .CardNum = CardNum      '指定坐标系的卡号
                    .crd = 1                '指定坐标系号
                    .X = PosXEnd            '圆弧X终点坐标
                    .Y = PosYEnd            '圆弧Y终点坐标
                    .xCenter = PosXCenter   '圆弧X原心坐标
                    .yCenter = PosYCenter   '圆弧Y原心坐标
                    '.circleDir = 0          '顺时针圆弧
                    .circleDir = 1          '逆时针圆弧
                    .synVel = VelXY         '合成目标速度
                    .synAcc = 0.1           '合成加速度
                    .velEnd = 0             '终点速度
                    .fifo = 0               '缓冲区选择
                End With
                rtn = GT_CrdClear(cArc.CardNum, cArc.crd, cArc.fifo) '清除坐标系1的FIFO0
                rtn = GT_ArcXYC(cArc.CardNum, cArc.crd, cArc.X, cArc.Y, cArc.xCenter, cArc.yCenter, cArc.circleDir, cArc.synVel, cArc.synAcc)     'X轴正向
                rtn = GT_CrdStart(cArc.CardNum, cArc.crd, cArc.fifo) '启动坐标系1的FIFO0的插补运动
                GetProbe_Step = GetProbe_Step + 1
            Case 31
                rtn = GT_CrdStatus(CardNum, 1, Run_Status, Segment) '判断插补运动是否完成
                If Run_Status = 0 Then
                    GetProbe_Step = GetProbe_Step + 1
                End If

            Case 32
                rtn = GT_GetCaptureStatus(CardNum, AxisX, Probe_Status, ValueX(2)) '获收X轴探针捕获的状态和位置
                rtn = GT_GetCaptureStatus(CardNum, AxisY, Probe_Status, ValueY(2)) '获收Y轴探针捕获的状态和位置
                If Probe_Status = 1 Then    '判断探针捕获是否触发
                    Probe_Status = 0        '探针捕获触发清零
                    GetProbe_Step = 40
                Else
                    GetProbe_Step = 1000
                End If

                '获取第4个校针点XY位置
            Case 40
                rtn = GT_SetCaptureMode(CardNum, AxisX, CAPTURE_PROBE) '设定X轴探针捕获
                rtn = GT_SetCaptureMode(CardNum, AxisY, CAPTURE_PROBE) '设定Y轴探针捕获
                VelXY = CDbl(Speed * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX) / 1000) '计算合成目标速度(Pluse/ms)
                TempPosX = X0 - (S2R / 2) / Sin(45) '计算X终点坐标(mm)
                TempPosY = Y0 - (S2R / 2) / Sin(45) '计算Y终点坐标(mm)
                PosXEnd = CLng(TempPosX * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX))  'X圆弧终点坐标(Pluse)
                PosYEnd = CLng(TempPosY * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY))  'Y圆弧终点坐标(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisX, TempPosX, 1, 0) '读取0号卡X轴当前规划位置(Pluse)
                rtn = GT_GetPrfPos(CardNum, AxisY, TempPosY, 1, 0) '读取0号卡Y轴当前规划位置(Pluse)
                PosXStart = X0 * AxisPar.pulse(CardNum, AxisX) * AxisPar.Gear(CardNum, AxisX) / AxisPar.Lead(CardNum, AxisX)    'X圆弧圆心坐标(Pluse)
                PosYStart = Y0 * AxisPar.pulse(CardNum, AxisY) * AxisPar.Gear(CardNum, AxisY) / AxisPar.Lead(CardNum, AxisY)    'Y圆弧圆心坐标(Pluse)
                PosXCenter = PosXStart - CLng(TempPosX) 'X圆弧相对于起的圆心坐标(Pluse)
                PosYCenter = PosYStart - CLng(TempPosY) 'Y圆弧相对于起的圆心坐标(Pluse)
                With cArc
                    .CardNum = CardNum      '指定坐标系的卡号
                    .crd = 1                '指定坐标系号
                    .X = PosXEnd            '圆弧X终点坐标
                    .Y = PosYEnd            '圆弧Y终点坐标
                    .xCenter = PosXCenter   '圆弧X原心坐标
                    .yCenter = PosYCenter   '圆弧Y原心坐标
                    '.circleDir = 0          '顺时针圆弧
                    .circleDir = 1          '逆时针圆弧
                    .synVel = VelXY         '合成目标速度
                    .synAcc = 0.1           '合成加速度
                    .velEnd = 0             '终点速度
                    .fifo = 0               '缓冲区选择
                End With
                rtn = GT_CrdClear(cArc.CardNum, cArc.crd, cArc.fifo) '清除坐标系1的FIFO0
                rtn = GT_ArcXYC(cArc.CardNum, cArc.crd, cArc.X, cArc.Y, cArc.xCenter, cArc.yCenter, cArc.circleDir, cArc.synVel, cArc.synAcc)     'X轴正向
                rtn = GT_CrdStart(cArc.CardNum, cArc.crd, cArc.fifo) '启动坐标系1的FIFO0的插补运动
                GetProbe_Step = GetProbe_Step + 1
            Case 41
                rtn = GT_CrdStatus(CardNum, 1, Run_Status, Segment) '判断插补运动是否完成
                If Run_Status = 0 Then
                    GetProbe_Step = GetProbe_Step + 1
                End If
            Case 42
                rtn = GT_GetCaptureStatus(CardNum, AxisX, Probe_Status, ValueX(3)) '获收X轴探针捕获的状态和位置
                rtn = GT_GetCaptureStatus(CardNum, AxisY, Probe_Status, ValueY(3)) '获收Y轴探针捕获的状态和位置
                If Probe_Status = 1 Then    '判断探针捕获是否触发
                    Probe_Status = 0        '探针捕获触发清零
                    GetProbe_Step = 50
                Else
                    GetProbe_Step = 1000
                End If

                '计算两直线交点坐标
            Case 50
                Call Intersection_XY(CDbl(ValueX(0)), CDbl(ValueY(0)), CDbl(ValueX(2)), CDbl(ValueY(2)), _
                    CDbl(ValueX(1)), CDbl(ValueY(1)), CDbl(ValueX(3)), CDbl(ValueY(3)), TempPosX, TempPosY)

                Intersection(0) = CDbl(TempPosX * AxisPar.Lead(CardNum, AxisX) / AxisPar.pulse(CardNum, AxisX) / AxisPar.Gear(CardNum, AxisX))   '交点位置X(mm)
                Intersection(1) = CDbl(TempPosY * AxisPar.Lead(CardNum, AxisY) / AxisPar.pulse(CardNum, AxisY) / AxisPar.Gear(CardNum, AxisY))   '交点位置Y(mm)
                GetProbe_Step = GetProbe_Step + 1
            Case 51
                GetProbe_Step = 800

                '自动校针成功
            Case 800
                GetProbe_Work.State = False
                GetProbe_Work.Result = True
                GetProbe_Step = 0
                Exit Sub

                '自动校针失败
            Case 1000
                GetProbe_Work.State = False
                GetProbe_Work.Result = False
                GetProbe_Step = 0
                Exit Sub
        End Select
    End Sub

    ''' <summary>
    ''' 求两直线的交点坐标XY
    ''' </summary>
    ''' <param name="Px1">参数1：直线1上点1的X坐标</param>
    ''' <param name="Py1">参数2：直线1上点1的Y坐标</param>
    ''' <param name="Px2">参数3：直线1上点2的X坐标</param>
    ''' <param name="Py2">参数4：直线1上点2的Y坐标</param>
    ''' <param name="Px11">参数5：直线2上点1的X坐标</param>
    ''' <param name="Py11">参数6：直线2上点1的Y坐标</param>
    ''' <param name="Px22">参数7：直线2上点2的X坐标</param>
    ''' <param name="Py22">参数8：直线2上点2的Y坐标</param>
    ''' <remarks></remarks>
    Private Sub Intersection_XY(Px1 As Double, Py1 As Double, Px2 As Double, Py2 As Double, _
                            Px11 As Double, Py11 As Double, Px22 As Double, Py22 As Long, ByRef Px As Double, ByRef Py As Double)
        Px = Intersection_X(Px1, Py1, Px2, Py2, Px11, Py11, Px22, Py22)
        Py = Intersection_Y(Px1, Py1, Px2, Py2, Px11, Py11, Px22, Py22)
    End Sub

    ''' <summary>
    ''' 求两直线的交点坐标X
    ''' </summary>
    ''' <param name="Px1">参数1：直线1上点1的X坐标</param>
    ''' <param name="Py1">参数2：直线1上点1的Y坐标</param>
    ''' <param name="Px2">参数3：直线1上点2的X坐标</param>
    ''' <param name="Py2">参数4：直线1上点2的Y坐标</param>
    ''' <param name="Px11">参数5：直线2上点1的X坐标</param>
    ''' <param name="Py11">参数6：直线2上点1的Y坐标</param>
    ''' <param name="Px22">参数7：直线2上点2的X坐标</param>
    ''' <param name="Py22">参数8：直线2上点2的Y坐标</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Intersection_X(Px1 As Double, Py1 As Double, Px2 As Double, Py2 As Double, _
                            Px11 As Double, Py11 As Double, Px22 As Double, Py22 As Long) As Double
        Dim Px, Py, k1, k2, B1, B2 As Double
        k1 = (Py2 - Py1) / (Px2 - Px1)                  '求直线方程y=Kx+B的斜率K
        k2 = (Py22 - Py11) / (Px22 - Px11)
        B1 = Py1 - ((Py2 - Py1)) / (Px2 - Px1) * Px1   '求直线方程y=Kx+B的B
        B2 = Py11 - ((Py22 - Py11)) / (Px22 - Px11) * Px11
        Px = (B2 - B1) / (k1 - k2)
        Py = k1 * Px + B1
        Intersection_X = Px
    End Function

    ''' <summary>
    ''' 求两直线的交点坐标Y
    ''' </summary>
    ''' <param name="Px1">参数1：直线1上点1的X坐标</param>
    ''' <param name="Py1">参数2：直线1上点1的Y坐标</param>
    ''' <param name="Px2">参数3：直线1上点2的X坐标</param>
    ''' <param name="Py2">参数4：直线1上点2的Y坐标</param>
    ''' <param name="Px11">参数5：直线2上点1的X坐标</param>
    ''' <param name="Py11">参数6：直线2上点1的Y坐标</param>
    ''' <param name="Px22">参数7：直线2上点2的X坐标</param>
    ''' <param name="Py22">参数8：直线2上点2的Y坐标</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Intersection_Y(Px1 As Double, Py1 As Double, Px2 As Double, Py2 As Double, _
                            Px11 As Double, Py11 As Double, Px22 As Double, Py22 As Long) As Double
        Dim Px, Py, k1, k2, B1, B2 As Double
        k1 = (Py2 - Py1) / (Px2 - Px1)                  '求直线方程y=Kx+B的斜率K
        k2 = (Py22 - Py11) / (Px22 - Px11)
        B1 = Py1 - ((Py2 - Py1) / (Px2 - Px1)) * Px1    '求直线方程y=Kx+B的B
        B2 = Py11 - ((Py22 - Py11) / (Px22 - Px11)) * Px11
        Px = (B2 - B1) / (k1 - k2)
        Py = k1 * Px + B1
        Intersection_Y = Py
    End Function

End Module
