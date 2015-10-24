
Imports System.ComponentModel.DataAnnotations

Namespace ViewModels
    Public Class ProductPurchasedViewModel

        Public Property ProductDesc As String

        <DisplayFormat(DataFormatString:="{0:#,##0.##}")> _
        Public Property Price As Integer

        <DisplayFormat(DataFormatString:="{0:#,##0.##}")> _
        Public Property Discount As Integer

    End Class
End Namespace
