Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class NotificationLogRepository
        Inherits GenericRepository(Of NotificationLog)
        Implements INotificationLogRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub
    End Class
End Namespace
