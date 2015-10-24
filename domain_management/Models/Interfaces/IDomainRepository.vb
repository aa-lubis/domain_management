Imports domain_management.Entities

Namespace Interfaces
    Public Interface IDomainRepository
        Inherits IGenericRepository(Of Domain)

        Function GetNewDomainRegID() As String

    End Interface
End Namespace