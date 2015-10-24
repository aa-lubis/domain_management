Imports System.IO
Imports System.Net.Mail
Imports System.Linq
Imports iTextSharp
Imports PagedList


Imports domain_management.Entities
Imports domain_management.Repositories
Imports domain_management.ViewModels
Imports domain_management.Interfaces
Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize()> _
    Public Class DashboardController
        Inherits System.Web.Mvc.Controller

        Private unitOfWork As IUnitOfWork
        Private domainRepository As IDomainRepository
        Private tldHostRepository As ITLDHostRepository
        Private invoiceRepository As IInvoiceRepository
        Private invoiceAttachmentRepository As IInvoiceAttachmentRepository
        Private userRepository As IUserRepository
        Private userRoleRepository As IUserRoleRepository
        Private productCategoryRepository As IProductCategoryRepository
        Private productRepository As IProductRepository
        Private bankAccountRepository As IBankAccountRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal domainRepository As IDomainRepository,
                ByVal tldHostRepository As ITLDHostRepository,
                ByVal invoiceRepository As IInvoiceRepository,
                ByVal invoiceAttachmentRepository As IInvoiceAttachmentRepository,
                ByVal userRepository As IUserRepository,
                ByVal userRoleRepository As IUserRoleRepository,
                ByVal productRepository As IProductRepository,
                ByVal productCategoryRepository As IProductCategoryRepository,
                ByVal bankAccountRepository As IBankAccountRepository)
            Me.unitOfWork = unitOfWork
            Me.domainRepository = domainRepository
            Me.tldHostRepository = tldHostRepository
            Me.invoiceRepository = invoiceRepository
            Me.invoiceAttachmentRepository = invoiceAttachmentRepository
            Me.userRepository = userRepository
            Me.userRoleRepository = userRoleRepository
            Me.productRepository = productRepository
            Me.productCategoryRepository = productCategoryRepository
            Me.bankAccountRepository = bankAccountRepository
        End Sub

        '
        ' GET: /Dashboard

        Function Index(ByVal filter As String, Optional ByVal p As Integer = 1) As ActionResult

            ' clear all session
            If Not Session("tempdomain") Is Nothing Then Session("tempdomain") = Nothing
            If Not Session("tempinvoice") Is Nothing Then Session("tempinvoice") = Nothing

            Dim mydomains As List(Of Domain) = domainRepository.GetAll.Where(Function(w) w.RegisterBy = Me.User.Identity.Name).ToList
            For Each mydomain As Domain In mydomains
                If mydomain.Status = "ACTIVE" And mydomain.DomainExpireDate <= Now.Date.AddDays(30) Then
                    mydomain.DisplayOrder = 0
                ElseIf mydomain.Status = "REGISTERED" Then
                    mydomain.DisplayOrder = 1
                ElseIf mydomain.Status = "EXTEND" Then
                    mydomain.DisplayOrder = 2
                Else
                    mydomain.DisplayOrder = 3
                End If
            Next
            If Not String.IsNullOrEmpty(filter) Then
                filter = filter.ToLower()
                mydomains = mydomains.Where(Function(w) w.DomainName.ToLower().Contains(filter) Or w.Status.ToLower().Contains(filter) Or w.Invoices.Any(Function(a) a.InvoiceID.ToLower().Contains(filter))).ToList()
                ViewBag.Filtered = True
            End If
            Return View(mydomains.OrderBy(Function(o) o.DisplayOrder).ToList().ToPagedList(p, 10))
        End Function

        Function Details(ByVal i As String) As ActionResult
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

        Function Register(ByVal cat As String, Optional ByVal s As Integer = 0) As ActionResult

            Dim model

            If Session("tempdomain") Is Nothing Then Session("tempdomain") = New Domain()
            If Session("tempinvoice") Is Nothing Then Session("tempinvoice") = New Invoice()

            Dim tempdomain As Domain = Session("tempdomain")
            Dim tempinvoice As Invoice = Session("tempinvoice")

            ' -- backward --
            If s > 0 Then
                If s < 4 Then
                    tempinvoice.ProductTermNumber = Nothing
                    If s < 3 Then
                        tempdomain.DomainName = Nothing
                        If s < 2 Then
                            tempinvoice.ProductID = Nothing
                        End If
                    End If
                End If
            End If
            ' --

            If String.IsNullOrEmpty(tempinvoice.ProductID) Or (s = 1 And Not String.IsNullOrEmpty(tempdomain.DomainRegID)) Then
                Dim ProductCategories As New SelectList(productCategoryRepository.GetAll().ToList(), "ProductCategoryID", "ProductCategoryName", cat)
                ViewBag.cat = ProductCategories
                model = productRepository.GetAll().Where(Function(w) w.ProductCategoryID = IIf(cat Is Nothing, ProductCategories.First.Value, cat)).ToList
                Return View("Register_Step1", model)
            End If

            If String.IsNullOrEmpty(tempdomain.DomainName) Or (s = 2 And Not String.IsNullOrEmpty(tempdomain.DomainRegID)) Then
                If tempdomain.DomainExpireDate <= Now.Date Then
                    tempinvoice.DomainRegTermNumber = 1
                    tempinvoice.DomainRegTermPeriod = "Year"
                End If
                Dim selectedproduct As Product = productRepository.GetAll(includeProperties:="ProductCategory").Where(Function(w) w.ProductID = tempinvoice.ProductID).FirstOrDefault
                If Not selectedproduct Is Nothing Then
                    ViewBag.SelectedProduct = selectedproduct.ProductCategory.ProductCategoryName & " - " & selectedproduct.ProductName
                End If
                ViewBag.TLD = New SelectList(tldHostRepository.GetAll().ToList(), "TLD", "TLD", ".id")
                If Not domainRepository.GetById(tempdomain.DomainRegID) Is Nothing Then
                    ViewBag.DomainName = domainRepository.GetById(tempdomain.DomainRegID).DomainName
                End If
                model = New Domain
                If Not String.IsNullOrEmpty(tempdomain.DomainRegID) Then
                    model = tempdomain
                End If
                Return View("Register_Step2", model)
            End If

            If tempinvoice.ProductTermNumber Is Nothing Or (s = 3 And Not String.IsNullOrEmpty(tempdomain.DomainRegID)) Then
                Dim product As Product = productRepository.GetById(tempinvoice.ProductID)
                Dim billingCycle As New List(Of Object)
                If product.Price > 0 Then
                    Dim max As Integer = Math.Floor(12 / product.Counter)
                    For i As Integer = 1 To max
                        billingCycle.Add(New With {.text = product.Counter * i & " " & product.UnitPeriod & IIf(product.Counter * i = 1, "", "s") & " Price : Rp " & Format(product.Price * i, "#,##0.##"), .value = i})
                    Next
                    If billingCycle.Count() > 0 Then
                        ViewBag.billingCycle = New SelectList(billingCycle, "value", "text")
                    Else
                        ViewBag.BillingCycle = Nothing
                    End If
                End If
                ViewBag.Product = product.ProductCategory.ProductCategoryName & " - " & product.ProductName
                ViewBag.ProductDesc = product.ProductDesc
                Return View("Register_Step3")
            End If

            Dim purchasedproduct As Product = productRepository.GetById(tempinvoice.ProductID)
            model = New List(Of ProductPurchasedViewModel)
            If String.IsNullOrEmpty(tempdomain.DomainRegID) Then
                model.Add(New ProductPurchasedViewModel With {.ProductDesc = "<strong>Domain Registration</strong> - " & tempdomain.DomainName & " - 1 Year", .Price = tempinvoice.DomainRegPrice, .Discount = tempinvoice.DomainRegDiscount})
            Else
                If tempinvoice.DomainRegTermNumber > 0 Then
                    model.Add(New ProductPurchasedViewModel With {.ProductDesc = "<strong>Domain Extend</strong> - " & tempdomain.DomainName & " - 1 Year", .Price = tempinvoice.DomainRegPrice, .Discount = tempinvoice.DomainRegDiscount})
                End If
            End If
            If purchasedproduct.Price > 0 Then model.Add(New ProductPurchasedViewModel With {.ProductDesc = "<strong>" & purchasedproduct.ProductName & "</strong>" & " - " & tempinvoice.ProductTermNumber & " " & tempinvoice.ProductTermPeriod & IIf(tempinvoice.ProductTermNumber = 1, "", "s"), .Price = tempinvoice.ProductPrice, .Discount = tempinvoice.ProductDiscount})

            Dim tldHosts As IEnumerable(Of TLDHost) = From q In tldHostRepository.GetAll() _
                                           Order By q.TLD.Length Descending _
                                           Select q

            Dim tldHost As TLDHost
            For Each item As TLDHost In tldHosts
                If tempdomain.DomainName.IndexOf(item.TLD) = tempdomain.DomainName.Length - item.TLD.Length Then
                    tldHost = item
                    ViewBag.Requirement = System.Uri.UnescapeDataString(tldHost.Requirement)
                    Exit For
                End If
            Next

            Return View("Register_Step4", model)

        End Function

        <HttpPost()> _
        Function Register(pid As String, model As Domain, TLD As String,
                          billingCycle As String, confirm As String, extendDomain As String,
                          doc1 As HttpPostedFileBase, doc2 As HttpPostedFileBase, doc3 As HttpPostedFileBase,
                          doc4 As HttpPostedFileBase, doc5 As HttpPostedFileBase) As ActionResult

            If Not String.IsNullOrEmpty(pid) Then
                Return Register_Step1(pid)
            End If

            If (Not String.IsNullOrEmpty(TLD) And Not model Is Nothing) Then
                Return Register_Step2(model, TLD)
            End If

            If Not String.IsNullOrEmpty(extendDomain) Then
                Return Register_Step2_Extend(model, extendDomain)
            End If

            If Not String.IsNullOrEmpty(billingCycle) Then
                Return Register_Step3(billingCycle)
            End If

            If Not String.IsNullOrEmpty(confirm) Then
                Return Register_Step4(confirm, doc1, doc2, doc3, doc4, doc5)
            End If

            Return RedirectToAction("Register", "Dashboard")
        End Function

        Function Register_Step1(pid As String) As ActionResult
            Dim tempinvoice As Invoice = Session("tempinvoice")
            tempinvoice.ProductID = pid
            Return RedirectToAction("Register", "Dashboard")
        End Function

        Function Register_Step2(model As Domain, TLD As String) As ActionResult
            Dim tempinvoice As Invoice = Session("tempinvoice")
            Dim tempdomain As Domain = Session("tempdomain")
            Dim selectedproduct As Product = productRepository.GetAll(includeProperties:="ProductCategory").Where(Function(w) w.ProductID = tempinvoice.ProductID).FirstOrDefault
            If Not selectedproduct Is Nothing Then
                ViewBag.SelectedProduct = selectedproduct.ProductCategory.ProductCategoryName & " - " & selectedproduct.ProductName
            End If
            ViewBag.TLD = New SelectList(tldHostRepository.GetAll().ToList(), "TLD", "TLD", TLD)
            If ModelState.IsValid Then
                Try

                    Dim TLDServer As String = tldHostRepository.GetById(TLD).Host
                    Select Case ModuleDomainMgr.DomainAvailability(TLDServer, model.DomainName)
                        Case 0  ' taken
                            ModelState.AddModelError("", model.DomainName & " has already taken") : Return View("Register_Step2", model)
                        Case 1
                            ' ok
                        Case 2  ' error
                            ModelState.AddModelError("", "Error occured! Please check your internet connection") : Return View("Register_Step2", model)
                        Case 3
                            ModelState.AddModelError("", "Domain pattern is not recognized") : Return View("Register_Step2", model)
                        Case Else
                            ModelState.AddModelError("", "Unknown error") : Return View("Register_Step2", model)
                    End Select

                    If domainRepository.GetAll().Any(Function(w) w.DomainName = model.DomainName And w.Status <> "SUSPENDED" And (w.DomainExpireDate.HasValue And w.DomainExpireDate > Now.Date)) Then
                        ModelState.AddModelError("", model.DomainName & " is currently registered by someone else") : Return View("Register_Step2", model)
                    End If

                    tempdomain.DomainName = model.DomainName
                    tempinvoice.DomainRegTermNumber = 1
                    tempinvoice.DomainRegTermPeriod = "Year"


                    Return RedirectToAction("Register", "Dashboard")

                Catch ex As Exception
                    ModelState.AddModelError("", ex.ToString)
                End Try
            End If

            Return View("Register_Step2", model)
        End Function

        Function Register_Step2_Extend(model As Domain, extendDomain As String) As ActionResult
            Dim tempinvoice As Invoice = Session("tempinvoice")
            Dim tempdomain As Domain = Session("tempdomain")
            If extendDomain = "yes" Then
                tempinvoice.DomainRegTermNumber = 1
            Else
                tempinvoice.DomainRegTermNumber = 0
            End If
            tempinvoice.DomainRegTermPeriod = "Year"
            Dim currentDomain As Domain = domainRepository.GetById(model.DomainRegID)
            If Not currentDomain Is Nothing Then
                tempdomain.DomainName = currentDomain.DomainName
            End If
            Return RedirectToAction("Register", "Dashboard", New With {.s = 3})
        End Function

        Function Register_Step3(ByVal billingCycle As String) As ActionResult
            Dim tempinvoice As Invoice = Session("tempinvoice")
            Dim tempdomain As Domain = Session("tempdomain")
            Dim product As Product = productRepository.GetById(tempinvoice.ProductID)

            Dim tldHosts As IEnumerable(Of TLDHost) = From q In tldHostRepository.GetAll() _
                                          Order By q.TLD.Length Descending _
                                          Select q

            Dim tldHost As TLDHost
            For Each item As TLDHost In tldHosts
                If tempdomain.DomainName.IndexOf(item.TLD) = tempdomain.DomainName.Length - item.TLD.Length Then
                    tldHost = item
                    Exit For
                End If
            Next

            With tempinvoice
                .ProductTermNumber = product.Counter * billingCycle
                .ProductTermPeriod = product.UnitPeriod
                .ProductPrice = product.Price * billingCycle
                .ProductDiscount = product.Discount
                If .DomainRegTermNumber > 0 Then
                    .DomainRegPrice = tldHost.Price
                    .DomainRegDiscount = tldHost.Discount
                Else
                    .DomainRegPrice = 0
                    .DomainRegDiscount = 0
                End If
            End With
            Return RedirectToAction("Register", "Dashboard")
        End Function

        Function Register_Step4(ByVal confirm As String,
                                ByVal doc1 As HttpPostedFileBase,
                                ByVal doc2 As HttpPostedFileBase,
                                ByVal doc3 As HttpPostedFileBase,
                                ByVal doc4 As HttpPostedFileBase,
                                ByVal doc5 As HttpPostedFileBase)

            Dim tempinvoice As Invoice = Session("tempinvoice")
            Dim tempdomain As Domain = Session("tempdomain")

            If confirm = "cancel" Then
                Return RedirectToAction("Index", "Dashboard")
            ElseIf confirm = "submit" Then
                Try

                    ' upload files
                    Dim maxContent As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("MaxUploadFileSizeAllowed"))
                    Dim allowedFileTypes() As String = Split(System.Configuration.ConfigurationManager.AppSettings("UploadFileFormatAllowed").ToString(), ";")
                    Dim fileName As String = String.Empty
                    Dim fileExt As String = String.Empty
                    Dim fileList() As HttpPostedFileBase = {doc1, doc2, doc3, doc4, doc5}

                    Dim process As Boolean = True

                    For i As Integer = 0 To fileList.Count - 1
                        If Not fileList(i) Is Nothing Then
                            If fileList(i).ContentLength > 0 Then
                                fileExt = fileList(i).FileName.Substring(fileList(i).FileName.LastIndexOf(".")).ToLower
                                If Not allowedFileTypes.Contains(fileExt) And process = True Then
                                    ModelState.AddModelError("", "Accepted file type : " & String.Join(", ", allowedFileTypes))
                                    process = False : Exit For
                                End If
                                If fileList(i).ContentLength > maxContent * 1024 * 1024 And process = True Then
                                    ModelState.AddModelError("", "Document " & i & " file is too large, maximum allowed size is : " & maxContent & " MB")
                                    process = False : Exit For
                                End If
                            End If
                        End If
                    Next

                    If process Then

                        Dim loggedinuserid As String = Me.User.Identity.Name
                        With tempinvoice
                            .InvoiceID = invoiceRepository.GetNewInvoiceID()
                            .InvoicedTo = loggedinuserid
                            .CreateDate = Now
                        End With

                        If Not String.IsNullOrEmpty(tempdomain.DomainRegID) Then
                            ' extend service
                            tempinvoice.DomainRegID = tempdomain.DomainRegID
                            With tempdomain
                                .Status = "EXTENDED"
                            End With
                            domainRepository.Update(tempdomain)
                        Else
                            With tempdomain
                                .DomainRegID = domainRepository.GetNewDomainRegID()
                                .RegisterBy = loggedinuserid
                                .RegisterDate = Now
                                .Status = "REGISTERED"
                            End With
                            tempinvoice.DomainRegID = tempdomain.DomainRegID
                            domainRepository.Insert(tempdomain)
                        End If
                        invoiceRepository.Insert(tempinvoice)

                        ' save to db
                        unitOfWork.Commit()

                        ' check directory for upload storage
                        If Not System.IO.Directory.Exists(Server.MapPath("~/Attachment")) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Attachment"))
                        End If
                        If Not System.IO.Directory.Exists(Server.MapPath("~/Attachment/Invoices")) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Attachment/Invoices"))
                        End If

                        Dim seq = 1
                        Dim invoiceAttch = invoiceAttachmentRepository.GetAll().Where(Function(w) w.InvoiceID = tempinvoice.InvoiceID)
                        If Not invoiceAttch Is Nothing Then
                            seq += invoiceAttch.Max(Function(m) m.Seq)
                        End If

                        For i As Integer = 0 To fileList.Count - 1
                            If Not fileList(i) Is Nothing Then
                                If fileList(i).ContentLength > 0 Then
                                    Dim newsequence = invoiceAttachmentRepository.GetNewSequence(tempinvoice.InvoiceID)
                                    fileExt = fileList(i).FileName.Substring(fileList(i).FileName.LastIndexOf(".")).ToLower
                                    fileName = tempinvoice.InvoiceID & "_" & newsequence & fileExt
                                    Dim savePath As String = Path.Combine(Server.MapPath("~/Attachment/Invoices/"), fileName)
                                    fileList(i).SaveAs(savePath)
                                    ' save to db
                                    invoiceAttachmentRepository.Insert(New InvoiceAttachment With {.InvoiceID = tempinvoice.InvoiceID, .Seq = newsequence, .FileName = fileList(i).FileName, .FileLink = Url.Content("~/Attachment/Invoices/") & fileName})
                                    unitOfWork.Commit()
                                End If
                            End If
                        Next

                        ' send notification
                        If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                            Dim loggedinuser As User = userRepository.GetAll().Where(Function(w) w.UserId = loggedinuserid).FirstOrDefault()
                            Dim loggedinusername As String = loggedinuser.UserName

                            ' get admin roles
                            Dim adminroles As List(Of UserRole) = userRoleRepository.GetAll().Where(Function(w) w.RoleId = "AD").Distinct().ToList()

                            ' send notification to all admin
                            Dim mail As New MailMessage()
                            For Each UserRole In adminroles
                                Dim adminuser As User = userRepository.GetById(UserRole.UserId)
                                Dim emailaddress As String = adminuser.UserEmailAddress
                                If Not String.IsNullOrEmpty(emailaddress) Then mail.To.Add(emailaddress)
                            Next
                            mail.Body = "Dear Administrators,<br /><br />"
                            If tempdomain.Status = "REGISTERED" Then
                                mail.Body &= loggedinusername & " (" & loggedinuserid & ") has just register domain name " & tempdomain.DomainName
                                mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Domain Registration"
                            Else
                                mail.Body &= loggedinusername & " (" & loggedinuserid & ") has requested extend service for " & tempdomain.DomainName
                                mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Extend Service"
                            End If

                            mail.IsBodyHtml = True
                            ModuleDomainMgr.SendNotification(mail)

                            ' send notification to user
                            mail = New MailMessage
                            mail.To.Add(loggedinuser.UserEmailAddress)


                            If tempdomain.Status = "REGISTERED" Then
                                mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Domain Registration"
                            Else
                                mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Extend Service"
                            End If
                            mail.Body = "Dear " & loggedinuser.UserName & ",<br /><br />"
                            mail.Body &= "Thank you for using our services. We will process you request and validate your documents at this moment<br />"
                            mail.Body &= "We'll keep you updated for the next process by email and system<br /><br />"
                            mail.Body &= "Thank you<br /><br />"

                            mail.IsBodyHtml = True
                            ModuleDomainMgr.SendNotification(mail)

                        End If

                        ViewBag.Message = "Thank you for using our services<br />" & _
                            "We will process your request soon and we'll keep you updated for the next process " & _
                            "by email and system"

                        Return View("Success")

                    End If
                Catch ex As Exception
                    unitOfWork.RollBack()
                    ModelState.AddModelError("", ex.ToString())
                End Try
            End If

            Dim purchasedproduct As Product = productRepository.GetById(tempinvoice.ProductID)
            Dim model = New List(Of ProductPurchasedViewModel)
            If String.IsNullOrEmpty(tempdomain.DomainRegID) Then
                model.Add(New ProductPurchasedViewModel With {.ProductDesc = "<strong>Domain Registration</strong> - " & tempdomain.DomainName & " - 1 Year", .Price = tempinvoice.DomainRegPrice, .Discount = tempinvoice.DomainRegDiscount})
            Else
                model.Add(New ProductPurchasedViewModel With {.ProductDesc = "<strong>Domain Extend</strong> - " & tempdomain.DomainName & " - 1 Year", .Price = tempinvoice.DomainRegPrice, .Discount = tempinvoice.DomainRegDiscount})
            End If
            If purchasedproduct.Price > 0 Then model.Add(New ProductPurchasedViewModel With {.ProductDesc = "<strong>" & purchasedproduct.ProductName & "</strong>" & " - " & tempinvoice.ProductTermNumber & " " & tempinvoice.ProductTermPeriod & IIf(tempinvoice.ProductTermNumber = 1, "", "s"), .Price = tempinvoice.ProductPrice, .Discount = tempinvoice.ProductDiscount})
            Dim tld As String = tempdomain.DomainName.Substring(tempdomain.DomainName.ToString().LastIndexOf("."))
            Dim tldHost As TLDHost = tldHostRepository.GetById(tld)
            If Not tldHost.Requirement Is Nothing Then
                ViewBag.Requirement = System.Uri.UnescapeDataString(tldHost.Requirement)
            End If
            Return View("Register_Step4", model)
        End Function

        Public Function UploadDocument(ByVal invoice As String)
            Dim model As Invoice = invoiceRepository.GetById(invoice)
            If Not invoiceRepository.GetAll().Any(Function(a) a.InvoiceID = invoice And a.InvoicedTo = User.Identity.Name) Or _
                model.DocumentIsComplete = True Then
                Return RedirectToAction("Index", "Home")
            End If
            Dim tld As String = model.Domain.DomainName.Substring(model.Domain.DomainName.ToString().LastIndexOf("."))
            Dim tldHost As TLDHost = tldHostRepository.GetById(tld)
            If Not tldHost.Requirement Is Nothing Then
                ViewBag.Requirement = System.Uri.UnescapeDataString(tldHost.Requirement)
            End If
            Return View(model)
        End Function

        <HttpPost()> _
        Public Function UploadDocument(ByVal model As Invoice,
                                       ByVal doc1 As HttpPostedFileBase,
                                       ByVal doc2 As HttpPostedFileBase,
                                       ByVal doc3 As HttpPostedFileBase,
                                       ByVal doc4 As HttpPostedFileBase,
                                       ByVal doc5 As HttpPostedFileBase)

            Dim loggedinuserid As String = Me.User.Identity.Name

            Dim invoice As Invoice = invoiceRepository.GetById(model.InvoiceID)

            ' upload files
            Dim maxContent As Integer = CInt(System.Configuration.ConfigurationManager.AppSettings("MaxUploadFileSizeAllowed"))
            Dim allowedFileTypes() As String = Split(System.Configuration.ConfigurationManager.AppSettings("UploadFileFormatAllowed").ToString(), ";")
            Dim fileName As String = String.Empty
            Dim fileExt As String = String.Empty
            Dim fileList() As HttpPostedFileBase = {doc1, doc2, doc3, doc4, doc5}

            Dim process As Boolean = True

            For i As Integer = 0 To fileList.Count - 1
                If Not fileList(i) Is Nothing Then
                    If fileList(i).ContentLength > 0 Then
                        fileExt = fileList(i).FileName.Substring(fileList(i).FileName.LastIndexOf(".")).ToLower
                        If Not allowedFileTypes.Contains(fileExt) And process = True Then
                            ModelState.AddModelError("", "Accepted file type : " & String.Join(", ", allowedFileTypes))
                            process = False : Exit For
                        End If
                        If fileList(i).ContentLength > maxContent * 1024 * 1024 And process = True Then
                            ModelState.AddModelError("", "Document " & i & " file is too large, maximum allowed size is : " & maxContent & " MB")
                            process = False : Exit For
                        End If
                    End If
                End If
            Next

            If process Then

                ' check directory for upload storage
                If Not System.IO.Directory.Exists(Server.MapPath("~/Attachment")) Then
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Attachment"))
                End If
                If Not System.IO.Directory.Exists(Server.MapPath("~/Attachment/Invoices")) Then
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Attachment/Invoices"))
                End If

                Dim attachments = invoiceAttachmentRepository.GetAll().Where(Function(w) w.InvoiceID = model.InvoiceID)

                ' delete files
                For Each attachment In attachments
                    If System.IO.File.Exists(Server.MapPath(attachment.FileLink)) Then
                        System.IO.File.Delete(Server.MapPath(attachment.FileLink))
                    End If
                    invoiceAttachmentRepository.Delete(attachment)
                Next

                invoice.DocumentVerifiedBy = Nothing
                invoice.DocumentVerifiedDate = Nothing
                invoice.DocumentVerificationRemark = Nothing
                invoiceRepository.Update(invoice)
                unitOfWork.Commit()

                Dim seq = 1
                Dim invoiceAttch = invoiceAttachmentRepository.GetAll().Where(Function(w) w.InvoiceID = model.InvoiceID)
                If Not invoiceAttch Is Nothing Then
                    seq += invoiceAttch.Max(Function(m) m.Seq)
                End If

                For i As Integer = 0 To fileList.Count - 1
                    If Not fileList(i) Is Nothing Then
                        If fileList(i).ContentLength > 0 Then
                            Dim newsequence = invoiceAttachmentRepository.GetNewSequence(model.InvoiceID)
                            fileExt = fileList(i).FileName.Substring(fileList(i).FileName.LastIndexOf(".")).ToLower
                            fileName = model.InvoiceID & "_" & newsequence & fileExt
                            Dim savePath As String = Path.Combine(Server.MapPath("~/Attachment/Invoices/"), fileName)
                            fileList(i).SaveAs(savePath)
                            ' save to db
                            invoiceAttachmentRepository.Insert(New InvoiceAttachment With {.InvoiceID = model.InvoiceID, .Seq = newsequence, .FileName = fileList(i).FileName, .FileLink = Url.Content("~/Attachment/Invoices/") & fileName})
                            unitOfWork.Commit()
                        End If
                    End If
                Next

                ' send notification
                If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                    Dim loggedinuser As User = userRepository.GetAll().Where(Function(w) w.UserId = loggedinuserid).FirstOrDefault()
                    Dim loggedinusername As String = loggedinuser.UserName

                    ' get admin roles
                    Dim adminroles As List(Of UserRole) = userRoleRepository.GetAll().Where(Function(w) w.RoleId = "AD").Distinct().ToList()

                    ' send notification to all admin
                    Dim mail As New MailMessage()
                    For Each UserRole In adminroles
                        Dim adminuser As User = userRepository.GetById(UserRole.UserId)
                        Dim emailaddress As String = adminuser.UserEmailAddress
                        If Not String.IsNullOrEmpty(emailaddress) Then mail.To.Add(emailaddress)
                    Next
                    mail.Body = "Dear Administrators,<br /><br />"
                    mail.Body &= loggedinusername & " (" & loggedinuserid & ") has just renew attached document "
                    mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Renewed Attached Document"

                    mail.IsBodyHtml = True
                    ModuleDomainMgr.SendNotification(mail)

                End If

                ViewBag.Message = "You have successfully update your documents<br />" & _
                    "We will process your registration soon and we'll keep you updated for the next process " & _
                    "by email and system"

                Return View("Success")
            End If

            model = invoiceRepository.GetById(model.InvoiceID)
            Dim tld As String = model.Domain.DomainName.Substring(model.Domain.DomainName.ToString().LastIndexOf("."))
            Dim tldHost As TLDHost = tldHostRepository.GetById(tld)
            If Not tldHost.Requirement Is Nothing Then
                ViewBag.Requirement = System.Uri.UnescapeDataString(tldHost.Requirement)
            End If
            Return View(model)
        End Function

        Public Function PaymentConfirmation(ByVal invoiceid As String)
            Dim model As Invoice = invoiceRepository.GetById(invoiceid)
            If Not invoiceRepository.GetAll().Any(Function(a) a.InvoiceID = invoiceid And a.InvoicedTo = User.Identity.Name) Or _
                model.PaymentIsComplete = True Then
                Return RedirectToAction("Index", "Home")
            End If
            ViewBag.PaymentMethod = New SelectList(New String() {"iBanking", "ATM", "Setor Tunai"})
            Dim bankAccounts As List(Of BankAccount) = bankAccountRepository.GetAll()
            For Each BankAccount In bankAccounts
                BankAccount.Bank = BankAccount.Bank & " - " & BankAccount.BankAccountName & " #" & BankAccount.BankAccountNo
            Next
            ViewBag.BankAccounts = New SelectList(bankAccounts, "BankAccountID", "Bank")

            Return View(model)
        End Function

        <HttpPost()> _
        Public Function PaymentConfirmation(ByVal model As Invoice)
            Dim editedmodel As Invoice = invoiceRepository.GetById(model.InvoiceID)
            editedmodel.PaymentMethod = model.PaymentMethod
            editedmodel.Bank = model.Bank
            editedmodel.BankAccountNo = model.BankAccountNo
            editedmodel.BankAccountName = model.BankAccountName
            editedmodel.BankAccountID = model.BankAccountID
            editedmodel.PaymentDate = model.PaymentDate.Value.Date
            editedmodel.PaymentAmount = model.PaymentAmount
            editedmodel.ValidationNo = model.ValidationNo
            editedmodel.PaymentVerifiedBy = Nothing
            editedmodel.PaymentVerifiedDate = Nothing
            editedmodel.PaymentVerificationRemark = Nothing
            invoiceRepository.Update(editedmodel)
            unitOfWork.Commit()

            ' send notification
            If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                Dim loggedinuserid As String = User.Identity.Name
                Dim loggedinuser As User = userRepository.GetAll().Where(Function(w) w.UserId = loggedinuserid).FirstOrDefault()
                Dim loggedinusername As String = loggedinuser.UserName

                ' get admin roles
                Dim adminroles As List(Of UserRole) = userRoleRepository.GetAll().Where(Function(w) w.RoleId = "AD").Distinct().ToList()

                ' send notification to all admin
                Dim mail As New MailMessage()
                For Each UserRole In adminroles
                    Dim adminuser As User = userRepository.GetById(UserRole.UserId)
                    Dim emailaddress As String = adminuser.UserEmailAddress
                    If Not String.IsNullOrEmpty(emailaddress) Then mail.To.Add(emailaddress)
                Next
                mail.Body = "Dear Administrators,<br /><br />"
                mail.Body &= loggedinusername & " (" & loggedinuserid & ") has just input payment confirmation"
                mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Payment Confirmation"

                mail.IsBodyHtml = True
                ModuleDomainMgr.SendNotification(mail)

            End If

            ViewBag.Message = "Thank you for submitting your payment confirmation <br />" & _
                "We will validate it before activating your domain"
            Return View("Success")
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

        Function Extend(id As String) As ActionResult
            Dim domain As Domain = domainRepository.GetById(id)
            Return PartialView("_ExtendMyDomain", domain)
        End Function

        <HttpPost()> _
        Function Extend(model As Domain) As ActionResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                If Session("tempdomain") Is Nothing Then
                    Dim domain As Domain = domainRepository.GetById(model.DomainRegID)
                    domain.DomainName = Nothing
                    Session("tempdomain") = domain
                End If
                Return RedirectToAction("Register", "Dashboard")
            End If
            Return View("Error")
        End Function

        Function Renew(id As String) As ActionResult
            Dim domain As Domain = domainRepository.GetById(id)
            Return PartialView("_RenewMyDomain", domain)
        End Function

        <HttpPost()> _
        Function Renew(model As Domain) As ActionResult
            Dim ret As String = ""
            If ModelState.IsValid Then
                If Session("tempdomain") Is Nothing Then
                    Dim domain As Domain = domainRepository.GetById(model.DomainRegID)
                    domain.DomainName = Nothing
                    Session("tempdomain") = domain
                End If
                Return RedirectToAction("Register", "Dashboard")
            End If
            Return View("Error")
        End Function

        Function Suspend(id As String) As ActionResult
            Dim domain As Domain = domainRepository.GetById(id)
            Return PartialView("_SuspendMyDomain", domain)
        End Function

        <HttpPost()> _
        Function Suspend(model As Domain) As ActionResult
            If ModelState.IsValid Then

                Dim loggedinuserid As String = Me.User.Identity.Name

                Dim editeddomain As Domain = domainRepository.GetById(model.DomainRegID)
                Dim editeddomainname As String = editeddomain.DomainName
                editeddomain.Status = "SUSPENDED"
                editeddomain.SuspendBy = loggedinuserid
                editeddomain.SuspendDate = Now.Date
                unitOfWork.Commit()

                If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                    Dim loggedinuser As User = userRepository.GetAll().Where(Function(w) w.UserId = loggedinuserid).FirstOrDefault()
                    Dim loggedinusername As String = loggedinuser.UserName

                    ' get admin roles
                    Dim adminroles As List(Of UserRole) = userRoleRepository.GetAll().Where(Function(w) w.RoleId = "AD").Distinct().ToList()

                    ' send notification to all admin
                    Dim mail As New MailMessage()
                    mail.To.Add(loggedinuser.UserEmailAddress)
                    mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Domain Suspend"
                    mail.Body = "Dear " & loggedinusername & ",<br /><br />"
                    mail.Body &= "You have suspend your domain " & editeddomain.DomainName & "<br />"
                    mail.Body &= "Thank you for using our services"
                    mail.IsBodyHtml = True
                    ModuleDomainMgr.SendNotification(mail)

                    mail = New MailMessage
                    For Each UserRole In adminroles
                        Dim adminuser As User = userRepository.GetById(UserRole.UserId)
                        Dim emailaddress As String = adminuser.UserEmailAddress
                        If Not String.IsNullOrEmpty(emailaddress) Then mail.To.Add(emailaddress)
                    Next
                    mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Domain Suspend"
                    mail.Body = "Dear Administrators,<br /><br />"
                    mail.Body &= loggedinusername & " (" & loggedinuserid & ") has suspend domain " & editeddomain.DomainName
                    mail.IsBodyHtml = True
                    ModuleDomainMgr.SendNotification(mail)
                End If

                ViewBag.Message = "Your domain has successfully suspended<br />" & _
                    "Thank you for using our services"

                Return View("Success")

            End If
            Return View("Error")
        End Function

        Function ShowInvoices(id As String) As ActionResult
            Dim model = invoiceRepository.GetAll().Where(Function(w) w.DomainRegID = id).OrderBy(Function(o) o.CreateDate).ToList()
            Return PartialView("_InvoiceList", model)
        End Function

    End Class

End Namespace
