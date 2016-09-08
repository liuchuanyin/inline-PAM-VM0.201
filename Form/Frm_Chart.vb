Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Math

Public Class Frm_Chart

    Private Sub Frm_Chart_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call LoadChart_Curve_TossingRate()
        Call LoadChart_Curve_YieldRate()
        Call LoadChart_Curve_UPH()
    End Sub

#Region "   良率刷新"
    Public Sub LoadChart_Curve_YieldRate()
        Dim dt_Yield As New DataTable
        Dim dr_Yield As DataRow
        Dim i, j As Integer

        j = CInt(TrackBar1.Value)

        dt_Yield.Columns.Add("Day_X")
        dt_Yield.Columns.Add("Yield_Y")
        For i = Yield_Month.Count - j To Yield_Month.Count - 1
            dr_Yield = dt_Yield.NewRow()    '新增行
            dr_Yield.Item(0) = Format(Yield_Month.Item(i).Date_Prouct, "MM.dd")
            If Yield_Month.Item(i).Count_Production = 0 Then
                dr_Yield.Item(1) = 100
            Else
                dr_Yield.Item(1) = Format((1 - Yield_Month.Item(i).Count_NG / Yield_Month.Item(i).Count_Production) * 100, "0.00")
            End If

            dt_Yield.Rows.Add(dr_Yield)
        Next
        dr_Yield = Nothing
        With Me.Chart_Yield
            .DataSource = dt_Yield
            .Series.Clear()
            .Legends.Clear()
            .ChartAreas.Clear()
            .Titles.Clear()
            .Titles.Add("Yield Rate（Units: %）")
            .Titles(0).Font = New System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性
            .ChartAreas.Add("Yield")
            .Legends.Add("Yield")
            .Series.Add("Yield")
            .Series.Add("Yield_Point")
            .Series("Yield").LegendToolTip = "压力数据"
            .Series("Yield").IsValueShownAsLabel = True    '标签显示数据值
            '.Legends("Yield").DockedToChartArea = "Yield"     '指定Legend所属ChartArea
            .ChartAreas("Yield").Area3DStyle.Enable3D = False  '启用3D样式

            .ChartAreas("Yield").AxisY.Maximum = 100
            .ChartAreas("Yield").AxisY.Minimum = 0
            .ChartAreas("Yield").AxisX.LineColor = System.Drawing.Color.LightGray            '设置轴的线条颜色
            .ChartAreas("Yield").AxisY.LineColor = System.Drawing.Color.LightGray            '设置轴的线条颜色
            .ChartAreas("Yield").AxisX.MajorGrid.Interval = 1
            .ChartAreas("Yield").AxisY.IsLabelAutoFit = True                             '设置是否自动调整轴标签  

            '.ChartAreas("Yield").AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性 
            '.ChartAreas("Yield").AxisY.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性 
            Me.Chart_Yield.Legends("Yield").Enabled = False

            .ChartAreas("Yield").AxisX.MajorGrid.Enabled = True
            .ChartAreas("Yield").AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas("Yield").AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
            .ChartAreas("Yield").AxisY.MajorGrid.Enabled = True
            .ChartAreas("Yield").AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas("Yield").AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

            .ChartAreas("Yield").AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
            .ChartAreas("Yield").AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
            .ChartAreas("Yield").AxisX.MajorTickMark.Interval = 1
            .ChartAreas("Yield").AxisY.MajorTickMark.Interval = 10
            .ChartAreas("Yield").AxisX.LabelStyle.Angle = 0

            .ChartAreas("Yield").BackColor = Color.White
            '.BackColor = BZColor_UnselectedBtn
        End With
        With (Chart_Yield.Series(0))
            '.Color = Color.Blue
            .YValueMembers = "Yield_Y"
            .XValueMember = "Day_X"
            .ChartType = DataVisualization.Charting.SeriesChartType.Line
        End With
        With (Chart_Yield.Series(1))
            '.Color = Color.Gold
            .YValueMembers = "Yield_Y"
            .XValueMember = "Day_X"
            .ChartType = DataVisualization.Charting.SeriesChartType.Point
        End With
        Me.Chart_Yield.DataBind()    '绑定数据源
    End Sub

    Private Sub TrackBar1_Subtraction_Click(sender As Object, e As EventArgs) Handles TrackBar1_Subtraction.Click
        If TrackBar1.Value <> 1 Then
            TrackBar1.Value = TrackBar1.Value - 1
            Call LoadChart_Curve_YieldRate()
        End If
    End Sub

    Private Sub TrackBar1_Addition_Click(sender As Object, e As EventArgs) Handles TrackBar1_Addition.Click
        If TrackBar1.Value <> 30 Then
            TrackBar1.Value = TrackBar1.Value + 1
            Call LoadChart_Curve_YieldRate()
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Call LoadChart_Curve_YieldRate()
    End Sub
#End Region

#Region "   抛料率刷新"
    Public Sub LoadChart_Curve_TossingRate()
        Dim dt_Tossing As New DataTable
        Dim dr_Tossing As DataRow
        Dim i, j As Integer

        j = CInt(TrackBar2.Value)

        dt_Tossing.Columns.Add("Day_X")
        dt_Tossing.Columns.Add("Tossing_Y")
        For i = Yield_Month.Count - j To Yield_Month.Count - 1
            dr_Tossing = dt_Tossing.NewRow()    '新增行
            dr_Tossing.Item(0) = Format(Yield_Month.Item(i).Date_Prouct, "MM.dd")
            If Yield_Month.Item(i).Count_Production = 0 Then
                dr_Tossing.Item(1) = 0.0
            Else
                dr_Tossing.Item(1) = Format(Yield_Month.Item(i).Tossing / (Yield_Month.Item(i).Count_Production + Yield_Month.Item(i).Tossing) * 100, "0.00")
                'dr_Tossing.Item(1) = Format(Yield_Month.Item(i).Tossing / (Yield_Month.Item(i).Count_Production) * 100, "0.00")
            End If

            dt_Tossing.Rows.Add(dr_Tossing)
        Next
        dr_Tossing = Nothing
        With Me.Chart_Tossing
            .DataSource = dt_Tossing
            .Series.Clear()
            .Legends.Clear()
            .ChartAreas.Clear()
            .Titles.Clear()
            .Titles.Add("Tossing Rate（Units: %）")
            .Titles(0).Font = New System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性
            .ChartAreas.Add("Tossing")
            .Legends.Add("Tossing")
            .Series.Add("Tossing")
            .Series.Add("Tossing_Point")
            .Series("Tossing").LegendToolTip = "压力数据"
            .Series("Tossing").IsValueShownAsLabel = True    '标签显示数据值
            '.Legends("Yield").DockedToChartArea = "Yield"     '指定Legend所属ChartArea
            .ChartAreas("Tossing").Area3DStyle.Enable3D = False  '启用3D样式

            .ChartAreas("Tossing").AxisY.Maximum = 100
            .ChartAreas("Tossing").AxisY.Minimum = 0
            .ChartAreas("Tossing").AxisX.LineColor = System.Drawing.Color.LightGray            '设置轴的线条颜色
            .ChartAreas("Tossing").AxisY.LineColor = System.Drawing.Color.LightGray            '设置轴的线条颜色
            .ChartAreas("Tossing").AxisX.MajorGrid.Interval = 1
            .ChartAreas("Tossing").AxisY.IsLabelAutoFit = True                             '设置是否自动调整轴标签  

            '.ChartAreas("Tossing").AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性 
            '.ChartAreas("Tossing").AxisY.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性 
            Me.Chart_Tossing.Legends("Tossing").Enabled = False

            .ChartAreas("Tossing").AxisX.MajorGrid.Enabled = True
            .ChartAreas("Tossing").AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas("Tossing").AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
            .ChartAreas("Tossing").AxisY.MajorGrid.Enabled = True
            .ChartAreas("Tossing").AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas("Tossing").AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

            .ChartAreas("Tossing").AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
            .ChartAreas("Tossing").AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
            .ChartAreas("Tossing").AxisX.MajorTickMark.Interval = 1
            .ChartAreas("Tossing").AxisY.MajorTickMark.Interval = 10
            .ChartAreas("Tossing").AxisX.LabelStyle.Angle = 0

            .ChartAreas("Tossing").BackColor = Color.White
            '.BackColor = BZColor_UnselectedBtn
        End With
        With (Chart_Tossing.Series(0))
            '.Color = Color.Blue
            .YValueMembers = "Tossing_Y"
            .XValueMember = "Day_X"
            .ChartType = DataVisualization.Charting.SeriesChartType.Line
        End With
        With (Chart_Tossing.Series(1))
            '.Color = Color.Gold
            .YValueMembers = "Tossing_Y"
            .XValueMember = "Day_X"
            .ChartType = DataVisualization.Charting.SeriesChartType.Point
        End With
        Me.Chart_Yield.DataBind()    '绑定数据源
    End Sub

    Private Sub TrackBar2_Subtraction_Click(sender As Object, e As EventArgs) Handles TrackBar2_Subtraction.Click
        If TrackBar2.Value <> 1 Then
            TrackBar2.Value = TrackBar2.Value - 1
            Call LoadChart_Curve_TossingRate()
        End If
    End Sub

    Private Sub TrackBar2_Addition_Click(sender As Object, e As EventArgs) Handles TrackBar2_Addition.Click
        If TrackBar2.Value <> 30 Then
            TrackBar2.Value = TrackBar2.Value + 1
            Call LoadChart_Curve_TossingRate()
        End If
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Call LoadChart_Curve_TossingRate()
    End Sub

#End Region

#Region "   UPH刷新"
    Public Sub LoadChart_Curve_UPH()
        Dim dt_UPH As New DataTable
        Dim dr_UPH As DataRow
        Dim i, j As Integer

        j = CInt(TrackBar3.Value)

        dt_UPH.Columns.Add("Day_X")
        dt_UPH.Columns.Add("UPH_Y")
        For i = Yield_Month.Count - j To Yield_Month.Count - 1
            dr_UPH = dt_UPH.NewRow()    '新增行
            dr_UPH.Item(0) = Format(Yield_Month.Item(i).Date_Prouct, "MM.dd")
            If Yield_Month.Item(i).Count_Production = 0 Then
                dr_UPH.Item(1) = 0.0
            Else
                dr_UPH.Item(1) = CInt(Yield_Month.Item(i).Count_Production / 24)
            End If
            dt_UPH.Rows.Add(dr_UPH)
        Next

        dr_UPH = Nothing
        With Me.Chart_UPH
            .DataSource = dt_UPH
            .Series.Clear()
            .Legends.Clear()
            .ChartAreas.Clear()
            .Titles.Clear()
            .Titles.Add("UPH（Units: Pcs）")
            .Titles(0).Font = New System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性
            .ChartAreas.Add("UPH")
            .Legends.Add("UPH")
            .Series.Add("UPH")
            '.Series.Add("UPH_Point")
            .Series("UPH").LegendToolTip = "压力数据"
            .Series("UPH").IsValueShownAsLabel = True    '标签显示数据值
            '.Legends("UPH").DockedToChartArea = "UPH"     '指定Legend所属ChartArea
            .ChartAreas("UPH").Area3DStyle.Enable3D = False  '启用3D样式

            .ChartAreas("UPH").AxisY.Maximum = 100
            .ChartAreas("UPH").AxisY.Minimum = 0
            .ChartAreas("UPH").AxisX.LineColor = System.Drawing.Color.LightGray            '设置轴的线条颜色
            .ChartAreas("UPH").AxisY.LineColor = System.Drawing.Color.LightGray            '设置轴的线条颜色
            .ChartAreas("UPH").AxisX.MajorGrid.Interval = 1
            .ChartAreas("UPH").AxisY.IsLabelAutoFit = True                             '设置是否自动调整轴标签  

            '.ChartAreas("UPH").AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性 
            '.ChartAreas("UPH").AxisY.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 10.25F, System.Drawing.FontStyle.Bold) '//设置Y轴左侧的提示信息的字体属性 
            Me.Chart_UPH.Legends("UPH").Enabled = False

            .ChartAreas("UPH").AxisX.MajorGrid.Enabled = True
            .ChartAreas("UPH").AxisX.MajorGrid.LineColor = Color.LightGray
            .ChartAreas("UPH").AxisX.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

            .ChartAreas("UPH").AxisY.MajorGrid.Enabled = True
            .ChartAreas("UPH").AxisY.MajorGrid.LineColor = Color.LightGray
            .ChartAreas("UPH").AxisY.MajorGrid.LineDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

            .ChartAreas("UPH").AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
            .ChartAreas("UPH").AxisY.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number

            .ChartAreas("UPH").AxisX.MajorTickMark.Interval = 1
            .ChartAreas("UPH").AxisY.MajorTickMark.Interval = 100
            .ChartAreas("UPH").AxisX.LabelStyle.Angle = 0

            .ChartAreas("UPH").BackColor = Color.White
            '.BackColor = BZColor_UnselectedBtn
        End With
        With (Chart_UPH.Series(0))
            '.Color = Color.Blue
            .XValueMember = "Day_X"
            .YValueMembers = "UPH_Y"
            .ChartType = DataVisualization.Charting.SeriesChartType.Column
        End With
        'With (Chart_UPH.Series(1))
        '    '.Color = Color.Gold
        '    .YValueMembers = "UPH_Y"
        '    .XValueMember = "Day_X"
        '    .ChartType = DataVisualization.Charting.SeriesChartType.Point
        'End With
        Me.Chart_UPH.DataBind()    '绑定数据源
    End Sub

    Private Sub TrackBar3_Subtraction_Click(sender As Object, e As EventArgs) Handles TrackBar3_Subtraction.Click
        If TrackBar3.Value <> 1 Then
            TrackBar3.Value = TrackBar3.Value - 1
            Call LoadChart_Curve_UPH()
        End If
    End Sub

    Private Sub TrackBar3_Addition_Click(sender As Object, e As EventArgs) Handles TrackBar3_Addition.Click
        If TrackBar3.Value <> 30 Then
            TrackBar3.Value = TrackBar3.Value + 1
            Call LoadChart_Curve_UPH()
        End If
    End Sub

    Private Sub TrackBar3_Scroll(sender As Object, e As EventArgs) Handles TrackBar3.Scroll
        Call LoadChart_Curve_UPH()
    End Sub

#End Region


End Class