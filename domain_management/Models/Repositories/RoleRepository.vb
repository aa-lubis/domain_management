Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class RoleRepository
        Inherits GenericRepository(Of Role)
        Implements IRoleRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub
    End Class
End Namespace
