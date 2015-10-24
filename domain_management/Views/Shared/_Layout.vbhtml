<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>@System.Configuration.ConfigurationManager.AppSettings("WebTitle")@IIf(String.IsNullOrEmpty(ViewData("Title")), "", " - " & ViewData("Title"))</title>
    <link rel="shortcut icon" href="@Url.Content("~/Content/favicon.png")"/>
    <!--[if IE]>
    <link rel="shortcut icon" href="@Url.Content("~/Content/favicon.ico")" type="image/vnd.microsoft.icon" />
    <![endif]-->

    @*<link rel="stylesheet/less" type="text/css" href="@Url.Content("~/Content/less/bootstrap.less")" />
    <link rel="stylesheet/less" type="text/css" href="@Url.Content("~/Content/less/responsive.less")" />
    <script src="@Url.Content("~/Scripts/less-1.5.1.min.js")" type="text/javascript"></script>*@

    <link href="@Url.Content("~/Content/custom.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/bootstrap.css")" rel="stylesheet" type="text/css" />
    <link href="@Url.Content("~/Content/bootstrap-responsive.css")" rel="stylesheet" type="text/css" />

    <script src="@Url.Content("~/Scripts/jquery-1.11.3.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/bootstrap.min.js")" type="text/javascript"></script>

    @RenderSection("HeadScript", required:=False)
</head>
<body>
    <noscript>
        <div class="modal in">
            <div class="modal-header">
                <h3>No Javascript</h3>
            </div>
            <div class="modal-body">This page require javascript to be enabled</div>
        </div>
        <div class="modal-backdrop in"></div>
    </noscript>



    <div id="wrapper">
        <div style="padding-bottom: 70px">
            @Html.Partial("_TopNav")
            <header class="jumbotron subhead" id="jumbotronheader">
                <div class="container">
                    <div class="pull-right">
                        <img src="@Url.Content("~/img/logo.png")" alt="" width="80" />
                    </div>
                    <h1 style="color: white; margin-bottom: 0; margin-top: 15px">@System.Configuration.ConfigurationManager.AppSettings("WebTitle")</h1>
                    <p class="lead" style="color: white; font-size: 1.2em">One Stop Domain and Hosting Solution</p>

                </div>
            </header>



            @*   <header class="jumbotron">
                <div class="container">
                    <div>
                        <h2 style="">Domain</h2>
                    </div>
                    
                </div>
            </header>*@
            <div class="container" style="border-top: 1px solid white; padding-top: 20px">
                @RenderBody()
            </div>
        </div>
    </div>
    @Html.Partial("_Footer")
</body>
</html>
