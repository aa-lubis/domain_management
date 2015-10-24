Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class ChangePasswordModel

        <Required> _
        <DataType(DataType.Password)> _
        <Display(Name:="Current password")> _
        <MaxLength(20)> _
        Public Property OldPassword As String

        <Required> _
        <StringLength(100, ErrorMessage:="The {0} must be at least {2} characters long", MinimumLength:=6)> _
        <DataType(DataType.Password)> _
        <Display(Name:="New password")> _
        <MaxLength(20)> _
        Public Property NewPassword As String

        <DataType(DataType.Password)> _
        <Display(Name:="Confirm new password")> _
        <Compare("NewPassword", ErrorMessage:="The new password and confirmation password do not match")> _
        <MaxLength(20)> _
        Public Property ConfirmPassword As String

    End Class

    Public Class LogOnModel

        <Required> _
        <Display(Name:="User ID")> _
        <MaxLength(15)> _
        Public Property UserID As String

        <Required> _
        <DataType(DataType.Password)> _
        <Display(Name:="Password")> _
        <MaxLength(20)> _
        Public Property Password As String

        <Display(Name:="Remember me?")> _
        Public Property RememberMe As Boolean

    End Class

    Public Class RegisterModel

        Public Property User As User

        <Required> _
        <StringLength(100, ErrorMessage:="The {0} must be at least {2} characters long", MinimumLength:=6)> _
        <DataType(DataType.Password)> _
        <Display(Name:="Password")> _
        <MaxLength(20)> _
        Public Property Password As String

        <DataType(DataType.Password)> _
        <Display(Name:="Confirm password")> _
        <Compare("Password", ErrorMessage:="The password and confirmation password do not match")> _
        <MaxLength(20)> _
        Public Property ConfirmPassword As String


    End Class

    Public Class ResetPasswordModel

        <Required> _
        <Display(Name:="User ID or email")> _
        <MaxLength(50)> _
        Public Property UserID As String

    End Class

End Namespace

