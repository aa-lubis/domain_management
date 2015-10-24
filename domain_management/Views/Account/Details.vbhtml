@ModelType domain_management.Entities.User
@Code
    ViewData("Title") = "Details"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section HeadScript
    <style>
        .control-label label {
            font-weight: bold;
        }
        .controls {
            min-height: 10px;
            padding: 5px;
            border-bottom: 1px solid #CCC;
        }
    </style>
End Section

<div class="row visible-desktop" style="height: 30px"></div>
<div class="row">
    <div class="span12">
        <fieldset>
            <legend>User Profile</legend>
            @Html.ValidationSummary(True)
            @If ViewBag.Success = True Then
                @<div class="alert alert-success">Your profile has been updated</div>
            End If

        </fieldset>
    </div>
</div>


<div class="row">
    <div class="span6">
        <div class="form-horizontal">
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.UserName)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.UserName)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.UserEmailAddress)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.UserEmailAddress)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(m) m.BirthPlace)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(m) m.BirthPlace)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(m) m.BirthDay)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(m) m.BirthDay)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.Organization)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.Organization)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.Address)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.Address)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.City)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.City)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.Province)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.Province)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.PostalCode)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.PostalCode)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.PhoneNo)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.PhoneNo)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.IdentityTypeID)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.IdentityType.IdentityTypeDesc)
                </div>
            </div>
            <div class="control-group">
                <div class="control-label">
                    @Html.LabelFor(Function(model) model.IdentityNo)
                </div>
                <div class="controls">
                    @Html.DisplayFor(Function(model) model.IdentityNo)
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


