Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class UserRoleRepository
        Inherits GenericRepository(Of UserRole)
        Implements IUserRoleRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub
    End Class
End Namespace
