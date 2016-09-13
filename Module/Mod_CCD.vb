Module Mod_CCD
      
    ''' <summary>
    ''' 存储CCD返回的第2位数据,状态标志位，1表示OK，0表示NG
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam_Status(7) As Integer

    ''' <summary>
    ''' CCD发送命令的时间
    ''' </summary>
    ''' <remarks></remarks>
    Public cmd_SendTime As Long

    ''' <summary>
    ''' CCD1返回的镭射点坐标
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam1LaserPoint() As Dist_XY

    ''' <summary>
    ''' CCD1返回的点胶点坐标
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam1GluePoint() As Dist_XY

    ''' <summary>
    ''' CCD1返回数据区分标志
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Cam1MatchFlg As String = "LC"

    ''' <summary>
    ''' CCD2返回的需要精补的X,Y,A
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam2Data As Dist_XYA

    ''' <summary>
    ''' CCD3返回的精补位置的坐标
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam3Data As Dist_XYA

    ''' <summary>
    ''' 获取PAM3贴的ROMEO的SN
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam3SN As String

    ''' <summary>
    ''' CCD4返回取料点的X,Y,A
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam4Data As Dist_XYA

    ''' <summary>
    ''' CCD4返回Module上的条码
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam4SN As String

    ''' <summary>
    ''' 复检下相机得到的数据
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam5Data() As String

    ''' <summary>
    ''' 复检上相机得到的数据
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam6Data() As String

    ''' <summary>
    ''' 触发相机
    ''' </summary>
    ''' <param name="CamNo_FunNo">相机和功能编号：如T4,1</param>
    ''' <param name="HoldIndex">穴位号</param>
    ''' <param name="Tray_SN">载具条码</param>
    ''' <param name="ModSN">Module条码</param>
    ''' <param name="color">颜色</param>
    ''' <returns>true表示成功，false表示失败</returns>
    ''' <remarks></remarks>
    Public Function TriggerCCD(ByVal CamNo_FunNo As String, HoldIndex As Integer, Tray_SN As String, ByVal ModSN As String, Optional color As String = "White") As Boolean
        Dim command As String
        Dim station As Short
        Dim CCD As Short

        If CCD_Lock_Flag Then
            Return False
            Exit Function
        End If

        CCD_Lock_Flag = True

        Try
            '根据通讯协议，来组织命令
            Select Case CamNo_FunNo
                Case "T1,1"
                    '复位CCD1相关数据
                    Cam_Status(1) = 0
                    ReDim Cam1GluePoint(0)
                    ReDim Cam1LaserPoint(0)

                    station = 1
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & "," & CurrEncPos(0, GlueX).ToString & "," & CurrEncPos(0, GlueY).ToString & vbCrLf
                Case "T2,1"
                    '复位CCD2相关数据
                    Cam_Status(2) = 0
                    Cam2Data.X = 0
                    Cam2Data.Y = 0
                    Cam2Data.A = 0

                    station = 4
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & "," & CurrEncPos(1, FineX).ToString & "," & CurrEncPos(1, FineY).ToString & vbCrLf
                Case "T3,1"
                    '复位CCD3相关数据
                    Cam_Status(3) = 0
                    Cam3Data.X = 0
                    Cam3Data.Y = 0
                    Cam3Data.A = 0
                     
                    station = 2
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & "," & CurrEncPos(0, PasteX).ToString & "," & CurrEncPos(2, PasteY1).ToString & "," & CurrEncPos(0, PasteR).ToString & vbCrLf
                Case "T3,2"
                    '复位CCD3相关数据 
                    Cam_Status(3) = 0
                    Cam3SN = ""

                    station = 2
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & vbCrLf
                Case "T4,1"
                    '复位CCD4相关数据 
                    Cam_Status(4) = 0 
                    Cam4Data.X = 0
                    Cam4Data.Y = 0
                    Cam4Data.A = 0
                    Cam4SN = ""

                    station = 3
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & "," & CurrEncPos(0, PreTakerX).ToString & "," & CurrEncPos(2, PreTakerY1).ToString & "," & CurrEncPos(1, PreTakerR).ToString & vbCrLf
                Case "T5,1"
                    '复位CCD5相关数据 
                    Cam_Status(5) = 0
                    ReDim Cam5Data(0)

                    station = 5
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & vbCrLf
                Case "T6,1"
                    '复位CCD6相关数据 
                    Cam_Status(6) = 0
                    ReDim Cam5Data(0)

                    station = 5
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & vbCrLf
                Case Else
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & vbCrLf
            End Select

            '发送数据
            Frm_Main.Winsock1.SendData(command)
            '记录发送时间
            Winsock1_TimmingWatch = GetTickCount
            cmd_SendTime = GetTickCount
            CCD = CShort(Mid(CamNo_FunNo, 2, 1))
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

    ''' <summary>
    ''' CCD数据处理程序
    ''' </summary>
    ''' <param name="str_winsock"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CamData_Process(ByVal str_winsock As String) As Boolean
        Dim MatchFlagBit, temp As Integer

        Try
            Winsock1_Data = Split(str_winsock, ",")
            List_DebugAddMessage(str_winsock)
            Select Case Winsock1_Data(0)
                Case "T1"
                    '记录标志位1为OK，0为NG
                    Cam_Status(1) = CType(Winsock1_Data(2), Integer)
                    '记录接收到的相机数据
                    Write_Log(1, "CCD1 Receive:" & str_winsock, Path_Log)
                    '判断是否CCD的结果为OK，并接受数据
                    If Cam_Status(1) = 1 Then
                        '先找到镭射点坐标集与点胶点坐标集的区分位置
                        For i = 0 To Winsock1_Data.Length - 1
                            If Winsock1_Data(i) = Cam1MatchFlg Then
                                MatchFlagBit = i
                                Exit For
                            End If
                        Next

                        '如果找不到标志位，则清除相机OK的标志位
                        If MatchFlagBit < 1 Then Cam_Status(1) = 0

                        '提取出镭射点坐标
                        temp = (MatchFlagBit - 4) / 2
                        '如果找不到镭射坐标数据集，则清除相机OK的标志位
                        If temp < 1 Then Cam_Status(1) = 0
                        ReDim Cam1LaserPoint(temp - 1)
                        For i = 0 To temp - 1
                            Cam1LaserPoint(i).X = CType(Winsock1_Data(i * 2 + 4), Double)
                            Cam1LaserPoint(i).Y = CType(Winsock1_Data(i * 2 + 5), Double)
                        Next

                        '提取出点胶点坐标
                        temp = (Winsock1_Data.Length - 2 - MatchFlagBit) / 2
                        '如果找不到点胶点坐标数据集，则清除相机OK的标志位
                        If temp < 1 Then Cam_Status(1) = 0
                        ReDim Cam1GluePoint(temp - 1)
                        For i = 0 To temp - 1
                            Cam1GluePoint(i).X = CType(Winsock1_Data(i * 2 + (MatchFlagBit + 1)), Double)
                            Cam1GluePoint(i).Y = CType(Winsock1_Data(i * 2 + (MatchFlagBit + 2)), Double)
                        Next 
                    End If

                Case "T2"
                    Cam_Status(2) = CType(Winsock1_Data(2), Integer)
                    '记录接收到的相机数据
                    Write_Log(4, "CCD2 Receive:" & str_winsock, Path_Log)
                    If Cam_Status(2) = 1 Then
                        '精补的坐标X
                        Cam2Data.X = CType(Winsock1_Data(4), Double)
                        '精补的坐标Y
                        Cam2Data.Y = CType(Winsock1_Data(5), Double)
                        '精补的坐标A
                        Cam2Data.A = CType(Winsock1_Data(6), Double)   
                    End If

                Case "T3"
                    Cam_Status(3) = CType(Winsock1_Data(2), Integer)
                    Write_Log(2, "CCD3 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(3) = 1 Then
                        Select Case Winsock1_Data(1)
                            Case 1
                                '精补点位的坐标X
                                Cam3Data.X = CType(Winsock1_Data(4), Double)
                                '精补点位的坐标Y
                                Cam3Data.Y = CType(Winsock1_Data(5), Double)
                                '精补点位的坐标A
                                Cam3Data.A = CType(Winsock1_Data(6), Double)
                            Case 2
                                Cam3SN = Winsock1_Data(4).ToString
                        End Select
                    End If

                Case "T4"
                    Cam_Status(4) = CType(Winsock1_Data(2), Integer)
                    Write_Log(3, "CCD4 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(4) = 1 Then
                        Cam4Data.X = CType(Winsock1_Data(4), Double)
                        Cam4Data.Y = CType(Winsock1_Data(5), Double)
                        Cam4Data.A = CType(Winsock1_Data(6), Double)
                        Cam4SN = CType(Winsock1_Data(7), Double)
                    End If

                Case "T5"
                    Cam_Status(5) = CType(Winsock1_Data(2), Integer)
                    Write_Log(4, "CCD5 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(5) = 1 Then
                       
                    End If

                Case "T6"
                    Cam_Status(6) = CType(Winsock1_Data(2), Integer)
                    Write_Log(5, "CCD6 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(6) = 1 Then
                        
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
