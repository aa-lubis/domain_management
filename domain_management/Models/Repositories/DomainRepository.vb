Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class DomainRepository
        Inherits GenericRepository(Of Domain)
        Implements IDomainRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub

        Public Function GetNewDomainRegID() As String Implements IDomainRepository.GetNewDomainRegID
            Dim digit = 8
            Dim newid As String = ""
            Dim prefix As String = "D" & Format(Now, "yyMMdd")
            Dim maxid = Me.GetAll().Where(Function(w) w.DomainRegID.StartsWith(prefix)).Max(Function(m) m.DomainRegID)
            If String.IsNullOrEmpty(maxid) Then
                newid = prefix + (Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            Else
                newid = prefix + (Convert.ToInt32(maxid.Replace(prefix, "")) + Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            End If
            Return newid
        End Function
    End Class
End Namespace
