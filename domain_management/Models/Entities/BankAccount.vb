Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Collections.Generic

Namespace Entities

    Public Class BankAccount

        <Display(Name:="Bank Account ID")> _
        <MaxLength(3)> _
        Public Property BankAccountID As String

        <Display(Name:="Bank")> _
        <MaxLength(30)> _
        Public Property Bank As String

        <Display(Name:="Account Number")> _
        <MaxLength(30)> _
        Public Property BankAccountNo As String

        <Display(Name:="Account Name")> _
        <MaxLength(50)> _
        Public Property BankAccountName As String

        <MaxLength(20)> _
        Public Property CreatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property CreatedDate As Date?

        <MaxLength(20)> _
        Public Property LastUpdatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property LastUpdatedDate As Date?

    End Class
End Namespace
