@ModelType domain_management.Entities.RegisterModel
@Code
    ViewBag.Title = "Register"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@section HeadScript
    <link href="@Url.Content("~/Content/bootstrap-datepicker.min.css")" type="text/css" rel="stylesheet" />
    <script src="@Url.Content("~/Scripts/bootstrap-datepicker.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var currdate = new Date();
            $(".datepicker").datepicker({ format: 'd-M-yyyy' });
            $(".datepicker").datepicker('setDate', new Date(currdate.getFullYear() - 25, currdate.getMonth(), currdate.getDate()));
        });
    </script>
    <style>
        .datepicker {
            text-align: center;
        }
    </style>
End Section

<div class="row visible-desktop" style="height: 30px"></div>
<div class="row">
    <div class="span12">

        <fieldset>
            <legend>Create New Account</legend>
            <div>
                @Using Html.BeginForm("Register", "Account", FormMethod.Post, New With {.class = "form-horizontal", .enctype = "multipart/form-data"})
        
                    @Html.ValidationSummary()

                    If ViewBag.Success = True Then
                    @<div class="alert alert-success">Your account has been created. Click <a href="@Url.Action("Index", "Home")">here</a> to login</div>
            
                    End If
                    @:Please enter your information
                    @<div class="control-group" style="margin-top:40px">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.UserId)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.UserId)
                            @Html.ValidationMessageFor(Function(m) m.User.UserId)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.UserName)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.UserName)
                            @Html.ValidationMessageFor(Function(m) m.User.UserName)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.UserEmailAddress)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.UserEmailAddress)
                            @Html.ValidationMessageFor(Function(m) m.User.UserEmailAddress)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.BirthPlace)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.BirthPlace)
                            @Html.ValidationMessageFor(Function(m) m.User.BirthPlace)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.BirthDay)
                        </div>
                        <div class="controls">
                            <div class="input-prepend datepickerdiv">
                                <span class="add-on"><i class="icon-calendar"></i></span>
                                @Html.TextBoxFor(Function(m) m.User.BirthDay, New With {.class = "input-small datepicker"})
                            </div>
                            @Html.ValidationMessageFor(Function(m) m.User.BirthDay)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.Password)
                        </div>
                        <div class="controls">
                            @Html.PasswordFor(Function(m) m.Password)
                            @Html.ValidationMessageFor(Function(m) m.Password)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.ConfirmPassword)
                        </div>
                        <div class="controls">
                            @Html.PasswordFor(Function(m) m.ConfirmPassword)
                            @Html.ValidationMessageFor(Function(m) m.ConfirmPassword)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.Organization)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.Organization)
                            @Html.ValidationMessageFor(Function(m) m.User.Organization)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.Address)
                        </div>
                        <div class="controls">
                            @Html.TextAreaFor(Function(m) m.User.Address)
                            @Html.ValidationMessageFor(Function(m) m.User.Address)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.City)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.City)
                            @Html.ValidationMessageFor(Function(m) m.User.City)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.Province)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.Province)
                            @Html.ValidationMessageFor(Function(m) m.User.Province)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.PostalCode)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.PostalCode)
                            @Html.ValidationMessageFor(Function(m) m.User.PostalCode)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.PhoneNo)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.PhoneNo)
                            @Html.ValidationMessageFor(Function(m) m.User.PhoneNo)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.IdentityTypeID)
                        </div>
                        <div class="controls">
                            @Html.DropDownListFor(Function(m) Model.User.IdentityTypeID, ViewBag.IdentityType)
                            @Html.ValidationMessageFor(Function(m) m.User.IdentityTypeID)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.IdentityNo)
                        </div>
                        <div class="controls">
                            @Html.TextBoxFor(Function(m) m.User.IdentityNo)
                            @Html.ValidationMessageFor(Function(m) m.User.IdentityNo)
                        </div>
                    </div>
                    @<div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(m) m.User.AttachmentFileName)
                        </div>
                        <div class="controls">
                            <input type="file" name="fileID" id="fileID" />
                        </div>
                    </div>
                    
@*Html.ActionLink("Back to login page", "Index", "Home")*@

                    @<div class="form-actions">
                        <input type="submit" class="btn btn-primary" value="Register" />
                    </div>
        
                End Using
            </div>
        </fieldset>

    </div>
</div>

<div id="myModal" class="modal hide">
    <div class="modal-header"></div>
    <div class="modal-body"></div>
    <div class="modal-footer"></div>
</div>

