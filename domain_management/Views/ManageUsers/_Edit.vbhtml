@ModelType domain_management.Entities.User
<style>
    .control-label label {
        font-weight: bold;
    }

    .controls {
        padding-top: 5px;
    }
</style>
<script type="text/javascript">

    $(document).ready(function () {
        $("#userrolelist input[type='checkbox']").click(function () {
            //alert($(this).prop('checked'));
        });
    });

    function submitForm() {
        var userroles = [];
        $("#userrolelist input[type='checkbox']").each(function () {
            if ($(this).prop('checked') == true || $(this).prop('checked') == 'checked') {
                userroles.push($(this).val());
            }
        });
        $("#userroles").val(userroles.join(","));
        var button = $("#myModal button");
        button.prop("disabled", true).parent().find(".btn-primary").html('<img src="@Url.Content("~/img/loader_small_white.gif")" alt="" /> Please wait..');
        var form = $("#formEditUser");

        $.ajax({
            url: form.prop('action'),
            type: form.prop('method'),
            data: form.serialize(),
            success: function (response) {
                if (response.result != "success") {
                    form.find(".alert").remove();
                    form.prepend('<div class="alert alert-error">' + response.result + '</div>');
                    $(".modal-body").scrollTop(0);
                } else {
                    $("#tr_" + response.id).replaceWith(response.html);
                    $("#myModal").modal("hide");
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                alert(textStatus);
            }
        }).promise().done(function () {
            button.prop('disabled', false).parent().find(".btn-primary").html("Save");
        });
    }
</script>

@Using Html.BeginForm("Edit", "ManageUsers", FormMethod.Post, New With {.id = "formEditUser", .onsubmit = "submitForm(); return false;"})
    @<div class="form-horizontal">
        @Html.HiddenFor(Function(m) Model.UserId)
        @Html.Hidden("userroles", "")
        @Html.AntiForgeryToken("edituser")
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
                @Html.DisplayFor(Function(m) Model.IdentityType.IdentityTypeDesc)
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
        <div class="control-group">
            <div class="control-label">
                <strong>User Role</strong>
            </div>
            <div class="controls">
                <div id="userrolelist">
                    @If Not ViewBag.UserRoles Is Nothing And Not Model.UserRoles Is Nothing Then
        For Each item As domain_management.Entities.Role In ViewBag.UserRoles
                        @<label class="checkbox">
                            <input type="checkbox" @IIf(Model.UserRoles.Any(Function(a) a.RoleId = item.RoleId), Html.Raw("checked=""checked"""), "") value="@item.RoleId" />
                            @item.RoleName
                        </label>
        Next
    End If
                </div>
            </div>
        </div>

        <div style="background-color: #999; text-align: center; padding: 5px">
            @Code
   
    Try
        Dim imgsrc = VirtualPathUtility.ToAbsolute("~/Attachment/Users/" & Model.AttachmentFileName)
                @<img width="100%" src="@imgsrc" alt="" />
    Catch ex As Exception
                
    End Try
      
        
            End Code
        </div>

    </div>
End Using
