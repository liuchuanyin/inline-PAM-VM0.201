Public Module Module1
    Structure Corrections
        Public Position As Double
        Public DPCM As Double
        Public PTPDis As Double
    End Structure

    Public CorrectList As New List(Of Corrections)

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

    Public Sub CorrectionsProcess(ByRef Statue As sFlag4, ByVal StartPosi As Double, ByVal EndPosi As Double, PointNum As Integer)
        Static Posi(PointNum - 1) As Double
        Static Index As Integer
        Static StartTime As Integer
        Dim MainEnc, FollowEnc As Double
        Dim CorrecVal As Corrections

        Select Case Statue.StepNum
            Case 10
                Statue.State = True
                Statue.Result = False

                CorrectList.Clear()

                Statue.StepNum = 20

            Case 20
                For i = 0 To Posi.Length - 1
                    Posi(i) = StartPosi + i * (EndPosi - StartPosi) / PointNum
                Next
                Statue.StepNum = 30

            Case 30
                Index = 0
                Statue.StepNum = 50

            Case 50
                If AbsMotion(2, PasteY1, 5, Posi(Index)) = True Then
                    Statue.StepNum = 60
                End If

            Case 60
                If isAxisMoving(2, PasteY1) = False Then
                    StartTime = GetTickCount
                    Statue.StepNum = 80
                End If

            Case 80
                If GetTickCount - StartTime >= 1000 Then
                    Statue.StepNum = 90
                End If

            Case 90
                MainEnc = CurrEncPos(2, PasteY1)
                FollowEnc = CurrEncPos(2, PasteY2)
                CorrecVal.Position = Posi(Index)
                CorrecVal.DPCM = MainEnc - FollowEnc
                CorrecVal.PTPDis = (EndPosi - StartPosi) / PointNum

                CorrectList.Add(CorrecVal)

                Statue.StepNum = 100

            Case 100
                Index = Index + 1

                If Index > Posi.Length - 1 Then
                    Statue.StepNum = 110
                Else
                    Statue.StepNum = 50
                End If

            Case 110
                WriteXML_Corr("E:\Corrections.xml", CorrectList)

                ReDim Posi(PointNum - 1)
                Index = 0

                Statue.State = False
                Statue.Result = vbOK
                Statue.StepNum = 0

        End Select
    End Sub


    Public Sub sxd()
        Dim A As Corrections
        A.Position = 20.15
        A.DPCM = 0.015

        CorrectList.Add(A)
        WriteXML_Corr("E:\Corrections.xml", CorrectList)
    End Sub

    Public Function CalculateOffset(SourcePosi As Double) As Double
        Dim xDoc As XDocument
        Dim mCorrection, nCorrection As Corrections

        xDoc = XDocument.Load("E:\Corrections.xml")

        Dim ps = From p In xDoc.Descendants("Corrections") _
                                Where CType(p.Element("Position"), Double) = SourcePosi _
                                Select New With {.Position = CType(p.Element("Position"), Double), _
                                                 .DPCM = p.Element("DPCM").Value, _
                                                 .PTPDis = p.Element("PTPDis").Value}
        '输出查询的数量()
        If ps.Count <= 0 Then

            Dim ps1 = From p1 In xDoc.Descendants("Corrections") _
                                Where Math.Abs((SourcePosi - CType(p1.Element("Position"), Double))) < CType(p1.Element("PTPDis"), Double) _
                                And (CType(p1.Element("PTPDis"), Double) + CType(p1.Element("Position"), Double)) > SourcePosi _
                                Select New With {.Position = CType(p1.Element("Position"), Double), _
                                                 .DPCM = p1.Element("DPCM").Value, _
                                                 .PTPDis = p1.Element("PTPDis").Value}

            Dim index As Integer = 0
            For Each x In ps1
                If index = 0 Then
                    mCorrection.Position = x.Position
                    mCorrection.DPCM = x.DPCM
                    mCorrection.PTPDis = x.PTPDis
                ElseIf index = 1 Then
                    nCorrection.Position = x.Position
                    nCorrection.DPCM = x.DPCM
                    nCorrection.PTPDis = x.PTPDis
                End If

                index = index + 1
            Next

            CalculateOffset = (SourcePosi - mCorrection.Position) / (nCorrection.Position - mCorrection.Position) * _
                            (nCorrection.DPCM - mCorrection.DPCM) + mCorrection.DPCM

        Else
            For Each n In ps
                CalculateOffset = n.DPCM
            Next
        End If

        Return CalculateOffset
        MessageBox.Show("OK")
    End Function












    'Dim ps2 = From p2 In xDoc.Descendants("Corrections") _
    '                    Where CType(p2.Element("Position"), Integer) = CType((mCorrection.Position + mCorrection.PTPDis), Integer) _
    '                    Select New With {.Position = CType(p2.Element("Position"), Double), _
    '                                     .DPCM = p2.Element("DPCM").Value, _
    '                                     .PTPDis = p2.Element("PTPDis").Value}

    'Dim ps2 = From p2 In xDoc.Descendants("Corrections") _
    '                    Where (SourcePosi + CType(p2.Element("PTPDis"), Double)) < CType(p2.Element("PTPDis"), Double) _
    '                    And (CType(p2.Element("PTPDis"), Double) + CType(p2.Element("Position"), Double)) > SourcePosi + CType(p2.Element("PTPDis"), Double) _
    '                    Select New With {.Position = CType(p2.Element("Position"), Double), _
    '                                     .DPCM = p2.Element("DPCM").Value, _
    '                                     .PTPDis = p2.Element("PTPDis").Value}

    'For Each x1 In ps2
    '    nCorrection.Position = x1.Position
    '    nCorrection.DPCM = x1.DPCM
    '    nCorrection.PTPDis = x1.PTPDis
    'Next

End Module
