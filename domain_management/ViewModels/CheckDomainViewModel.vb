Imports System.ComponentModel.DataAnnotations

Namespace ViewModels
    Public Class CheckDomainViewModel

        Public DomainName As String
        Public Status As String

        <DisplayFormat(DataFormatString:="{0:#,##0}")> _
         Public Price As Integer

        Public UnitPrice As String

    End Class
End Namespace
