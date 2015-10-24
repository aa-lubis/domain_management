Imports domain_management.Entities

Namespace Interfaces
    Public Interface IInvoiceAttachmentRepository
        Inherits IGenericRepository(Of InvoiceAttachment)

        Function GetNewSequence(ByVal InvoiceID As String) As String

    End Interface
End Namespace
