Imports System.Data.Entity
Imports domain_management.DataAccess

Public Class UnitOfWork
    Implements IUnitOfWork

    Private _context As DomainMgrContexts

    Public Sub New(ByVal context As DomainMgrContexts)
        Me._context = context
    End Sub

    Public ReadOnly Property context As DataAccess.DomainMgrContexts Implements IUnitOfWork.context
        Get
            Return _context
        End Get
    End Property

    Public Function Commit() As Integer Implements IUnitOfWork.Commit
        Return _context.SaveChanges()
    End Function

    Public Sub RollBack() Implements IUnitOfWork.RollBack

        Dim changedEntries = _context.ChangeTracker.Entries().Where(Function(w) w.State <> EntityState.Unchanged).ToList()

        For Each entry In changedEntries.Where(Function(w) w.State = EntityState.Modified)
            'entry.CurrentValues.SetValues(entry.OriginalValues)
            entry.State = EntityState.Unchanged
        Next

        For Each entry In changedEntries.Where(Function(w) w.State = EntityState.Added)
            entry.State = EntityState.Detached
        Next

        For Each entry In changedEntries.Where(Function(w) w.State = EntityState.Deleted)
            entry.State = EntityState.Unchanged
        Next

    End Sub

End Class
