<script type="text/javascript">

    function submitForm() {
        var button = $("#myModal button");
        button.prop('disabled', true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        var form = $("#formSuspendDomain");
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    $("#domainitem_" + response.id).replaceWith(response.html);
                    $("#myModal").modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
            }
        }).promise().done(function () {
            button.prop('disabled', false).parent().find(".btn-primary").html("Suspend");
        });
    }
</script>

@ModelType domain_management.Entities.Domain
@Using Html.BeginForm("Suspend", "Domain", FormMethod.Post, New With {.id = "formSuspendDomain", .onsubmit = "submitForm();return false;"})
    @:Are you sure you want to suspend this domain ?<br /><br />
    @Html.HiddenFor(Function(m) Model.DomainRegID)
    @<table>
        <tr style="height: 30px">
            <td style="width: 100px">@Html.LabelFor(Function(m) Model.DomainName)</td>
            <td>
                <label style="font-weight: bold">: @Html.DisplayFor(Function(m) Model.DomainName)</label></td>
        </tr>
        <tr style="height: 30px">
            <td>@Html.LabelFor(Function(m) Model.InvoiceID)</td>
            <td>
                <label style="font-weight: bold">: @Html.DisplayFor(Function(m) Model.InvoiceID)</label></td>
        </tr>
    </table>
End Using

