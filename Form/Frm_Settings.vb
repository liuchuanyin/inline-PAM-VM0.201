Public Class Frm_Settings

    Private Sub Btn_ReadPar_Click(sender As Object, e As EventArgs) Handles Btn_ReadPar.Click
        Call Par_SaveRead(2)
    End Sub

    Private Sub Btn_WritePar_Click(sender As Object, e As EventArgs) Handles Btn_WritePar.Click
        Call Par_SaveRead(1)
        Call Change_Enable(False)
    End Sub

    Private Sub Btn_Enable_Click(sender As Object, e As EventArgs) Handles Btn_Enable.Click
        Frm_Login.Show(Me)
        Btn_Enable.Enabled = False
        Login_Mode = 1
    End Sub

    ''' <summary>
    ''' 窗体加载
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Frm_Settings_Load(sender As Object, e As EventArgs) Handles Me.Load
        Call Par_SaveRead(2)
        Call Change_Enable(False)
    End Sub
    Public Sub Change_Enable(ByVal enable As Boolean)
        If enable Then
            BZ_RoundPanel1.Enabled = True
            BZ_RoundPanel2.Enabled = True
            BZ_RoundPanel3.Enabled = True
            Btn_ReadPar.Enabled = True
            Btn_WritePar.Enabled = True
            Btn_Enable.Enabled = False
        Else
            BZ_RoundPanel1.Enabled = False
            BZ_RoundPanel2.Enabled = False
            BZ_RoundPanel3.Enabled = False
            Btn_ReadPar.Enabled = False
            Btn_WritePar.Enabled = False
            Btn_Enable.Enabled = True
        End If
    End Sub

#Region "功能：DownTime信息的保存和读取"
    ''' <summary>
    ''' Type=1 保存信息，=2 读取并显DownTime示信息
    ''' </summary>
    ''' <param name="type">Type=1 保存信息，=2 读取并显DownTime示信息</param>
    ''' <remarks></remarks>
    Public Sub Par_SaveRead(ByVal type As Integer)
        Select Case type
            Case 1
                par.Machine_Info.Project = txt_DownTime1.Text
                par.Machine_Info.BU = txt_DownTime2.Text
                par.Machine_Info.Floor = txt_DownTime3.Text
                par.Machine_Info.Line = txt_DownTime4.Text
                par.Machine_Info.AE_ID = txt_DownTime5.Text
                par.Machine_Info.AE_SubID = txt_DownTime6.Text
                par.Machine_Info.AE_Vendor = txt_DownTime7.Text
                par.Machine_Info.Machine_SN = txt_DownTime8.Text
                par.Machine_Info.SW_rev = VERSION_SOFTWARE
                par.Machine_Info.HW_rev = txt_DownTime10.Text
                par.Machine_Info.Station_ID = txt_DownTime11.Text
                par.Machine_Info.StandBy1 = txt_DownTime12.Text
                par.Machine_Info.StandBy2 = txt_DownTime13.Text
                par.Machine_Info.StandBy3 = txt_DownTime14.Text
                par.Machine_Info.StandBy4 = txt_DownTime15.Text

                '功能勾选项,存储布尔型变量
                For i = 1 To 40
                    For Each f As Object In TableLayoutPanel1.Controls
                        If f.name = "CheckBox" & CStr(i) Then
                            If f.Checked Then
                                par.chkFn(i) = True
                            Else
                                par.chkFn(i) = False
                            End If
                        End If
                    Next
                Next

                '设备设置参数结构体，存储数据值
                For i = 1 To 60
                    For Each f As Object In TableLayoutPanel3.Controls
                        If f.name = "txt_Par" & CStr(i) Then
                            par.num(i) = Val(f.text)
                        End If
                    Next
                Next

                'Save parameters
                Call Write_par(Path_Par, par)
                '在主界面上显示设备名称及编号
                Frm_Main.Btn_MachineInfo.Text = par.Machine_Info.AE_SubID & "-" & par.Machine_Info.Machine_SN
            Case 2
                'Read parameters
                Call Read_par(Path_Par, par)

                txt_DownTime1.Text = par.Machine_Info.Project
                txt_DownTime2.Text = par.Machine_Info.BU
                txt_DownTime3.Text = par.Machine_Info.Floor
                txt_DownTime4.Text = par.Machine_Info.Line
                txt_DownTime5.Text = par.Machine_Info.AE_ID
                txt_DownTime6.Text = par.Machine_Info.AE_SubID
                txt_DownTime7.Text = par.Machine_Info.AE_Vendor
                txt_DownTime8.Text = par.Machine_Info.Machine_SN
                par.Machine_Info.SW_rev = VERSION_SOFTWARE
                txt_DownTime9.Text = par.Machine_Info.SW_rev
                txt_DownTime10.Text = par.Machine_Info.HW_rev
                txt_DownTime11.Text = par.Machine_Info.Station_ID
                txt_DownTime12.Text = par.Machine_Info.StandBy1
                txt_DownTime13.Text = par.Machine_Info.StandBy2
                txt_DownTime14.Text = par.Machine_Info.StandBy3
                txt_DownTime15.Text = par.Machine_Info.StandBy4

                '功能勾选项,存储布尔型变量
                For i = 1 To par.chkFn.Length - 1
                    For Each f As Object In TableLayoutPanel1.Controls
                        If f.Name = "CheckBox" & CStr(i) Then
                            f.Checked = par.chkFn(i)
                        End If
                    Next
                Next

                '设备设置参数结构体，存储数据值
                For i = 1 To 40
                    For Each f As Object In TableLayoutPanel3.Controls
                        If f.Name = "txt_Par" & CStr(i) Then
                            f.text = par.num(i)
                        End If
                    Next
                Next
        End Select
    End Sub
#End Region


End Class