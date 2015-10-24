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
        
    Dim ExpireDate As New Date
    If Not item.DomainExpireDate Is Nothing Then
        ExpireDate = item.DomainExpireDate
        If status = "ACTIVE" And ExpireDate < Now.Date Then
            status = "EXPIRED"
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
            statusclass = "text-warning"
    End Select
    
    Dim DomainItemID As String = "domainitem_" & DomainID
    
    @<tr id="@DomainItemID">

        <td><a href="http://@item.DomainName" style="font-size: 1.2em" target="_blank">@item.DomainName</a></td>
        <td>
            <a href="#" onclick="viewUser('@item.RegisterBy.Substring(item.RegisterBy.IndexOf("("), item.RegisterBy.Length - item.RegisterBy.IndexOf("(")).Replace("(", "").Replace(")", "")')">@item.RegisterBy</a>
        </td>
        <td>@Html.DisplayFor(Function(m) item.RegisterDate)</td>
        <td class="hidden-phone">@Html.DisplayFor(Function(m) item.ActivateDate)</td>
        <td class="hidden-phone">@Html.DisplayFor(Function(m) item.ActivateBy)</td>
        <td>@Html.DisplayFor(Function(m) item.DomainExpireDate)</td>
        <td>@Html.DisplayFor(Function(m) item.ProductExpireDate)</td>
        <td class="@statusclass">@status</td>

        <td style="text-align: center">
            @If item.Invoices.Count > 0 Then
        Dim invoice As domain_management.Entities.Invoice = item.Invoices.OrderByDescending(Function(o) o.CreateDate).FirstOrDefault
        If invoice.DocumentVerifiedBy Is Nothing Or (invoice.PaymentVerifiedBy Is Nothing And Not String.IsNullOrEmpty(invoice.BankAccountNo)) Then
                @<span class="badge badge-important">!</span>
        End If
    End If
        </td>

        <td style="text-align: center;">

@*<a title="Verify" class="btn btn-mini btn-primary" onclick="verify('@item.DomainRegID');">VERIFY</a>
                @<a title="Verify" class="btn btn-mini btn-danger" onclick="suspend('@item.DomainRegID');">SUSPEND</a>*@
                @Html.ActionLink("Details", "Details", New With {.i = item.DomainRegID})
        </td>
    </tr>
Next

