Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Math
Public Class Frm_Home

#Region "功能：界面刷新，由Frm_Main中定时器控制，当OP操作界面可见时，执行该函数刷新OP界面"
    ''' <summary>
    ''' 变量刷新☞OP操作界面
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub VariablesRefresh()

        Dim Total, TotalNG As Integer
        For i = 0 To Yield_Month.Count - 1
            Total = Total + Yield_Month.Item(i).Count_Production
            TotalNG = TotalNG + Yield_Month.Item(i).Count_NG
        Next
        'YieldBar.BZ_MonthNG = CInt(Format(Yield_Data.Rate_NGTotal, "0.00") * 100)
        If Total = 0 Then
            YieldBar.BZ_MonthNG = 0
        Else
            YieldBar.BZ_MonthNG = CInt(Format(TotalNG / Total, "0.00") * 100)
        End If
        If Yield_Today.Count_Production = 0 Then
            YieldBar.BZ_DayNG = 0
        Else
            YieldBar.BZ_DayNG = CInt(Format(Yield_Today.Count_NG / Yield_Today.Count_Production, "0.00") * 100)
        End If


        BZ_Label3.BZ_BigText = Format(Yield_Today.Count_Production / 24, "0")
        'If CamNum < 10 Then
        'BZ_Label4.BZ_BigText = "Low"
        'Else
        'BZ_Label4.BZ_BigText = "High"
        'End If



    End Sub
#End Region

#Region "功能：Frm_Home OP 操作界面的操作"
    ''' <summary>
    ''' 良率 Reset
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Btn_Reset_Click(sender As Object, e As EventArgs) Handles Btn_Reset.Click
        If MessageBox.Show("确定要清除产量信息吗，请注意，该操作不可恢复！！！", "提示", MessageBoxButtons.YesNo) = DialogResult.No Then
            Exit Sub
        End If

    End Sub
#End Region

#Region "功能： CPK计算，显示图像"
    '*********************************************************************************************************************
    '功能：根据参数计算CPK各项参数，以数组形式返回
    '输入：生产数据数组，规格上限，规格下限，中心值
    '输出：最大值，最小值，变化范围，均值，标准差，Ca，Cpu，Cpl，Cp，CPK
    '*********************************************************************************************************************
    Private Function CPK_Calculate(ByVal Xn() As Double, USL As Double, ByVal LSL As Double, ByVal Center As Double) As Double()
        Dim X_Mean As Double 'X=(X1+X2+… …+Xn)/n 平均值；(n为样本数)
        Dim T As Double 'T=USL-LSL：即规格公差
        Dim Ca As Double 'Capability of Accuracy：制程准确度
        Dim Cp As Double 'Capability of Precision :制程精密度
        Dim Cpu As Double
        Dim Cpl As Double
        Dim CPK As Double 'Process Capability Index 的定义：制程能力指数
        Dim Sigma As Double '标准差
        Dim N As Integer = Xn.Count     'Xn的维数
        Dim Yn(N) As Double  ' 存储（Xi-X_Mean)*（Xi-X_Mean)
        Dim Result(10) As Double


        X_Mean = Xn.Sum / N
        T = USL - LSL
        For i = 0 To UBound(Xn)
            Yn(i) = (Xn(i) - X_Mean) * (Xn(i) - X_Mean)
        Next
        Sigma = Sqrt(Yn.Sum / (N - 1))

        Ca = (X_Mean - Center) / (T / 2)
        Cpu = (USL - X_Mean) / (3 * Sigma)
        Cpl = (X_Mean - LSL) / (3 * Sigma)
        Cp = (USL - LSL) / (6 * Sigma)
        CPK = Cp * Abs(1 - Ca)

        Result(0) = Xn.Max
        Result(1) = Xn.Min
        Result(2) = Result(0) - Result(1) 'Range
        Result(3) = X_Mean
        Result(4) = Sigma
        Result(5) = Ca
        Result(6) = Cpu
        Result(7) = Cpl
        Result(8) = Cp
        Result(9) = CPK

        Return Result

    End Function

    Private Sub CPK_Paint(ByVal Xn() As Double, ByVal USL As Double, ByVal LSL As Double, ByVal Center As Double)
        Chart_CPK.ChartAreas.Clear() '清除所有绘图区
        Dim CPK_Area As New ChartArea("CPK") '新增绘图区
        Chart_CPK.ChartAreas.Add(CPK_Area)
        Chart_CPK.Titles.Clear()
        Chart_CPK.ChartAreas("CPK").AxisX.Maximum = USL * 1.5
        Chart_CPK.ChartAreas("CPK").AxisX.Minimum = LSL * 1.5
        'Chart_CPK.ChartAreas("CPK").AxisX.LabelStyle.IsEndLabelVisible = False
        'Chart_CPK.ChartAreas("CPK").AxisX.IsLabelAutoFit = False
        Chart_CPK.ChartAreas("CPK").AxisX.LabelAutoFitStyle = LabelAutoFitStyles.None
        Chart_CPK.ChartAreas("CPK").AxisY.Maximum = 1.1
        Chart_CPK.ChartAreas("CPK").AxisY.Minimum = 0
        Chart_CPK.ChartAreas("CPK").AxisX.Interval = 0.5   '设置刻度间隔
        Chart_CPK.ChartAreas("CPK").AxisY.Interval = 0.2   '设置刻度间隔
        Chart_CPK.ChartAreas("CPK").AxisX.MajorGrid.Interval = 0.1 '设置网格线间隔
        Chart_CPK.ChartAreas("CPK").AxisY.MajorGrid.Interval = 0.1 '设置网格线间隔
        Chart_CPK.ChartAreas("CPK").AxisX.LineColor = Color.Gray
        Chart_CPK.ChartAreas("CPK").AxisY.LineColor = Color.Gray
        Chart_CPK.ChartAreas("CPK").AxisX.MajorGrid.LineColor = Color.Gray
        Chart_CPK.ChartAreas("CPK").AxisY.MajorGrid.LineColor = Color.Gray

        Dim LSL_Line, Target_Line, USL_Line As New DataVisualization.Charting.StripLine
        LSL_Line.BorderColor = Color.Green
        Target_Line.BorderColor = Color.Black
        USL_Line.BorderColor = Color.Red
        '>>设置线型
        LSL_Line.BorderDashStyle = ChartDashStyle.Dash
        Target_Line.BorderDashStyle = ChartDashStyle.Dash
        USL_Line.BorderDashStyle = ChartDashStyle.Dash
        '>>设置线相对与零点的偏移
        LSL_Line.IntervalOffset = LSL
        Target_Line.IntervalOffset = Center
        USL_Line.IntervalOffset = USL
        '>>设置线条的宽度
        LSL_Line.BorderWidth = 1.5
        Target_Line.BorderWidth = 1.4
        USL_Line.BorderWidth = 1.5
        '>> Add Line
        Chart_CPK.ChartAreas("CPK").AxisX.StripLines.Add(LSL_Line)
        Chart_CPK.ChartAreas("CPK").AxisX.StripLines.Add(Target_Line)
        Chart_CPK.ChartAreas("CPK").AxisX.StripLines.Add(USL_Line)

        '************** 散点图      ********************
        Chart_CPK.Series.Clear() '清除所有数据集
        Dim Scatter As New Series("Scatter") '新增数据集
        Scatter.ChartType = SeriesChartType.Point  '直线
        Scatter.BorderWidth = 1
        Scatter.MarkerStyle = MarkerStyle.Diamond
        Scatter.Color = Color.Red
        Scatter.XValueType = ChartValueType.Double
        Scatter.IsValueShownAsLabel = False
        Chart_CPK.Series.Add(Scatter)
        For i = 0 To UBound(Xn)
            Chart_CPK.Series("Scatter").Points.AddXY(Xn(i), 1.01)
        Next


        '************** Normal Dist      ********************
        Dim Normal_Dist_line As New Series("Normal Dist") '新增数据集
        Normal_Dist_line.ChartType = SeriesChartType.Spline   '直线
        Normal_Dist_line.BorderWidth = 2
        Normal_Dist_line.Color = Color.Blue
        Normal_Dist_line.XValueType = ChartValueType.Double
        Normal_Dist_line.IsValueShownAsLabel = False
        Chart_CPK.Series.Add(Normal_Dist_line)

        Dim X_Mean As Double 'X=(X1+X2+… …+Xn)/n 平均值；(n为样本数)
        Dim Sigma As Double '标准差
        Dim N As Integer = Xn.Count     'Xn的维数
        Dim Yn(N) As Double  ' 存储（Xi-X_Mean)*（Xi-X_Mean)
        Dim Normal_distX(63) As Double
        Dim Normal_distY(63) As Double
        Dim temp As Double
        Dim Shrink As Double = 0.95 '参数来自CPK计算表格
        Dim ScaleSpan As Double = 0.15 '参数来自CPK计算表格
        Dim PP(63) As Double
        Dim PI As Double = 3.14
        Dim Point_count As Integer = 64 '参数来自CPK计算表格
        Dim Bins_count As Integer = 40  '参数来自CPK计算表格
        Dim Bins(40) As Double
        Dim Values(40) As Double
        Dim Frequency(41) As Double
        Dim pt3_Average(41) As Double
        Dim Scale_(41) As Double

        Dim Scale_Max As Double = USL + ScaleSpan * (USL - Center)  '参数来自CPK计算表格
        Dim Scale_Min As Double = LSL - ScaleSpan * (Center - LSL)  '参数来自CPK计算表格
        Dim Point_interval As Double = (Scale_Max - Scale_Min) / Point_count
        Dim Bin_Width As Double = (Scale_Max - Scale_Min) / Bins_count

        X_Mean = Xn.Sum / N
        For i = 0 To UBound(Xn)
            Yn(i) = (Xn(i) - X_Mean) * (Xn(i) - X_Mean)
        Next
        Sigma = Sqrt(Yn.Sum / (N - 1))

        For i = 0 To 63
            Normal_distX(i) = Scale_Min + i * Point_interval
            temp = (Normal_distX(i) - X_Mean) * (Normal_distX(i) - X_Mean)
            temp = Exp(-temp / (2 * Sigma * Sigma))
            Normal_distY(i) = temp / (Sqrt(2 * PI) * Sigma)
            temp = 0
        Next
        For i = 0 To 63
            PP(i) = Shrink * Normal_distY(i) / Normal_distY.Max
            Chart_CPK.Series("Normal Dist").Points.AddXY(Normal_distX(i), PP(i))
        Next

        For i = 0 To 40
            Bins(i) = Scale_Min + i * Bin_Width
            Values(i) = Bins(i) - Bin_Width / 2
        Next

        For j = 0 To 31
            For i = 0 To 41
                If i = 0 Then
                    If Xn(j) <= Bins(0) Then
                        Frequency(0) += 1

                    End If
                ElseIf 0 < i And i < 41 Then
                    If Bins(i - 1) < Xn(j) And Xn(j) <= Bins(i) Then
                        Frequency(i) += 1
                    End If
                ElseIf i = 41 Then
                    If Xn(j) > Bins(i - 1) Then
                        Frequency(i) += 1
                    End If
                End If
            Next
        Next


        For i = 0 To 41
            If i = 0 Then
                pt3_Average(i) = (Frequency(i) + Frequency(i + 1)) / 2
            End If
            If 0 < i And i < 41 Then
                pt3_Average(i) = (Frequency(i - 1) + Frequency(i) + Frequency(i + 1)) / 3
            End If
            If i = 41 Then
                pt3_Average(i) = (Frequency(i - 1) + Frequency(i)) / 2
            End If
        Next

        Dim Max_pt3_Average As Double
        Max_pt3_Average = pt3_Average.Max
        For i = 0 To 41
            Scale_(i) = pt3_Average(i) / (Max_pt3_Average * 1.1)
        Next

        '************** Pop Density      ********************
        Dim Pop_Density_line As New Series("Pop Density") '新增数据集
        Pop_Density_line.ChartType = SeriesChartType.Spline    '直线
        Pop_Density_line.BorderWidth = 2
        Pop_Density_line.Color = Color.Green
        Pop_Density_line.XValueType = ChartValueType.Double
        Pop_Density_line.IsValueShownAsLabel = False
        Chart_CPK.Series.Add(Pop_Density_line)
        For i = 0 To 40
            Chart_CPK.Series("Pop Density").Points.AddXY(Values(i), Scale_(i))
        Next

        Chart_CPK.Series("Pop Density").IsVisibleInLegend = False '不显示图例
        Chart_CPK.Series("Normal Dist").IsVisibleInLegend = False '不显示图例
        Chart_CPK.Series("Scatter").IsVisibleInLegend = False '不显示图例


    End Sub

#End Region

    Private Sub BZ_Button7_Click(sender As Object, e As EventArgs) Handles BZ_Button7.Click
        Dim Xn() As Double
        Dim CPK_Result() As Double
        Xn = {0.1, 0.2, 0.1, 0.2, 0.111, 0.02, 0.11, 0.02, 0.15, 0.32, 0.1, 0.2, 0.1, -0.1, 0.1, 0.2, 0.1, -0.12, 0.1, 0.2, 0.1, -0.13, 0.1, 0.2, 0.1, 0.2, 0.1, 0.2, 0.1, 0.2, 0.1, 0.2}
        Call CPK_Paint(Xn, 1, -1, 0)
        CPK_Result = CPK_Calculate(Xn, 1, -1, 0)
        lbl_CPKResult.Text = "Count: " & Xn.Count.ToString & "      CPK: " & Format(CPK_Result(9), "0.00")
    End Sub
End Class