Imports System.Web
Imports PagedList

Imports domain_management.Entities
Imports domain_management.ViewModels
Imports domain_management.Repositories
Imports domain_management.Interfaces
Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize(Roles:="SUPERADMIN")> _
    Public Class ManageTLDHostController
        Inherits System.Web.Mvc.Controller

        Private unitOfWork As IUnitOfWork
        Private tldHostRepository As ITLDHostRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal tldHostRepository As ITLDHostRepository)
            Me.unitOfWork = unitOfWork
            Me.tldHostRepository = tldHostRepository
        End Sub

        '
        ' GET: /ManageTLDHost

        Function Index(ByVal filter As String, Optional ByVal p As Integer = 1) As ActionResult
            Dim model = tldHostRepository.GetAll().ToList()
            If Not String.IsNullOrEmpty(filter) Then
                filter = filter.ToLower
                model = model.Where(Function(w) w.TLD.ToLower().Contains(filter) Or w.Host.ToLower.Contains(filter)).ToList
            End If
            Return View(model.ToPagedList(p, 10))
        End Function

        Function Create() As ActionResult
            Return PartialView("_Create", New TLDHost())
        End Function

        <HttpPost()> _
        Function Create(model As TLDHost) As JsonResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                If tldHostRepository.GetAll().Any(Function(a) a.TLD = model.TLD) Then
                    ret = "Top Level Domain specified has already exists!"
                Else
                    Try
                        model.CreatedBy = User.Identity.Name
                        model.CreatedDate = Now
                        Me.tldHostRepository.Insert(model)
                        Me.unitOfWork.Commit()
                        Return New JsonResult With {.Data = New With {.result = "success", .html = RenderPartialViewToString("_List", New List(Of TLDHost) From {model})}}
                    Catch ex As Exception
                        ret = ex.ToString
                    End Try
                End If
            Else
                For Each key In ModelState.Keys
                    For Each errMsg In ModelState(key).Errors
                        ret &= errMsg.ErrorMessage & "<br />"
                    Next
                Next
            End If
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

        Function Edit(id As String) As ActionResult
            Dim model As TLDHost = tldHostRepository.GetById(id)
            If Not model.Requirement Is Nothing Then
                model.Requirement = System.Uri.UnescapeDataString(model.Requirement)
            End If
            Return PartialView("_Edit", tldHostRepository.GetById(id))
        End Function

        <HttpPost()> _
        Function Edit(model As TLDHost) As JsonResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                Try
                    Dim editedmodel As TLDHost = tldHostRepository.GetById(model.TLD)
                    editedmodel.Host = model.Host
                    editedmodel.Discount = model.Discount
                    editedmodel.Price = model.Price
                    editedmodel.Requirement = model.Requirement
                    editedmodel.LastUpdatedBy = User.Identity.Name
                    editedmodel.LastUpdatedDate = Now
                    Me.tldHostRepository.Update(editedmodel)
                    Me.unitOfWork.Commit()
                    Return New JsonResult With {.Data = New With {.result = "success", .id = editedmodel.TLD, .html = RenderPartialViewToString("_List", New List(Of TLDHost) From {editedmodel})}}
                Catch ex As Exception
                    ret = ex.ToString
                End Try
            Else
                For Each keys In ModelState.Keys
                    For Each errMsg In ModelState(keys).Errors
                        ret &= errMsg.ErrorMessage & "<br />"
                    Next
                Next
            End If
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

        Function Delete(id As String) As ActionResult
            Return PartialView("_Delete", tldHostRepository.GetById(id))
        End Function

        <HttpPost()> _
        Function Delete(model As TLDHost) As JsonResult
            Dim ret As String = ""
            If Not model.TLD Is Nothing Then
                Try
                    tldHostRepository.Delete(model.TLD)
                    unitOfWork.Commit()
                    Return New JsonResult With {.Data = New With {.result = "success", .id = model.TLD}}
                Catch ex As Exception
                    ret = ex.ToString
                End Try
            End If
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

    End Class
End Namespace
