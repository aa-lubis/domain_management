Imports domain_management.Entities

Namespace Interfaces
    Public Interface IInvoiceRepository
        Inherits IGenericRepository(Of Invoice)

        Function GetNewInvoiceID() As String

    End Interface
End Namespace
