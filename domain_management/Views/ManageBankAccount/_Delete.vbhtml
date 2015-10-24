<script type="text/javascript">
    function submitForm() {
        var modal = $("#myModal");
        var button = modal.find("button");
        var form = $("#formDeleteBankAccount");
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
                    $("#tr_" + jq(response.id) + " td").fadeOut().promise().done(function () {
                        $("#tr_" + jq(response.id)).remove();
                    });
                    $("#myModal").modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
            }
        }).promise().done(function () {
            button.prop("disabled", false).parent().find(".btn-primary").html("Yes");
        });
    }
</script>

@ModelType domain_management.Entities.BankAccount

@Using Html.BeginForm("Delete", "ManageBankAccount", FormMethod.Post, New With {.id = "formDeleteBankAccount", .onsubmit = "submitForm(); return false;"})
    @<div>
        @Html.HiddenFor(Function(m) Model.BankAccountID)
        Are you sure you want to delete bank account
        <strong>@Model.BankAccountID</strong> ?
    </div>
End Using

