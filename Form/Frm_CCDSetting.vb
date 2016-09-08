Public Class Frm_CCDSetting


    Private Sub Btn_Enable_Click(sender As Object, e As EventArgs) Handles Btn_Enable.Click
        Frm_Login.Show(Me)
        Btn_Enable.Enabled = False
        Login_Mode = 3
    End Sub

    Public Sub Change_Enable(ByVal enable As Boolean)
        If enable Then
            BZ_RoundPanel1.Enabled = True
            Btn_ReadPar.Enabled = True
            Btn_WritePar.Enabled = True
            Btn_Enable.Enabled = False
        Else
            BZ_RoundPanel1.Enabled = False
            Btn_ReadPar.Enabled = False
            Btn_WritePar.Enabled = False
            Btn_Enable.Enabled = True
        End If
    End Sub


    Private Sub Frm_CCDSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Par_SaveRead(2)
        Call Change_Enable(False)
    End Sub


    Private Sub Btn_WritePar_Click(sender As Object, e As EventArgs) Handles Btn_WritePar.Click
        Call Par_SaveRead(1)
        Call Change_Enable(False)
    End Sub


    ''' <summary>
    ''' Type=1 保存信息，=2 读取并显示
    ''' </summary>
    ''' <param name="type"></param>
    ''' <remarks></remarks>
    Public Sub Par_SaveRead(ByVal type As Integer)
        Select Case type
            Case 1
                For i = 1 To 30
                    For Each f As Object In TableLayoutPanel1.Controls
                        If f.name = "TextBox" & CStr(i) Then
                            par.CCD(i) = f.text
                        End If
                    Next
                Next

                '
                If Val(par.CCD(5)) = Val(par.CCD(6)) Or Val(par.CCD(5)) = Val(par.CCD(7)) Or Val(par.CCD(5)) = Val(par.CCD(8)) _
                    Or Val(par.CCD(6)) = Val(par.CCD(7)) Or Val(par.CCD(6)) = Val(par.CCD(8)) Or Val(par.CCD(7)) = Val(par.CCD(8)) Then
                    Frm_DialogAddMessage("不允许使用相同的串口号，请检查要保存的内容！")
                    Exit Sub
                Else
                    Frm_Main.COM1.Close()
                    Frm_Main.COM2.Close()
                    Frm_Main.COM3.Close()
                    Frm_Main.COM4.Close()
                    'Press S3
                    Frm_Main.COM1_Init(par.CCD(7))
                    'Laser S2
                    Frm_Main.COM2_Init(par.CCD(5))
                    'Laser S3
                    Frm_Main.COM3_Init(par.CCD(6))
                    'Calibration Loadcell
                    Frm_Main.COM4_Init(par.CCD(8))
                End If

                Call Write_par(Path_Par, par)
            Case 2
                Call Read_par(Path_Par, par)
                For i = 1 To 30
                    For Each f As Object In TableLayoutPanel1.Controls
                        If f.Name = "TextBox" & CStr(i) Then
                            f.text = par.CCD(i)
                        End If
                    Next
                Next
        End Select
    End Sub

    Private Sub Btn_ReadPar_Click(sender As Object, e As EventArgs) Handles Btn_ReadPar.Click
        Call Par_SaveRead(2)
    End Sub
End Class