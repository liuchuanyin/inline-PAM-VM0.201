Public Class Frm_Login

    Private Sub Frm_Login_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Location = New Point(205, 180)
        Me.Size = New Size(560, 220)
        Me.ShowInTaskbar = False

        'OP
        ComboBox_UserName.Items.Add(BOZHON.User1.Name)
        'BOTECH
        ComboBox_UserName.Items.Add(BOZHON.User2.Name)
        'SW Engineering
        ComboBox_UserName.Items.Add(BOZHON.User3.Name)
        '选中BOTECH
        ComboBox_UserName.SelectedIndex = 1
    End Sub

    Private Sub btn_Cancle_Click(sender As Object, e As EventArgs) Handles btn_Cancle.Click
        If Frm_Settings.Visible = True Then
            Frm_Settings.Btn_Enable.Enabled = True
        End If

        If Login_Mode = 4 Then
            Call Frm_Main.Save_MacType(False)
        End If
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub Label3_DoubleClick(sender As Object, e As EventArgs) Handles Label3.DoubleClick
        Me.Size = New Size(560, 400)
    End Sub

    Private Sub btn_Exit_Click(sender As Object, e As EventArgs) Handles btn_Exit.Click
        Me.Size = New Size(560, 220)
    End Sub

    Private Sub btn_Change_Click(sender As Object, e As EventArgs) Handles btn_Change.Click
        Select Case ComboBox_UserName.SelectedIndex
            Case 0
                If BOZHON.User1.Code = txtPassword.Text Then
                    If txt_NewPassword1.Text = txt_NewPassword2.Text Then
                        BOZHON.User1.Code = txt_NewPassword1.Text
                    Else
                        MessageBox.Show("两次密码输入不一致，请重新输入！")
                        Exit Sub
                    End If
                Else
                    MessageBox.Show("原始密码输入不正确，请重新输入！")
                    txt_NewPassword1.Text = ""
                    txt_NewPassword2.Text = ""
                    Exit Sub
                End If
            Case 1
                If BOZHON.User2.Code = txtPassword.Text Then
                    If txt_NewPassword1.Text = txt_NewPassword2.Text Then
                        BOZHON.User2.Code = txt_NewPassword1.Text
                    Else
                        MessageBox.Show("两次密码输入不一致，请重新输入！")
                        Exit Sub
                    End If
                Else
                    MessageBox.Show("原始密码输入不正确，请重新输入！")
                    txt_NewPassword1.Text = ""
                    txt_NewPassword2.Text = ""
                    Exit Sub
                End If
            Case 2
                If BOZHON.User3.Code = txtPassword.Text Then
                    If txt_NewPassword1.Text = txt_NewPassword2.Text Then
                        BOZHON.User3.Code = txt_NewPassword1.Text
                    Else
                        MessageBox.Show("两次密码输入不一致，请重新输入！")
                        Exit Sub
                    End If
                Else
                    MessageBox.Show("原始密码输入不正确，请重新输入！")
                    txt_NewPassword1.Text = ""
                    txt_NewPassword2.Text = ""
                    Exit Sub
                End If
            Case Else
                Exit Sub
        End Select
        Me.Size = New Size(560, 220)
        Call Write_Code(Path_UserCode, BOZHON)
    End Sub

    Private Sub btn_Login_Click(sender As Object, e As EventArgs) Handles btn_Login.Click
        Select Case ComboBox_UserName.SelectedIndex
            Case 0
                If BOZHON.User1.Code <> txtPassword.Text Then
                    MessageBox.Show("密码输入不正确，请重新输入！")
                    Exit Sub
                End If
            Case 1
                If BOZHON.User2.Code <> txtPassword.Text Then
                    MessageBox.Show("密码输入不正确，请重新输入！")
                    Exit Sub
                End If
            Case 2
                If BOZHON.User3.Code <> txtPassword.Text Then
                    MessageBox.Show("密码输入不正确，请重新输入！")
                    Exit Sub
                End If
            Case Else
                Exit Sub
        End Select

        Select Case Login_Mode
            Case 1
                If ComboBox_UserName.SelectedIndex = 0 Then
                    MsgBox("当前用户无权限修改参数！")
                    Exit Sub
                End If
                Call Frm_Settings.Change_Enable(True)
            Case 3
                If ComboBox_UserName.SelectedIndex = 0 Then
                    MsgBox("当前用户无权限修改参数！")
                    Exit Sub
                End If
                Call Frm_CCDSetting.Change_Enable(True)

            Case 4
                If ComboBox_UserName.SelectedIndex <> 2 Then
                    MsgBox("当前用户无权限修改参数！")
                    Call Frm_Main.Save_MacType(False)
                    Exit Sub
                End If
                Call Frm_Main.Save_MacType(True)
            Case Else
        End Select

        Me.Close()
        Me.Dispose()
    End Sub
End Class