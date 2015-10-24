Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class BankAccountRepository
        Inherits GenericRepository(Of BankAccount)
        Implements IBankAccountRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub

        Public Function GetNewBankAccountID() As String Implements IBankAccountRepository.GetNewBankAccountID
            Dim digit = 2
            Dim newid As String = ""
            Dim prefix As String = "B"
            Dim maxid = Me.GetAll().Where(Function(w) w.BankAccountID.StartsWith(prefix)).Max(Function(m) m.BankAccountID)
            If String.IsNullOrEmpty(maxid) Then
                newid = prefix + (Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            Else
                newid = prefix + (Convert.ToInt32(maxid.Replace(prefix, "")) + Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            End If
            Return newid
        End Function
    End Class
End Namespace
