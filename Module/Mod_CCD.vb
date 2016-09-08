Module Mod_CCD

    ''' <summary>
    ''' Camera 1 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam1Data(22, 1) As Double
    ''' <summary>
    ''' Camera 2 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam2Data(0, 2) As Double
    ''' <summary>
    ''' 获取PAM1贴的CAMERA的SN
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam2SN(2) As String
    ''' <summary>
    ''' 获取PAM2贴的ROMEO的SN
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam4SN(2) As String
    ''' <summary>
    ''' Camera 3 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam3Data(72, 4) As Double
    ''' <summary>
    ''' Camera 4 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam4Data(20) As Double
    ''' <summary>
    ''' Camera 5 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam5Data(8, 1) As Double
    ''' <summary>
    ''' Camera 6 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam6Data(10) As Double
    ''' <summary>
    ''' Camera 7 Data
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam7Data(4) As Double
    ''' <summary>
    ''' 存储CCD返回的第2位数据,状态标志位
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam_Status(7) As Integer

    Public cmd_SendTime As Long

    ''' <summary>
    ''' 触发相机
    ''' </summary>
    ''' <param name="CamNo_FunNo">相机和功能编号：如T4,1</param>
    ''' <param name="SN"></param>
    ''' <remarks></remarks>
    Public Function TriggerCCD(ByVal CamNo_FunNo As String, ByVal SN As String) As Boolean
        Dim command As String
        Dim color As Short = 1
        Dim station As Short
        Dim CCD As Short

        If CCD_Lock_Flag Then
            Return False
            Exit Function
        End If

        CCD_Lock_Flag = True
        Try
            command = CamNo_FunNo & "," & SN & "," & MACTYPE & "," & color
            'If CamNo_FunNo = "T4,1" Or CamNo_FunNo = "T4,2" Or CamNo_FunNo = "T4,3" Then
            '    command = command & "," & CurrEncPos(0, S3_X).ToString & "," & CurrEncPos(0, S3_Y).ToString & "," & CurrEncPos(0, S3_R).ToString & vbCrLf
            'Else
            '    command = command & vbCrLf
            'End If

            CCD = CShort(Mid(CamNo_FunNo, 2, 1))
            Cam_Status(CCD) = 0
            Winsock1_String = ""
            Frm_Main.Winsock1.SendData(command)
            Winsock1_TimmingWatch = GetTickCount
            cmd_SendTime = GetTickCount
            If CCD = 1 Then
                station = 2
            ElseIf CCD = 2 Or CCD = 3 Or CCD = 4 Then
                station = 3
            ElseIf CCD = 5 Then
                station = 4
            ElseIf CCD = 6 Or CCD = 7 Then
                station = 5
            End If
            Write_Log(station, "CCD" & CCD & " Send:" & command.Replace(vbCrLf, ""), Path_Log) '记录发送的命令
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Group 指令
    ''' </summary>
    ''' <param name="SN">产品条码</param>
    ''' <remarks></remarks>
    Public Sub TriggerCCD(ByVal SN As String)
        Dim command As String
        command = "GROUP" & "," & SN & vbCrLf
        Frm_Main.Winsock1.SendData(command)
    End Sub

    Public Function CamData_Process(ByVal str_winsock As String) As Boolean

        Try
            Winsock1_Data = Split(str_winsock, ",")
            List_DebugAddMessage(str_winsock)
            Select Case Winsock1_Data(0)
                Case "T1"
                    Cam_Status(1) = CType(Winsock1_Data(2), Integer)
                    Write_Log(2, "CCD1 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(1) = 1 Then
                        For i = 1 To 22
                            Cam1Data(i, 0) = CType(Winsock1_Data(3 + 2 * (i - 1)), Double)  'Pos_XY.X
                            Cam1Data(i, 1) = CType(Winsock1_Data(4 + 2 * (i - 1)), Double)  'Pos_XY.Y
                        Next
                    End If

                Case "T2"
                    Cam_Status(2) = CType(Winsock1_Data(2), Integer)
                    Write_Log(3, "CCD2 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(2) = 1 Then
                        Select Case Winsock1_Data(1)
                            Case 1
                                Cam2Data(0, 0) = CType(Winsock1_Data(3), Double)  'Bracket X
                                Cam2Data(0, 1) = CType(Winsock1_Data(4), Double)  'Bracket Y
                                Cam2Data(0, 2) = CType(Winsock1_Data(5), Double)  'Bracket A

                            Case 2
                                Cam2SN(0) = Winsock1_Data(3) 'SN

                        End Select
                    End If

                Case "T3"
                    Cam_Status(3) = CType(Winsock1_Data(2), Integer)
                    Write_Log(3, "CCD3 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(3) = 1 Then
                        Select Case Winsock1_Data(1)
                            Case 1
                                For i = 1 To 30
                                    Cam3Data(i, 0) = CType(Winsock1_Data(3 + 4 * (i - 1)), Double)
                                    Cam3Data(i, 1) = CType(Winsock1_Data(4 + 4 * (i - 1)), Double)
                                    Cam3Data(i, 2) = CType(Winsock1_Data(5 + 4 * (i - 1)), Double)
                                    Cam3Data(i, 3) = CType(Winsock1_Data(6 + 4 * (i - 1)), Double)
                                Next
                            Case 2
                                'For i = 25 To 48
                                '    Cam3Data(i, 0) = CType(Winsock1_Data(3 + 4 * (i - 25)), Double)
                                '    Cam3Data(i, 1) = CType(Winsock1_Data(4 + 4 * (i - 25)), Double)
                                '    Cam3Data(i, 2) = CType(Winsock1_Data(5 + 4 * (i - 25)), Double)
                                '    Cam3Data(i, 3) = CType(Winsock1_Data(6 + 4 * (i - 25)), Double)
                                'Next
                            Case 3
                                'For i = 49 To 72
                                '    Cam3Data(i, 0) = CType(Winsock1_Data(3 + 4 * (i - 49)), Double)
                                '    Cam3Data(i, 1) = CType(Winsock1_Data(4 + 4 * (i - 49)), Double)
                                '    Cam3Data(i, 2) = CType(Winsock1_Data(5 + 4 * (i - 49)), Double)
                                '    Cam3Data(i, 3) = CType(Winsock1_Data(6 + 4 * (i - 49)), Double)
                                'Next
                        End Select
                    End If

                Case "T4"
                    Cam_Status(4) = CType(Winsock1_Data(2), Integer)
                    Write_Log(3, "CCD4 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(4) = 1 Then
                        Select Case CType(Winsock1_Data(1), Integer)
                            Case 1
                                If MACTYPE <> "PAM-1" Then
                                    Cam4Data(0) = CType(Winsock1_Data(3), Double)  'Target X
                                    Cam4Data(1) = CType(Winsock1_Data(4), Double)  'Target Y
                                    Cam4Data(2) = CType(Winsock1_Data(5), Double)  'Target A
                                End If

                            Case 2
                                Cam4Data(0) = CType(Winsock1_Data(3), Double)  'Target X
                                Cam4Data(1) = CType(Winsock1_Data(4), Double)  'Target Y
                                Cam4Data(2) = CType(Winsock1_Data(5), Double)  'Target A
                                Cam4Data(3) = CType(Winsock1_Data(6), Double)   'CC
                                Cam4Data(4) = CType(Winsock1_Data(7), Double)  'Brc_X
                                Cam4Data(5) = CType(Winsock1_Data(8), Double)  'Brc_Y
                                Cam4Data(6) = CType(Winsock1_Data(9), Double)  'Mod_X
                                Cam4Data(7) = CType(Winsock1_Data(10), Double)   'Mod_Y
                                Cam4Data(8) = CType(Winsock1_Data(11), Double)   'CC2

                            Case 3
                                If MACTYPE = "PAM-2" Then
                                    Cam4SN(0) = Winsock1_Data(3) 'SN
                                Else
                                    Cam4Data(0) = CType(Winsock1_Data(3), Double)  'Target X
                                    Cam4Data(1) = CType(Winsock1_Data(4), Double)  'Target Y
                                    Cam4Data(2) = CType(Winsock1_Data(5), Double)  'Target A
                                End If
                        End Select
                    End If

                Case "T5"
                    Cam_Status(5) = CType(Winsock1_Data(2), Integer)
                    Write_Log(4, "CCD5 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(5) = 1 Then
                        For i = 1 To 8
                            Cam5Data(i, 0) = CType(Winsock1_Data(3 + 2 * (i - 1)), Double)  'Pos_XY.X
                            Cam5Data(i, 1) = CType(Winsock1_Data(4 + 2 * (i - 1)), Double)  'Pos_XY.Y
                        Next
                    End If

                Case "T6"
                    Cam_Status(6) = CType(Winsock1_Data(2), Integer)
                    Write_Log(5, "CCD6 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(6) = 1 Then
                        Cam6Data(0) = CType(Winsock1_Data(3), Double)  'X  Bracket_X
                        Cam6Data(1) = CType(Winsock1_Data(4), Double)  'Y   Bracket_Y
                        Cam6Data(2) = CType(Winsock1_Data(5), Double)  'A   Module_X
                        Cam6Data(3) = CType(Winsock1_Data(6), Double)  'CC  Module_Y

                        Cam6Data(4) = CType(Winsock1_Data(7), Double)  'CC
                        Cam6Data(5) = CType(Winsock1_Data(8), Double)  'Brc_X
                        Cam6Data(6) = CType(Winsock1_Data(9), Double)  'Brc_Y
                        Cam6Data(7) = CType(Winsock1_Data(10), Double)  'Mod_X
                        Cam6Data(8) = CType(Winsock1_Data(11), Double)  'Mod_Y
                        Cam6Data(9) = CType(Winsock1_Data(12), Double)  'CC mod_brc
                    End If


                Case "T7"
                    Cam_Status(7) = CType(Winsock1_Data(2), Integer)
                    Write_Log(5, "CCD7 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(7) = 1 Then
                        Cam7Data(0) = CType(Winsock1_Data(3), Double)  'X
                        Cam7Data(1) = CType(Winsock1_Data(4), Double)  'Y
                        Cam7Data(2) = CType(Winsock1_Data(5), Double)  'A
                        Cam7Data(3) = CType(Winsock1_Data(6), Double)  'CC
                    End If
            End Select
        Catch ex As Exception
            CCD_Lock_Flag = False
            Return False
            'Frm_DialogAddMessage("接收CCD返回数据异常！")
            Exit Function
        End Try

        CCD_Lock_Flag = False
        Return True

    End Function

End Module
