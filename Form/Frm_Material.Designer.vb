<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Material
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btn_Cancle = New BoTech.BZ_Button()
        Me.SuspendLayout()
        '
        'btn_Cancle
        '
        Me.btn_Cancle.BZ_Color = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.btn_Cancle.BZ_Radius = 11
        Me.btn_Cancle.BZ_RoundStyle = BoTech.BZ_Button.RoundStyle.All
        Me.btn_Cancle.Location = New System.Drawing.Point(131, 151)
        Me.btn_Cancle.Name = "btn_Cancle"
        Me.btn_Cancle.Size = New System.Drawing.Size(60, 30)
        Me.btn_Cancle.TabIndex = 0
        Me.btn_Cancle.Text = "取消"
        Me.btn_Cancle.UseVisualStyleBackColor = True
        '
        'Frm_Material
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Orange
        Me.ClientSize = New System.Drawing.Size(335, 309)
        Me.Controls.Add(Me.btn_Cancle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Frm_Material"
        Me.Text = "Frm_Material"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Cancle As BoTech.BZ_Button
End Class
