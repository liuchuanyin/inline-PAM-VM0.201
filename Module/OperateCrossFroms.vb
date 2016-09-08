Module OperateCrossFroms

    ''' <summary>
    ''' 在Frm_Engineering界面上的ListBox上添加提示消息
    ''' </summary>
    ''' <param name="str"></param>
    ''' <remarks></remarks>
    Public Sub ListBoxAddMessage(ByVal str As String)
        Dim time As String
        time = Format(Now, "mm:ss")
        Write_Log(str, Path_Log)
        If Flag_FrmEngineeringOpned Then
            'Frm_Engineering.ListBox1.Items.Add(time & " : " & str)
            Frm_Engineering.ListBox1.Items.Add(str)
            If Frm_Engineering.ListBox1.Items.Count > 20 Then
                Frm_Engineering.ListBox1.Items.RemoveAt(0)
            End If
        End If

    End Sub


    ''' <summary>
    ''' 在Frm_Dialog界面添加提示信息
    ''' </summary>
    ''' <param name="str"></param>
    ''' <remarks></remarks>
    Public Sub Frm_DialogAddMessage(ByVal str As String)
        Frm_Dialog.AddMessage(str)
        ListBoxAddMessage(str)
    End Sub


    Public Sub List_DebugAddMessage(ByVal str As String)
        Dim time As String
        time = Format(Now, "mm:ss:fff")
        Write_Log(str, Path_Log)
        If Frm_Engineering.Visible Then
            Frm_Engineering.listbox_Debug.Items.Add(time & ": " & str)
            'List_DebugAddMessage(str)
            Frm_Engineering.listbox_Debug.TopIndex = Frm_Engineering.listbox_Debug.Items.Count - 1
        End If
    End Sub

End Module
