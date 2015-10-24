<script type="text/javascript">

    function submitForm() {
        var button = $("#myModal button");
        button.prop('disabled', true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        @*var form = $("#formExtendDomain");
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                    button.prop('disabled', false).parent().find(".btn-primary").html("Yes");
                } else {
                    window.location.replace('@Url.Action("Register", "Dashboard")');
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
                button.prop('disabled', false).parent().find(".btn-primary").html("Yes");
            }
        });*@
    }
</script>

@ModelType domain_management.Entities.Domain
@Using Html.BeginForm("Extend", "Dashboard", FormMethod.Post, New With {.id = "formExtendDomain", .onsubmit = "submitForm();"})
    @:Are you sure you want to extend your domain <strong>@Html.DisplayFor(Function(m) Model.DomainName)</strong> ?
    @Html.HiddenFor(Function(m) Model.DomainRegID)
End Using


