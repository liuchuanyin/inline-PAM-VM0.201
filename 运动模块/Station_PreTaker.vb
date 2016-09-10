Module Station_PreTaker

    Public Step_PreTaker As Integer

    ''' <summary>
    ''' 中转平台上的物料信息
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure TransferPlate
        Dim isHaveCam As Boolean
        Dim Barcode As String
        Dim TakerPress As Double
    End Structure

    Public Cam_OnTransferPlate As TransferPlate

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

        If Math.Abs(CurrEncPos(2, PasteY1) - Par_Pos.St_Paste(0).Y) > 2 Then
            List_DebugAddMessage("请检查组装模组是否在安全位置！")
            Exit Sub
        End If

        Step_Gopos(3) = 0
        Do While True
            My.Application.DoEvents()
            Delay(10)
            Select Case Step_Gopos(3)
                Case 0
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(0).Z)
                    Step_Gopos(3) = 10

                Case 10
                    If isAxisMoving(0, PreTakerZ) = False Then
                        List_DebugAddMessage("取料站Z轴运动到待机位置完成")
                        Step_Gopos(3) = 20
                    End If

                Case 20
                    Call AbsMotion(0, PreTakerX, AxisPar.MoveVel(0, PreTakerX), Par_Pos.St_PreTaker(index).X)
                    Call AbsMotion(1, PreTakerR, AxisPar.MoveVel(1, PreTakerR), Par_Pos.St_PreTaker(index).R)
                    Call AbsMotion(2, PreTakerY1, AxisPar.MoveVel(0, PreTakerY1), Par_Pos.St_PreTaker(index).Y)
                    Step_Gopos(3) = 30

                Case 30
                    If isAxisMoving(0, PreTakerX) = False And isAxisMoving(1, PreTakerR) = False And isAxisMoving(2, PreTakerY1) = False Then
                        List_DebugAddMessage("取料站XYR运动到" & Par_Pos.St_PreTaker(index).Name & "完成")
                        Step_Gopos(3) = 40
                    End If

                Case 40
                    Call AbsMotion(0, PreTakerZ, AxisPar.MoveVel(0, PreTakerZ), Par_Pos.St_PreTaker(index).Z)
                    Step_Gopos(3) = 50

                Case 50
                    If isAxisMoving(0, PreTakerZ) = False Then
                        List_DebugAddMessage("取料站Z运动到" & Par_Pos.St_PreTaker(index).Name & "完成")
                        Step_Gopos(3) = 0
                        Exit Do
                    End If

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

        Select Case index
            Case 0
                Call AbsMotion(1, FeedZ, AxisPar.MoveVel(1, FeedZ), Par_Pos.St_Feed(0).Z)
                Call AbsMotion(1, RecycleZ, AxisPar.MoveVel(1, RecycleZ), Par_Pos.St_Recycle(0).Z)

            Case 1, 2
                Call AbsMotion(1, FeedZ, AxisPar.MoveVel(1, FeedZ), Par_Pos.St_Feed(index).Z)

            Case 3, 4
                Call AbsMotion(1, RecycleZ, AxisPar.MoveVel(1, RecycleZ), Par_Pos.St_Recycle(index).Z)
        End Select

        Do While True
            My.Application.DoEvents()
            If isAxisMoving(1, FeedZ) = False And isAxisMoving(1, RecycleZ) = False Then
                List_DebugAddMessage("供料Z轴运动到指定位置OK")
            End If
        Loop
    End Sub

    Public Function isHaveBracketNotPasteCamera() As Boolean
        Dim mValue As Boolean

        Return mValue
    End Function

    Public Sub AutoRun_PreTakerStation()
        'PreTaker_Sta.workState = 0   '工作进行中
        'PreTaker_Sta.workState = 1   '工作完成
        'PreTaker_Sta.workState = 2   '工作进行中：拍物料
        'PreTaker_Sta.workState = 3   '工作进行中：取料
        'PreTaker_Sta.workState = 4   '工作进行中：去放料，夹镜头保护盖
        Select Case Step_PreTaker
            Case 10
                If Flag_MachineStop = False And Cam_OnTransferPlate.isHaveCam = False And Feed_Sta.isWorking = False And Feed_Sta.workState = 1 Then

                End If


        End Select
    End Sub

End Module
