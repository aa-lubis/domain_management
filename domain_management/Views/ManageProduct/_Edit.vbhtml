@ModelType domain_management.Entities.Product
<script type="text/javascript">
    function submitForm() {
        var button = $("#myModal button");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        var form = $("#formCreateProduct");
        $.ajax({
            url: form.attr("action"),
            type: form.attr("method"),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                } else {
                    $("#" + response.id).replaceWith(response.html);
                    $("#myModal").modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                form.find(".alert").remove();
                form.prepend('<div class="alert alert-error">' + textStatus + '</div>');
            }
        }).promise().done(function () {
            button.prop("disabled", false).parent().find("btn-primary").html("Save");
        });
    }
</script>

@Using Html.BeginForm("Edit", "ManageProduct", FormMethod.Post, New With {.id = "formCreateProduct", .onsubmit = "submitForm(); return false;"})
    @<div class="form-horizontal">
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.ProductCategory.ProductCategoryName)</div>
            <div class="controls">
                <label style="padding-top: 5px">
                    <strong>@Model.ProductCategory.ProductCategoryName</strong>
                    @Html.HiddenFor(Function(m) Model.ProductCategoryID)
                    @Html.HiddenFor(Function(m) Model.ProductID)
                </label>
            </div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.ProductName)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.ProductName)</div>
        </div>
        <div class="control-group">
            <div class="control-label">@Html.LabelFor(Function(m) Model.ProductDesc)</div>
            <div class="controls">@Html.TextAreaFor(Function(m) Model.ProductDesc, New With {.class = "input-xlarge", .style = "height: 70px"})</div>
        </div>
        <div class="control-group">
            <div class="control-label">Price (Rp)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.Price, New With {.style = "width: 60px"})</div>
        </div>
        <div class="control-group">
            <div class="control-label">Unit Price</div>
            <div class="controls">
                @Html.TextBoxFor(Function(m) Model.Counter, New With {.style = "width: 20px;"})
                @Html.TextBoxFor(Function(m) Model.UnitPeriod, New With {.style = "width: 60px;", .value = "Month"})
            </div>
        </div>
        <div class="control-group">
            <div class="control-label">Discount (Rp)</div>
            <div class="controls">@Html.TextBoxFor(Function(m) Model.Discount, New With {.style = "width: 60px"})</div>
        </div>

    </div>
End Using
