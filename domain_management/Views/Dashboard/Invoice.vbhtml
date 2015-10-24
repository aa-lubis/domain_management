@Code
    Layout = Nothing
End Code

@ModelType domain_management.ViewModels.InvoiceViewModel
<!DOCTYPE html>

<html>
<head runat="server">
    <title>@System.Configuration.ConfigurationManager.AppSettings("WebTitle").ToString()</title>
    <link href="@Url.Content("~/Content/bootstrap.min.css")" rel="stylesheet" type="text/css" />
    <style>
        body {
            background-color: #DDD;
            font-size: .75em;
        }

        #invoicepage {
            margin-top: 40px;
            background-color: white;
            padding: 20px 20px 60px 20px;
            border: 1px solid #AAA;
            -webkit-border-radius: 3px;
            -moz-border-radius: 3px;
            border-radius: 3px;
            -webkit-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            -moz-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        }

    </style>
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="span10 offset1">
                @Html.Partial("_InvoiceContent", Model)
                <div id="links" style="text-align: center; margin-top: 10px">
                    @Html.ActionLink("Back to Home", "Index", "Home")
                    &nbsp;&nbsp;|&nbsp;&nbsp;
                    @If Not Model.Invoice Is Nothing Then
                        @<a href="javascript:window.print()">Print</a> 
                        @:&nbsp;&nbsp;|&nbsp;&nbsp;
                        @<a style="cursor:pointer" href="@Url.Action("Invoice", "Dashboard", New With {.i = Model.Invoice.InvoiceID, .export = "pdf"})" target="_blank">Download as PDF</a>
                    End If
                </div>
            </div>
        </div>
    </div>
</body>
</html>
