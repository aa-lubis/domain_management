<script type="text/javascript">
    function submitForm() {
        var button = $("#myModal button");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        var form = $("#formDeleteUser");
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    $("#tr_" + response.id + " td").fadeOut().promise().done(function () {
                        $("#tr_" + response.id).remove();
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

@ModelType domain_management.Entities.User

@Using Html.BeginForm("Delete", "ManageUsers", FormMethod.Post, New With {.id = "formDeleteUser", .onsubmit = "submitForm(); return false;"})
    @<div>
        @Html.HiddenFor(Function(m) Model.UserId)
        @Html.AntiForgeryToken("deleteuser")
        Are you sure you want to delete user <strong>@Model.UserName (@Model.UserId)</strong> ?
    </div>
End Using


