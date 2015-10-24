<link href="@Url.Content("~/Content/datepicker.css")" type="text/css" rel="stylesheet" />
<style>
    #DomainExpireDate, #ProductExpireDate {
        width: 100px;
        text-align: center;
    }
</style>
<script src="@Url.Content("~/Scripts/bootstrap-datepicker.js")" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#DomainExpireDate").datepicker({ format: 'd-M-yyyy' });
        $("#DomainExpireDate").datepicker('update');
        $("#ProductExpireDate").datepicker({ format: 'd-M-yyyy' });
        $("#ProductExpireDate").datepicker('update');
    });

    function submitForm() {
        var button = $("#myModal button");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
    }
</script>

@ModelType domain_management.Entities.Domain
@Using Html.BeginForm("Verify", "AdminDashboard", FormMethod.Post, New With {.id = "formVerifyDomain", .onsubmit="submitForm()" })
    @:Are you sure you want to verify this domain ?<br /><br />
    @Html.HiddenFor(Function(m) Model.DomainRegID)
    @<div class="form-horizontal">
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.DomainName)</div>
            <div class="controls">
                <a href="http://@Model.DomainName" target="_blank">@Model.DomainName</a>
            </div>
        </div>
        <div class="control-group">
            <div class="control-label">Domain Expiry Date</div>
            <div class="controls">
                <span class="input-prepend"><span class="add-on"><i class="icon-calendar"></i></span>
                    @Html.EditorFor(Function(m) Model.DomainExpireDate, New With {.class = "datepicker"})
                </span>
            </div>
        </div>
        <div class="control-group">
            <div class="control-label">Product Expiry Date</div>
            <div class="controls">
                <span class="input-prepend"><span class="add-on"><i class="icon-calendar"></i></span>
                    @Html.EditorFor(Function(m) Model.ProductExpireDate, New With { .class = "datepicker" })
                </span>
            </div>
        </div>

    </div>
End Using

