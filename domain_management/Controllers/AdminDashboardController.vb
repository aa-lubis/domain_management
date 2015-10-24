Imports System.Linq
Imports System.Net.Mail
Imports System.IO
Imports iTextSharp
Imports PagedList

Imports domain_management.Entities
Imports domain_management.Repositories
Imports domain_management.Interfaces
Imports domain_management.ViewModels
Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize(Roles:="ADMIN,SUPERADMIN")> _
    Public Class AdminDashboardController
        Inherits System.Web.Mvc.Controller

        Private unitOfWork As IUnitOfWork
        Private userRepository As IUserRepository
        Private domainRepository As IDomainRepository
        Private invoiceRepository As IInvoiceRepository
        Private bankAccountRepository As IBankAccountRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal userRepository As IUserRepository,
                ByVal domainRepository As IDomainRepository,
                ByVal invoiceRepository As IInvoiceRepository,
                ByVal bankAccountRepository As IBankAccountRepository)
            Me.unitOfWork = unitOfWork
            Me.userRepository = userRepository
            Me.domainRepository = domainRepository
            Me.invoiceRepository = invoiceRepository
            Me.bankAccountRepository = bankAccountRepository
        End Sub

        '
        ' GET: /Domain

        Function Index(ByVal filter As String, Optional ByVal p As Integer = 1) As ActionResult
            Dim model As IEnumerable(Of Domain) = domainRepository.GetAll()
            For Each item In model
                Dim user As User = userRepository.GetById(item.RegisterBy)
                If Not user Is Nothing Then item.RegisterBy = user.UserName & " (" & item.RegisterBy & ")"
                user = userRepository.GetById(item.ActivateBy)
                If Not user Is Nothing Then item.ActivateBy = user.UserName & " (" & item.ActivateBy & ")"
            Next
            If Not String.IsNullOrEmpty(filter) Then
                filter = filter.ToLower
                model = model.Where(Function(w) w.DomainName.ToLower().Contains(filter) Or w.RegisterBy.ToLower().Contains(filter) Or w.Status.ToLower().Contains(filter) Or w.Invoices.Any(Function(a) a.InvoiceID.ToLower().Contains(filter)))
            End If
            Return View(model.ToPagedList(p, 10))
        End Function

        Function Details(ByVal i As String, ByVal Message As String) As ActionResult
            Dim model As Domain = domainRepository.GetById(i)
            Dim user As User = userRepository.GetById(model.RegisterBy)
            If Not user Is Nothing Then
                model.RegisterBy = userRepository.GetAll().Where(Function(w) w.UserId = model.RegisterBy).Select(Function(s) s.UserName).FirstOrDefault() & " (" & model.RegisterBy & ")"
            End If
            If Not model.Invoices Is Nothing Then
                For Each Invoice As Invoice In model.Invoices
                    user = userRepository.GetById(Invoice.DocumentVerifiedBy)
                    If Not user Is Nothing Then
                        Invoice.DocumentVerifiedBy = user.UserName & " (" & user.UserId & ")"
                    End If
                    user = userRepository.GetById(Invoice.PaymentVerifiedBy)
                    If Not user Is Nothing Then
                        Invoice.PaymentVerifiedBy = user.UserName & " (" & user.UserId & ")"
                    End If
                Next
            End If
            Return View(model)
        End Function

        <HttpPost()> _
        Function AcceptAndRequestPayment(ByVal invoiceid As String)
            Dim invoice As Invoice = invoiceRepository.GetById(invoiceid)
            invoice.DocumentIsComplete = True
            invoice.DocumentVerifiedBy = User.Identity.Name
            invoice.DocumentVerifiedDate = Now
            invoice.DocumentVerificationRemark = ""
            invoice.PaymentDueDate = Now().Date.AddDays(3)
            unitOfWork.Commit()
            If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                Dim reguser As User = userRepository.GetAll().Where(Function(w) w.UserId = invoice.InvoicedTo).FirstOrDefault
                If Not reguser Is Nothing Then

                    Dim useremail As String = reguser.UserEmailAddress

                    Dim totalPrice As Integer = invoice.DomainRegPrice + invoice.DomainRegDiscount + invoice.ProductPrice + invoice.ProductDiscount

                    Dim mail As New MailMessage()
                    mail.To.Add(useremail)
                    mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Payment Confirmation Needed"
                    mail.Body = "Dear " & reguser.UserName & ",<br /><br />"
                    mail.Body &= "We have receive and validate your domain registration<br />"
                    mail.Body &= "Please proceed with payment confirmation from this <a href=""" & Url.Action("PaymentConfirmation", "Dashboard", New With {.invoiceid = invoice.InvoiceID}, Me.Request.Url.Scheme) & """>link</a><br /><br />"
                    mail.Body &= "Your domain details :<br />"
                    mail.Body &= "<table>"
                    mail.Body &= "<tr><td>Domain</td><td>:</td><td><strong>" & invoice.Domain.DomainName & "</strong></td></tr>"
                    mail.Body &= "<tr><td>Registration Date</td><td>:</td><td><strong>" & Format(invoice.Domain.RegisterDate, "d MMMM yyyy") & "</strong></td></tr>"
                    mail.Body &= "<tr><td style=""vertical-align: top"">Product</td><td sytyle=""vertical-align: top"">:</td><td><strong>" & invoice.Product.ProductName & "<br />" & invoice.Product.ProductDesc & "</strong></td></tr>"
                    mail.Body &= "<tr><td>Total Price</td><td>:</td><td><strong>" & Format(totalPrice, "#,##0.##") & "</strong></td></tr>"
                    mail.Body &= "<tr><td>Payment Due Date</td><td>:</td><td><strong>" & Format(invoice.PaymentDueDate, "d MMMM yyyy") & "</strong></td></tr>"
                    mail.Body &= "</table><br />Thank you"
                    mail.IsBodyHtml = True

                    ModuleDomainMgr.SendNotification(mail)

                End If
            End If
            ViewBag.Message = "An email has been sent to user requesting for payment confirmation"
            Return View("Success")
        End Function

        Function RejectDocVerification(ByVal i As String) As ActionResult
            Dim model As Invoice = invoiceRepository.GetById(i)
            Return PartialView("_RejectDocVerification", model)
        End Function

        <HttpPost> _
        Function RejectDocVerification(ByVal model As Invoice) As ActionResult
            If ModelState.IsValid Then
                Dim invoice As Invoice = invoiceRepository.GetById(model.InvoiceID)
                invoice.DocumentIsComplete = False
                invoice.DocumentVerifiedBy = User.Identity.Name
                invoice.DocumentVerifiedDate = Now
                invoice.DocumentVerificationRemark = model.DocumentVerificationRemark
                unitOfWork.Commit()
                ViewBag.Message = "Request has been rejected and waiting for user to fix it"

                If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                    Dim reguser As User = userRepository.GetAll().Where(Function(w) w.UserId = invoice.Domain.RegisterBy).FirstOrDefault
                    If Not reguser Is Nothing Then

                        Dim useremail As String = reguser.UserEmailAddress
                        Dim totalPrice As Integer = invoice.DomainRegPrice - invoice.DomainRegDiscount + invoice.ProductPrice - invoice.ProductDiscount

                        Dim mail As New MailMessage()
                        mail.To.Add(useremail)
                        mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Registration Incomplete"
                        mail.Body = "Dear " & reguser.UserName & ",<br /><br />"
                        mail.Body &= "Your registered domain " & invoice.Domain.DomainName & " cannot be processed because your document doesn't meet the requirement<br />"
                        If Not String.IsNullOrEmpty(invoice.DocumentVerificationRemark) Then
                            mail.Body &= "Admin Message : <strong>" & invoice.DocumentVerificationRemark & "</strong><br />"
                        End If
                        mail.Body &= "Please fix this issue from this <a href=""" & Url.Action("UploadDocument", "Dashboard", New With {.invoice = invoice.InvoiceID}, Me.Request.Url.Scheme) & """>link</a><br /><br />"
                        mail.Body &= "Your domain details :<br />"
                        mail.Body &= "<table>"
                        mail.Body &= "<tr><td>Domain</td><td>:</td><td><strong>" & invoice.Domain.DomainName & "</strong></td></tr>"
                        mail.Body &= "<tr><td>Registration Date</td><td>:</td><td><strong>" & Format(invoice.Domain.RegisterDate, "d MMMM yyyy") & "</strong></td></tr>"
                        mail.Body &= "<tr><td style=""vertical-align:top"">Product</td><td style=""vertical-align: top"">:</td><strong>" & invoice.Product.ProductName & "<br />" & invoice.Product.ProductDesc & "<br />" & invoice.ProductTermNumber & " " & invoice.ProductTermPeriod & IIf(invoice.ProductTermNumber > 1, "s", "") & "</strong><td>"
                        mail.Body &= "<tr><td>Total Price</td><td>:</td><td><strong>" & Format(totalPrice, "#,##0.##") & "</strong></td></tr>"
                        mail.Body &= "</table><br />Thank you"
                        mail.IsBodyHtml = True

                        ModuleDomainMgr.SendNotification(mail)

                    End If
                End If

                Return View("Success")
            End If
        End Function

        Function RejectPaymentConfirmation(ByVal i As String) As ActionResult
            Dim model As Invoice = invoiceRepository.GetById(i)
            Return PartialView("_RejectPaymentConfirmation", model)
        End Function

        <HttpPost> _
        Function RejectPaymentConfirmation(ByVal model As Invoice) As ActionResult
            If ModelState.IsValid Then
                Dim invoice As Invoice = invoiceRepository.GetById(model.InvoiceID)
                invoice.PaymentIsComplete = False
                invoice.PaymentVerifiedBy = User.Identity.Name
                invoice.PaymentVerifiedDate = Now
                invoice.PaymentVerificationRemark = model.PaymentVerificationRemark
                unitOfWork.Commit()

                If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                    Dim reguser As User = userRepository.GetAll().Where(Function(w) w.UserId = invoice.Domain.RegisterBy).FirstOrDefault
                    If Not reguser Is Nothing Then

                        Dim useremail As String = reguser.UserEmailAddress
                        Dim totalPrice As Integer = invoice.DomainRegPrice - invoice.DomainRegDiscount + invoice.ProductPrice - invoice.ProductDiscount

                        Dim mail As New MailMessage()
                        mail.To.Add(useremail)
                        mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Payment Confirmation Incomplete"
                        mail.Body = "Dear " & reguser.UserName & ",<br /><br />"
                        mail.Body &= "Your payment confirmation for domain " & invoice.Domain.DomainName & " cannot be processed<br />"
                        If Not String.IsNullOrEmpty(invoice.PaymentVerificationRemark) Then
                            mail.Body &= "Admin Message : <strong>" & invoice.PaymentVerificationRemark & "</strong><br />"
                        End If
                        mail.Body &= "Please fix this issue from this link <a href=""" & Url.Action("PaymentConfirmation", "Dashboard", New With {.invoice = invoice.InvoiceID}, Me.Request.Url.Scheme) & """>link</a><br /><br />"
                        mail.Body &= "Your domain details :<br />"
                        mail.Body &= "<table>"
                        mail.Body &= "<tr><td>Domain</td><td>:</td><td><strong>" & invoice.Domain.DomainName & "</strong></td></tr>"
                        mail.Body &= "<tr><td>Registration Date</td><td>:</td><td><strong>" & Format(invoice.Domain.RegisterDate, "d MMMM yyyy") & "</strong></td></tr>"
                        mail.Body &= "<tr><td style=""vertical-align:top"">Product</td><td style=""vertical-align: top"">:</td><strong>" & invoice.Product.ProductName & "<br />" & invoice.Product.ProductDesc & "<br />" & invoice.ProductTermNumber & " " & invoice.ProductTermPeriod & IIf(invoice.ProductTermNumber > 1, "s", "") & "</strong><td>"
                        mail.Body &= "<tr><td>Total Price</td><td>:</td><td><strong>" & Format(totalPrice, "#,##0.##") & "</strong></td></tr>"
                        mail.Body &= "</table><br />Thank you"
                        mail.IsBodyHtml = True

                        ModuleDomainMgr.SendNotification(mail)

                    End If
                End If

                ViewBag.Message = "Request has been rejected and waiting for user to fix it"
                Return View("Success")
            End If
        End Function

        Function Verify(id As String)
            Dim model As Domain = domainRepository.GetAll().Where(Function(w) w.Invoices.Any(Function(a) a.InvoiceID = id)).SingleOrDefault()
            Dim invoice As Invoice
            If Not model Is Nothing Then
                invoice = invoiceRepository.GetById(id)

                ' set new product expire date
                If Not invoice.ProductTermPeriod Is Nothing Then
                    Select Case invoice.ProductTermPeriod.ToLower
                        Case "month"
                            If model.ProductExpireDate Is Nothing Then
                                model.ProductExpireDate = model.RegisterDate.Value.AddMonths(invoice.ProductTermNumber)
                            Else
                                model.ProductExpireDate = model.ProductExpireDate.Value.AddMonths(invoice.ProductTermNumber)
                            End If
                        Case "year"
                            If model.ProductExpireDate Is Nothing Then
                                model.ProductExpireDate = model.RegisterDate.Value.AddYears(invoice.ProductTermNumber)
                            Else
                                model.ProductExpireDate = model.ProductExpireDate.Value.AddYears(invoice.ProductTermNumber)
                            End If
                    End Select
                End If

                ' set new domain expire date
                If Not invoice.DomainRegTermPeriod Is Nothing Then
                    Select Case invoice.DomainRegTermPeriod.ToLower
                        Case "month"
                            If model.DomainExpireDate Is Nothing Then
                                model.DomainExpireDate = model.RegisterDate.Value.AddMonths(invoice.DomainRegTermNumber)
                            Else
                                model.DomainExpireDate = model.DomainExpireDate.Value.AddMonths(invoice.DomainRegTermNumber)
                            End If
                        Case "year"
                            If model.DomainExpireDate Is Nothing Then
                                model.DomainExpireDate = model.RegisterDate.Value.AddYears(invoice.DomainRegTermNumber)
                            Else
                                model.DomainExpireDate = model.DomainExpireDate.Value.AddYears(invoice.DomainRegTermNumber)
                            End If
                    End Select
                End If
            End If

            Return PartialView("_VerifyDomain", model)
        End Function

        <HttpPost()> _
        Function Verify(model As Domain) As ActionResult

            If ModelState.IsValid Then
                Try
                    Dim editeddomain As Domain = domainRepository.GetById(model.DomainRegID)
                    If Not model.DomainExpireDate Is Nothing Then editeddomain.DomainExpireDate = model.DomainExpireDate
                    If Not model.ProductExpireDate Is Nothing Then editeddomain.ProductExpireDate = model.ProductExpireDate
                    editeddomain.LastUpdateBy = Me.User.Identity.Name
                    editeddomain.LastUpdateDate = Now
                    editeddomain.ActivateBy = Me.User.Identity.Name
                    editeddomain.ActivateDate = Now
                    Dim actionmsg As String = "activated"
                    If editeddomain.Status = "EXTENDED" Then actionmsg = "extended"
                    editeddomain.Status = "ACTIVE"

                    Dim editedinvoice As Invoice = invoiceRepository.GetAll() _
                                                   .Where(Function(w) w.DomainRegID = editeddomain.DomainRegID) _
                                                   .OrderByDescending(Function(o) o.CreateDate).FirstOrDefault()

                    editedinvoice.PaymentIsComplete = True
                    editedinvoice.PaymentVerifiedBy = Me.User.Identity.Name
                    editedinvoice.PaymentVerifiedDate = Now
                    editedinvoice.PaymentVerificationRemark = ""

                    Me.unitOfWork.Commit()

                    If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                        Dim reguser As User = userRepository.GetAll().Where(Function(w) w.UserId = editeddomain.RegisterBy).FirstOrDefault
                        If Not reguser Is Nothing Then

                            Dim useremail As String = reguser.UserEmailAddress

                            Dim mail As New MailMessage()
                            mail.To.Add(useremail)
                            mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Domain Activation"
                            mail.Body = "Dear " & reguser.UserName & ",<br /><br />"
                            mail.Body &= "<strong>Congratulation!</strong><br />Your registered domain " & editeddomain.DomainName & " has been " & actionmsg & "<br /><br />"
                            mail.Body &= "Your domain details :<br />"
                            mail.Body &= "<table>"
                            mail.Body &= "<tr><td>Domain</td><td>:</td><td><strong>" & editeddomain.DomainName & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Registration Date</td><td>:</td><td><strong>" & Format(editeddomain.RegisterDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "<tr><td>" & IIf(actionmsg = "activated", "Activation", "Extend") & " Date</td><td>:</td><td><strong>" & Format(editeddomain.ActivateDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Domain Expiry Date</td><td>:</td><td><strong>" & Format(editeddomain.DomainExpireDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Hosting Expiry Date</td><td>:</td><td><strong>" & Format(editeddomain.ProductExpireDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "</table><br />Thank you"
                            mail.IsBodyHtml = True

                            ModuleDomainMgr.SendNotification(mail)

                        End If
                    End If

                    Dim user As User = userRepository.GetById(editeddomain.RegisterBy)
                    If Not user Is Nothing Then editeddomain.RegisterBy = user.UserName & " (" & editeddomain.RegisterBy & ")"
                    user = userRepository.GetById(editeddomain.ActivateBy)
                    If Not user Is Nothing Then editeddomain.ActivateBy = user.UserName & " (" & editeddomain.ActivateBy & ")"

                    ViewBag.Message = "Domain " & model.DomainName & " has successfully activated"
                    Return View("Success")
                Catch ex As Exception
                    ModelState.AddModelError("", ex.ToString())
                End Try
            End If
            Return View("Error")
        End Function

        Function Suspend(id As String) As ActionResult
            Dim model As Domain = domainRepository.GetById(id)
            model.DomainExpireDate = model.RegisterDate.Value.AddYears(1)
            Return PartialView("_SuspendDomain", model)
        End Function

        <HttpPost()> _
        Function Suspend(model As Domain) As JsonResult

            Dim ret As String = ""
            If ModelState.IsValid Then
                Try

                    Dim editeddomain As Domain = domainRepository.GetById(model.DomainRegID)
                    editeddomain.Status = "SUSPENDED"
                    editeddomain.SuspendBy = Me.User.Identity.Name
                    editeddomain.SuspendDate = Now
                    Me.unitOfWork.Commit()

                    If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                        Dim reguser As User = userRepository.GetAll().Where(Function(w) w.UserId = editeddomain.RegisterBy).FirstOrDefault
                        If Not reguser Is Nothing Then

                            Dim useremail As String = reguser.UserEmailAddress

                            Dim mail As New MailMessage()
                            mail.To.Add(useremail)
                            mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Domain Suspended"
                            mail.Body = "Dear " & reguser.UserName & ",<br /><br />"
                            mail.Body &= "Your domain " & editeddomain.DomainName & " has been suspended<br /><br />"
                            mail.Body &= "Your domain details :<br />"
                            mail.Body &= "<table>"
                            mail.Body &= "<tr><td>Domain</td><td>:</td><td><strong>" & editeddomain.DomainName & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Registration Date</td><td>:</td><td><strong>" & Format(editeddomain.RegisterDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Activation Date</td><td>:</td><td><strong>" & Format(editeddomain.ActivateDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Expiry Date</td><td>:</td><td><strong>" & Format(editeddomain.DomainExpireDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "<tr><td>Suspend Date</td><td>:</td><td><strong>" & Format(editeddomain.SuspendDate, "d MMMM yyyy") & "</strong></td></tr>"
                            mail.Body &= "</table><br />Thank you"
                            mail.IsBodyHtml = True

                            ModuleDomainMgr.SendNotification(mail)

                        End If
                    End If

                    Dim user As User = userRepository.GetById(editeddomain.RegisterBy)
                    If Not user Is Nothing Then editeddomain.RegisterBy = user.UserName & " (" & editeddomain.RegisterBy & ")"
                    user = userRepository.GetById(editeddomain.ActivateBy)
                    If Not user Is Nothing Then editeddomain.ActivateBy = user.UserName & " (" & editeddomain.ActivateBy & ")"
                    user = userRepository.GetById(editeddomain.SuspendBy)
                    If Not user Is Nothing Then editeddomain.SuspendBy = user.UserName & " (" & editeddomain.SuspendBy & ")"

                    Return New JsonResult With {.Data = New With {.result = "success", .id = model.DomainRegID, .html = RenderPartialViewToString("_DomainList", New List(Of Domain) From {editeddomain})}}
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

        Public Function Invoice(ByVal i As String, ByVal export As String) As ActionResult
            Dim invoiceviewmodel As New InvoiceViewModel
            invoiceviewmodel.Invoice = invoiceRepository.GetAll(includeProperties:="Domain,Product, BankAccount").Where(Function(w) w.InvoiceID = i).FirstOrDefault
            If Not invoiceviewmodel.Invoice Is Nothing Then invoiceviewmodel.User = userRepository.GetById(invoiceviewmodel.Invoice.InvoicedTo)
            ViewBag.DefaultBankAccount = bankAccountRepository.GetAll().FirstOrDefault()
            If export = "pdf" Then
                Return InvoiceDownload(invoiceviewmodel)
            End If
            Return View(invoiceviewmodel)
        End Function

        Public Function InvoiceDownload(ByVal invoiceviewmodel As InvoiceViewModel) As FileStreamResult

            Dim workStream As New MemoryStream
            Dim document As New iTextSharp.text.Document
            document.SetPageSize(iTextSharp.text.PageSize.LETTER)
            Dim pdfWriter As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, workStream)
            pdfWriter.CloseStream = False

            document.Open()

            Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath("~/img/logo-plain.png"))
            document.Add(image)

            Dim html As String = RenderRazorViewToString(Me.ControllerContext, "_InvoiceContent", invoiceviewmodel)
            Dim htmlContext As New iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(Nothing)
            htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory())

            Dim cssResolver = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().GetDefaultCssResolver(False)
            cssResolver.AddCssFile(Server.MapPath("~/Content/invoice.css"), True)

            ' pipeline
            Dim pdfWriterPipeline As New iTextSharp.tool.xml.pipeline.end.PdfWriterPipeline(document, pdfWriter)
            Dim cssPipeline As New iTextSharp.tool.xml.pipeline.css.CssResolverPipeline(cssResolver, New iTextSharp.tool.xml.pipeline.html.HtmlPipeline(htmlContext, pdfWriterPipeline))
            Dim worker As New iTextSharp.tool.xml.XMLWorker(cssPipeline, True)
            Dim parser As New iTextSharp.tool.xml.parser.XMLParser(worker)
            parser.Parse(New StringReader(html))

            document.Close()
            pdfWriter.Close()

            Dim byteInfo As Byte() = workStream.ToArray()
            workStream.Write(byteInfo, 0, byteInfo.Length)
            workStream.Position = 0

            Return New FileStreamResult(workStream, "application/pdf")
        End Function

        Function RenderRazorViewToString(context As ControllerContext, viewName As String, model As Object) As String
            context.Controller.ViewData.Model = model
            Using sw As New StringWriter
                Dim viewResult = ViewEngines.Engines.FindPartialView(context, viewName)
                Dim viewContext As New ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, sw)
                viewResult.View.Render(viewContext, sw)
                viewResult.ViewEngine.ReleaseView(context, viewResult.View)
                Return sw.GetStringBuilder().ToString()
            End Using
        End Function

        Function ViewUser(id As String) As ActionResult
            Dim model = userRepository.GetById(id)
            Return PartialView("_ViewUser", model)
        End Function


    End Class
End Namespace
