@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code



<div class="row">
    <div class="span12">

        <h3>Success !</h3>
        @Html.Raw(ViewBag.Message)
        <br />
        <br />
        <div class="form-inline">
            <a class="btn" href="@Url.Action("Index", "Dashboard")">Dashboard</a>
        </div>

    </div>
</div>
