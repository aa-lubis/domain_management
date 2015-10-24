@ModelType domain_management.Entities.Invoice
<script type="text/javascript">

</script>

You are going to reject this request back to user. Notification will be sent to user by email<br />
Are you sure want to continue?
@Using Html.BeginForm()
    @<div class="form-horizontal" style="margin-top: 20px">
        @Html.HiddenFor(Function(m) Model.InvoiceID)
        @If Not ViewData.ModelState.IsValid Then
            @<div class="alert alert-error">@Html.ValidationSummary()</div>
    End If
        <div class="control-group">
            <div class="control-label">
                Additional Message<br />
                (optional)
            </div>
            <div class="controls">@Html.TextAreaFor(Function(m) Model.DocumentVerificationRemark)</div>
        </div>
    </div>
End Using