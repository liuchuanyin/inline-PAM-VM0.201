Imports System.Math
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Frm_Main
    Private ActiveFrmName As String '活动窗体的名称

#Region "功能：窗体加载"
    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        '窗体加载时机器停止选中，模式选择按钮选中，颜色提提
        Btn_Mode.BZ_Color = Color_Selected 'Button 选中，颜色提示
        Btn_Stop.BZ_Color = Color_Red
        Time_SWOpened = Now

        '创建必要的文件夹
        Call FileInit()
        Call UserCodeInit()
        Call Read_MacType(Path_MacType, MACTYPE)
        '读取本地保存的参数设置
        Call Read_par(Path_Par, par)
        '读取点胶参数
        Call Read_GluePar(Path_Par_Glue, Par_Glue)
        '良率统计信息初始化
        Call Yield_Data_Init()
        '加载回原点需要的参数
        Call HomeValue()
        '运动控制卡初始化
        Call MotionCardInit()
        '串口初始化
        Call SerialPortInit()
        '网络初始化
        Call WinSock_Init()

        Call SetMachine(0)

        'Call OpenShellExecute("D:\Cognex-Run\COGNEX\", "AlignVisSystem.exe")   '开启CCD程序

        '//读取本地保存的轴参数
        Call DatabaseRead("D:\BZ-Parameter\ParameterMdb\BZ001.mdb", "AxisPar")
        '//读取本地保存的点位信息
        Call read_Par_Pos(Path_Par_Pos, Par_Pos)
        ''//获取扫条码位置和取料位置之间X，Y的距离
        'Call Get_GS()
        ''//获取镭射和针头位置之间X，Y的距离
        'Call Get_NL()
        'Call Get_NLS4()
        'UV Light Ini
        Call UV_Init()

        '实例化结构体
        For i = 0 To Tray_Pallet.Count - 1
            Tray_Pallet(i).init()
        Next

        If GTS_Opened_EX = True And GTS_Opened_EM = True Then
            Thread_IORefresh.Start()
        End If
        Timer_Sys.Enabled = True

        '在主界面上显示设备名称及编号
        Btn_MachineInfo.Text = par.Machine_Info.AE_SubID & "-" & par.Machine_Info.Machine_SN

        If IO.File.Exists(Path_IniFile) = False Then '判断INI文件是否存在
            Call CreateIniFile(Path_IniFile)  '不存在则创建默认的INI文件
        End If
    End Sub
#End Region

#Region "功能：窗体卸载"
    Private Sub Frm_Main_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed

        '刷新IO线程关闭
        Thread_IORefresh.Abort()


        '*********************  统计软件使用时间  **********************
        Dim temp As Long
        Time_SWClosed = Now
        temp = DateDiff(DateInterval.Minute, Time_SWOpened, Time_SWClosed)
        temp += Val(GetINI("机台信息", "总工作时间", "", Path_IniFile))
        Call WriteINI("机台信息", "总工作时间", temp, Path_IniFile)

    End Sub
#End Region

#Region "功能：界面操作"

    Private Sub Btn_MouseDown(sender As Object, e As EventArgs) Handles Btn_ModeEngineer.MouseDown, Btn_Alarm.MouseDown, _
        Btn_CCD.MouseDown, Btn_Chart.MouseDown, Btn_CPKGRR.MouseDown, Btn_Exit.MouseDown, Btn_Home.MouseDown, _
        Btn_Login.MouseDown, Btn_MachineInfo.MouseDown, Btn_Mode.MouseDown, Btn_ModeEngineer.MouseDown, Btn_ModeProduction.MouseDown, _
         Btn_Settings.MouseDown

        '调用执行方法
        Me.BackColor = Color.White
        LoadBtnMouseDown(sender)
    End Sub

    '设备控制操作，启动，暂停和停止
    Private Sub Btn_MachineAction(sender As Object, e As EventArgs) Handles Btn_Pause.MouseDown, Btn_Start.MouseDown, Btn_Stop.MouseDown
        Load_MachineAction(sender)
    End Sub

    '打开文件夹操作
    Private Sub Btn_OpenFiles(sender As Object, e As EventArgs) Handles Btn_OpenExcelData.MouseDown, Btn_OpenImageFiles.MouseDown
        Load_Files(sender)
    End Sub

    Private Function IsNotShow(frmName As String)
        If Application.OpenForms.Item(frmName) Is Nothing Then
            IsNotShow = True
        Else
            IsNotShow = False
        End If

        ActiveFrmName = frmName
        Timer_UI.Enabled = True
    End Function

    'UI控制定时器
    Private Sub Timer_UIControl_Tick(sender As Object, e As EventArgs) Handles Timer_UI.Tick
        Dim TimerUIControlStartFlag As Boolean
        If TimerUIControlStartFlag = False Then
            TimerUIControlStartFlag = True
            For Each Fr As Form In Application.OpenForms
                If Fr.Name <> ActiveFrmName And Fr.Name <> "Frm_Main" And Fr.Name <> "Frm_Login" And Fr.Name <> "Frm_Dialog" Then
                    If Fr.Name <> "Frm_Engineering" Then
                        Fr.Close()
                        Fr.Dispose()
                        Exit For
                    Else
                        Me.BackColor = Color.White  '如果退出工程界面，Mode_select 界面背景为白色
                        Fr.Visible = False
                    End If
                End If
            Next
            Timer_UI.Enabled = False
            TimerUIControlStartFlag = False
        End If
    End Sub

    '界面按钮操作
    Private Sub LoadBtnMouseDown(ByVal sender As Button)

        '设置所有主界面的Button背景颜色未选中
        Btn_Home.BZ_Color = Color_Unselected
        Btn_Settings.BZ_Color = Color_Unselected
        Btn_CCD.BZ_Color = Color_Unselected
        Btn_Chart.BZ_Color = Color_Unselected
        Btn_Alarm.BZ_Color = Color_Unselected
        Btn_MachineInfo.BZ_Color = Color_Unselected
        Btn_Mode.BZ_Color = Color_Unselected
        Btn_ModeProduction.BZ_Color = Color_Unselected
        Btn_ModeEngineer.BZ_Color = Color_Unselected
        Btn_CPKGRR.BZ_Color = Color_Unselected

        Select Case sender.Name
            Case "Btn_ModeProduction", "Btn_Home"
                Btn_Home.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_Home") Then Frm_Home.Show(Me)
                If sender.Name = "Btn_ModeProduction" Then
                    Btn_ModeProduction.BZ_Color = Color.FromArgb(200, 200, 200)
                    Btn_ModeEngineer.BZ_Color = Color.FromArgb(200, 200, 200)
                    Btn_CPKGRR.BZ_Color = Color.FromArgb(200, 200, 200)
                End If
            Case "Btn_Settings"
                Btn_Settings.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_Settings") Then Frm_Settings.Show(Me)
            Case "Btn_CCD"
                Btn_CCD.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_CCDSetting") Then Frm_CCDSetting.Show(Me)
            Case "Btn_Chart"
                Btn_Chart.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_Chart") Then Frm_Chart.Show(Me)
            Case "Btn_Mode"
                Btn_Mode.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_Main") Then
                End If
            Case "Btn_Alarm"
                Btn_Alarm.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_Alarm") Then Frm_Alarm.Show(Me)
            Case "Btn_MachineInfo"
                Btn_MachineInfo.BZ_Color = Color_Selected 'Button 选中，颜色提示
                If IsNotShow("Frm_MachineInfo") Then Frm_MachineInfo.Show(Me)
            Case "Btn_Login"
                '进入工程师界面
                If ComboBox_User.SelectedIndex = 1 And BOZHON.User2.Code = txt_Password.Text Then
                    Btn_Mode.BZ_Color = Color_Selected 'Button 选中，颜色提示
                    Me.BackColor = Color.FromArgb(252, 223, 222) '登陆工程界面，显示红色
                    Frm_Engineering.Show(Me)
                    Frm_Engineering.Visible = True
                    txt_Password.Text = ""
                    '登陆成功后，关闭使能
                    Login_Engineering_Enable(False)
                    Btn_Login.BZ_Color = Color_Unselected
                Else
                    MessageBox.Show("Please Use the Correct User Name and Enter the Password!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

            Case "Btn_ModeEngineer"
                '登陆到工程模式
                Login_Engineering_Enable(True)
                '提示登陆到工程师界面
                Btn_ModeEngineer.BZ_Color = Color.FromArgb(179, 202, 255)
                ComboBox_User.SelectedIndex = 1
                txt_Password.Focus()
                Btn_Login.BZ_Color = Color.FromArgb(253, 253, 191)
                Me.BackColor = Color.FromArgb(252, 223, 222)

        End Select
    End Sub

    '界面暂停，继续和停止操作
    Private Sub Load_MachineAction(ByVal sender As Button)
        Select Case sender.Name
            Case "Btn_Start"
                If Flag_MachinePause Then
                    Call Machine_Continue()
                    SetMachine(1)
                End If
                If Flag_MachineStop Then
                    Call Machine_Start()
                End If

            Case "Btn_Pause"
                If Flag_MachineAutoRun Then
                    Call Machine_Pause()
                End If
            Case "Btn_Stop"
                If Flag_MachineAutoRun Or Flag_MachinePause Then
                    Call Machine_Stop()
                End If
        End Select
    End Sub

    '打开文件夹操作
    Private Sub Load_Files(ByVal sender As Button)
        Select Case sender.Name
            Case "Btn_OpenExcelData"
                '打开数据文件夹
                Shell("explorer E:\BZ-Data\Data", vbNormalFocus)
            Case "Btn_OpenImageFiles"
                '打开图像文件夹
                Shell("explorer E:\BZ-Data\Images", vbNormalFocus)
        End Select
    End Sub

    '退出程序按钮
    Private Sub Btn_Exit_Click(sender As Object, e As EventArgs) Handles Btn_Exit.Click
        Dim rtn As Short

        If Flag_MachineAutoRun Then
            Frm_DialogAddMessage("设备正在自动运行中，禁止退出程序")
            Exit Sub
        End If
        If Flag_MachinePause Then
            Frm_DialogAddMessage("设备暂停中，禁止退出程序")
            Exit Sub
        End If

        '//轴伺服OFF
        For n = 0 To GTS_CardNum - 1
            For i = 1 To GTS_AxisNum(n)
                rtn = GT_ClrSts(n, i, 1)  '清除当前轴报警标志
                rtn = GT_AxisOff(n, i) '当前轴伺服OFF
            Next i
        Next n

        '断开UV灯连接
        Call UV_DisConnect()

        SetEXO(0, 0, False) '关闭启动按钮指示灯
        SetEXO(0, 1, False) '关闭1工位真空吸
        SetEXO(0, 2, False) '关闭2工位真空吸
        SetEXO(0, 3, False) '关闭3工位真空吸
        SetEXO(0, 4, False) '关闭4工位真空吸
        SetEXO(0, 5, False) '关闭5工位真空吸
        SetEXO(0, 6, False)  '关闭2工位点胶
        SetEXO(0, 7, False) '关闭3工位点胶
        SetEXO(0, 8, False) '关闭4工位取料吸嘴真空吸
        SetEXO(0, 9, False) '关闭蜂鸣器
        SetEXO(0, 10, False) '关闭OK指示灯
        SetEXO(0, 11, False) '关闭NG指示灯

        SetEXO(1, 2, False) '三色灯红
        SetEXO(1, 3, False) '三色灯黄
        SetEXO(1, 4, False) '三色灯绿
        SetEXO(1, 5, False) '三色灯蜂鸣器
        SetEXO(1, 6, False) '日光灯控制
        SetEXO(1, 7, False) '真空泵控制
        SetEXO(1, 8, False) '2工位点胶气缸电磁阀
        SetEXO(1, 9, False) '3工位点胶气缸电磁阀
        SetEXO(1, 10, False) '4工位光源气缸电磁阀

        SetEXO(1, 15, False) 'UV灯升降气缸缩回


        SetEMO(0, 0, False) '关闭料盘步进使能
        SetEMO(1, 4, False) '关闭2工位步进使能
        SetEMO(1, 6, False) '关闭4工位步进使能

        Timer_Sys.Enabled = False
        Call SetPressure(0, 0)
        Call SetPressure(1, 0)
        'Delay(3000)
        GT_CloseExtMdlGts(0)
        GT_CloseExtMdlGts(1)
        GT_Close(0)
        GT_Close(1)
        Me.Close()
        Me.Dispose()
    End Sub

    '自动填充密码
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        txt_Password.Text = BOZHON.User2.Code
    End Sub
#End Region

#Region "功能：读取用户账户名和密码"
    ''' <summary>
    ''' 读取用户账户名和密码
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UserCodeInit()
        '读取密码保存文件
        Read_Code(Path_UserCode, BOZHON)
        'OP
        ComboBox_User.Items.Add(BOZHON.User1.Name)
        'BOTECH
        ComboBox_User.Items.Add(BOZHON.User2.Name)
        'SW Engineering
        ComboBox_User.Items.Add(BOZHON.User3.Name)
        '窗体加载时登陆到工程界面未使能
        Login_Engineering_Enable(False)
    End Sub
    ''' <summary>
    ''' 登陆工程界面是否打开
    ''' </summary>
    ''' <param name="enable">True：打开，False关闭</param>
    ''' <remarks></remarks>
    Private Sub Login_Engineering_Enable(ByVal enable As Boolean)
        ComboBox_User.Enabled = enable
        Btn_Login.Enabled = enable
        txt_Password.Enabled = enable
    End Sub
#End Region

#Region "功能：串口"
    ''' <summary>
    ''' Loadcell 1
    ''' </summary>
    ''' <param name="port"></param>
    ''' <remarks></remarks>
    Public Sub COM1_Init(ByVal port As Short)
        On Error Resume Next
        COM1.PortName = "COM" & port       '设置端口号
        COM1.BaudRate = 115200
        COM1.DataBits = 8
        COM1.StopBits = IO.Ports.StopBits.One
        COM1.Parity = IO.Ports.Parity.None
        On Error GoTo Com_Err                                     '激活错误检测机制
        COM1.Open()                               '打开串行通信口
        Delay(50)
        Com1_Send(":O000000o" & vbCrLf)
        Flag_COMOpened(1) = True
        Exit Sub
Com_Err:
        Frm_DialogAddMessage("Loadcell 1 串口" & COM1.PortName.ToString & "打开失败，请检查！")
        Flag_COMOpened(1) = False
        Exit Sub
        Select Case Err.Number
            Case 8000
                MsgBox("串口" & COM1.PortName.ToString & "端口打开时操作不合法", vbCritical + vbOKOnly)
            Case 8002
                MsgBox("串口" & COM1.PortName.ToString & "无效端口号", vbCritical + vbOKOnly)
            Case 8005
                MsgBox("串口" & COM1.PortName.ToString & "端口已经打开", vbCritical + vbOKOnly)
            Case 8007
                MsgBox("串口" & COM1.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8010
                MsgBox("串口" & COM1.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8015
                MsgBox("串口" & COM1.PortName.ToString & "有一个或多个无效的通信参数", vbCritical + vbOKOnly)
            Case Else
                MsgBox("串口" & COM1.PortName.ToString & "打开通信端口时发生未知错误", vbCritical + vbOKOnly)
        End Select
    End Sub

    ''' <summary>
    ''' Laser
    ''' </summary>
    ''' <param name="port"></param>
    ''' <remarks></remarks>
    Public Sub COM2_Init(ByVal port As Short)
        On Error Resume Next
        COM2.PortName = "COM" & port       '设置端口号
        COM2.BaudRate = 9600
        COM2.DataBits = 8
        COM2.StopBits = IO.Ports.StopBits.One
        COM2.Parity = IO.Ports.Parity.None
        On Error GoTo Com_Err                                     '激活错误检测机制
        COM2.Open()                               '打开串行通信口
        Flag_COMOpened(2) = True
        Exit Sub
Com_Err:
        Frm_DialogAddMessage("镭射串口" & COM2.PortName.ToString & "打开失败，请检查！")
        Flag_COMOpened(2) = False
        Exit Sub
        Select Case Err.Number
            Case 8000
                MsgBox("串口" & COM2.PortName.ToString & "端口打开时操作不合法", vbCritical + vbOKOnly)
            Case 8002
                MsgBox("串口" & COM2.PortName.ToString & "无效端口号", vbCritical + vbOKOnly)
            Case 8005
                MsgBox("串口" & COM2.PortName.ToString & "端口已经打开", vbCritical + vbOKOnly)
            Case 8007
                MsgBox("串口" & COM2.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8010
                MsgBox("串口" & COM2.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8015
                MsgBox("串口" & COM2.PortName.ToString & "有一个或多个无效的通信参数", vbCritical + vbOKOnly)
            Case Else
                MsgBox("串口" & COM2.PortName.ToString & "打开通信端口时发生未知错误", vbCritical + vbOKOnly)
        End Select
    End Sub

    ''' <summary>
    ''' Loadcell 2
    ''' </summary>
    ''' <param name="port"></param>
    ''' <remarks></remarks>
    Public Sub COM3_Init(ByVal port As Short)
        On Error Resume Next
        COM3.PortName = "COM" & port       '设置端口号
        COM3.BaudRate = 115200
        COM3.DataBits = 8
        COM3.StopBits = IO.Ports.StopBits.One
        COM3.Parity = IO.Ports.Parity.None
        On Error GoTo Com_Err                                     '激活错误检测机制
        COM3.Open()                               '打开串行通信口
        Flag_COMOpened(3) = True
        Exit Sub
Com_Err:
        Frm_DialogAddMessage("Loadcell 2 串口" & COM3.PortName.ToString & "打开失败，请检查！")
        Flag_COMOpened(3) = False
        Exit Sub
        Select Case Err.Number
            Case 8000
                MsgBox("串口" & COM3.PortName.ToString & "端口打开时操作不合法", vbCritical + vbOKOnly)
            Case 8002
                MsgBox("串口" & COM3.PortName.ToString & "无效端口号", vbCritical + vbOKOnly)
            Case 8005
                MsgBox("串口" & COM3.PortName.ToString & "端口已经打开", vbCritical + vbOKOnly)
            Case 8007
                MsgBox("串口" & COM3.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8010
                MsgBox("串口" & COM3.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8015
                MsgBox("串口" & COM3.PortName.ToString & "有一个或多个无效的通信参数", vbCritical + vbOKOnly)
            Case Else
                MsgBox("串口" & COM3.PortName.ToString & "打开通信端口时发生未知错误", vbCritical + vbOKOnly)
        End Select
    End Sub

    ''' <summary>
    ''' 标准压力传感器串口初始化
    ''' </summary>
    ''' <param name="port"></param>
    ''' <remarks></remarks>
    Public Sub COM4_Init(ByVal port As Short)
        On Error Resume Next
        COM4.PortName = "COM" & port       '设置端口号
        COM4.BaudRate = 115200
        COM4.DataBits = 8
        COM4.StopBits = IO.Ports.StopBits.One
        COM4.Parity = IO.Ports.Parity.None
        On Error GoTo Com_Err                                     '激活错误检测机制
        COM4.Open()                               '打开串行通信口
        Delay(50)
        Com4_Send(":O000000o" & vbCrLf)
        Flag_COMOpened(4) = True
        Exit Sub
Com_Err:
        Frm_DialogAddMessage("Loadcell 3 串口" & COM4.PortName.ToString & "打开失败，请检查！")
        Flag_COMOpened(4) = False
        Exit Sub
        Select Case Err.Number
            Case 8000
                MsgBox("串口" & COM4.PortName.ToString & "端口打开时操作不合法", vbCritical + vbOKOnly)
            Case 8002
                MsgBox("串口" & COM4.PortName.ToString & "无效端口号", vbCritical + vbOKOnly)
            Case 8005
                MsgBox("串口" & COM4.PortName.ToString & "端口已经打开", vbCritical + vbOKOnly)
            Case 8007
                MsgBox("串口" & COM4.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8010
                MsgBox("串口" & COM4.PortName.ToString & "不支持设备的波特率", vbCritical + vbOKOnly)
            Case 8015
                MsgBox("串口" & COM4.PortName.ToString & "有一个或多个无效的通信参数", vbCritical + vbOKOnly)
            Case Else
                MsgBox("串口" & COM4.PortName.ToString & "打开通信端口时发生未知错误", vbCritical + vbOKOnly)
        End Select
    End Sub

    Public Sub SerialPortInit()
        'LoadCell 1
        COM1_Init(par.CCD(6))
        'Laser
        COM2_Init(par.CCD(5))
        'LoadCell 2
        COM3_Init(par.CCD(7))
        'LoadCell 3
        COM4_Init(par.CCD(8))
    End Sub

    'COM1接收数据
    Private Sub COM1_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles COM1.DataReceived
        Static TempCom As String
        Dim TempStr As String = ""
        Dim startNum As Long, EndNum As Long
        'Dim rtn As Short
        Dim i As Integer
        Dim LoadCell As Double

        Try
            TempStr = COM1.ReadExisting.ToString
            If InStr(1, TempCom, vbCr, 0) <> 0 And Len(TempCom) > 4 Then
                TempCom = TempCom & TempStr
                If Len(TempCom) - 10 > 0 Then
                    For i = Len(TempCom) To Len(TempCom) - 10 Step -1
                        If Mid(TempCom, i, 1) = vbCr Then
                            EndNum = i
                        End If
                        If Mid(TempCom, i, 1) = "V" And i < EndNum Then
                            startNum = i
                        End If
                        If startNum * EndNum <> 0 Then
                            COM1_Work.State = False
                            COM1_Work.Result = True
                            LoadCell = Val(Mid(TempCom, startNum + 1, EndNum - startNum - 1))
                            Press(0) = Format(LoadCell / 100, "0.000")
                            'If S3_AutoRun_Step = 500 Then
                            '    If Press(0) > par.num(19) Then
                            '        rtn = GT_Stop(0, 2 ^ (S3_Z - 1), 2 ^ (S3_Z - 1)) '紧急停止Z 轴
                            '        ListBoxAddMessage("3工位贴合压力：" & Press(0))
                            '        'Com1_Send(":Q000000q" & vbCrLf) ' 串口接收数据关闭
                            '    End If
                            'End If
                            Exit For
                        End If
                    Next i
                    TempCom = ""
                End If
            Else
                TempCom = TempCom & TempStr
            End If
        Catch ex As Exception
            Call Write_Log(4, "获取压力传感器数据异常！" & ex.Message, Path_Log)
        End Try
    End Sub

    'COM2接收数据
    Private Sub COM2_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles COM2.DataReceived
        Static TempCom As String
        Dim TempStr As String
        Dim i As Long
        TempStr = COM2.ReadExisting.ToString

        If Strings.Right(TempStr, 1) <> vbCr Then
            TempCom = TempCom & TempStr
        Else
            TempCom = TempCom & TempStr '将字符串组合
            'COM2_String = TempCom       '将字符串存储到全局变量
            TempCom = Trim(TempCom)     '删除前导和尾随空格
            TempCom = Replace(TempCom, vbCr, "")    '删除未尾的回车符
            COM2_String = TempCom

            If Strings.Left(COM2_String, 2) = "M1" Then     '判断字符串头是否为镭射命令
                TempStr = Mid(TempCom, 4, Abs(Len(TempCom) - 3)) '删除前3个字符(命令字头)
                COM2_sData = Split(TempStr, ",")    '以逗号分隔截取各段字符数据
                'ReDim Com2_Data(UBound(Com2_sData))
                For i = 0 To UBound(COM2_sData)
                    COM2_Data(i) = Val(COM2_sData(i))  '将字符转换为数值
                Next i
                COM2_Work.State = False
                COM2_Work.Result = True     'COM2接收到正确的数据
                TempCom = ""
            End If
            If Strings.Left(COM2_String, 2) = "ER" Then     '判断字符串头是否为镭射命令(返回错误值)
                TempStr = Mid(TempCom, 4, Len(TempCom) - 3) '删除前3个字符(命令字头)
                COM2_sData = Split(TempStr, ",")    '以逗号分隔截取各段字符数据
                'ReDim Com2_Data(UBound(Com2_sData))
                For i = 0 To UBound(COM2_sData)
                    COM2_Data(i) = Val(COM2_sData(i))  '将字符转换为数值
                Next i
                COM2_Work.State = False
                COM2_Work.Result = False    'COM2接收到错误的数据
                TempCom = ""
                TempStr = COM2.ReadExisting.ToString
            End If
        End If
    End Sub

    'COM3接收数据
    Private Sub COM3_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles COM3.DataReceived
        Static TempCom As String
        Dim TempStr As String = ""
        Dim startNum As Long, EndNum As Long
        'Dim rtn As Short
        Dim i As Integer
        Dim LoadCell As Double

        Try
            TempStr = COM3.ReadExisting.ToString
            If InStr(1, TempCom, vbCr, 0) <> 0 And Len(TempCom) > 4 Then
                TempCom = TempCom & TempStr
                If Len(TempCom) - 10 > 0 Then
                    For i = Len(TempCom) To Len(TempCom) - 10 Step -1
                        If Mid(TempCom, i, 1) = vbCr Then
                            EndNum = i
                        End If
                        If Mid(TempCom, i, 1) = "V" And i < EndNum Then
                            startNum = i
                        End If
                        If startNum * EndNum <> 0 Then
                            COM1_Work.State = False
                            COM1_Work.Result = True
                            LoadCell = Val(Mid(TempCom, startNum + 1, EndNum - startNum - 1))
                            Press(1) = Format(LoadCell / 100, "0.000")
                            'If S3_AutoRun_Step = 500 Then
                            '    If Press(0) > par.num(19) Then
                            '        rtn = GT_Stop(0, 2 ^ (S3_Z - 1), 2 ^ (S3_Z - 1)) '紧急停止Z 轴
                            '        ListBoxAddMessage("3工位贴合压力：" & Press(0))
                            '        'Com1_Send(":Q000000q" & vbCrLf) ' 串口接收数据关闭
                            '    End If
                            'End If
                            Exit For
                        End If
                    Next i
                    TempCom = ""
                End If
            Else
                TempCom = TempCom & TempStr
            End If
        Catch ex As Exception
            Call Write_Log("获取压力传感器数据异常！" & ex.Message, Path_Log)
        End Try
    End Sub

    'COM4接收数据
    Private Sub COM_PressCa_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles COM4.DataReceived
        Static TempCom As String
        Dim TempStr As String = ""
        Dim startNum As Long, EndNum As Long
        Dim i As Integer
        Dim LoadCell As Double

        Try
            TempStr = COM4.ReadExisting.ToString
            If InStr(1, TempCom, vbCr, 0) <> 0 And Len(TempCom) > 4 Then
                TempCom = TempCom & TempStr
                If Len(TempCom) - 10 > 0 Then
                    For i = Len(TempCom) To Len(TempCom) - 10 Step -1
                        If Mid(TempCom, i, 1) = vbCr Then
                            EndNum = i
                        End If
                        If Mid(TempCom, i, 1) = "V" And i < EndNum Then
                            startNum = i
                        End If
                        If startNum * EndNum <> 0 Then
                            COM4_Work.State = False
                            COM4_Work.Result = True
                            LoadCell = Val(Mid(TempCom, startNum + 1, EndNum - startNum - 1))
                            Press(2) = Format(LoadCell / 100, "0.000")
                            Exit For
                        End If
                    Next i
                    TempCom = ""
                End If
            Else
                TempCom = TempCom & TempStr
            End If
        Catch ex As Exception
            Call Write_Log("获取标准压力表压力值异常！" & ex.Message, Path_Log)
        End Try
    End Sub

#End Region

#Region "功能：网络 "
    Private Sub WinSock_Init()

        Call WinSock1_Connect()
        Call WinSock2_Connect()
        Call WinSock3_Connect()

    End Sub

    ''' <summary>
    ''' 检查网络连接状态
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub WinSock_Check()

        '///////////CCD ////////////////
        If Winsock1.CtlState = 7 Then    '判断网络连接是否成功
            Winsock1State = True
        Else
            Winsock1State = False
            Call WinSock1_Connect()   '不成功则重新去连接
        End If

        '///////////Barcode Scanner St 1 ////////////////
        If Winsock2.CtlState = 7 Then    '判断网络连接是否成功
            Winsock2State = True
        Else
            Winsock2State = False
            Call WinSock2_Connect()   '不成功则重新去连接
        End If

        '///////////Barcode Scanner ST 3 ////////////////
        If Winsock3.CtlState = 7 Then    '判断网络连接是否成功
            Winsock3State = True
        Else
            Winsock3State = False
            Call WinSock3_Connect()   '不成功则重新去连接
        End If


    End Sub

    ''' <summary>
    ''' WinSock连接
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub WinSock1_Connect()
        If par.CCD(9) <> "" And par.CCD(24) <> "" Then
            Winsock1.Close()
            Winsock1.RemoteHost = par.CCD(9)
            Winsock1.RemotePort = par.CCD(24)
            Winsock1.Connect()
        End If
    End Sub

    Private Sub Winsock1_DataArrival(sender As Object, e As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles Winsock1.DataArrival
        Static EthernetBufferStr As String  '以太网数据读取缓存字符
        Dim EthernetData As String
        'Dim TempStr As String
        'Dim i As Long

        EthernetData = ""
        Winsock1_String = ""
        Winsock1.GetData(EthernetData)                              '获取CCD字符串数据
        EthernetBufferStr = EthernetBufferStr & EthernetData
        Winsock1_String = Trim(EthernetBufferStr)                  '获取原始数据
        Winsock1_String = Winsock1_String.Replace(vbCrLf, "")       '删除未尾的回车符
        Winsock1_String = Trim(Winsock1_String)

        If Winsock1_String <> "" Then
            CamData_Process(Winsock1_String)
        End If
        EthernetBufferStr = ""
    End Sub


    ''' <summary>
    ''' WinSock连接---S1 Barcode Scanner
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub WinSock2_Connect()
        If par.CCD(10) <> "" And par.CCD(25) <> "" Then
            Winsock2.Close()
            Winsock2.RemoteHost = par.CCD(10)
            Winsock2.RemotePort = par.CCD(25)
            Winsock2.Connect()
        End If
    End Sub

    ''' <summary>
    ''' St 1 Barcode Scanner Data Arrival
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Winsock2_DataArrival(sender As Object, e As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles Winsock2.DataArrival
        Static EthernetBufferStr As String  '以太网数据读取缓存字符
        Dim EthernetData As String
        'Dim TempStr As String
        'Dim i As Long

        EthernetData = ""
        Winsock2_String = ""
        Winsock2.GetData(EthernetData)                              '获取字符串数据
        EthernetBufferStr = EthernetBufferStr & EthernetData
        Winsock2_String = Trim(EthernetBufferStr)                  '获取原始数据
        Winsock2_String = Winsock2_String.Replace(vbCrLf, "")       '删除未尾的回车符
        Winsock2_String = Trim(Winsock2_String)

        If Winsock2_String <> "" Then
            Call BarData_Process(1, Winsock2_String)
        End If
        EthernetBufferStr = ""
    End Sub


    ''' <summary>
    ''' WinSock连接---S3 Barcode Scanner
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub WinSock3_Connect()
        If par.CCD(11) <> "" And par.CCD(26) <> "" Then
            Winsock3.Close()
            Winsock3.RemoteHost = par.CCD(11)
            Winsock3.RemotePort = par.CCD(26)
            Winsock3.Connect()
        End If
    End Sub

    ''' <summary>
    ''' St 3 Barcode Scanner Data Arrival
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Winsock3_DataArrival(sender As Object, e As AxMSWinsockLib.DMSWinsockControlEvents_DataArrivalEvent) Handles Winsock3.DataArrival
        Static EthernetBufferStr As String  '以太网数据读取缓存字符
        Dim EthernetData As String
        'Dim TempStr As String
        'Dim i As Long

        EthernetData = ""
        Winsock3_String = ""
        Winsock3.GetData(EthernetData)                              '获取字符串数据
        EthernetBufferStr = EthernetBufferStr & EthernetData
        Winsock3_String = Trim(EthernetBufferStr)                  '获取原始数据
        Winsock3_String = Winsock3_String.Replace(vbCrLf, "")       '删除未尾的回车符
        Winsock3_String = Trim(Winsock3_String)

        If Winsock3_String <> "" Then
            Call BarData_Process(2, Winsock3_String)
        End If
        EthernetBufferStr = ""
    End Sub


#End Region

#Region "设备启动、暂停和停止操作"
    Private Sub Machine_Start()

        '设备自动运行
        '设备初始化
        If Frm_Home.Visible = False Then
            MsgBox("请先进入生产模式，再单击此按钮完成设备初始化并自动运行！", MsgBoxStyle.OkOnly)
            Exit Sub
        End If

        '判断是否紧急停止中
        If IsSysEmcStop Then
            Frm_DialogAddMessage("紧急停止中，请先解除急停")
            Exit Sub
        End If
        '判断是否所有伺服ON
        For i = 0 To GTS_CardNum - 1
            For j = 1 To GTS_AxisNum(i)
                If ServoOn(i, j) = False Then
                    Frm_DialogAddMessage("请打开所有伺服使能！")
                    Exit Sub
                End If
            Next
        Next
        Call Machine_Init()
        'Do While Flag_MachineInitOngoing
        '    My.Application.DoEvents()
        'Loop

        'If Flag_MachineInit Then
        '    Call Machine_AutoRun()
        'Else
        '    SetMachine(0)
        'End If
    End Sub

    ''' <summary>
    ''' 暂停设备自动运行
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Machine_Pause()
        ' 
        Timer_AutoRun.Enabled = False
        SetMachine(2)
    End Sub

    ''' <summary>
    ''' 继续设备自动运行
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Machine_Continue()
        ' 
        Timer_AutoRun.Enabled = True
        SetMachine(1)
    End Sub

    ''' <summary>
    ''' 停止设备自动运行
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Machine_Stop()

        If Flag_MachineAutoRun = False Then
            ListBoxAddMessage("停止自动运行无效")
        End If

        ListBoxAddMessage("设备停止自动运行")
        CycelTime = 0
        CycelTimeEn = False
        SetMachine(0)
        If Flag_FrmEngineeringOpned Then
            Frm_Engineering.Btn_initialize.Text = "初始化"
            Frm_Engineering.Btn_initialize.Enabled = True
            Frm_Engineering.Btn_initialize.BZ_Color = Color_Unselected

            Frm_Engineering.Btn_AutoRun.Text = "自动运行"
            Frm_Engineering.Btn_AutoRun.BZ_Color = Color_Unselected
            Frm_Engineering.Btn_AutoRun.Enabled = True
        End If
        Timer_AutoRun.Enabled = False

    End Sub
#End Region

#Region "   Timer"

    ''' <summary>
    ''' 设备初始化定时器
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Timer_MacInit_Tick(sender As Object, e As EventArgs) Handles Timer_MacInit.Tick
        Dim en As Boolean
        If en Then Exit Sub
        en = True

        If Flag_MachineInitOngoing Then
            Call Machine_Initialize()
            Frm_ProgressBar.Show()
        End If

        '初始化完成，关闭进度条，提示初始化完成，初始化按钮颜色选中，并Disabled，关闭定时器
        If Flag_MachineInit Then
            Frm_ProgressBar.Close()
            Frm_ProgressBar.Dispose()
            ListBoxAddMessage("初始化完成")
            Frm_Engineering.Btn_initialize.Text = "初始化完成"
            Btn_Start.BZ_Color = Color_Unselected
            Btn_Pause.BZ_Color = Color_Unselected
            Btn_Stop.BZ_Color = Color_Selected

            Frm_Engineering.Btn_initialize.Enabled = False
            Frm_Engineering.Btn_initialize.BZ_Color = Color_Selected
            Timer_MacInit.Enabled = False
        End If

        '初始化失败，关闭进度条，提示初始化失败，初始化按钮颜色未选中，并Enabled，关闭定时器
        If Flag_MachineInit = False And Flag_MachineInitOngoing = False Then
            Frm_ProgressBar.Close()
            Frm_ProgressBar.Dispose()
            ListBoxAddMessage("初始化失败")
            Frm_Engineering.Btn_initialize.Text = "初始化"

            Btn_Start.BZ_Color = Color_Unselected
            Btn_Pause.BZ_Color = Color_Unselected
            Btn_Stop.BZ_Color = Color_Red

            Frm_Engineering.Btn_initialize.Enabled = True
            Frm_Engineering.Btn_initialize.BZ_Color = Color_Unselected
            Timer_MacInit.Enabled = False
        End If

        en = False
    End Sub

    ''' <summary>
    ''' Set the Machine State: state =0 Machine Stop; =1 Machine Auto Run; =2 Machine Pause
    ''' </summary>
    ''' <param name="state"></param>
    ''' <remarks></remarks>
    Public Sub SetMachine(ByVal state As Short)
        Select Case state
            Case 0
                Btn_Start.BZ_Color = Color_Unselected
                Btn_Pause.BZ_Color = Color_Unselected
                Btn_Stop.BZ_Color = Color_Red
                'Flag_MachineInit = False
                Flag_MachinePause = False
                Flag_MachineAutoRun = False
                Flag_MachineStop = True
            Case 1
                Btn_Start.BZ_Color = Color_Selected
                Btn_Pause.BZ_Color = Color_Unselected
                Btn_Stop.BZ_Color = Color_Unselected
                Flag_MachinePause = False
                Flag_MachineAutoRun = True
                Flag_MachineStop = False
            Case 2
                Btn_Start.BZ_Color = Color_Unselected
                Btn_Pause.BZ_Color = Color_Blue
                Btn_Stop.BZ_Color = Color_Unselected
                Flag_MachinePause = True
                Flag_MachineAutoRun = True
                Flag_MachineStop = False
        End Select

    End Sub

    ''' <summary>
    ''' 系统刷新定时器
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Timer_Sys_Tick(sender As Object, e As EventArgs) Handles Timer_Sys.Tick
        Dim en As Boolean
        Static CountT As Integer = 0

        If en Then
            Exit Sub
        End If
        en = True


        Call EMC_Stop()

        'OK NG指示灯
        Call Auto_ShowLight()
        '自动排胶
        'Call AutoPurge_S2(2, 2)

        '如果OP操作界面可见，那么刷新
        If Frm_Home.Visible Then Call Frm_Home.VariablesRefresh()

        '生产周期计时
        If CycelTimeEn Then
            CycelTime = CycelTime + 10
            'If S1_Work.State = False And S2_Work.State = False And S3_Work.State = False And S4_Work.State = False Then
            '    CT = Format(CycelTime / 1000, "0.00")
            '    CycelTimeEn = False
            'End If
        End If

        '每100ms检查一次
        If CountT = 10 Then
            Call WinSock_Check()

            '设置点胶工作气压
            If EXI(0, 8) Then
                If Abs(ReadPressure(0) - par.num(28)) > 0.01 Then
                    Call SetPressure(0, par.num(28))
                End If
                If Abs(ReadPressure(1) - par.num(29)) > 0.01 Then
                    Call SetPressure(1, par.num(29))
                End If
            Else
                Call SetPressure(0, 0)
                Call SetPressure(1, 0)
            End If

            CountT = 0
        End If
        CountT += 1


        If CCD_Lock_Flag And GetTickCount - Winsock1_TimmingWatch > 5000 Then
            CCD_Lock_Flag = False
        End If


        en = False
    End Sub

    '///////////////自动运行定时器
    Private Sub Timer_AutoRun_Tick(sender As Object, e As EventArgs) Handles Timer_AutoRun.Tick
        Dim en As Boolean
        If en Then
            Exit Sub
        End If
        en = True

        If Not Flag_MachineAutoRun Then '判断是否进入自动运行状态
            Exit Sub
        End If

        en = False
    End Sub



#End Region

    '量测OK/NG指示灯
    Private Sub Auto_ShowLight()
        Static TimingWatchBuz As Long
        Static timingwatchFlash As Long = GetTickCount
        Static buzen As Boolean = True
        'Static TimingWatchLight As Long

        If par.chkFn(4) = False And Flag_MachineAutoRun = True Then '判断是否开启演示装配
            If TotalResult = 1 Then     '判断最终检测结果
                Frm_Engineering.lbl_OKNG.Text = "OK"
                Frm_Engineering.lbl_OKNG.ForeColor = Color.Black
                SetEXO(0, 10, True) '亮绿色灯
                SetEXO(0, 11, False) '灭红色指示灯
                buzen = True
            ElseIf TotalResult = 2 Then
                Frm_Engineering.lbl_OKNG.Text = "NG"
                Frm_Engineering.lbl_OKNG.ForeColor = Color.Red
                SetEXO(0, 10, False) '灭绿色灯
                SetEXO(0, 11, True) '亮红色指示灯
                '///////蜂鸣器鸣叫
                If GetTickCount - TimingWatchBuz > 2000 Then
                    If buzen Then
                        SetEXO(0, 9, True)
                        buzen = False
                    End If
                    TimingWatchBuz = GetTickCount()
                ElseIf GetTickCount - TimingWatchBuz > 1000 Then
                    SetEXO(0, 9, False)
                End If
            ElseIf TotalResult = 0 Then
                Frm_Engineering.lbl_OKNG.Text = "--"
                Frm_Engineering.lbl_OKNG.ForeColor = Color.Black
                SetEXO(0, 11, False) '灭红色指示灯
                SetEXO(0, 10, False) '灭绿色灯
                SetEXO(0, 9, False) 'NG警示灯停止鸣叫
                buzen = True
            End If

        End If

        '//////////   三色灯工作   /////////////////////////
        If GetTickCount - timingwatchFlash > 2000 Then
            If Flag_MachineAutoRun And Flag_MachinePause = False Then
                SetEXO(1, 2, False)    '三色灯红色
                SetEXO(1, 3, False)    '三色灯黄色
                SetEXO(1, 4, True)    '三色灯绿色
            ElseIf Flag_MachinePause Then
                SetEXO(1, 2, False)    '三色灯红色
                SetEXO(1, 3, True)    '三色灯黄色
                SetEXO(1, 4, False)    '三色灯绿色
            Else
                SetEXO(1, 2, True)    '三色灯红色
                SetEXO(1, 3, False)    '三色灯黄色
                SetEXO(1, 4, False)    '三色灯绿色
            End If
            timingwatchFlash = GetTickCount
        ElseIf GetTickCount - timingwatchFlash > 1000 Then
            SetEXO(1, 2, False)    '三色灯红色
            SetEXO(1, 3, False)    '三色灯黄色
            'SetEXO(1, 4, False)    '三色灯绿色
        End If

    End Sub

    ''' <summary>
    ''' 打开外部应用程序，如果应用程序已打开，则将其显示
    ''' </summary>
    ''' <param name="path">路径名称，不包含应用文件名，如"D:\Cognex-Run\COGNEX\"</param>
    ''' <param name="AppName">应用程序名称，如"AlignVisSystem.exe"</param>
    ''' <remarks></remarks>
    Public Sub OpenShellExecute(path As String, AppName As String)
        Dim Soft_hwnd, Temp_hwnd As Long
        Dim strAppName(2) As String

        strAppName = Split(AppName, ".")
        Temp_hwnd = FindWindow(vbNullString, "Frm_Main")
        Soft_hwnd = FindWindow(vbNullString, strAppName(0))
        If Soft_hwnd > 0 Then
            BringWindowToTop(Soft_hwnd)                          '
        Else
            ShellExecute(Temp_hwnd, "Open", AppName, "", path, 4)
        End If
    End Sub

    Private Sub btn_test_Click(sender As Object, e As EventArgs) Handles btn_test.Click
        'Dim Command As String
        'If MessageBox.Show("第二工位标定　or 第四工位标定，Yes为二工位标定，No为四工位标定", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
        '    Command = "Cal1," & Par_Pos.St_Glue(5).X & "," & Par_Pos.St_Glue(5).Y & "," & Par_Pos.St_Glue(6).X & "," & Par_Pos.St_Glue(6).Y & "," & Par_Pos.St_Glue(7).X & "," & Par_Pos.St_Glue(7).Y & "," & Par_Pos.St_Glue(8).X & "," & Par_Pos.St_Glue(8).Y & vbCrLf
        '    Winsock1.SendData(Command)
        'Else
        '    Command = "Cal5," & Par_Pos.Pos_S4(5).X & "," & Par_Pos.Pos_S4(5).Y & "," & Par_Pos.Pos_S4(6).X & "," & Par_Pos.Pos_S4(6).Y & "," & Par_Pos.Pos_S4(7).X & "," & Par_Pos.Pos_S4(7).Y & "," & Par_Pos.Pos_S4(8).X & "," & Par_Pos.Pos_S4(8).Y & vbCrLf
        '    Winsock1.SendData(Command)
        'End If

        'Frm_Engineering.DataGridView1.Rows.Add(par.Machine_Info.AE_SubID & "-" & par.Machine_Info.Machine_SN, ST(5).num, Format(Now, "yyyyMMdd HH:mm:ss"), ST(5).BarCodeS3, "Kirin", "OK", _
        '      Cam6Data(0), Cam6Data(1), Cam6Data(2), _
        '      Cam6Data(3), ST(5).BarCodeS1, ST(5).Cnt_Pao, ST(5).Paste_Press, CT)

        'Frm_Engineering.DataGridView1.Rows(Frm_Engineering.DataGridView1.Rows.Count - 2).Cells(3).Value = "小丹"

        List_DebugAddMessage("axis servo on failed please check the servo driver")

        'Flag_HuanliaoOK = True


    End Sub

#Region "   功能：更改设备工作模式"
    Private Sub lbl_Show_Click(sender As Object, e As EventArgs) Handles lbl_Show.Click
        cbo_MacType.Items.Clear()
        cbo_MacType.Items.Add("PAM-1")
        cbo_MacType.Items.Add("PAM-2")
        cbo_MacType.Items.Add("PAM-3")
        cbo_MacType.Items.Add("PAM-B")
        For i = 0 To cbo_MacType.Items.Count - 1
            If cbo_MacType.Items(i).ToString = MACTYPE Then
                cbo_MacType.SelectedIndex = i
                Exit For
            End If
        Next
        lbl_MacType.Visible = True
        cbo_MacType.Visible = True
        btn_SaveMacType.Visible = True
    End Sub

    Private Sub btn_SaveMacType_Click(sender As Object, e As EventArgs) Handles btn_SaveMacType.Click
        Frm_Login.Show(Me)
        Login_Mode = 4
    End Sub

    Public Sub Save_MacType(ByVal Yes As Boolean)
        lbl_MacType.Visible = False
        cbo_MacType.Visible = False
        btn_SaveMacType.Visible = False
        If Yes Then
            MACTYPE = cbo_MacType.Items(cbo_MacType.SelectedIndex).ToString
            Try
                Write_MacType(Path_MacType, MACTYPE)
                par.Machine_Info.AE_SubID = MACTYPE
                'Save parameters
                Call Write_par(Path_Par, par)
                '在主界面上显示设备名称及编号
                Me.Btn_MachineInfo.Text = par.Machine_Info.AE_SubID & "-" & par.Machine_Info.Machine_SN
                Frm_DialogAddMessage("设备类型配置成功！")
            Catch ex As Exception
                Frm_DialogAddMessage("设备类型配置失败！")
            End Try
        End If

    End Sub

    ''' <summary>
    ''' 写DAT二进制文件
    ''' </summary>
    ''' <param name="FileName">参数1：文件名(包含文件路径)</param>
    ''' <param name="WriteData">参数2：写入的数据</param>
    ''' <remarks></remarks>
    Private Sub Write_MacType(ByVal FileName As String, ByVal WriteData As String)
        Dim FileNo As Integer
        Try
            'FileNo = FreeFile()                      '获取空闲可用的文件号
            'FileOpen(FileNo, FileName, OpenMode.Binary)      '以二进制的形式打开文件
            'FilePut(FileNo, WriteData)
            'FileClose(FileNo)

            FileIO.FileSystem.WriteAllText(FileName, WriteData, False)
        Catch ex As Exception
            FileClose(FileNo)                               '读取出错关闭当前打开的文件
            Frm_DialogAddMessage("写入设备类型配置文件失败！")
            Frm_DialogAddMessage(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' 读DAT二进制文件
    ''' </summary>
    ''' <param name="FileName">参数1：文件名(包含文件路径)</param>
    ''' <param name="ReadData">参数2：读取的数据存储地址</param>
    ''' <remarks></remarks>
    Private Sub Read_MacType(ByVal FileName As String, ByRef ReadData As String)
        Dim FileNo As Integer
        Try
            If IO.File.Exists(FileName) = False Then
                ReadData = "PAM-1"
                Call Write_MacType(FileName, ReadData)
            End If
            'FileNo = FreeFile()                      '获取空闲可用的文件号
            'FileOpen(FileNo, FileName, OpenMode.Binary)      '以二进制的形式打开文件
            'FileGet(FileNo, ReadData)
            'FileClose(FileNo)
            ReadData = FileIO.FileSystem.ReadAllText(FileName)

        Catch ex As Exception
            FileClose(FileNo)                               '读取出错关闭当前打开的文件
            Frm_DialogAddMessage("读取设备类型配置文件失败！")
            Frm_DialogAddMessage(ex.ToString)
        End Try
    End Sub

#End Region

    ' ''' <summary>
    ' ''' 自动按时间间隔排胶
    ' ''' </summary>
    ' ''' <param name="interval">间隔时间 单位：min</param>
    ' ''' <param name="timelast">持续时间 单位：second</param>
    ' ''' <remarks></remarks>
    'Public Sub AutoPurge_S2(ByVal interval As Integer, ByVal timelast As Long)
    '    Static TimeCount As Long = GetTickCount
    '    Static TimingClrGlue As Long = GetTickCount
    '    Static flag_NeedClose As Boolean

    '    If Flag_MachineAutoRun = False And Flag_MachineInit = False Then
    '        '设备未初始化或未进入自动运行，则退出
    '        Exit Sub
    '    End If

    '    '判断是否在待机位置
    '    If Abs(CurrEncPos(0, 1) - Par_Pos.St_Glue(0).X) > 5 Or Abs(CurrEncPos(0, 2) - Par_Pos.St_Glue(0).Y) > 5 Then
    '        Exit Sub
    '    End If

    '    If EXO(0, 6) And flag_NeedClose = False Then
    '        TimingClrGlue = GetTickCount
    '    End If

    '    If GetTickCount - TimingClrGlue > interval * 60 * 1000 Then
    '        If par.chkFn(15) And EXO(0, 6) = False Then
    '            SetEXO(0, 6, True) '2工位点胶打开
    '            SetEXO(0, 9, True) '蜂鸣器
    '            flag_NeedClose = True
    '            Flag_Purged_S2 = True
    '            TimeCount = GetTickCount
    '        End If

    '        If par.chkFn(15) And EXO(0, 6) And S2_Work.State = False And flag_NeedClose Then
    '            If GetTickCount - TimeCount > 500 Then
    '                SetEXO(0, 9, False) '蜂鸣器
    '            End If
    '            If GetTickCount - TimeCount > timelast * 1000 Then
    '                SetEXO(0, 6, False) '2工位点胶关闭
    '                flag_NeedClose = False
    '                TimingClrGlue = GetTickCount
    '            End If
    '        End If
    '    End If



    'End Sub

End Class
