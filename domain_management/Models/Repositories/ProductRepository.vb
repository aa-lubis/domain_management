Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.DataAccess

Namespace Repositories
    Public Class ProductRepository
        Inherits GenericRepository(Of Product)
        Implements IProductRepository

        Public Sub New(ByVal context As DomainMgrContexts)
            MyBase.New(context)
        End Sub

        Public Function GetNewProductID() As String Implements IProductRepository.GetNewProductID
            Dim digit = 2
            Dim newid As String = ""
            Dim prefix As String = "P"
            Dim maxid = Me.GetAll().Where(Function(w) w.ProductID.StartsWith(prefix)).Max(Function(m) m.ProductID)
            If String.IsNullOrEmpty(maxid) Then
                newid = prefix + (Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            Else
                newid = prefix + (Convert.ToInt32(maxid.Replace(prefix, "")) + Math.Pow(10, digit) + 1).ToString().Substring(1, digit)
            End If
            Return newid
        End Function

    End Class
End Namespace
