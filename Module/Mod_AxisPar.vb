Module Mod_AxisPar

    ''' <summary>
    ''' 参数界面-轴运行参数
    ''' </summary>
    ''' <remarks>##AABBCC</remarks>
    Public AxisPar As AxisParameter

    Public Structure AxisParameter
        Dim axisName(,) As String    '轴名称
        Public pulse(,) As Long       '每转脉冲数
        Public Lead(,) As Double      '导程
        Public Gear(,) As Integer     '减速比
        Public acc(,) As Double       '加速度
        Public dec(,) As Double       '减速度
        Public MoveVel(,) As Double   '移动速度
        Public HomeVel(,) As Double   '回零速度
        Public OrgOffset(,) As Double '原点偏移
        Public JogVel(,) As Double    '手动连续运动速度
        Public rate As Double
    End Structure

    ''' <summary>
    ''' 参数界面-轴运行参数  初始化
    ''' </summary>
    ''' <remarks>##AABBCC</remarks>
    Public Sub Load_AxisPar()
        ReDim AxisPar.axisName(2, 8)    '轴名称
        ReDim AxisPar.pulse(2, 8) 'As Long       '每转脉冲数
        ReDim AxisPar.Lead(2, 8) 'As Double      '导程
        ReDim AxisPar.Gear(2, 8) 'As Integer     '减速比
        ReDim AxisPar.acc(2, 8) 'As Double       '加速度
        ReDim AxisPar.dec(2, 8) 'As Double       '减速度
        ReDim AxisPar.MoveVel(2, 8) 'As Double   '移动速度
        ReDim AxisPar.HomeVel(2, 8) 'As Double   '回零速度
        ReDim AxisPar.OrgOffset(2, 8) 'As Double '原点偏移
        ReDim AxisPar.JogVel(2, 8) 'As Double    '手动连续运动速度 
    End Sub

#Region "功能：Read读取轴参数"
    '//<strPathFileFolder> 路径及文件(D:\db.mdb")
    '//<FormDataGridViewName> DataGridView控件所在的窗体及名称(DataGridView1)
    '//<DatabaseName> 数据库名称
    'Public Function DatabaseRead(ByVal strPathFileFolder As String, ByVal DatabaseName As String, ByVal FormDataGridViewName As DataGridView, ByRef booShow As Boolean)
    Public Function DatabaseRead(ByVal strPathFileFolder As String, ByVal DatabaseName As String) As String
        Dim DatabasePassword As String = "123"
        Load_AxisPar()
        Try
            '打开数据连接
            'Dim strsql As String = "select * from " & DatabaseName
            Dim strsql As String = "select * from " & DatabaseName & " order by ID asc"
            Dim str As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strPathFileFolder & "; Jet OLEDB:database Password=" & DatabasePassword & ";"
            Dim conn As OleDb.OleDbConnection
            conn = New OleDb.OleDbConnection(str)
            conn.Open()
            '更新数据源
            Dim da As New System.Data.OleDb.OleDbDataAdapter
            da = New System.Data.OleDb.OleDbDataAdapter(strsql, conn)
            '刷新数据
            Dim ds As New DataSet
            Dim card, axis, i As Short

            da.Fill(ds)
            'Frm_Engineering.DataGridView2.DataSource = ds.Tables(0)

            i = 2   '参数从第2行开始
            For card = 0 To GTS_CardNum - 1
                For axis = 1 To GTS_AxisNum(card)
                    AxisPar.axisName(card, axis) = ds.Tables(0).Rows(i)("N2")
                    AxisPar.MoveVel(card, axis) = ds.Tables(0).Rows(i)("N4")
                    AxisPar.HomeVel(card, axis) = ds.Tables(0).Rows(i)("N5")
                    AxisPar.pulse(card, axis) = ds.Tables(0).Rows(i)("N6")
                    AxisPar.Lead(card, axis) = ds.Tables(0).Rows(i)("N7")
                    AxisPar.Gear(card, axis) = ds.Tables(0).Rows(i)("N8")
                    AxisPar.acc(card, axis) = ds.Tables(0).Rows(i)("N9")
                    AxisPar.dec(card, axis) = ds.Tables(0).Rows(i)("N10")
                    AxisPar.OrgOffset(card, axis) = ds.Tables(0).Rows(i)("N11")
                    i += 1
                Next
            Next

            conn.Close()
            Return "OK"
        Catch ex As Exception
            Frm_DialogAddMessage("读取轴参数失败，请检查本地保存的轴参数数据库!")
            Return "NG"
        End Try
    End Function
#End Region

End Module
