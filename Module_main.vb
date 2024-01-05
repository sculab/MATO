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
End Module
