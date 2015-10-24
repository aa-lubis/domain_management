<link href="@Url.Content("~/Content/stylesheets/yellow-text-default.css")" rel="stylesheet" type="text/css" />
<script src="@Url.Content("~/Scripts/yellow-text.js")" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#Requirement").YellowText();
    });
    function submitForm() {
        var button = $("#myModal button");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        var form = $("#formEditTLDHost");
        $("#Requirement").val(encodeURIComponent($("#Requirement").val()));
        $.ajax({
            url: form.prop("action"),
            type: form.prop("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    $("#tr_" + jq(response.id)).replaceWith(response.html);
                    $("#myModal").modal("hide");
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

@ModelType domain_management.Entities.TLDHost
@Using Html.BeginForm("Edit", "ManageTLDHost", FormMethod.Post, New With {.id = "formEditTLDHost", .onsubmit = "submitForm(); return false;"})
    @<div class="form-horizontal">
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.TLD)</div>
            <div class="controls">
                <label style="padding-top: 5px; font-weight: bold">@Html.DisplayFor(Function(m) Model.TLD)</label>@Html.HiddenFor(Function(m) Model.TLD)
            </div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.Host)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.Host)</div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.Price)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.Price)</div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.Discount)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.Discount)</div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.Requirement)</div>
            <div class="controls">@Html.TextAreaFor(Function(m) Model.Requirement)</div>
        </div>
    </div>
End Using

