@ModelType domain_management.Entities.LogOnModel
@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section HeadScript
    <style>
        #tldlist tr td {
            min-width: 90px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajaxSetup({ cache: false });

            $("#txtDomain").keyup(function (e) {
                if (e.keyCode == 13) {
                    $("#btnSearch").trigger("click");
                } else {
                    $("#searchresult").html("");
                }
            });

            $("#btnSearch").click(function () {
                var domain = $("#txtDomain").val();
                var selectedtlds = [];
                $("#selectedtlds input:checked").each(function () {
                    selectedtlds.push($(this).val());
                });
                if (selectedtlds.length > 0 && domain.trim() != "") {
                    $("#searchresult").html('<div style="text-align: center"><img src="@Url.Content("~/img/loader_large.gif")" alt="" /></div>');
                    $.ajax({
                        url: "@Url.Action("CheckDomain", "Home")",
                        type: "post",
                        data: { domain: domain, selectedtld: selectedtlds.join(",") },
                        success: function (response) {
                            $("#searchresult").html(response);
                        },
                        error: function (e, xmlHttpStatus, textStatus) {
                            $("#searchresult").html(textStatus);
                        }
                    });
                }
            });
        });

    </script>
End Section


<script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
<script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>



<div class="row" style="padding-bottom: 50px">

    <div class="span8 visible-desktop">
        <div class="span5 offset1" style="margin-top: 70px;">
            <div style="font-size: 20px">Check domain availability</div>
            <div class="form-inline" style="margin-top: 10px">
                <label style="font-size: 20px">www.</label>
                <div class="input-append">
                    <input id="txtDomain" type="text" class="input-block-level" style="font-size: 20px" maxlength="200" />
                    <button class="btn" type="button" id="btnSearch">Go!</button>
                </div>
            </div>

            <div id="selectedtlds" style="padding-top: 0; margin-left: 55px">

                @If Not ViewBag.TLDs Is Nothing Then
                                     
                    
                    Dim TLDs As List(Of domain_management.Entities.TLDHost) = ViewBag.TLDs
                    Dim tdIndex As Integer = 1
                                   
                    @<table id="tldlist">

                        @For i As Integer = 0 To TLDs.Count - 1 Step 3
                            @<tr>
                                @For j As Integer = 0 To 2
                            If i + j < TLDs.Count Then
                                    @<td>
                                        <label class="checkbox inline">
                                            <input type="checkbox" value="@TLDs(i + j).TLD" checked="checked" /><span style="font-size: 16px">@TLDs(i + j).TLD</span>
                                        </label>
                                    </td>
                            End If
                        Next
                            </tr>
                    Next
                    </table>
                                       
                End If
            </div>
            <div id="searchresult" style="margin-top: 30px"></div>
        </div>

    </div>
    <div class="span4">
        <div id="loginform">
            <h3>Login</h3>
            <p>
                Please enter your user name and password.
                <br />@Html.ActionLink("Register", "Register", "Account") if you don't have an account.
            </p>

            @Html.ValidationSummary(True)

            @Using Html.BeginForm("Index", "Home", FormMethod.Post, New With {.class = "form-horizontal"})
                
                If Not Request.QueryString("ReturnUrl") Is Nothing Then
                    @<input type="hidden" name="ReturnUrl" value="@Request.QueryString("ReturnUrl")" />
                End If
    
                @<div>
                    @Html.LabelFor(Function(m) m.UserID)
                    @Html.TextBoxFor(Function(m) m.UserID, New With {.class = "input-block-level"})
                    @Html.ValidationMessageFor(Function(m) m.UserID)
                </div>
         
                @<div style="margin-top: 5px">
                    @Html.LabelFor(Function(m) m.Password)
                    @Html.PasswordFor(Function(m) m.Password, New With {.class = "input-block-level"})
                    @Html.ValidationMessageFor(Function(m) m.Password)
                </div>

                @<label class="checkbox" style="margin-top: 5px">
                    @Html.CheckBoxFor(Function(m) m.RememberMe)
                    @Html.LabelFor(Function(m) m.RememberMe)
                </label>
                @<br />
                @<input type="submit" class="btn btn-primary btn-block " value="Login" />
            
            End Using

            <a href="@Url.Action("ResetPassword", "Account")">Forgot your password?</a>
        </div>
    </div>
</div>

