<script type="text/javascript">
    function submitForm() {
        var modal = $("#myModal");
        var button = modal.find("button");
        var form = $("#formCreateBankAccount");
        var table = $("#table_bankaccount");
        button.prop('disabled', true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    table.find("tbody").prepend(response.html);
                    modal.modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
            }
        }).promise().done(function () {
            button.prop('disabled', false).parent().find(".btn-primary").html("Save");
        });
    }
</script>

@ModelType domain_management.Entities.BankAccount

@Using Html.BeginForm("Create", "ManageBankAccount", FormMethod.Post, New With {.id = "formCreateBankAccount", .onsubmit = "submitForm(); return false;"})
    @<div class="form-horizontal">
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
