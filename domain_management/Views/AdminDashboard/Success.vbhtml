@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="row">
    <div class="span12">

        <h3>Success !</h3>
        @ViewBag.Message
        <br />
        <br />
        <div class="form-inline">
            <a class="btn" href="@Url.Action("Index", "AdminDashboard")">Dashboard</a>
        </div>

    </div>
</div>
