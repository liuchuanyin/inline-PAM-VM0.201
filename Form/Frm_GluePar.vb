Public Class Frm_GluePar


    Private Sub Frm_GluePar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.txt_Vel1.Text = Par_Glue.Segment(1).vel
        Me.txt_Vel2.Text = Par_Glue.Segment(2).vel
        Me.txt_Vel3.Text = Par_Glue.Segment(3).vel
        Me.txt_Vel4.Text = Par_Glue.Segment(4).vel
        Me.txt_Vel5.Text = Par_Glue.Segment(5).vel
        Me.txt_Vel6.Text = Par_Glue.Segment(6).vel
        Me.txt_Vel7.Text = Par_Glue.Segment(7).vel
        Me.txt_Vel8.Text = Par_Glue.Segment(8).vel
        Me.txt_Vel9.Text = Par_Glue.Segment(9).vel
        Me.txt_Vel10.Text = Par_Glue.Segment(10).vel
        Me.txt_Vel11.Text = Par_Glue.Segment(11).vel
        Me.txt_Vel12.Text = Par_Glue.Segment(12).vel
        Me.txt_Vel13.Text = Par_Glue.Segment(13).vel
        Me.txt_Vel14.Text = Par_Glue.Segment(14).vel
        Me.txt_Vel15.Text = Par_Glue.Segment(15).vel

        Me.txt_StartDelay1.Text = Par_Glue.Segment(1).startDelay
        Me.txt_StartDelay2.Text = Par_Glue.Segment(2).startDelay
        Me.txt_StartDelay3.Text = Par_Glue.Segment(3).startDelay
        Me.txt_StartDelay4.Text = Par_Glue.Segment(4).startDelay
        Me.txt_StartDelay5.Text = Par_Glue.Segment(5).startDelay
        Me.txt_StartDelay6.Text = Par_Glue.Segment(6).startDelay
        Me.txt_StartDelay7.Text = Par_Glue.Segment(7).startDelay
        Me.txt_StartDelay8.Text = Par_Glue.Segment(8).startDelay
        Me.txt_StartDelay9.Text = Par_Glue.Segment(9).startDelay
        Me.txt_StartDelay10.Text = Par_Glue.Segment(10).startDelay
        Me.txt_StartDelay11.Text = Par_Glue.Segment(11).startDelay
        Me.txt_StartDelay12.Text = Par_Glue.Segment(12).startDelay
        Me.txt_StartDelay13.Text = Par_Glue.Segment(13).startDelay
        Me.txt_StartDelay14.Text = Par_Glue.Segment(14).startDelay
        Me.txt_StartDelay15.Text = Par_Glue.Segment(15).startDelay

        Me.txt_EndDelay1.Text = Par_Glue.Segment(1).endDelay
        Me.txt_EndDelay2.Text = Par_Glue.Segment(2).endDelay
        Me.txt_EndDelay3.Text = Par_Glue.Segment(3).endDelay
        Me.txt_EndDelay4.Text = Par_Glue.Segment(4).endDelay
        Me.txt_EndDelay5.Text = Par_Glue.Segment(5).endDelay
        Me.txt_EndDelay6.Text = Par_Glue.Segment(6).endDelay
        Me.txt_EndDelay7.Text = Par_Glue.Segment(7).endDelay
        Me.txt_EndDelay8.Text = Par_Glue.Segment(8).endDelay
        Me.txt_EndDelay9.Text = Par_Glue.Segment(9).endDelay
        Me.txt_EndDelay10.Text = Par_Glue.Segment(10).endDelay
        Me.txt_EndDelay11.Text = Par_Glue.Segment(11).endDelay
        Me.txt_EndDelay12.Text = Par_Glue.Segment(12).endDelay
        Me.txt_EndDelay13.Text = Par_Glue.Segment(13).endDelay
        Me.txt_EndDelay14.Text = Par_Glue.Segment(14).endDelay
        Me.txt_EndDelay15.Text = Par_Glue.Segment(15).endDelay

    End Sub




    Private Sub Btn_Ok_Click(sender As Object, e As EventArgs) Handles Btn_Ok.Click
        Par_Glue.Segment(1).vel = Me.txt_Vel1.Text
        Par_Glue.Segment(2).vel = Me.txt_Vel2.Text
        Par_Glue.Segment(3).vel = Me.txt_Vel3.Text
        Par_Glue.Segment(4).vel = Me.txt_Vel4.Text
        Par_Glue.Segment(5).vel = Me.txt_Vel5.Text
        Par_Glue.Segment(6).vel = Me.txt_Vel6.Text
        Par_Glue.Segment(7).vel = Me.txt_Vel7.Text
        Par_Glue.Segment(8).vel = Me.txt_Vel8.Text
        Par_Glue.Segment(9).vel = Me.txt_Vel9.Text
        Par_Glue.Segment(10).vel = Me.txt_Vel10.Text
        Par_Glue.Segment(11).vel = Me.txt_Vel11.Text
        Par_Glue.Segment(12).vel = Me.txt_Vel12.Text
        Par_Glue.Segment(13).vel = Me.txt_Vel13.Text
        Par_Glue.Segment(14).vel = Me.txt_Vel14.Text
        Par_Glue.Segment(15).vel = Me.txt_Vel15.Text

        Par_Glue.Segment(1).startDelay = Me.txt_StartDelay1.Text
        Par_Glue.Segment(2).startDelay = Me.txt_StartDelay2.Text
        Par_Glue.Segment(3).startDelay = Me.txt_StartDelay3.Text
        Par_Glue.Segment(4).startDelay = Me.txt_StartDelay4.Text
        Par_Glue.Segment(5).startDelay = Me.txt_StartDelay5.Text
        Par_Glue.Segment(6).startDelay = Me.txt_StartDelay6.Text
        Par_Glue.Segment(7).startDelay = Me.txt_StartDelay7.Text
        Par_Glue.Segment(8).startDelay = Me.txt_StartDelay8.Text
        Par_Glue.Segment(9).startDelay = Me.txt_StartDelay9.Text
        Par_Glue.Segment(10).startDelay = Me.txt_StartDelay10.Text
        Par_Glue.Segment(11).startDelay = Me.txt_StartDelay11.Text
        Par_Glue.Segment(12).startDelay = Me.txt_StartDelay12.Text
        Par_Glue.Segment(13).startDelay = Me.txt_StartDelay13.Text
        Par_Glue.Segment(14).startDelay = Me.txt_StartDelay14.Text
        Par_Glue.Segment(15).startDelay = Me.txt_StartDelay15.Text


        Par_Glue.Segment(1).endDelay = Me.txt_EndDelay1.Text
        Par_Glue.Segment(2).endDelay = Me.txt_EndDelay2.Text
        Par_Glue.Segment(3).endDelay = Me.txt_EndDelay3.Text
        Par_Glue.Segment(4).endDelay = Me.txt_EndDelay4.Text
        Par_Glue.Segment(5).endDelay = Me.txt_EndDelay5.Text
        Par_Glue.Segment(6).endDelay = Me.txt_EndDelay6.Text
        Par_Glue.Segment(7).endDelay = Me.txt_EndDelay7.Text
        Par_Glue.Segment(8).endDelay = Me.txt_EndDelay8.Text
        Par_Glue.Segment(9).endDelay = Me.txt_EndDelay9.Text
        Par_Glue.Segment(10).endDelay = Me.txt_EndDelay10.Text
        Par_Glue.Segment(11).endDelay = Me.txt_EndDelay11.Text
        Par_Glue.Segment(12).endDelay = Me.txt_EndDelay12.Text
        Par_Glue.Segment(13).endDelay = Me.txt_EndDelay13.Text
        Par_Glue.Segment(14).endDelay = Me.txt_EndDelay14.Text
        Par_Glue.Segment(15).endDelay = Me.txt_EndDelay15.Text

        Call Write_GluePar(Path_Par_Glue, Par_Glue)
    End Sub
End Class