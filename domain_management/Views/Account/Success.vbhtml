@Code
    ViewData("Title") = "Success"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@If Not ViewBag.RedirectLink Is Nothing Then
    @<script type="text/javascript">
         setTimeout(redirect, 1000);
         function redirect() {
             window.location.replace("@ViewBag.RedirectLink");
         }
     </script>
End If

<div class="row">
    <div class="span8 offset2">
        <div class="alert alert-success" style="text-align: center">
            @Html.Raw(ViewBag.Message)
        </div>
    </div>
</div>

