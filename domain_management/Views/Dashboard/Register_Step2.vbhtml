@Code
    ViewBag.Step = 2
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
@ModelType domain_management.Entities.Domain
@Section HeadScript
    <style>
        #myForm {
            -webkit-border-radius: 5px;
            -moz-border-radius: 5px;
            border-radius: 5px;
            padding: 10px 20px 10px 20px;
            background-color: #dfe6ee;
        }

        #extendForm {
            background-color: #dfe6ee;
        }

        a {
            color: black;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#domainnameentry").val($("#DomainName").val().replace($("select#TLD").val(), ''));
        });

        function submitForm() {
            if ($("select#TLD").val() != null) {
                $("#DomainName").val($("#domainnameentry").val() + $("select#TLD").val());
            } else {
                $("#DomainName").val($("#domainnameentry").val());
            }
            return true;
        }
    </script>
End Section

@Code
    Dim tempdomain As domain_management.Entities.Domain = Session("tempdomain")
    Dim renew As Boolean = False
    If tempdomain.DomainExpireDate <= Now.Date Then renew = True
End Code

<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Register</legend>
        </fieldset>
    </div>
</div>


<div class="row">
    <div class="span2">
        @Html.Partial("_Register_StepNav")
    </div>
    <div class="span10">

        <h3>@ViewBag.SelectedProduct</h3>
        <br />
        @Html.ValidationSummary()

        @If String.IsNullOrEmpty(tempdomain.DomainRegID) Or tempdomain.DomainExpireDate <= Now.Date Then
            
            Using Html.BeginForm("Register", "Dashboard", FormMethod.Post, New With {.onsubmit = "return submitForm();"})
           
            @<div id="myForm">
                <div style="margin-bottom: 5px">
                    Enter domain name that you wish to register
                </div>
                <div class="input-prepend input-append">
                    <span class="add-on">www.
                    </span>
                    <input id="domainnameentry" type="text" @IIf(renew, "readonly=""readonly""", "") />
                    @Html.Hidden("DomainName", ViewBag.DomainName)
                    @If renew = True Then
                        @Html.Hidden("TLD", ViewBag.DomainName.Substring(ViewBag.DomainName.IndexOf("."), ViewBag.DomainName.Length - ViewBag.DomainName.IndexOf(".")))
                Else
                        @Html.DropDownList("TLD", Nothing, New With {.style = "width: 80px"})
                End If
                </div>
            </div>

            @<div class="form-actions">
                <span class="pull-right">
                    <button name="confirm" value="cancel" class="btn">Cancel</button>
                    <button class="btn btn-primary" type="submit">Next</button>
                </span>
            </div>

            End Using
            
        Else
            Using Html.BeginForm()
            @Html.HiddenFor(Function(m) m.DomainRegID)
            @<div id="myForm">
                <p>Choose your domain option :</p>
                <label class="radio">
                    <input type="radio" name="extendDomain" value="no" @IIf(Model.DomainExpireDate <= Now.Date.AddDays(30), "", "checked=""checked""") />
                    I will use my existing domain <strong>@Model.DomainName</strong> (no change)</label>
                <label class="radio">
                    <input type="radio" name="extendDomain" value="yes" @IIf(Model.DomainExpireDate <= Now.Date.AddDays(30), "checked=""checked""", "") />
                    I want to extend my domain <strong>@Model.DomainName</strong> service for 1 year</label>
            </div>
            @<div class="form-actions">
                <span class="pull-right">
                    <a href="@Url.Action("Index", "Dashboard")" class="btn">Cancel</a>
                    <button class="btn btn-primary" type="submit">Next</button>
                </span>
            </div>
            End Using
        End If
    </div>
</div>
