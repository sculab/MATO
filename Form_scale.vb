Imports System.IO
Public Class Form_scale
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedIndices.Count > 0 Then
            TextBox0.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(0).Text
            TextBox1.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text
            TextBox2.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text
            ComboBox1.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text
            TextBox4.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text
            TextBox5.Text = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(5).Text
        End If
    End Sub


    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ListView1.SelectedIndices.Count > 0 Then
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(0).Text = TextBox0.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text = TextBox1.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text = TextBox2.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text = ComboBox1.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text = TextBox4.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(5).Text = CSng(TextBox2.Text) / CSng(TextBox1.Text)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListView1.SelectedIndices.Count > 0 Then
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(0).Text = TextBox0.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text = TextBox1.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text = TextBox2.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text = ComboBox1.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text = TextBox4.Text
            ListView1.Items(ListView1.SelectedIndices(0)).SubItems(5).Text = CSng(TextBox2.Text) / CSng(TextBox1.Text)
        End If
        scale_size = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text
        scale_unit = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text
        current_scale = ListView1.Items(ListView1.SelectedIndices(0)).SubItems(5).Text
        Dim sw As New StreamWriter(exepath + "scales.csv")
        For Each i As ListViewItem In ScaleForm.ListView1.Items
            sw.WriteLine(i.SubItems(0).Text + "," + i.SubItems(1).Text + "," + i.SubItems(2).Text + "," +
                         i.SubItems(3).Text + "," + i.SubItems(4).Text + "," + i.SubItems(5).Text)
        Next
        sw.Close()
        Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
        Dim sr As New StreamReader(exepath + "scales.csv")
        Dim line As String = sr.ReadLine
        Do
            If line Is Nothing = False Then
                Dim scale_name() As String = line.Split(",")
                If scale_name.Length = 6 Then
                    ListView1.Items.Add(New ListViewItem(scale_name))
                    scale_size = scale_name(4)
                    scale_unit = scale_name(3)
                End If
            End If
            line = sr.ReadLine
        Loop Until line Is Nothing
        sr.Close()

        Me.Hide()
    End Sub

    Private Sub AddToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddToolStripMenuItem.Click
        ListView1.Items.Add(New ListViewItem({"new", 1000, CInt(1000 * current_scale), scale_unit, 10, current_scale}))
        ListView1.Items(ScaleForm.ListView1.Items.Count - 1).Selected = True
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        ListView1.Items.RemoveAt(ListView1.SelectedIndices(0))
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub
End Class