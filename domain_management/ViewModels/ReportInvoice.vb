Imports System.ComponentModel.DataAnnotations

Namespace ViewModels
    Public Class ReportInvoice

        <Display(Name:="Invoice ID")> _
        Public InvoiceID As String

        <Display(Name:="Create Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public CreatedDate As Date?

        <Display(Name:="Invoiced To")> _
        Public InvoicedTo As String

        <Display(Name:="Domain Name")> _
        Public DomainName As String

        <Display(Name:="Product")> _
        Public ProductName As String

        <Display(Name:="Setup Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public ActivateDate As Date?

        <Display(Name:="Domain Expiry Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public DomainExpireDate As Date?

        <Display(Name:="Hosting Expiry Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public ProductExpireDate As Date?

        <DisplayFormat(DataFormatString:="{0:#,##0.##}")> _
        Public Price As Integer
        Public Status As String

        <Display(Name:="Payment Status")> _
        Public PaymentStatus As String

    End Class
End Namespace
