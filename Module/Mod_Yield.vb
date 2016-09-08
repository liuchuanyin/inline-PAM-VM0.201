Public Module Mod_Yield

    Public Structure Yield
        ''' <summary>
        ''' 生产日期
        ''' </summary>
        ''' <remarks></remarks>
        Dim Date_Prouct As Date
        ''' <summary>
        ''' 产量统计
        ''' </summary>
        ''' <remarks></remarks>
        Dim Count_Production As Integer
        ''' <summary>
        ''' 抛料数量统计
        ''' </summary>
        ''' <remarks></remarks>
        Dim Tossing As Integer
        ''' <summary>
        ''' NG统计
        ''' </summary>
        ''' <remarks></remarks>
        Dim Count_NG As Integer
    End Structure
    Public Yield_Today As [Yield]
    ''' <summary>
    ''' 生产数据结构体
    ''' </summary>
    ''' <remarks></remarks>
    Public Yield_Month As List(Of [Yield])

    Public Function Yield_Data_Init() As Boolean
        Dim Temp As Integer
        Try
            Call ReadXML_ProductionData(Path_YieldData, Yield_Month)
            If Yield_Month.Count = 0 Then
                Yield_Today.Count_NG = 0
                Yield_Today.Count_Production = 0
                Yield_Today.Tossing = 0
                Yield_Today.Date_Prouct = Now.Date.AddDays(-30)
                Yield_Month.Clear()
                Yield_Month.Add(Yield_Today)
                Call WriteXML_Production(Path_YieldData, Yield_Month)
            End If

            Temp = DateDiff(DateInterval.Day, Yield_Month.Item(Yield_Month.Count - 1).Date_Prouct, Now.Date)

            For i = Temp To 1 Step -1
                Yield_Today.Count_NG = 0
                Yield_Today.Count_Production = 0
                Yield_Today.Tossing = 0
                Yield_Today.Date_Prouct = Now.Date.AddDays(-i + 1)
                Yield_Month.Add(Yield_Today)
                Call WriteXML_Production(Path_YieldData, Yield_Month)
            Next

            Yield_Today.Count_NG = Yield_Month.Item(Yield_Month.Count - 1).Count_NG
            Yield_Today.Count_Production = Yield_Month.Item(Yield_Month.Count - 1).Count_Production

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' 生产产量信息增加
    ''' </summary>
    ''' <param name="OK">OK=True add 1 pcs OK, else add 1 pcs NG</param>
    ''' <remarks></remarks>
    Public Sub Yield_Add(ByVal OK As Boolean)
        Dim Temp As Integer

        '如果本地保存数据为空
        If Yield_Month.Count = 0 Then
            Yield_Today.Count_NG = 0
            Yield_Today.Count_Production = 0
            Yield_Today.Tossing = 0
            Yield_Today.Date_Prouct = Now.Date
            Yield_Month.Add(Yield_Today)
            Call WriteXML_Production(Path_YieldData, Yield_Month)
        Else
            Yield_Today.Count_NG = Yield_Month.Item(Yield_Month.Count - 1).Count_NG
            Yield_Today.Count_Production = Yield_Month.Item(Yield_Month.Count - 1).Count_Production
        End If

        '判断本地保存的最早一天到今天经历了多少天，如果有统计数据且距离今天超过30天，那么删除最早一天的数据
        If Yield_Month.Count > 1 Then
            If DateDiff(DateInterval.Day, Yield_Month.Item(0).Date_Prouct, Now.Date) >= 30 Then
                Yield_Month.RemoveAt(0)
            End If
        End If

        Temp = DateDiff(DateInterval.Day, Yield_Month.Item(Yield_Month.Count - 1).Date_Prouct, Now.Date)
        If Temp > 0 Then
            Yield_Today.Count_NG = 0
            Yield_Today.Count_Production = 0
            Yield_Today.Tossing = 0
            Yield_Today.Date_Prouct = Now.Date.AddDays(-Temp + 1)
            Yield_Month.Add(Yield_Today)
            Call WriteXML_Production(Path_YieldData, Yield_Month)
            Call Yield_Add(OK)  '递归调用
        Else
            If OK Then
                Yield_Today.Count_Production += 1
                Yield_Today.Date_Prouct = Now.Date
            Else
                Yield_Today.Count_NG += 1
                Yield_Today.Count_Production += 1
                Yield_Today.Date_Prouct = Now.Date
            End If
        End If
        Yield_Month.Item(Yield_Month.Count - 1) = Yield_Today
        Call WriteXML_Production(Path_YieldData, Yield_Month)

    End Sub

    ''' <summary>
    '''抛料数量增加
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Tossing_Add()
        Dim Temp As Integer

        '如果本地保存数据为空
        If Yield_Month.Count = 0 Then
            Yield_Today.Count_NG = 0
            Yield_Today.Count_Production = 0
            Yield_Today.Tossing = 0
            Yield_Today.Date_Prouct = Now.Date
            Yield_Month.Add(Yield_Today)
            Call WriteXML_Production(Path_YieldData, Yield_Month)
        Else
            Yield_Today.Count_NG = Yield_Month.Item(Yield_Month.Count - 1).Count_NG
            Yield_Today.Tossing = Yield_Month.Item(Yield_Month.Count - 1).Tossing
            Yield_Today.Count_Production = Yield_Month.Item(Yield_Month.Count - 1).Count_Production
        End If

        '判断本地保存的最早一天到今天经历了多少天，如果有统计数据且距离今天超过30天，那么删除最早一天的数据
        If Yield_Month.Count > 1 Then
            If DateDiff(DateInterval.Day, Yield_Month.Item(0).Date_Prouct, Now.Date) >= 30 Then
                Yield_Month.RemoveAt(0)
            End If
        End If

        Temp = DateDiff(DateInterval.Day, Yield_Month.Item(Yield_Month.Count - 1).Date_Prouct, Now.Date)
        If Temp > 0 Then
            Yield_Today.Count_NG = 0
            Yield_Today.Count_Production = 0
            Yield_Today.Tossing = 0
            Yield_Today.Date_Prouct = Now.Date.AddDays(-Temp + 1)
            Yield_Month.Add(Yield_Today)
            Call WriteXML_Production(Path_YieldData, Yield_Month)
            Call Tossing_Add()  '递归调用
        Else
            Yield_Today.Tossing += 1
            Yield_Today.Date_Prouct = Now.Date
        End If
        Yield_Month.Item(Yield_Month.Count - 1) = Yield_Today
        Call WriteXML_Production(Path_YieldData, Yield_Month)
    End Sub

    ''' <summary>
    ''' 读取本地保存的产量信息
    ''' </summary>
    ''' <param name="path"></param>
    ''' <param name="data"></param>
    ''' <remarks></remarks>
    Public Sub ReadXML_ProductionData(ByVal path As String, ByVal data As List(Of [Yield]))
        Try
            If IO.File.Exists(path) = False Then
                Call WriteXML_Production(path, data)
            End If

            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(List(Of [Yield])))
            Dim file As New System.IO.StreamReader(path)
            'Yield_Data为定义的共用变量，用来保存读取的信息
            Yield_Month = CType(reader.Deserialize(file), List(Of [Yield]))
            file.Close()
        Catch ex As Exception
            MsgBox("DownTime XML文件读取失败:" & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' 保存产量信息到本地
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="WriteData"></param>
    ''' <remarks></remarks>
    Public Sub WriteXML_Production(ByVal FileName As String, ByRef WriteData As List(Of [Yield]))
        Try
            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of [Yield])))
            Dim file As New System.IO.StreamWriter(FileName)
            writer.Serialize(file, WriteData)
            file.Close()
        Catch ex As Exception
            MsgBox("DownTime XML文件创建失败:" & ex.Message)
        End Try
    End Sub


End Module
