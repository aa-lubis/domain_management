Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Collections.Generic

Namespace Entities

    Public Class Invoice

        <Key> _
        <MaxLength(15)> _
        <Display(Name:="Invoice No.")> _
        Public Property InvoiceID As String

        Public Property InvoicedTo As String

        <MaxLength(3)> _
        Public Property ProductID As String

        Public Property ProductTermNumber As Integer?

        <MaxLength(20)> _
        Public Property ProductTermPeriod As String

        <DisplayFormat(DataFormatString:="{0:#,##0.##}")> _
        Public Property ProductPrice As Double

        <DisplayFormat(DataformatString:="{0:#,##0.##}")> _
        Public Property ProductDiscount As Double?

        <MaxLength(15)> _
        Public Property DomainRegID As String

        Public Property DomainRegTermNumber As Integer?

        <MaxLength(20)> _
        Public Property DomainRegTermPeriod As String

        <DisplayFormat(DataformatString:="{0:#,##0.##}")> _
        Public Property DomainRegPrice As Double

        <DisplayFormat(DataformatString:="{0:#,##0.##}")> _
        Public Property DomainRegDiscount As Double?

        <Display(Name:="Create Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMMM-yyyy}")> _
        Public Property CreateDate As Date?

        <Display(Name:="Document is complete")> _
        Public Property DocumentIsComplete As Boolean

        <Display(Name:="Document verified by")> _
        <MaxLength(20)> _
        Public Property DocumentVerifiedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMMM-yyyy}")> _
        Public Property DocumentVerifiedDate As Date?

        <Display(Name:="Document verification remark")> _
        <MaxLength(500)> _
        Public Property DocumentVerificationRemark As String

        <Display(Name:="Payment Due Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMMM-yyyy}")> _
        Public Property PaymentDueDate As Date

        <Display(Name:="Payment Method")> _
        <MaxLength(20)>
        Public Property PaymentMethod As String

        <MaxLength(30)> _
        Public Property Bank As String

        <Display(Name:="Bank Account Number")> _
        <MaxLength(30)> _
        Public Property BankAccountNo As String

        <Display(Name:="Account Name")> _
        <MaxLength(50)> _
        Public Property BankAccountName As String

        <Display(Name:="Pay to")> _
        Public Property BankAccountID As String

        <Display(Name:="Payment Date")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMMM-yyyy}")> _
        Public Property PaymentDate As Date?

        <Display(Name:="Payment Amount")> _
        <DisplayFormat(DataformatString:="{0:#,##0.##}")> _
        Public Property PaymentAmount As Double

        <Display(Name:="Validation Number")> _
        <MaxLength(20)> _
        Public Property ValidationNo As String

        <Display(Name:="Payment is complete")> _
        Public Property PaymentIsComplete As Boolean

        <Display(Name:="Payment verified by")> _
        <MaxLength(20)> _
        Public Property PaymentVerifiedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMMM-yyyy}")> _
        Public Property PaymentVerifiedDate As Date?

        <Display(Name:="Payment verification remark")> _
        <MaxLength(500)> _
        Public Property PaymentVerificationRemark As String

        Public Overridable Property Product As Product
        Public Overridable Property Domain As Domain
        Public Overridable Property InvoiceAttachments As ICollection(Of InvoiceAttachment)
        Public Overridable Property BankAccount As BankAccount

    End Class

End Namespace
