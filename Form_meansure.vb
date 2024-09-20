Imports System.Math
Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports Emgu.CV
Imports System.Threading
Imports Emgu.CV.Structure

Public Class Form_meansure
    Dim bPictureBoxDragging As Boolean
    Dim oPointClicked As Point
    Dim loactionClicked As Point
    Dim TempGrap As Graphics
    Dim DrawSize As String
    Dim current_bmp As Bitmap

    Private Sub clear_view()
        ListView1.Items.Clear()
        ListView1.Items.Add(New ListViewItem({data_count.ToString, "", "", "0", "F"}))
        ListView1.Items(0).Selected = True

        ListView2.Items.Clear()
        ListView2.Items.Add(New ListViewItem({data_count.ToString, "", "", "", ""}))
        ListView2.Items(0).Selected = True

        Picture.Image = New Bitmap(560, 460)
        Picture.Width = 560
        Picture.Height = 460
        Picture.Refresh()
        current_bmp = Nothing
        current_scale = 1
        scale_size = 100
        scale_title = ""
        scale_unit = "px"
    End Sub
    Private Sub open_image(ByVal filename As String)
        Dim mystream As IO.Stream
        mystream = File.OpenRead(filename)
        image_path = filename
        Picture.Image = Drawing.Image.FromStream(mystream)
        Picture.Size = Picture.Image.Size
        zoom_radio = 1
        PicArea = Picture.Image.Size.Height * Picture.Image.Size.Width
        mystream.Close()
        current_bmp = New Bitmap(Picture.Image)
    End Sub
    Private Sub find_image(ByVal filename As String)
        Dim nuc_floder As String = filename.Substring(0, filename.LastIndexOf("\") + 1)
        Dim pic_name As String = ""
        If image_path.Contains("\") Then
            pic_name = image_path.Substring(image_path.LastIndexOf("\") + 1)
        Else
            pic_name = image_path
        End If
        If File.Exists(image_path) = False And File.Exists(nuc_floder + pic_name) Then
            image_path = nuc_floder + pic_name
        End If
        If File.Exists(image_path) Then

            Dim mystream As IO.Stream

            mystream = File.OpenRead(image_path)
            image_path = image_path
            Picture.Image = Drawing.Image.FromStream(mystream)
            Picture.Size = Picture.Image.Size
            pic_W = Picture.Size.Width
            pic_H = Picture.Size.Height
            zoom_radio = 1
            mystream.Close()
        Else
            If pic_W > 0 Then
                Picture.Width = pic_W
                Picture.Height = pic_H
            End If
            MsgBox("The postion of linked photo has been changed, please load the photo manually.")
        End If
    End Sub
    Private Sub read_karyo(ByVal filename As String)
        Dim sr As StreamReader = New StreamReader(filename)
        Dim data(5) As String
        Dim line As String = "start"
        ListView1.Items.Clear()
        If filename.ToUpper.EndsWith(".KARYO") Or filename.ToUpper.EndsWith(".NUC") Then
            '清空上次数据
            Do
                '读取开头
                line = line.ToUpper()
                If line.StartsWith("BEGIN;") Then
                    line = sr.ReadLine()
                    line = line.ToUpper()
                    If line.StartsWith("NUM=") Then
                        data_count = CInt(line.Replace("NUM=", "").Replace(";", ""))
                        line = sr.ReadLine()
                        line = line.ToUpper()
                        If line.StartsWith("LEVEL=") Then
                            times = CInt(line.Replace("LEVEL=", "").Replace(";", ""))
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        If line.StartsWith("PICSIZE=") Then
                            Dim pic_size() As String = line.Replace("PICSIZE=", "").Replace(";", "").Split("|")
                            pic_W = CInt(pic_size(0))
                            pic_H = CInt(pic_size(1))
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        If line.StartsWith("PICPATH=") Then
                            image_path = line.Replace("PICPATH=", "").Replace(";", "")
                            find_image(filename)
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        If line.StartsWith("SCALE=") Then
                            current_scale = CSng(line.Replace("SCALE=", "").Replace(";", ""))
                            ' MsgBox("该测定的标尺为："+Staff.ToString)
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        If line.StartsWith("STAFF=") Then
                            current_scale = CSng(line.Replace("STAFF=", "").Replace(";", ""))
                            ' MsgBox("该测定的标尺为："+Staff.ToString)
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        If line.StartsWith("SCALE_SIZE=") Then
                            scale_size = CSng(line.Replace("SCALE_SIZE=", "").Replace(";", ""))
                            ' MsgBox("该测定的标尺为："+Staff.ToString)
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        If line.StartsWith("UNIT=") Then
                            scale_unit = line.Replace("UNIT=", "").Replace(";", "").ToLower
                            line = sr.ReadLine()
                            line = line.ToUpper()
                        End If
                        'MessageBox.Show(data_count)
                    Else
                        Exit Do
                        MsgBox("Bad Format!")
                    End If
                End If
                '读取数据
                If line.StartsWith("DATA;") Then
                    For i As Integer = 1 To data_count
                        line = sr.ReadLine()
                        If line.ToUpper.StartsWith("ANALYSE;") Then
                            MsgBox("Bad Format!")
                            data_count = i
                            Exit Sub
                        Else
                            data = line.Split(";")
                            data_id(i) = data(0)
                            longarm(i) = data(1)
                            shortarm(i) = data(2)
                            suiarm(i) = data(3)
                            Range(i) = data(4)
                            If ListView1.Items.Count - 1 < i Then
                                If data.Length = 6 Then
                                    ListView1.Items.Add(New ListViewItem({i.ToString, data(1), data(2), data(3), data(5)}))
                                    If data(5) = "T" Then
                                        bchrom_list(i) = True
                                    Else
                                        bchrom_list(i) = False
                                    End If

                                Else
                                    ListView1.Items.Add(New ListViewItem({i.ToString, data(1), data(2), data(3), "F"}))

                                End If
                            End If

                            'If Resultform.CheckedListBox1.Items.Count - 1 < i Then
                            '    Resultform.CheckedListBox1.Items.Add(i)
                            'End If
                        End If
                    Next
                End If
                If line.ToUpper.StartsWith("COORDINATE;") Then
                    For i As Integer = 1 To data_count
                        line = sr.ReadLine()
                        data = line.Split(";")
                        his_point(i).X = data(0)
                        his_point(i).Y = data(1)
                    Next
                End If
                If line.StartsWith("POINTS;") Then
                    For i As Integer = 1 To data_count
                        line = sr.ReadLine()
                        Dim my_line() As String = line.Split("|")
                        points_group(i).points_type_1 = CInt(my_line(0))
                        points_group(i).points_count_1 = CInt(my_line(1))
                        points_group(i).points_value_1 = CSng(my_line(2))
                        Dim my_points_1() As String = my_line(3).Split(";")
                        For j = 0 To points_group(i).points_count_1
                            points_group(i).points_group_1(j) = New Point(my_points_1(j).Split(",")(0), my_points_1(j).Split(",")(1))
                        Next
                        points_group(i).points_type_2 = CInt(my_line(4))
                        points_group(i).points_count_2 = CInt(my_line(5))
                        points_group(i).points_value_2 = CSng(my_line(6))
                        Dim my_points_2() As String = my_line(7).Split(";")
                        For j = 0 To points_group(i).points_count_2
                            points_group(i).points_group_2(j) = New Point(my_points_2(j).Split(",")(0), my_points_2(j).Split(",")(1))
                        Next
                    Next
                End If

                '读取分组
                line = sr.ReadLine()
            Loop Until line Is Nothing
        Else
            Do
                '读取开头
                Dim i As Integer = 0
                line = sr.ReadLine()
                If line <> "" Then
                    i = i + 1
                    data = line.Split(",")
                    longarm(i) = data(0)
                    shortarm(i) = data(1)
                    ChangeArm(i)
                    If ListView1.Items.Count - 1 < i Then
                        ListView1.Items.Add(New ListViewItem({i.ToString, data(0), data(1), "0", "F"}))
                    End If
                    'If Resultform.CheckedListBox1.Items.Count - 1 < i Then
                    '    Resultform.CheckedListBox1.Items.Add(i)
                    'End If
                    data_count = i
                End If
            Loop Until line Is Nothing
        End If
        ListView1.Items(ListView1.Items.Count - 1).Selected = True
        sr.Close()
    End Sub
    Private Sub write_karyo(ByVal filename As String)
        Dim sw As StreamWriter = New StreamWriter(filename)
        ' Add some text to the file.
        Dim line As Integer
        sw.WriteLine("Begin;")
        sw.WriteLine("NUM=" + Str(data_count))
        sw.WriteLine("LEVEL=" + Str(times))
        sw.WriteLine("PICSIZE=" + Str(pic_W) + "|" + Str(pic_H))
        sw.WriteLine("PICPATH=" + image_path)
        sw.WriteLine("SCALE=" + current_scale.ToString)
        sw.WriteLine("SCALE_SIZE=" + scale_size.ToString)
        sw.WriteLine("UNIT=" + scale_unit)
        sw.WriteLine("Data;")
        For line = 1 To data_count
            sw.WriteLine(Str(data_id(line)) + ";" + Str(longarm(line)) + ";" + Str(shortarm(line)) + ";" + Str(suiarm(line)) + ";" + Str(Range(line)))
        Next
        sw.WriteLine("Analyse;")
        sw.WriteLine(Analysisform.RichTextBox1.Text)
        sw.WriteLine("Coordinate;")
        For line = 1 To data_count
            sw.WriteLine(Str(his_point(line).X) + ";" + Str(his_point(line).Y))
        Next
        sw.WriteLine("Points;")
        For i As Integer = 1 To data_count
            Dim new_line As String = ""
            new_line += points_group(i).points_type_1.ToString + "|"
            new_line += points_group(i).points_count_1.ToString + "|"
            new_line += points_group(i).points_value_1.ToString + "|"
            For j = 0 To points_group(i).points_count_1
                new_line += points_group(i).points_group_1(j).X.ToString + "," + points_group(i).points_group_1(j).Y.ToString + ";"
            Next
            new_line += "|"
            new_line += points_group(i).points_type_2.ToString + "|"
            new_line += points_group(i).points_count_2.ToString + "|"
            new_line += points_group(i).points_value_2.ToString + "|"
            For j = 0 To points_group(i).points_count_2
                new_line += points_group(i).points_group_2(j).X.ToString + "," + points_group(i).points_group_2(j).Y.ToString + ";"
            Next
            sw.WriteLine(new_line)
        Next
        ' Arbitrary objects can also be written to the file.
        sw.WriteLine("End;")
        sw.WriteLine(DateTime.Now)
        sw.Close()
    End Sub
    Private Sub open_karyo()
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Karyotype Files(*.karyo;*.nuc)|*.karyo;*.KARYO;*.nuc;*.NUC|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
        opendialog.FileName = ""
        opendialog.DefaultExt = ".karyo"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            clear_result()
            read_karyo(opendialog.FileName)
        End If
    End Sub
    Private Sub load_scale() '加载标尺
        Dim line As String
        If Not File.Exists(exepath + "scales.csv") Then
            Dim sw As New StreamWriter(exepath + "scales.csv")
            sw.WriteLine("default,100,100,px,100,1")
            ScaleForm.ListView1.Items.Add(New ListViewItem("default,100,100,px,100,1".Split(",")))
            sw.Close()
        End If
        Dim sr As New StreamReader(exepath + "scales.csv")
        line = sr.ReadLine
        Try
            Do
                If line Is Nothing = False Then
                    Dim scale_name() As String = line.Split(",")
                    If scale_name.Length = 6 Then
                        ScaleForm.ListView1.Items.Add(New ListViewItem(scale_name))
                        scale_size = scale_name(4)
                        scale_unit = scale_name(3)
                    End If
                End If
                line = sr.ReadLine
            Loop Until line Is Nothing
            sr.Close()
            ScaleForm.ListView1.Items(ScaleForm.ListView1.Items.Count - 1).Selected = True
        Catch ex As Exception
            sr.Close()
        End Try

    End Sub
    Public Sub save_csv(ByVal filename As String, ByRef my_listview As ListView)
        Dim sw As StreamWriter = New StreamWriter(filename)
        For i = 0 To my_listview.Items.Count - 1
            Dim line As String = my_listview.Items(i).SubItems(0).Text
            For j = 1 To my_listview.Items(i).SubItems.Count - 1
                line += "," + my_listview.Items(i).SubItems(j).Text
            Next
            sw.WriteLine(line)
        Next
        sw.WriteLine("image_path=" + image_path)
        sw.Close()
    End Sub
    Public Sub read_csv(ByVal filename As String, ByRef my_listview As ListView)
        my_listview.Items.Clear()
        Dim sr As StreamReader = New StreamReader(filename)
        Dim line As String = sr.ReadLine
        Do
            If line <> "" Then
                Dim temp_list() As String = line.Split(",")
                If temp_list.Length = 5 Then
                    my_listview.Items.Add(New ListViewItem(temp_list))
                    points_group(my_listview.Items.Count).points_value_1 = temp_list(1)
                    points_group(my_listview.Items.Count).points_type_1 = name2type(temp_list(2))
                    points_group(my_listview.Items.Count).points_count_1 = CInt(temp_list(3)) - 1
                    last_count = node_count
                    Dim new_line() As String = temp_list(4).Split(";")
                    For j = 0 To UBound(new_line) - 1
                        If new_line(j) <> "" Then
                            points_group(my_listview.Items.Count).points_group_1(j) = New Point(CSng(new_line(j).Split("_")(0)), CSng(new_line(j).Split("_")(1)))
                        End If
                    Next
                ElseIf line.StartsWith("image_path=") Then
                    image_path = line.Split("=")(1)
                End If
            End If
            line = sr.ReadLine
        Loop Until line Is Nothing
        sr.Close()
        data_count = my_listview.Items.Count
        my_listview.Items(data_count - 1).Selected = True
    End Sub
    Private Function get_extend_line(ByVal p1 As Point, ByVal p2 As Point) As Point()
        Dim temp_points(1) As Point
        If p1.Y = p2.Y Then
            temp_points(0) = New Point(0, p1.Y)
            temp_points(1) = New Point(Picture.Width, p2.Y)
            Return temp_points
        End If
        If p1.X = p2.X Then
            temp_points(0) = New Point(p1.X, 0)
            temp_points(1) = New Point(p2.X, Picture.Height)
            Return temp_points
        End If
        Dim a1 As Single = (p1.Y - p2.Y) / (p1.X - p2.X)
        Dim b1 As Single = (p2.Y * p1.X - p2.X * p1.Y) / (p1.X - p2.X)
        Dim point_x(4) As Single
        Dim point_y(4) As Single
        point_x(1) = Picture.Width
        point_y(1) = a1 * Picture.Width + b1
        point_x(2) = 0
        point_y(2) = b1
        point_x(3) = -b1 / a1
        point_y(3) = 0
        point_x(4) = (Picture.Height - b1) / a1
        point_y(4) = Picture.Height

        For i = 1 To 4
            If point_x(i) >= 0 And point_x(i) <= Picture.Width And point_y(i) >= 0 And point_y(i) <= Picture.Height Then
                temp_points(0) = New Point(point_x(i), point_y(i))
                Exit For
            End If
        Next
        For i = 4 To 1 Step -1
            If point_x(i) >= 0 And point_x(i) <= Picture.Width And point_y(i) >= 0 And point_y(i) <= Picture.Height Then
                temp_points(1) = New Point(point_x(i), point_y(i))
                Exit For
            End If
        Next
        Return temp_points
    End Function
    Private Sub draw_lines(ByVal g As Graphics, ByVal my_radio As Single, ByRef my_listview As ListView)
        '显示标尺
        Dim draw_font As New Font("Arial", 12 * my_radio)
        If my_radio <> 1 Then
            point_size *= my_radio
            scale_pen.Width *= my_radio
            pen_circle.Width *= my_radio
            pen_arm_1.Width *= my_radio
            pen_arm_2.Width *= my_radio
            pen_line.Width *= my_radio
        End If


        If ScaleAxisToolStripMenuItem.Checked Then
            g.SmoothingMode = SmoothingMode.HighSpeed

            g.DrawLine(scale_pen, 11 * my_radio, 31 * my_radio, 11 * my_radio, 37 * my_radio)
            g.DrawLine(scale_pen, 10 * my_radio, 35 * my_radio, CInt(10 * my_radio + scale_size / current_scale * zoom_radio), 35 * my_radio)
            g.DrawLine(scale_pen, CInt(10 * my_radio + scale_size / current_scale * zoom_radio) - 1, 31 * my_radio, CInt(10 * my_radio + scale_size / current_scale * zoom_radio) - 1, 37 * my_radio)
            g.DrawString(scale_size.ToString & scale_unit, draw_font, scale_brush, 18 * my_radio, 12 * my_radio)
        End If
        ''显示网格
        'If GridToolStripMenuItem.Checked Then
        '    g.SmoothingMode = SmoothingMode.HighSpeed
        '    For i As Integer = 1 To (Picture.Height - Picture.Height Mod 20) / 20
        '        g.DrawLine(netPen, 0, Picture.Height - i * 20, Picture.Width, Picture.Height - i * 20)
        '    Next
        '    For j As Integer = 1 To (Picture.Width - Picture.Width Mod 20) / 20
        '        g.DrawLine(netPen, j * 20, 0, j * 20, Picture.Height)
        '    Next
        'End If
        '显示关系
        If GroupToolStripMenuItem1.Checked = True Then
            g.SmoothingMode = SmoothingMode.HighSpeed
            Dim linePoint() As Point
            ReDim linePoint(times - 1)
            If times > 1 Then
                For i As Integer = 1 To (data_count - bchrom) / times
                    Dim x(), y() As Integer
                    ReDim x(times), y(times)
                    For j As Integer = 1 To times
                        x(j) = his_point(Range((i - 1) * times + j)).X * zoom_radio
                        y(j) = his_point(Range((i - 1) * times + j)).Y * zoom_radio
                        linePoint(j - 1).X = x(j)
                        linePoint(j - 1).Y = y(j)
                    Next
                    If times > 2 Then
                        Array.Sort(x, 1, times)
                        Array.Sort(y, 1, times)
                        g.DrawRectangle(pen_line, x(1), y(1), x(times) - x(1), y(times) - y(1))
                    Else
                        g.DrawLine(pen_line, x(1), y(1), x(2), y(2))
                        g.DrawEllipse(pen_line, CInt(((x(1) + x(2)) / 2) - ((x(1) - x(2)) ^ 2 + (y(1) - y(2)) ^ 2) ^ 0.5 / 2), CInt(((y(1) + y(2)) / 2) - ((x(1) - x(2)) ^ 2 + (y(1) - y(2)) ^ 2) ^ 0.5 / 2), CInt(((x(1) - x(2)) ^ 2 + (y(1) - y(2)) ^ 2) ^ 0.5), CInt(((x(1) - x(2)) ^ 2 + (y(1) - y(2)) ^ 2) ^ 0.5))
                    End If
                Next
            End If
        End If



        g.SmoothingMode = SmoothingMode.AntiAlias
        For i As Integer = 1 To data_count
            '绘制历史折线
            If points_group(i).points_type_1 = 0 Then
                If points_group(i).points_count_1 >= 1 Then
                    For j = 0 To points_group(i).points_count_1 - 1
                        If points_group(i).points_value_1 > points_group(i).points_value_2 Then
                            g.DrawLine(pen_arm_1, points_group(i).points_group_1(j).X * zoom_radio, points_group(i).points_group_1(j).Y * zoom_radio, points_group(i).points_group_1(j + 1).X * zoom_radio, points_group(i).points_group_1(j + 1).Y * zoom_radio)
                        Else
                            g.DrawLine(pen_arm_2, points_group(i).points_group_1(j).X * zoom_radio, points_group(i).points_group_1(j).Y * zoom_radio, points_group(i).points_group_1(j + 1).X * zoom_radio, points_group(i).points_group_1(j + 1).Y * zoom_radio)
                        End If
                        '距离
                        If LineLengthToolStripMenuItem.Checked Then
                            g.DrawString((o_distance(points_group(i).points_group_1(j), points_group(i).points_group_1(j + 1)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_1(j + 1).X * zoom_radio, points_group(i).points_group_1(j + 1).Y * zoom_radio - 20)
                        End If
                        g.FillEllipse(brush_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                        g.DrawEllipse(pen_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    Next
                    g.FillEllipse(brush_circle, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio - point_size, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio - point_size, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio)
                End If
            End If
            If points_group(i).points_type_2 = 0 Then
                If points_group(i).points_count_2 >= 1 Then
                    For j = 0 To points_group(i).points_count_2 - 1
                        If points_group(i).points_value_1 > points_group(i).points_value_2 Then
                            g.DrawLine(pen_arm_2, points_group(i).points_group_2(j).X * zoom_radio, points_group(i).points_group_2(j).Y * zoom_radio, points_group(i).points_group_2(j + 1).X * zoom_radio, points_group(i).points_group_2(j + 1).Y * zoom_radio)
                        Else
                            g.DrawLine(pen_arm_1, points_group(i).points_group_2(j).X * zoom_radio, points_group(i).points_group_2(j).Y * zoom_radio, points_group(i).points_group_2(j + 1).X * zoom_radio, points_group(i).points_group_2(j + 1).Y * zoom_radio)
                        End If
                        '距离
                        If LineLengthToolStripMenuItem.Checked Then
                            g.DrawString((o_distance(points_group(i).points_group_2(j), points_group(i).points_group_2(j + 1)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_2(j + 1).X * zoom_radio, points_group(i).points_group_2(j + 1).Y * zoom_radio - 20)
                        End If
                        g.FillEllipse(brush_circle, points_group(i).points_group_2(j).X * zoom_radio - point_size, points_group(i).points_group_2(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                        g.DrawEllipse(pen_circle, points_group(i).points_group_2(j).X * zoom_radio - point_size, points_group(i).points_group_2(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    Next
                    g.FillEllipse(brush_circle, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio - point_size, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio - point_size, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio)
                End If
            End If

            '绘制历史面积
            If points_group(i).points_type_1 = 1 Then
                If points_group(i).points_count_1 > 0 Then
                    '先划线，再画点，最后填充
                    For j = 0 To points_group(i).points_count_1 - 1
                        If points_group(i).points_value_1 > points_group(i).points_value_2 Then
                            g.DrawLine(pen_arm_1, points_group(i).points_group_1(j).X * zoom_radio, points_group(i).points_group_1(j).Y * zoom_radio, points_group(i).points_group_1(j + 1).X * zoom_radio, points_group(i).points_group_1(j + 1).Y * zoom_radio)
                        Else
                            g.DrawLine(pen_arm_2, points_group(i).points_group_1(j).X * zoom_radio, points_group(i).points_group_1(j).Y * zoom_radio, points_group(i).points_group_1(j + 1).X * zoom_radio, points_group(i).points_group_1(j + 1).Y * zoom_radio)
                        End If
                        '距离
                        If LineLengthToolStripMenuItem.Checked Then
                            g.DrawString((o_distance(points_group(i).points_group_1(j), points_group(i).points_group_1(j + 1)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_1(j + 1).X * zoom_radio, points_group(i).points_group_1(j + 1).Y * zoom_radio - 20)
                        End If
                    Next

                    Dim arm_brush As SolidBrush
                    If points_group(i).points_value_1 > points_group(i).points_value_2 Then
                        arm_brush = brush_arm_1
                        g.DrawLine(pen_arm_1, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio)
                    Else
                        arm_brush = brush_arm_2
                        g.DrawLine(pen_arm_2, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio)
                    End If
                    '距离
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(points_group(i).points_group_1(points_group(i).points_count_1), points_group(i).points_group_1(0)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio - 20)
                    End If
                    For j = 0 To points_group(i).points_count_1 - 1
                        g.FillEllipse(brush_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                        g.DrawEllipse(pen_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    Next
                    g.FillEllipse(brush_circle, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio - point_size, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio - point_size, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio)

                    Dim newFillMode As FillMode = FillMode.Winding
                    Dim fillPoint() As Point

                    ReDim fillPoint(points_group(i).points_count_1)
                    For k As Integer = 0 To points_group(i).points_count_1
                        fillPoint(k) = New Point(points_group(i).points_group_1(k).X * zoom_radio, points_group(i).points_group_1(k).Y * zoom_radio)
                    Next
                    g.FillPolygon(arm_brush, fillPoint, newFillMode)
                End If
            End If

            If points_group(i).points_type_2 = 1 Then
                If points_group(i).points_count_2 > 0 Then
                    For j = 0 To points_group(i).points_count_2 - 1
                        If points_group(i).points_value_1 > points_group(i).points_value_2 Then
                            g.DrawLine(pen_arm_2, points_group(i).points_group_2(j).X * zoom_radio, points_group(i).points_group_2(j).Y * zoom_radio, points_group(i).points_group_2(j + 1).X * zoom_radio, points_group(i).points_group_2(j + 1).Y * zoom_radio)
                        Else
                            g.DrawLine(pen_arm_1, points_group(i).points_group_2(j).X * zoom_radio, points_group(i).points_group_2(j).Y * zoom_radio, points_group(i).points_group_2(j + 1).X * zoom_radio, points_group(i).points_group_2(j + 1).Y * zoom_radio)
                        End If
                        '距离
                        If LineLengthToolStripMenuItem.Checked Then
                            g.DrawString((o_distance(points_group(i).points_group_2(j), points_group(i).points_group_2(j + 1)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_2(j + 1).X * zoom_radio, points_group(i).points_group_2(j + 1).Y * zoom_radio - 20)
                        End If
                    Next
                    Dim arm_brush As SolidBrush
                    If points_group(i).points_value_1 > points_group(i).points_value_2 Then
                        arm_brush = brush_arm_2
                        g.DrawLine(pen_arm_2, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio, points_group(i).points_group_2(0).X * zoom_radio, points_group(i).points_group_2(0).Y * zoom_radio)
                    Else
                        arm_brush = brush_arm_1
                        g.DrawLine(pen_arm_1, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio, points_group(i).points_group_2(0).X * zoom_radio, points_group(i).points_group_2(0).Y * zoom_radio)
                    End If
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(points_group(i).points_group_2(points_group(i).points_count_2), points_group(i).points_group_2(0)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_2(0).X * zoom_radio, points_group(i).points_group_2(0).Y * zoom_radio - 20)
                    End If
                    For j = 0 To points_group(i).points_count_2 - 1
                        g.FillEllipse(brush_circle, points_group(i).points_group_2(j).X * zoom_radio - point_size, points_group(i).points_group_2(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                        g.DrawEllipse(pen_circle, points_group(i).points_group_2(j).X * zoom_radio - point_size, points_group(i).points_group_2(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    Next
                    g.FillEllipse(brush_circle, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio - point_size, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio - point_size, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_2(points_group(i).points_count_2).X * zoom_radio, points_group(i).points_group_2(points_group(i).points_count_2).Y * zoom_radio)

                    Dim newFillMode As FillMode = FillMode.Winding
                    Dim fillPoint() As Point

                    ReDim fillPoint(points_group(i).points_count_2)
                    For k As Integer = 0 To points_group(i).points_count_2
                        fillPoint(k) = New Point(points_group(i).points_group_2(k).X * zoom_radio, points_group(i).points_group_2(k).Y * zoom_radio)
                    Next
                    g.FillPolygon(arm_brush, fillPoint, newFillMode)
                End If
            End If
            '历史计数
            If points_group(i).points_type_1 = 2 Then
                For j = 0 To points_group(i).points_count_1
                    g.FillEllipse(brush_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawString(Str(i) & "-" & Str(j + 1), draw_font, brush_font, points_group(i).points_group_1(j).X * zoom_radio, points_group(i).points_group_1(j).Y * zoom_radio)
                Next
            End If
            '绘制历史角度
            If points_group(i).points_type_1 = 3 Then
                If points_group(i).points_count_1 >= 0 Then
                    If (points_group(i).points_group_1(1).X > 0 Or points_group(i).points_group_1(1).Y > 0) Then
                        g.DrawLine(pen_arm_1, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio, points_group(i).points_group_1(1).X * zoom_radio, points_group(i).points_group_1(1).Y * zoom_radio)
                        If LineLengthToolStripMenuItem.Checked Then
                            g.DrawString((o_distance(points_group(i).points_group_1(0), points_group(i).points_group_1(1)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_1(1).X * zoom_radio, points_group(i).points_group_1(1).Y * zoom_radio - 20)
                        End If
                    End If
                    g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio)
                End If
                If points_group(i).points_count_1 >= 2 Then
                    If (points_group(i).points_group_1(3).X > 0 Or points_group(i).points_group_1(3).Y > 0) Then
                        g.DrawLine(pen_arm_2, points_group(i).points_group_1(2).X * zoom_radio, points_group(i).points_group_1(2).Y * zoom_radio, points_group(i).points_group_1(3).X * zoom_radio, points_group(i).points_group_1(3).Y * zoom_radio)
                        If LineLengthToolStripMenuItem.Checked Then
                            g.DrawString((o_distance(points_group(i).points_group_1(2), points_group(i).points_group_1(3)) * current_scale).ToString("F2"), draw_font, brush_font, points_group(i).points_group_1(3).X * zoom_radio, points_group(i).points_group_1(3).Y * zoom_radio - 20)
                        End If
                    End If
                    g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_1(2).X * zoom_radio, points_group(i).points_group_1(2).Y * zoom_radio)

                End If
                For j = 0 To points_group(i).points_count_1
                    g.FillEllipse(brush_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                Next
            End If
            '绘制历史颜色
            If points_group(i).points_type_1 = 4 Then
                '先划线，再画点，最后填充
                g.DrawLine(pen_arm_1, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(1).Y * zoom_radio)
                g.DrawLine(pen_arm_1, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio, points_group(i).points_group_1(1).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio)
                g.DrawLine(pen_arm_1, points_group(i).points_group_1(1).X * zoom_radio, points_group(i).points_group_1(1).Y * zoom_radio, points_group(i).points_group_1(0).X * zoom_radio, points_group(i).points_group_1(1).Y * zoom_radio)
                g.DrawLine(pen_arm_1, points_group(i).points_group_1(1).X * zoom_radio, points_group(i).points_group_1(1).Y * zoom_radio, points_group(i).points_group_1(1).X * zoom_radio, points_group(i).points_group_1(0).Y * zoom_radio)



                For j = 0 To points_group(i).points_count_1 - 1
                    g.FillEllipse(brush_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, points_group(i).points_group_1(j).X * zoom_radio - point_size, points_group(i).points_group_1(j).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                Next
                g.FillEllipse(brush_circle, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio - point_size, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                g.DrawEllipse(pen_circle, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio - point_size, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                g.DrawString(Str(i), draw_font, brush_font, points_group(i).points_group_1(points_group(i).points_count_1).X * zoom_radio, points_group(i).points_group_1(points_group(i).points_count_1).Y * zoom_radio)

            End If
        Next
        '颜色
        If (operate = "color" Or last_type = 4) And has_record = False Then
            Dim meansure_brush As SolidBrush = brush_size
            Dim temp_brush As New SolidBrush(Color.FromArgb(32, brush_size.Color.R, brush_size.Color.G, brush_size.Color.B))
            If in_meansure And node_count >= 0 Then
                If (line_point(1).X > 0 Or line_point(1).Y > 0) Then
                    g.DrawLine(pen_line, line_point(0), New Point(line_point(0).X, line_point(1).Y))
                    g.DrawLine(pen_line, line_point(0), New Point(line_point(1).X, line_point(0).Y))
                    g.DrawLine(pen_line, line_point(1), New Point(line_point(0).X, line_point(1).Y))
                    g.DrawLine(pen_line, line_point(1), New Point(line_point(1).X, line_point(0).Y))

                    'Dim newFillMode As FillMode = FillMode.Winding
                    'Dim fillPoint(3) As Point
                    'fillPoint(0) = line_point(1)
                    'fillPoint(1) = New Point(line_point(0).X, line_point(1).Y)
                    'fillPoint(2) = line_point(0)
                    'fillPoint(3) = New Point(line_point(1).X, line_point(0).Y)
                    'g.FillPolygon(meansure_brush, fillPoint, newFillMode)
                End If



            ElseIf node_count = 1 Then
                g.DrawLine(pen_line, line_point(0), New Point(line_point(0).X, line_point(1).Y))
                g.DrawLine(pen_line, line_point(0), New Point(line_point(1).X, line_point(0).Y))
                g.DrawLine(pen_line, line_point(1), New Point(line_point(0).X, line_point(1).Y))
                g.DrawLine(pen_line, line_point(1), New Point(line_point(1).X, line_point(0).Y))

                'If node_count = 1 Then
                '    Dim newFillMode As FillMode = FillMode.Winding
                '    Dim fillPoint() As Point

                '    ReDim fillPoint(3)
                '    fillPoint(0) = line_point(1)
                '    fillPoint(1) = New Point(line_point(0).X, line_point(1).Y)
                '    fillPoint(2) = line_point(0)
                '    fillPoint(3) = New Point(line_point(1).X, line_point(0).Y)

                '    g.FillPolygon(meansure_brush, fillPoint, newFillMode)

                'End If
            End If
            If node_count = 1 Then
                For j = 0 To node_count - 1
                    g.FillEllipse(brush_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                Next
                '防止原点闪烁
                If line_point(node_count).X > 0 Or line_point(node_count).Y > 0 Then
                    g.FillEllipse(brush_circle, line_point(node_count).X - point_size, line_point(node_count).Y - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, line_point(node_count).X - point_size, line_point(node_count).Y - point_size, point_size * 2, point_size * 2)
                    If my_listview.SelectedIndices.Count > 0 Then
                        g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(node_count))
                    End If
                End If
            ElseIf node_count >= 0 And (line_point(0).X > 0 Or line_point(0).Y > 0) Then
                g.FillEllipse(brush_circle, line_point(0).X - point_size, line_point(0).Y - point_size, point_size * 2, point_size * 2)
                g.DrawEllipse(pen_circle, line_point(0).X - point_size, line_point(0).Y - point_size, point_size * 2, point_size * 2)
            End If
        End If
        '角度
        If (operate = "angle" Or last_type = 3) Then
            If node_count >= 0 Then
                If (line_point(1).X > 0 Or line_point(1).Y > 0) Then
                    g.DrawLine(pen_arm_1, line_point(0), line_point(1))
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(line_point(0), line_point(1)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(1).X, line_point(1).Y - 20)
                    End If
                End If
                If my_listview.SelectedIndices.Count > 0 Then
                    g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(0))
                End If
                Dim temp_points() As Point = get_extend_line(line_point(0), line_point(1))
                g.DrawLine(New Pen(pen_arm_1.Color, 1), temp_points(1), temp_points(0))

            End If
            If node_count >= 2 Then
                If (line_point(3).X > 0 Or line_point(3).Y > 0) Then
                    g.DrawLine(pen_arm_2, line_point(2), line_point(3))
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(line_point(2), line_point(3)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(3).X, line_point(3).Y - 20)
                    End If
                End If
                If my_listview.SelectedIndices.Count > 0 Then
                    g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(2))
                End If
                Dim temp_points() As Point = get_extend_line(line_point(2), line_point(3))
                g.DrawLine(New Pen(pen_arm_2.Color, 1), temp_points(1), temp_points(0))
            End If
            For j = 0 To node_count
                g.FillEllipse(brush_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                g.DrawEllipse(pen_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)

            Next
        End If

        '计数
        If (operate = "count" Or last_type = 2) Then
            For j = 0 To node_count
                g.FillEllipse(brush_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                g.DrawEllipse(pen_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                If my_listview.SelectedIndices.Count > 0 Then
                    g.DrawString(Str(my_listview.SelectedIndices(0) + 1) & "-" & Str(j + 1), draw_font, brush_font, line_point(j))

                Else
                    g.DrawString("#" & Str(j + 1), draw_font, brush_font, line_point(j))

                End If
            Next
        End If

        '划折线
        If (operate = "lines" Or last_type = 0) And has_record = False Then
            '先画线，再画点
            If in_meansure And node_count >= 0 Then
                '防止原点闪烁
                If (line_point(node_count + 1).X > 0 Or line_point(node_count + 1).Y > 0) Then
                    g.DrawLine(pen_line, line_point(node_count), line_point(node_count + 1))
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(line_point(node_count), line_point(node_count + 1)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(node_count + 1).X, line_point(node_count + 1).Y - 20)
                    End If
                End If
                If my_listview.SelectedIndices.Count > 0 Then
                    g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(node_count + 1))
                End If
            ElseIf in_meansure Then
                If my_listview.SelectedIndices.Count > 0 Then
                    If (line_point(node_count + 1).X > 0 Or line_point(node_count + 1).Y > 0) Then
                        g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(node_count + 1))
                    End If
                End If
            End If

            If node_count >= 1 Then
                For j = 0 To node_count - 1
                    g.DrawLine(pen_line, line_point(j), line_point(j + 1))
                    '距离
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(line_point(j), line_point(j + 1)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(j + 1).X, line_point(j + 1).Y - 20)
                    End If

                Next
                For j = 0 To node_count - 1
                    g.FillEllipse(brush_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                Next
                '防止原点闪烁
                If line_point(node_count).X > 0 Or line_point(node_count).Y > 0 Then
                    g.FillEllipse(brush_circle, line_point(node_count).X - point_size, line_point(node_count).Y - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, line_point(node_count).X - point_size, line_point(node_count).Y - point_size, point_size * 2, point_size * 2)
                    If my_listview.SelectedIndices.Count > 0 Then
                        g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(node_count))
                    End If
                End If

            ElseIf node_count >= 0 And (line_point(0).X > 0 Or line_point(0).Y > 0) Then
                g.FillEllipse(brush_circle, line_point(0).X - point_size, line_point(0).Y - point_size, point_size * 2, point_size * 2)
                g.DrawEllipse(pen_circle, line_point(0).X - point_size, line_point(0).Y - point_size, point_size * 2, point_size * 2)
            End If
        End If

        '填充面积
        If (operate = "size" Or last_type = 1) And has_record = False Then
            Dim meansure_brush As SolidBrush = brush_size
            Dim temp_brush As New SolidBrush(Color.FromArgb(32, brush_size.Color.R, brush_size.Color.G, brush_size.Color.B))
            If in_meansure And node_count >= 0 Then
                If (line_point(node_count + 1).X > 0 Or line_point(node_count + 1).Y > 0) Then
                    g.DrawLine(pen_line, line_point(node_count), line_point(node_count + 1))
                    g.DrawLine(pen_line, line_point(node_count + 1), line_point(0))
                    '距离
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(line_point(node_count), line_point(node_count + 1)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(node_count + 1).X, line_point(node_count + 1).Y - 20)
                        g.DrawString((o_distance(line_point(node_count + 1), line_point(0)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(0).X, line_point(0).Y - 20)

                    End If
                End If

                If node_count > 0 Then
                    Dim newFillMode As FillMode = FillMode.Winding
                    Dim fillPoint() As Point
                    ReDim fillPoint(node_count)
                    For k As Integer = 0 To node_count
                        fillPoint(k) = line_point(k)
                    Next
                    g.FillPolygon(meansure_brush, fillPoint, newFillMode)
                    '防止原点闪烁
                    If (line_point(node_count + 1).X > 0 Or line_point(node_count + 1).Y > 0) Then
                        Dim AreaPoint() As Point = {line_point(0), line_point(node_count + 1), line_point(node_count)}
                        g.FillPolygon(temp_brush, AreaPoint, newFillMode)
                    End If
                End If
            ElseIf node_count >= 0 Then
                g.DrawLine(pen_line, line_point(node_count), line_point(0))
                '距离
                If LineLengthToolStripMenuItem.Checked Then
                    g.DrawString((o_distance(line_point(node_count), line_point(0)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(0).X, line_point(0).Y - 20)
                End If
                If node_count > 0 Then
                    Dim newFillMode As FillMode = FillMode.Winding
                    Dim fillPoint() As Point

                    ReDim fillPoint(node_count)
                    For k As Integer = 0 To node_count
                        fillPoint(k) = line_point(k)
                    Next
                    g.FillPolygon(meansure_brush, fillPoint, newFillMode)
                End If
            End If
            If node_count >= 1 Then
                For j = 0 To node_count - 1
                    g.DrawLine(pen_line, line_point(j), line_point(j + 1))
                    '距离
                    If LineLengthToolStripMenuItem.Checked Then
                        g.DrawString((o_distance(line_point(j), line_point(j + 1)) / zoom_radio * current_scale).ToString("F2"), draw_font, brush_font, line_point(j + 1).X, line_point(j + 1).Y - 20)
                    End If
                Next
                For j = 0 To node_count - 1
                    g.FillEllipse(brush_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, line_point(j).X - point_size, line_point(j).Y - point_size, point_size * 2, point_size * 2)
                Next
                '防止原点闪烁
                If line_point(node_count).X > 0 Or line_point(node_count).Y > 0 Then
                    g.FillEllipse(brush_circle, line_point(node_count).X - point_size, line_point(node_count).Y - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, line_point(node_count).X - point_size, line_point(node_count).Y - point_size, point_size * 2, point_size * 2)
                    If my_listview.SelectedIndices.Count > 0 Then
                        g.DrawString(Str(my_listview.SelectedIndices(0) + 1), draw_font, brush_font, line_point(node_count))
                    End If
                End If
            ElseIf node_count >= 0 And (line_point(0).X > 0 Or line_point(0).Y > 0) Then
                g.FillEllipse(brush_circle, line_point(0).X - point_size, line_point(0).Y - point_size, point_size * 2, point_size * 2)
                g.DrawEllipse(pen_circle, line_point(0).X - point_size, line_point(0).Y - point_size, point_size * 2, point_size * 2)
            End If
        End If

        If my_radio <> 1 Then
            point_size = point_size / my_radio
            scale_pen.Width = 3
            pen_circle.Width = 2
            pen_arm_1.Width = 2
            pen_arm_2.Width = 2
            pen_line.Width = 2
        End If
        '显示历史测量点
        If NodeIDToolStripMenuItem.Checked And mode_type = 1 Then
            For k As Integer = 1 To data_count
                If his_point(k).X * his_point(k).Y <> 0 Then
                    g.FillEllipse(brush_circle, his_point(k).X * zoom_radio - point_size, his_point(k).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawEllipse(pen_circle, his_point(k).X * zoom_radio - point_size, his_point(k).Y * zoom_radio - point_size, point_size * 2, point_size * 2)
                    g.DrawString(Str(k), draw_font, brush_font, his_point(k).X * zoom_radio, his_point(k).Y * zoom_radio)
                End If
            Next
        End If

    End Sub
    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        e.Cancel = True
        Select Case mode_type
            Case 0
                If data_count > 0 Then
                    Dim CloseDialog As DialogResult
                    CloseDialog = MessageBox.Show("Save current result?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk)
                    If CloseDialog = DialogResult.Yes Then
                        Dim sfd As New SaveFileDialog
                        sfd.Filter = "csv file(*.csv;*.CSV)|*.csv;*.CSV|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
                        sfd.FileName = ""
                        sfd.DefaultExt = ".csv"
                        sfd.CheckPathExists = True
                        Dim resultdialog As DialogResult = sfd.ShowDialog()
                        If resultdialog = DialogResult.OK Then
                            save_csv(sfd.FileName, ListView2)
                        End If
                        End
                    ElseIf CloseDialog = DialogResult.No Then
                        End
                    End If
                Else
                    End
                End If
            Case 1
                Dim temp_data_count As Integer = data_count
                If longarm(data_count) = -1 Or shortarm(data_count) = -1 Then
                    data_count -= 1
                End If
                If data_count > 0 Then
                    Dim CloseDialog As DialogResult
                    CloseDialog = MessageBox.Show("Save current result?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk)
                    If CloseDialog = DialogResult.Yes Then
                        If longarm(data_count) = -1 Or shortarm(data_count) = -1 Then
                            data_count -= 1
                        End If
                        Dim sfd As New SaveFileDialog
                        sfd.Filter = "Karyotype Files(*.karyo;*.nuc)|*.karyo;*.KARYO;*.nuc;*.NUC|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
                        sfd.FileName = ""
                        sfd.DefaultExt = ".karyo"
                        sfd.CheckPathExists = True
                        Dim resultdialog As DialogResult = sfd.ShowDialog()
                        If resultdialog = DialogResult.OK Then
                            write_karyo(sfd.FileName)
                        End If
                        End
                    ElseIf CloseDialog = DialogResult.No Then
                        End
                    End If
                Else
                    End
                End If
                data_count = temp_data_count
        End Select
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Threading.Thread.CurrentThread.CurrentCulture = ci
        '初始化界面
        Select Case mode_type
            Case 0
                GeneralToolStripMenuItem.Checked = True
                KaryotypeToolStripMenuItem.Checked = False
                TabPage2.Parent = Nothing
                TabPage1.Parent = TabControl1
                ListView1.Visible = False
                ListView2.Visible = True

                TS_Group.Visible = False
                TS_Count.Visible = True
                TS_Angle.Visible = True
                TS_Color.Visible = True
                TS_Remove.Visible = True

                KaryotypeFileToolStripMenuItem.Visible = False
                ResultsToolStripMenuItem.Visible = True
                GroupToolStripMenuItem.Visible = False
                GroupToolStripMenuItem1.Visible = True
                CombineResultsToolStripMenuItem.Visible = False
                ToolStripSeparator1.Visible = False
                SaveFileToolStripMenuItem.Visible = False
            Case 1
                GeneralToolStripMenuItem.Checked = False
                KaryotypeToolStripMenuItem.Checked = True
                TabPage1.Parent = Nothing
                TabPage2.Parent = TabControl1
                ListView1.Visible = True
                ListView2.Visible = False
                TS_Group.Visible = True
                TS_Count.Visible = False
                TS_Angle.Visible = False
                TS_Color.Visible = False
                TS_Remove.Visible = False

                KaryotypeFileToolStripMenuItem.Visible = True
                ResultsToolStripMenuItem.Visible = False
                GroupToolStripMenuItem1.Visible = False
                GroupToolStripMenuItem.Visible = True
                CombineResultsToolStripMenuItem.Visible = True
                ToolStripSeparator1.Visible = True
                SaveFileToolStripMenuItem.Visible = True
        End Select


        Me.Text = Me.Text '+ " " + build_version
        '定义核型种类
        typestr(1) = "M"
        typestr(2) = "m"
        typestr(3) = "sm"
        typestr(4) = "st"
        typestr(5) = "t"
        typestr(6) = "T"
        '初始化数组
        For i As Integer = 0 To 2048
            Range(i) = i
            data_id(i) = i
            longarm(i) = -1
            shortarm(i) = -1
            suiarm(i) = 0
            points_group(i) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
        Next

        'ListView1.Items.Add(New ListViewItem({data_count.ToString, "", "", "0", "F"}))
        'ListView1.Items(0).Selected = True

        'ListView2.Items.Add(New ListViewItem({data_count.ToString, "", "", "", ""}))
        'ListView2.Items(0).Selected = True


        Dim loadfile As String = Command()
        exepath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\") + 1)
        If loadfile <> "" Then
            If loadfile.EndsWith(Chr(34)) Then
                loadfile = loadfile.Remove(0, 1)
                loadfile = loadfile.Remove(loadfile.Length - 1, 1)
            End If
            If loadfile.ToUpper.EndsWith(".NUC") Or loadfile.ToUpper.EndsWith(".KARYO") Or loadfile.ToUpper.EndsWith(".TXT") Then
                read_karyo(loadfile)
                sumarm = 1000
                summod = 1000
                For j As Integer = 1 To data_count
                    sumarm = sumarm + longarm(j) + shortarm(j)
                    If shortarm(j) > 0.1 Then
                        summod = summod + longarm(j) / shortarm(j)
                    End If
                Next
                sumarm = sumarm - 1000
                summod = summod - 1000
            End If
            exepath = GetSetting("Karyotype", "exePath", "exePath", exepath)
        Else
            'win10下会异常
            'mak_fa("nuc", "nuc", exepath + "KaryoType.exe", exepath + "KaryoType.exe")
            'mak_fa("karyo", "karyo", exepath + "KaryoType.exe", exepath + "KaryoType.exe")
            SaveSetting("Karyotype", "exePath", "exePath", exepath)
        End If

        'JudgeReg()
        Try
            load_scale()
        Catch ex As Exception
        End Try

    End Sub
    Private Sub Picture_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Picture.Paint
        Try
            Select Case mode_type
                Case 0
                    draw_lines(e.Graphics, 1, ListView2)
                Case 1
                    draw_lines(e.Graphics, 1, ListView1)
            End Select

        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Picture_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Picture.MouseUp
        Try
            If e.Button = MouseButtons.Middle Or (e.Button = MouseButtons.Left And DisableLeftButtonDragToolStripMenuItem.Checked = False) Then
                bPictureBoxDragging = False
                Picture.Cursor = Cursors.Default
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Sub Picture_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Picture.MouseDown
        Try
            If e.Button = MouseButtons.Middle Or (e.Button = MouseButtons.Left And DisableLeftButtonDragToolStripMenuItem.Checked = False) Then
                Picture.Cursor = Cursors.Hand
                bPictureBoxDragging = True
                oPointClicked = Me.PointToClient(Picture.PointToScreen(New Point(e.X, e.Y)))
                loactionClicked = New Point(Picture.Location.X, Picture.Location.Y)
            End If
            '显示颜色
            Select Case mode_type
                Case 0
                    If current_bmp IsNot Nothing And e.Button = MouseButtons.Left Then
                        Picture_color.BackColor = current_bmp.GetPixel(e.X / zoom_radio, e.Y / zoom_radio)
                        Picture_color.Refresh()
                        TextBox5.Text = Picture_color.BackColor.R
                        TextBox6.Text = Picture_color.BackColor.G
                        TextBox7.Text = Picture_color.BackColor.B
                        '简化 sRGB IEC61966-2.1 [gamma=2.20]
                        'Gray = (R ^ 2.2 * 0.2126 + G ^ 2.2 * 0.7152 + B ^ 2.2 * 0.0722) ^ (1 / 2.2)
                        TextBox8.Text = CInt((CInt(Picture_color.BackColor.R) ^ 2.2 * 0.2126 + CInt(Picture_color.BackColor.G) ^ 2.2 * 0.7152 + CInt(Picture_color.BackColor.B) ^ 2.2 * 0.0722) ^ (1 / 2.2))
                        TextBox9.Text = "#" + DEC_to_HEX(Picture_color.BackColor.R) + DEC_to_HEX(Picture_color.BackColor.G) + DEC_to_HEX(Picture_color.BackColor.B)
                    End If
                Case 1
                Case Else
            End Select
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Public CropImage = New Bitmap(225, 150)
    Dim wait_count As Integer = 0
    Private Sub Picture_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Picture.MouseMove
        Try
            If bPictureBoxDragging Then
                Dim oMoveToPoint As Point
                oMoveToPoint = Me.PointToClient(Picture.PointToScreen(New Point(e.X, e.Y)))
                oMoveToPoint.Offset(loactionClicked.X - oPointClicked.X, loactionClicked.Y - oPointClicked.Y)
                Picture.Location = oMoveToPoint
            Else
                If operate <> "null" Then
                    Picture.Cursor = Cursors.Cross
                    If in_meansure Then
                        line_point(node_count + 1) = New Point(e.X, e.Y)

                        If wait_count < 2 Then
                            wait_count += 1
                        Else
                            wait_count = 0
                        End If
                        If wait_count Mod 2 = 1 Then
                            Picture.Refresh()
                        Else
                            'Dim g As Graphics = Graphics.FromHwnd(Picture.Handle)
                            'Dim pe As PaintEventArgs = New PaintEventArgs(g, Picture.ClientRectangle)

                            'Picture_Paint(Picture, pe)
                            'g.Dispose()
                        End If
                        If operate = "angle" Then
                            If node_count = 2 Then
                                Dim angle1 As Single = Math.Atan2((line_point(0).Y - line_point(1).Y), (line_point(0).X - line_point(1).X))
                                Dim angle2 As Single = Math.Atan2((line_point(2).Y - line_point(3).Y), (line_point(2).X - line_point(3).X))
                                Select Case mode_type
                                    Case 0
                                        TextBox4.Text = CSng((Abs(angle1 - angle2) / Math.PI * 180)).ToString("F2")
                                    Case 1
                                        'TextBox1.Text = CInt((Abs(angle1 - angle2) / Math.PI * 180) Mod 180)
                                End Select
                            End If
                        End If
                    End If
                Else
                    'Picture.Cursor = Cursors.Arrow
                End If

                '显示坐标
                ToolStripStatusLabel3.Visible = CoordinatesToolStripMenuItem.Checked
                If CoordinatesToolStripMenuItem.Checked Then
                    ToolStripStatusLabel3.Text = "(" + (e.X / zoom_radio).ToString("F2") + "," + (e.Y / zoom_radio).ToString("F2") + ")"
                End If
                If operate = "null" Then
                    ToolStripStatusLabel2.Text = "Left double click to start new meansurement"
                Else
                    ToolStripStatusLabel2.Text = "Right click to stop meansurement"
                End If

                If current_bmp IsNot Nothing Then
                    'Dim th1 As New Thread(AddressOf zoom_pic)

                    Dim CropRect As New Rectangle(e.X / zoom_radio - 75, e.Y / zoom_radio - 50, 150, 100)
                    Dim g As Graphics = Graphics.FromImage(CropImage)
                    g.SmoothingMode = SmoothingMode.HighSpeed
                    g.DrawImage(current_bmp, New Rectangle(0, 0, 225, 150), CropRect, GraphicsUnit.Pixel)
                    g.DrawLine(pen_arm_1, New Point(0, 75), New Point(225, 75))
                    g.DrawLine(pen_arm_2, New Point(112, 0), New Point(112, 150))
                    g.Dispose()
                    PictureZoom.Image = CropImage
                End If


            End If


        Catch ex As Exception
            System.Windows.Forms.MessageBox.Show(ex.Message)
        End Try
    End Sub
    Public Sub zoom_pic()

    End Sub
    Public Sub meansure_new()
        Select Case mode_type
            Case 0F
                data_count += 1
                ListView2.Items.Add(New ListViewItem({data_count.ToString, "", "", "", ""}))
                ListView2.Items(data_count - 1).Selected = True
                Select Case last_type
                    Case 0
                        meansure_lines()
                    Case 1
                        meansure_size()
                    Case 2
                        meansure_count()
                    Case 3
                        meansure_angle()
                    Case Else
                        operate = "null"
                End Select
                Me.ToolStripStatusLabel1.Text = "New meansurement"
            Case 1
                If data_count > 0 Then
                    If longarm(data_count) <> -1 And shortarm(data_count) <> -1 Then
                        sumarm = 1000
                        summod = 1000
                        For j As Integer = 1 To data_count
                            sumarm = sumarm + longarm(j) + shortarm(j)
                            If shortarm(j) > 0.1 Then
                                summod = summod + longarm(j) / shortarm(j)
                            End If
                        Next
                        sumarm = sumarm - 1000
                        summod = summod - 1000
                        data_count += 1
                        Range(data_count) = data_count
                        ListView1.Items.Add(New ListViewItem({data_count.ToString, "", "", "0", "F"}))
                        Label1.Text = "->"
                        Label2.Text = ""
                        TextBox2.Text = ""
                        TextBox3.Text = ""
                        ListView1.Items(data_count - 1).Selected = True
                        Me.ToolStripStatusLabel1.Text = "New chromosomes"
                        Select Case last_type
                            Case 0
                                meansure_lines()
                            Case 1
                                meansure_size()
                            Case Else
                                operate = "null"
                        End Select
                    Else
                        MessageBox.Show("Please complete the current work！", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                    End If
                ElseIf data_count = 0 Then
                    data_count += 1
                    Range(data_count) = data_count
                    ListView1.Items.Add(New ListViewItem({data_count.ToString, "", "", "0", "F"}))
                    Label1.Text = "->"
                    Label2.Text = ""
                    TextBox2.Text = ""
                    TextBox3.Text = ""
                    ListView1.Items(data_count - 1).Selected = True
                    Me.ToolStripStatusLabel1.Text = "New chromosomes"
                    Select Case last_type
                        Case 0
                            meansure_lines()
                        Case 1
                            meansure_size()
                        Case Else
                            operate = "null"
                    End Select
                End If

        End Select

    End Sub
    Private Sub TS_New_Click(sender As Object, e As EventArgs) Handles TS_New.Click
        meansure_new()
    End Sub
    Public Sub meansure_lines(Optional clean As Boolean = True)
        If TS_Lines.Checked = False Then
            TS_Lines.Checked = True
            TS_Size.Checked = False
            TS_In.Checked = False
            TS_Color.Checked = False
            TS_Out.Checked = False
            bPictureBoxDragging = False
            TS_Angle.Checked = False
            TS_Count.Checked = False
            last_type = 0
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Cross
            operate = "lines"
            ListView1.Text = "Length"
            has_record = False
            Me.ToolStripStatusLabel1.Text = "Tracing length"
        Else
            TS_Lines.Checked = False
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Arrow
            operate = "null"
            Me.ToolStripStatusLabel1.Text = ""
        End If
    End Sub
    Private Sub TS_Lines_Click(sender As Object, e As EventArgs) Handles TS_Lines.Click
        meansure_lines()
    End Sub
    Public Sub meansure_size(Optional clean As Boolean = True)
        If TS_Size.Checked = False Then
            TS_Lines.Checked = False
            TS_Size.Checked = True
            TS_Color.Checked = False
            TS_In.Checked = False
            TS_Out.Checked = False
            TS_Angle.Checked = False
            TS_Count.Checked = False
            bPictureBoxDragging = False
            last_type = 1
            If clean Then
                Init()
            End If

            Picture.Cursor = Cursors.Cross
            operate = "size"

            ListView1.Text = "Size"

            has_record = False
            Me.ToolStripStatusLabel1.Text = "Tracing size"
        Else
            TS_Size.Checked = False
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Arrow
            operate = "null"
            Me.ToolStripStatusLabel1.Text = ""
        End If
    End Sub
    Private Sub TS_Size_Click(sender As Object, e As EventArgs) Handles TS_Size.Click
        meansure_size()
    End Sub

    Private Sub TS_Delete_Click(sender As Object, e As EventArgs) Handles TS_Delete.Click
        Select Case mode_type
            Case 0
                Dim delDialog As DialogResult
                delDialog = MessageBox.Show("Decrease the number of ID?", "Decrease", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                If delDialog = DialogResult.Yes Then
                    If data_count > 1 Then
                        ListView2.Items.RemoveAt(data_count - 1)
                        points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)

                        his_point(data_count).X = 0
                        his_point(data_count).Y = 0
                        data_count = data_count - 1
                        Me.ToolStripStatusLabel1.Text = ""
                        Picture.Refresh()
                        ListView2.Items(data_count - 1).Selected = True
                    ElseIf data_count = 1 Then
                        ListView2.Items.RemoveAt(data_count - 1)
                        points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)

                        his_point(data_count).X = 0
                        his_point(data_count).Y = 0
                        Me.ToolStripStatusLabel1.Text = ""
                        Picture.Refresh()
                        data_count -= 1
                        'ListView2.Items.Add(New ListViewItem({data_count.ToString, "", "", "", ""}))
                        'ListView2.Items(data_count - 1).Selected = True
                        'MsgBox("There is no ID left！")
                    End If
                End If
            Case 1
                Dim delDialog As DialogResult
                delDialog = MessageBox.Show("Decrease the number of chromosomes?", "Decrease", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                If delDialog = DialogResult.Yes Then
                    If data_count > 1 Then
                        ListView1.Items.RemoveAt(data_count - 1)
                        '清空数据
                        points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                        bchrom_list(data_count) = False
                        longarm(data_count) = -1
                        shortarm(data_count) = -1
                        suiarm(data_count) = 0

                        his_point(data_count).X = 0
                        his_point(data_count).Y = 0
                        data_count = data_count - 1
                        Me.ToolStripStatusLabel1.Text = ""
                        Picture.Refresh()
                        ListView1.Items(data_count - 1).Selected = True
                    ElseIf data_count = 1 Then
                        ListView1.Items.RemoveAt(data_count - 1)
                        '清空数据
                        points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                        bchrom_list(data_count) = False
                        longarm(data_count) = -1
                        shortarm(data_count) = -1
                        suiarm(data_count) = 0


                        his_point(data_count).X = 0
                        his_point(data_count).Y = 0
                        data_count = data_count - 1
                        Me.ToolStripStatusLabel1.Text = ""
                        Picture.Refresh()
                    End If
                End If
        End Select

    End Sub
    Public Sub meansure_angle(Optional clean As Boolean = True)
        If TS_Angle.Checked = False Then
            TS_Lines.Checked = False
            TS_Size.Checked = False
            TS_In.Checked = False
            TS_Color.Checked = False
            TS_Out.Checked = False
            bPictureBoxDragging = False
            TS_Angle.Checked = True
            TS_Count.Checked = False
            last_type = 3
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Cross
            operate = "angle"
            has_record = False
            Me.ToolStripStatusLabel1.Text = "Tracing count"
        Else
            TS_Angle.Checked = False
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Arrow
            operate = "null"
            Me.ToolStripStatusLabel1.Text = ""
        End If
    End Sub
    Private Sub TS_Angle_Click(sender As Object, e As EventArgs) Handles TS_Angle.Click
        meansure_angle()
    End Sub

    Public Sub meansure_count(Optional clean As Boolean = True)
        If TS_Count.Checked = False Then
            TS_Lines.Checked = False
            TS_Size.Checked = False
            TS_In.Checked = False
            TS_Out.Checked = False
            TS_Color.Checked = False
            bPictureBoxDragging = False
            TS_Angle.Checked = False
            TS_Count.Checked = True
            last_type = 2
            If clean Then
                Init()
            End If
            operate = "count"
            Picture.Cursor = Cursors.Cross
            has_record = False
            Me.ToolStripStatusLabel1.Text = "Tracing count"
        Else
            TS_Count.Checked = False
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Arrow
            operate = "null"
            Me.ToolStripStatusLabel1.Text = ""
        End If
    End Sub
    Private Sub TS_Remove_Click(sender As Object, e As EventArgs) Handles TS_Remove.Click
        If node_count >= 0 Then
            node_count -= 1
            last_count = node_count
            Picture.Refresh()
        End If
    End Sub
    Private Sub TS_Count_Click(sender As Object, e As EventArgs) Handles TS_Count.Click
        meansure_count()
    End Sub
    Private Sub TS_In_Click(sender As Object, e As EventArgs) Handles TS_In.Click
        Dim newsize As Size
        newsize = Me.Picture.Size
        If newsize.Width < 15000 And newsize.Height < 15000 Then
            zoom_radio = 1.25 * zoom_radio
            newsize.Width = CInt(newsize.Width * 1.25)
            newsize.Height = CInt(newsize.Height * 1.25)
            Picture.Size = newsize
            Picture.Location = New Point(Picture.Location.X * 1.25, Picture.Location.Y * 1.25)
            Me.ToolStripStatusLabel1.Text = "zoom in"

            For i = 0 To node_count
                line_point(i) = New Point(line_point(i).X * 1.25, line_point(i).Y * 1.25)
            Next
            Picture.Refresh()
        Else
            MessageBox.Show("The photo is too large to zoom in")
        End If
    End Sub

    Private Sub TS_Out_Click(sender As Object, e As EventArgs) Handles TS_Out.Click
        Dim newsize As Size
        zoom_radio = 0.8 * zoom_radio
        newsize = Me.Picture.Size
        newsize.Width = CInt(newsize.Width * 0.8)
        newsize.Height = CInt(newsize.Height * 0.8)
        Me.Picture.Size = newsize
        Picture.Location = New Point(Picture.Location.X * 0.8, Picture.Location.Y * 0.8)
        Me.ToolStripStatusLabel1.Text = "zoom out"
        For i = 0 To node_count
            line_point(i) = New Point(line_point(i).X * 0.8, line_point(i).Y * 0.8)
        Next
        Picture.Refresh()
    End Sub

    Private Sub TS_Group_Click(sender As Object, e As EventArgs) Handles TS_Group.Click
        Dim beiti As String = InputBox("Enter the ploidy level: ", "ploidy", times)
        If beiti <> "" Then
            For i As Integer = 0 To 2048
                Range(i) = i
            Next
            times = CInt(beiti)
            Analyse1(1)
            info()
            'Dim CloseDialog As DialogResult
            'CloseDialog = MessageBox.Show("Show the result of group？", "GROUP", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
            'If CloseDialog = DialogResult.Yes Then
            Analysisform.Show()
            Analysisform.Refresh()
            'End If
        End If
        Me.ToolStripStatusLabel1.Text = ""
    End Sub

    Private Sub PhotosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PhotosToolStripMenuItem.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Image Files(*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff)|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff|ALL Files(*.*)|*.*"
        opendialog.FileName = ""
        opendialog.DefaultExt = ".jpg"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        zoom_radio = 1
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            open_image(opendialog.FileName)
        End If

        GC.Collect()
        Picture.Refresh()
    End Sub


    Public Sub ChangeArm(ByVal i As Integer)
        Dim temparm As Single
        If longarm(i) < shortarm(i) Then
            temparm = shortarm(i)
            shortarm(i) = longarm(i)
            longarm(i) = temparm
            'If points_group(ListView1.SelectedIndices(0) + 1).points_count_1 > 0 And points_group(ListView1.SelectedIndices(0) + 1).points_count_2 > 0 Then
            '    Dim temp_points_count As Integer = points_group(ListView1.SelectedIndices(0) + 1).points_count_1
            '    Dim temp_points_type As Integer = points_group(ListView1.SelectedIndices(0) + 1).points_type_1
            '    Dim temp_points() As Point = points_group(ListView1.SelectedIndices(0) + 1).points_group_1.Clone
            '    points_group(ListView1.SelectedIndices(0) + 1).points_count_1 = points_group(ListView1.SelectedIndices(0) + 1).points_count_2
            '    points_group(ListView1.SelectedIndices(0) + 1).points_type_1 = points_group(ListView1.SelectedIndices(0) + 1).points_type_2
            '    points_group(ListView1.SelectedIndices(0) + 1).points_group_1 = points_group(ListView1.SelectedIndices(0) + 1).points_group_2.Clone
            '    points_group(ListView1.SelectedIndices(0) + 1).points_count_2 = temp_points_count
            '    points_group(ListView1.SelectedIndices(0) + 1).points_type_2 = temp_points_type
            '    points_group(ListView1.SelectedIndices(0) + 1).points_group_2 = temp_points.Clone
            'End If
        End If

    End Sub
    Public Sub save_point_group_1(ByVal id As Integer, ByVal my_value As Object)
        points_group(id).points_type_1 = last_type
        points_group(id).points_count_1 = last_count
        points_group(id).points_value_1 = my_value
        For i As Integer = 0 To last_count
            points_group(id).points_group_1(i) = New Point(line_point(i).X / zoom_radio, line_point(i).Y / zoom_radio)
        Next
        his_point(id) = points_group(id).points_group_1(last_count)
        has_record = True
    End Sub
    Public Sub save_point_group_2(ByVal id As Integer, ByVal my_value As Single)

        points_group(id).points_type_2 = last_type
        points_group(id).points_count_2 = last_count
        points_group(id).points_value_2 = my_value
        For i As Integer = 0 To last_count
            points_group(id).points_group_2(i) = New Point(line_point(i).X / zoom_radio, line_point(i).Y / zoom_radio)
        Next
        his_point(id) = points_group(id).points_group_2(last_count)
        has_record = True

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ListView1.SelectedIndices.Count > 0 Then

            TextBox2.Text = TextBox1.Text
            save_point_group_1(ListView1.SelectedIndices(0) + 1, TextBox2.Text)

            If TextBox2.Text <> "" Then
                longarm(ListView1.SelectedIndices(0) + 1) = TextBox2.Text
            End If
            If TextBox3.Text <> "" Then
                shortarm(ListView1.SelectedIndices(0) + 1) = TextBox3.Text
            End If
            If TextBox2.Text <> "" And TextBox3.Text <> "" Then
                ChangeArm(ListView1.SelectedIndices(0) + 1)
                ListView1.SelectedItems(0).SubItems(2).Text = shortarm(ListView1.SelectedIndices(0) + 1)
                ListView1.SelectedItems(0).SubItems(1).Text = longarm(ListView1.SelectedIndices(0) + 1)
                sumarm = 1000
                summod = 1000
                For j As Integer = 1 To data_count
                    sumarm = sumarm + longarm(j) + shortarm(j)
                    If shortarm(j) > 0.1 Then
                        summod = summod + longarm(j) / shortarm(j)
                    End If
                Next
                sumarm = sumarm - 1000
                summod = summod - 1000
            End If

            Label1.Text = "->"
            Label2.Text = ""
            Picture.Refresh()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListView1.SelectedIndices.Count > 0 Then
            TextBox3.Text = TextBox1.Text
            save_point_group_2(ListView1.SelectedIndices(0) + 1, TextBox3.Text)

            If TextBox2.Text <> "" Then
                longarm(ListView1.SelectedIndices(0) + 1) = TextBox2.Text
            End If
            If TextBox3.Text <> "" Then
                shortarm(ListView1.SelectedIndices(0) + 1) = TextBox3.Text
            End If
            If TextBox2.Text <> "" And TextBox3.Text <> "" Then
                ChangeArm(ListView1.SelectedIndices(0) + 1)
                ListView1.SelectedItems(0).SubItems(2).Text = shortarm(ListView1.SelectedIndices(0) + 1)
                ListView1.SelectedItems(0).SubItems(1).Text = longarm(ListView1.SelectedIndices(0) + 1)
                sumarm = 1000
                summod = 1000
                For j As Integer = 1 To data_count
                    sumarm = sumarm + longarm(j) + shortarm(j)
                    If shortarm(j) > 0.1 Then
                        summod = summod + longarm(j) / shortarm(j)
                    End If
                Next
                sumarm = sumarm - 1000
                summod = summod - 1000
            End If
            Label1.Text = ""
            Label2.Text = "->"
            Me.Refresh()
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked And ListView1.SelectedIndices.Count > 0 Then
            RadioButton1.Checked = False
            'RadioButton2.Checked = False
            RadioButton3.Checked = False
            RadioButton4.Checked = False
            suiarm(ListView1.SelectedIndices(0) + 1) = 1

            ListView1.SelectedItems(0).SubItems(3).Text = 1
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked And ListView1.SelectedIndices.Count > 0 Then
            'RadioButton1.Checked = False
            RadioButton2.Checked = False
            RadioButton3.Checked = False
            RadioButton4.Checked = False
            suiarm(ListView1.SelectedIndices(0) + 1) = 0

            ListView1.SelectedItems(0).SubItems(3).Text = 0
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked And ListView1.SelectedIndices.Count > 0 Then
            RadioButton1.Checked = False
            RadioButton2.Checked = False
            'RadioButton3.Checked = False
            RadioButton4.Checked = False
            suiarm(ListView1.SelectedIndices(0) + 1) = 2
            ListView1.SelectedItems(0).SubItems(3).Text = 2
        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked And ListView1.SelectedIndices.Count > 0 Then
            RadioButton1.Checked = False
            RadioButton2.Checked = False
            RadioButton3.Checked = False
            ListView1.SelectedItems(0).SubItems(3).Text = 3
            suiarm(ListView1.SelectedIndices(0) + 1) = 3
        End If
    End Sub

    Private Sub Picture_Click(sender As Object, e As EventArgs) Handles Picture.Click

    End Sub


    Private Sub Form_main_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

    End Sub

    Private Sub Picture_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Picture.MouseDoubleClick
        If DisableDoubleClickToolStripMenuItem.Checked = False Then
            If e.Button = MouseButtons.Left Then
                Select Case last_type
                    Case 0
                        meansure_lines()
                    Case 1
                        meansure_size()
                    Case Else
                        operate = "null"
                End Select
            End If
            If e.Button = MouseButtons.Right Then
                meansure_new()
            End If
        End If

    End Sub

    Private Sub KaryotypeFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KaryotypeFileToolStripMenuItem.Click
        Try
            GC.Collect()
            If longarm(1) <> -1 And shortarm(1) <> -1 Then
                Dim CloseDialog As DialogResult
                CloseDialog = MessageBox.Show("Open a new file will clean the current data!", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                If CloseDialog = DialogResult.Yes Then

                    open_karyo()
                    sumarm = 1000
                    summod = 1000
                    For j As Integer = 1 To data_count
                        sumarm = sumarm + longarm(j) + shortarm(j)
                        If shortarm(j) > 0.1 Then
                            summod = summod + longarm(j) / shortarm(j)
                        End If

                    Next
                    sumarm = sumarm - 1000
                    summod = summod - 1000
                End If
            Else

                open_karyo()
                sumarm = 1000
                summod = 1000
                For j As Integer = 1 To data_count
                    sumarm = sumarm + longarm(j) + shortarm(j)
                    If shortarm(j) > 0.1 Then
                        summod = summod + longarm(j) / shortarm(j)
                    End If

                Next
                sumarm = sumarm - 1000
                summod = summod - 1000
            End If
            Picture.Refresh()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedIndices.Count > 0 Then
            If longarm(ListView1.SelectedIndices(0) + 1) <> -1 And shortarm(ListView1.SelectedIndices(0) + 1) <> -1 Then
                If points_group(ListView1.SelectedIndices(0) + 1).points_count_1 > 0 Then
                    TextBox2.Text = points_group(ListView1.SelectedIndices(0) + 1).points_value_1
                Else
                    TextBox2.Text = longarm(ListView1.SelectedIndices(0) + 1)
                End If
                If points_group(ListView1.SelectedIndices(0) + 1).points_count_2 > 0 Then
                    TextBox3.Text = points_group(ListView1.SelectedIndices(0) + 1).points_value_2
                Else
                    TextBox3.Text = shortarm(ListView1.SelectedIndices(0) + 1)
                End If
                CheckBox1.Checked = bchrom_list(ListView1.SelectedIndices(0) + 1)
                If suiarm(ListView1.SelectedIndices(0) + 1) = 0 Then
                    RadioButton1.Checked = True
                    RadioButton2.Checked = False
                    RadioButton3.Checked = False
                    RadioButton4.Checked = False
                End If
                If suiarm(ListView1.SelectedIndices(0) + 1) = 1 Then
                    RadioButton1.Checked = False
                    RadioButton2.Checked = True
                    RadioButton3.Checked = False
                    RadioButton4.Checked = False
                End If
                If suiarm(ListView1.SelectedIndices(0) + 1) = 2 Then
                    RadioButton1.Checked = False
                    RadioButton2.Checked = False
                    RadioButton3.Checked = True
                    RadioButton4.Checked = False
                End If
                If suiarm(ListView1.SelectedIndices(0) + 1) = 3 Then
                    RadioButton1.Checked = False
                    RadioButton2.Checked = False
                    RadioButton3.Checked = False
                    RadioButton4.Checked = True
                End If
            Else
                TextBox2.Text = ""
                TextBox3.Text = ""
                If longarm(ListView1.SelectedIndices(0) + 1) <> -1 Then
                    TextBox2.Text = longarm(ListView1.SelectedIndices(0) + 1)
                End If
                If shortarm(ListView1.SelectedIndices(0) + 1) <> -1 Then
                    TextBox3.Text = shortarm(ListView1.SelectedIndices(0) + 1)
                End If
            End If
            Picture.Refresh()
        Else
            TextBox2.Text = ""
            TextBox3.Text = ""
            RadioButton1.Checked = False
            RadioButton2.Checked = False
            RadioButton3.Checked = False
            RadioButton4.Checked = False
            Me.Refresh()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If ListView1.SelectedIndices.Count > 0 Then
            bchrom_list(ListView1.SelectedIndices(0) + 1) = CheckBox1.Checked
            If CheckBox1.Checked Then
                ListView1.SelectedItems(0).SubItems(4).Text = "T"
            Else
                ListView1.SelectedItems(0).SubItems(4).Text = "F"
            End If

        End If
    End Sub

    Private Sub NewNToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewNToolStripMenuItem.Click
        Dim CloseDialog As DialogResult
        CloseDialog = MessageBox.Show("Clear the current meansurement?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
        If CloseDialog = DialogResult.Yes Then
            clear_result()
            clear_view()
        End If
    End Sub

    Private Sub SaveFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveFileToolStripMenuItem.Click
        Dim temp_data_count As Integer = data_count
        If longarm(data_count) = -1 Or shortarm(data_count) = -1 Then
            data_count -= 1
        End If
        If data_count > 0 Then
            Dim sfd As New SaveFileDialog
            sfd.Filter = "Karyo Files(*.karyo)|*.karyo;*.nuc;*.NUC|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
            sfd.FileName = ""
            sfd.DefaultExt = ".karyo"
            sfd.CheckPathExists = True
            Dim resultdialog As DialogResult = sfd.ShowDialog()
            If resultdialog = DialogResult.OK Then
                write_karyo(sfd.FileName)
            End If
        Else
            MsgBox("You should have one chromosomes at least!")
        End If
        data_count = temp_data_count
    End Sub

    Private Sub SaveResultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveResultsToolStripMenuItem.Click
        Select Case mode_type
            Case 0
                If data_count > 0 Then
                    Dim sfd As New SaveFileDialog
                    sfd.Filter = "csv file(*.csv;*.CSV)|*.csv;*.CSV|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
                    sfd.FileName = ""
                    sfd.DefaultExt = ".csv"
                    sfd.CheckPathExists = True
                    Dim resultdialog As DialogResult = sfd.ShowDialog()
                    If resultdialog = DialogResult.OK Then
                        save_csv(sfd.FileName, ListView2)
                    End If
                End If
            Case 1
                If data_count > 1 Then
                    Dim sfd As New SaveFileDialog
                    sfd.Filter = "Excel File(*.xls)|*.xls;*.XlS|CSV File(*.csv)|*.CSV;*.csv|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
                    sfd.FileName = ""
                    sfd.DefaultExt = ".xls"
                    sfd.CheckPathExists = True
                    Dim resultdialog As DialogResult = sfd.ShowDialog()
                    If resultdialog = DialogResult.OK Then
                        info()
                        Analysisform.RichTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText)
                    End If
                Else
                    MsgBox("You should have two chromosomes at least!")
                End If
        End Select

    End Sub

    Private Sub SavePhotosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SavePhotosToolStripMenuItem.Click
        Try
            If image_path <> "" Then


                Dim sfd As New SaveFileDialog
                sfd.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpg)|*.jpg|Bitmap Files (*.bmp)|*.bmp|ALL Files (*.*)|*.*"
                sfd.FileName = ""
                sfd.DefaultExt = ".png"
                sfd.CheckPathExists = True

                Dim resultdialog As DialogResult = sfd.ShowDialog()
                If resultdialog = DialogResult.OK Then
                    Dim my_bitmap As New Bitmap(Picture.Image) ' 创建一个基于Picture控件的Bitmap对象
                    Dim temp_zoom As Single = zoom_radio
                    zoom_radio = 1 ' 重置缩放比例
                    TempGrap = Graphics.FromImage(my_bitmap)

                    ' 根据mode_type调用不同的绘制方法
                    Select Case mode_type
                        Case 0
                            draw_lines(TempGrap, 1 / temp_zoom, ListView2)
                        Case 1
                            draw_lines(TempGrap, 1 / temp_zoom, ListView1)
                    End Select

                    ' 获取用户选择的文件扩展名
                    Dim fileExtension As String = System.IO.Path.GetExtension(sfd.FileName).ToLower()

                    ' 根据文件扩展名选择正确的图像格式进行保存
                    Select Case fileExtension
                        Case ".png"
                            my_bitmap.Save(sfd.FileName, Imaging.ImageFormat.Png)
                        Case ".jpg", ".jpeg"
                            my_bitmap.Save(sfd.FileName, Imaging.ImageFormat.Jpeg)
                        Case ".bmp"
                            my_bitmap.Save(sfd.FileName, Imaging.ImageFormat.Bmp)
                        Case Else
                            ' 如果扩展名未知，则默认保存为PNG
                            my_bitmap.Save(sfd.FileName, Imaging.ImageFormat.Png)
                    End Select

                    ' 还原缩放比例
                    zoom_radio = temp_zoom
                    my_bitmap.Dispose()
                    Picture.Refresh()
                End If
            Else
                MsgBox("Could not save the picture!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub GroupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GroupToolStripMenuItem.Click
        Analysisform.Show()
    End Sub

    Private Sub ScalesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScalesToolStripMenuItem.Click
        ScaleForm.Show()
    End Sub

    Private Sub CombineResultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CombineResultsToolStripMenuItem.Click
        Dim Combineform As New Form_Combine
        Combineform.Show()
    End Sub

    Private Sub AboutMATTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutMATTToolStripMenuItem.Click
        Aboutform.Show()
    End Sub

    Private Sub Form_main_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        Picture.Refresh
    End Sub

    Private Sub Picture_MouseClick(sender As Object, e As MouseEventArgs) Handles Picture.MouseClick
        Select Case operate
            Case "color"
                If e.Button = MouseButtons.Right Then
                    Picture.Cursor = Cursors.Arrow
                    in_meansure = False
                    last_count = node_count
                    TS_Lines.Checked = False
                    TS_Size.Checked = False
                    TS_Color.Checked = False
                    TS_Count.Checked = False
                    TS_Angle.Checked = False
                    operate = "null"
                End If
                If e.Button = MouseButtons.Middle Then
                    If node_count >= 0 Then
                        node_count -= 1
                    End If
                End If
                If e.Button = MouseButtons.Left Then
                    in_meansure = True
                    Picture.Cursor = Cursors.Cross
                    '先赋值再加1避免闪烁
                    If node_count < 2 Then
                        line_point(node_count + 1) = New Point(e.X, e.Y)
                        node_count += 1
                    End If
                    If node_count = 1 Then
                        If current_bmp IsNot Nothing Then

                            Dim r As Integer = 0
                            Dim g As Integer = 0
                            Dim b As Integer = 0
                            Dim count As Integer = 0

                            For x As Integer = Math.Min(line_point(0).X, line_point(1).X) To Math.Max(line_point(0).X, line_point(1).X)
                                For y As Integer = Math.Min(line_point(0).Y, line_point(1).Y) To Math.Max(line_point(0).Y, line_point(1).Y)
                                    Dim temp_color As Color = current_bmp.GetPixel(x / zoom_radio, y / zoom_radio)
                                    r += temp_color.R
                                    g += temp_color.G
                                    b += temp_color.B
                                    count += 1
                                Next
                            Next
                            r = r / count
                            g = g / count
                            b = b / count
                            Picture_color.BackColor = Color.FromArgb(255, r, g, b)
                            Picture_color.Refresh()

                            TextBox5.Text = Picture_color.BackColor.R
                            TextBox6.Text = Picture_color.BackColor.G
                            TextBox7.Text = Picture_color.BackColor.B
                            '简化 sRGB IEC61966-2.1 [gamma=2.20]
                            'Gray = (R ^ 2.2 * 0.2126 + G ^ 2.2 * 0.7152 + B ^ 2.2 * 0.0722) ^ (1 / 2.2)
                            TextBox8.Text = CInt((CInt(Picture_color.BackColor.R) ^ 2.2 * 0.2126 + CInt(Picture_color.BackColor.G) ^ 2.2 * 0.7152 + CInt(Picture_color.BackColor.B) ^ 2.2 * 0.0722) ^ (1 / 2.2))
                            If RedToolStripMenuItem.Checked Then
                                TextBox4.Text = TextBox5.Text
                            End If
                            If GreenToolStripMenuItem.Checked Then
                                TextBox4.Text = TextBox6.Text
                            End If
                            If BlueToolStripMenuItem.Checked Then
                                TextBox4.Text = TextBox7.Text
                            End If
                            If GrayToolStripMenuItem.Checked Then
                                TextBox4.Text = TextBox8.Text
                            End If
                            TextBox9.Text = "#" + DEC_to_HEX(Picture_color.BackColor.R) + DEC_to_HEX(Picture_color.BackColor.G) + DEC_to_HEX(Picture_color.BackColor.B)
                            If CODEToolStripMenuItem.Checked Then
                                TextBox4.Text = TextBox9.Text
                            End If
                            If RGBToolStripMenuItem.Checked Then
                                TextBox4.Text = TextBox5.Text + "|" + TextBox6.Text + "|" + TextBox7.Text
                            End If
                        End If
                    End If


                End If
            Case "angle"
                If e.Button = MouseButtons.Right Then
                    Picture.Cursor = Cursors.Arrow
                    in_meansure = False
                    last_count = node_count
                    TS_Color.Checked = False
                    TS_Lines.Checked = False
                    TS_Size.Checked = False
                    TS_Count.Checked = False
                    TS_Angle.Checked = False
                    operate = "null"
                End If
                If e.Button = MouseButtons.Middle Then
                    If node_count >= 0 Then
                        node_count -= 1
                    End If
                End If
                If e.Button = MouseButtons.Left Then
                    in_meansure = True
                    Picture.Cursor = Cursors.Cross
                    '先赋值再加1避免闪烁
                    If node_count < 3 Then
                        line_point(node_count + 1) = New Point(e.X, e.Y)
                        node_count += 1
                    End If
                    If node_count = 3 Then
                        Dim angle1 As Single = Math.Atan2((line_point(0).Y - line_point(1).Y), (line_point(0).X - line_point(1).X))
                        Dim angle2 As Single = Math.Atan2((line_point(2).Y - line_point(3).Y), (line_point(2).X - line_point(3).X))
                        Select Case mode_type
                            Case 0
                                TextBox4.Text = CSng((Abs(angle1 - angle2) / Math.PI * 180)).ToString("F2")
                            Case 1
                                'TextBox2.Text = CInt((Abs(angle1 - angle2) / Math.PI * 180) Mod 180)
                                'TextBox3.Text = 180 - CInt((Abs(angle1 - angle2) / Math.PI * 180) Mod 180)
                        End Select
                    End If
                End If
            Case "count"
                If e.Button = MouseButtons.Right Then
                    Picture.Cursor = Cursors.Arrow
                    in_meansure = False
                    last_count = node_count
                    TS_Color.Checked = False
                    TS_Lines.Checked = False
                    TS_Size.Checked = False
                    TS_Count.Checked = False
                    TS_Angle.Checked = False
                    operate = "null"
                End If
                If e.Button = MouseButtons.Middle Then
                    If node_count >= 0 Then
                        node_count -= 1
                    End If
                End If
                If e.Button = MouseButtons.Left Then
                    in_meansure = True
                    Picture.Cursor = Cursors.Cross
                    '先赋值再加1避免闪烁
                    line_point(node_count + 1) = New Point(e.X, e.Y)
                    node_count += 1
                    Select Case mode_type
                        Case 0
                            TextBox4.Text = node_count + 1
                        Case 1

                    End Select
                End If
            Case "lines"
                If e.Button = MouseButtons.Right Then
                    Picture.Cursor = Cursors.Arrow
                    in_meansure = False
                    last_count = node_count
                    TS_Color.Checked = False
                    TS_Lines.Checked = False
                    TS_Size.Checked = False
                    TS_Count.Checked = False
                    TS_Angle.Checked = False
                    operate = "null"
                End If
                If e.Button = MouseButtons.Middle Then
                    If node_count >= 0 Then
                        node_count -= 1
                    End If
                End If
                If e.Button = MouseButtons.Left Then
                    in_meansure = True
                    Picture.Cursor = Cursors.Cross
                    '先赋值再加1避免闪烁
                    line_point(node_count + 1) = New Point(e.X, e.Y)
                    node_count += 1

                    MDown1X = line_point(node_count).X / zoom_radio
                    MDown1Y = line_point(node_count).Y / zoom_radio
                    If armlongth = 0 And MDown2Y = 0 And MDown2X = 0 Then
                        MDown2Y = MDown1Y
                        MDown2X = MDown1X
                    End If
                    armlongth = armlongth + ((MDown1X - MDown2X) ^ 2 + (MDown1Y - MDown2Y) ^ 2) ^ 0.5
                    MDown2Y = MDown1Y
                    MDown2X = MDown1X
                    Select Case mode_type
                        Case 0
                            TextBox4.Text = armlongth * current_scale
                        Case 1
                            TextBox1.Text = armlongth * current_scale
                    End Select
                End If
            Case "size"
                If e.Button = MouseButtons.Right Then
                    Picture.Cursor = Cursors.Arrow
                    in_meansure = False
                    last_count = node_count
                    TS_Color.Checked = False
                    TS_Lines.Checked = False
                    TS_Size.Checked = False
                    TS_Count.Checked = False
                    TS_Angle.Checked = False
                    operate = "null"
                End If
                If e.Button = MouseButtons.Middle Then
                    If node_count >= 0 Then
                        node_count -= 1
                    End If
                End If
                If e.Button = MouseButtons.Left Then
                    in_meansure = True
                    Picture.Cursor = Cursors.Cross
                    '检测是否相交
                    Dim test_cross As Boolean = False
                    For i = 0 To node_count - 1
                        test_cross = test_cross Or is_cross(line_point(i), line_point(i + 1), line_point(node_count), New Point(e.X, e.Y))
                    Next
                    If test_cross = False Then
                        '先赋值再加1避免闪烁
                        line_point(node_count + 1) = New Point(e.X, e.Y)
                        node_count += 1
                        DrawSize = (line_point(0).X / zoom_radio).ToString + "," + (line_point(0).Y / zoom_radio).ToString
                        For i As Integer = 1 To node_count
                            DrawSize = DrawSize + ";" + (line_point(i).X / zoom_radio).ToString + "," + (line_point(i).Y / zoom_radio).ToString
                        Next
                        Select Case mode_type
                            Case 0
                                TextBox4.Text = gu_GetArea(DrawSize) * current_scale * current_scale
                            Case 1
                                TextBox1.Text = gu_GetArea(DrawSize) * current_scale * current_scale
                        End Select
                    End If
                End If
            Case Else
                bPictureBoxDragging = False
                Picture.Cursor = Cursors.Default
        End Select
        If e.Button = MouseButtons.Right Then
            Me.ToolStripStatusLabel1.Text = ""
        End If
        Picture.Refresh()
    End Sub


    Private Sub FontColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FontColorToolStripMenuItem.Click
        ColorDialog1.ShowDialog()
        If DialogResult.OK Then
            brush_font.Color = Color.FromArgb(brush_font.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
        End If
        Picture.Refresh()
    End Sub

    Private Sub LineColorToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles LineColorToolStripMenuItem1.Click
        ColorDialog1.ShowDialog()
        If DialogResult.OK Then
            pen_line.Color = Color.FromArgb(pen_line.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
        End If
        Picture.Refresh()
    End Sub

    Private Sub LineColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineColorToolStripMenuItem.Click
        ColorDialog1.ShowDialog()
        If DialogResult.OK Then
            pen_arm_1.Color = Color.FromArgb(pen_arm_1.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
            brush_arm_1.Color = Color.FromArgb(brush_arm_1.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
        End If
        Picture.Refresh()
    End Sub

    Private Sub ShortArmColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShortArmColorToolStripMenuItem.Click
        ColorDialog1.ShowDialog()
        If DialogResult.OK Then
            pen_arm_2.Color = Color.FromArgb(pen_arm_2.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
            brush_arm_2.Color = Color.FromArgb(brush_arm_2.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
        End If
        Picture.Refresh()
    End Sub

    Private Sub PointColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PointColorToolStripMenuItem.Click
        ColorDialog1.ShowDialog()
        If DialogResult.OK Then
            brush_circle.Color = Color.FromArgb(brush_circle.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
            pen_circle.Color = Color.FromArgb(pen_circle.Color.A, ColorDialog1.Color.R * 0.8, ColorDialog1.Color.G * 0.8, ColorDialog1.Color.B * 0.8)
        End If
        Picture.Refresh()
    End Sub

    Private Sub ScaleColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScaleColorToolStripMenuItem.Click
        If DialogResult.OK Then
            scale_pen.Color = Color.FromArgb(scale_pen.Color.A, ColorDialog1.Color.R, ColorDialog1.Color.G, ColorDialog1.Color.B)
            scale_brush.Color = Color.FromArgb(scale_brush.Color.A, ColorDialog1.Color.R * 0.8, ColorDialog1.Color.G * 0.8, ColorDialog1.Color.B * 0.8)
        End If
        Picture.Refresh()
    End Sub

    Private Sub KaryotypeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KaryotypeToolStripMenuItem.Click
        If KaryotypeToolStripMenuItem.Checked = False Then
            Dim CloseDialog As DialogResult
            CloseDialog = MessageBox.Show("Clear the current meansurement?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
            If CloseDialog = DialogResult.Yes Then
                GeneralToolStripMenuItem.Checked = False
                mode_type = 1
                TabPage1.Parent = Nothing
                TabPage2.Parent = TabControl1
                ListView1.Visible = True
                ListView2.Visible = False
                KaryotypeToolStripMenuItem.Checked = True
                TS_Group.Visible = True
                TS_Count.Visible = False
                TS_Color.Visible = False
                TS_Angle.Visible = False
                TS_Remove.Visible = False

                KaryotypeFileToolStripMenuItem.Visible = True
                ResultsToolStripMenuItem.Visible = False
                GroupToolStripMenuItem.Visible = True
                CombineResultsToolStripMenuItem.Visible = True
                ToolStripSeparator1.Visible = True
                SaveFileToolStripMenuItem.Visible = True

                clear_result()
                clear_view()
            End If
        End If
    End Sub


    Private Sub GeneralToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GeneralToolStripMenuItem.Click
        If GeneralToolStripMenuItem.Checked = False Then
            Dim CloseDialog As DialogResult
            CloseDialog = MessageBox.Show("Clear the current meansurement?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
            If CloseDialog = DialogResult.Yes Then
                GeneralToolStripMenuItem.Checked = True
                mode_type = 0
                TabPage2.Parent = Nothing
                TabPage1.Parent = TabControl1
                ListView2.Visible = True
                ListView1.Visible = False

                KaryotypeToolStripMenuItem.Checked = False
                TS_Group.Visible = False
                TS_Count.Visible = True
                TS_Color.Visible = True
                TS_Angle.Visible = True
                TS_Remove.Visible = True

                KaryotypeFileToolStripMenuItem.Visible = False
                ResultsToolStripMenuItem.Visible = True
                GroupToolStripMenuItem.Visible = False
                CombineResultsToolStripMenuItem.Visible = False
                ToolStripSeparator1.Visible = False
                SaveFileToolStripMenuItem.Visible = False

                clear_result()
                clear_view()
            End If
        End If
    End Sub
    Public Function type2name(ByVal type As Integer) As String
        Select Case type
            Case 0
                Return "lines"
            Case 1
                Return "size"
            Case 2
                Return "count"
            Case 3
                Return "angle"
            Case 4
                Return "color"
            Case Else
                Return CStr(type)
        End Select
    End Function
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListView2.SelectedIndices.Count > 0 Then
            If node_count > 0 Then
                Picture.Cursor = Cursors.Arrow
                in_meansure = False
                last_count = node_count
                TS_Lines.Checked = False
                TS_Size.Checked = False
                TS_Count.Checked = False
                TS_Angle.Checked = False
                operate = "null"
                If TextBox4.Text <> "" Then
                    save_point_group_1(ListView2.SelectedIndices(0) + 1, TextBox4.Text)
                    Dim new_line As String = ""
                    For j = 0 To points_group(ListView2.SelectedIndices(0) + 1).points_count_1
                        new_line += points_group(ListView2.SelectedIndices(0) + 1).points_group_1(j).X.ToString + "_" + points_group(ListView2.SelectedIndices(0) + 1).points_group_1(j).Y.ToString + ";"
                    Next
                    ListView2.SelectedItems(0).SubItems(1).Text = TextBox4.Text
                    ListView2.SelectedItems(0).SubItems(2).Text = type2name(last_type)
                    ListView2.SelectedItems(0).SubItems(3).Text = last_count + 1
                    ListView2.SelectedItems(0).SubItems(4).Text = new_line

                End If
                Picture.Refresh()
            Else
                MsgBox("Please redo the measurement.")
            End If


        Else
            MsgBox("You should select an ID to save the result!")
        End If
    End Sub

    Private Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        If ListView2.SelectedIndices.Count > 0 Then
            If ListView2.SelectedItems(0).SubItems(4).Text <> "" Then
                recover_point_group(ListView2.SelectedIndices(0) + 1)
            Else
                node_count = -1
            End If
        End If
    End Sub
    Public Function name2type(ByVal name As String) As Integer
        Select Case name
            Case "lines"
                Return 0
            Case "size"
                Return 1
            Case "count"
                Return 2
            Case "angle"
                Return 3
            Case "color"
                Return 4
            Case Else
                Return CInt(name)

        End Select
    End Function
    Public Sub recover_point_group(ByVal id As Integer)

        'For j = 0 To points_group(ListView2.SelectedIndices(0) + 1).points_count_1
        '    new_line += points_group(ListView2.SelectedIndices(0) + 1).points_group_1(j).X.ToString + "_" + points_group(ListView2.SelectedIndices(0) + 1).points_group_1(j).Y.ToString + ";"
        'Next
        TextBox4.Text = ListView2.SelectedItems(0).SubItems(1).Text
        points_group(id).points_value_1 = ListView2.SelectedItems(0).SubItems(1).Text
        last_type = name2type(ListView2.SelectedItems(0).SubItems(2).Text)
        points_group(id).points_type_1 = name2type(ListView2.SelectedItems(0).SubItems(2).Text)
        node_count = ListView2.SelectedItems(0).SubItems(3).Text - 1
        points_group(id).points_count_1 = node_count
        last_count = node_count
        Dim new_line() As String = ListView2.SelectedItems(0).SubItems(4).Text.Split(";")
        For j = 0 To UBound(new_line) - 1
            If new_line(j) <> "" Then
                points_group(ListView2.SelectedIndices(0) + 1).points_group_1(j) = New Point(CSng(new_line(j).Split("_")(0)), CSng(new_line(j).Split("_")(1)))
            End If
        Next
        For i As Integer = 0 To node_count
            line_point(i) = New Point(points_group(id).points_group_1(i).X / zoom_radio, points_group(id).points_group_1(i).Y / zoom_radio)
        Next
        has_record = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Select Case last_type
            Case 0
                meansure_lines(False)
            Case 1
                meansure_size(False)
            Case 2
                meansure_count(False)
            Case 3
                meansure_angle(False)
            Case 4
                meansure_color(False)
            Case Else
                MsgBox(":P")
        End Select
        in_meansure = True
    End Sub

    Private Sub ResultsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResultsToolStripMenuItem.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "csv file(*.csv;*.CSV)|*.csv;*.CSV|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
        opendialog.FileName = ""
        opendialog.DefaultExt = ".csv"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        zoom_radio = 1
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            clear_result()
            read_csv(opendialog.FileName, ListView2)
            If image_path <> "" Then
                Dim CloseDialog As DialogResult
                CloseDialog = MessageBox.Show("Do you want to open the related image?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                If CloseDialog = DialogResult.Yes Then
                    find_image(opendialog.FileName)
                End If
            End If
            Picture.Refresh()
        End If
    End Sub

    Private Sub NodeSizeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NodeSizeToolStripMenuItem.Click
        point_size = InputBox("Enter the ploidy level: ", "ploidy", point_size)
        Picture.Refresh()
    End Sub

    Private Sub NodeIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NodeIDToolStripMenuItem.Click
        Picture.Refresh()
    End Sub

    Private Sub LineLengthToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineLengthToolStripMenuItem.Click
        Picture.Refresh()
    End Sub

    Private Sub GroupToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles GroupToolStripMenuItem1.Click
        Picture.Refresh()
    End Sub
    Public Sub meansure_color(Optional clean As Boolean = True)
        If TS_Color.Checked = False Then
            TS_Lines.Checked = False
            TS_Size.Checked = False
            TS_Color.Checked = True
            TS_In.Checked = False
            TS_Out.Checked = False
            TS_Angle.Checked = False
            TS_Count.Checked = False
            bPictureBoxDragging = False
            last_type = 4
            If clean Then
                Init()
            End If

            Picture.Cursor = Cursors.Cross
            operate = "color"

            ListView1.Text = "Color"

            has_record = False
            Me.ToolStripStatusLabel1.Text = "Tracing color"
        Else
            TS_Color.Checked = False
            If clean Then
                Init()
            End If
            Picture.Cursor = Cursors.Arrow
            operate = "null"
            Me.ToolStripStatusLabel1.Text = ""
        End If
    End Sub
    Private Sub TS_Color_Click(sender As Object, e As EventArgs) Handles TS_Color.Click
        meansure_color()
    End Sub

    Private Sub DeleteIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteIDToolStripMenuItem.Click

        Select Case mode_type
            Case 0
                If ListView2.SelectedIndices.Count > 0 Then
                    Dim delDialog As DialogResult
                    delDialog = MessageBox.Show("Delete selected item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                    If delDialog = DialogResult.Yes Then
                        If data_count > 1 Then
                            For i As Integer = ListView2.SelectedIndices(0) + 1 To data_count - 1
                                points_group(i) = New my_points(points_group(i + 1).points_group_1, points_group(i + 1).points_type_1, points_group(i + 1).points_count_1, points_group(i + 1).points_value_1,
    points_group(i + 1).points_group_2, points_group(i + 1).points_type_2, points_group(i + 1).points_count_2, points_group(i + 1).points_value_2)
                                ListView2.Items(i).SubItems(0).Text = i.ToString
                            Next
                            points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)

                            his_point(data_count).X = 0
                            his_point(data_count).Y = 0

                            data_count = data_count - 1
                            Me.ToolStripStatusLabel1.Text = ""
                            Picture.Refresh()
                            ListView2.Items.RemoveAt(ListView2.SelectedIndices(0))
                            ListView2.Items(data_count - 1).Selected = True

                        ElseIf data_count = 1 Then
                            ListView2.Items.RemoveAt(data_count - 1)
                            points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)

                            his_point(data_count).X = 0
                            his_point(data_count).Y = 0
                            Me.ToolStripStatusLabel1.Text = ""
                            Picture.Refresh()
                            data_count -= 1
                        End If
                    End If
                End If
            Case 1
                If ListView1.SelectedIndices.Count > 0 Then
                    Dim delDialog As DialogResult
                    delDialog = MessageBox.Show("Delete selected chromosome?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                    If delDialog = DialogResult.Yes Then
                        If data_count > 1 Then
                            For i As Integer = ListView1.SelectedIndices(0) + 1 To data_count - 1
                                points_group(i) = New my_points(points_group(i + 1).points_group_1, points_group(i + 1).points_type_1, points_group(i + 1).points_count_1, points_group(i + 1).points_value_1,
    points_group(i + 1).points_group_2, points_group(i + 1).points_type_2, points_group(i + 1).points_count_2, points_group(i + 1).points_value_2)
                                ListView1.Items(i).SubItems(0).Text = i.ToString
                                bchrom_list(i) = bchrom_list(i + 1)
                                longarm(i) = longarm(i + 1)
                                shortarm(i) = shortarm(i + 1)
                                suiarm(i) = suiarm(i + 1)
                            Next
                            '清空数据
                            points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                            bchrom_list(data_count) = False
                            longarm(data_count) = -1
                            shortarm(data_count) = -1
                            suiarm(data_count) = 0

                            his_point(data_count).X = 0
                            his_point(data_count).Y = 0
                            data_count = data_count - 1
                            Me.ToolStripStatusLabel1.Text = ""
                            ListView1.Items.RemoveAt(ListView1.SelectedIndices(0))
                            ListView1.Items(data_count - 1).Selected = True
                            Picture.Refresh()

                        ElseIf data_count = 1 Then
                            ListView1.Items.RemoveAt(data_count - 1)
                            '清空数据
                            points_group(data_count) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                            bchrom_list(data_count) = False
                            longarm(data_count) = -1
                            shortarm(data_count) = -1
                            suiarm(data_count) = 0


                            his_point(data_count).X = 0
                            his_point(data_count).Y = 0
                            data_count = data_count - 1
                            Me.ToolStripStatusLabel1.Text = ""
                            Picture.Refresh()
                        End If
                    End If
                End If

        End Select
    End Sub

    Private Sub ClearSelectIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearSelectIDToolStripMenuItem.Click
        Select Case mode_type
            Case 0
                If ListView2.SelectedIndices.Count > 0 Then
                    Dim delDialog As DialogResult
                    delDialog = MessageBox.Show("Clear selected item?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                    If delDialog = DialogResult.Yes Then
                        points_group(ListView2.SelectedIndices(0) + 1) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                        his_point(ListView2.SelectedIndices(0) + 1).X = 0
                        his_point(ListView2.SelectedIndices(0) + 1).Y = 0
                        ListView2.Items(ListView2.SelectedIndices(0)).SubItems(1).Text = ""
                        ListView2.Items(ListView2.SelectedIndices(0)).SubItems(2).Text = ""
                        ListView2.Items(ListView2.SelectedIndices(0)).SubItems(3).Text = ""
                        ListView2.Items(ListView2.SelectedIndices(0)).SubItems(4).Text = ""
                        Me.ToolStripStatusLabel1.Text = ""
                        Picture.Refresh()
                    End If
                End If
            Case 1
                If ListView1.SelectedIndices.Count > 0 Then
                    Dim delDialog As DialogResult
                    delDialog = MessageBox.Show("Clear selected item?", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk)
                    If delDialog = DialogResult.Yes Then
                        points_group(ListView1.SelectedIndices(0) + 1) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                        bchrom_list(ListView1.SelectedIndices(0) + 1) = False
                        longarm(ListView1.SelectedIndices(0) + 1) = -1
                        shortarm(ListView1.SelectedIndices(0) + 1) = -1
                        suiarm(ListView1.SelectedIndices(0) + 1) = 0
                        ListView1.Items(ListView1.SelectedIndices(0)).SubItems(1).Text = ""
                        ListView1.Items(ListView1.SelectedIndices(0)).SubItems(2).Text = ""
                        ListView1.Items(ListView1.SelectedIndices(0)).SubItems(3).Text = "0"
                        ListView1.Items(ListView1.SelectedIndices(0)).SubItems(4).Text = "F"

                        his_point(ListView1.SelectedIndices(0) + 1).X = 0
                        his_point(ListView1.SelectedIndices(0) + 1).Y = 0
                        Me.ToolStripStatusLabel1.Text = ""
                        Picture.Refresh()
                    End If
                End If

        End Select
    End Sub

    Private Sub InsertIDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertIDToolStripMenuItem.Click
        Select Case mode_type
            Case 0
                If ListView2.SelectedIndices.Count > 0 Then

                    If data_count >= 1 Then
                        Dim temp_selected As Integer = ListView2.SelectedIndices(0)
                        For i As Integer = temp_selected + 1 To data_count
                            Dim j As Integer = data_count + temp_selected + 2 - i
                            points_group(j) = New my_points(points_group(j - 1).points_group_1, points_group(j - 1).points_type_1, points_group(j - 1).points_count_1, points_group(j - 1).points_value_1,
points_group(j - 1).points_group_2, points_group(j - 1).points_type_2, points_group(j - 1).points_count_2, points_group(j - 1).points_value_2)
                            ListView2.Items(i - 1).SubItems(0).Text = (i + 1).ToString
                        Next
                        points_group(temp_selected + 1) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                        his_point(temp_selected + 1).X = 0
                        his_point(temp_selected + 1).Y = 0
                        ListView2.Items.Insert(temp_selected, New ListViewItem({temp_selected + 1, "", "", "", ""}))
                        data_count = data_count + 1
                        Me.ToolStripStatusLabel1.Text = ""

                        ListView2.Items(temp_selected).Selected = True
                        Picture.Refresh()
                    End If
                End If
            Case 1
                If ListView1.SelectedIndices.Count > 0 Then
                    If data_count >= 1 Then
                        Dim temp_selected As Integer = ListView1.SelectedIndices(0)
                        For i As Integer = temp_selected + 1 To data_count
                            Dim j As Integer = data_count + temp_selected + 2 - i
                            points_group(j) = New my_points(points_group(j - 1).points_group_1, points_group(j - 1).points_type_1, points_group(j - 1).points_count_1, points_group(j - 1).points_value_1,
points_group(j - 1).points_group_2, points_group(j - 1).points_type_2, points_group(j - 1).points_count_2, points_group(j - 1).points_value_2)
                            bchrom_list(j) = bchrom_list(i + 1)
                            longarm(j) = longarm(j - 1)
                            shortarm(j) = shortarm(j - 1)
                            suiarm(j) = suiarm(j - 1)
                            ListView1.Items(i - 1).SubItems(0).Text = (i + 1).ToString
                        Next
                        points_group(temp_selected + 1) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
                        bchrom_list(temp_selected + 1) = False
                        longarm(temp_selected + 1) = -1
                        shortarm(temp_selected + 1) = -1
                        suiarm(temp_selected + 1) = 0

                        his_point(temp_selected + 1).X = 0
                        his_point(temp_selected + 1).Y = 0

                        ListView1.Items.Insert(temp_selected, New ListViewItem({temp_selected + 1, "", "", "0", "F"}))
                        data_count = data_count + 1
                        Me.ToolStripStatusLabel1.Text = ""
                        ListView1.Items(temp_selected).Selected = True
                        Picture.Refresh()
                    End If
                End If

        End Select
    End Sub

    Private Sub WhiteBalanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WhiteBalanceToolStripMenuItem.Click
        'Dim temp_bmp As Bitmap = New Bitmap(Picture.Image)
        'gray_bmp(temp_bmp)
        'Dim my_image As Image = Picture.Image.Clone
        If TextBox5.Text <> "" Then
            Picture.Image = ColorWhiteBal(current_bmp)
        Else
            MsgBox("Please select a color first!")
        End If
    End Sub
    Public Sub gray_bmp(ByRef MyBitmap As Bitmap)
        Dim t, tt As Integer
        Dim b As Integer, c As Color
        With MyBitmap
            For t = 0 To .Width - 1
                For tt = 0 To .Height - 1
                    c = .GetPixel(t, tt)
                    b = (c.R ^ 2.2 * 0.2126 + c.G ^ 2.2 * 0.7152 + c.B ^ 2.2 * 0.0722) ^ (1 / 2.2)
                    .SetPixel(t, tt, Color.FromArgb(b, b, b))
                Next
            Next
        End With
    End Sub
    Public Function ColorWhiteBal(ByRef img As Image) As Image
        '白平衡调整
        Dim g As Graphics = Graphics.FromImage(img)
        Dim ia As New ImageAttributes()
        Dim rate_r As Single = 255 / CSng(TextBox5.Text)
        Dim rate_g As Single = 255 / CSng(TextBox6.Text)
        Dim rate_b As Single = 255 / CSng(TextBox7.Text)

        Dim colorMatrix As Single()() = {
            New Single() {rate_r, 0.0, 0.0, 0.0, 0.0},
            New Single() {0.0, rate_g, 0.0, 0.0, 0.0},
            New Single() {0.0, 0.0, rate_b, 0.0, 0.0},
            New Single() {0.0, 0.0, 0.0, 1.0, 0.0},
            New Single() {0.0, 0.0, 0.0, 0.0, 1.0}
        }
        Dim cm As New ColorMatrix(colorMatrix)

        ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Default)
        g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height,
        GraphicsUnit.Pixel, ia)
        g.Dispose()
        Return img
    End Function
    Public Function ColorTran(ByRef img As Image) As Image
        '图片切变
        Dim g As Graphics = Graphics.FromImage(img)
        Dim dpoints As Point() = {
            New Point(0, 100),
            New Point(100, 100),
            New Point(0, 0)
            }
        g.DrawImage(img, dpoints)
        g.Dispose()
        Return img
    End Function
    Public Function PerspectiveTran(ByRef img As Image, ByVal AffinePoints0 As PointF(), ByVal AffinePoints1 As PointF()) As Image
        '图片变形
        Dim dst_perspective As Mat = CvInvoke.GetPerspectiveTransform(AffinePoints0, AffinePoints1)

        Dim four_points As PointF() = {New PointF(0, 0), New PointF(0, img.Height), New PointF(img.Width, img.Height), New PointF(img.Width, 0)}
        Dim tran_points As PointF() = CvInvoke.PerspectiveTransform(four_points, dst_perspective)
        Dim min_w, max_w, min_h, max_h As Single
        min_w = 0
        max_w = img.Width
        min_h = 0
        max_h = img.Height
        For Each i As PointF In tran_points
            If i.X < min_w Then
                min_w = i.X
            End If
            If i.X > max_w Then
                max_w = i.X
            End If
            If i.Y < min_h Then
                min_h = i.Y
            End If
            If i.Y > max_h Then
                max_h = i.Y
            End If
        Next
        Dim tmp_scale As Single = 1
        If (max_w - min_w) > 8096 And (max_h - min_h) > (max_w - min_w) Then
            tmp_scale = 8096 / (max_w - min_w)
        End If
        If (max_h - min_h) > 8096 And (max_h - min_h) < (max_w - min_w) Then
            tmp_scale = 8096 / (max_h - min_h)
        End If
        min_w *= tmp_scale
        max_w *= tmp_scale
        min_h *= tmp_scale
        max_h *= tmp_scale
        For i As Integer = 0 To 3
            AffinePoints0(i).X = AffinePoints0(i).X - min_w
            AffinePoints0(i).Y = AffinePoints0(i).Y - min_h
            AffinePoints1(i).X = AffinePoints1(i).X - min_w
            AffinePoints1(i).Y = AffinePoints1(i).Y - min_h
        Next
        dst_perspective = CvInvoke.GetPerspectiveTransform(AffinePoints0, AffinePoints1)
        Dim imagesrc As Image(Of Bgr, Byte) = New Image(Of Bgr, Byte)(image_path)


        Dim imagedec As Image(Of Bgr, Byte) = New Image(Of Bgr, Byte)(Int(max_w - min_w), Int(max_h - min_h))

        For i As Integer = 0 To imagesrc.Rows - 1
            For j As Integer = 0 To imagesrc.Cols - 1
                Dim x As Integer = i - min_h
                Dim y As Integer = j - min_w
                If x >= 0 And y >= 0 And x < imagedec.Rows And y < imagedec.Cols Then
                    imagedec(x, y) = imagesrc(i, j)
                End If
            Next
        Next
        Dim dec_mat As Mat = New Mat
        CvInvoke.WarpPerspective(imagedec.Mat, dec_mat, dst_perspective, New Size(Int(max_w - min_w), Int(max_h - min_h)), CvEnum.Inter.Linear, CvEnum.Warp.Default, CvEnum.BorderType.Constant, New MCvScalar(0))

        img = dec_mat.ToImage(Of Bgr, Byte)().ToBitmap()
        imagesrc.Dispose()
        imagedec.Dispose()
        dec_mat.Dispose()
        Return img
    End Function
    'Public Function PerspectiveTran(ByRef img As Image, ByVal AffinePoints0 As PointF(), ByVal AffinePoints1 As PointF()) As Image
    '    '图片变形
    '    Dim dst_perspective As Mat = CvInvoke.GetPerspectiveTransform(AffinePoints0, AffinePoints1)

    '    Dim four_points As PointF() = {New PointF(0, 0), New PointF(0, img.Height), New PointF(img.Width, img.Height), New PointF(img.Width, 0)}
    '    Dim tran_points As PointF() = CvInvoke.PerspectiveTransform(four_points, dst_perspective)
    '    Dim min_w, max_w, min_h, max_h As Single
    '    min_w = tran_points(0).X
    '    max_w = tran_points(0).X
    '    min_h = tran_points(0).Y
    '    max_h = tran_points(0).Y
    '    For Each i As PointF In tran_points
    '        If i.X < min_w Then
    '            min_w = i.X
    '        End If
    '        If i.X > max_w Then
    '            max_w = i.X
    '        End If
    '        If i.Y < min_h Then
    '            min_h = i.Y
    '        End If
    '        If i.Y > max_h Then
    '            max_h = i.Y
    '        End If
    '    Next
    '    Dim tmp_scale As Single = 1
    '    If (max_w - min_w) > 8096 And (max_h - min_h) > (max_w - min_w) Then
    '        tmp_scale = 8096 / (max_w - min_w)
    '    End If
    '    If (max_h - min_h) > 8096 And (max_h - min_h) < (max_w - min_w) Then
    '        tmp_scale = 8096 / (max_h - min_h)
    '    End If
    '    min_w *= tmp_scale
    '    max_w *= tmp_scale
    '    min_h *= tmp_scale
    '    max_h *= tmp_scale
    '    Dim tmpbmp As Bitmap = New Bitmap(Int(max_w - min_w), Int(max_h - min_h), Imaging.PixelFormat.Format24bppRgb)
    '    Dim g As Graphics = Graphics.FromImage(tmpbmp)

    '    g.DrawImage(img, Int(-min_w), Int(-min_h), CInt(img.Width * tmp_scale), CInt(img.Height * tmp_scale))
    '    g.Dispose()
    '    For i As Integer = 0 To 3
    '        AffinePoints0(i).X = AffinePoints0(i).X - min_w
    '        AffinePoints0(i).Y = AffinePoints0(i).Y - min_h
    '        AffinePoints1(i).X = AffinePoints1(i).X - min_w
    '        AffinePoints1(i).Y = AffinePoints1(i).Y - min_h
    '    Next
    '    dst_perspective = CvInvoke.GetPerspectiveTransform(AffinePoints0, AffinePoints1)
    '    Dim imageCV As Image(Of Bgr, Byte) = New Image(Of Bgr, Byte)(tmpbmp)
    '    Dim org_mat As Mat = imageCV.Mat
    '    Dim dec_mat As Mat = New Mat
    '    CvInvoke.WarpPerspective(org_mat, dec_mat, dst_perspective, New Size(org_mat.Cols, org_mat.Rows), CvEnum.Inter.Linear, CvEnum.Warp.Default, CvEnum.BorderType.Constant, New MCvScalar(0))

    '    img = dec_mat.ToImage(Of Bgr, Byte)().ToBitmap()
    '    tmpbmp.Dispose()
    '    dec_mat.Dispose()
    '    Return img
    'End Function
    Public Function ColorMatrixGray(ByRef img As Image) As Image
        '获得灰度图像
        Dim g As Graphics = Graphics.FromImage(img)
        Dim ia As New ImageAttributes()
        Dim colorMatrix As Single()() = {
            New Single() {0.3, 0.3, 0.3, 0.0, 0.0},
            New Single() {0.59, 0.59, 0.59, 0.0, 0.0},
            New Single() {0.11, 0.11, 0.11, 0.0, 0.0},
            New Single() {0.0, 0.0, 0.0, 1.0, 0.0},
            New Single() {0.0, 0.0, 0.0, 0.0, 1.0}
        }
        Dim cm As New ColorMatrix(colorMatrix)

        ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Default)
        g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height,
        GraphicsUnit.Pixel, ia)
        g.Dispose()
        Return img
    End Function

    Private Sub GrayScaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GrayScaleToolStripMenuItem.Click
        Picture.Image = ColorMatrixGray(current_bmp)
    End Sub

    Private Sub ImageCorrectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImageCorrectToolStripMenuItem.Click

        If node_count = 3 Then
            Dim AffinePoints0(3) As PointF
            Dim AffinePoints1(3) As PointF

            Dim tmp_w As Integer = Abs(line_point(3).X - line_point(0).X) / zoom_radio
            Dim tmp_h As Integer = Abs(line_point(3).Y - line_point(0).Y) / zoom_radio
            For j As Integer = 0 To 3
                AffinePoints0(j) = New PointF(line_point(j).X / zoom_radio, line_point(j).Y / zoom_radio)
                If j > 1 Then
                    If Abs(line_point(j).X - line_point(j - 1).X) > tmp_w Then
                        tmp_w = Abs(line_point(j).X / zoom_radio - line_point(j - 1).X / zoom_radio)
                    End If
                    If Abs(line_point(j).Y - line_point(j - 1).Y) > tmp_h Then
                        tmp_h = Abs(line_point(j).Y / zoom_radio - line_point(j - 1).Y / zoom_radio)
                    End If
                End If
            Next

            Dim b1 As Single = (AffinePoints0(0).Y - AffinePoints0(2).Y) / (AffinePoints0(0).X - AffinePoints0(2).X)
            Dim c1 As Single = AffinePoints0(0).Y - AffinePoints0(0).X * b1

            Dim b2 As Single = (AffinePoints0(1).Y - AffinePoints0(3).Y) / (AffinePoints0(1).X - AffinePoints0(3).X)
            Dim c2 As Single = AffinePoints0(1).Y - AffinePoints0(1).X * b2

            Dim tmp_x As Single = (c2 - c1) / (b1 - b2)
            Dim tmp_y As Single = tmp_x * b1 + c1
            Try
                tmp_w = CInt(InputBox("Enter the Width: ", "Width", tmp_w))
                tmp_h = CInt(InputBox("Enter the Height: ", "Height", tmp_h))
            Catch ex As Exception
                Exit Sub
            End Try


            Dim left_top As Integer = 0
            For i As Integer = 0 To 3
                If AffinePoints0(i).X < tmp_x And AffinePoints0(i).Y < tmp_y Then
                    AffinePoints1(i) = New PointF(AffinePoints0(i).X, AffinePoints0(i).Y)
                    left_top = i
                End If
            Next
            For i As Integer = 0 To 3
                If AffinePoints0(i).X > tmp_x And AffinePoints0(i).Y < tmp_y Then
                    AffinePoints1(i) = New PointF(AffinePoints1(left_top).X + tmp_w, AffinePoints1(left_top).Y)
                End If
                If AffinePoints0(i).X > tmp_x And AffinePoints0(i).Y > tmp_y Then
                    AffinePoints1(i) = New PointF(AffinePoints1(left_top).X + tmp_w, AffinePoints1(left_top).Y + tmp_h)
                End If
                If AffinePoints0(i).X < tmp_x And AffinePoints0(i).Y > tmp_y Then
                    AffinePoints1(i) = New PointF(AffinePoints1(left_top).X, AffinePoints1(left_top).Y + tmp_h)
                End If
            Next

            node_count = 0
            Init()
            Try
                Picture.Image = PerspectiveTran(current_bmp, AffinePoints0, AffinePoints1)
                Picture.Size = New Size(current_bmp.Width * zoom_radio, current_bmp.Height * zoom_radio)
            Catch ex As Exception
                MsgBox("Out of memory. Please reduce the size of the image.")
            End Try

        Else
            MsgBox("You need four points to get perspective transform. Use SIZE tool to make a quadrilateral.")
        End If

    End Sub

    'Private Sub GeographicLayersToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GeographicLayersToolStripMenuItem.Click
    '    GeoForm.Show()
    'End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        current_bmp.RotateFlip(RotateFlipType.Rotate90FlipNone)
        Picture.Size = New Size(Picture.Height, Picture.Width)
        Picture.Image = current_bmp
    End Sub

    Private Sub DegreesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DegreesToolStripMenuItem.Click
        current_bmp.RotateFlip(RotateFlipType.Rotate270FlipNone)
        Picture.Size = New Size(Picture.Height, Picture.Width)
        Picture.Image = current_bmp
    End Sub

    Private Sub FlipHorizontallyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FlipHorizontallyToolStripMenuItem.Click
        current_bmp.RotateFlip(RotateFlipType.RotateNoneFlipY)
        Picture.Image = current_bmp
    End Sub

    Private Sub VerticalFlipToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerticalFlipToolStripMenuItem.Click
        current_bmp.RotateFlip(RotateFlipType.RotateNoneFlipX)
        Picture.Image = current_bmp
    End Sub

    Private Function Rotate0_90(ByRef img As Image, ByVal degree As Integer) As Image
        Dim ImgTarget As Bitmap
        Dim alpha As Double = (degree / 180) * PI

        Dim iWidth As Integer = img.Width * Math.Cos(alpha) + img.Height * Math.Sin(alpha)
        Dim iHeight As Integer = img.Width * Math.Sin(alpha) + img.Height * Math.Cos(alpha)

        ImgTarget = New Bitmap(iWidth, iHeight, Drawing.Imaging.PixelFormat.Format24bppRgb)
        Dim g As Graphics
        g = Graphics.FromImage(ImgTarget)

        g.TranslateTransform(img.Height * Math.Sin(alpha), 0)

        g.RotateTransform(degree)
        'ImgTarget.MakeTransparent(ImgTarget.GetPixel(1, 1))
        g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height))
        g.Dispose()
        img = ImgTarget.Clone
        ImgTarget.Dispose()
        Return img
    End Function
    Private Function Rotate90_180(ByRef img As Image, ByVal degree As Integer) As Image
        Dim ImgTarget As Bitmap
        Dim alpha As Double = ((degree - 90) / 180) * PI

        Dim iHeight As Integer = img.Width * Math.Cos(alpha) + img.Height * Math.Sin(alpha)
        Dim iWidth As Integer = img.Width * Math.Sin(alpha) + img.Height * Math.Cos(alpha)

        ImgTarget = New Bitmap(iWidth, iHeight, Drawing.Imaging.PixelFormat.Format24bppRgb)
        Dim g As Graphics
        g = Graphics.FromImage(ImgTarget)

        g.TranslateTransform(iWidth, img.Height * Math.Sin(alpha))

        g.RotateTransform(degree)
        'ImgTarget.MakeTransparent(ImgTarget.GetPixel(1, 1))
        g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height))
        g.Dispose()
        img = ImgTarget.Clone
        ImgTarget.Dispose()
        Return img
    End Function
    Private Function Rotate180_270(ByRef img As Image, ByVal degree As Integer) As Image
        Dim ImgTarget As Bitmap
        Dim alpha As Double = ((degree - 180) / 180) * PI

        Dim iWidth As Integer = img.Width * Math.Cos(alpha) + img.Height * Math.Sin(alpha)
        Dim iHeight As Integer = img.Width * Math.Sin(alpha) + img.Height * Math.Cos(alpha)

        ImgTarget = New Bitmap(iWidth, iHeight, Drawing.Imaging.PixelFormat.Format24bppRgb)
        Dim g As Graphics
        g = Graphics.FromImage(ImgTarget)

        g.TranslateTransform(img.Width * Math.Cos(alpha), iHeight)

        g.RotateTransform(degree)
        'ImgTarget.MakeTransparent(ImgTarget.GetPixel(1, 1))
        g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height))
        g.Dispose()
        img = ImgTarget.Clone
        ImgTarget.Dispose()
        Return img
    End Function
    Private Function Rotate270_360(ByRef img As Image, ByVal degree As Integer) As Image
        Dim ImgTarget As Bitmap
        Dim alpha As Double = ((degree - 270) / 180) * PI

        Dim iHeight As Integer = img.Width * Math.Cos(alpha) + img.Height * Math.Sin(alpha)
        Dim iWidth As Integer = img.Width * Math.Sin(alpha) + img.Height * Math.Cos(alpha)

        ImgTarget = New Bitmap(iWidth, iHeight, Drawing.Imaging.PixelFormat.Format24bppRgb)
        Dim g As Graphics
        g = Graphics.FromImage(ImgTarget)

        g.TranslateTransform(0, img.Width * Math.Cos(alpha))

        g.RotateTransform(degree)
        'ImgTarget.MakeTransparent(ImgTarget.GetPixel(1, 1))
        g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height))
        g.Dispose()
        img = ImgTarget.Clone
        ImgTarget.Dispose()
        Return img
    End Function

    Private Sub HorizontalCorrectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HorizontalCorrectionToolStripMenuItem.Click
        If node_count = 1 Then
            Dim tmp_degree As Single = 0
            If line_point(0).Y > line_point(1).Y Then
                tmp_degree = Math.Atan2(-line_point(0).Y + line_point(1).Y, line_point(0).X - line_point(1).X) * 180 / Math.PI
            Else
                tmp_degree = Math.Atan2(-line_point(1).Y + line_point(0).Y, line_point(1).X - line_point(0).X) * 180 / Math.PI
            End If
            tmp_degree = (tmp_degree + 360) Mod 360
            If tmp_degree >= 270 Then
                Picture.Image = Rotate270_360(current_bmp, tmp_degree)
                Picture.Size = New Size(current_bmp.Width * zoom_radio, current_bmp.Height * zoom_radio)
            Else
                tmp_degree = tmp_degree Mod 180
                Picture.Image = Rotate0_90(current_bmp, tmp_degree)
                Picture.Size = New Size(current_bmp.Width * zoom_radio, current_bmp.Height * zoom_radio)
            End If
            node_count = 0
            Init()
        Else
            MsgBox("You need two points to get horizontal correction. Use LINE tool to make a line.")
        End If
    End Sub

    Private Sub VerticalCorrectionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VerticalCorrectionToolStripMenuItem.Click
        If node_count = 1 Then
            Dim tmp_degree As Single = 0
            If line_point(0).X > line_point(1).X Then
                tmp_degree = 270 + Math.Atan2(-line_point(0).Y + line_point(1).Y, line_point(0).X - line_point(1).X) * 180 / Math.PI
            Else
                tmp_degree = 270 + Math.Atan2(-line_point(1).Y + line_point(0).Y, line_point(1).X - line_point(0).X) * 180 / Math.PI
            End If
            tmp_degree = (tmp_degree + 360) Mod 360
            If tmp_degree >= 270 Then
                Picture.Image = Rotate270_360(current_bmp, tmp_degree)
                Picture.Size = New Size(current_bmp.Width * zoom_radio, current_bmp.Height * zoom_radio)
            Else
                tmp_degree = tmp_degree Mod 180
                Picture.Image = Rotate0_90(current_bmp, tmp_degree)
                Picture.Size = New Size(current_bmp.Width * zoom_radio, current_bmp.Height * zoom_radio)
            End If
            node_count = 0
            Init()
        Else
            MsgBox("You need two points to get vertical correction. Use LINE tool to make a line.")
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        If node_count = 1 Then
            Dim CropRect As New Rectangle(min(line_point(0).X, line_point(1).X) / zoom_radio, min(line_point(0).Y, line_point(1).Y) / zoom_radio, Abs(line_point(0).X - line_point(1).X) / zoom_radio, Abs(line_point(0).Y - line_point(1).Y) / zoom_radio)
            Dim CropImage = New Bitmap(CropRect.Width, CropRect.Height)
            Dim g As Graphics = Graphics.FromImage(CropImage)
            g.DrawImage(current_bmp, New Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel)
            g.Dispose()
            current_bmp = CropImage.Clone
            CropImage.Dispose()
            Picture.Image = current_bmp
            Picture.Size = New Size(current_bmp.Width * zoom_radio, current_bmp.Height * zoom_radio)
            node_count = 0
            Init()
        Else
            MsgBox("You need two points to crop the image. Use COLOR tool to make a rectangle.")
        End If


    End Sub

    Private Sub BlueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlueToolStripMenuItem.Click
        RedToolStripMenuItem.Checked = False
        GreenToolStripMenuItem.Checked = False
        BlueToolStripMenuItem.Checked = True
        CODEToolStripMenuItem.Checked = False
        GrayToolStripMenuItem.Checked = False
        RGBToolStripMenuItem.Checked = False

    End Sub

    Private Sub GrayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GrayToolStripMenuItem.Click
        RedToolStripMenuItem.Checked = False
        GreenToolStripMenuItem.Checked = False
        BlueToolStripMenuItem.Checked = False
        CODEToolStripMenuItem.Checked = False
        RGBToolStripMenuItem.Checked = False
        GrayToolStripMenuItem.Checked = True
    End Sub

    Private Sub GreenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GreenToolStripMenuItem.Click
        RedToolStripMenuItem.Checked = False
        GreenToolStripMenuItem.Checked = True
        BlueToolStripMenuItem.Checked = False
        CODEToolStripMenuItem.Checked = False
        RGBToolStripMenuItem.Checked = False
        GrayToolStripMenuItem.Checked = False
    End Sub

    Private Sub RedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedToolStripMenuItem.Click
        RedToolStripMenuItem.Checked = True
        GreenToolStripMenuItem.Checked = False
        BlueToolStripMenuItem.Checked = False
        GrayToolStripMenuItem.Checked = False
        RGBToolStripMenuItem.Checked = False
        CODEToolStripMenuItem.Checked = False
    End Sub

    Private Sub CODEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CODEToolStripMenuItem.Click
        RedToolStripMenuItem.Checked = False
        GreenToolStripMenuItem.Checked = False
        BlueToolStripMenuItem.Checked = False
        GrayToolStripMenuItem.Checked = False
        CODEToolStripMenuItem.Checked = True
        RGBToolStripMenuItem.Checked = False
    End Sub

    Private Sub RGBToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RGBToolStripMenuItem.Click
        RedToolStripMenuItem.Checked = False
        GreenToolStripMenuItem.Checked = False
        BlueToolStripMenuItem.Checked = False
        GrayToolStripMenuItem.Checked = False
        CODEToolStripMenuItem.Checked = False
        RGBToolStripMenuItem.Checked = True
    End Sub

    Private Sub InsertToToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InsertToToolStripMenuItem.Click
        If ListView1.SelectedIndices.Count > 0 Then
            Dim target_str As String = InputBox("Please enter a new ID (starting from 1): ", "New ID", "")
            If target_str <> "" Then
                Dim target_id As Integer = CInt(target_str)
                If target_id >= 1 And target_id <= ListView1.Items.Count Then
                    Dim current_id As Integer = ListView1.SelectedIndices(0) + 1
                    Dim temp_longarm() As Single = longarm.Clone
                    Dim temp_shortarm() As Single = shortarm.Clone
                    Dim temp_suiarm() As Single = suiarm.Clone
                    Dim temp_points_group() As Object = points_group.Clone

                    Dim temp_list As New List(Of Integer)
                    For i As Integer = 0 To data_count
                        temp_list.Add(i)
                    Next
                    temp_list.Remove(current_id)
                    temp_list.Insert(target_id, current_id)

                    Dim temp_ListView1 As Object = ListView1.Items(current_id - 1)
                    ListView1.Items.RemoveAt(current_id - 1)
                    ListView1.Items.Insert(target_id - 1, temp_ListView1)

                    For i As Integer = 1 To data_count
                        If temp_list(i) <> i Then
                            longarm(i) = temp_longarm(temp_list(i))
                            shortarm(i) = temp_shortarm(temp_list(i))
                            suiarm(i) = temp_suiarm(temp_list(i))
                            points_group(i) = temp_points_group(temp_list(i))
                        End If
                        ListView1.Items(i - 1).SubItems(0).Text = i
                        ListView1.Items(i - 1).SubItems(3).Text = suiarm(i)
                    Next
                    Picture.Refresh()
                    Me.Refresh()
                Else
                    MsgBox("Please enter a valid ID!")
                End If
            Else
                MsgBox("Please enter a valid ID!")
            End If
        Else
            MsgBox("Please select the item you want to modify!")
        End If
    End Sub

    Private Sub AscendingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AscendingToolStripMenuItem.Click
        Dim temp_longarm() As Single = longarm.Clone
        Dim temp_shortarm() As Single = shortarm.Clone
        Dim temp_suiarm() As Single = suiarm.Clone
        Dim temp_points_group() As Object = points_group.Clone

        Dim temp_list() As Integer
        ReDim Preserve temp_list(data_count)
        Dim temp_armsum() As Single
        ReDim Preserve temp_armsum(data_count)
        Dim temp_listview() As Object
        ReDim Preserve temp_listview(data_count - 1)

        For i As Integer = 1 To data_count
            temp_list(i) = i
            temp_armsum(i) = temp_shortarm(i) + temp_longarm(i)
            temp_listview(i - 1) = ListView1.Items(i - 1).Clone
        Next
        Array.Sort(temp_armsum, temp_list, 1, data_count)
        For i As Integer = 1 To data_count
            If temp_list(i) <> i Then
                longarm(i) = temp_longarm(temp_list(i))
                shortarm(i) = temp_shortarm(temp_list(i))
                suiarm(i) = temp_suiarm(temp_list(i))
                points_group(i) = temp_points_group(temp_list(i))
                ListView1.Items(i - 1) = temp_listview(temp_list(i) - 1)
            End If

        Next
        For i As Integer = 1 To data_count
            ListView1.Items(i - 1).SubItems(0).Text = i
            ListView1.Items(i - 1).SubItems(3).Text = suiarm(i)
        Next
        Picture.Refresh()
        Me.Refresh()
    End Sub

    Private Sub DescendingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DescendingToolStripMenuItem.Click
        Dim temp_longarm() As Single = longarm.Clone
        Dim temp_shortarm() As Single = shortarm.Clone
        Dim temp_suiarm() As Single = suiarm.Clone
        Dim temp_points_group() As Object = points_group.Clone

        Dim temp_list() As Integer
        ReDim Preserve temp_list(data_count)
        Dim temp_armsum() As Single
        ReDim Preserve temp_armsum(data_count)
        Dim temp_listview() As Object
        ReDim Preserve temp_listview(data_count - 1)

        For i As Integer = 1 To data_count
            temp_list(i) = i
            temp_armsum(i) = temp_shortarm(i) + temp_longarm(i)
            temp_listview(i - 1) = ListView1.Items(i - 1).Clone
        Next
        Array.Sort(temp_armsum, temp_list, 1, data_count, Descending)
        For i As Integer = 1 To data_count
            If temp_list(i) <> i Then
                longarm(i) = temp_longarm(temp_list(i))
                shortarm(i) = temp_shortarm(temp_list(i))
                suiarm(i) = temp_suiarm(temp_list(i))
                points_group(i) = temp_points_group(temp_list(i))
                ListView1.Items(i - 1) = temp_listview(temp_list(i) - 1)
            End If

        Next
        For i As Integer = 1 To data_count
            ListView1.Items(i - 1).SubItems(0).Text = i
            ListView1.Items(i - 1).SubItems(3).Text = suiarm(i)
        Next
        Picture.Refresh()
        Me.Refresh()
    End Sub
End Class
