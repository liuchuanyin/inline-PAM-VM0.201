Public Class Frm_Alarm
    ''' <summary>
    ''' Frm Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Frm_Alarm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call ReadXML_Alarm(Path_AlarmFile, AlarmList)
        Call AlarmDataViewInit()
        Call LoadAlarmHistoryToDataGrid()
        Call LoadAlarmHistoryToPieChart()
    End Sub

#Region "   功能：DataGrid初始化"
    Public Structure DataViewInfo
        Dim ColumnName As String
        Dim HeaderText As String
        Dim Width As Integer
        Public Sub New(HeaderName As String, Header As String, HeaderWidth As Integer)
            ColumnName = HeaderName
            HeaderText = Header
            Width = HeaderWidth
        End Sub
    End Structure

    Public AlarmViewInfo() As DataViewInfo = {
          New DataViewInfo("C0", "Num", 40),
          New DataViewInfo("C1", "Date", 90),
          New DataViewInfo("C2", "Time", 90),
          New DataViewInfo("C3", "Code", 70),
          New DataViewInfo("C4", "Category", 130),
          New DataViewInfo("C5", "Duration", 92)
          }

    Public Sub AlarmDataViewInit()
        AlarmDataGridView.AllowUserToAddRows = False
        AlarmDataGridView.RowTemplate.Height = 25                                                             '行高度设置
        AlarmDataGridView.RowHeadersWidth = 8                                                            '行头的宽度设置
        AlarmDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        AlarmDataGridView.ColumnHeadersDefaultCellStyle.Font = New Font("HelveticaNeue", 10)
        AlarmDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        AlarmDataGridView.RowsDefaultCellStyle.Font = New Font("Calibri", 12, FontStyle.Regular)
        AlarmDataGridView.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        For i As Integer = 0 To AlarmViewInfo.Length - 1
            AlarmDataGridView.Columns.Add(AlarmViewInfo(i).ColumnName, AlarmViewInfo(i).HeaderText)
            AlarmDataGridView.Columns.Item(i).SortMode = DataGridViewColumnSortMode.NotSortable
            AlarmDataGridView.Columns.Item(i).Width = AlarmViewInfo(i).Width
            AlarmDataGridView.Columns.Item(i).DefaultCellStyle.ForeColor = Color.Black
        Next
    End Sub
#End Region

#Region "   功能：加载错误信息到DataGridView"

    ''' <summary>
    ''' 把本地所有的错误信息都加载到DataGridView
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadAlarmHistoryToDataGrid()
        Try
            AlarmDataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            AlarmDataGridView.Rows.Clear()
            For i As Integer = 0 To AlarmList.Count - 1
                AlarmDataGridView.Rows.Add()
                Dim tempNewRow As Integer = AlarmDataGridView.RowCount - 1
                AlarmDataGridView(0, tempNewRow).Value = AlarmDataGridView.RowCount
                AlarmDataGridView(1, tempNewRow).Value = AlarmList(i).dDate.ToString("dd/MM/yy")
                AlarmDataGridView(2, tempNewRow).Value = Format(DateTime.ParseExact(AlarmList(i).StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture), "HH:mm:ss")
                AlarmDataGridView(3, tempNewRow).Value = AlarmList(i).Code
                AlarmDataGridView(4, tempNewRow).Value = AlarmList(i).Category
                AlarmDataGridView(5, tempNewRow).Value = AlarmList(i).Duration

                If AlarmList(i).EndTime <> "" Then
                    Dim time1, time2 As DateTime
                    time1 = DateTime.ParseExact(AlarmList(i).StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)
                    time2 = DateTime.ParseExact(AlarmList(i).EndTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)
                    If (time2 - time1).Minutes >= 2 Then
                        AlarmDataGridView.Rows(i).DefaultCellStyle.BackColor = Color.Red
                    End If
                    If (time2 - time1).Minutes >= 1 And (time2 - time1).Minutes < 2 Then
                        AlarmDataGridView.Rows(i).DefaultCellStyle.BackColor = Color.Yellow
                    End If
                    If (time2 - time1).Minutes < 1 Then
                        AlarmDataGridView.Rows(i).DefaultCellStyle.BackColor = Color.White
                    End If
                End If
                AlarmDataGridView.Rows.Item(tempNewRow).Selected = False
            Next
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "   功能：数据添加到PieChart"
    Public Sub LoadAlarmHistoryToPieChart()
        Try
            Dim dr As DataRow
            Dim dt As New DataTable

            dt.Columns.Add("X轴")
            dt.Columns.Add("Y轴")

            Dim tmpTotalAlarmNum As Integer = 0
            CalCategory(tmpTotalAlarmNum)

            If AlarmList.Count = 0 Then
                dr = dt.NewRow()
                dr.Item(0) = "NO ERROR"
                dr.Item(1) = "-1"
                dt.Rows.Add(dr)
            End If

            For i As Integer = 0 To AlarmCategoryDataList.Count - 1
                dr = dt.NewRow()
                'dr.Item(0) = ""
                dr.Item(0) = AlarmCategoryDataList(i).Category
                dr.Item(1) = Val((AlarmCategoryDataList(i).Rate / tmpTotalAlarmNum * 100).ToString("f2"))
                'dr.Item(1) = AlarmCategoryDataList(i).Rate
                '******************************************************

                dt.Rows.Add(dr)
            Next

            dr = Nothing
            With Me.ChartCricle
                .DataSource = dt
                .Series.Clear()
                .Legends.Clear()
                .ChartAreas.Clear()
                .ChartAreas.Add("Y轴")
                .Legends.Add("Y轴")
                .Series.Add("Y轴")
                .Series("Y轴").LegendToolTip = "ALARM_INFO"
                .Series("Y轴").IsValueShownAsLabel = True        '标签显示数据值
                .Legends("Y轴").DockedToChartArea = "Y轴"        '指定Legend所属ChartArea
                .ChartAreas("Y轴").Area3DStyle.Enable3D = False  '启用3D样式
                .Legends("Y轴").Enabled = True
                .Legends("Y轴").Alignment = StringAlignment.Center
                .Legends("Y轴").Docking = DataVisualization.Charting.Docking.Top
                .Legends("Y轴").IsDockedInsideChartArea = False
                .Legends(0).BackColor = Color.FromArgb(238, 238, 238)
                .ChartAreas(0).BackColor = Color.FromArgb(238, 238, 238)
                .BackColor = Color.Transparent
            End With
            With (ChartCricle.Series(0))
                .YValueMembers = "Y轴"
                .XValueMember = "X轴"
                .ChartType = DataVisualization.Charting.SeriesChartType.Pie
                '.Label = "#VALX:#VAL"
                .Label = "#VALX"
                .SmartLabelStyle.CalloutStyle = DataVisualization.Charting.LabelCalloutStyle.Underlined
                .SmartLabelStyle.AllowOutsidePlotArea = DataVisualization.Charting.LabelOutsidePlotAreaStyle.Yes
                .SmartLabelStyle.Enabled = True
            End With
            Me.ChartCricle.DataBind()    '绑定数据源
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
#End Region

#Region "   功能：计算ErrorCode比例"
    Public Sub CalCategory(ByRef iTotalAlarmNum As Integer)
        Dim tmp As CategoryStatistics
        Dim j As Integer = 0
        Dim rate(ERROR_CODE.Length - 1) As Integer

        iTotalAlarmNum = 0
        For i As Integer = 0 To AlarmList.Count - 1
            For j = 0 To ERROR_CODE.Length - 1
                If AlarmList(i).Code = ERROR_CODE(j).ErrorCode Then
                    rate(j) = rate(j) + 1
                    iTotalAlarmNum = iTotalAlarmNum + 1
                End If
            Next
        Next

        '//加载数据到CateGory列表
        AlarmCategoryDataList.Clear()
        For K As Integer = 0 To ERROR_CODE.Length - 1
            If rate(K) <> 0 Then
                tmp.Category = ERROR_CODE(K).ErrorCode
                tmp.Rate = rate(K)
                AlarmCategoryDataList.Add(tmp)
            End If
        Next
    End Sub
#End Region

    Private Sub btn_Download_Click(sender As Object, e As EventArgs) Handles btn_Download.Click
        Call AlarmInfo_DownLoad("C:\Users\CTOS\Desktop\Alarm.csv")
    End Sub

    Private Sub AlarmDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles AlarmDataGridView.CellContentClick
        Dim DisplayCode As String
        Dim DisplayCategory As String

        If Me.AlarmDataGridView.RowCount - 1 > 0 Then
            DisplayCode = AlarmDataGridView.Item("C3", AlarmDataGridView.CurrentCell.RowIndex).Value
            DisplayCategory = AlarmDataGridView.Item("C4", AlarmDataGridView.CurrentCell.RowIndex).Value
            Me.ListBox_Alarm.Items.Clear()
            Me.ListBox_Alarm.Items.Add("● Code: " & DisplayCode & " , " & DisplayCategory & " Error")
            'Me.ListBox_Alarm.Items.Add("● Description: " & GetErrDescription(DisplayCode))
            Me.ListBox_Alarm.Items.Add("● Description: " & DisplayCategory)
        End If

    End Sub
End Class