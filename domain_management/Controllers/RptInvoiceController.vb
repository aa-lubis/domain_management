
Imports System.IO

Imports domain_management.Entities
Imports domain_management.Interfaces
Imports domain_management.ViewModels

Namespace domain_management
    Public Class RptInvoiceController
        Inherits System.Web.Mvc.Controller

        Private invoiceRepository As IInvoiceRepository
        Private productRepository As IProductRepository
        Private tldHostRepository As ITLDHostRepository
        Private userRepository As IUserRepository

        Public Sub New(ByVal invoiceRepository As IInvoiceRepository,
                       ByVal productRepository As IProductRepository,
                       ByVal tldHostRepository As ITLDHostRepository,
                       ByVal userRepository As IUserRepository)
            Me.invoiceRepository = invoiceRepository
            Me.productRepository = productRepository
            Me.tldHostRepository = tldHostRepository
            Me.userRepository = userRepository
        End Sub

        '
        ' GET: /RptInvoice

        Function Index(ByVal FromDate As Date?, ByVal ToDate As Date?,
                       ByVal payment As String, ByVal product As String,
                       ByVal tld As String, ByVal domainexpirymonth As String,
                       ByVal domainexpiryyear As String, ByVal productexpirymonth As String,
                       ByVal productexpiryyear As String,
                       ByVal submit As String) As ActionResult

            ViewBag.InfoPayment = payment
            ViewBag.InfoFromDate = FromDate
            ViewBag.InfoToDate = ToDate
            If Not product Is Nothing Then
                ViewBag.InfoProduct = productRepository.GetAll().Where(Function(w) w.ProductID = product).Select(Function(s) s.ProductName).SingleOrDefault()
            End If
            ViewBag.InfoTld = tld
            If Not String.IsNullOrEmpty(domainexpirymonth) Then
                ViewBag.InfoDomainExpiryMonth = Format(New Date(Now.Year, CInt(domainexpirymonth), 1), "MMMM")
            End If
            ViewBag.InfoDomainExpiryYear = domainexpiryyear
            If Not String.IsNullOrEmpty(productexpirymonth) Then
                ViewBag.InfoProductExpiryMonth = Format(New Date(Now.Year, CInt(productexpirymonth), 1), "MMMM")
            End If
            ViewBag.InfoProductExpiryYear = productexpiryyear

            Dim model As New List(Of ReportInvoice)
            If Not FromDate Is Nothing And Not ToDate Is Nothing Then

                Dim invoices As IEnumerable(Of Invoice) = invoiceRepository.GetAll(includeProperties:="Domain,Product").Where(Function(w) w.CreateDate >= CDate(FromDate).Date And w.CreateDate <= CDate(ToDate).Date).ToList()
                If payment.ToLower() = "unpaid" Then
                    invoices = invoices.Where(Function(w) w.PaymentIsComplete = False).ToList()
                End If
                If payment.ToLower() = "paid" Then
                    invoices = invoices.Where(Function(w) w.PaymentIsComplete = True).ToList()
                End If
                If Not String.IsNullOrEmpty(product) Then
                    invoices = invoices.Where(Function(w) w.ProductID = product).ToList()
                End If
                If Not String.IsNullOrEmpty(tld) Then
                    invoices = invoices.Where(Function(w) w.Domain.DomainName.Substring(w.Domain.DomainName.IndexOf(".")) = tld.ToLower()).ToList()
                End If
                If Not String.IsNullOrEmpty(domainexpirymonth) Then
                    invoices = invoices.Where(Function(w) w.Domain.DomainExpireDate.GetValueOrDefault.Month = domainexpirymonth).ToList()
                End If
                If Not String.IsNullOrEmpty(domainexpiryyear) Then
                    invoices = invoices.Where(Function(w) w.Domain.DomainExpireDate.GetValueOrDefault.Month = domainexpiryyear).ToList()
                End If

                If Not String.IsNullOrEmpty(productexpirymonth) Then
                    invoices = invoices.Where(Function(w) w.Domain.ProductExpireDate.GetValueOrDefault.Month = productexpirymonth).ToList()
                End If
                If Not String.IsNullOrEmpty(productexpiryyear) Then
                    invoices = invoices.Where(Function(w) w.Domain.ProductExpireDate.GetValueOrDefault.Year = productexpiryyear).ToList()
                End If


                For Each Invoice As Invoice In invoices
                    Dim invoicedto As User = userRepository.GetById(Invoice.InvoicedTo)
                    If Not invoicedto Is Nothing Then
                        Invoice.InvoicedTo = invoicedto.UserName
                    End If
                    Dim reportInvoice As New ReportInvoice
                    With reportInvoice
                        .ActivateDate = Invoice.Domain.ActivateDate
                        .CreatedDate = Invoice.CreateDate
                        .DomainExpireDate = Invoice.Domain.DomainExpireDate
                        .DomainName = Invoice.Domain.DomainName
                        .InvoicedTo = Invoice.InvoicedTo
                        .InvoiceID = Invoice.InvoiceID
                        .Price = Invoice.DomainRegPrice - Invoice.DomainRegDiscount + Invoice.ProductPrice - Invoice.ProductDiscount
                        .ProductExpireDate = Invoice.Domain.ProductExpireDate
                        .ProductName = Invoice.Product.ProductName
                        Select Case Invoice.Domain.Status
                            Case "REGISTERED"
                                .Status = "Pending"
                            Case "EXTENDED"
                                .Status = "Extend Pending"
                            Case "SUSPENDED"
                                .Status = "Suspended"
                            Case "ACTIVE"
                                .Status = "Active"
                        End Select
                        If Invoice.PaymentIsComplete = True Then
                            .PaymentStatus = "Paid"
                        Else
                            .PaymentStatus = "Unpaid"
                        End If
                    End With
                    model.Add(reportInvoice)
                Next

                If submit = "exporttopdf" Then
                    Return ExportAsPdf(model)
                End If

                ViewBag.ExportAllowed = True
            End If


            Dim yearoptions As New List(Of SelectListItem)
            yearoptions.Add(New SelectListItem() With {.Value = "", .Text = "- All -"})
            For i As Integer = Now.Year - 5 To Now.Year + 5
                yearoptions.Add(New SelectListItem() With {.Value = i, .Text = i})
            Next

            Dim monthoptions As New List(Of SelectListItem)
            monthoptions.Add(New SelectListItem() With {.Value = "", .Text = "- All -"})
            For i As Integer = 1 To 12
                monthoptions.Add(New SelectListItem() With {.Text = Format(New Date(Now.Year, i, 1), "MMMM"), .Value = i})
            Next

            Dim tldoptions As New List(Of SelectListItem)
            tldoptions.Add(New SelectListItem() With {.Value = "", .Text = "- All -"})
            For Each TLDHost As TLDHost In tldHostRepository.GetAll()
                tldoptions.Add(New SelectListItem() With {.Value = TLDHost.TLD, .Text = TLDHost.TLD})
            Next

            Dim productoptions As New List(Of SelectListItem)
            productoptions.Add(New SelectListItem() With {.Value = "", .Text = "- All -"})
            For Each productItem As Product In productRepository.GetAll()
                productoptions.Add(New SelectListItem() With {.Value = productItem.ProductID, .Text = productItem.ProductName})
            Next

            Dim paymentoptions As New List(Of SelectListItem)
            paymentoptions.Add(New SelectListItem() With {.Value = "", .Text = "- All -"})
            paymentoptions.Add(New SelectListItem() With {.Value = "Unpaid", .Text = "Unpaid"})
            paymentoptions.Add(New SelectListItem() With {.Value = "Paid", .Text = "Paid"})

            ViewBag.domainexpirymonth = New SelectList(monthoptions, "Value", "Text", domainexpirymonth)
            ViewBag.productexpirymonth = New SelectList(monthoptions, "Value", "Text", productexpirymonth)
            ViewBag.domainexpiryyear = New SelectList(yearoptions, "Value", "Text", domainexpiryyear)
            ViewBag.productexpiryyear = New SelectList(yearoptions, "Value", "Text", productexpiryyear)
            ViewBag.tld = New SelectList(tldoptions, "Value", "Text", tld)
            ViewBag.product = New SelectList(productoptions, "Value", "Text", product)
            ViewBag.payment = New SelectList(paymentoptions, "Value", "Text", payment)

            Return View(model)
        End Function

        Public Function ExportAsPdf(ByVal model As List(Of ReportInvoice)) As FileStreamResult

            Dim workStream As New MemoryStream
            Dim document As New iTextSharp.text.Document
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate)
            Dim pdfWriter As iTextSharp.text.pdf.PdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, workStream)
            pdfWriter.CloseStream = False

            document.Open()

            Dim html As String = RenderRazorViewToString(Me.ControllerContext, "_Content", model)
            Dim htmlContext As New iTextSharp.tool.xml.pipeline.html.HtmlPipelineContext(Nothing)
            htmlContext.SetTagFactory(iTextSharp.tool.xml.html.Tags.GetHtmlTagProcessorFactory())

            Dim cssResolver = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().GetDefaultCssResolver(False)
            cssResolver.AddCssFile(Server.MapPath("~/Content/report.css"), True)

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

    End Class
End Namespace
