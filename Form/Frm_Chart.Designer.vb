<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Chart
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
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend3 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.BZ_RoundPanel1 = New BoTech.BZ_RoundPanel()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.TrackBar1_Addition = New System.Windows.Forms.Button()
        Me.TrackBar1_Subtraction = New System.Windows.Forms.Button()
        Me.Chart_Yield = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.BZ_RoundPanel2 = New BoTech.BZ_RoundPanel()
        Me.TrackBar3_Addition = New System.Windows.Forms.Button()
        Me.TrackBar3_Subtraction = New System.Windows.Forms.Button()
        Me.TrackBar3 = New System.Windows.Forms.TrackBar()
        Me.Chart_UPH = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.TrackBar2_Addition = New System.Windows.Forms.Button()
        Me.TrackBar2_Subtraction = New System.Windows.Forms.Button()
        Me.TrackBar2 = New System.Windows.Forms.TrackBar()
        Me.Chart_Tossing = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.BZ_RoundPanel1.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chart_Yield, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BZ_RoundPanel2.SuspendLayout()
        CType(Me.TrackBar3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chart_UPH, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Chart_Tossing, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BZ_RoundPanel1
        '
        Me.BZ_RoundPanel1.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_RoundPanel1.BZ_Radius = 11
        Me.BZ_RoundPanel1.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel1.Controls.Add(Me.TrackBar1)
        Me.BZ_RoundPanel1.Controls.Add(Me.TrackBar1_Addition)
        Me.BZ_RoundPanel1.Controls.Add(Me.TrackBar1_Subtraction)
        Me.BZ_RoundPanel1.Controls.Add(Me.Chart_Yield)
        Me.BZ_RoundPanel1.Location = New System.Drawing.Point(5, 0)
        Me.BZ_RoundPanel1.Name = "BZ_RoundPanel1"
        Me.BZ_RoundPanel1.Size = New System.Drawing.Size(1014, 325)
        Me.BZ_RoundPanel1.TabIndex = 0
        '
        'TrackBar1
        '
        Me.TrackBar1.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.TrackBar1.Location = New System.Drawing.Point(356, 273)
        Me.TrackBar1.Maximum = 30
        Me.TrackBar1.Minimum = 1
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(300, 45)
        Me.TrackBar1.TabIndex = 24
        Me.TrackBar1.Value = 7
        '
        'TrackBar1_Addition
        '
        Me.TrackBar1_Addition.BackColor = System.Drawing.Color.LightGray
        Me.TrackBar1_Addition.Font = New System.Drawing.Font("HelveticaNeue", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBar1_Addition.ForeColor = System.Drawing.Color.Black
        Me.TrackBar1_Addition.Location = New System.Drawing.Point(672, 273)
        Me.TrackBar1_Addition.Name = "TrackBar1_Addition"
        Me.TrackBar1_Addition.Size = New System.Drawing.Size(53, 33)
        Me.TrackBar1_Addition.TabIndex = 23
        Me.TrackBar1_Addition.Text = "+"
        Me.TrackBar1_Addition.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage
        Me.TrackBar1_Addition.UseVisualStyleBackColor = False
        '
        'TrackBar1_Subtraction
        '
        Me.TrackBar1_Subtraction.BackColor = System.Drawing.Color.LightGray
        Me.TrackBar1_Subtraction.Font = New System.Drawing.Font("HelveticaNeue", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBar1_Subtraction.ForeColor = System.Drawing.Color.Black
        Me.TrackBar1_Subtraction.Location = New System.Drawing.Point(298, 273)
        Me.TrackBar1_Subtraction.Name = "TrackBar1_Subtraction"
        Me.TrackBar1_Subtraction.Size = New System.Drawing.Size(40, 30)
        Me.TrackBar1_Subtraction.TabIndex = 22
        Me.TrackBar1_Subtraction.Text = "-"
        Me.TrackBar1_Subtraction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.TrackBar1_Subtraction.UseVisualStyleBackColor = False
        '
        'Chart_Yield
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart_Yield.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart_Yield.Legends.Add(Legend1)
        Me.Chart_Yield.Location = New System.Drawing.Point(5, 10)
        Me.Chart_Yield.Name = "Chart_Yield"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.Chart_Yield.Series.Add(Series1)
        Me.Chart_Yield.Size = New System.Drawing.Size(1004, 250)
        Me.Chart_Yield.TabIndex = 21
        Me.Chart_Yield.Text = "Chart1"
        '
        'BZ_RoundPanel2
        '
        Me.BZ_RoundPanel2.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.BZ_RoundPanel2.BZ_Radius = 11
        Me.BZ_RoundPanel2.BZ_RoundStyle = BoTech.BZ_RoundPanel.RoundStyle.All
        Me.BZ_RoundPanel2.Controls.Add(Me.TrackBar3_Addition)
        Me.BZ_RoundPanel2.Controls.Add(Me.TrackBar3_Subtraction)
        Me.BZ_RoundPanel2.Controls.Add(Me.TrackBar3)
        Me.BZ_RoundPanel2.Controls.Add(Me.Chart_UPH)
        Me.BZ_RoundPanel2.Controls.Add(Me.TrackBar2_Addition)
        Me.BZ_RoundPanel2.Controls.Add(Me.TrackBar2_Subtraction)
        Me.BZ_RoundPanel2.Controls.Add(Me.TrackBar2)
        Me.BZ_RoundPanel2.Controls.Add(Me.Chart_Tossing)
        Me.BZ_RoundPanel2.Location = New System.Drawing.Point(5, 330)
        Me.BZ_RoundPanel2.Name = "BZ_RoundPanel2"
        Me.BZ_RoundPanel2.Size = New System.Drawing.Size(1014, 325)
        Me.BZ_RoundPanel2.TabIndex = 1
        '
        'TrackBar3_Addition
        '
        Me.TrackBar3_Addition.Font = New System.Drawing.Font("HelveticaNeue", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBar3_Addition.ForeColor = System.Drawing.Color.Black
        Me.TrackBar3_Addition.Location = New System.Drawing.Point(922, 279)
        Me.TrackBar3_Addition.Name = "TrackBar3_Addition"
        Me.TrackBar3_Addition.Size = New System.Drawing.Size(40, 30)
        Me.TrackBar3_Addition.TabIndex = 33
        Me.TrackBar3_Addition.Text = "+"
        Me.TrackBar3_Addition.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.TrackBar3_Addition.UseVisualStyleBackColor = True
        '
        'TrackBar3_Subtraction
        '
        Me.TrackBar3_Subtraction.BackColor = System.Drawing.Color.LightGray
        Me.TrackBar3_Subtraction.Font = New System.Drawing.Font("HelveticaNeue", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBar3_Subtraction.ForeColor = System.Drawing.Color.Black
        Me.TrackBar3_Subtraction.Location = New System.Drawing.Point(570, 279)
        Me.TrackBar3_Subtraction.Name = "TrackBar3_Subtraction"
        Me.TrackBar3_Subtraction.Size = New System.Drawing.Size(40, 30)
        Me.TrackBar3_Subtraction.TabIndex = 32
        Me.TrackBar3_Subtraction.Text = "-"
        Me.TrackBar3_Subtraction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.TrackBar3_Subtraction.UseVisualStyleBackColor = False
        '
        'TrackBar3
        '
        Me.TrackBar3.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.TrackBar3.Location = New System.Drawing.Point(616, 280)
        Me.TrackBar3.Maximum = 30
        Me.TrackBar3.Minimum = 1
        Me.TrackBar3.Name = "TrackBar3"
        Me.TrackBar3.Size = New System.Drawing.Size(300, 45)
        Me.TrackBar3.TabIndex = 31
        Me.TrackBar3.Value = 7
        '
        'Chart_UPH
        '
        ChartArea2.Name = "ChartArea1"
        Me.Chart_UPH.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.Chart_UPH.Legends.Add(Legend2)
        Me.Chart_UPH.Location = New System.Drawing.Point(512, 10)
        Me.Chart_UPH.Name = "Chart_UPH"
        Series2.ChartArea = "ChartArea1"
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.Chart_UPH.Series.Add(Series2)
        Me.Chart_UPH.Size = New System.Drawing.Size(492, 250)
        Me.Chart_UPH.TabIndex = 30
        Me.Chart_UPH.Text = "Chart1"
        '
        'TrackBar2_Addition
        '
        Me.TrackBar2_Addition.BackColor = System.Drawing.Color.LightGray
        Me.TrackBar2_Addition.Font = New System.Drawing.Font("HelveticaNeue", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBar2_Addition.ForeColor = System.Drawing.Color.Black
        Me.TrackBar2_Addition.Location = New System.Drawing.Point(404, 280)
        Me.TrackBar2_Addition.Name = "TrackBar2_Addition"
        Me.TrackBar2_Addition.Size = New System.Drawing.Size(40, 30)
        Me.TrackBar2_Addition.TabIndex = 29
        Me.TrackBar2_Addition.Text = "+"
        Me.TrackBar2_Addition.UseVisualStyleBackColor = False
        '
        'TrackBar2_Subtraction
        '
        Me.TrackBar2_Subtraction.BackColor = System.Drawing.Color.LightGray
        Me.TrackBar2_Subtraction.Font = New System.Drawing.Font("HelveticaNeue", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackBar2_Subtraction.ForeColor = System.Drawing.Color.Black
        Me.TrackBar2_Subtraction.Location = New System.Drawing.Point(52, 279)
        Me.TrackBar2_Subtraction.Name = "TrackBar2_Subtraction"
        Me.TrackBar2_Subtraction.Size = New System.Drawing.Size(40, 30)
        Me.TrackBar2_Subtraction.TabIndex = 28
        Me.TrackBar2_Subtraction.Text = "-"
        Me.TrackBar2_Subtraction.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.TrackBar2_Subtraction.UseVisualStyleBackColor = False
        '
        'TrackBar2
        '
        Me.TrackBar2.BackColor = System.Drawing.Color.FromArgb(CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.TrackBar2.Location = New System.Drawing.Point(98, 280)
        Me.TrackBar2.Maximum = 30
        Me.TrackBar2.Minimum = 1
        Me.TrackBar2.Name = "TrackBar2"
        Me.TrackBar2.Size = New System.Drawing.Size(300, 45)
        Me.TrackBar2.TabIndex = 27
        Me.TrackBar2.Value = 5
        '
        'Chart_Tossing
        '
        ChartArea3.Name = "ChartArea1"
        Me.Chart_Tossing.ChartAreas.Add(ChartArea3)
        Legend3.Name = "Legend1"
        Me.Chart_Tossing.Legends.Add(Legend3)
        Me.Chart_Tossing.Location = New System.Drawing.Point(10, 10)
        Me.Chart_Tossing.Name = "Chart_Tossing"
        Series3.ChartArea = "ChartArea1"
        Series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series3.Legend = "Legend1"
        Series3.Name = "Series1"
        Me.Chart_Tossing.Series.Add(Series3)
        Me.Chart_Tossing.Size = New System.Drawing.Size(492, 250)
        Me.Chart_Tossing.TabIndex = 26
        Me.Chart_Tossing.Text = "Chart1"
        '
        'Frm_Chart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1024, 660)
        Me.Controls.Add(Me.BZ_RoundPanel2)
        Me.Controls.Add(Me.BZ_RoundPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Location = New System.Drawing.Point(0, 70)
        Me.Name = "Frm_Chart"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Frm_Chart"
        Me.BZ_RoundPanel1.ResumeLayout(False)
        Me.BZ_RoundPanel1.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chart_Yield, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BZ_RoundPanel2.ResumeLayout(False)
        Me.BZ_RoundPanel2.PerformLayout()
        CType(Me.TrackBar3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chart_UPH, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Chart_Tossing, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BZ_RoundPanel1 As BoTech.BZ_RoundPanel
    Friend WithEvents BZ_RoundPanel2 As BoTech.BZ_RoundPanel
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents TrackBar1_Addition As System.Windows.Forms.Button
    Friend WithEvents TrackBar1_Subtraction As System.Windows.Forms.Button
    Friend WithEvents Chart_Yield As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents TrackBar3_Addition As System.Windows.Forms.Button
    Friend WithEvents TrackBar3_Subtraction As System.Windows.Forms.Button
    Friend WithEvents TrackBar3 As System.Windows.Forms.TrackBar
    Friend WithEvents Chart_UPH As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents TrackBar2_Addition As System.Windows.Forms.Button
    Friend WithEvents TrackBar2_Subtraction As System.Windows.Forms.Button
    Friend WithEvents TrackBar2 As System.Windows.Forms.TrackBar
    Friend WithEvents Chart_Tossing As System.Windows.Forms.DataVisualization.Charting.Chart
End Class
