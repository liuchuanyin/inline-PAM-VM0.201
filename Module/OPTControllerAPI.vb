

Module OPTControllerAPI

    Public Const OPT_SUCCEED As Integer = 0                                  'Operation succeed
    Public Const OPT_ERR_INVALIDHANDLE As Integer = 3001001                  'Invalid handle
    Public Const OPT_ERR_UNKNOWN As Integer = 3001002                        'Error unknown 
    Public Const OPT_ERR_INITSERIAL_FAILED As Integer = 3001003              'Failed to initialize a serial port
    Public Const OPT_ERR_RELEASESERIALPORT_FAILED As Integer = 3001004       'Failed to release a serial port
    Public Const OPT_ERR_SERIALPORT_UNOPENED As Integer = 3001005            'Attempt to access an unopened serial port
    Public Const OPT_ERR_CREATEETHECON_FAILED As Integer = 3001006           'Failed to create an Ethernet connection
    Public Const OPT_ERR_DESTORYETHECON_FAILED As Integer = 3001007          'Failed to destroy an Ethernet connection
    Public Const OPT_ERR_SN_NOTFOUND As Integer = 3001008                    'SN is not found
    Public Const OPT_ERR_TURNONCH_FAILED As Integer = 3001009                'Failed to turn on the specified channel(s)
    Public Const OPT_ERR_TURNOFFCH_FAILED As Integer = 3001019               'Failed to turn off the specified channel(s)
    Public Const OPT_ERR_SET_INTENSITY_FAILED As Integer = 3001011           'Failed to set the intensity for the specified channel(s)
    Public Const OPT_ERR_READ_INTENSITY_FAILED As Integer = 3001012          'Failed to read the intensity for the specified channel(s)	
    Public Const OPT_ERR_SET_TRIGGERWIDTH_FAILED As Integer = 3001013        'Failed to set trigger pulse width	
    Public Const OPT_ERR_READ_TRIGGERWIDTH_FAILED As Integer = 3001014       'Failed to read trigger pulse width
    Public Const OPT_ERR_READ_HBTRIGGERWIDTH_FAILED As Integer = 3001015     'Failed to read high brightness trigger pulse width
    Public Const OPT_ERR_SET_HBTRIGGERWIDTH_FAILED As Integer = 3001016      'Failed to set high brightness trigger pulse width
    Public Const OPT_ERR_READ_SN_FAILED As Integer = 3001017                 'Failed to read serial number
    Public Const OPT_ERR_READ_IPCONFIG_FAILED As Integer = 3001018           'Failed to read IP address
    Public Const OPT_ERR_CHINDEX_OUTRANGE As Integer = 3001019               'Index(es) out of the range
    Public Const OPT_ERR_WRITE_FAILED As Integer = 3001020                   'Failed to write data
    Public Const OPT_ERR_PARAM_OUTRANGE As Integer = 3001021                 'Parameter(s) out of the range 

    Public Structure IntensityItem
        Dim channelIndex As Integer            'The channel index value of controller
        Dim intensity As Integer               'The intensity for the corresponding channel index 
    End Structure

    Public Structure TriggerWidthItem
        Dim channelIndex As Integer            'The channel index value of controller
        Dim triggerWidth As Integer            'The trigger width for the corresponding channel index 
    End Structure

    Public Structure HBTriggerWidthItem
        Dim channelIndex As Integer            'The channel index value of controller
        Dim HBTriggerWidth As Integer          'The high brightness trigger width for the corresponding channel index 
    End Structure

    'Declare the functions
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_InitSerialPort(ByVal comName As String, ByRef controllerHandle As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReleaseSerialPort(ByVal controllerHandle As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_CreateEtheConnectionByIP(ByVal serverIPAddress As String, ByRef controllerHandle As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_CreateEtheConnectionBySN(ByVal serialNumber As String, ByRef controllerHandle As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_DestoryEtheConnection(ByVal controllerHandle As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_TurnOnChannel(ByVal controllerHandle As Integer, ByVal channelIndex As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_TurnOnMultiChannel(ByVal controllerHandle As Integer, ByRef channelIndexArray As Integer, ByVal length As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_TurnOffChannel(ByVal controllerHandle As Integer, ByVal channelIndex As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_TurnOffMultiChannel(ByVal controllerHandle As Integer, ByRef channelIndexArray As Integer, ByVal length As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_SetIntensity(ByVal controllerHandle As Integer, ByVal channelIndex As Integer, ByVal intensity As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_SetMultiIntensity(ByVal controllerHandle As Integer, ByRef intensityArray As IntensityItem, ByVal arrayLength As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReadIntensity(ByVal controllerHandle As Integer, ByVal channelIndex As Integer, ByRef intensity As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_SetTriggerWidth(ByVal controllerHandle As Integer, ByVal channelIndex As Integer, ByVal triggerWidth As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_SetMultiTriggerWidth(ByVal controllerHandle As Integer, ByRef triggerWidthArray As TriggerWidthItem, ByVal arrayLength As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReadTriggerWidth(ByVal controllerHandle As Integer, ByVal channelIndex As Integer, ByRef triggerWidth As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_SetHBTriggerWidth(ByVal controllerHandle As Integer, ByVal channelIndex As Integer, ByVal HBTriggerWidth As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_SetMultiHBTriggerWidth(ByVal controllerHandle As Integer, ByRef HBtriggerWidthArray As HBTriggerWidthItem, ByVal arrayLength As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReadHBTriggerWidth(ByVal controllerHandle As Integer, ByVal channelIndex As Integer, ByRef HBTriggerWidth As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_EnableResponse(ByVal controllerHandle As Integer, ByVal isResponse As Boolean) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_EnableCheckSum(ByVal controllerHandle As Integer, ByVal enable As Boolean) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_EnablePowerOffBackup(ByVal controllerHandle As Integer, ByVal isSave As Boolean) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReadSN(ByVal controllerHandle As Integer, ByRef SN As String) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReadIPConfig(ByVal controllerHandle As Integer, ByRef IP As String, ByRef subnetMask As String, ByRef defaultGateway As String) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_IsConnect(ByVal controllerHandle As Integer) As Integer
    End Function
    <Runtime.InteropServices.DllImport("OPTController.dll", CallingConvention:=Runtime.InteropServices.CallingConvention.Cdecl)>
    Public Function OPTController_ReadChannelState(ByVal controllerHandle As Integer, ByVal channelIndex As Integer) As Integer
    End Function
End Module
