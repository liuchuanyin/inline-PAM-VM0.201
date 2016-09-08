Module Mod_IO_Refresh
    ''' <summary>
    ''' 规划器位置(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public CurrPrfPos(2, 8) As Double
    ''' <summary>
    ''' 编码器位置(card,axis)
    ''' </summary>
    ''' <remarks></remarks>
    Public CurrEncPos(2, 8) As Double

#Region "线程定义"
    ''' <summary>
    ''' IO刷新
    ''' </summary>
    ''' <remarks></remarks>
    Public Thread_IORefresh As New System.Threading.Thread(AddressOf IO_GTS)

#End Region

    Public Sub IO_Refresh()
        While True
            Call IO_GTS()
            Delay(10)
        End While
    End Sub

#Region "固高板卡IO刷新"
    ''' <summary>
    ''' 固高板卡IO刷新
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub IO_GTS()
        Dim i, j As Integer
        Dim sts, temp As Long
        Dim n As Integer
        Dim prfPos(8) As Double
        Dim encPos(8) As Double
        Dim en As Boolean

        While (True)
            If en Then
                Exit Sub
            End If
            en = True

            Try
                Threading.Thread.Sleep(10)

                For n = 0 To GTS_CardNum - 1 Step 1
                    '板卡输入信号
                    If GT_GetDi(n, MC_GPI, sts) = 0 Then     '
                        For i = 0 To 15
                            temp = sts And 2 ^ i
                            If temp = 0 Then
                                EXI(n, i) = True
                            Else
                                EXI(n, i) = False
                            End If
                        Next i
                    End If
                    '板卡输出信号
                    If GT_GetDo(n, MC_GPO, sts) = 0 Then
                        For i = 0 To 15
                            temp = sts And 2 ^ i
                            If temp = 0 Then
                                EXO(n, i) = True
                            Else
                                EXO(n, i) = False
                            End If
                        Next i
                    End If
                    'Home信号
                    j = GTS_AxisNum(n) - 1
                    If GT_GetDi(n, MC_HOME, sts) = 0 Then
                        For i = 0 To j
                            temp = sts And 2 ^ i
                            If temp = 0 Then
                                Home(n, i + 1) = True
                            Else
                                Home(n, i + 1) = False
                            End If
                        Next i
                    End If
                    '负极限
                    If GT_GetDi(n, MC_LIMIT_NEGATIVE, sts) = 0 Then
                        For i = 0 To j
                            temp = sts And 2 ^ i
                            If temp = 0 Then
                                LimitN(n, i + 1) = False
                            Else
                                LimitN(n, i + 1) = True
                            End If
                        Next i
                    End If
                    '正极限
                    If GT_GetDi(n, MC_LIMIT_POSITIVE, sts) = 0 Then
                        For i = 0 To j
                            temp = sts And 2 ^ i
                            If temp = 0 Then
                                LimitP(n, i + 1) = False
                            Else
                                LimitP(n, i + 1) = True
                            End If
                        Next i
                    End If

                    If GT_GetDi(n, MC_ALARM, sts) = 0 Then '1驱动器报警
                        For i = 0 To j
                            temp = sts And 2 ^ i
                            If temp = 0 Then
                                ServoErr(n, i + 1) = False
                            Else
                                ServoErr(n, i + 1) = True '驱动器报警
                            End If
                        Next i
                    End If

                    For i = 1 To GTS_AxisNum(n)
                        If GT_GetSts(n, i, sts, 1, 0) = 0 Then '9电机使能
                            temp = sts And 2 ^ 9
                            If temp = 0 Then
                                ServoOn(n, i) = False
                            Else
                                ServoOn(n, i) = True
                            End If
                        End If

                        'If GT_GetSts(n, i, sts, 1, 0) = 0 Then '11电机到位 
                        '    temp = sts And 2 ^ 10
                        '    If temp = 0 Then
                        '        MotorArrive(n, i) = True
                        '    Else
                        '        MotorArrive(n, i) = False
                        '    End If
                        'End If

                    Next i

                    '规划位置和编码器位置()
                    For i = 1 To GTS_AxisNum(n)
                        If GT_GetPrfPos(n, i, prfPos(i), 1, 0) = 0 Then
                            If AxisPar.pulse(n, i) > 0 Then
                                Select Case i
                                    Case 1, 2, 3, 4, 5, 6, 7, 8
                                        If n = 0 And i = 5 Then
                                            CurrPrfPos(n, i) = Format(prfPos(i) * AxisPar.Lead(n, i) * 360 / AxisPar.pulse(n, i) / AxisPar.Gear(n, i), "0.0000")
                                        ElseIf n = 1 And i = 1 Then
                                            CurrPrfPos(n, i) = Format(prfPos(i) * AxisPar.Lead(n, i) * 360 / AxisPar.pulse(n, i) / AxisPar.Gear(n, i), "0.0000")
                                        Else
                                            CurrPrfPos(n, i) = Format(prfPos(i) * AxisPar.Lead(n, i) / AxisPar.pulse(n, i), "0.0000")
                                        End If
                                End Select
                            End If
                        End If
                    Next i

                    For i = 1 To GTS_AxisNum(n)
                        If GT_GetEncPos(n, i, encPos(i), 1, 0) = 0 Then
                            If AxisPar.pulse(n, i) > 0 Then
                                Select Case i
                                    Case 1, 2, 3, 4, 5, 6, 7, 8
                                        If n = 0 And i = 5 Then
                                            CurrEncPos(n, i) = Format(encPos(i) * AxisPar.Lead(n, i) * 360 / AxisPar.pulse(n, i) / AxisPar.Gear(n, i), "0.0000")
                                        ElseIf n = 1 And i = 1 Then
                                            CurrEncPos(n, i) = Format(encPos(i) * AxisPar.Lead(n, i) * 360 / AxisPar.pulse(n, i) / AxisPar.Gear(n, i), "0.0000")
                                        Else
                                            CurrEncPos(n, i) = Format(encPos(i) * AxisPar.Lead(n, i) / AxisPar.pulse(n, i), "0.0000")
                                        End If
                                End Select
                            End If
                        End If
                    Next i
                Next n

                Dim rtn As Integer
                If GTS_Opened_EM Then
                    For n = 0 To GTS_ModNum - 1
                        rtn = GT_GetStsExtMdlGts(0, n, 0, sts)
                        If GT_GetStsExtMdlGts(0, n, 0, sts) = 0 Then
                            If sts = 0 Then
                                If GT_GetExtIoValueGts(0, n, sts) = 0 Then
                                    For i = 0 To 15
                                        temp = sts And 2 ^ i
                                        If temp = 0 Then
                                            EMI(n, i) = True
                                        Else
                                            EMI(n, i) = False
                                        End If
                                    Next i
                                End If
                                If GT_GetExtDoValueGts(0, n, sts) = 0 Then
                                    For i = 0 To 15
                                        temp = sts And 2 ^ i
                                        If temp = 0 Then
                                            EMO(n, i) = True
                                        Else
                                            EMO(n, i) = False
                                        End If
                                    Next i
                                End If
                            End If
                        Else
                            'MessageBox.Show(rtn)
                        End If
                    Next n
                End If

            Catch ex As Exception

            End Try
            en = False
        End While

    End Sub
#End Region

    ''' <summary>
    ''' 运动控制卡单点输出
    ''' </summary>
    ''' <param name="card">卡号从0开始</param>
    ''' <param name="index">IO端口号从0开始0-15</param>
    ''' <param name="_ON">True：输出低电平；False：输出高电平</param>
    ''' <remarks></remarks>
    Public Sub SetEXO(card As Short, index As Short, ByVal _ON As Boolean)
        Dim value As Short
        If Not GTS_Opened_Card Then
            Exit Sub
        End If
        If _ON Then
            value = 0
        Else
            value = 1
        End If
        Call GT_SetDoBit(card, MC_GPO, index + 1, value)
    End Sub

    ''' <summary>
    ''' 扩展模块单点输出
    ''' </summary>
    ''' <param name="mdl">从0开始</param>
    ''' <param name="index">IO端口号从0开始0-15</param>
    ''' <param name="_ON">True：输出低电平；False：输出高电平</param>
    ''' <remarks></remarks>
    Public Sub SetEMO(mdl As Short, index As Short, ByVal _ON As Boolean)
        Dim value As Short
        Dim rtn As Short
        Dim cardum = 0

        If Not GTS_Opened_EM Then
            Exit Sub
        End If
        If _ON Then
            value = 0
        Else
            value = 1
        End If
        rtn = GT_SetCardNo(cardum)
        Call GT_SetExtIoBitGts(cardum, mdl, index, value)
    End Sub

End Module
