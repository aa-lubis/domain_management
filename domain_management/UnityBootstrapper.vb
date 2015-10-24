Imports Microsoft.Practices.Unity
Imports System.Web.Mvc
Imports Unity.Mvc3

Imports Quartz
Imports Quartz.Impl
Imports Quartz.Spi

Imports domain_management.Interfaces
Imports domain_management.Repositories
Imports domain_management.DataAccess

Public NotInheritable Class UnityBootstrapper

    Private Sub New()
    End Sub

    Public Shared Sub Initialise()
        Dim container = BuildUnityContainer()
        DependencyResolver.SetResolver(New UnityDependencyResolver(container))
        RegisterTypes(container)

        ' scheduler
        SetJobScheduler(container)

    End Sub

    Private Shared Function BuildUnityContainer() As IUnityContainer
        Dim container = New UnityContainer()
        Return container
    End Function

    Private Shared Sub RegisterTypes(ByVal container As UnityContainer)
        container.RegisterType(Of IBankAccountRepository, BankAccountRepository)()
        container.RegisterType(Of DomainMgrContexts)(New HierarchicalLifetimeManager())
        container.RegisterType(Of IUnitOfWork, UnitOfWork)()
        container.RegisterType(Of IDomainRepository, DomainRepository)()
        container.RegisterType(Of IIdentityTypeRepository, IdentityTypeRepository)()
        container.RegisterType(Of IInvoiceRepository, InvoiceRepository)()
        container.RegisterType(Of IInvoiceAttachmentRepository, InvoiceAttachmentRepository)()
        container.RegisterType(Of INotificationLogRepository, NotificationLogRepository)()
        container.RegisterType(Of IProductCategoryRepository, ProductCategoryRepository)()
        container.RegisterType(Of IProductRepository, ProductRepository)()
        container.RegisterType(Of IRoleRepository, RoleRepository)()
        container.RegisterType(Of ITLDHostRepository, TLDHostRepository)()
        container.RegisterType(Of IUserRepository, UserRepository)()
        container.RegisterType(Of IUserRoleRepository, UserRoleRepository)()
    End Sub


    Private Shared Sub SetJobScheduler(ByVal container As IUnityContainer)

        ' job scheduler
        Dim schedFact As ISchedulerFactory = New UnitySchedulerFactory(New UnityJobFactory(container))

        ' get a scheduler
        Dim sched As IScheduler = schedFact.GetScheduler()
        sched.Start()

        ' construct job info
        Dim job As IJobDetail = JobBuilder.Create(GetType(ScheduledJob)).WithIdentity("job1", "group1").Build()

        ' construct trigger
        Dim tempTrigger As ITrigger = TriggerBuilder.Create().WithIdentity("Trigger1").WithCronSchedule("0 0 7 * * ?").StartNow().Build()
        Dim trigger As ICronTrigger = DirectCast(tempTrigger, ICronTrigger)

        sched.ScheduleJob(job, trigger)

    End Sub

End Class

