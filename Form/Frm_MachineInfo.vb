Public Class Frm_MachineInfo

    Private Sub Frm_MachineInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Label_SoftwareVersion.Text = VERSION_SOFTWARE
        Label_HardwareVersion.Text = par.Machine_Info.HW_rev
        Label_SoftWareUpdate.Text = UPDATE_SOFTWARE
    End Sub
End Class