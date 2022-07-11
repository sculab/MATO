Imports System.IO
Module Module_main
#Const TargetOS = "win32"
#If TargetOS = "linux" Then
    Public TargetOS As String = "linux"
#ElseIf TargetOS = "macos" Then
    Public TargetOS As String = "macos"
#ElseIf TargetOS = "win32" Then
	Public TargetOS As String = "win32"
#End If
	Public meansureform As New Form_meansure
	Public Analysisform As New Form_analysis
	Public Aboutform As New Form_about
	Public ScaleForm As New Form_scale
	Public WelForm As New Welcome
    'Public GeoForm As New Form_geog

    'Public ci As Globalization.CultureInfo = New Globalization.CultureInfo("en-us")
    'Public path_char As String
    'Public root_path As String
    'Public lib_path As String
    'Public Dec_Sym As String
    'Public Sub format_path()
    '	Select Case TargetOS
    '		Case "linux"
    '			path_char = "/"
    '		Case "win32", "macos"
    '			path_char = "\"
    '		Case Else
    '			path_char = "\"
    '	End Select
    '	root_path = (Application.StartupPath + path_char).Replace(path_char + path_char, path_char)
    '	Dec_Sym = CInt("0").ToString("F1").Replace("0", "")
    '	If Dec_Sym <> "." Then
    '		MsgBox("Notice: We will use dat (.) as decimal quotation instead of comma (,). We recommand to change your system's number format to English! ")
    '	End If
    'End Sub
End Module
