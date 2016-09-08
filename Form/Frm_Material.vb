Public Class Frm_Material
    Public RowNumber As Integer
    Public ColumnNumber As Integer
    Public Btn_SelectMaterial(200) As Button
    Public Material_Index As Integer

    Private Sub Frm_Material_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RowNumber = 5
        ColumnNumber = 6
        For i = 0 To RowNumber * ColumnNumber - 1
            Btn_SelectMaterial(i) = New Button
        Next

        For i = 0 To RowNumber - 1
            For j = 0 To ColumnNumber - 1
                Btn_SelectMaterial(i * ColumnNumber + j).Parent = Me
                Btn_SelectMaterial(i * ColumnNumber + j).Location = New Point(5 + j * 65, 5 + i * 35)
                Btn_SelectMaterial(i * ColumnNumber + j).Size = New Size(60, 30)
                Btn_SelectMaterial(i * ColumnNumber + j).Text = i * ColumnNumber + j + 1
                Btn_SelectMaterial(i * ColumnNumber + j).Tag = i * ColumnNumber + j
                AddHandler Btn_SelectMaterial(i * ColumnNumber + j).Click, AddressOf Btn_Select '注册事件
            Next
        Next

        Me.Size = New Size(5 + ColumnNumber * 65, 40 + RowNumber * 35)
        btn_Cancle.Location = New Point(Me.Width - 65, Me.Height - 35)
    End Sub

    Private Sub Btn_Select(ByVal sender As System.Object, ByVal e As System.EventArgs)
        MessageBox.Show(sender.Tag.ToString)
        Material_Index = sender.Tag
    End Sub

    Private Sub btn_Cancle_Click(sender As Object, e As EventArgs) Handles btn_Cancle.Click
        Me.Dispose()
        Me.Close()
    End Sub
End Class