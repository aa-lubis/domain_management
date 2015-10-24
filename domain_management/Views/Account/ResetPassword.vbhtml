@ModelType domain_management.Entities.ResetPasswordModel
@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<div class="row visible-desktop" style="height: 30px"></div>
<div class="row">
    <div class="span8 offset2">
        @Using Html.BeginForm
            @<fieldset>
                <legend>Reset Password</legend>
                @Html.ValidationSummary()
                @If ViewBag.Success = True Then
                    @<div class="alert alert-success">New password has been sent to your email</div>
            End If
                <div class="editor-label">
                    Enter your user ID or email
                </div>
                <div class="editor-field">
                    @Html.TextBoxFor(Function(model) model.UserID)
                </div>

                @*Html.ActionLink("Back to login page", "Index", "Home")*@
                <div class="form-actions">
                    <input type="submit" class="btn btn-primary" value="Reset" />
                </div>
            </fieldset>
        End Using

    </div>
</div>
</div>