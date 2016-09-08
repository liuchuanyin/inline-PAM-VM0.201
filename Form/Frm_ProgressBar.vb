Public Class Frm_ProgressBar

    Private Sub Frm_ProgressBar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With Me
            .Location = New Point(260, 280)
        End With

    End Sub

    Public Sub ShowProgressBar(ByVal show As Boolean)
        If show Then
            If Not Me.Visible Then
                Me.Show()
                Me.Visible = True
            End If
        Else
            If Me.Visible Then
                Me.Close()
                Me.Dispose()
            End If
        End If
    End Sub
    Public Sub SetProgressBarValue(ByVal value As Integer)
        If value >= 0 And value <= 100 Then
            ProgressBar1.Value = value
        End If
    End Sub

    Private Sub Frm_ProgressBar_LostFocus(sender As Object, e As EventArgs) Handles Me.LostFocus
        Me.Focus()
    End Sub

End Class