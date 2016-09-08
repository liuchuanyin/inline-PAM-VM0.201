Module MachineInitialize

    Private OldTickCount As Long
    Public Step_MachineIni As Integer
    Public Flag_MachineInit As Boolean '机器初始化完成标志位
    Public Flag_MachineInitOngoing As Boolean '机器初始化是否正在进行标志位

    Public Sub Machine_Init()
        '判断是否紧急停止中
        If IsSysEmcStop And Flag_MachineInitOngoing = False Then
            Frm_DialogAddMessage("紧急停止中，请先解除急停")
            Exit Sub
        End If

        '确认是否要初始化
        If MsgBox("是否进行初始化机器，请确认！！", vbOKCancel + vbQuestion) <> vbOK Then
            Exit Sub
        End If

        '如果初始化已完成，退出，否则执行初始化函数
        If Flag_MachineInit Or Flag_MachineInitOngoing Then
            Exit Sub
        End If

        Step_MachineIni = 10
        Flag_MachineInitOngoing = True
        '打开设备初始化定时器
        Frm_Main.Timer_MacInit.Enabled = True
    End Sub

    ''' <summary>
    ''' 设备初始化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Machine_Initialize()
        'Static CurrentPos, CurrentPosEnc As Double
        Dim i, j As Short
        Dim rtn As Short
        Dim Status As Long

        '轴回原点成功记录
        Static Once(2, 8) As Boolean


        If Flag_MachineInit Then
            Exit Sub
        End If

        Select Case Step_MachineIni
            Case 10
                '判断是否开启安全门功能
                If par.chkFn(5) Then
                    If EXI(0, 3) And EXI(0, 4) Then
                    Else
                        Frm_DialogAddMessage("开启安全门功能，请检查安全门是否已关闭！")
                        Step_MachineIni = 9000
                        Exit Sub
                    End If
                End If
                Step_MachineIni = 20

            Case 20
                ListBoxAddMessage("开始初始化，谨防装机！")
                '所有轴回原点步序号清零,使能关闭
                ListBoxAddMessage("轴回原点步序号清零！")
                For i = 0 To GTS_CardNum - 1
                    For j = 1 To GTS_AxisNum(i)
                        HomeStep(i, j) = 0
                        AxisHome(i, j).Enable = False
                        Once(i, j) = False
                    Next
                Next
                Frm_ProgressBar.SetProgressBarValue(10)
                Step_MachineIni = 30

            Case 30
                '检查总正气压信号
                If EXI(2, 1) = False Then
                    Frm_DialogAddMessage("请检查设备气源信号是否正常！")
                    Step_MachineIni = 9000
                    Exit Sub
                End If
                Step_MachineIni = 50

            Case 50
                SetEXO(0, 8, False) '关闭组装站取料吸排线吸真空电磁阀
                SetEXO(0, 9, False) '关闭组装站取料吸排线破真空电磁阀
                SetEXO(0, 10, False) '关闭取料站取料吸排线吸真空电磁阀
                SetEXO(0, 11, False) '关闭取料站取料吸排线破真空电磁阀
                SetEXO(0, 12, False) '关闭组装站取料吸真空电磁阀
                SetEXO(0, 13, False) '关闭组装站取料破真空电磁阀
                SetEXO(0, 14, False) '关闭取料站取料吸真空电磁阀
                SetEXO(0, 15, False) '关闭取料站取料破真空电磁阀
                Step_MachineIni = 70

            Case 70
                SetEXO(1, 0, False) '关闭L0阻挡气缸电磁阀
                SetEXO(1, 2, False) '关闭L1阻挡气缸电磁阀
                SetEXO(1, 4, False) '关闭L2阻挡气缸电磁阀
                SetEXO(1, 6, False) '关闭L3阻挡气缸电磁阀

                SetEXO(1, 8, False) '关闭L1顶升气缸电磁阀
                SetEXO(1, 10, False) '关闭L2顶升气缸电磁阀
                SetEXO(1, 12, False) '关闭L3顶升气缸电磁阀

                SetEXO(1, 13, False) '关闭点胶1点胶
                SetEXO(1, 14, False) '关闭点胶2点胶
                SetEXO(1, 15, False) '关闭真空泵
                Frm_ProgressBar.SetProgressBarValue(15)
                Step_MachineIni = 80

            Case 80
                SetEXO(1, 0, False) '关闭启动按钮指示灯
                SetEXO(1, 5, False) '关闭三色灯红
                SetEXO(1, 6, False) '关闭三色灯黄
                SetEXO(1, 7, False) '关闭三色灯绿
                SetEXO(1, 8, False) '关闭三色灯蜂鸣器

                SetEXO(1, 9, False) '关闭点胶1升降气缸电磁阀
                SetEXO(1, 11, False) '关闭点胶2升降气缸电磁阀

                SetEXO(1, 13, False) '关闭OK指示灯
                SetEXO(1, 14, False) '关闭NG指示灯
                SetEXO(1, 15, False) '关闭蜂鸣器
                Step_MachineIni = 100

            Case 100
                SetEMO(0, 0, True)  '步进电机使能
                SetEMO(0, 5, True)  '左料盘电磁铁工作
                SetEMO(0, 6, True)  '右料盘电磁铁工作
                SetEMO(0, 11, False)  '关闭UV固化移动气缸

                SetEMO(1, 8, False)  '关闭L1真空吸载具电磁阀
                SetEMO(1, 9, False)  '关闭L2真空吸载具电磁阀
                SetEMO(1, 10, False)  '关闭L3真空吸载具电磁阀
                SetEMO(1, 11, False)  '关闭夹镜头保护盖气缸电磁阀
                SetEMO(1, 12, False)  '关闭夹镜头保护盖升降气缸电磁阀
                Step_MachineIni = 120

            Case 120
                If EMI(1, 0) Or EMI(1, 1) Or EMI(1, 2) Or EMI(1, 3) Or EMI(1, 4) Or EMI(1, 5) Or EMI(1, 6) Or EMI(1, 7) Then
                    Frm_DialogAddMessage("请检查流水线上是否有载具，并在移除载具后重新初始化！")
                    Step_MachineIni = 9000
                Else
                    Step_MachineIni = 130
                End If

            Case 130
                'PAM4 设备要判断2个气缸的上磁簧信号、、、、、
                If EXI(2, 9) Then
                    Step_MachineIni = 150
                Else
                    Frm_DialogAddMessage("请检查点胶气缸是否上升到位！")
                    Step_MachineIni = 9000
                End If

            Case 150
                '//如下使能
                For n = 0 To GTS_CardNum - 1
                    For i = 1 To GTS_AxisNum(n)
                        Call GT_ClrSts(n, i, 1)
                        Call GT_SetAxisBand(n, i, 500, 20)
                        Call GT_AxisOn(n, i)
                    Next i
                Next n
                OldTickCount = GetTickCount
                Step_MachineIni = 170

            Case 170
                If isTimeout(OldTickCount, 200) Then
                    Step_MachineIni = 180
                End If

            Case 180
                '//位置清零
                For n = 0 To GTS_CardNum - 1
                    For i = 1 To GTS_AxisNum(n)
                        rtn = GT_SetPrfPos(n, i, 0)
                        rtn = GT_SetEncPos(n, i, 0)
                        Call GT_SynchAxisPos(n, 2 ^ (i - 1))
                    Next i
                Next n
                Step_MachineIni = 200

            Case 200
                For n = 0 To GTS_CardNum - 1
                    For i = 1 To GTS_AxisNum(n)
                        If ServoOn(n, i) = False Then
                            Frm_DialogAddMessage("请打开所有伺服使能！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    Next
                Next
                Step_MachineIni = 300


            Case 300
                Step_MachineIni = 400

            Case 400
                '所有Z轴回到极限位置，负极限
                Call AbsMotion(0, GlueZ, AxisPar.HomeVel(0, GlueZ), -200)
                Call AbsMotion(0, PasteZ, AxisPar.HomeVel(0, PasteZ), -200)
                Call AbsMotion(0, PreTakerZ, AxisPar.HomeVel(0, PreTakerZ), -200)
                Step_MachineIni = 420

            Case 420
                '判断Z轴是否停止
                If isAxisMoving(0, GlueZ) = False And isAxisMoving(0, PasteZ) = False And isAxisMoving(0, PreTakerZ) = False Then
                    Step_MachineIni = 450
                End If

            Case 450
                Step_MachineIni = 500

            Case 500
                '取料站Y轴回负极限位置
                Call AbsMotion(2, PreTakerY1, AxisPar.HomeVel(2, PreTakerY1), -2000)
                AxisHome(0, GlueX).Enable = True    '点胶站X轴回原点
                AxisHome(0, GlueY).Enable = True    '点胶站Y轴回原点
                AxisHome(1, FineX).Enable = True    '精补站X轴回原点
                AxisHome(1, FineY).Enable = True    '精补站Y轴回原点
                AxisHome(1, RecheckX).Enable = True    '复检X轴回原点
                AxisHome(1, RecheckY).Enable = True    '复检Y轴回原点
                Step_MachineIni = 600

            Case 600
                Call Motor_Home(0, GlueX)
                Call Motor_Home(0, GlueY)
                Call Motor_Home(1, FineX)
                Call Motor_Home(1, FineY)
                Call Motor_Home(1, RecheckX)
                Call Motor_Home(1, RecheckY)

                If AxisHome(0, GlueX).State = False Then '等GlueX轴回原点完成
                    If Once(0, GlueX) = False Then
                        Once(0, GlueX) = True
                        If AxisHome(0, GlueX).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, GlueX) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, GlueX) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, GlueY).State = False Then '等GlueY轴回原点完成
                    If Once(0, GlueY) = False Then
                        Once(0, GlueY) = True
                        If AxisHome(0, GlueY).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, GlueY) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, GlueY) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, FineX).State = False Then '等FineX轴回原点完成
                    If Once(1, FineX) = False Then
                        Once(1, FineX) = True
                        If AxisHome(1, FineX).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, FineX) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, FineX) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, FineY).State = False Then '等FineY轴回原点完成
                    If Once(1, FineY) = False Then
                        Once(1, FineY) = True
                        If AxisHome(1, FineY).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, FineY) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, FineY) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, RecheckX).State = False Then '等RecheckX轴回原点完成
                    If Once(1, RecheckX) = False Then
                        Once(1, RecheckX) = True
                        If AxisHome(1, RecheckX).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, RecheckX) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, RecheckX) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, RecheckY).State = False Then '等RecheckY轴回原点完成
                    If Once(1, RecheckY) = False Then
                        Once(1, RecheckY) = True
                        If AxisHome(1, RecheckY).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, RecheckY) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, RecheckY) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, GlueX).State = False And AxisHome(0, GlueY).State = False And _
                    AxisHome(1, FineX).State = False And AxisHome(1, FineY).State = False And _
                    AxisHome(1, RecheckX).State = False And AxisHome(1, RecheckY).State = False Then
                    Step_MachineIni = 700
                End If


            Case 700
                '     取料站Y轴碰到负极限停止
                rtn = GT_GetSts(2, PreTakerY1, Status, 1, 0) '获取当前轴的状态
                If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                    Step_MachineIni = 800
                End If

            Case 800
                '组装站Y轴向取料站方向偏移150mm 方向是加
                Call AbsMotion(2, PasteY1, AxisPar.HomeVel(2, PasteY1), 150)
                Step_MachineIni = 900

            Case 900
                If GetDi(0, 1) = 1 Then
                    '如果光电感应到信号，那么急停组装站Y轴
                    Call GT_Stop(2, 2 ^ (PasteY1 - 1), 1)
                End If
                '     组装站Y轴停止
                rtn = GT_GetSts(2, PasteY1, Status, 1, 0) '获取当前轴的状态
                If CBool(Status And &H400) = False Then '判断当前轴是否运动停止
                    Step_MachineIni = 1000
                End If

            Case 1000
                AxisHome(1, CureX).Enable = True
                AxisHome(1, FeedZ).Enable = True
                AxisHome(1, RecycleZ).Enable = True
                Step_MachineIni = 1100

            Case 1100
                Call Motor_Home(1, CureX)
                Call Motor_Home(1, FeedZ)
                Call Motor_Home(1, RecycleZ)

                If AxisHome(1, CureX).State = False Then '等CureX轴回原点完成
                    If Once(1, CureX) = False Then
                        Once(1, CureX) = True
                        If AxisHome(1, CureX).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, CureX) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, CureX) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, FeedZ).State = False Then '等FeedZ轴回原点完成
                    If Once(1, FeedZ) = False Then
                        Once(1, FeedZ) = True
                        If AxisHome(1, FeedZ).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, FeedZ) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, FeedZ) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, RecycleZ).State = False Then '等RecycleZ轴回原点完成
                    If Once(1, RecycleZ) = False Then
                        Once(1, RecycleZ) = True
                        If AxisHome(1, RecycleZ).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, RecycleZ) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, RecycleZ) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                '
                If AxisHome(1, CureX).State = False And AxisHome(1, FeedZ).State = False And AxisHome(1, RecycleZ).State = False Then
                    Step_MachineIni = 1200
                End If

            Case 1200
                Call AbsMotion(1, CureX, AxisPar.MoveVel(1, CureX), Par_Pos.St_Cure(0).X)   'CureX 运动到待机位置
                Call AbsMotion(1, FeedZ, AxisPar.MoveVel(1, FeedZ), Par_Pos.St_Feed(0).Z)   'FeedZ 运动到待机位置
                Call AbsMotion(1, RecycleZ, AxisPar.MoveVel(1, RecycleZ), Par_Pos.St_Recycle(0).Z)   'FeedZ 运动到待机位置
                OldTickCount = GetTickCount
                Step_MachineIni = 1300


            Case 1300
                If isAxisMoving(1, CureX) = False Then
                    Step_MachineIni = 1400
                ElseIf isTimeout(OldTickCount, 5000) Then
                    Frm_DialogAddMessage(AxisPar.axisName(1, CureX) & "回待机位置超时！")
                    Step_MachineIni = 9000
                End If

            Case 1400
                '组装站X，Y同时回原点
                AxisHome(2, PasteY1).Enable = True
                AxisHome(0, PasteX).Enable = True
                Step_MachineIni = 1410

            Case 1410
                Call Motor_Home(0, PasteX)
                Call Motor_Home(2, PasteY1)

                If AxisHome(0, PasteX).State = False Then '等待PasteX轴回原点完成
                    If Once(0, PasteX) = False Then
                        Once(0, PasteX) = True
                        If AxisHome(0, PasteX).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, PasteX) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, PasteX) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(2, PasteY1).State = False Then '等待PasteY1轴回原点完成
                    If Once(2, PasteY1) = False Then
                        Once(2, PasteY1) = True
                        If AxisHome(2, PasteY1).Result Then
                            ListBoxAddMessage(AxisPar.axisName(2, PasteY1) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(2, PasteY1) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, PasteX).State = False And AxisHome(2, PasteY1).State = False Then
                    Step_MachineIni = 1500
                End If

            Case 1500
                '取料站X，Y同时回原点
                AxisHome(2, PreTakerY1).Enable = True
                AxisHome(0, PreTakerX).Enable = True
                Step_MachineIni = 1510

            Case 1510
                Call Motor_Home(0, PreTakerX)
                Call Motor_Home(2, PreTakerY1)

                If AxisHome(0, PreTakerX).State = False Then '等待PreTakerX轴回原点完成
                    If Once(0, PreTakerX) = False Then
                        Once(0, PreTakerX) = True
                        If AxisHome(0, PreTakerX).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, PreTakerX) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, PreTakerX) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(2, PreTakerY1).State = False Then '等待PreTakerY1轴回原点完成
                    If Once(2, PreTakerY1) = False Then
                        Once(2, PreTakerY1) = True
                        If AxisHome(2, PreTakerY1).Result Then
                            ListBoxAddMessage(AxisPar.axisName(2, PreTakerY1) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(2, PreTakerY1) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, PreTakerX).State = False And AxisHome(2, PreTakerY1).State = False Then
                    Step_MachineIni = 1600
                End If


            Case 1600
                '点胶工站X,Y，组装工站X,Y，取料工站X,Y回待机位置
                Call AbsMotion(0, GlueX, AxisPar.MoveVel(0, GlueX), Par_Pos.St_Glue(0).X)
                Call AbsMotion(0, GlueY, AxisPar.MoveVel(0, GlueY), Par_Pos.St_Glue(0).Y)

                Call AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(0).X)
                Call AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(0).Y1)

                Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), Par_Pos.St_PreTaker(0).X)
                Call AbsMotion(2, PreTakerY1, AxisPar.MoveVel(2, PreTakerY1), Par_Pos.St_PreTaker(0).Y1)
                OldTickCount = GetTickCount
                Step_MachineIni = 1610

            Case 1610
                If isAxisMoving(0, GlueX) = False And isAxisMoving(0, GlueY) = False _
                    And isAxisMoving(0, PasteX) = False And isAxisMoving(2, PasteY1) = False _
                    And isAxisMoving(0, PreTakerX) = False And isAxisMoving(2, PreTakerY1) = False Then
                    Step_MachineIni = 1700
                ElseIf isTimeout(OldTickCount, 5000) Then
                    Frm_DialogAddMessage("点胶XY，组装XY和取料XY同时回待机位置超时！")
                    Step_MachineIni = 9000
                End If

            Case 1700
                '点胶站、组装站和取料站Z轴回原点
                AxisHome(0, GlueZ).Enable = True
                AxisHome(0, PasteZ).Enable = True
                AxisHome(0, PreTakerZ).Enable = True
                Step_MachineIni = 1710

            Case 1710
                'Z轴回原点
                Call Motor_Home(0, GlueZ)
                Call Motor_Home(0, PasteZ)
                Call Motor_Home(0, PreTakerZ)

                If AxisHome(0, GlueZ).State = False Then '等待GlueZ轴回原点完成
                    If Once(0, GlueZ) = False Then
                        Once(0, GlueZ) = True
                        If AxisHome(0, GlueZ).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, GlueZ) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, GlueZ) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, PasteZ).State = False Then '等待PasteZ轴回原点完成
                    If Once(0, PasteZ) = False Then
                        Once(0, PasteZ) = True
                        If AxisHome(0, PasteZ).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, PasteZ) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, PasteZ) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, PreTakerZ).State = False Then '等待PreTakerZ轴回原点完成
                    If Once(0, PreTakerZ) = False Then
                        Once(0, PreTakerZ) = True
                        If AxisHome(0, PreTakerZ).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, PreTakerZ) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, PreTakerZ) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, GlueZ).State = False And AxisHome(0, PreTakerZ).State = False And AxisHome(0, PasteZ).State = False Then
                    Step_MachineIni = 1800
                End If

            Case 1800
                '所有Z轴回待机位置
                Call AbsMotion(0, GlueZ, AxisPar.MoveVel(0, GlueZ), Par_Pos.St_Glue(0).Z)
                Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                Step_MachineIni = 1810

            Case 1810
                If isAxisMoving(0, GlueZ) = False And isAxisMoving(0, PasteZ) = False And isAxisMoving(0, PreTakerZ) = False Then
                    Step_MachineIni = 1900
                End If

            Case 1900
                'R 轴回原点
                AxisHome(0, PasteR).Enable = True
                AxisHome(1, PreTakerR).Enable = True
                Step_MachineIni = 1910

            Case 1910
                Call Motor_Home(0, PasteR, True)
                Call Motor_Home(1, PreTakerR, True)

                If AxisHome(0, PasteR).State = False Then '等待PasteR轴回原点完成
                    If Once(0, PasteR) = False Then
                        Once(0, PasteR) = True
                        If AxisHome(0, PasteR).Result Then
                            ListBoxAddMessage(AxisPar.axisName(0, PasteR) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(0, PasteR) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(1, PreTakerR).State = False Then '等待PreTakerR轴回原点完成
                    If Once(1, PreTakerR) = False Then
                        Once(1, PreTakerR) = True
                        If AxisHome(1, PreTakerR).Result Then
                            ListBoxAddMessage(AxisPar.axisName(1, PreTakerR) & "轴回原点成功！")
                        Else
                            Frm_DialogAddMessage(AxisPar.axisName(1, PreTakerR) & "轴回原点失败！")
                            Step_MachineIni = 9000
                            Exit Sub
                        End If
                    End If
                End If

                If AxisHome(0, PasteR).State = False And AxisHome(1, PreTakerR).State = False Then
                    Step_MachineIni = 2000
                End If

            Case 2000
                Call AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(0).R)    '组装站R回待机位置
                Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Par_Pos.St_PreTaker(0).R)    '取料R回待机位置

                Call AbsMotion(1, FineX, AxisPar.MoveVel(1, FineX), Par_Pos.St_FineCompensation(0).X)   '精补X回待机位置
                Call AbsMotion(1, FineY, AxisPar.MoveVel(1, FineY), Par_Pos.St_FineCompensation(0).Y)   '精补Y回待机位置

                Call AbsMotion(1, RecheckX, AxisPar.MoveVel(1, RecheckX), Par_Pos.St_Recheck(0).X)   '复检X回待机位置
                Call AbsMotion(1, RecheckY, AxisPar.MoveVel(1, RecheckY), Par_Pos.St_Recheck(0).Y)   '复检Y回待机位置
                OldTickCount = GetTickCount
                Step_MachineIni = 2110

            Case 2110
                If isAxisMoving(0, PasteR) = False And isAxisMoving(1, PreTakerR) = False And _
                    isAxisMoving(1, FineX) = False And isAxisMoving(1, FineY) = False And _
                    isAxisMoving(1, RecheckX) = False And isAxisMoving(1, RecheckY) = False Then

                    Step_MachineIni = 2200
                ElseIf isTimeout(GetTickCount, 5000) Then
                    Frm_DialogAddMessage("R轴，精补XY和复检XY同时回待机位置超时！")
                    Step_MachineIni = 9000
                End If

            Case 2200

                '需要处理的变量


            Case 2300
                Step_MachineIni = 8000

                '初始化成功
            Case 8000
                Flag_MachineInit = True
                Flag_MachineInitOngoing = False
                ListBoxAddMessage("初始化成功！")
                Step_MachineIni = 0

                '初始化失败
            Case 9000
                '所有轴回原点步序号清零,使能关闭
                rtn = GT_Stop(0, 255, 255)  '紧急停止0号卡所有轴
                rtn = GT_Stop(1, 255, 255)  '紧急停止1号卡所有轴
                For i = 0 To GTS_CardNum - 1
                    For j = 1 To GTS_AxisNum(i)
                        HomeStep(i, j) = 0
                        AxisHome(i, j).Enable = False
                        Once(i, j) = False
                    Next
                Next
                Frm_DialogAddMessage("初始化失败！")
                Flag_MachineInit = False
                Flag_MachineInitOngoing = False
                Step_MachineIni = 0

        End Select

    End Sub
End Module
