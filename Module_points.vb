Module Module_points
    Class my_points
        Public points_group_1(2048) As Point
        Public points_type_1 As Integer
        Public points_count_1 As Integer
        Public points_value_1 As Single
        Public points_group_2(2048) As Point
        Public points_type_2 As Integer
        Public points_count_2 As Integer
        Public points_value_2 As Single
        Sub New(ByVal new_points_1() As Point, ByVal points_type_1 As Integer,
                ByVal points_count_1 As Integer, ByVal points_value_1 As Single,
                ByVal new_points_2() As Point, ByVal points_type_2 As Integer,
                 ByVal points_count_2 As Integer, ByVal points_value_2 As Single)
            Me.points_group_1 = new_points_1.Clone
            Me.points_type_1 = points_type_1
            Me.points_count_1 = points_count_1
            Me.points_value_1 = points_value_1
            Me.points_group_2 = new_points_2.Clone
            Me.points_type_2 = points_type_2
            Me.points_count_2 = points_count_2
            Me.points_value_2 = points_value_2
        End Sub
    End Class
End Module
