<script type="text/javascript">
    function submitForm() {
        var button = $("#myModal button");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        var form = $("#formDeleteProduct");
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    $("#" + response.id).fadeOut().promise().done(function () {
                        $("#" + response.id).remove();
                    });
                    $("#myModal").modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
            }
        }).promise().done(function () {
            button.prop('disabled', false).parent().find(".btn-primary").html("Yes");
        });
    }
</script>

@ModelType domain_management.Entities.Product

@Using Html.BeginForm("Delete", "ManageProduct", FormMethod.Post, New With {.id = "formDeleteProduct", .onsubmit = "submitForm(); return false;"})
    @<div>
        @Html.HiddenFor(Function(m) Model.ProductID)
        Are you sure you want to delete product
        <strong>@Model.ProductName</strong> ?
    </div>
End Using
