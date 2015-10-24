Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class NotificationLog

        <Key> _
        <MaxLength(20)> _
        <Display(Name:="ID")> _
        Public Property LogID As String

        <MaxLength(500)> _
        <Display(Name:="Send To")> _
        Public Property SendTo As String

        <MaxLength(500)> _
        <Display(Name:="CC")> _
        Public Property SendCC As String

        <MaxLength(500)> _
        <Display(Name:="BCC")> _
        Public Property SendBCC As String

        <MaxLength(200)> _
        <Display(Name:="Subject")> _
        Public Property Subject As String

        <Column(TypeName:="text")> _
        Public Property Body As String

        <Display(Name:="Log Created time")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMM-yy HH:mm}")> _
        Public Property LogCreateTime As DateTime?

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yy HH:mm}")> _
        Public Property ResendTime As DateTime?

        <Column(TypeName:="text")> _
        Public Property Status As String

    End Class

End Namespace
