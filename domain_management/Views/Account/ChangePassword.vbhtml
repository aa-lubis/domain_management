@ModelType domain_management.Entities.ChangePasswordModel
@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>

<div class="row visible-desktop" style="height:30px"></div>
<div class="row">
    <div class="span8 offset2">

        @Using Html.BeginForm("ChangePassword", "Account", FormMethod.Post, New With {.class = "form-horizontal"})
    
            @<fieldset>
                <legend>Change Password</legend>
                @Html.ValidationSummary(True)
                @If ViewBag.Success = True Then
                    @<div class="alert alert-success">Your password has been changed</div>
            End If

                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.OldPassword)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.OldPassword)
                        @Html.ValidationMessageFor(Function(model) model.OldPassword)
                    </div>
                </div>

                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.NewPassword)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.NewPassword)
                        @Html.ValidationMessageFor(Function(model) model.NewPassword)
                    </div>
                </div>

                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.ConfirmPassword)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.ConfirmPassword)
                        @Html.ValidationMessageFor(Function(model) model.ConfirmPassword)
                    </div>
                </div>
                <div class="form-actions">
                    <input type="submit" class="btn btn-primary" value="Change" />
                </div>
            </fieldset>
   
        End Using
    </div>
</div>
