<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Home
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend3 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series4 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series5 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.BZ_Label1 = New BoTech.BZ_Label()
        Me.BZ_Label2 = New BoTech.BZ_Label()
        Me.BZ_Label3 = New BoTech.BZ_Label()
        Me.BZ_Label4 = New BoTech.BZ_Label()
        Me.BZ_RoundPanel3 = New BoTech.BZ_RoundPanel()
        Me.Btn_Reset = New BoTech.BZ_Button()
        Me.Btn_Yield = New BoTech.BZ_Button()
        Me.YieldBar = New BoTech.BZ_DoubleNG_Rate()
        Me.BZ_RoundPanel2 = New BoTech.BZ_RoundPanel()
        Me.BZ_Button8 = New BoTech.BZ_Button()
        Me.BZ_Button9 = New BoTech.BZ_Button()
        Me.lbl_CPKResult = New BoTech.BZ_Label()
        Me.BZ_Button7 = New BoTech.BZ_Button()
        Me.Chart_CPK = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.BZ_RoundPanel1 = New BoTech.BZ_RoundPanel()
        Me.BZ_Button4 = New BoTech.BZ_Button()
        Me.BZ_Button5 = New BoTech.BZ_Button()
        Me.BZ_Button6 = New BoTech.BZ_Button()
        Me.BZ_Button3 = New BoTech.BZ_Button()
        Me.BZ_Button2 = New BoTech.BZ_Button()
        Me.BZ_Button1 = New BoTech.BZ_Button()
        Me.Chart2 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.BZ_RoundPanel3.SuspendLayout()
        Me.BZ_RoundPanel2.SuspendLayout()
        CType(Me.Chart_CPK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BZ_RoundPanel1.SuspendLayout()
        CType(Me.Chart2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BZ_Label1
        '
        Me.BZ_Label1.BZ_BigText = ""
        Me.BZ_Label1.BZ_BigTextFont = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BZ_Label1.BZ_BigTextOffset = 0
        Me.BZ_Label1.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(151, Byte), Integer))
        Me.BZ_Label1.BZ_Radius = 11
        Me.BZ_Label1.BZ_RoundStyle = BoTech.BZ_Label.RoundStyle.All
        Me.BZ_Label1.BZ_SmallText = ""
        Me.BZ_Label1.BZ_SmallTextFont = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.BZ_Label1.BZ_SmallTextOffset = 0
        Me.BZ_Label1.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label1.Location = New System.Drawing.Point(5, 0)
        Me.BZ_Label1.Name = "BZ_Label1"
        Me.BZ_Label1.Size = New System.Drawing.Size(125, 60)
        Me.BZ_Label1.TabIndex = 0
        Me.BZ_Label1.Text = "No Alarm"
        '
        'BZ_Label2
        '
        Me.BZ_Label2.BZ_BigText = "--:--:--"
        Me.BZ_Label2.BZ_BigTextFont = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label2.BZ_BigTextOffset = 0
        Me.BZ_Label2.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(151, Byte), Integer))
        Me.BZ_Label2.BZ_Radius = 11
        Me.BZ_Label2.BZ_RoundStyle = BoTech.BZ_Label.RoundStyle.All
        Me.BZ_Label2.BZ_SmallText = "ALARM TIME"
        Me.BZ_Label2.BZ_SmallTextFont = New System.Drawing.Font("HelveticaNeue", 9.749999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label2.BZ_SmallTextOffset = 5
        Me.BZ_Label2.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label2.Location = New System.Drawing.Point(135, 0)
        Me.BZ_Label2.Name = "BZ_Label2"
        Me.BZ_Label2.Size = New System.Drawing.Size(125, 60)
        Me.BZ_Label2.TabIndex = 1
        '
        'BZ_Label3
        '
        Me.BZ_Label3.BZ_BigText = "210"
        Me.BZ_Label3.BZ_BigTextFont = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label3.BZ_BigTextOffset = 0
        Me.BZ_Label3.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(191, Byte), Integer))
        Me.BZ_Label3.BZ_Radius = 11
        Me.BZ_Label3.BZ_RoundStyle = BoTech.BZ_Label.RoundStyle.All
        Me.BZ_Label3.BZ_SmallText = "UPH"
        Me.BZ_Label3.BZ_SmallTextFont = New System.Drawing.Font("HelveticaNeue", 9.749999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label3.BZ_SmallTextOffset = 5
        Me.BZ_Label3.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label3.Location = New System.Drawing.Point(265, 0)
        Me.BZ_Label3.Name = "BZ_Label3"
        Me.BZ_Label3.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Label3.TabIndex = 2
        '
        'BZ_Label4
        '
        Me.BZ_Label4.BZ_BigText = "Low"
        Me.BZ_Label4.BZ_BigTextFont = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label4.BZ_BigTextOffset = 0
        Me.BZ_Label4.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(253, Byte), Integer), CType(CType(253, Byte), Integer), CType(CType(191, Byte), Integer))
        Me.BZ_Label4.BZ_Radius = 11
        Me.BZ_Label4.BZ_RoundStyle = BoTech.BZ_Label.RoundStyle.All
        Me.BZ_Label4.BZ_SmallText = "Material"
        Me.BZ_Label4.BZ_SmallTextFont = New System.Drawing.Font("HelveticaNeue", 9.749999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label4.BZ_SmallTextOffset = 5
        Me.BZ_Label4.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Label4.Location = New System.Drawing.Point(330, 0)
        Me.BZ_Label4.Name = "BZ_Label4"
        Me.BZ_Label4.Size = New System.Drawing.Size(160, 60)
        Me.BZ_Label4.TabIndex = 3
        '
        'BZ_RoundPanel3
        '
        Me.BZ_RoundPanel3.BZ_Color = System.Drawing.Color.WhiteSmoke
        Me.BZ_RoundPanel3.BZ_Radius = 11
        Me.BZ_RoundPanel3.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel3.Controls.Add(Me.Btn_Reset)
        Me.BZ_RoundPanel3.Controls.Add(Me.Btn_Yield)
        Me.BZ_RoundPanel3.Controls.Add(Me.YieldBar)
        Me.BZ_RoundPanel3.Location = New System.Drawing.Point(625, 330)
        Me.BZ_RoundPanel3.Name = "BZ_RoundPanel3"
        Me.BZ_RoundPanel3.Size = New System.Drawing.Size(394, 325)
        Me.BZ_RoundPanel3.TabIndex = 6
        '
        'Btn_Reset
        '
        Me.Btn_Reset.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Reset.BZ_Radius = 11
        Me.Btn_Reset.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Reset.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_Reset.Location = New System.Drawing.Point(252, 209)
        Me.Btn_Reset.Name = "Btn_Reset"
        Me.Btn_Reset.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Reset.TabIndex = 7
        Me.Btn_Reset.Text = "Reset"
        Me.Btn_Reset.UseVisualStyleBackColor = True
        '
        'Btn_Yield
        '
        Me.Btn_Yield.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.Btn_Yield.BZ_Radius = 11
        Me.Btn_Yield.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.Btn_Yield.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn_Yield.Location = New System.Drawing.Point(72, 209)
        Me.Btn_Yield.Name = "Btn_Yield"
        Me.Btn_Yield.Size = New System.Drawing.Size(60, 60)
        Me.Btn_Yield.TabIndex = 6
        Me.Btn_Yield.Text = "Yield"
        Me.Btn_Yield.UseVisualStyleBackColor = True
        '
        'YieldBar
        '
        Me.YieldBar.BZ_DayNG = 40
        Me.YieldBar.BZ_MonthNG = 30
        Me.YieldBar.BZ_NGColor = System.Drawing.Color.Red
        Me.YieldBar.BZ_OKColor = System.Drawing.Color.Green
        Me.YieldBar.Font = New System.Drawing.Font("HelveticaNeue", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.YieldBar.Location = New System.Drawing.Point(72, 43)
        Me.YieldBar.Name = "YieldBar"
        Me.YieldBar.Size = New System.Drawing.Size(240, 160)
        Me.YieldBar.TabIndex = 0
        Me.YieldBar.Text = "NG"
        Me.YieldBar.UseVisualStyleBackColor = True
        '
        'BZ_RoundPanel2
        '
        Me.BZ_RoundPanel2.BZ_Color = System.Drawing.Color.WhiteSmoke
        Me.BZ_RoundPanel2.BZ_Radius = 11
        Me.BZ_RoundPanel2.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel2.Controls.Add(Me.BZ_Button8)
        Me.BZ_RoundPanel2.Controls.Add(Me.BZ_Button9)
        Me.BZ_RoundPanel2.Controls.Add(Me.lbl_CPKResult)
        Me.BZ_RoundPanel2.Controls.Add(Me.BZ_Button7)
        Me.BZ_RoundPanel2.Controls.Add(Me.Chart_CPK)
        Me.BZ_RoundPanel2.Location = New System.Drawing.Point(625, 0)
        Me.BZ_RoundPanel2.Name = "BZ_RoundPanel2"
        Me.BZ_RoundPanel2.Size = New System.Drawing.Size(394, 325)
        Me.BZ_RoundPanel2.TabIndex = 7
        '
        'BZ_Button8
        '
        Me.BZ_Button8.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button8.BZ_Radius = 11
        Me.BZ_Button8.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button8.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button8.Location = New System.Drawing.Point(323, 191)
        Me.BZ_Button8.Name = "BZ_Button8"
        Me.BZ_Button8.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button8.TabIndex = 11
        Me.BZ_Button8.Text = "A"
        Me.BZ_Button8.UseVisualStyleBackColor = True
        '
        'BZ_Button9
        '
        Me.BZ_Button9.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button9.BZ_Radius = 11
        Me.BZ_Button9.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button9.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button9.Location = New System.Drawing.Point(323, 125)
        Me.BZ_Button9.Name = "BZ_Button9"
        Me.BZ_Button9.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button9.TabIndex = 10
        Me.BZ_Button9.Text = "Y"
        Me.BZ_Button9.UseVisualStyleBackColor = True
        '
        'lbl_CPKResult
        '
        Me.lbl_CPKResult.BZ_BigText = ""
        Me.lbl_CPKResult.BZ_BigTextFont = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.lbl_CPKResult.BZ_BigTextOffset = 0
        Me.lbl_CPKResult.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(174, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(151, Byte), Integer))
        Me.lbl_CPKResult.BZ_Radius = 11
        Me.lbl_CPKResult.BZ_RoundStyle = BoTech.BZ_Label.RoundStyle.All
        Me.lbl_CPKResult.BZ_SmallText = ""
        Me.lbl_CPKResult.BZ_SmallTextFont = New System.Drawing.Font("宋体", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.lbl_CPKResult.BZ_SmallTextOffset = 0
        Me.lbl_CPKResult.Font = New System.Drawing.Font("HelveticaNeue", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_CPKResult.Location = New System.Drawing.Point(18, 258)
        Me.lbl_CPKResult.Name = "lbl_CPKResult"
        Me.lbl_CPKResult.Size = New System.Drawing.Size(276, 60)
        Me.lbl_CPKResult.TabIndex = 9
        Me.lbl_CPKResult.Text = "No Alarm"
        '
        'BZ_Button7
        '
        Me.BZ_Button7.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button7.BZ_Radius = 11
        Me.BZ_Button7.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button7.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button7.Location = New System.Drawing.Point(323, 59)
        Me.BZ_Button7.Name = "BZ_Button7"
        Me.BZ_Button7.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button7.TabIndex = 8
        Me.BZ_Button7.Text = "X"
        Me.BZ_Button7.UseVisualStyleBackColor = True
        '
        'Chart_CPK
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart_CPK.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart_CPK.Legends.Add(Legend1)
        Me.Chart_CPK.Location = New System.Drawing.Point(18, 65)
        Me.Chart_CPK.Name = "Chart_CPK"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series1.IsVisibleInLegend = False
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Series1.YValuesPerPoint = 2
        Me.Chart_CPK.Series.Add(Series1)
        Me.Chart_CPK.Size = New System.Drawing.Size(276, 184)
        Me.Chart_CPK.TabIndex = 0
        Me.Chart_CPK.Text = "Chart3"
        '
        'BZ_RoundPanel1
        '
        Me.BZ_RoundPanel1.BZ_Color = System.Drawing.Color.WhiteSmoke
        Me.BZ_RoundPanel1.BZ_Radius = 11
        Me.BZ_RoundPanel1.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel1.Controls.Add(Me.BZ_Button4)
        Me.BZ_RoundPanel1.Controls.Add(Me.BZ_Button5)
        Me.BZ_RoundPanel1.Controls.Add(Me.BZ_Button6)
        Me.BZ_RoundPanel1.Controls.Add(Me.BZ_Button3)
        Me.BZ_RoundPanel1.Controls.Add(Me.BZ_Button2)
        Me.BZ_RoundPanel1.Controls.Add(Me.BZ_Button1)
        Me.BZ_RoundPanel1.Controls.Add(Me.Chart2)
        Me.BZ_RoundPanel1.Controls.Add(Me.Chart1)
        Me.BZ_RoundPanel1.Location = New System.Drawing.Point(5, 65)
        Me.BZ_RoundPanel1.Name = "BZ_RoundPanel1"
        Me.BZ_RoundPanel1.Size = New System.Drawing.Size(615, 590)
        Me.BZ_RoundPanel1.TabIndex = 8
        '
        'BZ_Button4
        '
        Me.BZ_Button4.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button4.BZ_Radius = 11
        Me.BZ_Button4.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button4.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button4.Location = New System.Drawing.Point(542, 474)
        Me.BZ_Button4.Name = "BZ_Button4"
        Me.BZ_Button4.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button4.TabIndex = 12
        Me.BZ_Button4.Text = "A"
        Me.BZ_Button4.UseVisualStyleBackColor = True
        '
        'BZ_Button5
        '
        Me.BZ_Button5.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button5.BZ_Radius = 11
        Me.BZ_Button5.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button5.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button5.Location = New System.Drawing.Point(542, 398)
        Me.BZ_Button5.Name = "BZ_Button5"
        Me.BZ_Button5.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button5.TabIndex = 11
        Me.BZ_Button5.Text = "Y"
        Me.BZ_Button5.UseVisualStyleBackColor = True
        '
        'BZ_Button6
        '
        Me.BZ_Button6.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button6.BZ_Radius = 11
        Me.BZ_Button6.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button6.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button6.Location = New System.Drawing.Point(542, 322)
        Me.BZ_Button6.Name = "BZ_Button6"
        Me.BZ_Button6.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button6.TabIndex = 10
        Me.BZ_Button6.Text = "X"
        Me.BZ_Button6.UseVisualStyleBackColor = True
        '
        'BZ_Button3
        '
        Me.BZ_Button3.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button3.BZ_Radius = 11
        Me.BZ_Button3.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button3.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button3.Location = New System.Drawing.Point(542, 179)
        Me.BZ_Button3.Name = "BZ_Button3"
        Me.BZ_Button3.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button3.TabIndex = 9
        Me.BZ_Button3.Text = "A"
        Me.BZ_Button3.UseVisualStyleBackColor = True
        '
        'BZ_Button2
        '
        Me.BZ_Button2.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button2.BZ_Radius = 11
        Me.BZ_Button2.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button2.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button2.Location = New System.Drawing.Point(542, 103)
        Me.BZ_Button2.Name = "BZ_Button2"
        Me.BZ_Button2.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button2.TabIndex = 8
        Me.BZ_Button2.Text = "Y"
        Me.BZ_Button2.UseVisualStyleBackColor = True
        '
        'BZ_Button1
        '
        Me.BZ_Button1.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_Button1.BZ_Radius = 11
        Me.BZ_Button1.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.BZ_Button1.Font = New System.Drawing.Font("HelveticaNeue", 14.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BZ_Button1.Location = New System.Drawing.Point(542, 27)
        Me.BZ_Button1.Name = "BZ_Button1"
        Me.BZ_Button1.Size = New System.Drawing.Size(60, 60)
        Me.BZ_Button1.TabIndex = 7
        Me.BZ_Button1.Text = "X"
        Me.BZ_Button1.UseVisualStyleBackColor = True
        '
        'Chart2
        '
        ChartArea2.Name = "ChartArea1"
        Me.Chart2.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.Chart2.Legends.Add(Legend2)
        Me.Chart2.Location = New System.Drawing.Point(10, 293)
        Me.Chart2.Name = "Chart2"
        Series2.ChartArea = "ChartArea1"
        Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series3.Legend = "Legend1"
        Series3.Name = "Series2"
        Me.Chart2.Series.Add(Series2)
        Me.Chart2.Series.Add(Series3)
        Me.Chart2.Size = New System.Drawing.Size(499, 269)
        Me.Chart2.TabIndex = 1
        Me.Chart2.Text = "Chart2"
        '
        'Chart1
        '
        ChartArea3.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea3)
        Legend3.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend3)
        Me.Chart1.Location = New System.Drawing.Point(10, 10)
        Me.Chart1.Name = "Chart1"
        Series4.ChartArea = "ChartArea1"
        Series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series4.Legend = "Legend1"
        Series4.Name = "Series1"
        Series5.ChartArea = "ChartArea1"
        Series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series5.Legend = "Legend1"
        Series5.Name = "Series2"
        Me.Chart1.Series.Add(Series4)
        Me.Chart1.Series.Add(Series5)
        Me.Chart1.Size = New System.Drawing.Size(499, 246)
        Me.Chart1.TabIndex = 0
        Me.Chart1.Text = "Chart1"
        '
        'Frm_Home
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1024, 660)
        Me.Controls.Add(Me.BZ_RoundPanel1)
        Me.Controls.Add(Me.BZ_RoundPanel2)
        Me.Controls.Add(Me.BZ_RoundPanel3)
        Me.Controls.Add(Me.BZ_Label4)
        Me.Controls.Add(Me.BZ_Label3)
        Me.Controls.Add(Me.BZ_Label2)
        Me.Controls.Add(Me.BZ_Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(0, 70)
        Me.MinimumSize = New System.Drawing.Size(0, 70)
        Me.Name = "Frm_Home"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Frm_Home"
        Me.BZ_RoundPanel3.ResumeLayout(False)
        Me.BZ_RoundPanel2.ResumeLayout(False)
        CType(Me.Chart_CPK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BZ_RoundPanel1.ResumeLayout(False)
        CType(Me.Chart2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BZ_Label1 As BoTech.BZ_Label
    Friend WithEvents BZ_Label2 As BoTech.BZ_Label
    Friend WithEvents BZ_Label3 As BoTech.BZ_Label
    Friend WithEvents BZ_Label4 As BoTech.BZ_Label
    Friend WithEvents BZ_RoundPanel3 As BoTech.BZ_RoundPanel
    Friend WithEvents YieldBar As BoTech.BZ_DoubleNG_Rate
    Friend WithEvents Btn_Reset As BoTech.BZ_Button
    Friend WithEvents Btn_Yield As BoTech.BZ_Button
    Friend WithEvents BZ_RoundPanel2 As BoTech.BZ_RoundPanel
    Friend WithEvents BZ_Button8 As BoTech.BZ_Button
    Friend WithEvents BZ_Button9 As BoTech.BZ_Button
    Friend WithEvents lbl_CPKResult As BoTech.BZ_Label
    Friend WithEvents BZ_Button7 As BoTech.BZ_Button
    Friend WithEvents Chart_CPK As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents BZ_RoundPanel1 As BoTech.BZ_RoundPanel
    Friend WithEvents BZ_Button4 As BoTech.BZ_Button
    Friend WithEvents BZ_Button5 As BoTech.BZ_Button
    Friend WithEvents BZ_Button6 As BoTech.BZ_Button
    Friend WithEvents BZ_Button3 As BoTech.BZ_Button
    Friend WithEvents BZ_Button2 As BoTech.BZ_Button
    Friend WithEvents BZ_Button1 As BoTech.BZ_Button
    Friend WithEvents Chart2 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
End Class
