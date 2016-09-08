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

        ' 判断否有某个轴在运动中
        If isAxisMoving(0, PasteX) Or isAxisMoving(0, PasteZ) Or isAxisMoving(0, PasteR) Or isAxisMoving(2, PasteY1) Then
            List_DebugAddMessage("组装工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Step_Gopos(2) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(2)
                Case 0
                Case 10
                Case 20
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
            Select Case Step_Gopos(4)
                Case 0
                Case 10
                Case 20
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

        Step_Gopos(6) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(6)
                Case 0
                Case 10
                Case 20
            End Select
        Loop
    End Sub

End Module
