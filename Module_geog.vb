Module Module_geog


End Module
Public Class scomparer
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Return New CaseInsensitiveComparer().Compare(y, x)
    End Function
End Class
Public Class lcomparer
    Implements IComparer
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
        Return New CaseInsensitiveComparer().Compare(x, y)
    End Function
End Class