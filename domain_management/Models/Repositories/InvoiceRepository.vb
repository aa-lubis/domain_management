Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class InvoiceRepository
        Inherits GenericRepository(Of Invoice)
        Implements IInvoiceRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub

        Public Function GetNewInvoiceID() As String Implements IInvoiceRepository.GetNewInvoiceID
            Dim digit = 4
            Dim newid As String = ""
            Dim prefix As String = "I" & Format(Now, "yyMMdd")
            Dim maxid = Me.GetAll().Where(Function(w) w.InvoiceID.StartsWith(prefix)).Max(Function(m) m.InvoiceID)
            If String.IsNullOrEmpty(maxid) Then
                newid = prefix + (Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            Else
                newid = prefix + (Convert.ToInt32(maxid.Replace(prefix, "")) + Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            End If
            Return newid
        End Function
    End Class
End Namespace
