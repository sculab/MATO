Public Class Welcome




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
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

    Private Sub Welcome_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs)

    End Sub



    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            RadioButton2.Checked = False
            mode_type = 0
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            RadioButton1.Checked = False
            mode_type = 1

        End If
    End Sub
End Class