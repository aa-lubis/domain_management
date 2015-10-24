@Code
    ViewBag.Step = 1
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType IEnumerable(Of domain_management.Entities.Product)
@Section HeadScript
    <style>
        .product {
            background-color: #dfe6ee;
            padding: 5px 20px 5px 20px;
            margin-bottom: 5px;
            border-left: 5px solid #bed7d4;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            @If Not Request.Cookies("pid") Is Nothing Then
                Dim product = Model.Where(Function(w) w.ProductID = Request.Cookies("pid").Value).FirstOrDefault
                If Not product Is Nothing Then
                    @:$("#ProductCategories").val("@product.ProductCategoryID");
                            End If
            End If

            $("#cat").change(function () {
                $("#frmCategory").submit();

                /*$(this).prop('disable', 'disable');
                var category = $(this).val();
                $("input:radio[name=pid]").prop("checked", false);
                $(".product").fadeOut('fast').promise().done(function () {
                    $("div.product[category=" + category + "]").fadeIn('fast').promise().done(function () {
                        $(this).removeAttr('disable');
                    });
                });*/
            });
        });

        function validateform() {
            if ($("input:radio[name=pid]:checked").length == 0 && $("input[type=hidden][name=pid]").length == 0) {
                $("form#frmProduct .alert").remove();
                $("form#frmProduct").prepend('<div class="alert alert-error">Please choose product first!</div>');
                setTimeout(function () { $("form#frmProduct .alert").fadeOut(); }, 1000);
                return false;
            } else {
                return true;
            }
        }
    </script>
End Section

<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Register</legend>
        </fieldset>
    </div>
</div>
<div class="row">

    <div class="span2">
        @Html.Partial("_Register_StepNav")
    </div>
    <div class="span10">

        @Using Html.BeginForm("Register", "Dashboard", Nothing, FormMethod.Get, New With {.id = "frmCategory"})
            @<div class="inline-form" style="margin-bottom: 10px">Choose Product &nbsp;@Html.DropDownList("cat")</div>
        End Using

        @Using Html.BeginForm("Register", "Dashboard", Nothing, FormMethod.Post, New With {.id = "frmProduct", .onsubmit = "return validateform()"})
   
            For Each item As domain_management.Entities.Product In Model
            @<div class="product" category="@Html.DisplayFor(Function(w) item.ProductCategoryID)">
                @Code
                Dim checked = False
                If Not Request.Cookies("pid") Is Nothing And Model.Count > 1 Then
                    If item.ProductID = Request.Cookies("pid").Value Then checked = True
                End If
                End Code
                <h4>
                    <input type="@IIf(Model.Count = 1, "hidden", "radio")" name="pid" value="@Html.DisplayFor(Function(m) item.ProductID)" @IIf(checked = True, "checked=""checked""", "") />
                    &nbsp;&nbsp;@Html.DisplayFor(Function(w) item.ProductName)
                </h4>

                <p style="margin-left: 30px">@Html.DisplayFor(Function(w) item.ProductDesc)<br />
                    @If item.Price > 0 Then
                        @:(Price: Rp @Html.DisplayFor(Function(w) item.Price) / @item.Counter @IIf(item.Counter = 1, item.UnitPeriod, item.UnitPeriod & "s"))
                End If
                </p>

            </div>
            Next
            
            @<div class="form-actions">
                <span class="pull-right">
                    <a href="@Url.Action("Index", "Dashboard")" class="btn">Cancel</a>
                    <button class="btn btn-primary" type="submit">Next</button>
                </span>
            </div>
        End Using
    </div>
</div>


