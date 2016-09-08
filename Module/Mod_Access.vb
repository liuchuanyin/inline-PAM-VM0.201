Imports System.Data.OleDb
Imports System.Math
Imports System.Text.StringBuilder
Imports VB = Microsoft.VisualBasic

Module Access
    Public Const conSearch As String = "Mod1PalBarcode"
    Public Const conSearch2 As String = "Mod1Time"
    Public Const conFilePahtName As String = "\\192.168.1.100\Server\PAMData.mdb"
    'Public Const conFilePahtName As String = "D:\Server\PAMData.mdb"
    Public Const conPassWord As String = "123"
    Public Const conSheetName As String = "Data"

    Public Structure PAMData
        Dim Time As String
        Dim Barcode As String
        Dim MoudleBarcode As String
        Dim CC As Double
        Dim Tossing As Integer
    End Structure

    ''' <summary>
    ''' 每台机要上传到数据库的数据
    ''' </summary>
    ''' <remarks></remarks>
    Public Data_Server As PAMData

#Region "功能：读取数据库"
    ''' <summary>
    ''' 从数据库读数据
    ''' </summary>
    ''' <param name="strFilePahtName">数据库的完整路径，含数据库名及后缀</param>
    ''' <param name="strPassWord">数据库密码</param>
    ''' <param name="strSheetName">表名</param>
    ''' <param name="strSerch">查询条件</param>
    ''' <param name="MachineID">几台号，0代表PAM-1,1代表PAM-2,2代表PAM-3,3代表PAM-4</param>
    ''' <param name="DataVaule">回传数据</param>
    ''' <returns>0表示OK，其他表示NG</returns>
    ''' <remarks></remarks>
    Public Function ReadDataBase(ByVal MachineID As Integer, ByRef DataVaule As PAMData, ByVal strSerch As String, Optional ByVal strFilePahtName As String = conFilePahtName, Optional ByVal strPassWord As String = conPassWord, Optional ByVal strSheetName As String = conSheetName) As Integer
        Dim strCon As String
        Dim conn As System.Data.OleDb.OleDbConnection
        Dim cmdString As String
        Dim cmd As OleDb.OleDbCommand
        Dim DataReader As OleDb.OleDbDataReader
        Dim en As Boolean = False

        Try
            DataVaule.Time = ""
            DataVaule.Barcode = ""
            DataVaule.MoudleBarcode = ""
            DataVaule.CC = 99999
            DataVaule.Tossing = 99999


            strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strFilePahtName & ";Jet OLEDB:Database Password=" & strPassWord
            conn = New System.Data.OleDb.OleDbConnection(strCon)   '建立连接
            conn.Open()                                            '打开连接

            cmdString = "select * from  [" & strSheetName & "] where [" & conSearch & "] = '" & strSerch & "'"
            cmd = New OleDb.OleDbCommand(cmdString, conn)
            DataReader = cmd.ExecuteReader                         '读取数据库的一种方法  

            While (DataReader.Read())                              '查询到多行读取OleDbDataReader的方法    
                en = True
                Select Case MachineID
                    Case 0
                        DataVaule.Time = DataReader.Item("Mod1Time").ToString
                        DataVaule.Barcode = DataReader.Item("Mod1PalBarcode").ToString
                        DataVaule.MoudleBarcode = DataReader.Item("Mod1Barcode").ToString
                        DataVaule.CC = Val(DataReader.Item("Mod1CC"))
                        DataVaule.Tossing = Val(DataReader.Item("Mod1Tossing"))
                    Case 1
                        DataVaule.Time = DataReader.Item("Mod2Time").ToString
                        DataVaule.Barcode = DataReader.Item("Mod2PalBarcode").ToString
                        DataVaule.MoudleBarcode = DataReader.Item("Mod2Barcode").ToString
                        DataVaule.CC = Val(DataReader.Item("Mod2CC"))
                        DataVaule.Tossing = Val(DataReader.Item("Mod2Tossing"))
                    Case 2
                        DataVaule.Time = DataReader.Item("Mod3Time").ToString
                        DataVaule.Barcode = DataReader.Item("Mod3PalBarcode").ToString
                        DataVaule.MoudleBarcode = DataReader.Item("Mod3Barcode").ToString
                        DataVaule.CC = Val(DataReader.Item("Mod3CC"))
                        DataVaule.Tossing = Val(DataReader.Item("Mod3Tossing"))
                    Case 3
                        DataVaule.Time = DataReader.Item("Mod4Time").ToString
                        DataVaule.Barcode = DataReader.Item("Mod4PalBarcode").ToString
                        DataVaule.MoudleBarcode = DataReader.Item("Mod4Barcode").ToString
                        DataVaule.CC = Val(DataReader.Item("Mod4CC"))
                        DataVaule.Tossing = Val(DataReader.Item("Mod4Tossing"))
                End Select
            End While

            conn.Close()

            If en Then Return 0 Else Return 1
        Catch ex As Exception
            ReadDataBase = Nothing
            MessageBox.Show(ex.Message)
            Return 1
        End Try
    End Function
#End Region

#Region "功能：写数据库"
    ''' <summary>
    ''' 从数据库读数据
    ''' </summary>
    ''' <param name="strFilePahtName">数据库的完整路径，含数据库名及后缀</param>
    ''' <param name="strPassWord">数据库密码</param>
    ''' <param name="strSheetName">表名</param>
    ''' <param name="strSerch">查询条件</param>
    ''' <param name="MachineID">几台号，0代表PAM-1,1代表PAM-2,2代表PAM-3,3代表PAM-4</param>
    ''' <param name="DataVaule">写入的数据</param>
    ''' <returns>0表示OK，其他表示NG</returns>
    ''' <remarks></remarks>
    Public Function WriteDataBase(ByVal MachineID As Integer, ByVal DataVaule As PAMData, ByVal strSerch As String, Optional ByVal strFilePahtName As String = conFilePahtName, Optional ByVal strPassWord As String = conPassWord, Optional ByVal strSheetName As String = conSheetName) As Integer
        Dim strCon As String
        Dim conn As System.Data.OleDb.OleDbConnection
        Dim strCommand As String
        Dim cmd As OleDbCommand
        Dim rtn As Integer
        Dim rs As System.Data.OleDb.OleDbDataReader
        Dim strNewTime As String = ""
        Dim intRecordNum As Integer

        Try
            strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strFilePahtName & ";Jet OLEDB:Database Password=" & strPassWord
            conn = New System.Data.OleDb.OleDbConnection(strCon)   '建立连接
            conn.Open()                                            '打开连接 

            strCommand = "select count(*)  from [" & strSheetName & "] where [" & conSearch & "] = '" & strSerch & "'"
            cmd = New OleDbCommand(strCommand, conn)
            intRecordNum = cmd.ExecuteScalar()

            If intRecordNum > 1 Then
                strCommand = "select max(Mod1Time) as Mod1Time from [" & strSheetName & "] where [" & conSearch & "] = '" & strSerch & "'"
                cmd = New OleDbCommand(strCommand, conn)
                rs = cmd.ExecuteReader()

                While (rs.Read())
                    strNewTime = rs.Item("Mod1Time")
                End While
            Else
                strNewTime = ""
            End If

            '表名及字段名最好用[]括起来，免得跟系统变量重名
            Select Case MachineID
                Case 0
                    strCommand = "INSERT INTO [" & strSheetName & "] VALUES ('" & DataVaule.Time & " ' , '" & DataVaule.Barcode & "' ,'" & DataVaule.MoudleBarcode & "' ," & DataVaule.CC & " ," & DataVaule.Tossing & _
                                                                            ",'' ,'' ,'' ,'' ,'' " & _
                                                                            ",'' ,'' ,'' ,'' ,'' " & _
                                                                            ",'' ,'' ,'' ,'' ,'' " & ") "
                    cmd = New OleDbCommand(strCommand, conn)
                    rtn = cmd.ExecuteNonQuery()
                Case 1
                    If intRecordNum > 1 Then
                        strCommand = "UPDATE [" & strSheetName & "] SET [Mod2Time] = '" & DataVaule.Time & "' , [Mod2PalBarcode] = '" & DataVaule.Barcode & "' , [Mod2Barcode] = '" & DataVaule.MoudleBarcode & "' , [Mod2CC] = " & DataVaule.CC & " , [Mod2Tossing] = " & DataVaule.Tossing & " where [" & conSearch & "] = '" & strSerch & "'" & " and [" & conSearch2 & "] = '" & strNewTime & "'"
                        cmd = New OleDbCommand(strCommand, conn)
                        rtn = cmd.ExecuteNonQuery()
                    Else
                        strCommand = "UPDATE [" & strSheetName & "] SET [Mod2Time] = '" & DataVaule.Time & "' , [Mod2PalBarcode] = '" & DataVaule.Barcode & "' , [Mod2Barcode] = '" & DataVaule.MoudleBarcode & "' , [Mod2CC] = " & DataVaule.CC & " , [Mod2Tossing] = " & DataVaule.Tossing & " where [" & conSearch & "] = '" & strSerch & "'"
                        cmd = New OleDbCommand(strCommand, conn)
                        rtn = cmd.ExecuteNonQuery()
                    End If
                Case 2
                    If intRecordNum > 1 Then
                        strCommand = "UPDATE [" & strSheetName & "] SET [Mod3Time] = '" & DataVaule.Time & "' , [Mod3PalBarcode] = '" & DataVaule.Barcode & "' , [Mod3Barcode] = '" & DataVaule.MoudleBarcode & "' , [Mod3CC] = " & DataVaule.CC & " , [Mod3Tossing] = " & DataVaule.Tossing & " where [" & conSearch & "] = '" & strSerch & "'" & " and [" & conSearch2 & "] = '" & strNewTime & "'"
                        cmd = New OleDbCommand(strCommand, conn)
                        rtn = cmd.ExecuteNonQuery()
                    Else
                        strCommand = "UPDATE [" & strSheetName & "] SET [Mod3Time] = '" & DataVaule.Time & "' , [Mod3PalBarcode] = '" & DataVaule.Barcode & "' , [Mod3Barcode] = '" & DataVaule.MoudleBarcode & "' , [Mod3CC] = " & DataVaule.CC & " , [Mod3Tossing] = " & DataVaule.Tossing & " where [" & conSearch & "] = '" & strSerch & "'"
                        cmd = New OleDbCommand(strCommand, conn)
                        rtn = cmd.ExecuteNonQuery()
                    End If
                Case 3
                    If intRecordNum > 1 Then
                        strCommand = "UPDATE [" & strSheetName & "] SET [Mod4Time] = '" & DataVaule.Time & "' , [Mod4PalBarcode] = '" & DataVaule.Barcode & "' , [Mod4Barcode] = '" & DataVaule.MoudleBarcode & "' , [Mod4CC] = " & DataVaule.CC & " , [Mod4Tossing] = " & DataVaule.Tossing & " where [" & conSearch & "] = '" & strSerch & "'" & " and [" & conSearch2 & "] = '" & strNewTime & "'"
                        cmd = New OleDbCommand(strCommand, conn)
                        rtn = cmd.ExecuteNonQuery()
                    Else
                        strCommand = "UPDATE [" & strSheetName & "] SET [Mod4Time] = '" & DataVaule.Time & "' , [Mod4PalBarcode] = '" & DataVaule.Barcode & "' , [Mod4Barcode] = '" & DataVaule.MoudleBarcode & "' , [Mod4CC] = " & DataVaule.CC & " , [Mod4Tossing] = " & DataVaule.Tossing & " where [" & conSearch & "] = '" & strSerch & "'"
                        cmd = New OleDbCommand(strCommand, conn)
                        rtn = cmd.ExecuteNonQuery()
                    End If

            End Select

            conn.Close()

            If rtn > 0 Then Return 0 Else Return 1
        Catch ex As Exception
            WriteDataBase = False
            MessageBox.Show(ex.Message)
            Return 1
        End Try
    End Function
#End Region

    Public Sub TimeTongBu()
        Shell("C:\Windows\system32\cmd.exe /c net use \\192.168.1.100 /delete", AppWinStyle.Hide)
        Shell("C:\Windows\system32\cmd.exe /c net use \\192.168.1.100\ipc$ """" /user:""administrator""", AppWinStyle.Hide)
        Shell("C:\Windows\system32\cmd.exe /c net time \\192.168.1.100 /set /yes", AppWinStyle.Hide)
    End Sub

End Module
