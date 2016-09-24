Module Mod_PipeLine

    Public Step_Line(4) As Integer
    Public Const Speed_LineMotor As Integer = 5000

    ''' <summary>
    ''' 控制第i段流水线运行
    ''' </summary>
    ''' <param name="PipeLine_Segment">i段流水线 0,1,2,3,4</param>
    ''' <remarks></remarks>
    Public Sub setMotorRun(ByVal PipeLine_Segment As Short)
        If Flag_isLineMotorControllerOpened Then
            Call LineMotor_Run(PipeLine_Segment + 1, Speed_LineMotor, "-")
        Else
            Frm_DialogAddMessage("流水线步进电机未打开！")
        End If
    End Sub

    ''' <summary>
    ''' 控制第i段流水线反转运行
    ''' </summary>
    ''' <param name="PipeLine_Segment">i段流水线 0,1,2,3,4</param>
    ''' <remarks></remarks>
    Public Sub setMotorRunBack(ByVal PipeLine_Segment As Short)
        If Flag_isLineMotorControllerOpened Then
            Call LineMotor_Run(PipeLine_Segment + 1, Speed_LineMotor, "+")
        Else
            Frm_DialogAddMessage("流水线步进电机未打开！")
        End If
    End Sub

    ''' <summary>
    ''' 控制第i段流水线停止
    ''' </summary>
    ''' <param name="PipeLine_Segment">i段流水线 0,1,2,3,4</param>
    ''' <remarks></remarks>
    Public Sub setMotorStop(ByVal PipeLine_Segment As Short)
        If Flag_isLineMotorControllerOpened Then
            Call LineMotor_Stop(PipeLine_Segment + 1)
        Else
            Frm_DialogAddMessage("流水线步进电机未打开！")
        End If
    End Sub

    ''' <summary>
    ''' 控制第i段流水线阻挡气缸伸出或缩回
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <param name="block">是阻挡还是放行？ true: 阻挡；false: 放行</param>
    ''' <remarks></remarks>
    Public Sub setBlock(ByVal PipeLine_Segment As Short, ByVal block As Boolean)
        Select Case PipeLine_Segment
            Case 0
                SetEXO(1, 0, block)
            Case 1
                SetEXO(1, 2, block)
            Case 2
                SetEXO(1, 4, block)
            Case 3
                SetEXO(1, 6, block)
            Case Else
        End Select
    End Sub

    ''' <summary>
    ''' 判断第i段流水线阻挡气缸是否阻挡到位
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isBlocked(ByVal PipeLine_Segment As Short) As Boolean
        Dim mValue As Boolean
        Select Case PipeLine_Segment
            Case 0
                mValue = EXI(1, 0)
            Case 1
                mValue = EXI(1, 2)
            Case 2
                mValue = EXI(1, 4)
            Case 3
                mValue = EXI(1, 6)
            Case Else
        End Select
        Return mValue
    End Function

    ''' <summary>
    ''' 判断第i段流水线阻挡气缸是否缩回到位
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isBlockedOff(ByVal PipeLine_Segment As Short) As Boolean
        Dim mValue As Boolean
        Select Case PipeLine_Segment
            Case 0
                mValue = EXI(1, 1)
            Case 1
                mValue = EXI(1, 3)
            Case 2
                mValue = EXI(1, 5)
            Case 3
                mValue = EXI(1, 7)
            Case Else
        End Select
        Return mValue
    End Function

    ''' <summary>
    ''' 控制第i段流水线顶升气缸伸出或缩回
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <param name="rise">是升起还是落下？ true: 升起；false: 落下</param>
    ''' <remarks></remarks>
    Public Sub setCylinderRise(ByVal PipeLine_Segment As Short, ByVal rise As Boolean)
        Select Case PipeLine_Segment
            Case 1
                SetEXO(1, 8, rise)
            Case 2
                SetEXO(1, 10, rise)
            Case 3
                SetEXO(1, 12, rise)
            Case Else
        End Select
    End Sub

    ''' <summary>
    ''' 判断第i段流水线顶升气缸是否升起到位
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isCylinderRised(ByVal PipeLine_Segment As Short) As Boolean
        Dim mValue As Boolean
        Select Case PipeLine_Segment
            'Case 0
            '    mValue = EXI(1, 8)
            Case 1
                mValue = EXI(1, 8)
            Case 2
                mValue = EXI(1, 10)
            Case 3
                mValue = EXI(1, 12)
            Case Else
        End Select
        Return mValue
    End Function

    ''' <summary>
    ''' 判断第i段流水线顶升气缸是否下降到位
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isCylinderDown(ByVal PipeLine_Segment As Short) As Boolean
        Dim mValue As Boolean
        Select Case PipeLine_Segment
            'Case 0
            '    mValue = EXI(1, 8)
            Case 1
                mValue = EXI(1, 9)
            Case 2
                mValue = EXI(1, 11)
            Case 3
                mValue = EXI(1, 13)
            Case Else
        End Select
        Return mValue
    End Function

    ''' <summary>
    ''' 判断第i段流水线上是否有产品
    ''' </summary>
    ''' <param name="PipeLine_Segment">第i段流水线 0,1,2,3,4</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function isHaveTray(ByVal PipeLine_Segment As Short) As Boolean
        Dim mValue As Boolean
        Select Case PipeLine_Segment
            Case 0
                mValue = EMI(1, 0) And EMI(1, 1)
            Case 1
                mValue = EMI(1, 2) And EMI(1, 3)
            Case 2
                mValue = EMI(1, 4) And EMI(1, 5)
            Case 3
                mValue = EMI(1, 6) And EMI(1, 7)
            Case Else
        End Select
        Return mValue
    End Function


End Module
