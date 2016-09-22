Module YieldStatistics
    ''' <summary>
    ''' 单个产品生产信息，用于记录统计产能
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure SingleProduct
        Public Time As Date
        Public ModSN As String
        Public TraySN As String
        Public IsGoodProduct As Boolean
    End Structure

    ''' <summary>
    ''' 抛料信息，用于记录统计抛料数
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure Tossing
        Public Time As Date
    End Structure

    ''' <summary>
    ''' 添加一个生产数据
    ''' </summary>
    ''' <param name="filePath">带路径的文件名</param>
    ''' <param name="TempData">要写入的数据</param>
    ''' <returns>返回0表示ok,返回1表示失败</returns>
    ''' <remarks></remarks>
    Public Function AddOnePCSProduct(filePath As String, TempData As SingleProduct) As Integer
        Dim xel, xEle As XElement
        Try
            If IO.File.Exists(filePath) = False Then
                xel = New XElement("StartInfo", "")
                xel.Save(filePath)
            End If

            xel = New XElement("SingleProduct", _
                                    New XElement("Time", TempData.Time), _
                                    New XElement("ModSN", TempData.ModSN), _
                                    New XElement("TraySN", TempData.TraySN), _
                                    New XElement("IsGoodProduct", TempData.IsGoodProduct))

            xEle = XElement.Load(filePath)
            xEle.Add(xel)
            xEle.Save(filePath)

            Return 0
        Catch ex As Exception
            Return 1
        End Try
    End Function

    ''' <summary>
    ''' 查询从当前开始计算，往上推N个小时的生产总数
    ''' </summary>
    ''' <param name="filePath">带路径的文件名，必须为Product_20160921.xml格式</param>
    ''' <param name="HoursNum">往上推的小时数</param>
    ''' <returns>正常返回查询到的数量，异常返回Nothing</returns>
    ''' <remarks></remarks>
    Public Function QueryInHourNum(filePath As String, HoursNum As Integer) As Integer
        Dim strPath() As String 
        Dim xDoc() As XDocument
        Dim m As Single
        Dim n, i, j As Integer
        Dim DateTemp As Date
        Dim DataMin As Date 

        Dim strTemp, strTemp1, strTemp2 As String

        Try
            '首先判断当前文件是否存在，如果不存在，则生成一个空文件xml
            If IO.File.Exists(filePath) = False Then
                Dim xel As XElement = New XElement("StartInfo", "")
                xel.Save(filePath)
            End If

            '根据输入的小时数，推算出需要的文件数
            If Hour(Now) - HoursNum < 0 Then
                m = Math.Abs(Hour(Now) - HoursNum)
                m = m / 24
                n = Math.Ceiling(m)
                If n > m Then
                    ReDim strPath(n) 
                    ReDim xDoc(n)
                Else
                    ReDim strPath(m) 
                    ReDim xDoc(m)
                End If

                '从路径中抽离中日期
                i = filePath.LastIndexOf("_")
                j = filePath.LastIndexOf(".")
                strTemp1 = filePath.Substring(0, i + 1)
                strTemp2 = filePath.Substring(j)
                strTemp = filePath.Substring(i + 1, j - i - 1)

                DateTemp = CDate(Format(CInt(strTemp), "0000-00-00"))

                For i = 0 To strPath.Length - 1
                    '根据当前日期提取N天前的日期，并转换成字符串
                    strPath(i) = DateAdd(DateInterval.Day, -i, DateTemp).ToString("yyyyMMdd")
                    '根据字符串，组成文件路径，并查找文件是否存在
                    If IO.File.Exists(strTemp1 & strPath(i) & strTemp2) = True Then 
                        '如果文件存在，即加载并搜索数据
                        xDoc(i) = XDocument.Load(strTemp1 & strPath(i) & strTemp2)  
                    End If 
                Next 
            Else
                ReDim strPath(0)
                'ReDim count(0)
                ReDim xDoc(0)

                '加载xml文件
                xDoc(0) = XDocument.Load(filePath) 
            End If

            DataMin = DateAdd(DateInterval.Hour, -HoursNum, Now)
            Dim ps = From p In xDoc.Descendants("SingleProduct") _
                                Where CType(p.Element("Time"), Date) < Now And CType(p.Element("Time"), Date) > DataMin _
                                Select New With {.time = CType(p.Element("Time"), Date), _
                                                 .ModSN = p.Element("ModSN").Value, _
                                                 .TraySN = p.Element("TraySN").Value, _
                                                 .IsGoodProduct = p.Element("IsGoodProduct").Value}
            '输出查询的数量()
            Return ps.Count
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
     
    ''' <summary>
    ''' 查询某段时间的生产数量
    ''' </summary>
    ''' <param name="filePath">带路径的文件名,必须为Product_20160921.xml格式</param>
    ''' <param name="startTime">起始时间</param>
    ''' <param name="endTime">终止时间</param>
    ''' <returns>这段时间的总数量</returns>
    ''' <remarks></remarks>
    Public Function QuerySegmentNum(filePath As String, startTime As Date, endTime As Date) As Integer
        Dim strPath() As String 
        Dim xDoc() As XDocument
        Dim m As Single
        Dim n, i, j As Integer
        Dim DateTemp As Date 
        Dim Duration As Long

        Dim strTemp1, strTemp2 As String

        Try
            '首先判断当前文件是否存在，如果不存在，则生成一个空文件xml
            If IO.File.Exists(filePath) = False Then
                Dim xel As XElement = New XElement("StartInfo", "")
                xel.Save(filePath)
            End If

            '根据输入起止时间，算出startTime（就是比较接近过去时间），endtime（接近现在）
            Duration = DateDiff(DateInterval.Hour, startTime, endTime)
            If Duration < 0 Then
                DateTemp = startTime
                startTime = endTime
                endTime = DateTemp
            ElseIf Duration = 0 Then
                If DateDiff(DateInterval.Minute, startTime, endTime) < 0 Then
                    DateTemp = startTime
                    startTime = endTime
                    endTime = DateTemp
                End If 
            End If

            m = Math.Abs(Duration)
            m = m / 24
            n = Math.Ceiling(m)
            If n > m Then
                ReDim strPath(n) 
                ReDim xDoc(n)
            Else
                ReDim strPath(m) 
                ReDim xDoc(m)
            End If

            '从路径中抽离中字符串
            i = filePath.LastIndexOf("_")
            j = filePath.LastIndexOf(".")
            strTemp1 = filePath.Substring(0, i + 1)
            strTemp2 = filePath.Substring(j)

            For i = 0 To strPath.Length - 1
                '根据当前起止的日期，并转换成字符串
                strPath(i) = DateAdd(DateInterval.Day, -i, endTime).ToString("yyyyMMdd")
                '根据字符串，组成文件路径，并查找文件是否存在
                If IO.File.Exists(strTemp1 & strPath(i) & strTemp2) = True Then
                    '如果文件存在，即加载并搜索数据
                    xDoc(i) = XDocument.Load(strTemp1 & strPath(i) & strTemp2)
                End If
            Next 
            Dim ps = From p In xDoc.Descendants("SingleProduct") _
                                Where CType(p.Element("Time"), Date) < endTime And CType(p.Element("Time"), Date) > startTime _
                                Select New With {.time = CType(p.Element("Time"), Date), _
                                                 .ModSN = p.Element("ModSN").Value, _
                                                 .TraySN = p.Element("TraySN").Value, _
                                                 .IsGoodProduct = p.Element("IsGoodProduct").Value}
            '输出查询的数量()
            Return ps.Count
        Catch ex As Exception
            Return Nothing
        End Try

























        'Try
        '    '首先判断当前文件是否存在，如果不存在，则生成一个空文件xml
        '    If IO.File.Exists(filePath) = False Then
        '        Dim xel As XElement = New XElement("StartInfo", "")
        '        xel.Save(filePath)
        '    End If

        '    '加载xml文件
        '    Dim xDoc As XDocument = XDocument.Load(filePath)

        '    '按条件进行查询
        '    Dim rows = From item In xDoc.Root.Elements Select item
        '    Dim ps = From p In xDoc.Descendants("SingleProduct") _
        '             Where Hour(CType(p.Element("Time"), Date)) = HoursNum _
        '             Select New With {.time = CType(p.Element("Time"), Date), _
        '                              .ModSN = p.Element("ModSN").Value, _
        '                              .TraySN = p.Element("TraySN").Value, _
        '                              .IsGoodProduct = p.Element("IsGoodProduct").Value}

        '    '输出查询的数量
        '    Return ps.Count

        'Catch ex As Exception
        '    Return Nothing
        'End Try
    End Function

End Module
