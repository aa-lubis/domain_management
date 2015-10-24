Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class Product

        <Key>
        <MaxLength(3)> _
        Public Property ProductID As String

        <MaxLength(4)> _
        Public Property ProductCategoryID As String

        <Required> _
        <MaxLength(50)> _
        <Display(Name:="Product Name")> _
        Public Property ProductName As String

        <MaxLength(500)> _
        <Display(Name:="Description")> _
        Public Property ProductDesc As String

        <DisplayFormat(DataFormatString:="{0:#,##0}", ApplyFormatInEditMode:=False)> _
        Public Property Price As Integer

        Public Property Counter As Integer

        <MaxLength(20)> _
        Public Property UnitPeriod As String

        <DisplayFormat(DataFormatString:="{0:#,##0}", ApplyFormatInEditMode:=False)> _
        Public Property Discount As Integer

        <MaxLength(20)> _
        Public Property CreatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property CreatedDate As Date?

        <MaxLength(20)> _
        Public Property LastUpdatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property LastUpdatedDate As Date?

        Public Overridable Property ProductCategory As ProductCategory

    End Class

End Namespace
