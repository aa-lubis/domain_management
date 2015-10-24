Imports domain_management.DataAccess

Public Interface IUnitOfWork

    ReadOnly Property context As DomainMgrContexts
    Function Commit() As Integer
    Sub RollBack()

End Interface
