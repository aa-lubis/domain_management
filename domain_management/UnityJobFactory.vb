
Imports Quartz.Spi
Imports Microsoft.Practices.Unity

Public Class UnityJobFactory
    Implements IJobFactory

    Private container As IUnityContainer

    Public Sub New(container As IUnityContainer)
        Me.container = container
    End Sub

    Public Function NewJob(bundle As TriggerFiredBundle, scheduler As Quartz.IScheduler) As Quartz.IJob Implements IJobFactory.NewJob
        Dim jobDetail = bundle.JobDetail
        Dim jobtype = jobDetail.JobType
        Return Me.container.Resolve(jobtype)
    End Function

    Public Sub ReturnJob(job As Quartz.IJob) Implements IJobFactory.ReturnJob
        ' nothing here
    End Sub
End Class
