Public Class Frm_Engineering

    Public lbl_PrfPos(2, 8) As Label
    Public lbl_EncPos(2, 8) As Label
    Public btn_Servo(2, 8) As Button
    Public btn_Home(2, 8) As Button
    Public chk_Brc(12) As CheckBox

#Region "   窗体加载"
    ''' <summary>
    ''' 工程界面加载
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Frm_Engineering_Load(sender As Object, e As EventArgs) Handles Me.Load

        Flag_FrmEngineeringOpned = True
        '窗体加载时，设置界面的显示颜色
        Me.BackColor = Color.FromArgb(252, 223, 222)
        For i = 0 To TabControl1.TabPages.Count - 1
            TabControl1.TabPages(i).BackColor = Color.FromArgb(252, 223, 222)
        Next

        Call Load_CurWorkState()
        Call OutPut_Button_Load()
        Call GoHome_Button_Load()
        Call ServoOn_Button_Load()
        Call AxisMove_Button_Load()
        Call Pos_Enc_Prf_Load()
        Call Load_ServoHome()
        Call Load_Pos()
        Call DataGridViewInit()
        Call Load_NeedlePar()
        Call Load_SelectBrc()
        Call Load_LineBtnTag()
    End Sub

    ''' <summary>
    ''' 加载IO输出Button时填写用户标记数据
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub OutPut_Button_Load()
        btn_Out_0_00.Tag = 0
        btn_Out_0_01.Tag = 1
        btn_Out_0_02.Tag = 2
        btn_Out_0_03.Tag = 3
        btn_Out_0_04.Tag = 4
        btn_Out_0_05.Tag = 5
        btn_Out_0_06.Tag = 6
        btn_Out_0_07.Tag = 7
        btn_Out_0_08.Tag = 8
        btn_Out_0_09.Tag = 9
        btn_Out_0_10.Tag = 10
        btn_Out_0_11.Tag = 11
        btn_Out_0_12.Tag = 12
        btn_Out_0_13.Tag = 13
        btn_Out_0_14.Tag = 14
        btn_Out_0_15.Tag = 15

        btn_Out_1_00.Tag = 0
        btn_Out_1_01.Tag = 1
        btn_Out_1_02.Tag = 2
        btn_Out_1_03.Tag = 3
        btn_Out_1_04.Tag = 4
        btn_Out_1_05.Tag = 5
        btn_Out_1_06.Tag = 6
        btn_Out_1_07.Tag = 7
        btn_Out_1_08.Tag = 8
        btn_Out_1_09.Tag = 9
        btn_Out_1_10.Tag = 10
        btn_Out_1_11.Tag = 11
        btn_Out_1_12.Tag = 12
        btn_Out_1_13.Tag = 13
        btn_Out_1_14.Tag = 14
        btn_Out_1_15.Tag = 15

        btn_Out_3_00.Tag = 0
        btn_Out_3_01.Tag = 1
        btn_Out_3_02.Tag = 2
        btn_Out_3_03.Tag = 3
        btn_Out_3_04.Tag = 4
        btn_Out_3_05.Tag = 5
        btn_Out_3_06.Tag = 6
        btn_Out_3_07.Tag = 7
        btn_Out_3_08.Tag = 8
        btn_Out_3_09.Tag = 9
        btn_Out_3_10.Tag = 10
        btn_Out_3_11.Tag = 11
        btn_Out_3_12.Tag = 12
        btn_Out_3_13.Tag = 13
        btn_Out_3_14.Tag = 14
        btn_Out_3_15.Tag = 15

        btn_Out_4_00.Tag = 0
        btn_Out_4_01.Tag = 1
        btn_Out_4_02.Tag = 2
        btn_Out_4_03.Tag = 3
        btn_Out_4_04.Tag = 4
        btn_Out_4_05.Tag = 5
        btn_Out_4_06.Tag = 6
        btn_Out_4_07.Tag = 7
        btn_Out_4_08.Tag = 8
        btn_Out_4_09.Tag = 9
        btn_Out_4_10.Tag = 10
        btn_Out_4_11.Tag = 11
        btn_Out_4_12.Tag = 12
        btn_Out_4_13.Tag = 13
        btn_Out_4_14.Tag = 14
        btn_Out_4_15.Tag = 15

        btn_Out_5_00.Tag = 0
        btn_Out_5_01.Tag = 1
        btn_Out_5_02.Tag = 2
        btn_Out_5_03.Tag = 3
        btn_Out_5_04.Tag = 4
        btn_Out_5_05.Tag = 5
        btn_Out_5_06.Tag = 6
        btn_Out_5_07.Tag = 7
        btn_Out_5_08.Tag = 8
        btn_Out_5_09.Tag = 9
        btn_Out_5_10.Tag = 10
        btn_Out_5_11.Tag = 11
        btn_Out_5_12.Tag = 12
        btn_Out_5_13.Tag = 13
        btn_Out_5_14.Tag = 14
        btn_Out_5_15.Tag = 15

        btn_S2IO1.Tag = 1
        btn_S2IO2.Tag = 2
        btn_S2IO3.Tag = 3
        btn_S2IO4.Tag = 4
        btn_S2IO5.Tag = 5
        btn_S2IO6.Tag = 6
        btn_S2IO7.Tag = 7
        btn_S2IO8.Tag = 8

        btn_S3IO1.Tag = 1
        btn_S3IO2.Tag = 2
        btn_S3IO3.Tag = 3
        btn_S3IO4.Tag = 4
        btn_S3IO5.Tag = 5
        btn_S3IO6.Tag = 6
        btn_S3IO7.Tag = 7
        btn_S3IO8.Tag = 8

        btn_S4IO1.Tag = 1
        btn_S4IO2.Tag = 2
        btn_S4IO3.Tag = 3
        btn_S4IO4.Tag = 4
        btn_S4IO5.Tag = 5
        btn_S4IO6.Tag = 6
        btn_S4IO7.Tag = 7
        btn_S4IO8.Tag = 8

        btn_S5IO1.Tag = 1
        btn_S5IO2.Tag = 2
        btn_S5IO3.Tag = 3
        btn_S5IO4.Tag = 4
        btn_S5IO5.Tag = 5
        btn_S5IO6.Tag = 6
        btn_S5IO7.Tag = 7
        btn_S5IO8.Tag = 8

    End Sub

    ''' <summary>
    ''' 为回原点Button添加轴号标记
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GoHome_Button_Load()
        btn_Home1.Tag = 1      'GlueX
        btn_Home2.Tag = 2      'GlueY
        btn_Home3.Tag = 3      'GLUeZ
        btn_Home4.Tag = Nothing
        btn_Home5.Tag = 4      'PasteX
        btn_Home6.Tag = 17     '18 龙门Gantary PasteY
        btn_Home7.Tag = 5      'PasteZ
        btn_Home8.Tag = 6      'PasteR
        btn_Home9.Tag = 7      'PreTakerX
        btn_Home10.Tag = 19      '20 龙门Gantary PreTakerY
        btn_Home11.Tag = 8      'PreTakerZ
        btn_Home12.Tag = 9      'PreTakerR
        btn_Home13.Tag = 11      'FineX
        btn_Home14.Tag = 12      'FineY
        btn_Home15.Tag = 13      'RecheckX
        btn_Home16.Tag = 14      'RecheckY
        btn_Home17.Tag = 10      'CureX
        btn_Home18.Tag = 15      'FeedZ
        btn_Home19.Tag = 16      'RecycleZ

    End Sub

    ''' <summary>
    ''' 为伺服使能Button添加轴号标记
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ServoOn_Button_Load()
        btn_Servo1.Tag = 1      'GlueX
        btn_Servo2.Tag = 2      'GlueY
        btn_Servo3.Tag = 3      'GLUeZ
        btn_Servo4.Tag = Nothing
        btn_Servo5.Tag = 4      'PasteX
        btn_Servo6.Tag = 17     '18 龙门Gantary PasteY
        btn_Servo7.Tag = 5      'PasteZ
        btn_Servo8.Tag = 6      'PasteR
        btn_Servo9.Tag = 7      'PreTakerX
        btn_Servo10.Tag = 19      '20 龙门Gantary PreTakerY
        btn_Servo11.Tag = 8      'PreTakerZ
        btn_Servo12.Tag = 9      'PreTakerR

        btn_Servo13.Tag = 11      'FineX
        btn_Servo14.Tag = 12      'FineY

        btn_Servo15.Tag = 13      'RecheckX
        btn_Servo16.Tag = 14      'RecheckY
        btn_Servo17.Tag = 10      'CureX

        btn_Servo18.Tag = 15      'FeedZ
        btn_Servo19.Tag = 16      'RecycleZ

    End Sub

    ''' <summary>
    ''' 加载显示规划位置和编码器位置的label
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Pos_Enc_Prf_Load()
        For n = 0 To GTS_CardNum - 1
            For i = 1 To GTS_AxisNum(n)
                lbl_EncPos(n, i) = New Label
                lbl_PrfPos(n, i) = New Label
            Next
        Next
        lbl_PrfPos(0, 1) = labPrfPos1
        lbl_PrfPos(0, 2) = labPrfPos2
        lbl_PrfPos(0, 3) = labPrfPos3
        lbl_PrfPos(0, 4) = labPrfPos5
        lbl_PrfPos(0, 5) = labPrfPos7
        lbl_PrfPos(0, 6) = labPrfPos8
        lbl_PrfPos(0, 7) = labPrfPos9
        lbl_PrfPos(0, 8) = labPrfPos11

        lbl_PrfPos(1, 1) = labPrfPos12
        lbl_PrfPos(1, 2) = labPrfPos17
        lbl_PrfPos(1, 3) = labPrfPos13
        lbl_PrfPos(1, 4) = labPrfPos14
        lbl_PrfPos(1, 5) = labPrfPos15
        lbl_PrfPos(1, 6) = labPrfPos16
        lbl_PrfPos(1, 7) = labPrfPos18
        lbl_PrfPos(1, 8) = labPrfPos19

        lbl_PrfPos(2, 1) = labPrfPos6
        lbl_PrfPos(2, 3) = labPrfPos10

        lbl_EncPos(0, 1) = labEncPos1
        lbl_EncPos(0, 2) = labEncPos2
        lbl_EncPos(0, 3) = labEncPos3
        lbl_EncPos(0, 4) = labEncPos5
        lbl_EncPos(0, 5) = labEncPos7
        lbl_EncPos(0, 6) = labEncPos8
        lbl_EncPos(0, 7) = labEncPos9
        lbl_EncPos(0, 8) = labEncPos11

        lbl_EncPos(1, 1) = labEncPos12
        lbl_EncPos(1, 2) = labEncPos17
        lbl_EncPos(1, 3) = labEncPos13
        lbl_EncPos(1, 4) = labEncPos14
        lbl_EncPos(1, 5) = labEncPos15
        lbl_EncPos(1, 6) = labEncPos16
        lbl_EncPos(1, 7) = labEncPos18
        lbl_EncPos(1, 8) = labEncPos19

        lbl_EncPos(2, 1) = labEncPos6
        lbl_EncPos(2, 3) = labEncPos10
    End Sub

    ''' <summary>
    ''' 加载伺服使能和回原点OK信号的按钮
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load_ServoHome()
        For n = 0 To GTS_CardNum - 1
            For i = 1 To GTS_AxisNum(n)
                btn_Servo(n, i) = New Button
                btn_Home(n, i) = New Button
            Next
        Next

        btn_Servo(0, 1) = btn_Servo1    'GlueX
        btn_Servo(0, 2) = btn_Servo2    'GlueZ
        btn_Servo(0, 3) = btn_Servo3    'GlueR
        btn_Servo(0, 4) = btn_Servo5    'PasteX
        'PasteY
        btn_Servo(0, 5) = btn_Servo7    'PasteZ
        btn_Servo(0, 6) = btn_Servo8    'PasteR
        btn_Servo(0, 7) = btn_Servo9    'PreTakerX
        'PreTakerY
        btn_Servo(0, 8) = btn_Servo11    'PreTakerZ

        btn_Servo(1, 1) = btn_Servo12    'PreTakerR
        btn_Servo(1, 2) = btn_Servo17   'CureX
        btn_Servo(1, 3) = btn_Servo13   'FineX
        btn_Servo(1, 4) = btn_Servo14   'FineY
        btn_Servo(1, 5) = btn_Servo15   'RecheckX
        btn_Servo(1, 6) = btn_Servo16   'RecheckY
        btn_Servo(1, 7) = btn_Servo18   'FeedZ
        btn_Servo(1, 8) = btn_Servo19   'RecycleZ

        btn_Servo(2, 1) = btn_Servo6    'PasteY
        btn_Servo(2, 3) = btn_Servo10   'PreTakerY

        btn_Home(0, 1) = btn_Home1
        btn_Home(0, 2) = btn_Home2
        btn_Home(0, 3) = btn_Home3
        btn_Home(0, 4) = btn_Home5
        btn_Home(0, 5) = btn_Home7
        btn_Home(0, 6) = btn_Home8
        btn_Home(0, 7) = btn_Home9
        btn_Home(0, 8) = btn_Home11

        btn_Home(1, 1) = btn_Home12
        btn_Home(1, 2) = btn_Home17
        btn_Home(1, 3) = btn_Home13
        btn_Home(1, 4) = btn_Home14
        btn_Home(1, 5) = btn_Home15
        btn_Home(1, 6) = btn_Home16
        btn_Home(1, 7) = btn_Home18
        btn_Home(1, 8) = btn_Home19

        btn_Home(2, 1) = btn_Home6
        btn_Home(2, 2) = btn_Home10

    End Sub

    ''' <summary>
    ''' 加载点位信息以及速度，步进和点位保存等信息
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load_Pos()
        cbo_Pos1.Items.Clear()
        cbo_Pos1.Items.Add("待机位置") '0
        cbo_Pos1.Items.Add("镭射打点位置")  '1
        cbo_Pos1.Items.Add("镭射  XY校正位置") '2
        cbo_Pos1.Items.Add("镭射  Z校正位置") '3
        cbo_Pos1.Items.Add("胶针1 点胶位置")    '4
        cbo_Pos1.Items.Add("胶针1 擦针头位置")  '5
        cbo_Pos1.Items.Add("胶针1 光纤校针位置")   '6
        cbo_Pos1.Items.Add("胶针1 XY轴校正位置") '7
        cbo_Pos1.Items.Add("胶针1 Z轴校正位置") '8
        cbo_Pos1.Items.Add("胶针2 点胶位置")    '9
        cbo_Pos1.Items.Add("胶针2 擦针头位置")  '10
        cbo_Pos1.Items.Add("胶针2 光纤校针位置")   '11
        cbo_Pos1.Items.Add("胶针2 XY轴校正位置") '12
        cbo_Pos1.Items.Add("胶针2 Z轴校正位置") '13

        cbo_Pos2.Items.Clear()
        cbo_Pos2.Items.Add("待机位置")
        cbo_Pos2.Items.Add("取料位置")
        cbo_Pos2.Items.Add("定位拍照位置")
        cbo_Pos2.Items.Add("精补第一颗料位置")
        cbo_Pos2.Items.Add("贴合第一颗料位置")
        cbo_Pos2.Items.Add("针头Z轴校正位置")
        cbo_Pos2.Items.Add("抛料位置")

        cbo_Pos3.Items.Clear()
        cbo_Pos3.Items.Add("待机位置") '0
        cbo_Pos3.Items.Add("取Tray盘第一颗料位置") '1
        cbo_Pos3.Items.Add("拍Tray盘第一颗料位置")   '2
        cbo_Pos3.Items.Add("扫Tray盘第一颗料条码位置")  '3
        cbo_Pos3.Items.Add("夹镜头保护盖位置") '4
        cbo_Pos3.Items.Add("压力传感器自动标定位置1") '5
        cbo_Pos3.Items.Add("压力传感器自动标定位置2") '6

        cbo_Pos4.Items.Clear()
        cbo_Pos4.Items.Add("初始位置")
        cbo_Pos4.Items.Add("第一颗料精补位置")

        cbo_Pos5.Items.Clear()
        cbo_Pos5.Items.Add("初始位置")
        cbo_Pos5.Items.Add("第一颗料复检位置")

        cbo_Pos6.Items.Clear()
        cbo_Pos6.Items.Add("初始位置")
        cbo_Pos6.Items.Add("第一颗料固化位置")

        cbo_Pos7.Items.Clear()
        cbo_Pos7.Items.Add("Z1，Z2初始位置")
        cbo_Pos7.Items.Add("Z1料盘满盘位置")
        cbo_Pos7.Items.Add("Z1料盘单盘位置")
        cbo_Pos7.Items.Add("Z2料盘满盘位置")
        cbo_Pos7.Items.Add("Z2料盘单盘位置")

        cbo_Pos1.SelectedIndex = 0
        cbo_Pos2.SelectedIndex = 0
        cbo_Pos3.SelectedIndex = 0
        cbo_Pos4.SelectedIndex = 0
        cbo_Pos5.SelectedIndex = 0
        cbo_Pos6.SelectedIndex = 0
        cbo_Pos7.SelectedIndex = 0

        radGet1.Checked = True
        radGet2.Checked = True
        radGet3.Checked = True
        radGet4.Checked = True
        radGet5.Checked = True
        radGet6.Checked = True
        radGet7.Checked = True

        radStepS2_0.Checked = True
        radVelS2_0.Checked = True

        txtStepS2.Text = 0.1
        txtVelS2.Text = 1

    End Sub

    ''' <summary>
    ''' 加载设备当前工作状态
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load_CurWorkState()

        '**********工工位是否工作颜色提示******

        Timer_Display.Enabled = True
        '如果初始化完成，初始化按钮显示初始化完成，并且disable
        If Flag_MachineInit Then
            Btn_initialize.Text = "初始化完成"
            Btn_initialize.Enabled = False
            Btn_initialize.BZ_Color = Color_Selected
        Else
            Btn_initialize.Text = "初始化"
            Btn_initialize.Enabled = True
            Btn_initialize.BZ_Color = Color_Unselected
        End If

        '如果进入自动运行，自动运行按钮颜色提示，并且disable
        If Flag_MachineAutoRun Then
            Btn_AutoRun.Text = "自动运行中"
            Btn_AutoRun.Enabled = False
            Btn_AutoRun.BZ_Color = Color_Selected
        Else
            Btn_AutoRun.Text = "自动运行"
            Btn_AutoRun.Enabled = True
            Btn_AutoRun.BZ_Color = Color_Unselected
        End If
    End Sub

    ''' <summary>
    ''' 为轴运动按钮按钮添加轴号
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AxisMove_Button_Load()
        btn_N1.Tag = 1
        btn_N2.Tag = 2
        btn_N3.Tag = 3
        btn_N4.Tag = 0
        btn_N5.Tag = 4
        btn_N6.Tag = 17
        btn_N7.Tag = 5
        btn_N8.Tag = 6
        btn_N9.Tag = 7
        btn_N10.Tag = 19
        btn_N11.Tag = 8
        btn_N12.Tag = 9
        btn_N13.Tag = 11
        btn_N14.Tag = 12
        btn_N15.Tag = 13
        btn_N16.Tag = 14
        btn_N17.Tag = 10
        btn_N18.Tag = 15
        btn_N19.Tag = 16

        btn_P1.Tag = 1
        btn_P2.Tag = 2
        btn_P3.Tag = 3
        btn_P4.Tag = 0
        btn_P5.Tag = 4
        btn_P6.Tag = 17
        btn_P7.Tag = 5
        btn_P8.Tag = 6
        btn_P9.Tag = 7
        btn_P10.Tag = 19
        btn_P11.Tag = 8
        btn_P12.Tag = 9
        btn_P13.Tag = 11
        btn_P14.Tag = 12
        btn_P15.Tag = 13
        btn_P16.Tag = 14
        btn_P17.Tag = 10
        btn_P18.Tag = 15
        btn_P19.Tag = 16

    End Sub

    Private Sub DataGridViewInit()
        With DataGridView1
            .ColumnCount = 14
            '.Name = "My_first_DataGridView"
            '.Size = New Size(900, 200)  '设置DataGridView控件大小
            .Columns(0).Name = Field0   '设置DataGridView控件表头内容
            .Columns(1).Name = Field1
            .Columns(2).Name = Field2
            .Columns(3).Name = Field3
            .Columns(4).Name = Field4
            .Columns(3).DefaultCellStyle.ForeColor = Color.Black

            .Columns(5).Name = Field5
            .Columns(6).Name = Field6
            .Columns(7).Name = Field7
            .Columns(8).Name = Field8
            .Columns(9).Name = Field9

            .Columns(10).Name = Field16
            .Columns(11).Name = Field17
            .Columns(12).Name = Field18
            .Columns(13).Name = Field19

            '设置DataGridView控件列内容排列方式
            .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
            '设置DataGridView控件列内字体形式
            .Columns(4).DefaultCellStyle.Font = _
                    New Font(Me.DataGridView1.DefaultCellStyle.Font, FontStyle.Regular)
            '设置DataGridView控件列选择模式
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            '设置DataGridView多行或多列选择
            .MultiSelect = False
            .Dock = DockStyle.None
            .Columns(0).Width = 80
            .Columns(1).Width = 40
            .Columns(2).Width = 140
            .Columns(3).Width = 160
            .Columns(4).Width = 60
            .Columns(5).Width = 60
            .Columns(6).Width = 60
            .Columns(7).Width = 60
            .Columns(8).Width = 60
            .Columns(9).Width = 60
            .Columns(10).Width = 60
            .Columns(11).Width = 60
            .Columns(12).Width = 60
            .Columns(13).Width = 60

            '设置DataGridView控件表头单元格内容排列方式
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            '.DataSource = adoRecordset
            '.RowHeadersVisible = False
            .RowHeadersWidth = 20
            .ColumnHeadersHeight = 20
            '.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            '.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .AllowUserToOrderColumns = False
            .AllowUserToDeleteRows = False

        End With

        For i = 0 To Me.DataGridView1.Columns.Count - 1
            DataGridView1.Columns(i).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        '*********************禁止datagridview点击排序列标题排序*******************************
        For i As Int32 = 0 To Me.DataGridView1.Columns.Count - 1
            Me.DataGridView1.Columns(i).SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub
#End Region

#Region "   窗体卸载"
    Private Sub Frm_Engineering_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Flag_FrmEngineeringOpned = False
    End Sub
#End Region

#Region "   功能：回原点定时器"
    ''' <summary>
    ''' 单轴回原点定时器
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Timer_GoHome_Tick(sender As Object, e As EventArgs) Handles Timer_GoHome.Tick
        Timer_GoHome.Enabled = False
        Call GoHome()
        Timer_GoHome.Enabled = True
    End Sub
#End Region

#Region "   刷新定时器"

    Private Sub Timer_Display_Tick(sender As Object, e As EventArgs) Handles Timer_Display.Tick
        Timer_Display.Enabled = False

        lbl_StepL1.Text = "L1:" & Step_Line(1)
        lbl_StepL2.Text = "L2:" & Step_Line(2)
        lbl_StepL3.Text = "L3:" & Step_Line(3)
        lbl_StepGlue.Text = "Glue:" & Step_Glue
        lbl_StepPaste.Text = "Paste:" & Step_Paste
        lbl_StepTaker.Text = "Taker:" & Step_PreTaker
        lbl_StepRecheck.Text = "Recheck:" & Step_Recheck

        lbl_PressPaste.Text = Format(Press(0), "0.00")
        lbl_PressTaker.Text = Format(Press(1), "0.00")
        lbl_PressStandard.Text = Format(Press(2), "0.00")

        ''读取点胶工作气压
        lbl_Pressure0.Text = Format(ReadPressure(0), "0.00")
        lbl_Pressure1.Text = Format(ReadPressure(1), "0.00")

        txt_CT.Text = Format(CycelTime / 1000, "0.00")

        Me.lbl_sts_Air.BackColor = IIf(EXI(2, 1), Color.Lime, Color.Transparent)    '正气源
        Me.lbl_sts_Vac.BackColor = IIf(EXI(2, 2), Color.Lime, Color.Transparent)    '负气源
        Me.lbl_sts_EMS.BackColor = IIf(IsSysEmcStop, Color.Lime, Color.Transparent) '急停
        Me.lbl_sts_CCD.BackColor = IIf(Frm_Main.Winsock1.CtlState = 7, Color.Lime, Color.Transparent) 'CCD
        Me.lbl_sts_BarReader.BackColor = IIf(Frm_Main.Winsock2.CtlState = 7, Color.Lime, Color.Transparent) '读码器
        Me.lbl_sts_Server.BackColor = IIf(Frm_Main.Winsock3.CtlState = 7, Color.Lime, Color.Transparent) '服务器
        Me.lbl_sts_PDCA.BackColor = IIf(Frm_Main.Winsock4.CtlState = 7, Color.Lime, Color.Transparent) 'PDCA
        Me.lbl_sts_loadcell1.BackColor = IIf(Frm_Main.COM1.IsOpen, Color.Lime, Color.Transparent) 'LoadCell 1
        Me.lbl_sts_loadcell2.BackColor = IIf(Frm_Main.COM3.IsOpen, Color.Lime, Color.Transparent) 'LoadCell 2
        Me.lbl_sts_loadcell3.BackColor = IIf(Frm_Main.COM4.IsOpen, Color.Lime, Color.Transparent) 'LoadCell 3
        Me.lbl_sts_Laser.BackColor = IIf(Frm_Main.COM2.IsOpen, Color.Lime, Color.Transparent) 'Laser
        Me.lbl_sts_Line.BackColor = IIf(Frm_Main.COM5.IsOpen, Color.Lime, Color.Transparent) '流水线步进电机控制器
        Me.lbl_sts_Safedoor.BackColor = IIf(EXI(0, 3) And EXI(0, 4), Color.Lime, Color.Transparent)    '安全门
        Me.lbl_sts_UV.BackColor = IIf(Flag_UVConnect(1), Color.Lime, Color.Transparent)    'UV 灯控制器

        '****************如果IO界面显示那么就刷新IO状态到界面，否则不刷新
        If TabControl1.SelectedIndex = 1 Or TabControl1.SelectedIndex = 2 Then Call IO_Controls_Display()
        '***************如果显示调试界面，那么就刷新调试界面的状态
        If TabControl1.SelectedIndex = 3 Then Call Debug_Display()

        For i = 0 To 11
            Tray1.Controls.Item(i).BackColor = IIf(Tray_Pallet(1).Hole(i).isProductOk, Color.Lime, Color.Red)
            Tray2.Controls.Item(i).BackColor = IIf(Tray_Pallet(2).Hole(i).isProductOk, Color.Lime, Color.Red)
            Tray3.Controls.Item(i).BackColor = IIf(Tray_Pallet(3).Hole(i).isProductOk, Color.Lime, Color.Red)
        Next

        Timer_Display.Enabled = True
    End Sub

    ''' <summary>
    ''' IO 显示到IO界面
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub IO_Controls_Display()
        '板卡端子板IO
        Dim i, n As Integer
        For n = 0 To GTS_CardNum - 1
            For i = 0 To 15
                Select Case n
                    Case 0      '控制卡0
                        If EXI(n, i) Then
                            TableLayoutPanel2.Controls.Item(i).BackColor = Color.GreenYellow
                        Else
                            TableLayoutPanel2.Controls.Item(i).BackColor = Color.White
                        End If

                        If EXO(n, i) Then
                            TableLayoutPanel7.Controls.Item(i).BackColor = Color.Lime
                        Else
                            TableLayoutPanel7.Controls.Item(i).BackColor = Color.Transparent
                        End If
                        '刷新原点极限和报警信号
                        If 0 < i And i <= GTS_AxisNum(n) Then
                            If Home(n, i) Then
                                TableLayoutPanel1.Controls.Item(i - 1).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i - 1).BackColor = Color.White
                            End If

                            If LimitP(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 29).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 29).BackColor = Color.White
                            End If

                            If LimitN(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 59).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 59).BackColor = Color.White
                            End If

                            If ServoErr(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 89).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 89).BackColor = Color.White
                            End If

                        End If

                    Case 1
                        If EXI(n, i) Then
                            TableLayoutPanel3.Controls.Item(i).BackColor = Color.GreenYellow
                        Else
                            TableLayoutPanel3.Controls.Item(i).BackColor = Color.White
                        End If

                        If EXO(n, i) Then
                            TableLayoutPanel8.Controls.Item(i).BackColor = Color.Lime
                        Else
                            TableLayoutPanel8.Controls.Item(i).BackColor = Color.Transparent
                        End If
                        '刷新原点极限和报警信号
                        If 0 < i And i <= GTS_AxisNum(n) Then
                            If Home(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + GTS_AxisNum(0) - 1).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + GTS_AxisNum(0) - 1).BackColor = Color.White
                            End If

                            If LimitP(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 29 + GTS_AxisNum(0)).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 29 + GTS_AxisNum(0)).BackColor = Color.White
                            End If

                            If LimitN(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 59 + GTS_AxisNum(0)).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 59 + GTS_AxisNum(0)).BackColor = Color.White
                            End If

                            If ServoErr(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 89 + GTS_AxisNum(0)).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 89 + GTS_AxisNum(0)).BackColor = Color.White
                            End If

                        End If
                    Case 2
                        If EXI(n, i) Then
                            TableLayoutPanel4.Controls.Item(i).BackColor = Color.GreenYellow
                        Else
                            TableLayoutPanel4.Controls.Item(i).BackColor = Color.White
                        End If

                        If EXO(n, i) Then
                            TableLayoutPanel9.Controls.Item(i).BackColor = Color.Lime
                        Else
                            TableLayoutPanel9.Controls.Item(i).BackColor = Color.Transparent
                        End If
                        '刷新原点极限和报警信号
                        If 0 < i And i <= GTS_AxisNum(n) Then
                            If Home(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + GTS_AxisNum(0) + GTS_AxisNum(1) - 1).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + GTS_AxisNum(0) + GTS_AxisNum(1) - 1).BackColor = Color.White
                            End If

                            If LimitP(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 29 + GTS_AxisNum(0) + GTS_AxisNum(1)).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 29 + GTS_AxisNum(0) + GTS_AxisNum(1)).BackColor = Color.White
                            End If

                            If LimitN(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 59 + GTS_AxisNum(0) + GTS_AxisNum(1)).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 59 + GTS_AxisNum(0) + GTS_AxisNum(1)).BackColor = Color.White
                            End If

                            If ServoErr(n, i) Then
                                TableLayoutPanel1.Controls.Item(i + 89 + GTS_AxisNum(0) + GTS_AxisNum(1)).BackColor = Color.Tomato
                            Else
                                TableLayoutPanel1.Controls.Item(i + 89 + GTS_AxisNum(0) + GTS_AxisNum(1)).BackColor = Color.White
                            End If

                        End If
                    Case 3

                    Case 4
                End Select
            Next
        Next

        '***************    扩展模块IO状态刷新值IO显示界面   ******************************
        For i = 0 To 15
            If EMI(0, i) Then
                TableLayoutPanel5.Controls.Item(i).BackColor = Color.GreenYellow
            Else
                TableLayoutPanel5.Controls.Item(i).BackColor = Color.White
            End If

            If EMO(0, i) Then
                TableLayoutPanel10.Controls.Item(i).BackColor = Color.Lime
            Else
                TableLayoutPanel10.Controls.Item(i).BackColor = Color.Transparent
            End If

            If EMI(1, i) Then
                TableLayoutPanel6.Controls.Item(i).BackColor = Color.GreenYellow
            Else
                TableLayoutPanel6.Controls.Item(i).BackColor = Color.White
            End If

            If EMO(1, i) Then
                TableLayoutPanel11.Controls.Item(i).BackColor = Color.Lime
            Else
                TableLayoutPanel11.Controls.Item(i).BackColor = Color.Transparent
            End If

        Next
        '***************    扩展模块IO状态刷新值IO显示界面   ******************************

    End Sub

    Public Sub Enable_SavePosition()
        '允许保存点位选择框操作
        If chk_En1.Checked Then
            btn_SavePos1.Enabled = True
        Else
            btn_SavePos1.Enabled = False
        End If
        '允许保存点位选择框操作
        If chk_En2.Checked Then
            btn_SavePos2.Enabled = True
        Else
            btn_SavePos2.Enabled = False
        End If
        '允许保存点位选择框操作
        If chk_En3.Checked Then
            btn_SavePos3.Enabled = True
        Else
            btn_SavePos3.Enabled = False
        End If
        '允许保存点位选择框操作
        If chk_En4.Checked Then
            btn_SavePos4.Enabled = True
        Else
            btn_SavePos4.Enabled = False
        End If
        '允许保存点位选择框操作
        If chk_En5.Checked Then
            btn_SavePos5.Enabled = True
        Else
            btn_SavePos5.Enabled = False
        End If
        '允许保存点位选择框操作
        If chk_En6.Checked Then
            btn_SavePos6.Enabled = True
        Else
            btn_SavePos6.Enabled = False
        End If
        '允许保存点位选择框操作
        If chk_En7.Checked Then
            btn_SavePos7.Enabled = True
        Else
            btn_SavePos7.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' 手动调试界面刷新
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Debug_Display()
        '允许保存点位洗洗选择框发生变化
        Call Enable_SavePosition()

        If radGet1.Checked Then
            txt_Pos1.Text = Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).X
            txt_Pos2.Text = Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).Y
            txt_Pos3.Text = Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).Z
        End If
        If radGet2.Checked Then
            txt_Pos5.Text = Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).X
            txt_Pos6.Text = Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Y
            txt_Pos7.Text = Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Z
            txt_Pos8.Text = Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).R
        End If
        If radGet3.Checked Then
            txt_Pos9.Text = Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).X
            txt_Pos10.Text = Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Y
            txt_Pos11.Text = Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Z
            txt_Pos12.Text = Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).R
        End If
        If radGet4.Checked Then
            txt_Pos13.Text = Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).X
            txt_Pos14.Text = Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).Y
        End If
        If radGet5.Checked Then
            txt_Pos15.Text = Par_Pos.St_Recheck(cbo_Pos5.SelectedIndex).X
            txt_Pos16.Text = Par_Pos.St_Recheck(cbo_Pos5.SelectedIndex).Y
        End If
        If radGet6.Checked Then
            txt_Pos17.Text = Par_Pos.St_Cure(cbo_Pos6.SelectedIndex).X
        End If
        If radGet7.Checked Then
            txt_Pos18.Text = Par_Pos.St_Feed(cbo_Pos7.SelectedIndex).Z
            txt_Pos19.Text = Par_Pos.St_Recycle(cbo_Pos7.SelectedIndex).Z
        End If

        '*********刷新轴规划和轴编码器位置*******************
        For n = 0 To GTS_CardNum - 1
            For i = 1 To GTS_AxisNum(n)
                lbl_EncPos(n, i).Text = CurrEncPos(n, i)
                lbl_PrfPos(n, i).Text = CurrPrfPos(n, i)
            Next
        Next

        '刷新伺服使能状态
        For n = 0 To GTS_CardNum - 1
            For i = 1 To GTS_AxisNum(n)
                btn_Servo(n, i).BackColor = IIf(ServoOn(n, i), Color.Lime, Color.Transparent)
            Next
        Next

        '刷新回原点OK信号
        For n = 0 To GTS_CardNum - 1
            For i = 1 To GTS_AxisNum(n)
                btn_Home(n, i).BackColor = IIf(AxisHome(n, i).Result, Color.Lime, Color.Transparent)
            Next
        Next

        Me.lbl_Blocked0.BackColor = IIf(EXI(1, 0), Color.Lime, Color.Transparent)   'L0阻挡气缸磁簧
        Me.lbl_NoBlock0.BackColor = IIf(EXI(1, 1), Color.Lime, Color.Transparent)   'L0阻挡气缸磁簧
        Me.lbl_Blocked1.BackColor = IIf(EXI(1, 2), Color.Lime, Color.Transparent)   'L1阻挡气缸磁簧
        Me.lbl_NoBlock1.BackColor = IIf(EXI(1, 3), Color.Lime, Color.Transparent)   'L1阻挡气缸磁簧
        Me.lbl_Blocked2.BackColor = IIf(EXI(1, 4), Color.Lime, Color.Transparent)   'L2阻挡气缸磁簧
        Me.lbl_NoBlock2.BackColor = IIf(EXI(1, 5), Color.Lime, Color.Transparent)   'L2阻挡气缸磁簧
        Me.lbl_Blocked3.BackColor = IIf(EXI(1, 6), Color.Lime, Color.Transparent)   'L3阻挡气缸磁簧
        Me.lbl_NoBlock3.BackColor = IIf(EXI(1, 7), Color.Lime, Color.Transparent)   'L3阻挡气缸磁簧
        Me.lbl_Rised1.BackColor = IIf(EXI(1, 8), Color.Lime, Color.Transparent)   'L1顶升气缸磁簧
        Me.lbl_Down1.BackColor = IIf(EXI(1, 9), Color.Lime, Color.Transparent)   'L1顶升气缸磁簧
        Me.lbl_Rised2.BackColor = IIf(EXI(1, 10), Color.Lime, Color.Transparent)   'L2顶升气缸磁簧
        Me.lbl_Down2.BackColor = IIf(EXI(1, 11), Color.Lime, Color.Transparent)   'L2顶升气缸磁簧
        Me.lbl_Rised3.BackColor = IIf(EXI(1, 12), Color.Lime, Color.Transparent)   'L3顶升气缸磁簧
        Me.lbl_Down3.BackColor = IIf(EXI(1, 13), Color.Lime, Color.Transparent)   'L3顶升气缸磁簧
        Me.lbl_Vac1.BackColor = IIf(EMI(1, 8), Color.Lime, Color.Transparent)  '1短流水线线吸载具真空负压信号
        Me.lbl_Vac2.BackColor = IIf(EMI(1, 9), Color.Lime, Color.Transparent)  '2短流水线线吸载具真空负压信号
        Me.lbl_Vac3.BackColor = IIf(EMI(1, 10), Color.Lime, Color.Transparent)  '3短流水线线吸载具真空负压信号
    End Sub

    ''' <summary>
    ''' 操作界面变换
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 2 Or TabControl1.SelectedIndex = 3 Then
            If Flag_MachineAutoRun Or Flag_MachinePause Then
                Frm_DialogAddMessage("设备自动运行过程中禁止进入调试界面和IO输出界面！")
                TabControl1.SelectedIndex = 0
                Exit Sub
            End If
            If Flag_MachineInitOngoing Then
                Frm_DialogAddMessage("设备初始化过程中禁止进入调试界面和IO输出界面！")
                TabControl1.SelectedIndex = 0
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "   Machine Operation"
    ''' <summary>
    ''' 设备初始化按钮鼠标按下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Btn_initialize_MouseDown(sender As Object, e As MouseEventArgs) Handles Btn_initialize.MouseDown
        Call Machine_Init()
    End Sub

    ''' <summary>
    ''' 自动运行单击
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Btn_AutoRun_Click(sender As Object, e As EventArgs) Handles Btn_AutoRun.Click
        'Call Machine_AutoRun()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged

        Try
            TextBox1.Text = ListBox1.Items(ListBox1.SelectedIndex).ToString()
        Catch ex As Exception
            'Timer1.Enabled = False
        End Try

    End Sub

    ''' <summary>
    ''' 卡0输出触发
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Out_0_Click(sender As Object, e As EventArgs) Handles btn_Out_0_00.Click, btn_Out_0_01.Click, btn_Out_0_02.Click, btn_Out_0_03.Click, _
        btn_Out_0_04.Click, btn_Out_0_05.Click, btn_Out_0_06.Click, btn_Out_0_07.Click, btn_Out_0_08.Click, btn_Out_0_09.Click, btn_Out_0_10.Click, _
        btn_Out_0_11.Click, btn_Out_0_12.Click, btn_Out_0_13.Click, btn_Out_0_14.Click, btn_Out_0_15.Click
        If EXO(0, sender.tag) Then
            Call SetEXO(0, sender.tag, False)
        Else
            Call SetEXO(0, sender.tag, True)
        End If
    End Sub

    ''' <summary>
    ''' 卡1输出触发
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Out_1_Click(sender As Object, e As EventArgs) Handles btn_Out_1_00.Click, btn_Out_1_01.Click, btn_Out_1_02.Click, btn_Out_1_03.Click, _
    btn_Out_1_04.Click, btn_Out_1_05.Click, btn_Out_1_06.Click, btn_Out_1_07.Click, btn_Out_1_08.Click, btn_Out_1_09.Click, btn_Out_1_10.Click, _
    btn_Out_1_11.Click, btn_Out_1_12.Click, btn_Out_1_13.Click, btn_Out_1_14.Click, btn_Out_1_15.Click
        If EXO(1, sender.tag) Then
            Call SetEXO(1, sender.tag, False)
        Else
            Call SetEXO(1, sender.tag, True)
        End If
    End Sub

    ''' <summary>
    ''' 卡2输出触发
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Out_3_Click(sender As Object, e As EventArgs) Handles btn_Out_3_00.Click, btn_Out_3_01.Click, btn_Out_3_02.Click, btn_Out_3_03.Click, _
    btn_Out_3_04.Click, btn_Out_3_05.Click, btn_Out_3_06.Click, btn_Out_3_07.Click, btn_Out_3_08.Click, btn_Out_3_09.Click, btn_Out_3_10.Click, _
    btn_Out_3_11.Click, btn_Out_3_12.Click, btn_Out_3_13.Click, btn_Out_3_14.Click, btn_Out_3_15.Click
        If EXO(2, sender.tag) Then
            Call SetEXO(2, sender.tag, False)
        Else
            Call SetEXO(2, sender.tag, True)
        End If
    End Sub

    ''' <summary>
    ''' 扩展模块0输出触发
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Out_4_Click(sender As Object, e As EventArgs) Handles btn_Out_4_00.Click, btn_Out_4_01.Click, btn_Out_4_02.Click, btn_Out_4_03.Click, _
    btn_Out_4_04.Click, btn_Out_4_05.Click, btn_Out_4_06.Click, btn_Out_4_07.Click, btn_Out_4_08.Click, btn_Out_4_09.Click, btn_Out_4_10.Click, _
    btn_Out_4_11.Click, btn_Out_4_12.Click, btn_Out_4_13.Click, btn_Out_4_14.Click, btn_Out_4_15.Click
        If EMO(0, sender.tag) Then
            Call SetEMO(0, sender.tag, False)
        Else
            Call SetEMO(0, sender.tag, True)
        End If
    End Sub

    ''' <summary>
    ''' 扩展模块1输出触发
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Out_5_00_Click(sender As Object, e As EventArgs) Handles btn_Out_5_00.Click, btn_Out_5_01.Click, btn_Out_5_02.Click, btn_Out_5_03.Click, _
    btn_Out_5_04.Click, btn_Out_5_05.Click, btn_Out_5_06.Click, btn_Out_5_07.Click, btn_Out_5_08.Click, btn_Out_5_09.Click, btn_Out_5_10.Click, _
    btn_Out_5_11.Click, btn_Out_5_12.Click, btn_Out_5_13.Click, btn_Out_5_14.Click, btn_Out_5_15.Click
        If EMO(1, sender.tag) Then
            Call SetEMO(1, sender.tag, False)
        Else
            Call SetEMO(1, sender.tag, True)
        End If
    End Sub

    ''' <summary>
    ''' 单击logo截图
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lbl_BOZHON_Click(sender As Object, e As EventArgs) Handles lbl_BOZHON.Click
        ScreenCut("E:\BZ-Data\ScreenCut\")
    End Sub

#End Region

#Region "   功能：伺服使能，回原点，运动到指定位置"

    ''' <summary>
    ''' Servo On
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Btn_ServoON_Click(sender As Object, e As EventArgs) Handles btn_Servo1.Click, btn_Servo2.Click, btn_Servo3.Click, btn_Servo4.Click, _
        btn_Servo5.Click, btn_Servo6.Click, btn_Servo7.Click, btn_Servo8.Click, _
        btn_Servo9.Click, btn_Servo10.Click, btn_Servo11.Click, btn_Servo12.Click, _
        btn_Servo13.Click, btn_Servo14.Click, btn_Servo15.Click, btn_Servo16.Click, _
        btn_Servo17.Click, btn_Servo18.Click, btn_Servo19.Click
        Dim rtn As Short
        Dim temp As Long

        Select Case sender.tag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                If ServoOn(0, sender.tag) Then  '判断当前轴伺服ON是否打开
                    rtn = GT_ClrSts(0, sender.tag, 1)  '清除当前轴报警标志
                    rtn = GT_AxisOff(0, sender.tag) '当前轴伺服OFF
                    Call Write_Log(AxisPar.axisName(0, sender.tag) & "轴使能关闭", Path_Log)
                Else
                    rtn = GT_ClrSts(0, sender.tag, 1)  '清除当前轴报警标志
                    rtn = GT_AxisOn(0, sender.tag) '当前轴伺服ON
                    rtn = GT_GetEncPos(0, sender.tag, temp, 1, 0) '读取0号卡各轴当前实际位置
                    rtn = GT_SetPrfPos(0, sender.tag, temp)    '伺服ON关闭则将实际位置同步到规划位置
                    rtn = GT_SynchAxisPos(0, 2 ^ (sender.tag - 1))              '将当前轴进行位置同步
                    Call Write_Log(AxisPar.axisName(0, sender.tag) & "轴使能", Path_Log)
                End If

            Case 9, 10, 11, 12, 13, 14, 15, 16
                If ServoOn(1, sender.tag - 8) Then  '判断当前轴伺服ON是否打开
                    rtn = GT_ClrSts(1, sender.tag - 8, 1)  '清除当前轴报警标志
                    rtn = GT_AxisOff(1, sender.tag - 8) '当前轴伺服OFF
                    Call Write_Log(AxisPar.axisName(1, sender.tag - 8) & "轴使能关闭", Path_Log)
                Else
                    rtn = GT_ClrSts(1, sender.tag - 8, 1)  '清除当前轴报警标志
                    rtn = GT_AxisOn(1, sender.tag - 8) '当前轴伺服ON
                    rtn = GT_GetEncPos(1, sender.tag - 8, temp, 1, 0) '读取0号卡各轴当前实际位置
                    rtn = GT_SetPrfPos(1, sender.tag - 8, temp)    '伺服ON关闭则将实际位置同步到规划位置
                    rtn = GT_SynchAxisPos(1, 2 ^ (sender.tag - 9))              '将当前轴进行位置同步
                    Call Write_Log(AxisPar.axisName(1, sender.tag - 8) & "轴使能", Path_Log)
                End If

            Case 17, 19
                '龙门使能
                If ServoOn(2, sender.tag - 16) Then  '判断当前轴伺服ON是否打开
                    rtn = GT_ClrSts(2, sender.tag - 16, 1)  '清除当前轴报警标志
                    rtn = GT_AxisOff(2, sender.tag - 8) '当前轴伺服OFF
                    Call Write_Log(AxisPar.axisName(2, sender.tag - 16) & "轴使能关闭", Path_Log)
                Else
                    rtn = GT_ClrSts(2, sender.tag - 16, 1)  '清除当前轴报警标志
                    rtn = GT_AxisOn(2, sender.tag - 16) '当前轴伺服ON
                    rtn = GT_GetEncPos(2, sender.tag - 16, temp, 1, 0) '读取0号卡各轴当前实际位置
                    rtn = GT_SetPrfPos(2, sender.tag - 16, temp)    '伺服ON关闭则将实际位置同步到规划位置
                    rtn = GT_SynchAxisPos(2, 2 ^ (sender.tag - 17))              '将当前轴进行位置同步
                    Call Write_Log(AxisPar.axisName(2, sender.tag - 16) & "轴使能", Path_Log)
                End If
        End Select

    End Sub

    ''' <summary>
    ''' Btn Home Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Btn_Home_Click(sender As Object, e As EventArgs) Handles btn_Home1.Click, btn_Home2.Click, btn_Home3.Click, btn_Home4.Click, _
        btn_Home5.Click, btn_Home6.Click, btn_Home7.Click, btn_Home8.Click, _
        btn_Home9.Click, btn_Home10.Click, btn_Home11.Click, btn_Home12.Click, _
        btn_Home13.Click, btn_Home14.Click, btn_Home15.Click, btn_Home16.Click, _
        btn_Home17.Click, btn_Home18.Click, btn_Home19.Click

        If IsSysEmcStop Then    '判断紧急停止是否按下
            MsgBox("紧急停止中，请先解除急停！", vbOKOnly, "警告提示")
            Exit Sub
        End If
        Select Case sender.tag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                Dim tmpAxis As Integer = sender.tag
                If ServoOn(0, sender.tag) = False Then   '判断当前轴伺服ON是否已打开
                    MsgBox("请先打开伺服ON！", vbOKOnly, "警告提示")
                    Exit Sub
                End If
                If isAxisMoving(0, sender.tag) Then  '判断当前轴是否正在运动中
                    MsgBox("正在运动中，请等待！", vbOKOnly, "警告提示")
                    Exit Sub
                End If

                If tmpAxis = 3 Then
                    If AxisHome(0, 1).Result = False Or AxisHome(0, 2).Result = False Then
                        MsgBox("请先将X轴、Y轴回零OK！", vbOKOnly, "警告提示")
                        Exit Sub
                    End If
                End If
                If tmpAxis = 6 Then
                    If AxisHome(0, 4).Result = False Or AxisHome(0, 5).Result = False Then
                        MsgBox("请先将X轴、Y轴回零OK！", vbOKOnly, "警告提示")
                        Exit Sub
                    End If
                End If

                If MsgBox(sender.tag & "准备回点复归，确认不会撞机？", vbOKCancel) <> vbOK Then '判断是否确认退出主窗体
                    Exit Sub
                End If

                Timer_GoHome.Enabled = True
                If AxisHome(0, sender.tag).State = False Then '判断当前轴是否在回原点中
                    AxisHome(0, sender.tag).Enable = True '当前轴回原点使能
                End If
            Case 9, 10, 11, 12, 13, 14, 15
                Dim tmpAxis As Integer = sender.tag - 8

                If ServoOn(1, sender.tag - 8) = False Then   '判断当前轴伺服ON是否已打开
                    MsgBox("请先打开伺服ON！", vbOKOnly, "警告提示")
                    Exit Sub
                End If
                If isAxisMoving(1, sender.tag - 8) Then  '判断当前轴是否正在运动中
                    MsgBox("正在运动中，请等待！", vbOKOnly, "警告提示")
                    Exit Sub
                End If

                If tmpAxis = 4 And EXI(1, 7) = False And MACTYPE <> "PAM-B" Then
                    List_DebugAddMessage("请检查3工位UV灯升降气缸是否在缩回位置！")
                    Exit Sub
                End If

                If MsgBox(sender.tag & "准备回点复归，确认不会撞机？", vbOKCancel) <> vbOK Then '判断是否确认退出主窗体
                    Exit Sub
                End If

                Timer_GoHome.Enabled = True
                If AxisHome(1, sender.tag - 8).State = False Then '判断当前轴是否在回原点中
                    AxisHome(1, sender.tag - 8).Enable = True '当前轴回原点使能
                End If
        End Select
    End Sub

    ''' <summary>
    ''' 定时器扫描各轴回原点使能信号，如果使能就回原点
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GoHome()
        Dim card, axis As Short
        Dim isRotateAxis, en As Boolean

        For card = 0 To GTS_CardNum - 1
            For axis = 1 To GTS_AxisNum(card)
                If (card = 0 And axis = 6) Or (card = 1 And axis = 1) Then
                    isRotateAxis = True
                Else
                    isRotateAxis = False
                End If
                If AxisHome(card, axis).Enable Then
                    Call Motor_Home(card, axis, isRotateAxis)
                    If AxisHome(card, axis).State = False Then '判断当前轴回原点是否完成
                        If AxisHome(card, axis).Result Then '判断当前轴回原点是否成功
                            MsgBox(AxisPar.acc(card, axis) & "轴回原点成功！")
                        Else
                            MsgBox(AxisPar.acc(card, axis) & "轴回原点失败！")
                        End If
                    End If
                End If
            Next
        Next

        For card = 0 To GTS_CardNum - 1
            For axis = 1 To GTS_AxisNum(card)
                en = en Or AxisHome(card, axis).State
            Next
        Next

        If en = False Then
            Timer_GoHome.Enabled = False
        End If
    End Sub

    '点胶工位运动到选中的位置
    Private Sub btn_GoPos1_Click(sender As Object, e As EventArgs) Handles btn_GoPos1.Click
        If MessageBox.Show("点胶工位确定要运动到 " & Me.cbo_Pos1.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos1.SelectedItem.ToString & "，请等待……")
        Call GoPos_Glue(Me.cbo_Pos1.SelectedIndex)
    End Sub

    '组装贴合工站运动到指定位置
    Private Sub btn_GoPos2_Click(sender As Object, e As EventArgs) Handles btn_GoPos2.Click
        If MessageBox.Show("组装工位确定要运动到 " & Me.cbo_Pos2.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos2.SelectedItem.ToString & "，请等待……")
        Call GoPos_Paste(Me.cbo_Pos2.SelectedIndex)
    End Sub

    Private Sub btn_GoPos3_Click(sender As Object, e As EventArgs) Handles btn_GoPos3.Click
        If MessageBox.Show("取料工位确定要运动到 " & Me.cbo_Pos3.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos3.SelectedItem.ToString & "，请等待……")
        Call GoPos_PreTaker(Me.cbo_Pos3.SelectedIndex)
    End Sub

    Private Sub btn_GoPos4_Click(sender As Object, e As EventArgs) Handles btn_GoPos4.Click
        If MessageBox.Show("精确补偿工位确定要运动到 " & Me.cbo_Pos4.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos4.SelectedItem.ToString & "，请等待……")
        Call GoPos_FineCompensation(Me.cbo_Pos4.SelectedIndex)
    End Sub

    Private Sub btn_GoPos5_Click(sender As Object, e As EventArgs) Handles btn_GoPos5.Click
        If MessageBox.Show("复检工位确定要运动到 " & Me.cbo_Pos5.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos5.SelectedItem.ToString & "，请等待……")
        Call GoPos_Recheck(Me.cbo_Pos5.SelectedIndex)
    End Sub

    Private Sub btn_GoPos6_Click(sender As Object, e As EventArgs) Handles btn_GoPos6.Click
        If MessageBox.Show("预固化工位确定要运动到 " & Me.cbo_Pos6.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos6.SelectedItem.ToString & "，请等待……")
        Call GoPos_Cure(Me.cbo_Pos6.SelectedIndex)
    End Sub

    Private Sub btn_GoPos7_Click(sender As Object, e As EventArgs) Handles btn_GoPos7.Click
        If MessageBox.Show("料盘工位确定要运动到 " & Me.cbo_Pos7.SelectedItem.ToString & " ?", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("运动到" & Me.cbo_Pos7.SelectedItem.ToString & "，请等待……")
        Call GoPos_Feed(Me.cbo_Pos7.SelectedIndex)
    End Sub

#End Region

#Region "   功能:点位信息保存"
    'Save Glue Station Position
    Private Sub btn_SavePos1_Click(sender As Object, e As EventArgs) Handles btn_SavePos1.Click
        If MessageBox.Show("确定要保存点胶工位 " & cbo_Pos1.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet1.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).X = CurrEncPos(0, 1)
            Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).Y = CurrEncPos(0, 2)
            Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).Z = CurrEncPos(0, 3)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).X = Val(txt_Pos1.Text)
            Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).Y = Val(txt_Pos2.Text)
            Par_Pos.St_Glue(cbo_Pos1.SelectedIndex).Z = Val(txt_Pos3.Text)
        End If

        '更改自动校针位置、镭射Z轴校正位置和针头Z轴位置校正后需要重新校针
        '更改后把自动校针完成标志位置为False
        Select Case cbo_Pos1.SelectedIndex
            Case 2
                Par_Pos.Needle_NeedCalibration(0) = False
            Case 3
            Case 4
        End Select

        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(1, "1工位 " & cbo_Pos1.SelectedItem.ToString & " 点位保存成功", Path_Log)
        Select Case cbo_Pos1.SelectedIndex
            Case 14, 15
                '//获取镭射和针头位置之间X，Y的距离
                'Call Get_NL()
        End Select

        chk_En1.Checked = False
        radGet1.Checked = True
    End Sub

    'Paste工位点位信息保存
    Private Sub btn_SavePos2_Click(sender As Object, e As EventArgs) Handles btn_SavePos2.Click
        If MessageBox.Show("确定要保存组装工位 " & cbo_Pos2.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet2.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).X = CurrEncPos(0, 4)
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Y = CurrEncPos(2, 1)
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Z = CurrEncPos(0, 5)
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).R = CurrEncPos(0, 6)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).X = Val(txt_Pos5.Text)
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Y = Val(txt_Pos6.Text)
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Z = Val(txt_Pos7.Text)
            Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).R = Val(txt_Pos8.Text)
        End If
        Par_Pos.St_Paste(cbo_Pos2.SelectedIndex).Name = cbo_Pos2.SelectedItem.ToString
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(2, "2工位 " & cbo_Pos2.SelectedItem.ToString & " 点位保存成功", Path_Log)

        chk_En2.Checked = False
        radGet2.Checked = True
    End Sub

    '取料工位点位信息保存
    Private Sub btn_SavePos3_Click(sender As Object, e As EventArgs) Handles btn_SavePos3.Click
        If MessageBox.Show("确定要保存取料工位 " & cbo_Pos3.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet3.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).X = CurrEncPos(0, 7)
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Y = CurrEncPos(2, 3)
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Z = CurrEncPos(0, 8)
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).R = CurrEncPos(1, 1)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).X = Val(txt_Pos9.Text)
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Y = Val(txt_Pos10.Text)
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Z = Val(txt_Pos11.Text)
            Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).R = Val(txt_Pos12.Text)
        End If
        Par_Pos.St_PreTaker(cbo_Pos3.SelectedIndex).Name = cbo_Pos3.SelectedItem.ToString
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(3, "3工位 " & cbo_Pos3.SelectedItem.ToString & " 点位保存成功", Path_Log)

        chk_En3.Checked = False
        radGet3.Checked = True
    End Sub

    'Fine Compensation
    Private Sub btn_SavePos4_Click(sender As Object, e As EventArgs) Handles btn_SavePos4.Click
        If MessageBox.Show("确定要保存精补工位 " & cbo_Pos4.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet4.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).X = CurrEncPos(1, 3)
            Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).Y = CurrEncPos(1, 4)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).X = Val(txt_Pos13.Text)
            Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).Y = Val(txt_Pos14.Text)
        End If
        Par_Pos.St_FineCompensation(cbo_Pos4.SelectedIndex).Name = cbo_Pos4.SelectedItem.ToString
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(4, "4工位 " & cbo_Pos4.SelectedItem.ToString & " 点位保存成功", Path_Log)

        chk_En4.Checked = False
        radGet4.Checked = True
    End Sub

    'Recheck station
    Private Sub btn_SavePos5_Click(sender As Object, e As EventArgs) Handles btn_SavePos5.Click
        If MessageBox.Show("确定要保存复检工位 " & cbo_Pos5.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet5.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_Recheck(cbo_Pos5.SelectedIndex).X = CurrEncPos(1, 5)
            Par_Pos.St_Recheck(cbo_Pos5.SelectedIndex).Y = CurrEncPos(1, 6)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_Recheck(cbo_Pos5.SelectedIndex).X = Val(txt_Pos15.Text)
            Par_Pos.St_Recheck(cbo_Pos5.SelectedIndex).Y = Val(txt_Pos16.Text)
        End If
        Par_Pos.St_FineCompensation(cbo_Pos5.SelectedIndex).Name = cbo_Pos5.SelectedItem.ToString
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(5, "5工位 " & cbo_Pos5.SelectedItem.ToString & " 点位保存成功", Path_Log)

        chk_En5.Checked = False
        radGet5.Checked = True
    End Sub

    'cure station
    Private Sub btn_SavePos6_Click(sender As Object, e As EventArgs) Handles btn_SavePos6.Click
        If MessageBox.Show("确定要保存预固化工位 " & cbo_Pos6.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet6.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_Cure(cbo_Pos6.SelectedIndex).X = CurrEncPos(1, 2)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_Cure(cbo_Pos6.SelectedIndex).X = Val(txt_Pos17.Text)
        End If
        Par_Pos.St_FineCompensation(cbo_Pos6.SelectedIndex).Name = cbo_Pos6.SelectedItem.ToString
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(6, "6工位 " & cbo_Pos6.SelectedItem.ToString & " 点位保存成功", Path_Log)

        chk_En6.Checked = False
        radGet6.Checked = True
    End Sub

    '供料 station
    Private Sub btn_SavePos7_Click(sender As Object, e As EventArgs) Handles btn_SavePos7.Click
        If MessageBox.Show("确定要保存供料工位 " & cbo_Pos7.SelectedItem.ToString & " 点位？", "Tips", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If

        If radGet7.Checked Then
            '获取轴的编码器位置
            Par_Pos.St_Feed(cbo_Pos7.SelectedIndex).Z = CurrEncPos(1, 7)
            Par_Pos.St_Recycle(cbo_Pos7.SelectedIndex).Z = CurrEncPos(1, 8)
        Else
            '获取手动输入的点位信息
            Par_Pos.St_Feed(cbo_Pos7.SelectedIndex).Z = Val(txt_Pos18.Text)
            Par_Pos.St_Recycle(cbo_Pos7.SelectedIndex).Z = Val(txt_Pos19.Text)
        End If
        Par_Pos.St_Feed(cbo_Pos7.SelectedIndex).Name = cbo_Pos7.SelectedItem.ToString
        Par_Pos.St_Recycle(cbo_Pos7.SelectedIndex).Name = cbo_Pos7.SelectedItem.ToString
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Write_Log(7, "7工位 " & cbo_Pos7.SelectedItem.ToString & " 点位保存成功", Path_Log)

        chk_En7.Checked = False
        radGet7.Checked = True
    End Sub

#End Region

#Region "   功能：镭射自动校正和自动校针"

#End Region

#Region "   功能：轴手动运动 Step模式和Jog模式"
    ''' <summary>
    ''' 轴手动正向运动
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Axis_PositiveMove(sender As Object, e As MouseEventArgs) Handles btn_P1.MouseDown, btn_P2.MouseDown, btn_P3.MouseDown, _
        btn_P4.MouseDown, btn_P5.MouseDown, btn_P6.MouseDown, btn_P7.MouseDown, btn_P8.MouseDown, _
        btn_P9.MouseDown, btn_P10.MouseDown, btn_P11.MouseDown, btn_P12.MouseDown, btn_P13.MouseDown, _
        btn_P14.MouseDown, btn_P15.MouseDown, btn_P16.MouseDown, btn_P17.MouseDown, btn_P18.MouseDown, _
        btn_P19.MouseDown

        Select Case sender.tag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                '判断当前轴伺服ON是否已打开
                If ServoOn(0, sender.tag) = False Then
                    List_DebugAddMessage("请先打开" & AxisPar.axisName(0, sender.tag) & "轴的的伺服使能！")
                End If
                If isAxisMoving(0, sender.tag) Then  '判断当前轴是否在运动中
                    List_DebugAddMessage(AxisPar.axisName(0, sender.tag) & "正在运动中，请等待！")
                    Exit Sub
                End If

            Case 9, 10, 11, 12, 13, 14, 15, 16
                '判断当前轴伺服ON是否已打开
                If ServoOn(1, Val(sender.tag) - 8) = False Then
                    List_DebugAddMessage("请先打开" & AxisPar.axisName(1, sender.tag - 8) & "轴的的伺服使能！")
                End If
                If isAxisMoving(1, Val(sender.tag) - 8) Then  '判断当前轴是否在运动中
                    List_DebugAddMessage(AxisPar.axisName(1, sender.tag - 8) & "正在运动中，请等待！")
                    Exit Sub
                End If

            Case 17, 18, 19, 20
                '判断当前轴伺服ON是否已打开
                If ServoOn(2, Val(sender.tag) - 16) = False Then
                    List_DebugAddMessage("请先打开" & AxisPar.axisName(2, sender.tag - 16) & "轴的的伺服使能！")
                End If
                If isAxisMoving(2, Val(sender.tag) - 16) Then  '判断当前轴是否在运动中
                    List_DebugAddMessage(AxisPar.axisName(2, sender.tag - 16) & "正在运动中，请等待！")
                    Exit Sub
                End If
        End Select

        Select Case sender.tag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                If radStepS2_0.Checked = False Then              '判断选择的是否为STEP模式
                    Call StepMotion(0, sender.tag, CW, Val(txtVelS2.Text), Val(txtStepS2.Text))     'Step运动子程序，正向运动
                ElseIf radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    Call JogMotion(0, sender.tag, CW, Val(txtVelS2.Text))      'Jog运动子程序，正向运动
                End If

            Case 9, 10, 11, 12, 13, 14, 15, 16
                If radStepS2_0.Checked = False Then              '判断选择的是否为STEP模式
                    Call StepMotion(1, sender.tag - 8, CW, Val(txtVelS2.Text), Val(txtStepS2.Text))     'Step运动子程序，正向运动
                ElseIf radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    Call JogMotion(1, sender.tag - 8, CW, Val(txtVelS2.Text))      'Jog运动子程序，正向运动
                End If

            Case 17, 18, 19, 20
                If radStepS2_0.Checked = False Then              '判断选择的是否为STEP模式
                    Call StepMotion(2, sender.tag - 16, CW, Val(txtVelS2.Text), Val(txtStepS2.Text))     'Step运动子程序，正向运动
                ElseIf radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    Call JogMotion(2, sender.tag - 16, CW, Val(txtVelS2.Text))      'Jog运动子程序，正向运动
                End If
        End Select
    End Sub

    ''' <summary>
    ''' 轴正向运动按钮鼠标弹起
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_S2XP_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_P1.MouseUp, btn_P2.MouseUp, btn_P3.MouseUp, _
        btn_P4.MouseUp, btn_P5.MouseUp, btn_P6.MouseUp, btn_P7.MouseUp, btn_P8.MouseUp, _
        btn_P9.MouseUp, btn_P10.MouseUp, btn_P11.MouseUp, btn_P12.MouseUp, btn_P13.MouseUp, _
        btn_P14.MouseUp, btn_P15.MouseUp, btn_P16.MouseUp, btn_P17.MouseUp, btn_P18.MouseUp, _
        btn_P19.MouseUp

        Dim rtn As Short
        Dim i As Short
        i = sender.tag - 1

        Select Case i
            Case 0, 1, 2, 3, 4, 5, 6, 7
                If radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    rtn = GT_Stop(0, 2 ^ i, 2 ^ i)  '轴紧急停止
                End If

            Case 8, 9, 10, 11, 12, 13, 14, 15
                If radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    rtn = GT_Stop(1, 2 ^ (i - 8), 2 ^ (i - 8))  '轴紧急停止
                End If
            Case 16, 17, 18, 19
                If radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    rtn = GT_Stop(2, 2 ^ (i - 16), 2 ^ (i - 16))  '轴紧急停止
                End If
        End Select

    End Sub

    ''' <summary>
    ''' 轴负向移动(手动)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Axis_NegativeMove(sender As Object, e As EventArgs) Handles btn_N1.MouseDown, btn_N2.MouseDown, btn_N3.MouseDown, _
        btn_N4.MouseDown, btn_N5.MouseDown, btn_N6.MouseDown, btn_N7.MouseDown, btn_N8.MouseDown, _
        btn_N9.MouseDown, btn_N10.MouseDown, btn_N11.MouseDown, btn_N12.MouseDown, btn_N13.MouseDown, _
        btn_N14.MouseDown, btn_N15.MouseDown, btn_N16.MouseDown, btn_N17.MouseDown, btn_N18.MouseDown, _
        btn_N19.MouseDown

        Select Case sender.tag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                '判断当前轴伺服ON是否已打开
                If ServoOn(0, sender.tag) = False Then
                    List_DebugAddMessage("请先打开" & AxisPar.axisName(0, sender.tag) & "轴的的伺服使能！")
                End If
                If isAxisMoving(0, sender.tag) Then  '判断当前轴是否在运动中
                    List_DebugAddMessage(AxisPar.axisName(0, sender.tag) & "正在运动中，请等待！")
                    Exit Sub
                End If

            Case 9, 10, 11, 12, 13, 14, 15, 16
                '判断当前轴伺服ON是否已打开
                If ServoOn(1, Val(sender.tag) - 8) = False Then
                    List_DebugAddMessage("请先打开" & AxisPar.axisName(1, sender.tag - 8) & "轴的的伺服使能！")
                End If
                If isAxisMoving(1, Val(sender.tag) - 8) Then  '判断当前轴是否在运动中
                    List_DebugAddMessage(AxisPar.axisName(1, sender.tag - 8) & "正在运动中，请等待！")
                    Exit Sub
                End If
            Case 17, 18, 19, 20
                '判断当前轴伺服ON是否已打开
                If ServoOn(2, Val(sender.tag) - 16) = False Then
                    List_DebugAddMessage("请先打开" & AxisPar.axisName(2, sender.tag - 16) & "轴的的伺服使能！")
                End If
                If isAxisMoving(2, Val(sender.tag) - 16) Then  '判断当前轴是否在运动中
                    List_DebugAddMessage(AxisPar.axisName(2, sender.tag - 16) & "正在运动中，请等待！")
                    Exit Sub
                End If
        End Select

        Select Case sender.tag
            Case 1, 2, 3, 4, 5, 6, 7, 8
                If radStepS2_0.Checked = False Then              '判断选择的是否为STEP模式
                    Call StepMotion(0, sender.tag, CCW, Val(txtVelS2.Text), Val(txtStepS2.Text))     'Step运动子程序，正向运动
                ElseIf radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    Call JogMotion(0, sender.tag, CCW, Val(txtVelS2.Text))      'Jog运动子程序，正向运动
                End If

            Case 9, 10, 11, 12, 13, 14, 15, 16
                If radStepS2_0.Checked = False Then              '判断选择的是否为STEP模式
                    Call StepMotion(1, sender.tag - 8, CCW, Val(txtVelS2.Text), Val(txtStepS2.Text))     'Step运动子程序，正向运动
                ElseIf radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    Call JogMotion(1, sender.tag - 8, CCW, Val(txtVelS2.Text))      'Jog运动子程序，正向运动
                End If
            Case 17, 18, 19, 20
                If radStepS2_0.Checked = False Then              '判断选择的是否为STEP模式
                    Call StepMotion(2, sender.tag - 16, CCW, Val(txtVelS2.Text), Val(txtStepS2.Text))     'Step运动子程序，正向运动
                ElseIf radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    Call JogMotion(2, sender.tag - 16, CCW, Val(txtVelS2.Text))      'Jog运动子程序，正向运动
                End If

        End Select
    End Sub

    ''' <summary>
    ''' 轴手动运动按钮弹起
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_AxisMove_MouseUp(sender As Object, e As EventArgs) Handles btn_N1.MouseUp, btn_N2.MouseUp, btn_N3.MouseUp, _
        btn_N4.MouseUp, btn_N5.MouseUp, btn_N6.MouseUp, btn_N7.MouseUp, btn_N8.MouseUp, _
        btn_N9.MouseUp, btn_N10.MouseUp, btn_N11.MouseUp, btn_N12.MouseUp, btn_N13.MouseUp, _
        btn_N14.MouseUp, btn_N15.MouseUp, btn_N16.MouseUp, btn_N17.MouseUp, btn_N18.MouseUp, _
        btn_N19.MouseUp

        Dim i, rtn As Short
        i = sender.tag - 1

        Select Case i
            Case 0, 1, 2, 3, 4, 5, 6, 7
                If radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    rtn = GT_Stop(0, 2 ^ i, 2 ^ i)  '轴紧急停止
                End If

            Case 8, 9, 10, 11, 12, 13, 14, 15
                If radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    rtn = GT_Stop(1, 2 ^ (i - 8), 2 ^ (i - 8))  '轴紧急停止
                End If
            Case 16, 17, 18, 19
                If radStepS2_0.Checked Then           '判断选择的是否为JOG模式
                    rtn = GT_Stop(2, 2 ^ (i - 16), 2 ^ (i - 16))  '轴紧急停止
                End If
        End Select

    End Sub

#End Region

#Region "   功能：轴手动运动速度和步长选择"
    '2工位步进步长选择发生变化
    Private Sub radStepS2_0_CheckedChanged(sender As Object, e As EventArgs) Handles radStepS2_0.CheckedChanged, _
        radStepS2_1.CheckedChanged, radStepS2_2.CheckedChanged, radStepS2_3.CheckedChanged, radStepS2_4.CheckedChanged, _
        radStepS2_5.CheckedChanged

        If radStepS2_0.Checked Then
            txtStepS2.Enabled = False
            Exit Sub
        End If

        If radStepS2_1.Checked Then
            txtStepS2.Text = 0.01
            txtStepS2.Enabled = False
        End If

        If radStepS2_2.Checked Then
            txtStepS2.Text = 0.05
            txtStepS2.Enabled = False
        End If

        If radStepS2_3.Checked Then
            txtStepS2.Text = 0.1
            txtStepS2.Enabled = False
        End If

        If radStepS2_4.Checked Then
            txtStepS2.Text = 0.5
            txtStepS2.Enabled = False
        End If

        If radStepS2_5.Checked Then
            txtStepS2.Text = 0.1
            txtStepS2.Enabled = True
        End If

    End Sub

    '2工位速度选择发生变化
    Private Sub radVelS2_0_CheckedChanged(sender As Object, e As EventArgs) Handles radVelS2_0.CheckedChanged, _
        radVelS2_1.CheckedChanged, radVelS2_2.CheckedChanged, radVelS2_3.CheckedChanged, radVelS2_4.CheckedChanged, _
        radVelS2_5.CheckedChanged
        If radVelS2_0.Checked Then
            txtVelS2.Text = 1
            txtVelS2.Enabled = False
        End If
        If radVelS2_1.Checked Then
            txtVelS2.Text = 5
            txtVelS2.Enabled = False
        End If
        If radVelS2_2.Checked Then
            txtVelS2.Text = 10
            txtVelS2.Enabled = False
        End If
        If radVelS2_3.Checked Then
            txtVelS2.Text = 15
            txtVelS2.Enabled = False
        End If
        If radVelS2_4.Checked Then
            txtVelS2.Text = 20
            txtVelS2.Enabled = False
        End If
        If radVelS2_5.Checked Then
            txtVelS2.Text = 1
            txtVelS2.Enabled = True
        End If
    End Sub

#End Region

#Region "   功能：单工位镭射触发"
    'Laser Trigger
    Private Sub LaserTri_S2_Click(sender As Object, e As EventArgs) Handles LaserTri_S2.Click
        Dim Result As Integer
        If Com2_Send("M1" & vbCr) = False Then
            List_DebugAddMessage("触发镭射命令发送失败，请检查！")
            Exit Sub
        End If
        Do While True
            Result = Com2_Return() '等待COM2镭射触发结束
            If Result = 0 Then
                List_DebugAddMessage("镭射测距的值为：" & Format(COM2_Data(0), "0.0000"))
                Exit Do
            ElseIf Result = 1 Then
                List_DebugAddMessage("镭射COM端口未响应，请检查")
                Exit Do
            End If
            My.Application.DoEvents()
        Loop
    End Sub
#End Region

#Region "   功能:单工位相机手动触发"
    ''' <summary>
    ''' Trigger CCD
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_TriggerCCD_Click(sender As Object, e As EventArgs) Handles btn_T11.Click, btn_T21.Click, btn_T31.Click, _
        btn_T41.Click, btn_T42.Click, btn_T43.Click, btn_T51.Click, btn_T61.Click

        If CCD_Lock_Flag Then
            Frm_DialogAddMessage("触发CCD拍照异常，请检查！")
            Exit Sub
        End If

        If TriggerCCD(sender.text, Format(Now, "yyyyMMddHHmmss")) Then
            List_DebugAddMessage(sender.text)
        Else
            Frm_DialogAddMessage("触发CCD拍照异常，请检查！")
        End If

    End Sub
#End Region

#Region "   功能：自动标定"
    '压力传感器自动校正
    Private Sub btn_LoadCell_Ca_Click(sender As Object, e As EventArgs) Handles btn_LoadCell_Ca.Click
        'Call LoadCell_Calibration()
    End Sub

#End Region

#Region "   功能：单工位自动运行"
    '点胶站自动运行
    Private Sub btn_AutoRun_GlueStation_Click(sender As Object, e As EventArgs) Handles btn_AutoRun_GlueStation.Click
        If MessageBox.Show("确定要点胶站单站自动运行 ?", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("点胶站开始自动运行……")
        Call ManualRun_Glue()
    End Sub

    '组装站自动运行
    Private Sub btn_AutoRun_PasteStation_Click(sender As Object, e As EventArgs) Handles btn_AutoRun_PasteStation.Click
        If MessageBox.Show("确定要组装站单站自动运行 ?", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("组装站开始自动运行……")
        'Call ManualRun_Glue()
    End Sub

    '取料站站自动运行
    Private Sub btn_AutoRun_PreTakerStation_Click(sender As Object, e As EventArgs) Handles btn_AutoRun_PreTakerStation.Click
        If MessageBox.Show("确定要取料站单站自动运行 ?", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        List_DebugAddMessage("取料站开始自动运行……")
        'Call ManualRun_Glue()
    End Sub
#End Region

#Region "   功能：单工位调试界面IO输出"

    Private Sub btn_S2IO1_Click(sender As Object, e As EventArgs) Handles btn_S2IO1.Click, btn_S2IO2.Click, _
        btn_S2IO3.Click, btn_S2IO4.Click, btn_S2IO5.Click, btn_S2IO6.Click, _
        btn_S2IO7.Click, btn_S2IO8.Click

        Dim index As Integer
        index = sender.tag
        Select Case index
            Case 1
                '点胶1气缸升降电磁阀
                If EXO(2, 9) Then
                    SetEXO(2, 9, False)
                Else
                    SetEXO(2, 9, True)
                End If

            Case 2
                '点胶2气缸升降电磁阀
                If EXO(2, 11) Then
                    SetEXO(2, 11, False)
                Else
                    SetEXO(2, 11, True)
                End If
            Case 3
                'Stand By
            Case 4
                'Stand By
            Case 5
                '点胶1点胶
                If EXO(1, 13) Then
                    SetEXO(1, 13, False)
                Else
                    SetEXO(1, 13, True)
                End If
            Case 6
                '点胶2点胶
                If EXO(1, 14) Then
                    SetEXO(1, 14, False)
                Else
                    SetEXO(1, 14, True)
                End If
            Case 7
                'Stand By
            Case 8
                'Stand By
        End Select

    End Sub

    Private Sub btn_S3IO1_Click(sender As Object, e As EventArgs) Handles btn_S3IO1.Click, btn_S3IO2.Click, _
        btn_S3IO3.Click, btn_S3IO4.Click, btn_S3IO5.Click, btn_S3IO6.Click, _
        btn_S3IO7.Click, btn_S3IO8.Click
        Dim index As Integer
        index = sender.tag
        Select Case index
            Case 1
                '吸物料吸真空
                If EXO(0, 12) Then
                    SetEXO(0, 12, False)
                Else
                    SetEXO(0, 12, True)
                End If
            Case 2
                '取料吸排线吸真空
                If EXO(0, 8) Then
                    SetEXO(0, 8, False)
                Else
                    SetEXO(0, 8, True)
                End If
            Case 3
                '破真空
                If EXO(0, 13) Then
                    SetEXO(0, 13, False)
                Else
                    SetEXO(0, 13, True)
                End If
            Case 4
                '破真空
                If EXO(0, 9) Then
                    SetEXO(0, 9, False)
                Else
                    SetEXO(0, 9, True)
                End If

            Case 5
                '3工位UV灯打开
                If MACTYPE = "PAM-B" Then
                    If Flag_UVConnect(1) Then
                        Call UV_Open(ControllerHandle(1), 0, 255)
                    Else
                        Frm_DialogAddMessage("请检查UV灯连接是否正常！")
                    End If
                Else
                    If Flag_UVConnect(1) And Flag_UVConnect(4) Then
                        Call UV_Open(ControllerHandle(1), 0, 255)
                        Call UV_Open(ControllerHandle(4), 0, 255)
                    Else
                        Frm_DialogAddMessage("请检查UV灯连接是否正常！")
                    End If
                End If

            Case 6
                '3工位UV灯关闭

                If MACTYPE = "PAM-B" Then
                    If Flag_UVConnect(1) Then
                        Call UV_Close(ControllerHandle(1), 0)
                    Else
                        Frm_DialogAddMessage("请检查UV灯连接是否正常！")
                    End If
                Else
                    If Flag_UVConnect(1) And Flag_UVConnect(4) Then
                        Call UV_Close(ControllerHandle(1), 0)
                        Call UV_Close(ControllerHandle(4), 0)
                    Else
                        Frm_DialogAddMessage("请检查UV灯连接是否正常！")
                    End If
                End If
            Case 7

            Case 8

        End Select
    End Sub

    Private Sub btn_S4IO1_Click(sender As Object, e As EventArgs) Handles btn_S4IO1.Click, btn_S4IO2.Click, _
        btn_S4IO3.Click, btn_S4IO4.Click, btn_S4IO5.Click, btn_S4IO6.Click, _
        btn_S4IO7.Click, btn_S4IO8.Click

        Dim index As Integer
        index = sender.tag
        Select Case index
            Case 1
                If EXO(0, 14) Then
                    SetEXO(0, 14, False)
                Else
                    SetEXO(0, 14, True)
                End If
            Case 2
                If EXO(0, 10) Then
                    SetEXO(0, 10, False)
                Else
                    SetEXO(0, 10, True)
                End If
            Case 3
                If EXO(0, 15) Then
                    SetEXO(0, 15, False)
                Else
                    SetEXO(0, 15, True)
                End If
            Case 4
                If EXO(0, 11) Then
                    SetEXO(0, 11, False)
                Else
                    SetEXO(0, 11, True)
                End If
            Case 5
            Case 6
            Case 7
            Case 8
        End Select
    End Sub

    Private Sub btn_S5IO1_Click(sender As Object, e As EventArgs) Handles btn_S5IO1.Click, btn_S5IO2.Click, _
        btn_S5IO3.Click, btn_S5IO4.Click, btn_S5IO5.Click, btn_S5IO6.Click, btn_S5IO7.Click, btn_S5IO8.Click
        Dim index As Integer
        index = sender.tag
        Select Case index
            Case 1

            Case 2

            Case 3

            Case 4

            Case 5

            Case 6

            Case 7
                'Stand By

            Case 8
                'Stand By
        End Select
    End Sub

#End Region

#Region "   功能:工程界面日光灯控制"
    Private Sub Btn_LightClose_Click(sender As Object, e As EventArgs) Handles Btn_LightClose.Click
        If EXO(2, 4) = False Then
            Call SetEXO(2, 4, True)
            Btn_LightClose.Visible = False
            Btn_LightOpen.Visible = True
        End If
    End Sub

    Private Sub Btn_LightOpen_Click(sender As Object, e As EventArgs) Handles Btn_LightOpen.Click
        If EXO(2, 4) Then
            Call SetEXO(2, 4, False)
            Btn_LightClose.Visible = True
            Btn_LightOpen.Visible = False
        End If
    End Sub
#End Region

    Private Sub btn_ClrGlue_Click(sender As Object, e As EventArgs) Handles btn_ClrGlue0.Click
        If MsgBox("准备自动擦胶" & vbCr & "谨防撞机，请确认？", vbOKCancel + vbQuestion) <> vbOK Then
            Exit Sub
        End If

        'Do While True
        '    Clr_GlueS2(ClrGlue_workS2) '2工位自动擦胶
        '    Delay(10)

        '    If ClrGlue_workS2.State = False Then         '等待校针结束
        '        If ClrGlue_workS2.Result = True Then
        '            Step_ClrGlueS2 = 0
        '            List_DebugAddMessage("2工位自动擦胶完成")
        '        Else
        '            Step_ClrGlueS2 = 0
        '            List_DebugAddMessage("2工位自动擦胶失败")
        '        End If
        '        Exit Do
        '    End If

        '    If IsSysEmcStop Then    '判断急停按钮是否按下
        '        Step_ClrGlueS2 = 0
        '        Frm_DialogAddMessage("2工位自动擦胶急停中断")
        '        Exit Do
        '    End If
        '    Application.DoEvents()
        'Loop
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Winsock2State Then
            TriggerBarcodeScanner(1)
        Else
            Frm_DialogAddMessage("条码枪未连接")
        End If

    End Sub

    Private Sub btn_enlarge_Click(sender As Object, e As EventArgs) Handles btn_enlarge.Click
        Static size_Small As Size = Me.tab_Log.Size
        Static size_Large As Size = New Size(Me.tab_Log.Size.Width + 200, Me.tab_Log.Size.Height)
        Static size_SmallListbox As Size = Me.listbox_Debug.Size
        Static size_LargeListbox As Size = New Size(Me.listbox_Debug.Size.Width + 200, Me.listbox_Debug.Size.Height)
        Static point_SmallLocationBtn_enlarge As Point = Me.btn_enlarge.Location
        Static point_LargeLocationBtn_enlarge As Point = New Point(Me.btn_enlarge.Location.X + 200, Me.btn_enlarge.Location.Y)

        If Me.tab_Log.Size = size_Small Then
            Me.tab_Log.Size = size_Large
            Me.listbox_Debug.Size = size_LargeListbox
            Me.listbox_Debug.HorizontalScrollbar = True
            btn_enlarge.Location = point_LargeLocationBtn_enlarge
            btn_enlarge.Text = "<<"
        Else
            Me.tab_Log.Size = size_Small
            Me.listbox_Debug.Size = size_SmallListbox
            Me.listbox_Debug.HorizontalScrollbar = False
            btn_enlarge.Location = point_SmallLocationBtn_enlarge
            btn_enlarge.Text = ">>"
        End If
    End Sub

    Private Sub tab_Log_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tab_Log.SelectedIndexChanged
        If Me.tab_Log.SelectedIndex <> 0 And Me.btn_enlarge.Text = "<<" Then
            Me.tab_Log.Size = New Size(Me.tab_Log.Size.Width - 200, Me.tab_Log.Size.Height)
            Me.listbox_Debug.Size = New Size(Me.listbox_Debug.Size.Width - 200, Me.listbox_Debug.Size.Height)
            Me.listbox_Debug.HorizontalScrollbar = True
            btn_enlarge.Location = New Point(Me.btn_enlarge.Location.X - 200, Me.btn_enlarge.Location.Y)
            btn_enlarge.Text = ">>"
        End If
    End Sub

    Private Sub btn_ClearLog_Click(sender As Object, e As EventArgs) Handles btn_ClearLog.Click
        Me.listbox_Debug.Items.Clear()
    End Sub

#Region "   功能： 校针相关"

#Region "   功能：校针基准、胶针以及镭射和CCD视野中心的距离 显示 和清除"
    Private Sub btn_Needle0BaseXYClear_Click(sender As Object, e As EventArgs) Handles btn_Needle0BaseXYClear.Click
        If MessageBox.Show("确定要清除校针基准？", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Par_Pos.Probe1_Base(0).X = 0
        Par_Pos.Probe1_Base(0).Y = 0
        Par_Pos.Needle_NeedCalibration(0) = True
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Call Load_NeedlePar()
    End Sub

    Private Sub btn_Needle0BaseZClear_Click(sender As Object, e As EventArgs) Handles btn_Needle0BaseZClear.Click
        If MessageBox.Show("确定要清除校针基准？", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Par_Pos.Probe1_Base(0).Z = 0
        Par_Pos.Needle_NeedCalibration(0) = True
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Call Load_NeedlePar()
    End Sub

    Private Sub btn_Needle1BaseXYClear_Click(sender As Object, e As EventArgs) Handles btn_Needle1BaseXYClear.Click
        If MessageBox.Show("确定要清除校针基准？", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Par_Pos.Probe1_Base(1).X = 0
        Par_Pos.Probe1_Base(1).Y = 0
        Par_Pos.Needle_NeedCalibration(1) = True
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Call Load_NeedlePar()
    End Sub

    Private Sub btn_Needle1BaseZClear_Click(sender As Object, e As EventArgs) Handles btn_Needle1BaseZClear.Click
        If MessageBox.Show("确定要清除校针基准？", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Par_Pos.Probe1_Base(1).Z = 0
        Par_Pos.Needle_NeedCalibration(1) = True
        Call Write_Par_Pos(Path_Par_Pos, Par_Pos)
        Call Load_NeedlePar()
    End Sub

    Public Sub Load_NeedlePar()
        txt_Base0X.Text = Format(Par_Pos.Probe1_Base(0).X, "0.000")
        txt_Base0Y.Text = Format(Par_Pos.Probe1_Base(0).Y, "0.000")
        txt_Base0Z.Text = Format(Par_Pos.Probe1_Base(0).Z, "0.000")
        txt_Base1X.Text = Format(Par_Pos.Probe1_Base(1).X, "0.000")
        txt_Base1Y.Text = Format(Par_Pos.Probe1_Base(1).Y, "0.000")
        txt_Base1Z.Text = Format(Par_Pos.Probe1_Base(1).Z, "0.000")
        txt_Diff0X.Text = Format(Par_Pos.Probe1_Diff(0).X, "0.000")
        txt_Diff0Y.Text = Format(Par_Pos.Probe1_Diff(0).Y, "0.000")
        txt_Diff0Z.Text = Format(Par_Pos.Probe1_Diff(0).Z, "0.000")
        txt_Diff1X.Text = Format(Par_Pos.Probe1_Diff(1).X, "0.000")
        txt_Diff1Y.Text = Format(Par_Pos.Probe1_Diff(1).Y, "0.000")
        txt_Diff1Z.Text = Format(Par_Pos.Probe1_Diff(1).Z, "0.000")

        txt_DistLaserToCCD_X.Text = Format(dist_LaserToCCDCenter.X, "0.000")
        txt_DistLaserToCCD_Y.Text = Format(dist_LaserToCCDCenter.Y, "0.000")

        txt_DistNeedle0ToCCD_X.Text = Format(dist_NeedleToCCDCenter(0).X, "0.000")
        txt_DistNeedle0ToCCD_Y.Text = Format(dist_NeedleToCCDCenter(0).Y, "0.000")

        txt_DistNeedle1ToCCD_X.Text = Format(dist_NeedleToCCDCenter(1).X, "0.000")
        txt_DistNeedle1ToCCD_Y.Text = Format(dist_NeedleToCCDCenter(1).Y, "0.000")
    End Sub
#End Region

    Private Sub btn_AtuoNeedle0_Click(sender As Object, e As EventArgs) Handles btn_AtuoNeedle0.Click
        If MessageBox.Show("确定要胶针1自动胶针？", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Call Auto_NeedleCalibration(0)
    End Sub

    Private Sub btn_AtuoNeedle1_Click(sender As Object, e As EventArgs) Handles btn_AtuoNeedle1.Click
        If MessageBox.Show("确定要胶针2自动胶针？", "inline PAM", MessageBoxButtons.YesNo) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        Call Auto_NeedleCalibration(1)
    End Sub
#End Region

#Region "   功能：选择要做的Bracket"
    Private Sub Load_SelectBrc()
        For i = 0 To chk_Brc.Count - 1
            chk_Brc(i) = New CheckBox
            chk_Brc(i).Checked = True
            chk_Brc(i).Enabled = False
        Next
        chk_Brc(0) = chk_Brc1
        chk_Brc(1) = chk_Brc2
        chk_Brc(2) = chk_Brc3
        chk_Brc(3) = chk_Brc4
        chk_Brc(4) = chk_Brc5
        chk_Brc(5) = chk_Brc6
        chk_Brc(6) = chk_Brc7
        chk_Brc(7) = chk_Brc8
        chk_Brc(8) = chk_Brc9
        chk_Brc(9) = chk_Brc10
        chk_Brc(10) = chk_Brc11
        chk_Brc(11) = chk_Brc12
        Call Enable_BrcSelect(False)
    End Sub
    Private Sub btn_EnSelecetBrc_Click(sender As Object, e As EventArgs) Handles btn_EnSelecetBrc.Click
        Call Enable_BrcSelect(True)
    End Sub
    Private Sub btn_lockSelecetBrc_Click(sender As Object, e As EventArgs) Handles btn_lockSelecetBrc.Click
        Call Enable_BrcSelect(False)
    End Sub
    Private Sub Enable_BrcSelect(ByVal en As Boolean)
        If en Then
            For i = 0 To chk_Brc.Count - 1
                chk_Brc(i).Enabled = True
            Next
            btn_EnSelecetBrc.Enabled = False
            btn_lockSelecetBrc.Enabled = True
            rad_SelectAll.Enabled = True
            rad_SelectNone.Enabled = True
        Else
            For i = 0 To chk_Brc.Count - 1
                chk_Brc(i).Enabled = False
            Next
            btn_EnSelecetBrc.Enabled = True
            btn_lockSelecetBrc.Enabled = False
            rad_SelectAll.Enabled = False
            rad_SelectNone.Enabled = False
        End If
    End Sub
    Private Sub rad_Select_CheckedChanged(sender As Object, e As EventArgs) Handles rad_SelectNone.CheckedChanged, rad_SelectAll.CheckedChanged
        If Me.Visible And rad_SelectAll.Enabled And rad_SelectNone.Enabled Then
            If rad_SelectNone.Checked Then
                For i = 0 To chk_Brc.Count - 1
                    chk_Brc(i).Checked = False
                Next
            End If
            If rad_SelectAll.Checked Then
                For i = 0 To chk_Brc.Count - 1
                    chk_Brc(i).Checked = True
                Next
            End If
        End If
    End Sub
#End Region


#Region "   功能：流水线调试相关IO，电机启停等"
    ''' <summary>
    ''' 加载流水线调试界面btn Tag标记数据
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Load_LineBtnTag()
        btn_setBlock0.Tag = 0 : btn_setBlock1.Tag = 1 : btn_setBlock2.Tag = 2 : btn_setBlock3.Tag = 3   '阻挡气缸
        btn_setRise1.Tag = 1 : btn_setRise2.Tag = 2 : btn_setRise3.Tag = 3  '顶升气缸
        btn_setVac1.Tag = 1 : btn_setVac2.Tag = 2 : btn_setVac3.Tag = 3  '真空吸载具
        btn_StopL0.Tag = 0 : btn_StopL1.Tag = 1 : btn_StopL2.Tag = 2 : btn_StopL3.Tag = 3
        btn_MotorRunNegative0.Tag = 0 : btn_MotorRunNegative1.Tag = 1
        btn_MotorRunNegative2.Tag = 2 : btn_MotorRunNegative3.Tag = 3
        btn_MotorRunPositive0.Tag = 0 : btn_MotorRunPositive1.Tag = 1
        btn_MotorRunPositive2.Tag = 2 : btn_MotorRunPositive3.Tag = 3
    End Sub

    ''' <summary>
    ''' IO阻挡气缸
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_setBlock_Click(sender As Object, e As EventArgs) Handles btn_setBlock0.Click, btn_setBlock1.Click, _
        btn_setBlock2.Click, btn_setBlock3.Click


    End Sub

    ''' <summary>
    ''' IO 顶升气缸
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_setRise_Click(sender As Object, e As EventArgs) Handles btn_setRise3.Click, btn_setRise1.Click, btn_setRise2.Click

    End Sub

    ''' <summary>
    ''' IO 真空吸载具
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_setVac_Click(sender As Object, e As EventArgs) Handles btn_setVac1.Click, btn_setVac2.Click, btn_setVac3.Click

    End Sub

    ''' <summary>
    ''' 流水线电机停止
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_Stop_Click(sender As Object, e As EventArgs) Handles btn_StopL0.Click, btn_StopL1.Click, btn_StopL2.Click, btn_StopL3.Click

    End Sub

    ''' <summary>
    ''' 流水线电机正转
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_MotorRunPositive_Click(sender As Object, e As EventArgs) Handles btn_MotorRunPositive0.Click, _
        btn_MotorRunPositive1.Click, btn_MotorRunPositive2.Click, btn_MotorRunPositive3.Click

    End Sub

    ''' <summary>
    ''' 流水线电机反转
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_MotorRunNegative_Click(sender As Object, e As EventArgs) Handles btn_MotorRunNegative0.Click, _
        btn_MotorRunNegative1.Click, btn_MotorRunNegative2.Click, btn_MotorRunNegative3.Click

    End Sub

#End Region



    Private Sub Select_Material_Click(sender As Object, e As EventArgs) Handles Select_Material.Click
        Frm_Material.Show()
    End Sub
End Class