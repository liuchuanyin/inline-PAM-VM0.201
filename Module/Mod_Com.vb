Module Mod_Com
    Public COM1_Work As sFlag3
    Public COM2_Work As sFlag3
    Public COM3_Work As sFlag3
    Public COM4_Work As sFlag3

    Public Com1_TimmingWatch As Long
    Public Com2_TimmingWatch As Long
    Public Com3_TimmingWatch As Long
    Public Com4_TimmingWatch As Long

    Public Flag_COMOpened(5) As Boolean

    '压力值，0：组装压力；1取料压力；2标准表压力
    Public Press(3) As Double

    Public COM2_String As String
    Public COM2_sData() As String
    Public COM2_Data(255) As Double

    Public COM3_String As String
    Public COM3_sData() As String
    Public COM3_Data(255) As Double

    ''' <summary>
    ''' 组装站压力传感器发送数据
    ''' </summary>
    ''' <param name="Command"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Com1_Send(ByVal Command As String) As Boolean
        On Error Resume Next
        If Frm_Main.COM1.IsOpen = False Then
            Com1_Send = False '串口1打开失败，返回false
            Exit Function
        End If
        COM1_Work.State = True
        Frm_Main.COM1.Write(Command)
        COM1_Work.State = False
        COM1_Work.Result = True
        Com1_Send = True    '串口1发送命令成功
        Com1_TimmingWatch = GetTickCount()

    End Function

    ''' <summary>
    ''' 组装站压力传感器命令响应
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Com1_Return() As Integer
        Com1_Return = 2
        If COM1_Work.State = False Then
            If COM1_Work.Result Then
                Com1_Return = 0
            Else
                Com1_Return = 1
            End If
        ElseIf (GetTickCount() - Com1_TimmingWatch) >= 3000 Then
            Com1_Return = 1
        End If
    End Function

    ''' <summary>
    ''' 点胶站Laser发送数据
    ''' </summary>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Com2_Send(ByVal command As String) As Boolean
        On Error Resume Next
        If Not Frm_Main.COM2.IsOpen Then
            Return False
            Exit Function
        End If
        Frm_Main.COM2.Write(command)
        COM2_Work.State = True
        COM2_Work.Result = False
        Com2_TimmingWatch = GetTickCount()
        Return True
    End Function

    ''' <summary>
    ''' 点胶站Laser命令响应
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Com2_Return() As Integer
        Com2_Return = 2
        If COM2_Work.State = False Then
            If COM2_Work.Result Then
                Com2_Return = 0
            Else
                Com2_Return = 1
            End If
        ElseIf (GetTickCount() - Com2_TimmingWatch) >= 3000 Then
            Com2_Return = 1
        End If
    End Function

    ''' <summary>
    ''' 预取料站压力传感器发送数据
    ''' </summary>
    ''' <param name="command"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Com3_Send(ByVal command As String) As Boolean
        On Error Resume Next
        If Not Frm_Main.COM3.IsOpen Then
            Return False
            Exit Function
        End If
        Frm_Main.COM3.Write(command)
        COM3_Work.State = True
        COM3_Work.Result = False
        Com3_TimmingWatch = GetTickCount()
        Return True
    End Function

    ''' <summary>
    ''' 预取料站压力传感器命令处理 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Com3_Return() As Integer
        Com3_Return = 2
        If COM3_Work.State = False Then
            If COM3_Work.Result Then
                Com3_Return = 0
            Else
                Com3_Return = 1
            End If
        ElseIf (GetTickCount() - Com3_TimmingWatch) >= 3000 Then
            Com3_Return = 1
        End If
    End Function

    Public Function Com4_Send(ByVal Command As String) As Integer
        On Error Resume Next
        If Frm_Main.COM4.IsOpen = False Then
            Com4_Send = False '串口4打开失败，返回false
            Exit Function
        End If
        COM4_Work.State = True
        Frm_Main.COM4.Write(Command)
        COM4_Work.State = False
        COM4_Work.Result = True
        Com4_Send = True    '串口4发送命令成功
        Com4_TimmingWatch = GetTickCount()

    End Function

    'COM4口命令响应
    Public Function Com4_Return() As Integer
        Com4_Return = 2
        If COM4_Work.State = False Then
            If COM4_Work.Result Then
                Com4_Return = 0
            Else
                Com4_Return = 1
            End If
        ElseIf (GetTickCount() - Com4_TimmingWatch) >= 3000 Then
            Com4_Return = 1
        End If
    End Function

End Module
