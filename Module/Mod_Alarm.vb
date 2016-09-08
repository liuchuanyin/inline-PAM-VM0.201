Public Module Mod_Alarm
    Public Structure AlarmHistory
        Dim dDate As Date
        Dim Code As String
        Dim Category As String
        Dim StartTime As String
        Dim EndTime As String
        Dim Duration As String
    End Structure

    Public AlarmList As List(Of AlarmHistory)

    Public Structure CategoryStatistics '//报警统计信息
        Dim Category As String
        Dim Rate As Double
    End Structure
    Public AlarmCategoryDataList As List(Of CategoryStatistics) = New List(Of CategoryStatistics)

    Public Structure ERR_INFO
        Dim ErrorCode As String
        Dim ErrDescription As String
        Dim NUM As Integer
        Public Sub New(strErrorCode As String, strErrDescription As String)
            ErrorCode = strErrorCode
            ErrDescription = strErrDescription
        End Sub
    End Structure

    Public ERROR_CODE() As ERR_INFO = {
New ERR_INFO("A0000", "Robot Alarm"),
New ERR_INFO("A0001", "Robot Alarm"),
New ERR_INFO("A0002", "Servo Alarm"),
New ERR_INFO("A0003", "Sensor Alarm"),
New ERR_INFO("E0005", "E-stop Alarm")
}


    ''' <summary>
    ''' 报警写入本地文件，如果已经有报警信息且未被清除，那么本次报警信息写入无效
    ''' </summary>
    ''' <param name="Code">错误代码</param>
    ''' <param name="Category">错误分类</param>
    ''' <remarks></remarks>
    Public Sub Alarm_Write(ByVal Code As String, ByVal Category As String)
        Dim tempAlarm As AlarmHistory
        tempAlarm.dDate = Now.Date
        tempAlarm.Code = Code
        tempAlarm.Category = Category
        tempAlarm.StartTime = Format(Now, "yyyyMMddHHmmss")
        tempAlarm.EndTime = ""
        tempAlarm.Duration = "Reparing"
        If AlarmList.Count = 0 Then
            AlarmList.Add(tempAlarm)
        Else
            If AlarmList.Item(AlarmList.Count - 1).EndTime <> "" Then
                AlarmList.Add(tempAlarm)
            End If
        End If
        Call WriteXML_Alarm(Path_AlarmFile, AlarmList)
    End Sub

    ''' <summary>
    ''' 结束本地保存的最后一次报警信息，维修完成
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Alarm_End()
        Dim tempAlarm As AlarmHistory
        Dim time1, time2 As DateTime
        Dim HH, MM, SS As Integer

        If AlarmList.Item(AlarmList.Count - 1).EndTime = "" Then
            tempAlarm.dDate = AlarmList.Item(AlarmList.Count - 1).dDate
            tempAlarm.Code = AlarmList.Item(AlarmList.Count - 1).Code
            tempAlarm.Category = AlarmList.Item(AlarmList.Count - 1).Category
            tempAlarm.StartTime = AlarmList.Item(AlarmList.Count - 1).StartTime
            tempAlarm.EndTime = Format(Now, "yyyyMMddHHmmss")
            time1 = DateTime.ParseExact(tempAlarm.StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)
            time2 = DateTime.ParseExact(tempAlarm.EndTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture)
            HH = (time2 - time1).Hours
            MM = (time2 - time1).Minutes
            SS = (time2 - time1).Seconds
            tempAlarm.Duration = Format(HH, "00") & ":" & Format(MM, "00") & ":" & Format(SS, "00")
            AlarmList.Item(AlarmList.Count - 1) = tempAlarm
            Call WriteXML_Alarm(Path_AlarmFile, AlarmList)
        End If
    End Sub

    ''' <summary>
    ''' 读取本地保存的ALarm信息
    ''' </summary>
    ''' <param name="path"></param>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Public Sub ReadXML_Alarm(ByVal path As String, ByRef data As List(Of AlarmHistory))
        Try
            If IO.File.Exists(path) = False Then
                Call WriteXML_Alarm(path, data)
            End If

            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(List(Of AlarmHistory)))
            Dim file As New System.IO.StreamReader(path)
            data = CType(reader.Deserialize(file), List(Of AlarmHistory))
            file.Close()
        Catch ex As Exception
            MsgBox("ALARM XML文件读取失败:" & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 保存Alarm信息到本地
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="WriteData"></param>
    ''' <remarks></remarks>
    Public Sub WriteXML_Alarm(ByVal FileName As String, ByRef WriteData As List(Of AlarmHistory))
        Try
            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of AlarmHistory)))
            Dim file As New System.IO.StreamWriter(FileName)
            writer.Serialize(file, WriteData)
            file.Close()
        Catch ex As Exception
            MsgBox("ALARM XML文件创建失败:" & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 把所有的错误信息下载到本地CSV文件
    ''' </summary>
    ''' <param name="path">路径</param>
    ''' <remarks></remarks>
    Public Sub AlarmInfo_DownLoad(ByVal path As String)
        Dim str As String = ""
        Dim strHead As String = ""

        strHead = "Num,Data,Time,Code,Category,Duration"
        If Dir(path, vbDirectory) <> "" Then
            FileSystem.Kill(path)
        End If

        WriteCsvFile(path, strHead)
        For i As Integer = 0 To AlarmList.Count - 1
            str = (i + 1).ToString & "," & _
                    AlarmList(i).dDate.ToString("yyyyMMdd") & "," & _
                    Format(DateTime.ParseExact(AlarmList(i).StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture), "HH:mm:ss") & "," & _
                    AlarmList(i).Code & "," & _
                    AlarmList(i).Category & "," & _
                    AlarmList(i).Duration
            WriteCsvFile(path, str)
        Next
    End Sub

#Region "功能：CSV文件写入"
    Public Sub WriteCsvFile(ByVal Filename As String, ByVal Savedata As String)
        Dim rs As New System.IO.StreamWriter(Filename, True)
        Try
            rs.WriteLine(Savedata)
            rs.Close()
        Catch ex As Exception
            MsgBox("文件读取失败：" & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "错误")
            rs.Close()
        End Try
    End Sub

    Public Sub AppendCsvFile(ByVal Filename As String, ByVal Savedata As String)
        Dim rs As New System.IO.StreamWriter(Filename, True)
        Try
            rs.Write(Savedata)
            rs.Close()
        Catch ex As Exception
            MsgBox("文件读取失败：" & ex.Message, MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "错误")
            rs.Close()
        End Try
    End Sub

#End Region

End Module
