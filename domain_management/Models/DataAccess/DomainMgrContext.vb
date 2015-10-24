Imports System.Data.Entity
Imports System.Data.Entity.ModelConfiguration
Imports System.Data.Entity.ModelConfiguration.Conventions

Imports domain_management.Entities

Namespace DataAccess

    '<DbConfigurationType(GetType(MySql.Data.Entity.MySqlEFConfiguration))> _
    Public Class DomainMgrContexts
        Inherits System.Data.Entity.DbContext

        Public Sub New()
            MyBase.New("DomainMgrConn")
            Me.Configuration.ValidateOnSaveEnabled = False
        End Sub

        Public Property BankAccounts As DbSet(Of BankAccount)
        Public Property Domains As DbSet(Of Domain)
        Public Property IdentityTypes As DbSet(Of IdentityType)
        Public Property Invoices As DbSet(Of Invoice)
        Public Property InvoiceAttachments As DbSet(Of InvoiceAttachment)
        Public Property NotificationLogs As DbSet(Of NotificationLog)
        Public Property Products As DbSet(Of Product)
        Public Property ProductCategories As DbSet(Of ProductCategory)
        Public Property Roles As DbSet(Of Role)
        Public Property TLDHosts As DbSet(Of TLDHost)
        Public Property Users As DbSet(Of User)
        Public Property UserRoles As DbSet(Of UserRole)

        Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
            MyBase.OnModelCreating(modelBuilder)
            modelBuilder.Conventions.Remove(Of PluralizingTableNameConvention)()
            modelBuilder.Conventions.Remove(Of OneToManyCascadeDeleteConvention)()
        End Sub

    End Class

End Namespace

