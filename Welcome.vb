Public Class Welcome




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If RadioButton1.Checked Then
            mode_type = 0
        End If
        If RadioButton2.Checked Then
            mode_type = 1
        End If
        Select Case mode_type
            Case 0, 1
                meansureform.Show()
            Case 3
            Case Else
        End Select

        Me.Hide()
    End Sub

    Private Sub Welcome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Threading.Thread.CurrentThread.CurrentCulture = ci
        Label1.Text = "V" + Version + " build " + build
        format_path()
        My.Computer.FileSystem.CreateDirectory(root_path + "temp")
    End Sub

    Private Sub Welcome_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs)

    End Sub
End Class