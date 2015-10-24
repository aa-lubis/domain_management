Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class IdentityType

        <Key> _
        <MaxLength(2)> _
        Public Property IdentityTypeID As String

        <MaxLength(20)> _
        Public Property IdentityTypeDesc As String

    End Class

End Namespace
