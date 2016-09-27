<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Main
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Main))
        Me.Btn_Home = New BoTech.BZ_Button()
        Me.Btn_Settings = New BoTech.BZ_Button()
        Me.Btn_CCD = New BoTech.BZ_Button()
        Me.Btn_Chart = New BoTech.BZ_Button()
        Me.Btn_Alarm = New BoTech.BZ_Button()
        Me.Btn_MachineInfo = New BoTech.BZ_Button()
        Me.Btn_Start = New BoTech.BZ_Button()
        Me.Btn_Pause = New BoTech.BZ_Button()
        Me.Btn_Stop = New BoTech.BZ_Button()
        Me.Btn_OpenExcelData = New BoTech.BZ_Button()
        Me.Btn_OpenImageFiles = New BoTech.BZ_Button()
        Me.Btn_Mode = New BoTech.BZ_Button()
        Me.BZ_RoundPanel1 = New BoTech.BZ_RoundPanel()
        Me.Winsock4 = New AxMSWinsockLib.AxWinsock()
        Me.Winsock3 = New AxMSWinsockLib.AxWinsock()
        Me.Winsock2 = New AxMSWinsockLib.AxWinsock()
        Me.Winsock1 = New AxMSWinsockLib.AxWinsock()
        Me.Btn_CPKGRR = New BoTech.BZ_Button()
        Me.Btn_ModeEngineer = New BoTech.BZ_Button()
        Me.Btn_ModeProduction = New BoTech.BZ_Button()
        Me.BZ_RoundPanel2 = New BoTech.BZ_RoundPanel()
        Me.txt_Password = New System.Windows.Forms.TextBox()
        Me.ComboBox_User = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Btn_Exit = New BoTech.BZ_Button()
        Me.Btn_Login = New BoTech.BZ_Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BZ_RoundPanel3 = New BoTech.BZ_RoundPanel()
        Me.btn_SaveMacType = New System.Windows.Forms.Button()
        Me.lbl_Show = New System.Windows.Forms.Label()
        Me.lbl_MacType = New System.Windows.Forms.Label()
        Me.cbo_MacType = New System.Windows.Forms.ComboBox()
        Me.Timer_UI = New System.Windows.Forms.Timer(Me.components)
        Me.COM2 = New System.IO.Ports.SerialPort(Me.components)
        Me.COM3 = New System.IO.Ports.SerialPort(Me.components)
        Me.Timer_MacInit = New System.Windows.Forms.Timer(Me.components)
        Me.btn_test = New System.Windows.Forms.Button()
        Me.Timer_Sys = New System.Windows.Forms.Timer(Me.components)
        Me.Timer_AutoRun = New System.Windows.Forms.Timer(Me.components)
        Me.COM1 = New System.IO.Ports.SerialPort(Me.components)
        Me.COM4 = New System.IO.Ports.SerialPort(Me.components)
        Me.COM5 = New System.IO.Ports.SerialPort(Me.components)
        Me.BZ_RoundPanel1.SuspendLayout()
        CType(Me.Winsock4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Winsock3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Winsock2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Winsock1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BZ_RoundPanel2.SuspendLayout()
        Me.BZ_RoundPanel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Btn_Home
        '
        Me.Btn_Home.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Home.BZ_Radius = 11
        Me.Btn_Home.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Home.Image = CType(resources.GetObject("Btn_Home.Image"), System.Drawing.Image)
        Me.Btn_Home.Location = New System.Drawing.Point(5, 5)
        Me.Btn_Home.Name = "Btn_Home"
        Me.Btn_Home.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Home.TabIndex = 0
        Me.Btn_Home.UseVisualStyleBackColor = True
        '
        'Btn_Settings
        '
        Me.Btn_Settings.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Settings.BZ_Radius = 11
        Me.Btn_Settings.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Settings.Image = CType(resources.GetObject("Btn_Settings.Image"), System.Drawing.Image)
        Me.Btn_Settings.Location = New System.Drawing.Point(70, 5)
        Me.Btn_Settings.Name = "Btn_Settings"
        Me.Btn_Settings.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Settings.TabIndex = 1
        Me.Btn_Settings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Btn_Settings.UseVisualStyleBackColor = True
        '
        'Btn_CCD
        '
        Me.Btn_CCD.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_CCD.BZ_Radius = 11
        Me.Btn_CCD.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_CCD.Image = CType(resources.GetObject("Btn_CCD.Image"), System.Drawing.Image)
        Me.Btn_CCD.Location = New System.Drawing.Point(135, 5)
        Me.Btn_CCD.Name = "Btn_CCD"
        Me.Btn_CCD.Size = New System.Drawing.Size(60, 60)
        Me.Btn_CCD.TabIndex = 2
        Me.Btn_CCD.UseVisualStyleBackColor = True
        '
        'Btn_Chart
        '
        Me.Btn_Chart.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Chart.BZ_Radius = 11
        Me.Btn_Chart.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Chart.Image = CType(resources.GetObject("Btn_Chart.Image"), System.Drawing.Image)
        Me.Btn_Chart.Location = New System.Drawing.Point(200, 5)
        Me.Btn_Chart.Name = "Btn_Chart"
        Me.Btn_Chart.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Chart.TabIndex = 3
        Me.Btn_Chart.UseVisualStyleBackColor = True
        '
        'Btn_Alarm
        '
        Me.Btn_Alarm.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Alarm.BZ_Radius = 11
        Me.Btn_Alarm.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Alarm.Image = CType(resources.GetObject("Btn_Alarm.Image"), System.Drawing.Image)
        Me.Btn_Alarm.Location = New System.Drawing.Point(265, 5)
        Me.Btn_Alarm.Name = "Btn_Alarm"
        Me.Btn_Alarm.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Alarm.TabIndex = 4
        Me.Btn_Alarm.UseVisualStyleBackColor = True
        '
        'Btn_MachineInfo
        '
        Me.Btn_MachineInfo.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_MachineInfo.BZ_Radius = 11
        Me.Btn_MachineInfo.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_MachineInfo.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_MachineInfo.Location = New System.Drawing.Point(330, 5)
        Me.Btn_MachineInfo.Name = "Btn_MachineInfo"
        Me.Btn_MachineInfo.Size = New System.Drawing.Size(160, 60)
        Me.Btn_MachineInfo.TabIndex = 5
        Me.Btn_MachineInfo.Text = "BZ-001"
        Me.Btn_MachineInfo.UseVisualStyleBackColor = True
        '
        'Btn_Start
        '
        Me.Btn_Start.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Start.BZ_Radius = 11
        Me.Btn_Start.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Start.Image = CType(resources.GetObject("Btn_Start.Image"), System.Drawing.Image)
        Me.Btn_Start.Location = New System.Drawing.Point(495, 5)
        Me.Btn_Start.Name = "Btn_Start"
        Me.Btn_Start.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Start.TabIndex = 6
        Me.Btn_Start.UseVisualStyleBackColor = True
        '
        'Btn_Pause
        '
        Me.Btn_Pause.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Pause.BZ_Radius = 11
        Me.Btn_Pause.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Pause.Image = CType(resources.GetObject("Btn_Pause.Image"), System.Drawing.Image)
        Me.Btn_Pause.Location = New System.Drawing.Point(560, 5)
        Me.Btn_Pause.Name = "Btn_Pause"
        Me.Btn_Pause.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Pause.TabIndex = 7
        Me.Btn_Pause.UseVisualStyleBackColor = True
        '
        'Btn_Stop
        '
        Me.Btn_Stop.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Stop.BZ_Radius = 11
        Me.Btn_Stop.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Stop.Image = CType(resources.GetObject("Btn_Stop.Image"), System.Drawing.Image)
        Me.Btn_Stop.Location = New System.Drawing.Point(625, 5)
        Me.Btn_Stop.Name = "Btn_Stop"
        Me.Btn_Stop.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Stop.TabIndex = 8
        Me.Btn_Stop.UseVisualStyleBackColor = True
        '
        'Btn_OpenExcelData
        '
        Me.Btn_OpenExcelData.BZ_Color = System.Drawing.Color.Transparent
        Me.Btn_OpenExcelData.BZ_Radius = 11
        Me.Btn_OpenExcelData.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_OpenExcelData.Image = CType(resources.GetObject("Btn_OpenExcelData.Image"), System.Drawing.Image)
        Me.Btn_OpenExcelData.Location = New System.Drawing.Point(829, 5)
        Me.Btn_OpenExcelData.Name = "Btn_OpenExcelData"
        Me.Btn_OpenExcelData.Size = New System.Drawing.Size(60, 60)
        Me.Btn_OpenExcelData.TabIndex = 9
        Me.Btn_OpenExcelData.UseVisualStyleBackColor = True
        '
        'Btn_OpenImageFiles
        '
        Me.Btn_OpenImageFiles.BZ_Color = System.Drawing.Color.Transparent
        Me.Btn_OpenImageFiles.BZ_Radius = 11
        Me.Btn_OpenImageFiles.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_OpenImageFiles.Image = CType(resources.GetObject("Btn_OpenImageFiles.Image"), System.Drawing.Image)
        Me.Btn_OpenImageFiles.Location = New System.Drawing.Point(894, 5)
        Me.Btn_OpenImageFiles.Name = "Btn_OpenImageFiles"
        Me.Btn_OpenImageFiles.Size = New System.Drawing.Size(60, 60)
        Me.Btn_OpenImageFiles.TabIndex = 10
        Me.Btn_OpenImageFiles.UseVisualStyleBackColor = True
        '
        'Btn_Mode
        '
        Me.Btn_Mode.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Mode.BZ_Radius = 11
        Me.Btn_Mode.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Mode.Image = CType(resources.GetObject("Btn_Mode.Image"), System.Drawing.Image)
        Me.Btn_Mode.Location = New System.Drawing.Point(959, 5)
        Me.Btn_Mode.Name = "Btn_Mode"
        Me.Btn_Mode.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Mode.TabIndex = 11
        Me.Btn_Mode.UseVisualStyleBackColor = True
        '
        'BZ_RoundPanel1
        '
        Me.BZ_RoundPanel1.BZ_Color = System.Drawing.Color.WhiteSmoke
        Me.BZ_RoundPanel1.BZ_Radius = 11
        Me.BZ_RoundPanel1.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel1.Controls.Add(Me.Winsock4)
        Me.BZ_RoundPanel1.Controls.Add(Me.Winsock3)
        Me.BZ_RoundPanel1.Controls.Add(Me.Winsock2)
        Me.BZ_RoundPanel1.Controls.Add(Me.Winsock1)
        Me.BZ_RoundPanel1.Controls.Add(Me.Btn_CPKGRR)
        Me.BZ_RoundPanel1.Controls.Add(Me.Btn_ModeEngineer)
        Me.BZ_RoundPanel1.Controls.Add(Me.Btn_ModeProduction)
        Me.BZ_RoundPanel1.Location = New System.Drawing.Point(5, 70)
        Me.BZ_RoundPanel1.Name = "BZ_RoundPanel1"
        Me.BZ_RoundPanel1.Size = New System.Drawing.Size(680, 655)
        Me.BZ_RoundPanel1.TabIndex = 12
        '
        'Winsock4
        '
        Me.Winsock4.Enabled = True
        Me.Winsock4.Location = New System.Drawing.Point(181, 620)
        Me.Winsock4.Name = "Winsock4"
        Me.Winsock4.OcxState = CType(resources.GetObject("Winsock4.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Winsock4.Size = New System.Drawing.Size(28, 28)
        Me.Winsock4.TabIndex = 6
        '
        'Winsock3
        '
        Me.Winsock3.Enabled = True
        Me.Winsock3.Location = New System.Drawing.Point(147, 620)
        Me.Winsock3.Name = "Winsock3"
        Me.Winsock3.OcxState = CType(resources.GetObject("Winsock3.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Winsock3.Size = New System.Drawing.Size(28, 28)
        Me.Winsock3.TabIndex = 5
        '
        'Winsock2
        '
        Me.Winsock2.Enabled = True
        Me.Winsock2.Location = New System.Drawing.Point(113, 620)
        Me.Winsock2.Name = "Winsock2"
        Me.Winsock2.OcxState = CType(resources.GetObject("Winsock2.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Winsock2.Size = New System.Drawing.Size(28, 28)
        Me.Winsock2.TabIndex = 4
        '
        'Winsock1
        '
        Me.Winsock1.Enabled = True
        Me.Winsock1.Location = New System.Drawing.Point(79, 620)
        Me.Winsock1.Name = "Winsock1"
        Me.Winsock1.OcxState = CType(resources.GetObject("Winsock1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.Winsock1.Size = New System.Drawing.Size(28, 28)
        Me.Winsock1.TabIndex = 3
        '
        'Btn_CPKGRR
        '
        Me.Btn_CPKGRR.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_CPKGRR.BZ_Radius = 11
        Me.Btn_CPKGRR.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_CPKGRR.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_CPKGRR.Location = New System.Drawing.Point(195, 296)
        Me.Btn_CPKGRR.Name = "Btn_CPKGRR"
        Me.Btn_CPKGRR.Size = New System.Drawing.Size(290, 60)
        Me.Btn_CPKGRR.TabIndex = 2
        Me.Btn_CPKGRR.Text = "CPK/GRR Mode"
        Me.Btn_CPKGRR.UseVisualStyleBackColor = True
        '
        'Btn_ModeEngineer
        '
        Me.Btn_ModeEngineer.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_ModeEngineer.BZ_Radius = 11
        Me.Btn_ModeEngineer.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_ModeEngineer.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_ModeEngineer.Location = New System.Drawing.Point(195, 198)
        Me.Btn_ModeEngineer.Name = "Btn_ModeEngineer"
        Me.Btn_ModeEngineer.Size = New System.Drawing.Size(290, 60)
        Me.Btn_ModeEngineer.TabIndex = 1
        Me.Btn_ModeEngineer.Text = "Engineering Mode"
        Me.Btn_ModeEngineer.UseVisualStyleBackColor = True
        '
        'Btn_ModeProduction
        '
        Me.Btn_ModeProduction.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_ModeProduction.BZ_Radius = 11
        Me.Btn_ModeProduction.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_ModeProduction.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_ModeProduction.Location = New System.Drawing.Point(195, 100)
        Me.Btn_ModeProduction.Name = "Btn_ModeProduction"
        Me.Btn_ModeProduction.Size = New System.Drawing.Size(290, 60)
        Me.Btn_ModeProduction.TabIndex = 0
        Me.Btn_ModeProduction.Text = "Production Mode"
        Me.Btn_ModeProduction.UseVisualStyleBackColor = True
        '
        'BZ_RoundPanel2
        '
        Me.BZ_RoundPanel2.BZ_Color = System.Drawing.Color.WhiteSmoke
        Me.BZ_RoundPanel2.BZ_Radius = 11
        Me.BZ_RoundPanel2.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel2.Controls.Add(Me.txt_Password)
        Me.BZ_RoundPanel2.Controls.Add(Me.ComboBox_User)
        Me.BZ_RoundPanel2.Controls.Add(Me.Label3)
        Me.BZ_RoundPanel2.Controls.Add(Me.Label2)
        Me.BZ_RoundPanel2.Controls.Add(Me.Btn_Exit)
        Me.BZ_RoundPanel2.Controls.Add(Me.Btn_Login)
        Me.BZ_RoundPanel2.Controls.Add(Me.Label1)
        Me.BZ_RoundPanel2.Location = New System.Drawing.Point(690, 70)
        Me.BZ_RoundPanel2.Name = "BZ_RoundPanel2"
        Me.BZ_RoundPanel2.Size = New System.Drawing.Size(329, 380)
        Me.BZ_RoundPanel2.TabIndex = 13
        '
        'txt_Password
        '
        Me.txt_Password.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Password.Location = New System.Drawing.Point(28, 228)
        Me.txt_Password.Name = "txt_Password"
        Me.txt_Password.Size = New System.Drawing.Size(252, 31)
        Me.txt_Password.TabIndex = 6
        Me.txt_Password.UseSystemPasswordChar = True
        '
        'ComboBox_User
        '
        Me.ComboBox_User.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox_User.FormattingEnabled = True
        Me.ComboBox_User.ItemHeight = 25
        Me.ComboBox_User.Location = New System.Drawing.Point(28, 152)
        Me.ComboBox_User.Name = "ComboBox_User"
        Me.ComboBox_User.Size = New System.Drawing.Size(252, 33)
        Me.ComboBox_User.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("HelveticaNeue", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 199)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Password"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("HelveticaNeue", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(25, 126)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "User"
        '
        'Btn_Exit
        '
        Me.Btn_Exit.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Exit.BZ_Radius = 11
        Me.Btn_Exit.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Exit.Font = New System.Drawing.Font("HelveticaNeue", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_Exit.Location = New System.Drawing.Point(170, 310)
        Me.Btn_Exit.Name = "Btn_Exit"
        Me.Btn_Exit.Size = New System.Drawing.Size(110, 40)
        Me.Btn_Exit.TabIndex = 2
        Me.Btn_Exit.Text = "Exit"
        Me.Btn_Exit.UseVisualStyleBackColor = True
        '
        'Btn_Login
        '
        Me.Btn_Login.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Login.BZ_Radius = 11
        Me.Btn_Login.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Login.Font = New System.Drawing.Font("HelveticaNeue", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_Login.Location = New System.Drawing.Point(25, 310)
        Me.Btn_Login.Name = "Btn_Login"
        Me.Btn_Login.Size = New System.Drawing.Size(110, 40)
        Me.Btn_Login.TabIndex = 1
        Me.Btn_Login.Text = "Login"
        Me.Btn_Login.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Image = CType(resources.GetObject("Label1.Image"), System.Drawing.Image)
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 90)
        Me.Label1.TabIndex = 0
        '
        'BZ_RoundPanel3
        '
        Me.BZ_RoundPanel3.BZ_Color = System.Drawing.Color.WhiteSmoke
        Me.BZ_RoundPanel3.BZ_Radius = 11
        Me.BZ_RoundPanel3.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel3.Controls.Add(Me.btn_SaveMacType)
        Me.BZ_RoundPanel3.Controls.Add(Me.lbl_Show)
        Me.BZ_RoundPanel3.Controls.Add(Me.lbl_MacType)
        Me.BZ_RoundPanel3.Controls.Add(Me.cbo_MacType)
        Me.BZ_RoundPanel3.Location = New System.Drawing.Point(690, 455)
        Me.BZ_RoundPanel3.Name = "BZ_RoundPanel3"
        Me.BZ_RoundPanel3.Size = New System.Drawing.Size(329, 270)
        Me.BZ_RoundPanel3.TabIndex = 14
        '
        'btn_SaveMacType
        '
        Me.btn_SaveMacType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_SaveMacType.Location = New System.Drawing.Point(139, 140)
        Me.btn_SaveMacType.Name = "btn_SaveMacType"
        Me.btn_SaveMacType.Size = New System.Drawing.Size(60, 60)
        Me.btn_SaveMacType.TabIndex = 72
        Me.btn_SaveMacType.Text = "Save"
        Me.btn_SaveMacType.UseVisualStyleBackColor = True
        Me.btn_SaveMacType.Visible = False
        '
        'lbl_Show
        '
        Me.lbl_Show.Location = New System.Drawing.Point(267, 213)
        Me.lbl_Show.Name = "lbl_Show"
        Me.lbl_Show.Size = New System.Drawing.Size(50, 50)
        Me.lbl_Show.TabIndex = 71
        '
        'lbl_MacType
        '
        Me.lbl_MacType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MacType.Location = New System.Drawing.Point(17, 99)
        Me.lbl_MacType.Name = "lbl_MacType"
        Me.lbl_MacType.Size = New System.Drawing.Size(90, 22)
        Me.lbl_MacType.TabIndex = 70
        Me.lbl_MacType.Text = "Machine Type:"
        Me.lbl_MacType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lbl_MacType.Visible = False
        '
        'cbo_MacType
        '
        Me.cbo_MacType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.999999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbo_MacType.FormattingEnabled = True
        Me.cbo_MacType.Location = New System.Drawing.Point(113, 99)
        Me.cbo_MacType.Name = "cbo_MacType"
        Me.cbo_MacType.Size = New System.Drawing.Size(120, 23)
        Me.cbo_MacType.TabIndex = 69
        Me.cbo_MacType.Visible = False
        '
        'Timer_UI
        '
        Me.Timer_UI.Interval = 20
        '
        'COM2
        '
        '
        'COM3
        '
        '
        'Timer_MacInit
        '
        Me.Timer_MacInit.Interval = 10
        '
        'btn_test
        '
        Me.btn_test.Location = New System.Drawing.Point(718, 24)
        Me.btn_test.Name = "btn_test"
        Me.btn_test.Size = New System.Drawing.Size(105, 23)
        Me.btn_test.TabIndex = 15
        Me.btn_test.Text = "Test Button"
        Me.btn_test.UseVisualStyleBackColor = True
        '
        'Timer_Sys
        '
        Me.Timer_Sys.Interval = 10
        '
        'Timer_AutoRun
        '
        Me.Timer_AutoRun.Interval = 10
        '
        'COM1
        '
        '
        'COM4
        '
        '
        'Frm_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1024, 730)
        Me.Controls.Add(Me.btn_test)
        Me.Controls.Add(Me.BZ_RoundPanel3)
        Me.Controls.Add(Me.BZ_RoundPanel2)
        Me.Controls.Add(Me.BZ_RoundPanel1)
        Me.Controls.Add(Me.Btn_Mode)
        Me.Controls.Add(Me.Btn_OpenImageFiles)
        Me.Controls.Add(Me.Btn_OpenExcelData)
        Me.Controls.Add(Me.Btn_Stop)
        Me.Controls.Add(Me.Btn_Pause)
        Me.Controls.Add(Me.Btn_Start)
        Me.Controls.Add(Me.Btn_MachineInfo)
        Me.Controls.Add(Me.Btn_Alarm)
        Me.Controls.Add(Me.Btn_Chart)
        Me.Controls.Add(Me.Btn_CCD)
        Me.Controls.Add(Me.Btn_Settings)
        Me.Controls.Add(Me.Btn_Home)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Frm_Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Main Window"
        Me.BZ_RoundPanel1.ResumeLayout(False)
        CType(Me.Winsock4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Winsock3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Winsock2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Winsock1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BZ_RoundPanel2.ResumeLayout(False)
        Me.BZ_RoundPanel2.PerformLayout()
        Me.BZ_RoundPanel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Btn_Home As BoTech.BZ_Button
    Friend WithEvents Btn_Settings As BoTech.BZ_Button
    Friend WithEvents Btn_CCD As BoTech.BZ_Button
    Friend WithEvents Btn_Chart As BoTech.BZ_Button
    Friend WithEvents Btn_Alarm As BoTech.BZ_Button
    Friend WithEvents Btn_MachineInfo As BoTech.BZ_Button
    Friend WithEvents Btn_Start As BoTech.BZ_Button
    Friend WithEvents Btn_Pause As BoTech.BZ_Button
    Friend WithEvents Btn_Stop As BoTech.BZ_Button
    Friend WithEvents Btn_OpenExcelData As BoTech.BZ_Button
    Friend WithEvents Btn_OpenImageFiles As BoTech.BZ_Button
    Friend WithEvents Btn_Mode As BoTech.BZ_Button
    Friend WithEvents BZ_RoundPanel1 As BoTech.BZ_RoundPanel
    Friend WithEvents BZ_RoundPanel2 As BoTech.BZ_RoundPanel
    Friend WithEvents Btn_Exit As BoTech.BZ_Button
    Friend WithEvents Btn_Login As BoTech.BZ_Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BZ_RoundPanel3 As BoTech.BZ_RoundPanel
    Friend WithEvents Btn_ModeProduction As BoTech.BZ_Button
    Friend WithEvents Btn_CPKGRR As BoTech.BZ_Button
    Friend WithEvents Btn_ModeEngineer As BoTech.BZ_Button
    Friend WithEvents txt_Password As System.Windows.Forms.TextBox
    Friend WithEvents ComboBox_User As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Timer_UI As System.Windows.Forms.Timer
    Friend WithEvents COM2 As System.IO.Ports.SerialPort
    Friend WithEvents COM3 As System.IO.Ports.SerialPort
    Friend WithEvents Winsock1 As AxMSWinsockLib.AxWinsock
    Friend WithEvents Winsock3 As AxMSWinsockLib.AxWinsock
    Friend WithEvents Winsock2 As AxMSWinsockLib.AxWinsock
    Friend WithEvents Timer_MacInit As System.Windows.Forms.Timer
    Friend WithEvents btn_test As System.Windows.Forms.Button
    Friend WithEvents Timer_Sys As System.Windows.Forms.Timer
    Friend WithEvents Timer_AutoRun As System.Windows.Forms.Timer
    Friend WithEvents COM1 As System.IO.Ports.SerialPort
    Friend WithEvents Winsock4 As AxMSWinsockLib.AxWinsock
    Friend WithEvents COM4 As System.IO.Ports.SerialPort
    Friend WithEvents cbo_MacType As System.Windows.Forms.ComboBox
    Friend WithEvents btn_SaveMacType As System.Windows.Forms.Button
    Friend WithEvents lbl_Show As System.Windows.Forms.Label
    Friend WithEvents lbl_MacType As System.Windows.Forms.Label
    Friend WithEvents COM5 As System.IO.Ports.SerialPort

End Class
