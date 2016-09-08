Public Class Frm_Material
    Public RowNumber As Integer
    Public ColumnNumber As Integer
    Public Btn_SelectMaterial(200) As Button
    Public Material_Index As Integer
    Public Material_ArrayX(200) As Double
    Public Material_ArrayY(200) As Double

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
        Material_Index = sender.Tag
        Me.Close()
    End Sub

    Private Sub btn_Cancle_Click(sender As Object, e As EventArgs) Handles btn_Cancle.Click
        Me.Dispose()
        Me.Close()
    End Sub

    '生成2维矩阵数据表
    '参数1：行数；参数2：列数；参数3：行间隔；参数4：列间隔；参数5：X方向阵列数据；参数6：Y方向阵列数据
    Public Function GetArray(ByVal RowNumber As Long, ByVal ColNumber As Long, ByVal RowDist As Double, ByVal ColDist As Double, ByRef xArray() As Double, ByRef yArray() As Double) As Integer
        Dim i As Long
        Dim j As Long
        For j = 0 To RowNumber - 1 '指定行数
            For i = 0 To ColNumber - 1 '指定列数
                xArray(i + j * ColNumber) = i * ColDist
                yArray(i + j * ColNumber) = j * RowDist
            Next i
        Next j
        Return 0
    End Function

End Class