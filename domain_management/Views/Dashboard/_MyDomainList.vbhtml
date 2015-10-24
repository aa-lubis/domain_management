@ModelType IEnumerable(Of domain_management.Entities.Domain)
<style>
    .badge-important {
        background-color: red;
        background-image: -moz-linear-gradient(top, #FF0000, #CC0000);
        background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#FF0000), to(#CC0000));
        background-image: -webkit-linear-gradient(top, #FF0000, #CC0000);
        background-image: -o-linear-gradient(top, #FF0000, #CC0000);
        background-image: linear-gradient(to bottom, #FF0000, #CC0000);
    }
</style>

@For Each item In Model
  
                                
    Dim DomainID As String = item.DomainRegID
                   
    Dim status As String = item.Status
    Dim statusclass As String = ""
    Dim trclass As String = ""
    Dim domainexpdateclass As String = ""
    Dim productexpdateclass As String = ""
       
    Dim DomainExpireDate As New Date
    Dim ProductExpireDate As New Date
    Dim DomainIsTobeExpired As Boolean = False
    Dim ProductIsTobeExpired As Boolean = False
   
    If Not item.DomainExpireDate Is Nothing Then
        DomainExpireDate = item.DomainExpireDate
        If status = "ACTIVE" And Now.Date >= DomainExpireDate Then
            status = "EXPIRED"
        End If
        If DomainExpireDate <= Now.Date.AddDays(30) Then
            domainexpdateclass = "text-error"
        End If
        
    End If
    If Not item.ProductExpireDate Is Nothing Then
        ProductExpireDate = item.ProductExpireDate
        If ProductExpireDate <= Now.Date.AddDays(30) Then
            productexpdateclass = "text-error"
        End If
    End If
        
    Select Case status.ToString
        Case "ACTIVE"
            status = "Active"
            statusclass = "text-success"
        Case "REGISTERED"
            status = "Pending"
            statusclass = "text-info"
        Case "EXTENDED"
            status = "Extend Pending"
            statusclass = "text-info"
        Case "SUSPENDED"
            status = "Suspended"
            statusclass = "text-error"
        Case "EXPIRED"
            status = "Expired"
            statusclass = "text-error"
            trclass = "error"
    End Select
    
    Dim DomainItemID As String = "domainitem_" & DomainID
    Dim invoice As domain_management.Entities.Invoice = item.Invoices.OrderByDescending(Function(o) o.CreateDate).FirstOrDefault()
    
    @<tr id="@DomainItemID" class="@trclass">
        @*<td><strong style="text-transform: uppercase">@Html.ActionLink(Html.DisplayFor(Function(m) item.DomainName).ToString(), "Invoice", "Dashboard", New With {.i = item.InvoiceID}, New With {.title = "click to view invoice"})</strong></td>*@
        <td><a href="http://@item.DomainName" target="_blank" style="font-size: 1.2em">@item.DomainName</a></td>
        <td>@Html.DisplayFor(Function(m) item.RegisterDate)</td>
        <td>
            <span class="@domainexpdateclass">@Html.DisplayFor(Function(m) item.DomainExpireDate)</span>
        </td>
        <td>
            <span class="@productexpdateclass">@Html.DisplayFor(Function(m) item.ProductExpireDate)</span>
        </td>
        <td><span class="@statusclass">@status</span></td>
        <td style="text-align: center; width: 20px">
            @If (invoice.DocumentIsComplete = False And Not String.IsNullOrEmpty(invoice.DocumentVerifiedBy)) Or _
                (invoice.DocumentIsComplete = True And String.IsNullOrEmpty(invoice.BankAccountNo)) Or _
                (invoice.PaymentIsComplete = False And Not String.IsNullOrEmpty(invoice.BankAccountNo) And Not String.IsNullOrEmpty(invoice.PaymentVerifiedBy)) Or _
                (invoice.Domain.Status.ToLower() = "active" And invoice.Domain.DomainExpireDate <= Now.AddDays(30)) Or _
                (invoice.Domain.Status.ToLower() = "active" And invoice.Domain.ProductExpireDate <= Now.AddDays(30)) Then
                @<span class="badge badge-important">!</span>
    End If
        </td>
        <td style="text-align: center">
            <a href="@Url.Action("Details", "Dashboard", New With {.i = item.DomainRegID})">Detail</a>
        </td>
    </tr>

Next

