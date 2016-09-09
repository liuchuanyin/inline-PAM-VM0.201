Module Station_Paste

    Public Step_Paste As Integer

    '组装XYZR去指定位置
    Public Sub GoPos_Paste(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(0, PasteX) And ServoOn(2, PasteY1) And ServoOn(0, PasteZ) And ServoOn(0, PasteR) Then
        Else
            List_DebugAddMessage("请先打开组装工位所有轴伺服ON")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(0, PasteX) Or isAxisMoving(0, PasteZ) Or isAxisMoving(0, PasteR) Or isAxisMoving(2, PasteY1) Then
            List_DebugAddMessage("组装工位有轴正在运动中，请等待")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(1, CureX) - Par_Pos.St_Cure(0).X) > 2 Then
            List_DebugAddMessage("请检查预固化轴是否在安全位置！")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(2, PreTakerY1) - Par_Pos.St_PreTaker(0).Y) > 2 Then
            List_DebugAddMessage("请检查取料模组是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(2) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(2)
                Case 0
                    Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(0).Z)
                    Step_Gopos(2) = 10

                Case 10
                    If isAxisMoving(0, PasteZ) = False Then
                        Frm_DialogAddMessage("组装站Z轴运动到待机位置完成")
                        Step_Gopos(2) = 20
                    End If

                Case 20
                    Call AbsMotion(0, PasteX, AxisPar.MoveVel(0, PasteX), Par_Pos.St_Paste(index).X)
                    Call AbsMotion(0, PasteR, AxisPar.MoveVel(0, PasteR), Par_Pos.St_Paste(index).R)
                    Call AbsMotion(2, PasteY1, AxisPar.MoveVel(2, PasteY1), Par_Pos.St_Paste(index).Y)
                    Step_Gopos(2) = 30

                Case 30
                    If isAxisMoving(0, PasteR) = False And isAxisMoving(0, PasteX) = False And isAxisMoving(2, PasteY1) = False Then
                        Frm_DialogAddMessage("组装站X,Y,R轴运动到" & Par_Pos.St_Paste(index).Name & "完成")
                        Step_Gopos(2) = 40
                    End If

                Case 40
                    Call AbsMotion(0, PasteZ, AxisPar.MoveVel(0, PasteZ), Par_Pos.St_Paste(index).Z)
                    Step_Gopos(2) = 50

                Case 50
                    If isAxisMoving(0, PasteZ) = False Then
                        Frm_DialogAddMessage("组装站Z轴运动到" & Par_Pos.St_Paste(index).Name & "完成")
                        Step_Gopos(2) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub
    '精补XY去指定位置
    Public Sub GoPos_FineCompensation(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, FineX) And ServoOn(1, FineY) Then
        Else
            List_DebugAddMessage("请先打开精补工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, FineX) Or isAxisMoving(1, FineY) Then
            List_DebugAddMessage("精补工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Step_Gopos(4) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(4)
                Case 0
                    Call AbsMotion(1, FineX, AxisPar.MoveVel(1, FineX), Par_Pos.St_FineCompensation(index).X)
                    Call AbsMotion(1, FineY, AxisPar.MoveVel(1, FineY), Par_Pos.St_FineCompensation(index).Y)
                    Step_Gopos(4) = 10

                Case 10
                    If isAxisMoving(1, FineX) = False And isAxisMoving(1, FineY) = False Then
                        Frm_DialogAddMessage("精补模组运动到" & Par_Pos.St_FineCompensation(index).Name & "完成！")
                        Step_Gopos(4) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub
    '预固化去指定位置
    Public Sub GoPos_Cure(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, CureX) Then
        Else
            List_DebugAddMessage("请先打开CureX轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, CureX) Then
            List_DebugAddMessage("CureX轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        If Math.Abs(CurrEncPos(2, PasteY1) - Par_Pos.St_Paste(0).Y) > 2 Then
            List_DebugAddMessage("请检组装站模组是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(6) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(6)
                Case 0
                    Call AbsMotion(1, CureX, AxisPar.MoveVel(1, CureX), Par_Pos.St_Cure(index).X)
                    Step_Gopos(6) = 10

                Case 10
                    If isAxisMoving(1, CureX) = False Then
                        Frm_DialogAddMessage("预固化模组运动到" & Par_Pos.St_Cure(index).Name & "完成")
                        Step_Gopos(6) = 0
                        Exit Do
                    End If
            End Select
        Loop
    End Sub

End Module
