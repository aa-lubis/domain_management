<link href="@Url.Content("~/Content/stylesheets/yellow-text-default.css")" rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/Scripts/yellow-text.js")" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#Requirement").YellowText();
    });
    function submitForm() {
        var modal = $("#myModal");
        var button = modal.find("button");
        var form = $("#formEditBankAccount");
        var table = $("#table_bankaccount");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    table.find("#tr_" + jq(response.id)).replaceWith(response.html);
                    modal.modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                alert("error here");
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
            }
        }).promise().done(function () {
            button.prop('disabled', false).parent().find(".btn-primary").html("Save");
        });
    }
</script>

@ModelType domain_management.Entities.BankAccount
@Using Html.BeginForm("Edit", "ManageBankAccount", FormMethod.Post, New With {.id = "formEditBankAccount", .onsubmit = "submitForm(); return false;"})
    @<div class="form-horizontal">
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.BankAccountID)</div>
            <div class="controls">
                <label style="padding-top: 5px; font-weight: bold">@Html.DisplayFor(Function(m) Model.BankAccountID)</label>@Html.HiddenFor(Function(m) Model.BankAccountID)
            </div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.Bank)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.Bank)</div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.BankAccountName)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.BankAccountName)</div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.BankAccountNo)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.BankAccountNo)</div>
        </div>
    </div>
End Using

