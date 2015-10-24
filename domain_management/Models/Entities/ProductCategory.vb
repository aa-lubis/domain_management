Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities

    Public Class ProductCategory

        <Key> _
        <MaxLength(4)> _
        Public Property ProductCategoryID As String

        <MaxLength(50)> _
        <Display(Name:="Product Category")> _
        Public Property ProductCategoryName As String

        <MaxLength(20)> _
        Public Property CreatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property CreatedDate As Date?

        <MaxLength(20)> _
        Public Property LastUpdatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property LastUpdatedDate As Date?

        Public Overridable Property Product As ICollection(Of Product)

    End Class

End Namespace
