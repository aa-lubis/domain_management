Imports domain_management.Entities

Namespace Interfaces
    Public Interface IBankAccountRepository
        Inherits IGenericRepository(Of BankAccount)

        Function GetNewBankAccountID() As String

    End Interface
End Namespace