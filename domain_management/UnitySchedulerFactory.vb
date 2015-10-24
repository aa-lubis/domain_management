Imports Quartz
Imports Quartz.Spi
Imports Quartz.Impl

Public Class UnitySchedulerFactory
    Inherits StdSchedulerFactory

    Private unityJobFactory As UnityJobFactory

    Public Sub New(unityJobFactory As UnityJobFactory)
        Me.unityJobFactory = unityJobFactory
    End Sub

    Protected Overrides Function Instantiate(rsrcs As Core.QuartzSchedulerResources, qs As Core.QuartzScheduler) As IScheduler
        qs.JobFactory = Me.unityJobFactory
        Return MyBase.Instantiate(rsrcs, qs)
    End Function


End Class
