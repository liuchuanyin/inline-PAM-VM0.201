Module Mod_Barcode

    Public BarcodeStrS1 As String
    Public BarcodeStrS3 As String

    ''' <summary>
    ''' 触发条码枪
    ''' </summary>
    ''' <param name="BarcodeScannerNo"></param>
    ''' <remarks></remarks>
    Public Function TriggerBarcodeScanner(ByVal BarcodeScannerNo As Short) As Boolean
        Dim command As String
        command = "+" & vbCrLf

        Try
            Select Case BarcodeScannerNo
                Case 1
                    '触发St 1条码枪
                    Frm_Main.Winsock2.SendData(command)
                    Write_Log(1, "Trigger Station 1 Barcode Scanner Send:" & command.Replace(vbCrLf, ""), Path_Log) '记录发送的命令

                Case 2
                    '触发St 3条码枪
                    Frm_Main.Winsock3.SendData(command)
                    Write_Log(3, "Trigger Station 3 Barcode Scanner Send:" & command.Replace(vbCrLf, ""), Path_Log) '记录发送的命令
            End Select
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Function BarData_Process(ByVal BarcodeScannerNo As Short, ByVal str As String) As Boolean
        Try
            Select Case BarcodeScannerNo
                Case 1
                    BarcodeStrS1 = str
                    Write_Log(1, "Trigger Station 1 Barcode Scanner Receive:" & str, Path_Log) '记录

                Case 2
                    BarcodeStrS3 = str
                    Write_Log(3, "Trigger Station 3 Barcode Scanner Receive:" & str, Path_Log) '记录

            End Select

            List_DebugAddMessage(str)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

End Module
