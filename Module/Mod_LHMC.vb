Imports LH_MC2_6CH

Module Mod_LHMC
    '定义步进电机控制器对象
    Public LineMotorController As SendDate
    ''' <summary>
    ''' 步进电机控制器是否打开
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_isLineMotorControllerOpened As Boolean

    ''' <summary>
    ''' 实例化控制器对象，并初始化 必须要先打开串口
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Init_LineMotorController(ByVal COM As System.IO.Ports.SerialPort) As Boolean
        If COM.IsOpen = False Then
            Return False
        Else
            Try
                LineMotorController = New SendDate(COM)
                LineMotorController.Open()
                Delay(50)
                LineMotorController.Get_Status()
                Delay(50)
                Enable_LineMotor()
                Flag_isLineMotorControllerOpened = True
                Return True
            Catch ex As Exception
                Return False
            End Try

        End If
    End Function

    ''' <summary>
    ''' 所有电机同时使能
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Enable_LineMotor()
        LineMotorController.Motor_Enable(Aixs.all)
    End Sub

    ''' <summary>
    ''' 所有电机同时失能
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Disable_LineMotor()
        LineMotorController.Motor_Disable(Aixs.all)
    End Sub

    ''' <summary>
    ''' 开启流水步进电机转动
    ''' </summary>
    ''' <param name="chn">通道</param>
    ''' <param name="speed">每秒脉冲数</param>
    ''' <param name="dir">方向</param>
    ''' <remarks></remarks>
    Public Sub LineMotor_Run(ByVal chn As Integer, ByVal speed As Integer, ByVal dir As String)
        LineMotorController.Move(chn, speed, dir)
    End Sub

    ''' <summary>
    ''' 停止流水线步进电机
    ''' </summary>
    ''' <param name="chn"></param>
    ''' <remarks></remarks>
    Public Sub LineMotor_Stop(ByVal chn As Integer)
        LineMotorController.Stop(chn)
    End Sub

    ''' <summary>
    ''' 关闭流水线步进电机控制器
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Close_LineMotorController()
        Disable_LineMotor()
        Delay(50)
        LineMotorController.C1ose()
        Flag_isLineMotorControllerOpened = False
    End Sub

End Module
