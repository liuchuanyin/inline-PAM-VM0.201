<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Alarm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Alarm))
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.BZ_RoundPanel1 = New BoTech.BZ_RoundPanel()
        Me.ListBox_Alarm = New System.Windows.Forms.ListBox()
        Me.BZ_RoundPanel2 = New BoTech.BZ_RoundPanel()
        Me.btn_Download = New BoTech.BZ_Button()
        Me.AlarmDataGridView = New System.Windows.Forms.DataGridView()
        Me.BZ_RoundPanel3 = New BoTech.BZ_RoundPanel()
        Me.ChartCricle = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.BZ_RoundPanel1.SuspendLayout()
        Me.BZ_RoundPanel2.SuspendLayout()
        CType(Me.AlarmDataGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BZ_RoundPanel3.SuspendLayout()
        CType(Me.ChartCricle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BZ_RoundPanel1
        '
        Me.BZ_RoundPanel1.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(222, Byte), Integer))
        Me.BZ_RoundPanel1.BZ_Radius = 11
        Me.BZ_RoundPanel1.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel1.Controls.Add(Me.ListBox_Alarm)
        Me.BZ_RoundPanel1.Location = New System.Drawing.Point(5, 0)
        Me.BZ_RoundPanel1.Name = "BZ_RoundPanel1"
        Me.BZ_RoundPanel1.Size = New System.Drawing.Size(550, 80)
        Me.BZ_RoundPanel1.TabIndex = 0
        '
        'ListBox_Alarm
        '
        Me.ListBox_Alarm.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(223, Byte), Integer), CType(CType(222, Byte), Integer))
        Me.ListBox_Alarm.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListBox_Alarm.Font = New System.Drawing.Font("HelveticaNeue", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBox_Alarm.FormattingEnabled = True
        Me.ListBox_Alarm.ItemHeight = 19
        Me.ListBox_Alarm.Items.AddRange(New Object() {"● Code: xxxx & Category", "● Description"})
        Me.ListBox_Alarm.Location = New System.Drawing.Point(12, 21)
        Me.ListBox_Alarm.Name = "ListBox_Alarm"
        Me.ListBox_Alarm.Size = New System.Drawing.Size(521, 38)
        Me.ListBox_Alarm.TabIndex = 1
        '
        'BZ_RoundPanel2
        '
        Me.BZ_RoundPanel2.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_RoundPanel2.BZ_Radius = 11
        Me.BZ_RoundPanel2.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel2.Controls.Add(Me.btn_Download)
        Me.BZ_RoundPanel2.Controls.Add(Me.AlarmDataGridView)
        Me.BZ_RoundPanel2.Location = New System.Drawing.Point(5, 85)
        Me.BZ_RoundPanel2.Name = "BZ_RoundPanel2"
        Me.BZ_RoundPanel2.Size = New System.Drawing.Size(550, 570)
        Me.BZ_RoundPanel2.TabIndex = 1
        '
        'btn_Download
        '
        Me.btn_Download.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.btn_Download.BZ_Radius = 11
        Me.btn_Download.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.btn_Download.Image = CType(resources.GetObject("btn_Download.Image"), System.Drawing.Image)
        Me.btn_Download.Location = New System.Drawing.Point(214, 332)
        Me.btn_Download.Name = "btn_Download"
        Me.btn_Download.Size = New System.Drawing.Size(60, 60)
        Me.btn_Download.TabIndex = 3
        Me.btn_Download.UseVisualStyleBackColor = True
        '
        'AlarmDataGridView
        '
        Me.AlarmDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AlarmDataGridView.Location = New System.Drawing.Point(5, 5)
        Me.AlarmDataGridView.Name = "AlarmDataGridView"
        Me.AlarmDataGridView.RowTemplate.Height = 23
        Me.AlarmDataGridView.Size = New System.Drawing.Size(540, 300)
        Me.AlarmDataGridView.TabIndex = 2
        '
        'BZ_RoundPanel3
        '
        Me.BZ_RoundPanel3.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_RoundPanel3.BZ_Radius = 11
        Me.BZ_RoundPanel3.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel3.Controls.Add(Me.ChartCricle)
        Me.BZ_RoundPanel3.Location = New System.Drawing.Point(560, 0)
        Me.BZ_RoundPanel3.Name = "BZ_RoundPanel3"
        Me.BZ_RoundPanel3.Size = New System.Drawing.Size(459, 655)
        Me.BZ_RoundPanel3.TabIndex = 2
        '
        'ChartCricle
        '
        Me.ChartCricle.BackColor = System.Drawing.Color.Transparent
        ChartArea1.Name = "ChartArea1"
        Me.ChartCricle.ChartAreas.Add(ChartArea1)
        Legend1.Alignment = System.Drawing.StringAlignment.Center
        Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top
        Legend1.Name = "Legend1"
        Legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide
        Me.ChartCricle.Legends.Add(Legend1)
        Me.ChartCricle.Location = New System.Drawing.Point(42, 45)
        Me.ChartCricle.Name = "ChartCricle"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.ChartCricle.Series.Add(Series1)
        Me.ChartCricle.Size = New System.Drawing.Size(354, 330)
        Me.ChartCricle.TabIndex = 0
        Me.ChartCricle.Text = "Chart1"
        '
        'Frm_Alarm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1024, 660)
        Me.Controls.Add(Me.BZ_RoundPanel3)
        Me.Controls.Add(Me.BZ_RoundPanel2)
        Me.Controls.Add(Me.BZ_RoundPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(0, 70)
        Me.Name = "Frm_Alarm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Frm_Alarm"
        Me.BZ_RoundPanel1.ResumeLayout(False)
        Me.BZ_RoundPanel2.ResumeLayout(False)
        CType(Me.AlarmDataGridView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BZ_RoundPanel3.ResumeLayout(False)
        CType(Me.ChartCricle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BZ_RoundPanel1 As BoTech.BZ_RoundPanel
    Friend WithEvents ListBox_Alarm As System.Windows.Forms.ListBox
    Friend WithEvents BZ_RoundPanel2 As BoTech.BZ_RoundPanel
    Friend WithEvents BZ_RoundPanel3 As BoTech.BZ_RoundPanel
    Friend WithEvents ChartCricle As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents AlarmDataGridView As System.Windows.Forms.DataGridView
    Friend WithEvents btn_Download As BoTech.BZ_Button
End Class
