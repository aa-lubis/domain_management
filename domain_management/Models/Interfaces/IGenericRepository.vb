

Namespace Interfaces

    Public Interface IGenericRepository(Of TEntity As Class)

        Function GetAll(Optional ByVal filter As Func(Of TEntity, Boolean) = Nothing, _
                                             Optional ByVal orderBy As Func(Of IQueryable(Of TEntity), IOrderedQueryable(Of TEntity)) = Nothing, _
                                             Optional ByVal includeProperties As String = ""
                                           ) As IEnumerable(Of TEntity)
        Function GetById(ByVal id As Object)
        Sub Insert(ByVal entity As TEntity)
        Sub Delete(ByVal id As Object)
        Sub Delete(ByVal entity As TEntity)
        Sub Update(ByVal entity As TEntity)

    End Interface

End Namespace

