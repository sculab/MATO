Imports System.IO
Public Class Form_Combine
    Dim group_count As Integer = 0
    Dim x() As Integer
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim opendialog As New OpenFileDialog
        opendialog.Filter = "Table format File(*.xls)|*.xls;*.xls|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
        opendialog.FileName = ""
        opendialog.DefaultExt = ".xls"
        opendialog.CheckFileExists = True
        opendialog.CheckPathExists = True
        Dim resultdialog As DialogResult = opendialog.ShowDialog()
        If resultdialog = DialogResult.OK Then
            Dim sr As New StreamReader(opendialog.FileName)
            Dim not_got As Boolean = True
            Dim line As String = ""
            Do
                line = sr.ReadLine
                If line <> "" Then
                    If line.StartsWith("Karyotype parameters table") Then
                        line = sr.ReadLine
                        line = sr.ReadLine
                        Dim temp_line() As String = line.Split("	")
                        Dim str(6) As String
                        Dim itm As ListViewItem
                        str(0) = opendialog.FileName
                        str(1) = temp_line(0)
                        str(2) = temp_line(1)
                        str(3) = temp_line(2)
                        str(4) = temp_line(3)
                        str(5) = temp_line(4)
                        str(6) = temp_line(5)
                        itm = New ListViewItem(str)
                        ListView1.Items.Add(itm)
                        not_got = False
                    End If
                End If
            Loop Until line Is Nothing
            If not_got Then
                MsgBox("This file could not be combined!")
            End If
            sr.Close()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ListView1.Items.RemoveAt(ListView1.SelectedIndices(0))
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub
    Dim global_x As Integer = 0
    Dim global_2n As Integer = 0
    Dim global_level As Integer = 0
    Dim gla_o() As Single 'global_long_arm original
    Dim gsa_o() As Single 'global_short_arm original
    Dim g_plus_o() As Single 'global_plus original
    Dim g_div_o() As Single 'global_divid original
    Dim g_type_o() As String 'global_type original
    Dim g_t_l_o(,) As Single 'global_table_long arm original
    Dim g_t_l(,) As Single 'global_table_long arm
    Dim g_t_s_o(,) As Single 'global_table_long arm original
    Dim g_t_s(,) As Single 'global_table_long arm
    Dim g_sat(,) As Integer

    Dim g_sd_t_o(,) As Single 'global_table_sd original
    Dim g_sd_t(,) As Single 'global_table_sd
    Dim gla() As Single 'global_long_arm
    Dim gsa() As Single 'global_short_arm

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim file_count As Integer = ListView1.Items.Count
        If file_count > 1 Then
            Dim pass_check As Boolean = True
            For i As Integer = 0 To file_count - 1
                For j As Integer = i + 1 To file_count - 1
                    If ListView1.Items(i).SubItems(2).Text <> ListView1.Items(j).SubItems(2).Text Then
                        pass_check = False
                    End If
                Next
            Next
            For i As Integer = 0 To file_count - 1
                For j As Integer = i + 1 To file_count - 1
                    If ListView1.Items(i).SubItems(1).Text <> ListView1.Items(j).SubItems(1).Text Then
                        pass_check = False
                    End If
                Next
            Next
            If pass_check = False Then
                MsgBox("All files should have the same 2n and x!")
            Else
                global_2n = CInt(ListView1.Items(0).SubItems(2).Text)
                global_x = CInt(ListView1.Items(0).SubItems(1).Text)
                global_level = global_2n / global_x
                ReDim gla_o(global_2n)
                ReDim gsa_o(global_2n)
                ReDim g_plus_o(global_2n)
                ReDim g_div_o(global_2n)
                ReDim g_type_o(global_2n)
                ReDim g_t_l_o(file_count, global_2n)
                ReDim g_t_l(file_count, global_x)
                ReDim g_t_s_o(file_count, global_2n)
                ReDim g_t_s(file_count, global_x)
                ReDim g_sd_t_o(6, global_2n)
                ReDim g_sd_t(6, global_x)
                ReDim g_sat(3, global_2n)
                ReDim gla(global_x)
                ReDim gsa(global_x)
                For k As Integer = 1 To global_x
                    gla(k) = 0
                    gsa(k) = 0
                Next
                For k As Integer = 1 To global_2n
                    gla_o(k) = 0
                    gsa_o(k) = 0
                    For p As Integer = 0 To 3
                        g_sat(p, k) = 0
                    Next
                Next
                For i As Integer = 0 To file_count - 1
                    Dim sr As New StreamReader(ListView1.Items(i).SubItems(0).Text)
                    Dim line As String = ""
                    Do
                        line = sr.ReadLine
                        If line <> "" Then
                            Dim temp_l() As Single
                            Dim temp_s() As Single
                            ReDim temp_l(global_2n)
                            ReDim temp_s(global_2n)
                            For k As Integer = 1 To global_2n
                                temp_l(k) = -1
                                temp_s(k) = -1
                            Next
                            If line.StartsWith("Original Table") Then
                                line = sr.ReadLine
                                For k As Integer = 1 To global_2n
                                    line = sr.ReadLine
                                    Dim temp_str() As String = line.Split("	")
                                    Dim temp_group_id As Integer = CInt(temp_str(6))
                                    For n As Integer = 1 To global_level
                                        If temp_l((temp_group_id - 1) * global_level + n) = -1 Then
                                            g_t_l_o(i + 1, (temp_group_id - 1) * global_level + n) = CSng(temp_str(1))
                                            g_t_s_o(i + 1, (temp_group_id - 1) * global_level + n) = CSng(temp_str(2))
                                            g_sat(CInt(temp_str(7)), (temp_group_id - 1) * global_level + n) = 1
                                            temp_l((temp_group_id - 1) * global_level + n) = CSng(temp_str(1))
                                            temp_s((temp_group_id - 1) * global_level + n) = CSng(temp_str(2))
                                            Exit For
                                        End If
                                    Next
                                Next
                                For k As Integer = 1 To global_2n
                                    gsa_o(k) += temp_s(k)
                                    gla_o(k) += temp_l(k)
                                Next

                            End If
                        End If
                    Loop Until line Is Nothing
                    sr.Close()
                Next
                Dim g_a_2 As Integer = 0 '比大于2的数目
                Dim g_max_arm As Single = 0
                For i As Integer = 1 To 6
                    typenum(i) = 0
                    sat(i) = 0
                Next
                For k As Integer = 1 To global_2n
                    gsa_o(k) = gsa_o(k) / file_count
                    gla_o(k) = gla_o(k) / file_count
                    g_plus_o(k) = gsa_o(k) + gla_o(k)
                    g_max_arm = max(g_plus_o(k), g_max_arm)
                    g_div_o(k) = gla_o(k) / (gsa_o(k) + 0.0001)
                    g_type_o(k) = Get_chromo_type(g_div_o(k), gsa_o(k), g_sat(1, k) + g_sat(2, k) + g_sat(3, k))
                    If g_div_o(k) > 2 Then
                        g_a_2 += 1
                    End If
                Next
                For k As Integer = 1 To global_x
                    For n As Integer = 1 To global_level
                        gsa(k) += gsa_o((k - 1) * global_level + n)
                        gla(k) += gla_o((k - 1) * global_level + n)
                    Next
                    gsa(k) = gsa(k) / global_level
                    gla(k) = gla(k) / global_level
                Next
                For i As Integer = 1 To file_count
                    For k As Integer = 1 To global_x
                        For n As Integer = 1 To global_level
                            g_t_s(i, k) += g_t_s_o(i, (k - 1) * global_level + n)
                            g_t_l(i, k) += g_t_l_o(i, (k - 1) * global_level + n)
                        Next
                        g_t_s(i, k) = g_t_s(i, k) / global_level
                        g_t_l(i, k) = g_t_l(i, k) / global_level
                    Next
                Next
                For k As Integer = 1 To global_x
                    Dim temp_array_l() As Single
                    Dim temp_array_s() As Single
                    Dim temp_array_l_p_s() As Single
                    Dim temp_array_l_d_s() As Single
                    Dim temp_array_l_m_s() As Single
                    Dim temp_array_s_d_ls() As Single

                    ReDim temp_array_l(file_count - 1)
                    ReDim temp_array_s(file_count - 1)
                    ReDim temp_array_l_p_s(file_count - 1)
                    ReDim temp_array_l_d_s(file_count - 1)
                    ReDim temp_array_l_m_s(file_count - 1)
                    ReDim temp_array_s_d_ls(file_count - 1)
                    For i As Integer = 1 To file_count
                        temp_array_l(i - 1) = g_t_l(i, k)
                        temp_array_s(i - 1) = g_t_s(i, k)
                        temp_array_l_p_s(i - 1) = g_t_l(i, k) + g_t_s(i, k)
                        temp_array_l_d_s(i - 1) = g_t_l(i, k) / g_t_s(i, k)
                        temp_array_l_m_s(i - 1) = g_t_l(i, k) - g_t_s(i, k)
                        temp_array_s_d_ls(i - 1) = g_t_s(i, k) / (g_t_l(i, k) + g_t_s(i, k)) * 100
                    Next
                    g_sd_t(1, k) = cal_sd(temp_array_l)
                    g_sd_t(2, k) = cal_sd(temp_array_s)
                    g_sd_t(3, k) = cal_sd(temp_array_l_p_s)
                    g_sd_t(5, k) = cal_sd(temp_array_l_d_s)
                    g_sd_t(4, k) = cal_sd(temp_array_l_m_s)
                    g_sd_t(6, k) = cal_sd(temp_array_s_d_ls)
                Next
                For k As Integer = 1 To global_2n
                    Dim temp_array_l() As Single
                    Dim temp_array_s() As Single
                    Dim temp_array_l_p_s() As Single
                    Dim temp_array_l_d_s() As Single
                    Dim temp_array_l_m_s() As Single
                    Dim temp_array_s_d_ls() As Single

                    ReDim temp_array_l(file_count - 1)
                    ReDim temp_array_s(file_count - 1)
                    ReDim temp_array_l_p_s(file_count - 1)
                    ReDim temp_array_l_d_s(file_count - 1)
                    ReDim temp_array_l_m_s(file_count - 1)
                    ReDim temp_array_s_d_ls(file_count - 1)
                    For i As Integer = 1 To file_count
                        temp_array_l(i - 1) = g_t_l_o(i, k)
                        temp_array_s(i - 1) = g_t_s_o(i, k)
                        temp_array_l_p_s(i - 1) = g_t_l_o(i, k) + g_t_s_o(i, k)
                        temp_array_l_d_s(i - 1) = g_t_l_o(i, k) / g_t_s_o(i, k)
                        temp_array_l_m_s(i - 1) = g_t_l_o(i, k) - g_t_s_o(i, k)
                        temp_array_s_d_ls(i - 1) = g_t_s_o(i, k) / (g_t_l_o(i, k) + g_t_s_o(i, k)) * 100
                    Next
                    g_sd_t_o(1, k) = cal_sd(temp_array_l)
                    g_sd_t_o(2, k) = cal_sd(temp_array_s)
                    g_sd_t_o(3, k) = cal_sd(temp_array_l_p_s)
                    g_sd_t_o(5, k) = cal_sd(temp_array_l_d_s)
                    g_sd_t_o(4, k) = cal_sd(temp_array_l_m_s)
                    g_sd_t_o(6, k) = cal_sd(temp_array_s_d_ls)
                Next
                For i As Integer = 1 To file_count
                    For k As Integer = 1 To global_x
                        g_t_s(i, 0) += (g_t_l(i, k) - g_t_s(i, k)) / (g_t_l(i, k) + g_t_s(i, k)) * 100
                        g_t_l(i, 0) += g_t_l(i, k) + g_t_s(i, k)
                    Next
                Next
                Dim mca_sd() As Single
                Dim thl_sd() As Single
                Dim cvci_sd() As Single
                Dim cvcl_sd() As Single
                ReDim mca_sd(file_count - 1)
                ReDim thl_sd(file_count - 1)
                ReDim cvci_sd(file_count - 1)
                ReDim cvcl_sd(file_count - 1)
                For i As Integer = 0 To file_count - 1
                    mca_sd(i) = g_t_s(i + 1, 0) / global_x
                    thl_sd(i) = g_t_l(i + 1, 0) / global_x
                    cvci_sd(i) = CSng(ListView1.Items(i).SubItems(4).Text)
                    cvcl_sd(i) = CSng(ListView1.Items(i).SubItems(5).Text)
                Next
                Dim g_sum As Single = 0 '染色体 total length
                Dim avx As Single = 0 'avx为染色体长度的平均值
                Dim asv As Single = 0 '长度的标准差
                Dim avm As Single = 0 'Average of S/(L+S)
                Dim asm As Single = 0 'SD of S/(L+S)
                Dim mca As Single = 0 'MCA (L-S)/(L+S)
                For k As Integer = 1 To global_x
                    g_sum += gla(k) + gsa(k)
                    avm += gsa(k) / (gla(k) + gsa(k)) * 100
                    mca += (gla(k) - gsa(k)) / (gla(k) + gsa(k)) * 100
                Next
                avx = g_sum / global_x
                avm = avm / global_x
                mca = mca / global_x

                For k As Integer = 1 To global_x
                    asv += ((gla(k) + gsa(k)) - avx) ^ 2
                Next
                asv = (asv / (global_x - 1)) ^ 0.5

                For k As Integer = 1 To global_x
                    asm = (gsa(k) / (gla(k) + gsa(k)) * 100 - avm) ^ 2 + asm
                Next
                asm = (asm / (global_x - 1)) ^ 0.5



                RichTextBox1.Text = "Karyotype asymmetry degree (Stebbins, 1971): "
                If g_a_2 / global_2n < 0.01 Then
                    RichTextBox1.AppendText("1")
                Else
                    If g_a_2 / global_2n < 0.5 Then
                        RichTextBox1.AppendText("2")
                    Else
                        If g_a_2 / global_2n < 0.99 Then
                            RichTextBox1.AppendText("3")
                        Else
                            RichTextBox1.AppendText("4")
                        End If
                    End If
                End If

                If g_plus_o(global_2n) / g_max_arm < 2 Then
                    RichTextBox1.AppendText("A" + vbCrLf)
                Else
                    If g_plus_o(global_2n) / g_max_arm <= 4 Then
                        RichTextBox1.AppendText("B" + vbCrLf)
                    Else
                        RichTextBox1.AppendText("C" + vbCrLf)
                    End If
                End If

                RichTextBox1.AppendText("Karyotype formula (Levan et al., 1964): " + "2n = " + Str(global_level) + "x = ")
                For k As Integer = 1 To 6
                    If typenum(k) > 0 Then
                        If sat(k) > 0 Then
                            RichTextBox1.AppendText(Str(typenum(k)) + typestr(k) + "(" + Str(sat(k)).Replace(" ", "") + "sat)" + " +")
                        Else
                            RichTextBox1.AppendText(Str(typenum(k)) + typestr(k) + " +")
                        End If
                    End If
                Next
                If RichTextBox1.Text.EndsWith("+") Then
                    RichTextBox1.Text = RichTextBox1.Text.Remove(RichTextBox1.Text.Length - 1, 1)
                End If


                RichTextBox1.AppendText(vbCrLf + vbCrLf + "Combined Table (L: long arms; S: short arms; &0: no satellite; &1: on the long arm; &2: on the short arm; &3: intercalary satellite)" + vbCrLf + "ID	L	S	L+S	L-S	L/S	Group ID	satellites" + vbCrLf)
                For k As Integer = 1 To global_2n
                    RichTextBox1.AppendText(k.ToString + "	" + (gla_o(k)).ToString("F2") + "±" + g_sd_t_o(1, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gsa_o(k)).ToString("F2") + "±" + g_sd_t_o(2, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla_o(k) + gsa_o(k)).ToString("F2") + "±" + g_sd_t_o(3, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla_o(k) - gsa_o(k)).ToString("F2") + "±" + g_sd_t_o(4, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla_o(k) / (gsa_o(k) + 0.0001)).ToString("F2") + "±" + g_sd_t_o(5, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((((k - 1) - (k - 1) Mod global_level) / global_level + 1).ToString("F0") + "	")
                    For p As Integer = 0 To 3
                        If g_sat(p, k) = 1 Then
                            RichTextBox1.AppendText("&" + p.ToString("F0"))
                        End If
                    Next
                    RichTextBox1.AppendText(vbCrLf)
                Next


                RichTextBox1.AppendText(vbCrLf + "Combined haploid Table")
                RichTextBox1.AppendText(vbCrLf + "Group ID	L	S	L+S	L-S	S/L+S(%)	L/S	" + vbCrLf)
                For k As Integer = 1 To global_x
                    RichTextBox1.AppendText(Str(k) + "	" + (gla(k)).ToString("F2") + "±" + g_sd_t(1, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gsa(k)).ToString("F2") + "±" + g_sd_t(2, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gsa(k) + gla(k)).ToString("F2") + "±" + g_sd_t(3, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla(k) - gsa(k)).ToString("F2") + "±" + g_sd_t(4, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gsa(k) / (gla(k) + gsa(k)) * 100).ToString("F2") + "±" + g_sd_t(6, k).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla(k) / (gsa(k) + 0.0001)).ToString("F2") + "±" + g_sd_t(5, k).ToString("F2") + vbCrLf)
                Next

                RichTextBox1.AppendText(vbCrLf + "Combined Karyotype parameters table")

                RichTextBox1.AppendText(vbCrLf + "x	2n	THL	CVCI	CVCL	MCA" + vbCrLf)
                RichTextBox1.AppendText((global_x).ToString + "	" + (global_2n).ToString + "	")
                RichTextBox1.AppendText((g_sum).ToString("F2") + "±" + cal_sd(thl_sd).ToString("F2") + "	")
                RichTextBox1.AppendText((asm / avm * 100).ToString("F2") + "±" + cal_sd(cvci_sd).ToString("F2") + "	")
                RichTextBox1.AppendText((asv / avx * 100).ToString("F2") + "±" + cal_sd(cvcl_sd).ToString("F2") + "	")
                RichTextBox1.AppendText(mca.ToString("F2") + "±" + cal_sd(mca_sd).ToString("F2") + vbCrLf)


                RichTextBox1.AppendText(vbCrLf + "The relative length")
                RichTextBox1.AppendText(vbCrLf + "Group	" + "L(%)	S(%)	L+S (%)	" + "L/S	" + "Type" + vbCrLf)
                For k As Integer = 1 To global_x
                    RichTextBox1.AppendText(Str(k) + "	")
                    RichTextBox1.AppendText((gla(k) / g_sum * 100).ToString("F2") + "	")
                    RichTextBox1.AppendText((gsa(k) / g_sum * 100).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla(k) / g_sum * 100).ToString("F2") + "+" + (gsa(k) * 100 / g_sum).ToString("F2") + "=" + (gsa(k) * 100 / g_sum + gla(k) * 100 / g_sum).ToString("F2") + "	")
                    RichTextBox1.AppendText((gla(k) / (gsa(k) + 0.0001)).ToString("F2") + "	" + g_type_o(k) + vbCrLf)
                Next


                RichTextBox1.AppendText(vbCrLf + "ID	" + "L(%)	S(%)	L+S (%)	" + "L/S	" + "Type" + vbCrLf)
                For k As Integer = 1 To global_2n
                    RichTextBox1.AppendText(k.ToString + "	" + (gla_o(k) / g_sum / global_level * 100).ToString("F2") + "	" + (gsa_o(k) * 100 / global_level / g_sum).ToString("F2") + "	" + (gla_o(k) / g_sum / global_level * 100).ToString("F2") + "+" + (gsa_o(k) / global_level * 100 / g_sum).ToString("F2") + "=" + ((gla_o(k) + gsa_o(k)) * 100 / global_level / g_sum).ToString("F2") + "	" + (gla_o(k) / (gsa_o(k) + 0.0001)).ToString("F2") + "	" + g_type_o(k) + vbCrLf)
                Next

                Me.TopMost = False
                Dim sfd As New SaveFileDialog
                sfd.Filter = "Karyo Files(*.karyo)|*.karyo;*.nuc;*.NUC|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
                sfd.FileName = ""
                sfd.DefaultExt = ".karyo"
                sfd.CheckPathExists = True
                Dim resultdialog As DialogResult = sfd.ShowDialog()
                If resultdialog = DialogResult.OK Then
                    WriteCombine(sfd.FileName)
                End If
                Me.TopMost = True
            End If
        Else
            MsgBox("You need at least two files!")
        End If
    End Sub
    Private Sub WriteCombine(ByVal filename As String)
        Dim sw As StreamWriter = New StreamWriter(filename)
        ' Add some text to the file.
        sw.WriteLine("Begin;")
        sw.WriteLine("NUM=" + global_2n.ToString + ";")
        sw.WriteLine("LEVEL=" + global_level.ToString + ";")
        sw.WriteLine("PICPATH=;")
        sw.WriteLine("Staff=1;")
        sw.WriteLine("UNIT=" + ScaleForm.ComboBox1.SelectedItem)
        sw.WriteLine("Data;")
        For k As Integer = 1 To global_2n
            sw.WriteLine(Str(k) + ";" + Str(gla_o(k)) + ";" + Str(gsa_o(k)) + ";0;" + Str(k))
        Next
        sw.WriteLine("Analyse;")
        sw.WriteLine(RichTextBox1.Text)
        ' Arbitrary objects can also be written to the file.
        sw.WriteLine("End;")
        sw.WriteLine(DateTime.Now)
        sw.Close()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Me.TopMost = False
        Dim sfd As New SaveFileDialog
        sfd.Filter = "Table format File(*.xls)|*.xls;*.xls|Text Files(*.txt)|*.txt;*.TXT|ALL Files(*.*)|*.*"
        sfd.FileName = ""
        sfd.DefaultExt = ".xls"
        sfd.CheckPathExists = True
        Dim resultdialog As DialogResult = sfd.ShowDialog()
        If resultdialog = DialogResult.OK Then
            RichTextBox1.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText)
        End If
        Me.TopMost = True
    End Sub

    Private Sub Form_Combine_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class