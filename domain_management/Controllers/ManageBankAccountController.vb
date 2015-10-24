Imports System.Web
Imports PagedList

Imports domain_management.Entities
Imports domain_management.Interfaces
Imports domain_management.Repositories
Imports domain_management.ViewModels
Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize(Roles:="SUPERADMIN")> _
     Public Class ManageBankAccountController
        Inherits System.Web.Mvc.Controller

        Private unitOfWork As IUnitOfWork
        Private bankAccountRepository As IBankAccountRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal bankAccountRepository As IBankAccountRepository)
            Me.unitOfWork = unitOfWork
            Me.bankAccountRepository = bankAccountRepository
        End Sub

        '
        ' GET: /ManageBankAccount

        Function Index(ByVal filter As String, Optional ByVal p As Integer = 1) As ActionResult
            Dim model = bankAccountRepository.GetAll()
            If Not String.IsNullOrEmpty(filter) Then
                filter = filter.ToLower
                model = model.Where(Function(w) w.Bank.ToLower().Contains(filter) Or w.BankAccountNo.ToLower.Contains(filter) Or w.BankAccountName.ToLower().Contains(filter)).ToList
            End If
            Return View(model.ToPagedList(p, 10))
        End Function

        '
        ' GET: /ManageBankAccount/Details/5

        Function Details(ByVal id As Integer) As ActionResult
            Return View()
        End Function

        '
        ' GET: /ManageBankAccount/Create

        Function Create() As ActionResult
            Return PartialView("_Create")
        End Function

        '
        ' POST: /ManageBankAccount/Create

        <HttpPost> _
        Function Create(model As BankAccount) As ActionResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                If bankAccountRepository.GetAll().Any(Function(a) a.Bank = model.Bank And a.BankAccountNo = model.BankAccountNo) Then
                    ret = "Bank Account specified has already exists!"
                Else
                    Try
                        model.BankAccountID = bankAccountRepository.GetNewBankAccountID()
                        model.CreatedBy = User.Identity.Name
                        model.CreatedDate = Now
                        Me.bankAccountRepository.Insert(model)
                        Me.unitOfWork.Commit()
                        Return New JsonResult With {.Data = New With {.result = "success", .html = RenderPartialViewToString("_List", New List(Of BankAccount) From {model})}}
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

        '
        ' GET: /ManageBankAccount/Edit/5

        Function Edit(ByVal id As String) As ActionResult
            Dim model As BankAccount = bankAccountRepository.GetById(id)
            Return PartialView("_Edit", model)
        End Function

        '
        ' POST: /ManageBankAccount/Edit/5

        <HttpPost> _
        Function Edit(model As BankAccount) As ActionResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                Try
                    Dim editedmodel As BankAccount = bankAccountRepository.GetById(model.BankAccountID)
                    editedmodel.Bank = model.Bank
                    editedmodel.BankAccountName = model.BankAccountName
                    editedmodel.BankAccountNo = model.BankAccountNo
                    editedmodel.LastUpdatedBy = User.Identity.Name
                    editedmodel.LastUpdatedDate = Now
                    Me.bankAccountRepository.Update(editedmodel)
                    Me.unitOfWork.Commit()
                    Return New JsonResult With {.Data = New With {.result = "success", .id = editedmodel.BankAccountID, .html = RenderPartialViewToString("_List", New List(Of BankAccount) From {editedmodel})}}
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

        '
        ' GET: /ManageBankAccount/Delete/5

        Function Delete(ByVal id As String) As ActionResult
            Dim model = bankAccountRepository.GetById(id)
            Return PartialView("_Delete", model)
        End Function

        '
        ' POST: /ManageBankAccount/Delete/5

        <HttpPost> _
        Function Delete(model As BankAccount) As ActionResult
            Dim ret As String = ""
            Try
                bankAccountRepository.Delete(model.BankAccountID)
                unitOfWork.Commit()
                Return New JsonResult With {.Data = New With {.result = "success", .id = model.BankAccountID}}
            Catch ex As Exception
                ret = ex.ToString
            End Try
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function
    End Class
End Namespace
