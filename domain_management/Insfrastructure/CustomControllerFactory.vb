Imports System.Web.Mvc

Imports domain_management.Interfaces
Imports domain_management.Repositories

Public Class CustomControllerFactory
    Inherits DefaultControllerFactory

    Protected Overrides Function GetControllerInstance(requestContext As RequestContext, controllerType As Type) As IController
        Dim unitOfWork As IUnitOfWork = New UnitOfWork()
        Dim controller As IController = Activator.CreateInstance(controllerType, New Object() {unitOfWork})


        'Return MyBase.GetControllerInstance(requestContext, controllerType)
        Return controller
    End Function

End Class

