Imports System
Imports System.Collections
Imports System.Text
Imports System.Web
Imports System.Web.Mvc
Imports System.Runtime.CompilerServices
Imports System.IO

Namespace HtmlHelpers
    Module ListExtensions

        <Extension()> _
        Public Function RenderPartialViewToString(controller As Controller, viewName As String, model As Object)
            Dim writer As New StringWriter
            Dim viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName)
            controller.ViewData.Model = model
            Dim viewCtx = New ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer)
            viewCtx.View.Render(viewCtx, writer)
            Return writer.ToString()
        End Function

    End Module
End Namespace
