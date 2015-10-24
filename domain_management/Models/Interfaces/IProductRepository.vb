Imports domain_management.Entities

Namespace Interfaces
    Public Interface IProductRepository
        Inherits IGenericRepository(Of Product)

        Function GetNewProductID() As String

    End Interface
End Namespace
