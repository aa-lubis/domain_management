Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class ProductCategoryRepository
        Inherits GenericRepository(Of ProductCategory)
        Implements IProductCategoryRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub
    End Class
End Namespace
