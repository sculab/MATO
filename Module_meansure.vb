
Imports System.Globalization.CultureInfo
Imports System.Math
Module Module_meansure
    Public exepath As String
    Public mode_type As Integer = 0
    Public data_count As Integer = 0
    Public times As Integer = 2
    Public last_type As Integer = 0
    Public last_count As Integer
    Public radio_div_len As Single = 1.732
    Public points_group_1(2048) As Point
    Public points_group_2(2048) As Point
    Public has_record As Boolean = True
    Public build_version As String = "build 20191119"
    Public bchrom, suichrom As Integer
    Public bchrom_list(2048) As Boolean
    Public longarm(2048) As Single
    Public shortarm(2048) As Single
    Public suiarm(2048) As Single
    Public armplus(2048) As Single
    Public armdivide(2048) As Single

    Public cycle(35, 8) As Integer
    Public Range(2048) As Integer

    Public data_id(2048) As Integer
    Public his_point(2048) As Point
    Public line_point(2048) As Point
    Public points_group(2048) As Object
    Public zoom_radio As Single = 1
    Public MDown1X As Single
    Public MDown1Y As Single
    Public MDown2X As Single
    Public MDown2Y As Single
    Public armlongth As Single
    Public operate As String = "null"
    Public image_path As String
    Public node_count As Integer
    Public in_meansure As Boolean
    Public sumarm As Single = 1000
    Public summod As Single = 1000

    '线
    '绿色pen_0
    Public pen_line As New Pen(Color.FromArgb(192, 112, 173, 71), 2)
    ''蓝色pen
    'Public pen_blue As New Pen(Color.FromArgb(255, 51, 133, 255), 5)
    ''橙色pen
    'Public pen_orange As New Pen(Color.FromArgb(192, 237, 125, 49), 2)

    '标尺
    Public scale_pen As New Pen(Color.Black, 3)
    Public scale_brush As New SolidBrush(Color.Black)

    '点
    Public point_size As Integer = 3
    '棕色pen
    Public pen_circle As New Pen(Color.FromArgb(224, 178, 93, 34), 2)
    '橙色brush
    Public brush_circle As New SolidBrush(Color.FromArgb(224, 237, 125, 49))

    '长短臂
    '蓝色pen_1
    Public pen_arm_1 As New Pen(Color.FromArgb(224, 0, 102, 202), 2)
    '绿色pen_2
    Public pen_arm_2 As New Pen(Color.FromArgb(224, 51, 153, 51), 2)
    '蓝色brush_1
    Public brush_arm_1 As New SolidBrush(Color.FromArgb(64, 0, 102, 202))
    '绿色brush_2
    Public brush_arm_2 As New SolidBrush(Color.FromArgb(64, 51, 153, 51))

    '面
    '绿色brush
    Public brush_size As New SolidBrush(Color.FromArgb(64, 112, 173, 71))

    '文字
    Public brush_font As New SolidBrush(Color.FromArgb(224, 0, 0, 0))

    '网格
    Public netPen As New Pen(Color.FromArgb(128, 128, 128, 128), 1)


    Public PicArea As Single = 1

    Public current_scale As Single = 1
    Public scale_size As Integer = 100
    Public scale_title As String = ""
    Public scale_unit As String = "px"

    Public upper As Object = New myReverserClass
    Public chtype(512) As String
    Public sat(6) As Integer
    Public typenum(6) As Integer
    Public typestr(6) As String

    Public Function gu_GetArea(ByVal strBorder As String) As Double
        '----------------------------------------------------------------------
        '功能:
        '     计算任意封闭多边形的水平面积
        '参数:
        '     按照顺时针或逆时针排列的顶点坐标的字符串，其中点之间用“;”分割,
        '     坐标之间用“,”分隔
        '返回：如果没有范围线，则面积为０
        '---------------------------------------------------------------------
        Dim i As Double
        Dim U, V, w As Double
        Dim S1 As Object
        Dim S2 As Object
        Dim q(,) As Double
        Dim k As Integer

        If Len(strBorder & "") = 0 Then gu_GetArea = 0 : Exit Function

        S1 = Split(strBorder, ";")

        k = UBound(S1)
        ReDim q(k, 1)
        For i = 0 To k
            S2 = Split(S1(i), ",")
            q(i, 0) = S2(0)
            q(i, 1) = S2(1)
        Next i

        w = 0
        For i = 0 To k
            If i = k Then
                U = q(0, 0)
                V = q(0, 1)
            Else
                U = q(i + 1, 0)
                V = q(i + 1, 1)
            End If
            w = w + (q(i, 0) + U) * (V - q(i, 1)) / 2
        Next

        gu_GetArea = Round(Abs(w), 2)
    End Function
    Public Function o_distance(ByVal p1 As Point, ByVal p2 As Point) As Single
        Return ((p1.X - p2.X) ^ 2 + (p1.Y - p2.Y) ^ 2) ^ 0.5
    End Function
    Public Function DEC_to_HEX(ByVal Dec As Long) As String
        Dim a As String
        Dim result As String = ""
        Do While Dec > 0
            a = CStr(Dec Mod 16)
            Select Case a
                Case "10" : a = "A"
                Case "11" : a = "B"
                Case "12" : a = "C"
                Case "13" : a = "D"
                Case "14" : a = "E"
                Case "15" : a = "F"
            End Select
            result = a & result
            Dec = Dec \ 16
        Loop
        result = ("00" + result).Substring(("00" + result).Length - 2)
        Return result
    End Function
    Public Function in_range(ByVal test_num As Single, ByVal x1 As Single, ByVal x2 As Single) As Boolean
        Dim r As Boolean = False
        test_num = CInt(test_num)
        If x1 < x2 Then
            If test_num > x1 And test_num < x2 Then
                r = True
            End If
        Else
            If test_num > x2 And test_num < x1 Then
                r = True
            End If
        End If
        Return r
    End Function
    Public Function is_cross(ByVal p1 As Point, ByVal p2 As Point, ByVal p3 As Point, ByVal p4 As Point) As Boolean
        Try
            Dim a1 As Single = (p1.Y - p2.Y) / (p1.X - p2.X)
            Dim b1 As Single = (p2.Y * p1.X - p2.X * p1.Y) / (p1.X - p2.X)
            Dim a2 As Single = (p3.Y - p4.Y) / (p3.X - p4.X)
            Dim b2 As Single = (p4.Y * p3.X - p4.X * p3.Y) / (p3.X - p4.X)
            Dim corss_x As Single = (b2 - b1) / (a1 - a2)
            Return in_range(corss_x, p1.X, p2.X) And in_range(corss_x, p3.X, p4.X)
        Catch ex As Exception
            Return True
        End Try

    End Function
    Public Function mak_fa(ByVal fext As String, ByVal extid As String, ByVal extanm As String, ByVal extico As String) As Boolean
        On Error GoTo erh
        mak_fa = True
        Dim o As Object
        o = CreateObject("wscript.shell")
        o.regwrite("HKCR\." + fext + "\", extid, "REG_SZ")
        o.regwrite("HKCR\." + fext + "\DefaultIcon\", extico, "REG_SZ")
        o.regwrite("HKCR\." + fext + "\Shell\Open\Command\", extanm + " %1", "REG_SZ")
        Exit Function
erh:
        mak_fa = False
    End Function
    Public Function rem_fa(ByVal fext As String) As Boolean
        On Error GoTo erh
        rem_fa = True
        Dim o As Object
        o = CreateObject("wscript.shell")
        o.regdelete("HKCR\." + fext)
        Exit Function
erh:
        rem_fa = False
    End Function
    Public Class myReverserClass
        Implements IComparer
        ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
        Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer _
           Implements IComparer.Compare
            Return New CaseInsensitiveComparer().Compare(y, x)
        End Function 'IComparer.Compare

    End Class 'myReverserClass
    Public Sub ChromType(ByVal chromdiv As Single, ByVal k As Integer)
        If chromdiv <= 1.01 Then
            chtype(k) = "M"
            typenum(1) = typenum(1) + 1
            If suiarm(Range(k)) > 0 Then
                sat(1) = sat(1) + 1
            End If

        End If
        If chromdiv > 1.01 And chromdiv <= 1.71 Then
            chtype(k) = "m"
            If suiarm(Range(k)) > 0 Then
                sat(2) = sat(2) + 1
            End If

            typenum(2) = typenum(2) + 1
        End If
        If chromdiv > 1.71 And chromdiv <= 3.01 Then
            chtype(k) = "sm"
            If suiarm(Range(k)) > 0 Then
                sat(3) = sat(3) + 1
            End If

            typenum(3) = typenum(3) + 1
        End If
        If chromdiv > 3.01 And chromdiv <= 7.01 Then
            chtype(k) = "st"
            If suiarm(Range(k)) > 0 Then
                sat(4) = sat(4) + 1
            End If

            typenum(4) = typenum(4) + 1
        End If
        If chromdiv > 7.01 Then
            chtype(k) = "t"
            If suiarm(Range(k)) > 0 Then
                sat(5) = sat(5) + 1
            End If

            typenum(5) = typenum(5) + 1
        End If
        If shortarm(k) = 0 Then
            chtype(k) = "T"
            If suiarm(Range(k)) > 0 Then
                sat(6) = sat(6) + 1
            End If
            typenum(6) = typenum(6) + 1
        End If
    End Sub
    Public Function Get_chromo_type(ByVal chromdiv As Single, ByVal shortarm As Single, ByVal sat_num As Integer) As String
        If shortarm = 0 Then
            typenum(6) = typenum(6) + 1
            If sat_num > 0 Then
                sat(6) = sat(6) + 1
            End If
            Return "T"
        Else
            If chromdiv <= 1.01 Then
                If sat_num > 0 Then
                    sat(1) = sat(1) + 1
                End If
                typenum(1) = typenum(1) + 1
                Return "M"
            End If
            If chromdiv > 1.01 And chromdiv <= 1.71 Then
                If sat_num > 0 Then
                    sat(2) = sat(2) + 1
                End If
                typenum(2) = typenum(2) + 1
                Return "m"

            End If
            If chromdiv > 1.71 And chromdiv <= 3.01 Then
                If sat_num > 0 Then
                    sat(3) = sat(3) + 1
                End If
                typenum(3) = typenum(3) + 1
                Return "sm"

            End If
            If chromdiv > 3.01 And chromdiv <= 7.01 Then
                If sat_num > 0 Then
                    sat(4) = sat(4) + 1
                End If
                typenum(4) = typenum(4) + 1
                Return "st"
            End If
            If sat_num > 0 Then
                sat(5) = sat(5) + 1
            End If
            typenum(5) = typenum(5) + 1
            Return "t"
        End If
    End Function
    Public Sub clear_result()
        data_count = 1
        last_type = 0
        'scale_size = 100
        'scale_unit = "unit"
        For i As Integer = 0 To 2048
            bchrom_list(i) = False
            points_group(i) = New my_points(points_group_1, 0, 0, 0, points_group_2, 0, 0, 0)
            Range(i) = i
            data_id(i) = i
            longarm(i) = -1
            shortarm(i) = -1
            suiarm(i) = 0
            his_point(i).X = 0
            his_point(i).Y = 0
        Next

        sumarm = 0
        summod = 0
    End Sub
    Public Sub ReRange()
        Dim armsumx() As Single
        Dim armsumx1() As Single
        Dim Rangex() As Single
        ReDim armsumx(data_count + 1), armsumx1(times + 1), Rangex(times + 1)
        For k As Integer = 1 To (data_count - bchrom) / times
            For i As Integer = 1 To times
                For j As Integer = 1 To times
                    armsumx((k - 1) * times + i) = armsumx((k - 1) * times + i) + longarm(Range((k - 1) * times + j)) + shortarm(Range((k - 1) * times + j))
                Next

            Next
        Next
        Array.Sort(armsumx, Range, 1, data_count - bchrom, upper)
        For k As Integer = 1 To (data_count - bchrom) / times
            For i As Integer = 1 To times
                armsumx1(i) = longarm(Range((k - 1) * times + i)) + shortarm(Range((k - 1) * times + i))
                Rangex(i) = Range((k - 1) * times + i)
            Next
            Array.Sort(armsumx1, Rangex, 1, times, upper)
            For i As Integer = 1 To times
                Range((k - 1) * times + i) = Rangex(i)
            Next
        Next
    End Sub
    Public Sub Init()
        last_count = node_count
        For i = 0 To 2048
            If line_point(i).X > 0 Or line_point(i).Y > 0 Then
                line_point(i) = New Point(0, 0)
            ElseIf i > node_count + 1 Then
                Exit For
            End If
        Next
        node_count = -1
        in_meansure = True
        MDown2Y = 0
        MDown1Y = 0
        MDown2X = 0
        MDown1X = 0
        armlongth = 0
    End Sub

    Public Sub info()
        Try
            Dim temp() As Single
            Dim templ() As Single
            Dim templ_sort() As Integer
            Dim temps_sort() As Integer
            Dim temps() As Single
            Dim temp_sum() As Single
            Dim Above2 As Integer = 0
            Dim nh As Single = (data_count - bchrom) / times 'number of homologous chromosome pairs or groups 
            ReDim temp(data_count - bchrom), temps(data_count - bchrom), templ(data_count - bchrom), temp_sum(data_count - bchrom), temps_sort(data_count - bchrom), templ_sort(data_count - bchrom)
            For i As Integer = 1 To 6
                sat(i) = 0
                typenum(i) = 0
            Next
            sumarm = 1000
            summod = 1000
            For k As Integer = 1 To (data_count - bchrom) / times
                For i As Integer = 1 To times
                    temps_sort((k - 1) * times + i) = (k - 1) * times + i
                    templ_sort((k - 1) * times + i) = (k - 1) * times + i
                    For j As Integer = 1 To times
                        temp((k - 1) * times + i) = temp((k - 1) * times + i) + longarm(Range((k - 1) * times + j)) / (shortarm(Range((k - 1) * times + j) + 0.0001))
                        templ((k - 1) * times + i) = templ((k - 1) * times + i) + longarm(Range((k - 1) * times + j))
                        temps((k - 1) * times + i) = temps((k - 1) * times + i) + shortarm(Range((k - 1) * times + j))
                        temp_sum((k - 1) * times + i) = temp_sum((k - 1) * times + i) + shortarm(Range((k - 1) * times + j)) + longarm(Range((k - 1) * times + j))
                    Next
                    temp((k - 1) * times + i) = temp((k - 1) * times + i) / times
                    temps((k - 1) * times + i) = temps((k - 1) * times + i) / times
                    templ((k - 1) * times + i) = templ((k - 1) * times + i) / times
                    temp_sum((k - 1) * times + i) = temp_sum((k - 1) * times + i) / times
                    sumarm = sumarm + longarm(Range((k - 1) * times + i)) + shortarm(Range((k - 1) * times + i))
                    If shortarm(Range((k - 1) * times + i)) > 0.1 Then
                        summod = summod + longarm(Range((k - 1) * times + i)) / shortarm(Range((k - 1) * times + i))
                    End If
                    armplus((k - 1) * times + i) = longarm(Range((k - 1) * times + i)) + shortarm(Range((k - 1) * times + i))
                    armdivide((k - 1) * times + i) = longarm(Range((k - 1) * times + i)) / (shortarm(Range((k - 1) * times + i)) + 0.0001)
                Next
            Next
            sumarm = sumarm - 1000
            summod = summod - 1000
            Dim sumlongarm As Single = 0
            For k As Integer = 1 To data_count - bchrom
                sumlongarm = sumlongarm + templ(k)
                If armdivide(k) > 2 Then
                    Above2 = Above2 + 1
                End If
            Next
            Dim A1 As Single, A2 As Single, A3 As Single
            For k As Integer = 1 To data_count - bchrom Step times
                A1 = A1 + temps(k) / templ(k)
                A3 = A3 + (templ(k) - temps(k)) / (templ(k) + temps(k))
            Next
            A1 = 1 - A1 / nh
            A3 = A3 / nh
            Array.Sort(armplus, 1, data_count - bchrom)

            Dim avx As Single = 0 'avx为染色体长度的平均值
            Dim asv As Single = 0 '长度的标准差
            Dim avm As Single = 0 'Average of S/(L+S)
            Dim asm As Single = 0 'SD of S/(L+S)
            Dim mca As Single = 0 'MCA (L-S)/(L+S)

            avx = sumarm / data_count

            For k As Integer = 1 To (data_count - bchrom) Step times
                asv += ((templ(k) + temps(k)) - avx) ^ 2
            Next
            asv = (asv / ((data_count - bchrom) / times - 1)) ^ 0.5

            For k As Integer = 1 To (data_count - bchrom) Step times
                mca += (templ(k) - temps(k)) / (templ(k) + temps(k))
            Next
            mca = mca / (data_count - bchrom) * times * 100
            A2 = asv / avx

            For k As Integer = 1 To (data_count - bchrom) Step times
                avm += temps(k) / (templ(k) + temps(k))
            Next
            avm = avm / (data_count - bchrom) * times * 100
            For k As Integer = 1 To (data_count - bchrom) Step times
                asm = (temps(k) / (templ(k) + temps(k)) * 100 - avm) ^ 2 + asm
            Next
            asm = (asm / ((data_count - bchrom) / times - 1)) ^ 0.5

            Array.Sort(temps, temps_sort)
            Array.Sort(templ, templ_sort)
            Dim median_sum, median_short As Single
            If (data_count - bchrom) Mod 2 = 1 Then
                median_short = temps((data_count - bchrom + 1) / 2) / times
                median_sum = temp_sum((data_count - bchrom + 1) / 2) / times
            Else
                median_short = (temps(data_count / 2) + temps(data_count / 2 + 1)) / 2 / times
                median_sum = (temp_sum(data_count / 2) + temp_sum(data_count / 2 + 1)) / 2 / times
            End If
            Dim CG, CV, DI, CI, AI As Single
            CG = median_short / median_sum * 100
            CV = A2 * 100
            CI = asm / avm * 100
            DI = CG * CV / 100
            AI = CV * CI / 100

            Analysisform.RichTextBox1.Text = "Karyotype asymmetry degree (Stebbins, 1971): "
            If Above2 / (data_count - bchrom) < 0.01 Then
                Analysisform.RichTextBox1.AppendText("1")
            Else
                If Above2 / (data_count - bchrom) < 0.5 Then
                    Analysisform.RichTextBox1.AppendText("2")
                Else
                    If Above2 / (data_count - bchrom) < 0.99 Then
                        Analysisform.RichTextBox1.AppendText("3")
                    Else
                        Analysisform.RichTextBox1.AppendText("4")
                    End If
                End If
            End If

            If armplus(data_count - bchrom) / armplus(1) < 2 Then
                Analysisform.RichTextBox1.AppendText("A" + Chr(13))
            Else
                If armplus(data_count - bchrom) / armplus(1) <= 4 Then
                    Analysisform.RichTextBox1.AppendText("B" + Chr(13))
                Else
                    Analysisform.RichTextBox1.AppendText("C" + Chr(13))
                End If
            End If



            For k As Integer = 1 To data_count
                ChromType(longarm(k) / (shortarm(k) + 0.0001), k)
            Next

            Analysisform.RichTextBox1.AppendText("Karyotype formula (Levan et al., 1964): " + "2n = " + Str(times) + "x = ")
            For k As Integer = 1 To 6
                If typenum(k) > 0 Then
                    If sat(k) > 0 Then
                        Analysisform.RichTextBox1.AppendText(Str(typenum(k)) + typestr(k) + "(" + Str(sat(k)).Replace(" ", "") + "sat)" + " +")
                    Else
                        Analysisform.RichTextBox1.AppendText(Str(typenum(k)) + typestr(k) + " +")
                    End If
                End If
            Next
            If Analysisform.RichTextBox1.Text.EndsWith("+") Then
                Analysisform.RichTextBox1.Text = Analysisform.RichTextBox1.Text.Remove(Analysisform.RichTextBox1.Text.Length - 1, 1)
            End If
            Array.Sort(temps_sort, temps)
            Array.Sort(templ_sort, templ)

            Analysisform.RichTextBox1.AppendText(Chr(13) + "Original Table (L: long arms; S: short arms; 0: no satellite; 1: on the long arm; 2: on the short arm; 3: intercalary satellite)" + Chr(13) + "ID	L	S	L+S	L-S	L/S	Group ID	Satellites" + Chr(13))
            For k As Integer = 1 To data_count
                Analysisform.RichTextBox1.AppendText(k.ToString + "	" + (longarm(k)).ToString("F2") + "	" + (shortarm(k)).ToString("F2") + "	" + (longarm(k) + shortarm(k)).ToString("F2") + "	" + (longarm(k) - shortarm(k)).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((longarm(k) / (shortarm(k) + 0.0001)).ToString("F2") + "	" + (((Array.IndexOf(Range, k) - 1) - (Array.IndexOf(Range, k) - 1) Mod times) / times + 1).ToString("F0") + "	" + suiarm(k).ToString("F0") + Chr(13))
            Next


            Analysisform.RichTextBox1.AppendText(Chr(13) + "Haploid Table")
            Analysisform.RichTextBox1.AppendText(Chr(13) + "Group ID	L	S	L+S	L-S	S/L+S(%)	L/S	" + Chr(13))
            For k As Integer = 1 To (data_count - bchrom) / times
                Analysisform.RichTextBox1.AppendText(Str(k) + "	" + (templ(k * times)).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((temps(k * times)).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((temps(k * times) + templ(k * times)).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((templ(k * times) - temps(k * times)).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((temps(k * times) / (templ(k * times) + temps(k * times)) * 100).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((templ(k * times) / (temps(k * times) + 0.0001)).ToString("F2") + Chr(13))
            Next

            Analysisform.RichTextBox1.AppendText(Chr(13) + "Karyotype parameters table")
            Analysisform.RichTextBox1.AppendText(Chr(13) + "x	2n	THL	CVCI	CVCL	MCA" + Chr(13))
            Analysisform.RichTextBox1.AppendText(((data_count - bchrom) / times).ToString + "	" + (data_count - bchrom).ToString + "	" + (sumarm / times).ToString("F2") + "	" + CI.ToString("F2") + "	" + CV.ToString("F2") + "	" + mca.ToString("F2") + "	" + Chr(13))


            Analysisform.RichTextBox1.AppendText(Chr(13) + "The relative length")
            Analysisform.RichTextBox1.AppendText(Chr(13) + "Group	" + "L(%)	S(%)	L+S (%)	" + "L/S	" + "Type" + Chr(13))
            For k As Integer = 1 To (data_count - bchrom) / times
                Analysisform.RichTextBox1.AppendText(Str(k) + "	")
                Analysisform.RichTextBox1.AppendText((templ(k * times) * times / sumarm * 100).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((temps(k * times) * times * 100 / sumarm).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((templ(k * times) / sumarm * times * 100).ToString("F2") + "+" + (temps(k * times) * 100 * times / sumarm).ToString("F2") + "=" + (temps(k * times) * 100 * times / sumarm + templ(k * times) * 100 * times / sumarm).ToString("F2") + "	")
                Analysisform.RichTextBox1.AppendText((templ(k * times) / (temps(k * times) + 0.0001)).ToString("F2") + "	" + chtype(k * times) + Chr(13))
            Next

            Analysisform.RichTextBox1.AppendText(Chr(13) + "ID	" + "L(%)	S(%)	L+S (%)	" + "L/S	" + "Type" + Chr(13))
            For k As Integer = 1 To data_count
                Analysisform.RichTextBox1.AppendText(k.ToString + "	" + (longarm(k) / sumarm * 100).ToString("F2") + "	" + (shortarm(k) * 100 / sumarm).ToString("F2") + "	" + (longarm(k) / sumarm * 100).ToString("F2") + "+" + (shortarm(k) * 100 / sumarm).ToString("F2") + "=" + ((longarm(k) + shortarm(k)) * 100 / sumarm).ToString("F2") + "	" + (longarm(k) / (shortarm(k) + 0.0001)).ToString("F2") + "	" + chtype(k) + Chr(13))
            Next

            Analysisform.RichTextBox1.AppendText(Chr(13) + "Other Information" + Chr(13))
            Analysisform.RichTextBox1.AppendText("longest/shortest = " + (armplus(data_count - bchrom) / armplus(1)).ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("The total haploid length of the chromosome set (Peruzzi et al., 2009), THL = " + (sumarm / times).ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("Coefficient of Variation of Centromeric Index (Paszko, 2006), CVCI = " + CI.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("Coefficient of Variation of Chromosome Length (Paszko, 2006), CVCL = " + CV.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("Mean Centromeric Asymmetry (Peruzzi and Eroglu, 2013), MCA = " + mca.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("-------------------------------------------------------------" + Chr(13))
            Analysisform.RichTextBox1.AppendText("Number of chromosome which (long arm/short arm) > 2: " + Str(Above2) + " (" + (Above2 / (data_count - bchrom) * 100).ToString("F2") + "%)" + Chr(13))
            '变异系数(coefficient of variation)
            Analysisform.RichTextBox1.AppendText("The Karyotype asymmetry index (Arano, 1963), AsK% = " + (sumlongarm / sumarm * 100).ToString("F2") + "%" + Chr(13))
            Analysisform.RichTextBox1.AppendText("The total form percent (Huziwara, 1962), TF% = " + ((1 - sumlongarm / sumarm) * 100).ToString("F2") + "%" + Chr(13))
            Analysisform.RichTextBox1.AppendText("The index of Karyotype symmetry (Greilhuber and Speta, 1976), Syi = " + (((sumarm - sumlongarm) / sumlongarm) * 100).ToString("F2") + "%" + Chr(13))
            Analysisform.RichTextBox1.AppendText("The index of chromosomal size resemblance (Greilhuber and Speta, 1976), Rec = " + ((sumarm / armplus(data_count - bchrom) / (data_count - bchrom)) * 100).ToString("F2") + "%" + Chr(13))
            Analysisform.RichTextBox1.AppendText("The intra chromosomal asymmetry index (Romero Zarco, 1986), A1 = " + A1.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("The inter chromosomal asymmetry index( Romero Zarco, 1986), A2 = " + A2.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("The degree of asymmetry of Karyotype (Watanabe et al., 1999), A = " + A3.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("The dispersion index (Lavania and Srivastava, 1992), DI = " + DI.ToString("F2") + Chr(13))
            Analysisform.RichTextBox1.AppendText("The asymmetry index (Paszko, 2006), AI = " + AI.ToString("F2") + Chr(13))

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Public Function cal_sd(ByVal single_array() As Single) As Single
        Dim u As Single = 0
        Dim m As Single = 0
        Dim n As Integer = single_array.Length
        For Each i As Single In single_array
            m += i
        Next
        m = m / n
        For Each i As Single In single_array
            u += (i - m) ^ 2
        Next
        u = (u / (n - 1)) ^ 0.5
        Return u
    End Function
    Function paintcyc(ByVal k() As Integer) As Single
        Dim x(), y(), sum As Single
        ReDim x(8), y(8)
        For j As Integer = 1 To 4
            x(j) = (longarm(k(j)) + shortarm(k(j))) / sumarm * 100
            y(j) = (longarm(k(j)) / (shortarm(k(j)) + 0.0001) / summod) * 100 * radio_div_len
        Next
        Array.Sort(x, 1, 4)
        Array.Sort(y, 1, 4)

        sum = (x(4) - x(1)) * (y(4) - y(1))
        For j As Integer = 1 To 4
            x(j) = (longarm(k(4 + j)) + shortarm(k(4 + j))) / sumarm * 100
            y(j) = (longarm(k(4 + j)) / (shortarm(k(4 + j)) + 0.0001) / summod) * 100 * radio_div_len
        Next
        Array.Sort(x, 1, 4)
        Array.Sort(y, 1, 4)
        sum = sum + (x(4) - x(1)) * (y(4) - y(1))
        Return sum
    End Function


    Private Sub Cyc3()
        Dim a, b, c, n As Integer
        Dim x(8) As Integer
        n = 0
        '循环开始
        For a = 1 To 4
            If a = 1 Then
                x(1) = a
                For b = a + 1 To 6
                    x(2) = b
                    For c = b + 1 To 6
                        x(3) = c
                        'For d = c + 1 To 6
                        'x(4) = d
                        n = n + 1
                        For i As Integer = 1 To 4
                            cycle(n, i) = x(i)
                        Next
                        'Next
                    Next
                Next
            Else
                x(4) = a
                For b = a + 1 To 6
                    x(5) = b
                    For c = b + 1 To 6
                        x(6) = c
                        'For d = c + 1 To 6
                        'x(8) = d
                        For i As Integer = 4 To 6
                            cycle(20 - n, i) = x(i)
                        Next
                        n = n + 1
                        'Next
                    Next
                Next
            End If
        Next
    End Sub
    Public Sub xAnaly()
        Dim x(8) As Integer
        Dim y(8) As Integer
        Dim cycrange(35) As Single
        Dim cycID(35) As Integer
        Cyc4()
        For k As Integer = 1 To data_count / times - 1

            For i As Integer = 1 To 35
                For j As Integer = 1 To 8
                    x(j) = cycle(i, j)
                    x(j) = Range((k - 1) * times + x(j))
                Next
                cycrange(i) = paintcyc(x)
                cycID(i) = i
            Next
            Array.Sort(cycrange, cycID, 1, 35)
            For j As Integer = 1 To 8
                y(j) = Range(cycle(cycID(1), j) + (k - 1) * times)
            Next
            For j As Integer = 1 To 8
                Range((k - 1) * times + j) = y(j)
            Next
        Next

    End Sub
    Public Sub Cyc4()
        Dim a, b, c, d, n As Integer
        Dim x(8) As Integer
        n = 0
        '循环开始
        For a = 1 To 5
            If a = 1 Then
                x(1) = a
                For b = a + 1 To 8
                    x(2) = b
                    For c = b + 1 To 8
                        x(3) = c
                        For d = c + 1 To 8
                            x(4) = d
                            n = n + 1
                            For i As Integer = 1 To 4
                                cycle(n, i) = x(i)
                            Next
                        Next
                    Next
                Next
            Else
                x(5) = a
                For b = a + 1 To 8
                    x(6) = b
                    For c = b + 1 To 8
                        x(7) = c
                        For d = c + 1 To 8
                            x(8) = d
                            For i As Integer = 5 To 8
                                cycle(70 - n, i) = x(i)
                            Next
                            n = n + 1
                        Next
                    Next
                Next
            End If
        Next

    End Sub

End Module
