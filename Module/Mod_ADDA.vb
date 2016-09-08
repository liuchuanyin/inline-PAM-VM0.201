''' <summary>
''' 模拟量输入输出模块
''' </summary>
''' <remarks></remarks>
Module Mod_ADDA
    ''' <summary>
    ''' 读取模拟量输入口的电压
    ''' </summary>
    ''' <remarks></remarks>
    Public Voltage_Read(6) As Double
    ''' <summary>
    ''' 设置模拟量输出口的电压
    ''' </summary>
    ''' <remarks></remarks>
    Public Voltage_Set(6) As Double

    Public Sub SetPressure(ByVal chn As Short, ByVal pressure As Double)
        Dim rtn As Integer
        Dim voltage As Double
        '根据比例计算
        voltage = pressure * 20
        If voltage < 0 Then
            voltage = 0
        End If
        If voltage > 10 Then
            voltage = 10
        End If
        rtn = GT_SetExtDaVoltageGts(1, 0, chn, voltage)

    End Sub

    Public Function ReadPressure(ByVal chn As Short) As Double
        Dim rtn As Integer
        Dim voltage As Double
        Dim result As Double
        rtn = GT_GetExtAdVoltageGts(1, 0, chn, voltage)
        '根据比例计算
        result = IIf(voltage <> 0, voltage * 0.125 - 0.12, 0)
        Return result
    End Function

End Module
