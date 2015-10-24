@ModelType IEnumerable(Of domain_management.ViewModels.ReportInvoice)
<p style="font-weight: bold; font-size: 20px; margin-bottom: 20px">INVOICE</p>
<table>
    @If Not ViewBag.InfoFromDate Is Nothing Then
        @Html.Raw("<tr><td>From</td><td>&nbsp;:&nbsp;</td><td>" & Format(CDate(ViewBag.InfoFromDate), "dd-MMMM-yyyy") & "</td></tr>")
    End If
    @If Not ViewBag.infoToDate Is Nothing Then
        @Html.Raw("<tr><td>To</td><td>&nbsp;:&nbsp;</td><td>" & Format(CDate(ViewBag.InfoToDate), "dd-MMMM-yyyy") & "</td></tr>")
    End If
    @If Not String.IsNullOrEmpty(ViewBag.InfoPayment) Then
        @Html.Raw("<tr><td>Payment</td><td>&nbsp;:&nbsp;</td><td>" & ViewBag.InfoPayment & "</td></tr>")
    End If
    @If Not String.IsNullOrEmpty(ViewBag.InfoTld) Then
        @Html.Raw("<tr><td>Type</td><td>&nbsp;:&nbsp;</td><td>" & ViewBag.InfoTld & "</td></tr>")
    End If
    @If Not String.IsNullOrEmpty(ViewBag.InfoProduct) Then
        @Html.Raw("<tr><td>Product</td><td>&nbsp;:&nbsp;</td><td>" & ViewBag.InfoProduct & "</td></tr>")
    End If

    @If Not String.IsNullOrEmpty(ViewBag.InfoDomainExpiryMonth) Or Not String.IsNullOrEmpty(ViewBag.InfoDomainExpiryYear) Then
        @Html.Raw("<tr><td>Domain Expiry</td><td>&nbsp;:&nbsp;</td><td>" & ViewBag.InfoDomainExpiryMonth & " " & ViewBag.InfoDomainExpiryYear() & "</td></tr>")   
    End If
    @If Not String.IsNullOrEmpty(ViewBag.InfoProductExpiryMonth) Or Not String.IsNullOrEmpty(ViewBag.InfoProductExpiryYear) Then
        @Html.Raw("<tr><td>Product Expiry</td><td>&nbsp;:&nbsp;</td><td>" & ViewBag.InfoProductExpiryMonth & " " & ViewBag.InfoProductExpiryYear() & "</td></tr>")   
    End If
    <tr>
        <td>&nbsp;</td>
        <td></td>
        <td></td>
    </tr>
</table>

<table class="reporttable" style="border-collapse: collapse; width: 100%; border: 1px solid #000">
    <thead>
        <tr>
            <th>Invoice ID</th>
            <th>Created</th>
            <th>Name</th>
            <th>Domain</th>
            <th>Product</th>
            <th>Setup Date</th>
            <th>Domain Expiry Date</th>
            <th>Product Expiry Date</th>
            <th>Price</th>
            <th>Status</th>
            <th>Payment</th>
        </tr>
    </thead>
    <tbody>
        @Code
            Dim Total As Double = 0
        End Code
        @For Each item As domain_management.ViewModels.ReportInvoice In Model
            Total += item.Price
            @<tr>
                <td>@Html.DisplayFor(Function(m) item.InvoiceID)</td>
                <td>
                    @If Not item.CreatedDate Is Nothing Then
                        @Format(item.CreatedDate, "dd-MMM-yyyy")
            End If
                </td>
                <td>@Html.DisplayFor(Function(m) item.InvoicedTo)</td>
                <td>@Html.DisplayFor(Function(m) item.DomainName)</td>
                <td>@Html.DisplayFor(Function(m) item.ProductName)</td>
                <td>
                    @If Not item.ActivateDate Is Nothing Then
                        @Format(item.ActivateDate, "dd-MMM-yyyy")  
            End If
                </td>
                <td>
                    @If Not item.DomainExpireDate Is Nothing Then
                        @Format(item.DomainExpireDate, "dd-MMM-yyyy")  
            End If
                </td>
                <td>
                    @If Not item.ProductExpireDate Is Nothing Then
                        @Format(item.ProductExpireDate, "dd-MMM-yyyy")  
            End If
                </td>
                <td style="text-align: right">
                    @Format(item.Price, "#,##0.##")
                </td>
                <td style="text-align: center">
                    @Html.DisplayFor(Function(m) item.Status)

                </td>
                <td style="text-align: center">
                    <span style="@IIf(item.PaymentStatus = "Unpaid", "color: #DD0000", "")">@item.PaymentStatus</span>
                </td>
            </tr>
                             
        Next
    </tbody>
    <tfoot>
        <tr>
            <th colspan="8">TOTAL
            </th>
            <th style="text-align: right">
                @Format(Total, "#,##0.##")
            </th>
            <th colspan="2"></th>
        </tr>
    </tfoot>
</table>
<br />
