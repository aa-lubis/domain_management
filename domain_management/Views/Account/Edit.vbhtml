@ModelType domain_management.Entities.User
@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@Section HeadScript
    <link href="@Url.Content("~/Content/datepicker.css")" type="text/css" rel="stylesheet" />
    <style>
        #BirthDay {
            text-align: center;
            width: 100px;
        }
    </style>
    <script src="@Url.Content("~/Scripts/bootstrap-datepicker.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#BirthDay").datepicker({ format: 'd-M-yyyy' });
            $("#BirthDay").datepicker('update');
        });
    </script>
End Section

<div class="row visible-desktop" style="height: 30px"></div>
<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Edit Profile</legend>
            @Html.ValidationSummary(True)
            @If ViewBag.Success = True Then
                @<div class="alert alert-success">Your profile has been updated</div>
            End If

        </fieldset>
    </div>
</div>

@Using Html.BeginForm("Edit", "Account", FormMethod.Post, New With {.class = "form-horizontal", .enctype = "multipart/form-data"})
    @<div class="row">
        <div class="span6">
            <div>
                @Html.HiddenFor(Function(model) model.UserId)
                @Html.HiddenFor(Function(model) model.AttachmentFileName)
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.UserName)
                    </div>
                    <div class="controls">
                        @Html.TextBoxFor(Function(model) model.UserName)
                        @Html.ValidationMessageFor(Function(model) model.UserName)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.UserEmailAddress)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.UserEmailAddress)
                        @Html.ValidationMessageFor(Function(model) model.UserEmailAddress)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(m) m.BirthPlace)
                    </div>
                    <div class="controls">
                        @Html.TextBoxFor(Function(m) m.BirthPlace)
                        @Html.ValidationMessageFor(Function(m) m.BirthPlace)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(m) m.BirthDay)
                    </div>
                    <div class="controls">
                        <div class="input-prepend datepickerdiv">
                            <span class="add-on"><i class="icon-calendar"></i></span>
                            @Html.EditorFor(Function(m) m.BirthDay)
                        </div>
                        @Html.ValidationMessageFor(Function(m) m.BirthDay)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.Organization)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.Organization)
                        @Html.ValidationMessageFor(Function(model) model.Organization)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.Address)
                    </div>
                    <div class="controls">
                        @Html.TextAreaFor(Function(model) model.Address, New With {.class = "input-large"})
                        @Html.ValidationMessageFor(Function(model) model.Address)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.City)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.City)
                        @Html.ValidationMessageFor(Function(model) model.City)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.Province)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.Province)
                        @Html.ValidationMessageFor(Function(model) model.Province)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.PostalCode)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.PostalCode)
                        @Html.ValidationMessageFor(Function(model) model.PostalCode)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.PhoneNo)
                    </div>
                    <div class="controls">
                        @Html.EditorFor(Function(model) model.PhoneNo)
                        @Html.ValidationMessageFor(Function(model) model.PhoneNo)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.IdentityTypeID)
                    </div>
                    <div class="controls">
                        @Html.DropDownListFor(Function(m) Model.IdentityTypeID, ViewBag.IdentityType)
                        @Html.ValidationMessageFor(Function(model) model.IdentityTypeID)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.IdentityNo)
                    </div>
                    <div class="controls">
                        @Html.TextBoxFor(Function(model) model.IdentityNo)
                        @Html.ValidationMessageFor(Function(model) model.IdentityNo)
                    </div>
                </div>
                <div class="control-group">
                    <div class="control-label">
                        @Html.LabelFor(Function(model) model.AttachmentFileName)
                    </div>
                    <div class="controls">
                        <input type="file" name="fileID" id="fileID" />
                    </div>
                </div>
            </div>
        </div>
        <div class="span6">

            @Code
     
    Try
        Dim imgsrc = VirtualPathUtility.ToAbsolute("~/Attachment/Users/" & Model.AttachmentFileName)
                @<img width="100%" src="@imgsrc" alt="@Model.UserName" />
    Catch ex As Exception
                
    End Try
      
        
            End Code

        </div>
    </div>
    @<div class="row">
        <div class="span12">
            <div class="form-actions">
                <input class="btn btn-primary" type="submit" value="Save" />
            </div>
        </div>
    </div>
        
      
End Using

