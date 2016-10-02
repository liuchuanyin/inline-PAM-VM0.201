Public Module Mod_LogicErrCompensation
    Structure Corrections
        Public Position As Double
        Public DPCM As Double
        Public PTPDis As Double
    End Structure

    Public CorrectList_Past As New List(Of Corrections)
    Public CorrectList_PreTaker As New List(Of Corrections)

    Public Sub WriteXML_Corr(ByVal FileName As String, ByRef WriteData As List(Of Corrections))
        Try

            Dim writer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of Corrections)))
            Dim file As New System.IO.StreamWriter(FileName)
            writer.Serialize(file, WriteData)
            file.Close()
        Catch ex As Exception
            MsgBox("DownTime XML文件创建失败:" & ex.Message)
        End Try
    End Sub

    Public Function ReadXML_Corr(ByVal path As String, ByVal data As List(Of Corrections)) As List(Of Corrections)
        Try
            If IO.File.Exists(path) = False Then
                Call WriteXML_Corr(path, data)
            End If

            Dim reader As New System.Xml.Serialization.XmlSerializer(GetType(List(Of Corrections)))
            Dim file As New System.IO.StreamReader(path)
            ReadXML_Corr = CType(reader.Deserialize(file), List(Of Corrections))
            file.Close()
            Return ReadXML_Corr
        Catch ex As Exception
            MsgBox("DownTime XML文件读取失败:" & ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 校准误差
    ''' </summary>
    ''' <param name="Statue">程序控制变量</param>
    ''' <param name="Card">卡号</param>
    ''' <param name="Axis">主轴号</param>
    ''' <param name="StartPosi">开始位置</param>
    ''' <param name="EndPosi">结束位置</param>
    ''' <param name="PointNum">点位数</param>
    ''' <param name="path">文件存储路径</param>
    ''' <remarks></remarks>
    Public Sub CorrectionsProcess(ByRef Statue As sFlag4, Card As Integer, Axis As Integer, ByVal StartPosi As Double, ByVal EndPosi As Double, PointNum As Integer, path As String)
        Static Posi(PointNum - 1) As Double
        Static Index As Integer
        Static StartTime As Integer
        Dim MainEnc, FollowEnc As Double
        Dim CorrecVal As Corrections

        Select Case Statue.StepNum
            Case 10
                Statue.State = True
                Statue.Result = False

                CorrectList_Past.Clear()

                Statue.StepNum = 20

            Case 20
                For i = 0 To Posi.Length - 1
                    Posi(i) = StartPosi + i * (EndPosi - StartPosi) / (PointNum - 1)
                Next
                Statue.StepNum = 30

            Case 30
                Index = 0

                GT_ClrSts(Card, Axis + 1, 1)  '清除从轴轴报警标志
                GT_AxisOff(Card, Axis + 1) '当前从轴伺服OFF 

                Statue.StepNum = 50

            Case 50
                If AbsMotion(Card, Axis, 5, Posi(Index)) = True Then
                    Statue.StepNum = 60
                End If

            Case 60
                If isAxisMoving(Card, Axis) = False Then
                    StartTime = GetTickCount
                    Statue.StepNum = 80
                End If

            Case 80
                If GetTickCount - StartTime >= 3000 Then
                    Statue.StepNum = 90
                End If

            Case 90
                MainEnc = CurrEncPos(Card, Axis)
                FollowEnc = CurrEncPos(Card, Axis + 1)
                CorrecVal.Position = Posi(Index)
                CorrecVal.DPCM = (MainEnc - FollowEnc)
                CorrecVal.PTPDis = (EndPosi - StartPosi) / (PointNum - 1)

                CorrectList_Past.Add(CorrecVal)

                Statue.StepNum = 100

            Case 100
                Index = Index + 1

                If Index > Posi.Length - 1 Then
                    Statue.StepNum = 110
                Else
                    Statue.StepNum = 50
                End If

            Case 110
                If AbsMotion(Card, Axis, 20, Posi(0)) = True Then
                    Statue.StepNum = 120
                End If

            Case 120
                If isAxisMoving(Card, Axis) = False Then
                    StartTime = GetTickCount
                    Statue.StepNum = 130
                End If

            Case 130
                WriteXML_Corr(path, CorrectList_Past)

                ReDim Posi(PointNum - 1)
                Index = 0

                Statue.State = False
                Statue.Result = vbOK
                Statue.StepNum = 0

        End Select
    End Sub

    ''' <summary>
    ''' 压补正表进入轴卡
    ''' </summary>
    ''' <param name="Card">卡号</param>
    ''' <param name="Axis">从轴</param>
    ''' <param name="CorrectListTemp"></param>
    ''' <returns>0正常，1异常</returns>
    ''' <remarks></remarks>
    Public Function DPCM(Card As Integer, Axis As Integer, CorrectListTemp As List(Of Corrections)) As Integer
        Dim rtn, count As Integer
        Dim ChartA() As Integer
        Dim ChartB() As Integer
        Dim StartPosi, LenPosi As Integer

        ReDim ChartA(CorrectListTemp.Count - 1)
        ReDim ChartB(CorrectListTemp.Count - 1)

        For i = 0 To CorrectListTemp.Count - 1
            ChartA(i) = Math.Round(-CorrectListTemp.Item(i).DPCM * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))
            ChartB(i) = Math.Round(-CorrectListTemp.Item(i).DPCM * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))
        Next

        count = CorrectListTemp.Count
        StartPosi = Math.Round(CorrectListTemp.First.Position * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis))
        LenPosi = Math.Round(CorrectListTemp.Last.Position * AxisPar.pulse(Card, Axis) * AxisPar.Gear(Card, Axis) / AxisPar.Lead(Card, Axis) - StartPosi)

        rtn = GT_EnableLeadScrewComp(Card, Axis, 0)
        rtn += GT_ZeroPos(Card, Axis - 1, 2)
        rtn += GT_SetLeadScrewComp(Card, Axis, count, StartPosi, LenPosi, ChartA(0), ChartB(0))
        rtn += GT_EnableLeadScrewComp(Card, Axis, 1)

        If rtn <> 0 Then Return 1 Else Return 0
    End Function

    ''' <summary>
    ''' 像固高压入补正值
    ''' </summary>
    ''' <param name="Filepath">文件路径</param>
    ''' <param name="Card">卡号</param>
    ''' <param name="Axis">主轴号</param>
    ''' <remarks></remarks>
    Public Sub FileToDPCM(Filepath As String, Card As Integer, Axis As Integer)
        Dim CorrectListTemp As New List(Of Corrections)
        CorrectListTemp = ReadXML_Corr(Filepath, CorrectListTemp)
        DPCM(Card, Axis + 1, CorrectListTemp)
    End Sub

    Public Sub test(data As List(Of Corrections))
        Dim rtn As Integer
        Dim a() As Integer = {0, 15, 25, 35, 45}
        Dim b() As Integer = {0, 15, 25, 35, 45}
        rtn = GT_EnableLeadScrewComp(2, 2, 0)
        rtn += GT_ZeroPos(2, 1, 2)
        rtn += GT_SetLeadScrewComp(2, 2, 5, 0, 20000, a(0), b(0))
        rtn += GT_EnableLeadScrewComp(2, 2, 1)

    End Sub

End Module
