Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class UserRole

        <Key> _
        <Column(Order:=0)> _
        <MaxLength(15)> _
        Public Property UserId As String

        <Key> _
        <Column(Order:=1)> _
        <MaxLength(2)> _
        Public Property RoleId As String

        Public Overridable Property Role As Role

    End Class

End Namespace

