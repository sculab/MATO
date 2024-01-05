Imports System.Drawing.Drawing2D
Public Class Form_analysis


    'Dim drawBrush As New SolidBrush(System.Drawing.Color.Red)
    'Dim armPen As New System.Drawing.Pen(System.Drawing.Color.Black, 8)

    Dim formGraphics As Graphics
    Dim my_zoom_radio As Single
    Dim draw_font As New Font("Arial", 12)
    Dim scale_pen As New Pen(Color.Black, 3)
    Dim axis_Pen As New Pen(Color.Black, 1)

    Private Sub PaintKaryoType(ByVal tempgraphics As Graphics, ByVal image_width As Integer, ByVal image_height As Integer, ByVal zoom_radio As Single)
        '显示标尺
        tempgraphics.SmoothingMode = SmoothingMode.AntiAlias

        Dim suiPen As New Pen(Color.FromArgb(brush_circle.Color.R, brush_circle.Color.G, brush_circle.Color.B), NumericUpDown3.Value)
        Dim bPen As New Pen(Color.Gray, NumericUpDown3.Value)
        tempgraphics.FillRectangle(Brushes.White, 0, 0, image_width, image_height)
        tempgraphics.DrawLine(scale_pen, 20, 30, CInt(20 + scale_size / (sumarm) * zoom_radio), 30)
        tempgraphics.DrawString(scale_size.ToString & scale_unit, draw_font, brush_font, 25, 10)
        '坐标线
        tempgraphics.DrawLine(axis_Pen, 0, 150, image_width, 150)
        tempgraphics.DrawLine(axis_Pen, 10, 0, 10, 300)


        If data_count > 1 Then
            Dim nextstep As Integer = 0
            Dim typeL As Integer = 0, typeS As Integer = 0
            Dim show_sat(3) As Integer
            show_sat(1) = 0
            show_sat(2) = 0
            show_sat(3) = 0
            For i As Integer = 1 To data_count
                Dim x, yl, ys As Integer
                x = i * NumericUpDown1.Value + nextstep
                If i Mod times = 0 Then
                    nextstep = nextstep + NumericUpDown2.Value
                End If

                yl = longarm(Range(i)) / (sumarm) * zoom_radio
                ys = shortarm(Range(i)) / (sumarm) * zoom_radio

                If MenuItem2.Checked Then
                    If bchrom_list(Range(i)) = False Then
                        If i Mod times = 0 Then
                            x = x - (i - 1) * NumericUpDown1.Value + nextstep
                            typeL = CInt((yl + typeL) / times)
                            typeS = CInt((ys + typeS) / times)
                            show_sat(suiarm(Range(i))) = 1
                            tempgraphics.DrawLine(New Pen(Color.FromArgb(pen_arm_2.Color.R, pen_arm_2.Color.G, pen_arm_2.Color.B), NumericUpDown3.Value), x, 147 - typeS, x, 147)
                            tempgraphics.DrawLine(New Pen(Color.FromArgb(pen_arm_1.Color.R, pen_arm_1.Color.G, pen_arm_1.Color.B), NumericUpDown3.Value), x, 153, x, 153 + typeL)
                            If show_sat(1) = 1 Then
                                tempgraphics.DrawLine(suiPen, x, 153 + typeL + 2, x, 153 + typeL + 7)
                            End If
                            If show_sat(2) = 1 Then
                                tempgraphics.DrawLine(suiPen, x, 147 - typeS - 7, x, 147 - typeS - 2)
                            End If
                            If show_sat(3) = 1 Then
                                tempgraphics.DrawLine(suiPen, x, 147, x, 153)
                            End If
                            tempgraphics.DrawString(Str(i / times), draw_font, brush_font, x + NumericUpDown3.Value / 2 - 4, 150)
                            typeL = 0
                            typeS = 0
                            show_sat(1) = 0
                            show_sat(2) = 0
                            show_sat(3) = 0
                        Else
                            typeL = yl + typeL
                            typeS = ys + typeS
                            show_sat(suiarm(Range(i))) = 1
                        End If
                    End If

                Else
                    If bchrom_list(Range(i)) = False Then
                        If suiarm(Range(i)) = 0 Then
                            tempgraphics.DrawLine(New Pen(Color.FromArgb(pen_arm_2.Color.R, pen_arm_2.Color.G, pen_arm_2.Color.B), NumericUpDown3.Value), x, 147 - ys, x, 147)
                            tempgraphics.DrawLine(New Pen(Color.FromArgb(pen_arm_1.Color.R, pen_arm_1.Color.G, pen_arm_1.Color.B), NumericUpDown3.Value), x, 153, x, 153 + yl)
                        Else
                            tempgraphics.DrawLine(New Pen(Color.FromArgb(pen_arm_2.Color.R, pen_arm_2.Color.G, pen_arm_2.Color.B), NumericUpDown3.Value), x, 147 - ys, x, 147)
                            tempgraphics.DrawLine(New Pen(Color.FromArgb(pen_arm_1.Color.R, pen_arm_1.Color.G, pen_arm_1.Color.B), NumericUpDown3.Value), x, 153, x, 153 + yl)
                            If suiarm(Range(i)) = 1 Then
                                tempgraphics.DrawLine(suiPen, x, 153 + yl + 2, x, 153 + yl + 7)
                            End If
                            If suiarm(Range(i)) = 2 Then
                                tempgraphics.DrawLine(suiPen, x, 147 - ys - 7, x, 147 - ys - 2)
                            End If
                            If suiarm(Range(i)) = 3 Then
                                tempgraphics.DrawLine(suiPen, x, 147, x, 153)
                            End If
                        End If
                    Else
                        tempgraphics.DrawLine(bPen, x, 147 - ys, x, 147)
                        tempgraphics.DrawLine(bPen, x, 153, x, 153 + yl)

                    End If
                    If i Mod times = 1 Then
                        tempgraphics.DrawLine(axis_Pen, x + 4, 290, x + (times - 1) * NumericUpDown1.Value - 4, 290)
                        tempgraphics.DrawString(Str((i - 1) / times + 1), draw_font, brush_font, CInt(x + (times - 1) * NumericUpDown1.Value / 2 - 10), 272)
                    End If
                    tempgraphics.DrawString(Str(Range(i)), draw_font, brush_font, x + NumericUpDown3.Value / 2 - 4, 150)
                End If

            Next


        End If

    End Sub


    Private Sub Form5_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Me.Hide()
    End Sub
    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PictureBox1.Refresh()
    End Sub


    Private Sub TrackBar2_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs)
        PictureBox1.Refresh()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If 0 < TextBox1.Text <= data_count And 0 < TextBox2.Text <= data_count Then
                Dim c1, c2 As Integer
                Dim x1, x2 As Integer
                For i As Integer = 1 To data_count
                    If CInt(TextBox1.Text) = Range(i) Then
                        c1 = Range(i)
                        x1 = i
                    End If
                    If CInt(TextBox2.Text) = Range(i) Then
                        c2 = Range(i)
                        x2 = i
                    End If
                Next
                Range(x1) = c2
                Range(x2) = c1
                If CheckBox1.Checked Then
                    ReRange()
                End If
                info()
                Me.Refresh()
            Else
                MsgBox("Please enter the ID of chromosome!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Dim sfd As New SaveFileDialog
        sfd.Filter = "Table format File(*.xls)|*.xls;*.xls|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
        sfd.FileName = ""
        sfd.DefaultExt = ".xls"
        sfd.CheckPathExists = True
        Dim resultdialog As DialogResult = sfd.ShowDialog()
        If resultdialog = DialogResult.OK Then
            RichTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText)
        End If

    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        MenuItem2.Checked = MenuItem2.Checked Xor True
        Me.Refresh()
    End Sub

    Private Sub MenuItem3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MenuItem3.Click
        Me.Refresh()
    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        'Try
        Dim Nucbitmap As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim formpic As System.Drawing.Graphics = Graphics.FromImage(Nucbitmap)
        Dim sfd As New SaveFileDialog
        sfd.Filter = "PNG Files(*.png)|*.png;*.PNG|Vector file(Adobe Illustrator)|*.emf;*.EMF|ALL Files(*.*)|*.*"
        sfd.FileName = ""
        sfd.DefaultExt = ".png"
        sfd.CheckPathExists = True
        Dim resultdialog As DialogResult = sfd.ShowDialog()
        If resultdialog = DialogResult.OK Then

            If sfd.FileName.ToLower.EndsWith(".emf") Then
                Dim g As Graphics = Graphics.FromImage(Nucbitmap)
                Dim wmf As New Drawing.Imaging.Metafile(sfd.FileName, g.GetHdc())
                Dim ig As Graphics = Graphics.FromImage(wmf)
                PaintKaryoType(ig, PictureBox1.Width, PictureBox1.Height, my_zoom_radio)
                ig.Dispose()
                wmf.Dispose()
                g.ReleaseHdc()
                g.Dispose()
            Else
                Dim TempGrap As Graphics = Graphics.FromImage(Nucbitmap)
                PaintKaryoType(TempGrap, PictureBox1.Width, PictureBox1.Height, my_zoom_radio)
                Nucbitmap.Save(sfd.FileName)
            End If
        End If
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try

    End Sub

    Private Sub Form5_VisibleChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.VisibleChanged
        If Visible = True And data_count > 1 And PictureBox1.Height > 40 Then
            'my_zoom_radio = get_zoom_radio(110)
            PictureBox1.Width = (data_count + 1) * NumericUpDown1.Value + (data_count / times + 1) * NumericUpDown2.Value + 30
            my_zoom_radio = get_zoom_radio(110)
            PictureBox1.Refresh
            info
        End If
    End Sub

    Private Sub PictureBox1_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.HandleDestroyed

    End Sub



    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
        If PictureBox1.Height > 10 Then
            formGraphics = e.Graphics
            formGraphics.SmoothingMode = SmoothingMode.HighSpeed
            PaintKaryoType(formGraphics, PictureBox1.Width, PictureBox1.Height, my_zoom_radio)
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub SaveResultToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveResultToolStripMenuItem.Click
        Dim sfd As New SaveFileDialog
        sfd.Filter = "Table format File(*.xls)|*.xls;*.xls|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
        sfd.FileName = ""
        sfd.DefaultExt = ".xls"
        sfd.CheckPathExists = True
        Dim resultdialog As DialogResult = sfd.ShowDialog()
        If resultdialog = DialogResult.OK Then
            RichTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText)
        End If
    End Sub

    Private Sub SaveGraphicToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveGraphicToolStripMenuItem.Click
        MenuItem4_Click(sender, e)
    End Sub

    Private Sub IdiogramToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IdiogramToolStripMenuItem.Click
        IdiogramToolStripMenuItem.Checked = IdiogramToolStripMenuItem.Checked Xor True
        MenuItem2.Checked = MenuItem2.Checked Xor True
        Me.Refresh()
    End Sub

    Private Sub Form_analysis_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Height > 220 Then
            SplitContainer1.SplitterDistance = SplitContainer1.Height - 214
            PictureBox1.Width = (data_count + 1) * NumericUpDown1.Value + (data_count / times + 1) * NumericUpDown2.Value + 30
            my_zoom_radio = get_zoom_radio(110)
            PictureBox1.Refresh
        End If
    End Sub
    Public Function get_zoom_radio(ByVal image_height As Single) As Single
        Dim max_long_arm As Single = 0
        For i As Integer = 1 To data_count
            If max_long_arm < longarm(Range(i)) / (sumarm) Then
                max_long_arm = longarm(Range(i)) / (sumarm)
            End If
        Next
        Return image_height / max_long_arm
    End Function
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        PictureBox1.Width = (data_count + 1) * NumericUpDown1.Value + (data_count / times + 1) * NumericUpDown2.Value + 30
        my_zoom_radio = get_zoom_radio(110)
        PictureBox1.Refresh()
    End Sub
End Class