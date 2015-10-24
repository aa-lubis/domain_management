Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class Domain

        <Key> _
        <MaxLength(15)> _
        Public Property DomainRegID As String

        <Display(Name:="Domain")> _
        <RegularExpression("^(([a-zA-Z]{1})|([a-zA-Z]{1}[a-zA-Z]{1})|([a-zA-Z]{1}[0-9]{1})|([0-9]{1}[a-zA-Z]{1})|([a-zA-Z0-9][a-zA-Z0-9-_]{1,61}[a-zA-Z0-9]))\.([a-zA-Z]{2,6}|[a-zA-Z0-9-]{2,30}\.[a-zA-Z]{2,3})$", ErrorMessage:="domain pattern is not recognized")> _
        Public Property DomainName As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        <Display(Name:="Register Date")> _
        Public Property RegisterDate As Date?

        <Display(Name:="Register By")> _
        <MaxLength(20)> _
        Public Property RegisterBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        <Display(Name:="Activate Date")> _
        Public Property ActivateDate As Date?

        <Display(Name:="Activate By")> _
        <MaxLength(20)> _
        Public Property ActivateBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        <Display(Name:="Suspend Date")> _
        Public Property SuspendDate As Date?

        <Display(Name:="Suspend By")> _
        <MaxLength(20)> _
        Public Property SuspendBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}", ApplyFormatInEditMode:=True)> _
        <Display(Name:="Domain Expiry Date")> _
        Public Property DomainExpireDate As Date?

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}", ApplyFormatInEditMode:=True)> _
        <Display(Name:="Product Expiry Date")> _
        Public Property ProductExpireDate As Date?

        <MaxLength(30)> _
        Public Property Status As String

        <MaxLength(20)> _
        Public Property LastUpdateBy As String
        Public Property LastUpdateDate As Date?
        Public Property LastNotificationDate As Date?

        <NotMapped> _
        Public Property DisplayOrder As Integer?

        Public Overridable Property Invoices As ICollection(Of Invoice)

    End Class

End Namespace
