Public Class Frm_Dialog
    Public Sub AddMessage(ByVal str As String)
        List_Message.Items.Add(List_Message.Items.Count.ToString & ": " & str)
        Me.Show()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ScreenCut("E:\BZ-Data\ScreenCut\")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Frm_Dialog_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        Me.Focus()
    End Sub

    Private Sub Frm_Dialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.TopMost = True
    End Sub
End Class