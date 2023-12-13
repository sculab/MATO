Imports System.IO
Imports System.Data
Module Module_ml
    Public Training_dtView As New DataView
    Public Training_Dataset As New DataSet
    Public Predict_dtView As New DataView
    Public Predict_Dataset As New DataSet
    Public setting_maxtrix(,) As String
    Public feature_name() As String
    Public feature_string() As Boolean
    Public feature_count As Integer
    Public rows_count As Integer = 0
    Public have_header As Integer = 0
    Public target_name As String
    Public target_table As Hashtable
    Public target_table_name As Hashtable
    Public path_orginal As String = ""
    Public path_clean As String = ""
    Public path_setting As String = ""
    Public path_model As String = ""
    Public path_model_info As String = ""
    Public path_data_format As String = ""
    Public learning_model As String = ""

    Public Sub load_setting(ByVal setting_path As String)
        Dim sr As New StreamReader(setting_path)
        Dim header_str() As String = sr.ReadLine.Split(",")
        have_header = header_str(1)
        feature_count = header_str(2)
        rows_count = header_str(3)
        learning_model = header_str(4)
        ReDim setting_maxtrix(feature_count, 5)
        For i As Integer = 0 To feature_count
            Dim temp_str() As String = sr.ReadLine.Split(",")
            For j As Integer = 0 To 5
                setting_maxtrix(i, j) = temp_str(j)
                If setting_maxtrix(i, 5) = "1" Then
                    target_name = setting_maxtrix(i, 0)
                End If
            Next
        Next
        sr.Close()
    End Sub
    Public Function clean_data(ByVal input_file As String, ByVal output_file As String, ByVal data_format_file As String, ByVal load_format As Boolean) As Integer
        clean_data = 0
        Dim line As String = ""
        Dim new_line As String = ""
        Dim str_features() As Object
        ReDim str_features(feature_count)
        If load_format Then
            Dim sr As New StreamReader(data_format_file)
            Dim temp_str() As String
            For i As Integer = 0 To feature_count
                Dim ht As Hashtable = New Hashtable()
                str_features(i) = ht
                temp_str = sr.ReadLine.Split(";")
                If UBound(temp_str) > 1 Then
                    For j = 0 To UBound(temp_str) Step 2
                        str_features(i)(temp_str(j)) = temp_str(j + 1)
                    Next
                End If
                If setting_maxtrix(i, 5) = "1" Then
                    target_table = str_features(i).Clone()
                    target_table_name = New Hashtable()
                    For Each key In target_table.Keys
                        target_table_name(target_table(key)) = key
                    Next

                End If
            Next
            sr.Close()
        Else
            Dim sr_1 As New StreamReader(input_file)
            If have_header = 1 Then
                sr_1.ReadLine()
            End If
            line = sr_1.ReadLine()

            For i As Integer = 0 To feature_count
                Dim ht As Hashtable = New Hashtable()
                str_features(i) = ht
                If setting_maxtrix(i, 4) = "1" Then
                    str_features(i)("sum") = 0
                    str_features(i)("count") = 0
                End If
            Next
            Do
                If line <> "" Then
                    Dim temp_str() As String = line.Split(",")
                    For i As Integer = 0 To feature_count
                        If temp_str(i) <> "" Then
                            If setting_maxtrix(i, 1) = "1" And setting_maxtrix(i, 5) = "0" Then
                                If setting_maxtrix(i, 3) = "1" Then
                                    If temp_str(i).Contains("/") Then
                                        For Each j In temp_str(i).Split("/")
                                            If j <> "" Then
                                                str_features(i)(j) = ""
                                            End If
                                        Next
                                        str_features(i)(temp_str(i)) = ""
                                    Else
                                        str_features(i)(temp_str(i)) = ""
                                    End If
                                Else

                                    str_features(i)("sum") = str_features(i)("sum") + CSng(temp_str(i))
                                    str_features(i)("count") = str_features(i)("count") + 1

                                End If
                            ElseIf setting_maxtrix(i, 5) = "1" Then
                                If setting_maxtrix(i, 3) = "1" Then
                                    If temp_str(i).Contains("/") Then
                                        For Each j In temp_str(i).Split("/")
                                            If j <> "" Then
                                                str_features(i)(j) = ""
                                            End If
                                        Next
                                    Else
                                        str_features(i)(temp_str(i)) = ""
                                    End If
                                Else
                                    If temp_str(i) <> "" Then
                                        str_features(i)("sum") = str_features(i)("sum") + CSng(temp_str(i))
                                        str_features(i)("count") = str_features(i)("count") + 1
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                line = sr_1.ReadLine()
            Loop Until line Is Nothing
            sr_1.Close()
            'mutiply-one-hot encoding
            For i As Integer = 0 To feature_count
                If setting_maxtrix(i, 3) = "1" And setting_maxtrix(i, 5) = "0" Then
                    Dim temp_ht_1 As Hashtable = New Hashtable()
                    For Each my_key In str_features(i).Keys
                        If Not my_key.ToString.Contains("/") Then
                            temp_ht_1(my_key) = ""
                        End If
                    Next
                    Dim temp_ht_2 As Hashtable = New Hashtable()
                    For Each my_key In str_features(i).Keys

                        Dim temp_line As String = ""
                        For k = 1 To temp_ht_1.Count
                            If my_key.ToString.Contains("/") Then
                                If Array.IndexOf(my_key.ToString.Split("/"), temp_ht_1.Item(k - 1)) >= 0 Then
                                    temp_line += "1,"
                                Else
                                    temp_line += "0,"
                                End If
                            Else
                                If my_key = temp_ht_1.Item(k - 1) Then
                                    temp_line += "1,"
                                Else
                                    temp_line += "0,"
                                End If
                            End If

                        Next
                        temp_ht_2(my_key) = temp_line
                    Next
                    str_features(i) = temp_ht_2
                ElseIf setting_maxtrix(i, 5) = "1" Then
                    Dim temp_ht_1 As Hashtable = New Hashtable()
                    Dim is_string As Boolean = False
                    Dim is_int As Boolean = True
                    For Each my_key In str_features(i).Keys
                        If IsNumeric(my_key) = False Then
                            is_string = True
                            Exit For
                        End If
                    Next
                    If is_string Then
                        For Each my_key In str_features(i).Keys
                            If Not my_key.ToString.Contains("/") Then
                                temp_ht_1(my_key) = temp_ht_1.Count
                            End If
                        Next
                        str_features(i) = temp_ht_1
                    Else
                        For Each my_key In str_features(i).Keys
                            If Not my_key.ToString.Contains("/") Then
                                temp_ht_1(my_key) = my_key + ","
                            End If
                        Next
                        str_features(i) = temp_ht_1
                    End If
                    target_table = str_features(i).Clone()
                    target_table_name = New Hashtable()
                    For Each key In target_table.Keys
                        target_table_name(target_table(key)) = key
                    Next
                End If
            Next
        End If



        Dim sw As New StreamWriter(output_file)
        Dim sr_2 As New StreamReader(input_file)
        If have_header = 1 Then
            sr_2.ReadLine()
        End If
        line = sr_2.ReadLine()
        'write header
        If load_format Then
            For i As Integer = 0 To feature_count
                If setting_maxtrix(i, 1) = "1" And setting_maxtrix(i, 5) = "0" Then
                    If setting_maxtrix(i, 3) = "1" Then
                        For Each j In str_features(i).Keys
                            If Not j.ToString.Contains("/") Then
                                new_line += setting_maxtrix(i, 0) + "_" + j.ToString + ","
                            End If
                        Next
                    Else
                        new_line += setting_maxtrix(i, 0) + ","
                    End If
                End If
            Next
        Else
            For i As Integer = 0 To feature_count
                If setting_maxtrix(i, 1) = "1" Or setting_maxtrix(i, 5) = "1" Then
                    If setting_maxtrix(i, 3) = "1" And setting_maxtrix(i, 5) = "0" Then
                        For Each j In str_features(i).Keys
                            If Not j.ToString.Contains("/") Then
                                new_line += setting_maxtrix(i, 0) + "_" + j.ToString + ","
                            End If
                        Next
                    Else
                        new_line += setting_maxtrix(i, 0) + ","
                    End If
                End If
            Next
        End If
        sw.WriteLine(new_line.Remove(new_line.Length - 1))
        'write data
        Do
            If line <> "" Then
                new_line = ""
                Dim temp_str() As String = line.Split(",")
                If load_format Then
                    For i As Integer = 0 To feature_count
                        If setting_maxtrix(i, 1) = "1" And setting_maxtrix(i, 5) = "0" Then
                            If temp_str(i) <> "" Then
                                If setting_maxtrix(i, 3) = "1" Then
                                    new_line += str_features(i)(temp_str(i)).ToString
                                Else
                                    new_line += temp_str(i).ToString + ","
                                End If
                            Else
                                new_line = ""
                                Exit For
                            End If
                        End If
                    Next
                Else
                    For i As Integer = 0 To feature_count
                        If temp_str(i) <> "" Then
                            If setting_maxtrix(i, 1) = "1" Or setting_maxtrix(i, 5) = "1" Then
                                If setting_maxtrix(i, 3) = "1" Then
                                    new_line += str_features(i)(temp_str(i)).ToString + ","
                                Else
                                    new_line += temp_str(i).ToString + ","
                                End If
                            End If
                        Else
                            new_line = ""
                            Exit For
                        End If
                    Next
                End If
                If new_line <> "" Then
                    sw.WriteLine(new_line.Remove(new_line.Length - 1))
                    clean_data += 1
                End If
            End If
            line = sr_2.ReadLine()
        Loop Until line Is Nothing
        sr_2.Close()
        sw.Close()
        'write data format
        If load_format = False Then
            Dim sw_df As New StreamWriter(path_data_format)
            For i As Integer = 0 To feature_count
                new_line = ""
                For Each my_key In str_features(i).Keys
                    new_line += my_key.ToString + ";" + str_features(i)(my_key).ToString + ";"
                Next
                If new_line <> "" Then
                    sw_df.WriteLine(new_line.Remove(new_line.Length - 1))
                Else
                    sw_df.WriteLine("")
                End If

            Next
            sw_df.Close()
        End If
    End Function

End Module
