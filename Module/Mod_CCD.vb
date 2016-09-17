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
    ''' CCD3返回精补坐标
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam2Data(5, 50) As Double

    ''' <summary>
    ''' CCD3返回的精补点的坐标
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam3Data(5, 50) As Double

    ''' <summary>
    ''' 获取PAM3贴的ROMEO的SN
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam3SN As String

    ''' <summary>
    ''' CCD4返回取料点的X,Y,A
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam4Data(5, 50) As Double

    ''' <summary>
    ''' 返回的moduleSN
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam4SN As String
      
    ''' <summary>
    ''' 复检下相机得到的数据
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam5Data(5, 50) As String

    ''' <summary>
    ''' 复检上相机得到的数据
    ''' </summary>
    ''' <remarks></remarks>
    Public Cam6Data(5, 50) As String

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

        '复位得到的数据
        ReDim Winsock1_Data(200)

        Try
            '根据通讯协议，来组织命令
            Select Case CamNo_FunNo
                Case "T1,1"
                    '复位CCD1相关数据
                    Cam_Status(1) = 0
                    ReDim Cam1GluePoint(0)
                    ReDim Cam1LaserPoint(0)

                    station = 1
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & _
                              color & "," & CurrEncPos(0, GlueX).ToString & "," & CurrEncPos(0, GlueY).ToString & vbCrLf
                Case "T2,1"
                    '复位CCD2相关数据
                    Cam_Status(2) = 0
                    For i = 0 To 50
                        Cam2Data(1, i) = 0
                    Next

                    station = 4
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & _
                              color & "," & CurrEncPos(1, FineX).ToString & "," & CurrEncPos(1, FineY).ToString & vbCrLf
                Case "T2,2"
                    '复位CCD2相关数据
                    Cam_Status(2) = 0
                    For i = 0 To 50
                        Cam2Data(2, i) = 0
                    Next

                    station = 4
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & _
                              color & "," & CurrEncPos(1, FineX).ToString & "," & CurrEncPos(1, FineY).ToString & vbCrLf
                     
                Case "T3,1"
                    '复位CCD3相关数据
                    Cam_Status(3) = 0
                    For i = 0 To 50
                        Cam3Data(1, i) = 0
                    Next

                    station = 2
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & _
                              color & "," & CurrEncPos(0, PasteX).ToString & "," & CurrEncPos(2, PasteY1).ToString & "," & CurrEncPos(0, PasteR).ToString & vbCrLf
                Case "T3,2"
                    '复位CCD3相关数据 
                    Cam_Status(3) = 0
                    Cam3SN = ""

                    station = 2
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & _
                              color & vbCrLf
                Case "T4,1"
                    '复位CCD4相关数据 
                    Cam_Status(4) = 0
                    For i = 0 To 50
                        Cam4Data(1, i) = 0
                    Next 

                    station = 3
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & _
                              color & "," & CurrEncPos(0, PreTakerX).ToString & "," & CurrEncPos(2, PreTakerY1).ToString & "," & CurrEncPos(1, PreTakerR).ToString & vbCrLf
                Case "T5,1"
                    '复位CCD5相关数据 
                    Cam_Status(5) = 0
                    For i = 0 To 50
                        Cam5Data(1, i) = 0
                    Next

                    station = 5
                    command = CamNo_FunNo & "," & HoldIndex & "," & Tray_SN & "," & ModSN & "," & MACTYPE & "," & color & vbCrLf

                Case "T6,1"
                    '复位CCD6相关数据 
                    Cam_Status(6) = 0
                    For i = 0 To 50
                        Cam6Data(1, i) = 0
                    Next

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
                        Select Case Winsock1_Data(1)
                            Case 1
                               
                            Case 2
                                '精补的坐标X
                                Cam2Data(2, 0) = CType(Winsock1_Data(4), Double)
                                '精补的坐标Y
                                Cam2Data(2, 1) = CType(Winsock1_Data(5), Double)
                                '精补的坐标A
                                Cam2Data(2, 2) = CType(Winsock1_Data(6), Double)
                                '精确补偿CC
                                Cam2Data(2, 3) = CType(Winsock1_Data(7), Double)
                        End Select
                    End If

                Case "T3"
                    Cam_Status(3) = CType(Winsock1_Data(2), Integer)
                    Write_Log(2, "CCD3 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(3) = 1 Then
                        Select Case Winsock1_Data(1)
                            Case 1 
                                '精补点位的坐标X
                                Cam3Data(1, 0) = CType(Winsock1_Data(4), Double)
                                '精补点位的坐标Y
                                Cam3Data(1, 1) = CType(Winsock1_Data(5), Double)
                                '精补点位的坐标A
                                Cam3Data(1, 2) = CType(Winsock1_Data(6), Double)
                            Case 2 
                                Cam3SN = "" = Winsock1_Data(4).ToString
                        End Select
                    End If

                Case "T4"
                    Cam_Status(4) = CType(Winsock1_Data(2), Integer)
                    Write_Log(3, "CCD4 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(4) = 1 Then
                        Select Winsock1_Data(1)
                            Case 1
                                Cam4Data(1, 0) = CType(Winsock1_Data(4), Double)  'X
                                Cam4Data(1, 1) = CType(Winsock1_Data(5), Double)  'Y
                                Cam4Data(1, 2) = CType(Winsock1_Data(6), Double)  'A
                                Cam4SN = CType(Winsock1_Data(7), String)
                            Case 2
                        End Select
                    End If

                Case "T5"
                    Cam_Status(5) = CType(Winsock1_Data(2), Integer)
                    Write_Log(4, "CCD5 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(5) = 1 Then
                       Select Winsock1_Data(1)
                            Case 1

                        End Select
                    End If

                Case "T6"
                    Cam_Status(6) = CType(Winsock1_Data(2), Integer)
                    Write_Log(5, "CCD6 Receive:" & str_winsock, Path_Log) '记录接收到的相机数据
                    If Cam_Status(6) = 1 Then
                        Select Winsock1_Data(1)
                            Case 1

                        End Select
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

#Region "9/11点标定方法"

    ''' <summary>
    ''' 轴的表示
    ''' </summary>
    ''' <remarks></remarks>
    Structure Axis
        Public CardNum As Integer
        Public AxisNum As Integer
        Public SafePoint As Double
        Public Speed As Double
        Public IsEnable As Integer
    End Structure

    ''' <summary>
    ''' 相机标定需要的参数结构体
    ''' </summary>
    ''' <remarks></remarks>
    Structure CalibrationPar
        ''' <summary>
        ''' 相机号，如1，2，3
        ''' </summary>
        ''' <remarks></remarks>
        Public CameraNum As Integer

        ''' <summary>
        ''' 标定时X轴每次的移动的偏移量
        ''' </summary>
        ''' <remarks></remarks>
        Public OffsetX As Double

        ''' <summary>
        ''' 标定时Y轴每次的移动的偏移量
        ''' </summary>
        ''' <remarks></remarks>
        Public OffsetY As Double

        ''' <summary>
        ''' 标定时A轴每次的移动的偏移量
        ''' </summary>
        ''' <remarks></remarks>
        Public OffsetA As Double

        ''' <summary>
        ''' 9点或者11点
        ''' </summary>
        ''' <remarks></remarks>
        Public PointNum As Integer

        ''' <summary>
        ''' 标定起始点坐标
        ''' </summary>
        ''' <remarks></remarks>
        Public StartPosition As Dist_XYZA

        ''' <summary>
        ''' X轴
        ''' </summary>
        ''' <remarks></remarks>
        Public X As Axis

        ''' <summary>
        ''' Y轴
        ''' </summary>
        ''' <remarks></remarks>
        Public Y As Axis

        ''' <summary>
        ''' Z轴
        ''' </summary>
        ''' <remarks></remarks>
        Public Z As Axis

        ''' <summary>
        ''' R轴
        ''' </summary>
        ''' <remarks></remarks>
        Public A As Axis

    End Structure

    ''' <summary>
    ''' 标定子程序的步序号
    ''' </summary>
    ''' <remarks></remarks>
    Public Step_CCD_Calibration As Integer

    ''' <summary>
    ''' CCD标定子程序
    ''' </summary>
    ''' <param name="Par"></param>
    ''' <param name="status"></param>
    ''' <remarks></remarks>
    Public Sub CCD_Calibration(ByVal Par As CalibrationPar, ByRef status As sFlag3)
        Static TimingWatch As Long
        Static index_Point As Integer
        Static Pos_C(11) As Dist_XYA
        Dim strCommand As String

        Select Case Step_CCD_Calibration
            Case 0
                List_DebugAddMessage("CCD" & Par.CameraNum & "开始自动标定")
                status.State = True
                Step_CCD_Calibration = 10

            Case 10
                If Par.Z.IsEnable = True Then
                    Call AbsMotion(Par.Z.CardNum, Par.Z.AxisNum, Par.Z.AxisNum, Par.Z.SafePoint)
                    Step_CCD_Calibration = 20
                Else
                    Step_CCD_Calibration = 50
                End If

            Case 20
                If isAxisMoving(Par.Z.CardNum, Par.Z.AxisNum) = False Then
                    TimingWatch = GetTickCount
                    Step_CCD_Calibration = 50
                End If

            Case 50
                Call AbsMotion(Par.X.CardNum, Par.X.AxisNum, Par.X.Speed, Par.StartPosition.X)
                Call AbsMotion(Par.Y.CardNum, Par.Y.AxisNum, Par.Y.Speed, Par.StartPosition.Y)
                Step_CCD_Calibration = 60

            Case 60
                If isAxisMoving(Par.X.CardNum, Par.X.AxisNum) = False And isAxisMoving(Par.Y.CardNum, Par.Y.AxisNum) = False Then
                    Step_CCD_Calibration = 70
                End If

            Case 70
                If Par.A.IsEnable Then
                    Call AbsMotion(Par.A.CardNum, Par.A.AxisNum, Par.A.Speed, Par.StartPosition.A)
                    Step_CCD_Calibration = 80
                Else
                    Step_CCD_Calibration = 90
                End If

            Case 80
                If isAxisMoving(Par.A.CardNum, Par.A.AxisNum) = False Then
                    Step_CCD_Calibration = 90
                End If

            Case 90
                If Par.Z.IsEnable = True Then
                    Call AbsMotion(Par.Z.CardNum, Par.Z.AxisNum, Par.Z.AxisNum, Par.StartPosition.Z)
                    Step_CCD_Calibration = 150
                Else
                    TimingWatch = GetTickCount
                    Step_CCD_Calibration = 160
                End If

            Case 150
                If isAxisMoving(Par.Z.CardNum, Par.Z.AxisNum) = False Then
                    TimingWatch = GetTickCount
                    Step_CCD_Calibration = 160
                End If

            Case 160
                If GetTickCount - TimingWatch > 200 Then
                    Step_CCD_Calibration = 200
                End If

            Case 200  '发送标定开始的命令
                Winsock1_String = ""
                strCommand = "SC," & Par.CameraNum & "," & Par.PointNum & vbCrLf
                Frm_Main.Winsock1.SendData(strCommand)
                Winsock1_TimmingWatch = GetTickCount
                Step_CCD_Calibration = 210

            Case 210
                If Winsock1_String <> "" And Winsock1_Data(0) = "SC" Then
                    If Winsock1_Data(1) = 1 Then
                        List_DebugAddMessage("CCD" & Par.CameraNum & "确认OK，可以自动标定")
                        Step_CCD_Calibration = 220
                    ElseIf Winsock1_Data(1) = 0 Then
                        List_DebugAddMessage("CCD" & Par.CameraNum & "进入自动标定失败！")
                        Step_CCD_Calibration = 9000
                    End If
                ElseIf GetTickCount - Winsock1_TimmingWatch > 5000 Then
                    Step_CCD_Calibration = 9000
                    List_DebugAddMessage("CCD" & Par.CameraNum & "拍照超时，请检查！")
                End If

            Case 220
                Step_CCD_Calibration = 240

            Case 240
                'S 型路线
                Pos_C(1).X = Par.StartPosition.X
                Pos_C(1).Y = Par.StartPosition.Y
                Pos_C(1).A = Par.StartPosition.A
                For i = 2 To 3
                    Pos_C(i).X = Pos_C(i - 1).X + 15
                    Pos_C(i).Y = Pos_C(i - 1).Y
                    Pos_C(i).A = Par.StartPosition.A
                Next
                Pos_C(4).X = Pos_C(3).X
                Pos_C(4).Y = Pos_C(3).Y + 15
                Pos_C(4).A = Par.StartPosition.A
                For i = 5 To 6
                    Pos_C(i).X = Pos_C(i - 1).X - 15
                    Pos_C(i).Y = Pos_C(i - 1).Y
                    Pos_C(i).A = Par.StartPosition.A
                Next
                Pos_C(7).X = Pos_C(6).X
                Pos_C(7).Y = Pos_C(6).Y + 15
                Pos_C(7).A = Par.StartPosition.A
                For i = 8 To 9
                    Pos_C(i).X = Pos_C(i - 1).X + 15
                    Pos_C(i).Y = Pos_C(i - 1).Y
                    Pos_C(i).A = Par.StartPosition.A
                Next
                Pos_C(10).X = Pos_C(5).X
                Pos_C(10).Y = Pos_C(5).Y
                Pos_C(10).A = Par.StartPosition.A + 15
                Pos_C(11).X = Pos_C(5).X
                Pos_C(11).Y = Pos_C(5).Y
                Pos_C(11).A = Par.StartPosition.A - 15
                index_Point = 1
                Step_CCD_Calibration = 260

            Case 260  '走11个点位
                List_DebugAddMessage("CCD" & Par.CameraNum & "去第" & index_Point & "个标定点位")
                Call AbsMotion(Par.X.CardNum, Par.X.AxisNum, Par.X.Speed, Pos_C(index_Point).X)
                Call AbsMotion(Par.Y.CardNum, Par.Y.AxisNum, Par.Y.Speed, Pos_C(index_Point).Y)
                If Par.A.IsEnable Then
                    Call AbsMotion(Par.A.CardNum, Par.A.AxisNum, Par.A.Speed, Pos_C(index_Point).A)
                    Step_CCD_Calibration = 280
                Else
                    Step_CCD_Calibration = 300
                End If

            Case 280
                If isAxisMoving(Par.X.CardNum, Par.X.AxisNum) = False And isAxisMoving(Par.Y.CardNum, Par.Y.AxisNum) = False And isAxisMoving(Par.A.CardNum, Par.A.AxisNum) = False Then
                    TimingWatch = GetTickCount
                    Step_CCD_Calibration = 320
                End If

            Case 300
                If isAxisMoving(Par.X.CardNum, Par.X.AxisNum) = False And isAxisMoving(Par.Y.CardNum, Par.Y.AxisNum) = False Then
                    TimingWatch = GetTickCount
                    Step_CCD_Calibration = 320
                End If

            Case 320
                If GetTickCount - TimingWatch > 500 Then
                    Step_CCD_Calibration = 530
                End If

            Case 530
                Winsock1_String = ""
                strCommand = "C" & Par.CameraNum & "," & Pos_C(index_Point).X & "," & Pos_C(index_Point).Y & "," & Pos_C(index_Point).A & vbCrLf
                Frm_Main.Winsock1.SendData(strCommand)
                List_DebugAddMessage(strCommand)
                Step_CCD_Calibration = 540

            Case 540
                If Winsock1_String <> "" And (Winsock1_Data(0) = "C" & Par.CameraNum) Then
                    If Winsock1_Data(1) = 1 Then
                        Step_CCD_Calibration = 550
                    ElseIf Winsock1_Data(1) = 0 Then
                        List_DebugAddMessage("CCD" & Par.CameraNum & "进入自动标定失败！")
                        Step_CCD_Calibration = 9000
                    End If
                End If

            Case 550
                If index_Point = Par.PointNum Then
                    '11个点走完
                    Step_CCD_Calibration = 551
                Else
                    index_Point = index_Point + 1
                    Step_CCD_Calibration = 260
                End If

            Case 551
                Winsock1_String = ""
                Frm_Main.Winsock1.SendData("EC" & vbCrLf)
                TimingWatch = GetTickCount
                Step_CCD_Calibration = 552

            Case 552
                If Winsock1_String <> "" Then
                    Step_CCD_Calibration = 553
                ElseIf GetTickCount - TimingWatch > 5000 Then
                    Step_CCD_Calibration = 9000
                    List_DebugAddMessage("CCD" & Par.CameraNum & "连接异常，请检查！")
                End If

            Case 553
                If Winsock1_Data(0) = "EC" Then
                    If Winsock1_Data(1) = 1 Then
                        '标定成功
                        List_DebugAddMessage("CCD" & Par.CameraNum & "标定成功")
                        Step_CCD_Calibration = 560
                    ElseIf Winsock1_Data(1) = 0 Then
                        '标定失败
                        List_DebugAddMessage("CCD" & Par.CameraNum & "标定失败")
                        Step_CCD_Calibration = 9000
                    End If
                End If

            Case 560
                List_DebugAddMessage("CCD" & Par.CameraNum & "标定完成")
                Step_CCD_Calibration = 8000

            Case 8000
                status.Result = True
                status.Enable = False
                status.State = False
                Step_CCD_Calibration = 0

            Case 9000  '相机标定失败
                status.Result = False
                status.Enable = False
                status.State = False
                Step_CCD_Calibration = 0

        End Select
    End Sub
#End Region

End Module
