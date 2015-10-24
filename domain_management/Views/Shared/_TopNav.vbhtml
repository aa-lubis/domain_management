<div class="navbar navbar-inverse navbar-static-top">
    <div class="navbar-inner">
        <div class="container">
            <button type="button" class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>

            @*<a class="brand hidden-desktop" href="#">@System.Configuration.ConfigurationManager.AppSettings("WebTitle")</a>*@
            <div class="nav-collapse collapse">

                <ul class="nav">
                    <li><a href="@Url.Action("Index", "Home")"><i class="icon-white icon-home"></i>&nbsp; Home</a></li>
                    @If User.IsInRole("REPORTVIEWER") Then
                        @<li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown">Report&nbsp;<b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="@Url.Action("Index", "RptInvoice")">Invoice</a></li>
                            </ul>
                        </li>
                    End If
                </ul>


                @If User.Identity.IsAuthenticated Then
                    @<ul class="nav pull-right">
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="icon-white icon-user"></i>&nbsp; @Request.Cookies("_userdisplayname").Value <b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="@Url.Action("Edit", "Account")"><i class="icon-cog"></i>&nbsp; Edit Profile</a></li>
                                <li><a href="@Url.Action("ChangePassword", "Account")"><i class="icon-wrench"></i>&nbsp; Change Password</a></li>
                                <li class="divider"></li>
                                <li><a href="@Url.Action("LogOff", "Account")"><i class="icon-off"></i>&nbsp; Log out</a></li>
                            </ul>
                        </li>
                    </ul>
                    If User.IsInRole("SUPERADMIN") Then
                    @<ul class="nav pull-right">
                        <li class="dropdown">
                            <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="icon-white icon-cog"></i>&nbsp;Master Data<b class="caret"></b></a>
                            <ul class="dropdown-menu">
                                <li><a href="@Url.Action("Index", "ManageBankAccount")">Bank Accounts</a></li>
                                <li><a href="@Url.Action("Index", "ManageTLDHost")">Top Level Domain</a></li>
                                <li><a href="@Url.Action("Index", "ManageProduct")">Products</a></li>
                                <li><a href="@Url.Action("Index", "ManageUsers")">Users</a></li>
                                <li class="divider"></li>
                                <li><a href="@Url.Action("Index", "NotificationLog")">Notification Log</a></li>
                            </ul>
                        </li>
                    </ul>
                    End If
                End If
            </div>
            <!--/.nav-collapse -->
        </div>
    </div>
</div>

