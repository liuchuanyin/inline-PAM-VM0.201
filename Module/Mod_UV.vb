Module Mod_UV

    ''' <summary>
    ''' UV灯连接状态标志位
    ''' </summary>
    ''' <remarks></remarks>
    Public Flag_UVConnect(7) As Boolean
    Public ControllerHandle(7) As Integer
    Public UVControllerIP(7) As String

    ''' <summary>
    ''' UV灯初始化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UV_Init()
        For i = 1 To Flag_UVConnect.Count - 1
            UVControllerIP(i) = "192.168.0.10" & i
            Flag_UVConnect(i) = False
            If UV_Connect(UVControllerIP(i), ControllerHandle(i)) Then
                Flag_UVConnect(1) = True
            End If
            If Flag_UVConnect(i) = False Then
                Frm_DialogAddMessage("UV Controller " & i & " Connect Failed!")
            Else
                UV_Close(ControllerHandle(i), 0)
            End If
        Next
    End Sub

    ''' <summary>
    ''' UV灯连接By IP Address
    ''' </summary>
    ''' <param name="IPaddr"></param>
    ''' <param name="controllerHandle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UV_Connect(ByVal IPaddr As String, ByRef controllerHandle As Integer) As Boolean
        Dim rtn As Integer
        rtn = OPTControllerAPI.OPTController_CreateEtheConnectionByIP(IPaddr, controllerHandle)
        If rtn = 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' OPEN UV Light
    ''' </summary>
    ''' <param name="controllerHandle">控制器</param>
    ''' <param name="Index">0打开所有通道</param>
    ''' <param name="indenstiy">亮度</param>
    ''' <remarks></remarks>
    Public Sub UV_Open(ByVal controllerHandle As Integer, Index As Integer, indenstiy As Integer)
        OPTControllerAPI.OPTController_TurnOnChannel(controllerHandle, 0)
        OPTControllerAPI.OPTController_SetIntensity(controllerHandle, 0, indenstiy)
    End Sub

    ''' <summary>
    ''' 读取UV灯的亮度值
    ''' </summary>
    ''' <param name="controllerHandle"></param>
    ''' <param name="Index"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getUVIndenstiy(ByVal controllerHandle As Integer, Index As Integer) As Integer
        Dim rtn As Integer
        OPTControllerAPI.OPTController_ReadIntensity(controllerHandle, Index, rtn)
        Return rtn
    End Function

    ''' <summary>
    ''' CLOSE UV Light
    ''' </summary>
    ''' <param name="controllerHandle"></param>
    ''' <param name="Index"></param>
    ''' <remarks></remarks>
    Public Sub UV_Close(ByVal controllerHandle As Integer, Index As Integer)
        'OPTControllerAPI.OPTController_TurnOffChannel(controllerHandle, 0)
        OPTControllerAPI.OPTController_SetIntensity(controllerHandle, Index, 0)
    End Sub

    ''' <summary>
    ''' DisConnect UV Controller
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UV_DisConnect()
        For i = 1 To 4
            If Flag_UVConnect(i) Then
                OPTControllerAPI.OPTController_DestoryEtheConnection(ControllerHandle(i))
            End If
        Next
    End Sub

    Public Sub UV_CheckCon()

    End Sub

End Module
