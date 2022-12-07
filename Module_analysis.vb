Module Module_analysis
    Public Function min(ByVal s1 As Single, ByVal s2 As Single) As Single
        If s1 > s2 Then
            Return s2
        Else
            Return s1
        End If
    End Function
    Public Function max(ByVal s1 As Single, ByVal s2 As Single) As Single
        If s1 < s2 Then
            Return s2
        Else
            Return s1
        End If
    End Function
    Public Sub Analyse2(ByVal chromnum As Integer)
        If chromnum > 4 Then
            Dim r(5) As Integer
            Dim a(5) As Single
            Dim d(5) As Single
            Dim s(5) As Single
            Dim rangex(4) As Single
            Dim rangenum(5) As Integer

            For i As Integer = 1 To chromnum / 2 - 1
                For j As Integer = 1 To 4
                    r(j) = Range((i - 1) * 2 + j)
                    a(j) = (i - 1) * 2 + j
                    d(j) = longarm(r(j)) / (shortarm(r(j)) + 0.0001) / summod * radio_div_len
                    s(j) = (longarm(r(j)) + shortarm(r(j))) / sumarm


                    rangenum(j) = j
                Next
                rangex(1) = ((d(1) - d(2)) ^ 2 + (s(1) - s(2)) ^ 2) ^ 0.5 + ((d(3) - d(4)) ^ 2 + (s(3) - s(4)) ^ 2) ^ 0.5
                rangex(2) = ((d(1) - d(3)) ^ 2 + (s(1) - s(3)) ^ 2) ^ 0.5 + ((d(2) - d(4)) ^ 2 + (s(2) - s(4)) ^ 2) ^ 0.5
                rangex(3) = ((d(1) - d(4)) ^ 2 + (s(1) - s(4)) ^ 2) ^ 0.5 + ((d(2) - d(3)) ^ 2 + (s(2) - s(3)) ^ 2) ^ 0.5
                Array.Sort(rangex, rangenum, 1, 3)
                If rangenum(1) = 2 Then
                    Range(a(3)) = r(2)
                    Range(a(2)) = r(3)
                End If
                If rangenum(1) = 3 Then
                    Range(a(4)) = r(2)
                    Range(a(2)) = r(4)
                End If
            Next
        End If
    End Sub
    Public Sub Analyse1(ByVal Firstid As Integer)
        Dim tempsum() As Single
        Dim tempdiv() As Single
        Dim temparm() As Single

        Dim tempsui(), tempsui1() As Single
        Dim temprange(), temprange1(), temprange2(2048) As Integer
        Dim temp(), temp1() As Single
        ReDim tempsum(data_count + 1), tempdiv(data_count + 1), temparm(data_count + 1), temprange(data_count + 1), temp1(data_count + 1), temp(data_count + 1), temprange1(data_count + 1), tempsui(data_count + 1), tempsui1(data_count + 1)
        '清除b染色体
        bchrom = 0
        suichrom = 0
        For i = 1 To data_count
            If bchrom_list(i) = False Then
                If suiarm(i) <> 0 Then
                    suichrom = suichrom + 1
                End If
            Else
                bchrom = bchrom + 1
            End If
        Next
        If (data_count - bchrom - suichrom) Mod times = 0 Then
            bchrom = 0
            suichrom = 0
            For i As Integer = 1 To data_count
                Dim j As Integer
                If bchrom_list(i) = False Then
                    If suiarm(i) = 0 Then
                        j = i - bchrom - suichrom
                        tempsum(j) = (longarm(i) + shortarm(i)) / sumarm
                        tempdiv(j) = longarm(i) / (shortarm(i) + 0.0001) / summod * radio_div_len
                        'temparm(j) = (tempsum(j) ^ 2 + tempdiv(j) ^ 2) ^ 0.5
                        temparm(j) = (longarm(j) ^ 2 + shortarm(j) ^ 2) ^ 0.5
                        temprange(j) = data_id(i)
                        temprange1(j) = data_id(i)

                        temp(j) = data_id(i)
                    Else
                        suichrom = suichrom + 1
                        tempsui(suichrom) = data_id(i)
                        tempsui1(suichrom) = ((longarm(i) / (shortarm(i) + 0.0001) / summod * radio_div_len) ^ 2 + ((longarm(i) + shortarm(i)) / sumarm) ^ 2) ^ 0.5
                    End If
                Else
                    Range(data_count - bchrom) = i
                    bchrom = bchrom + 1
                End If
            Next
            '有随体的话获取随体
            If suichrom > 0 Then
                Array.Sort(tempsui1, tempsui, 1, suichrom)
                For i As Integer = 1 To suichrom
                    Range(data_count - bchrom - i + 1) = tempsui(i)
                Next
            End If
        ElseIf (data_count - bchrom) Mod times = 0 Then
            bchrom = 0
            For i As Integer = 1 To data_count
                Dim j As Integer
                If bchrom_list(i) = False Then
                    j = i - bchrom
                    tempsum(j) = (longarm(i) + shortarm(i)) / sumarm
                    tempdiv(j) = longarm(i) / (shortarm(i) + 0.0001) / summod * radio_div_len
                    'temparm(j) = (tempsum(j) ^ 2 + tempdiv(j) ^ 2) ^ 0.5
                    temparm(j) = (longarm(j) ^ 2 + shortarm(j) ^ 2) ^ 0.5
                    temprange(j) = data_id(i)
                    temprange1(j) = data_id(i)
                    temp(j) = data_id(i)
                Else
                    Range(data_count - bchrom) = i
                End If
            Next
        Else
            MsgBox("Errors! Please check b chromosomes")
            Exit Sub
        End If
        If (data_count - bchrom - suichrom) Mod times = 0 Then
            '排序算法
            Dim armid As Integer
            data_count = data_count - bchrom - suichrom

            For i As Integer = 1 To data_count / times
                '获取离原点最近的染色体
                Array.Sort(temparm, temprange, 1, data_count, Descending)
                Array.Sort(temparm, temprange, 1, data_count - (i - 1) * times)
                armid = temprange(1)
                Range(1 + (i - 1) * times) = armid

                Dim idrange As Integer = 0
                'MsgBox(armid)
                Do
                    idrange = idrange + 1
                Loop Until temp(idrange) = armid
                armid = idrange
                idrange = 0

                tempsum(armid) = 0
                For j As Integer = 1 To data_count
                    If tempsum(j) = 0 Then
                        temp1(j) = 0
                    Else
                        temp1(j) = ((tempsum(armid) - tempsum(j)) ^ 2 + (tempdiv(armid) - tempdiv(j)) ^ 2) ^ 0.5
                    End If
                Next
                '清空最小值
                temparm(1) = 0

                Array.Sort(temp1, temprange1, 1, data_count)
                Array.Sort(temprange, temparm, 1, data_count)
                For j As Integer = 2 To times
                    armid = temprange1((i - 1) * times + j) '获取距离最近的点

                    Range(j + (i - 1) * times) = armid
                    'MsgBox(armid)
                    '清空temp 
                    Do
                        idrange = idrange + 1
                    Loop Until temp(idrange) = armid
                    armid = idrange
                    idrange = 0

                    temparm(armid) = 0
                    tempsum(armid) = 0
                Next

                For j As Integer = 1 To data_count
                    temprange1(j) = temp(j)
                Next
                '获取第二个点
            Next
            If times = 2 Then
                Do
                    temprange2 = Range
                    Analyse2(data_count)
                Loop Until temprange2 Is Range
            End If
            If times = 4 Then
                Do
                    temprange2 = Range
                    xAnaly()
                Loop Until temprange2 Is Range
            End If
            data_count = data_count + bchrom + suichrom
            ReRange()
        ElseIf (data_count - bchrom) Mod times = 0 Then
            Dim armid As Integer
            data_count = data_count - bchrom

            For i As Integer = 1 To data_count / times
                '获取离原点最近的染色体
                Array.Sort(temparm, temprange, 1, data_count, Descending)
                Array.Sort(temparm, temprange, 1, data_count - (i - 1) * times)
                armid = temprange(1)
                Range(1 + (i - 1) * times) = armid

                Dim idrange As Integer = 0
                'MsgBox(armid)
                Do
                    idrange = idrange + 1
                Loop Until temp(idrange) = armid
                armid = idrange
                idrange = 0

                tempsum(armid) = 0
                For j As Integer = 1 To data_count
                    If tempsum(j) = 0 Then
                        temp1(j) = 0
                    Else
                        temp1(j) = ((tempsum(armid) - tempsum(j)) ^ 2 + (tempdiv(armid) - tempdiv(j)) ^ 2) ^ 0.5
                    End If
                Next
                '清空最小值
                temparm(1) = 0

                Array.Sort(temp1, temprange1, 1, data_count)
                Array.Sort(temprange, temparm, 1, data_count)
                For j As Integer = 2 To times
                    armid = temprange1((i - 1) * times + j) '获取距离最近的点

                    Range(j + (i - 1) * times) = armid
                    'MsgBox(armid)
                    '清空temp 
                    Do
                        idrange = idrange + 1
                    Loop Until temp(idrange) = armid
                    armid = idrange
                    idrange = 0

                    temparm(armid) = 0
                    tempsum(armid) = 0
                Next

                For j As Integer = 1 To data_count
                    temprange1(j) = temp(j)
                Next
                '获取第二个点
            Next
            If times = 2 Then
                Do
                    temprange2 = Range
                    Analyse2(data_count)
                Loop Until temprange2 Is Range
            End If
            If times = 4 Then
                Do
                    temprange2 = Range
                    xAnaly()
                Loop Until temprange2 Is Range
            End If
            data_count = data_count + bchrom
            ReRange()
        Else
            MsgBox("Errors! Please check b chromosomes")
            Exit Sub
        End If
    End Sub


End Module
