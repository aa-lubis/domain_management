Imports System.ComponentModel.DataAnnotations

Namespace Entities

    Public Class Role

        <Key> _
        <MaxLength(2)> _
        Public Property RoleId As String

        <MaxLength(20)> _
        Public Property RoleName As String

        <MaxLength(50)> _
        Public Property RoleDescription As String

    End Class

End Namespace
