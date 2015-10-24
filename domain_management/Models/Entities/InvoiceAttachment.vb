Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Entities
    Public Class InvoiceAttachment

        <Key>
        <Column(Order:=0)>
        Public Property InvoiceID As String

        <Key>
        <Column(Order:=1)>
        Public Property Seq As String

        Public Property FileName As String
        Public Property FileLink As String

        Public Overridable Property Invoice As Invoice

    End Class
End Namespace
