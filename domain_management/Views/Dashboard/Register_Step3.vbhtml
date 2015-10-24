@Code
    ViewBag.Step = 3
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@section HeadScript
    <style>
        #productDesc {
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            padding: 5px 20px 10px 20px;
            background-color: #dfe6ee;
        }
    </style>
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
        <h3>Product Configuration</h3>
        <div id="productDesc">
            <h4 style="margin-bottom: 5px">@ViewBag.Product</h4>@ViewBag.ProductDesc
        </div>
        <br />
        @Using Html.BeginForm("Register", "Dashboard", Nothing, FormMethod.Post)
            If ViewBag.BillingCycle Is Nothing Then
            @<input type="hidden" id="BillingCycle" name="BillingCycle" value="1" />
            Else
            @:Billing Cycle @Html.DropDownList("BillingCycle")
            End If
            @<div class="form-actions">
                <span class="pull-right">
                    <a href="@Url.Action("Index", "Dashboard")" class="btn">Cancel</a>
                    <button class="btn btn-primary" type="submit">Next</button>
                </span>
            </div>    
        End Using


    </div>
</div>
