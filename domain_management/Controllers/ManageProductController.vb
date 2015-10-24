Imports domain_management.Entities
Imports domain_management.ViewModels
Imports domain_management.Interfaces

Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize(Roles:="SUPERADMIN")> _
    Public Class ManageProductController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /ManageProduct

        Dim unitOfWork As IUnitOfWork
        Dim productCategoryRepository As IProductCategoryRepository
        Dim productRepository As IProductRepository
        Dim invoiceRepository As IInvoiceRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal productCategoryRepository As IProductCategoryRepository,
                ByVal productRepository As IProductRepository,
                ByVal invoiceRepository As IInvoiceRepository)
            Me.unitOfWork = unitOfWork
            Me.productCategoryRepository = productCategoryRepository
            Me.productRepository = productRepository
            Me.invoiceRepository = invoiceRepository
        End Sub

        Function Index(ByVal cat As String) As ActionResult
            Dim viewmodel As New ProductViewModel
            viewmodel.ProductCategory = productCategoryRepository.GetAll()
            If Not viewmodel.ProductCategory Is Nothing Then
                If String.IsNullOrEmpty(cat) Then
                    viewmodel.Product = productRepository.GetAll().Where(Function(w) w.ProductCategoryID = viewmodel.ProductCategory.First().ProductCategoryID)
                Else
                    viewmodel.Product = productRepository.GetAll().Where(Function(w) w.ProductCategoryID = cat)
                End If
            End If
            If cat Is Nothing Then ViewBag.SelectedCategory = productCategoryRepository.GetAll().FirstOrDefault.ProductCategoryID Else ViewBag.SelectedCategory = cat
            Return View(viewmodel)
        End Function

        Function Create(cat As String) As ActionResult
            Dim category As ProductCategory = productCategoryRepository.GetById(cat)
            Dim model As New Product With {.ProductCategoryID = cat, .Counter = 1, .UnitPeriod = "Month", .ProductCategory = productCategoryRepository.GetById(cat)}
            Return PartialView("_Create", model)
        End Function

        <HttpPost()> _
        Function Create(model As Product) As JsonResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                Try
                    model.ProductID = productRepository.GetNewProductID()
                    model.CreatedBy = User.Identity.Name
                    model.CreatedDate = Now
                    Me.productRepository.Insert(model)
                    Me.unitOfWork.Commit()
                    Return New JsonResult With {.Data = New With {.result = "success", .html = RenderPartialViewToString("_List", New List(Of Product) From {model})}}
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

        Function Edit(id As String) As ActionResult
            Dim model As Product = productRepository.GetAll(includeProperties:="ProductCategory").Where(Function(w) w.ProductID = id).FirstOrDefault
            Return PartialView("_Edit", model)
        End Function

        <HttpPost()> _
        Function Edit(model As Product) As JsonResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                Try
                    Dim editedmodel As Product = productRepository.GetById(model.ProductID)
                    editedmodel.ProductName = model.ProductName
                    editedmodel.ProductDesc = model.ProductDesc
                    editedmodel.Price = model.Price
                    editedmodel.UnitPeriod = model.UnitPeriod
                    editedmodel.Counter = model.Counter
                    editedmodel.Discount = model.Discount
                    editedmodel.LastUpdatedBy = User.Identity.Name
                    editedmodel.LastUpdatedDate = Now
                    Me.productRepository.Update(editedmodel)
                    Me.unitOfWork.Commit()
                    Return New JsonResult With {.Data = New With {.result = "success", .id = editedmodel.ProductID, .html = RenderPartialViewToString("_List", New List(Of Product) From {editedmodel})}}
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
            Dim model As Product = productRepository.GetAll(includeProperties:="ProductCategory").Where(Function(w) w.ProductID = id).FirstOrDefault
            Return PartialView("_Delete", model)
        End Function

        <HttpPost()> _
        Function Delete(model As Product) As JsonResult
            Dim ret As String = ""
            If Not model.ProductID Is Nothing Then
                If invoiceRepository.GetAll().Any(Function(w) w.ProductID = model.ProductID) Then
                    ret = "Cannot delete selected product because it has been used!"
                Else
                    Try
                        Me.productRepository.Delete(model.ProductID)
                        Me.unitOfWork.Commit()
                        Return New JsonResult With {.Data = New With {.result = "success", .id = model.ProductID}}
                    Catch ex As Exception
                        ret = ex.ToString
                    End Try
                End If
            Else
                ret = "Unknown error!"
            End If
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

    End Class
End Namespace
