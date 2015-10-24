Imports System.Data.Entity

Imports domain_management.Entities
Imports domain_management.DataAccess
Imports domain_management.Interfaces

Namespace Repositories

    Public Class GenericRepository(Of TEntity As Class)
        Implements IGenericRepository(Of TEntity)

        Friend context As DomainMgrContexts
        Friend dbSet As DbSet(Of TEntity)

        Public Sub New(ByVal context As DomainMgrContexts)
            Me.context = context
            Me.dbSet = context.Set(Of TEntity)()
        End Sub

        Public Overridable Function GetAll(Optional filter As Func(Of TEntity, Boolean) = Nothing, Optional orderBy As Func(Of IQueryable(Of TEntity), IOrderedQueryable(Of TEntity)) = Nothing, Optional includeProperties As String = "") As IEnumerable(Of TEntity) Implements IGenericRepository(Of TEntity).GetAll
            Dim query As IQueryable(Of TEntity) = dbSet

            If Not filter Is Nothing Then
                query = query.Where(filter)
            End If

            For Each includeProperty As String In includeProperties.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
                query = query.Include(includeProperty)
            Next

            If Not orderBy Is Nothing Then
                Return orderBy(query).ToList()
            Else
                Return query.ToList()
            End If
        End Function

        Public Overridable Function GetById(id As Object) As Object Implements IGenericRepository(Of TEntity).GetById
            Return dbSet.Find(id)
        End Function

        Public Overridable Sub Insert(entity As TEntity) Implements IGenericRepository(Of TEntity).Insert
            dbSet.Add(entity)
        End Sub

        Public Overridable Sub Update(entity As TEntity) Implements IGenericRepository(Of TEntity).Update
            dbSet.Attach(entity)
            context.Entry(entity).State = EntityState.Modified
        End Sub

        Public Overridable Sub Delete(id As Object) Implements IGenericRepository(Of TEntity).Delete
            Dim entityToDelete As TEntity = dbSet.Find(id)
            Delete(entityToDelete)
        End Sub

        Public Overridable Sub Delete(entity As TEntity) Implements IGenericRepository(Of TEntity).Delete
            If context.Entry(entity).State = EntityState.Detached Then
                dbSet.Attach(entity)
            End If
            dbSet.Remove(entity)
        End Sub

    End Class

End Namespace
