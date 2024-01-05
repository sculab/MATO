Imports System.ComponentModel

Public Class Form_about
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Form_about_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Hide
    End Sub
End Class