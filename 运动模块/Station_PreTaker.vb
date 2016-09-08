Module Station_PreTaker

    Public Step_PreTaker As Integer

    Public Sub GoPos_PreTaker(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(0, PreTakerX) And ServoOn(2, PreTakerY1) And ServoOn(0, PreTakerZ) And ServoOn(1, PreTakerR) Then
        Else
            List_DebugAddMessage("请先打开取料工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(0, PreTakerX) Or isAxisMoving(0, PreTakerZ) Or isAxisMoving(1, PreTakerR) Or isAxisMoving(2, PreTakerY1) Then
            List_DebugAddMessage("取料工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Step_Gopos(3) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(3)
                Case 0
                Case 10
                Case 20
            End Select
        Loop
    End Sub

    Public Sub GoPos_Feed(ByVal index As Short)
        '判断是否所有轴伺服ON
        If ServoOn(1, FeedZ) And ServoOn(1, RecycleZ) Then
        Else
            List_DebugAddMessage("请先打开供料工位所有轴伺服ON")
            Exit Sub
        End If

        ' 判断否有某个轴在运动中
        If isAxisMoving(1, FeedZ) Or isAxisMoving(1, RecycleZ) Then
            List_DebugAddMessage("供料工位有轴正在运动中，请等待")
            Exit Sub
        End If

        '判断设备是否初始化完成
        If Flag_MachineInit = False Then
            List_DebugAddMessage("机器未就绪，请先初始化")
            Exit Sub
        End If

        Step_Gopos(7) = 0
        Do While True
            My.Application.DoEvents()
            Select Case Step_Gopos(7)
                Case 0
                Case 10
                Case 20
            End Select
        Loop
    End Sub

End Module
