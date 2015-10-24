Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class InvoiceAttachmentRepository
        Inherits GenericRepository(Of InvoiceAttachment)
        Implements IInvoiceAttachmentRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub

        Public Function GetNewSequence(ByVal InvoiceID As String) As String Implements IInvoiceAttachmentRepository.GetNewSequence
            Dim maxentities = Me.GetAll().Where(Function(w) w.InvoiceID = InvoiceID)
            Dim sequence As Integer = 1
            If Not maxentities Is Nothing Then
                sequence += maxentities.Max(Function(m) m.Seq)
            End If
            Return sequence.ToString()
        End Function

    End Class
End Namespace
