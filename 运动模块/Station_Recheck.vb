Module Station_Recheck
    Public Step_Recheck As Integer

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

End Module
