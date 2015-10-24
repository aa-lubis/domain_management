@ModelType domain_management.Entities.Domain
@Code
    ViewData("Title") = "_Details"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section HeadScript
    <link href="@Url.Content("~/Content/colorbox.css")" rel="stylesheet" type="text/css" />
    <style>
        .accordion-group {
            border: 1px solid #33a4ca;
            background-color: white;
        }

        .accordion-heading .accordion-toggle {
            padding: 2px 10px !important;
        }

        .accordion-heading {
            background-image: -moz-linear-gradient(top, #BBBBBB, #999999);
            background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#BBBBBB), to(#999999));
            background-image: -webkit-linear-gradient(top, #BBBBBB, #999999);
            background-image: -o-linear-gradient(top, #BBBBBB, #999999);
            background-image: linear-gradient(to bottom, #BBBBBB, #999999);
        }

            .accordion-heading.active {
                background-image: -moz-linear-gradient(top, #33a4ca, #197ea1);
                background-image: -webkit-gradient(linear, 0 0, 0 100%, from(#33a4ca), to(#197ea1));
                background-image: -webkit-linear-gradient(top, #33a4ca, #197ea1);
                background-image: -o-linear-gradient(top, #33a4ca, #197ea1);
                background-image: linear-gradient(to bottom, #33a4ca, #197ea1);
            }

            .accordion-heading a {
                text-decoration: none;
                color: white;
            }

        .accordion-inner {
            border-top: 1px solid #BBBBBB;
        }

        .form-horizontal .control-group {
            padding-bottom: 4px;
            margin-bottom: 5px;
        }

        .accordion-inner .form-horizontal .control-label {
            width: 146px;
        }

        .accordion-inner .form-horizontal {
            padding-top: 5px;
        }

            .accordion-inner .form-horizontal .controls {
                margin-left: 165px;
            }

        .form-horizontal label {
            padding: 0;
            margin: 0;
        }

        .form-horizontal .control-label {
            padding-top: 0px;
        }
    </style>
    <script src="@Url.Content("~/Scripts/colorbox/jquery.colorbox-min.js")" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("a.colorboximg").colorbox();
        });
        function rejectDocument(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Reject");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("RejectDocVerification", "AdminDashboard")",
                data: { i: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn btn-primary">Send</button><button class="btn" data-dismiss="modal">Cancel</button>');
                    $("#myModal .modal-footer button.btn-primary").click(function () {
                        var form = $("#myModal .modal-body form"); form.submit();
                    });
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

        function rejectPaymentConfirmation(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Reject");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("RejectPaymentConfirmation", "AdminDashboard")",
                data: { i: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn btn-primary">Send</button><button class="btn" data-dismiss="modal">Cancel</button>');
                    $("#myModal .modal-footer button.btn-primary").click(function () {
                        var form = $("#myModal .modal-body form"); form.submit();
                    });
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

        function verify(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Verify");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Verify", "AdminDashboard")",
                data: { id: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn btn-primary">Verify</button><button class="btn" data-dismiss="modal">Cancel</button>');
                    $("#myModal .modal-footer button.btn-primary").click(function () {
                        var form = $("#myModal .modal-body form"); form.submit();
                    });
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }
    </script>
End Section
@Code
    
    Dim status As String = Model.Status
    Dim statusclass As String = ""
    
    Dim DomainExpireDate As New Date
    Dim ProductExpireDate As New Date
    Dim DomainIsTobeExpired As Boolean = False
    Dim ProductIsTobeExpired As Boolean = False
   
    If Not Model.DomainExpireDate Is Nothing Then
        DomainExpireDate = Model.DomainExpireDate
        If status = "ACTIVE" And Now.Date >= DomainExpireDate Then
            status = "EXPIRED"
        End If
    End If
        
    Select Case status.ToString
        Case "ACTIVE"
            status = "Active"
            statusclass = "text-success"
        Case "REGISTERED"
            status = "Pending"
            statusclass = "text-info"
        Case "EXTENDED"
            status = "Extend Pending"
            statusclass = "text-info"
        Case "SUSPENDED"
            status = "Suspended"
            statusclass = "text-error"
        Case "EXPIRED"
            status = "Expired"
            statusclass = "text-error"
    End Select
            
End Code

<div class="row">
    <div class="span12" style="padding-bottom: 40px">

        <fieldset>
            <legend>Domain</legend>

            <div class="form-horizontal">
                <div class="control-group">
                    <div class="control-label">@Html.LabelFor(Function(model) model.DomainName)</div>
                    <div class="controls">
                        <a href="http://@Model.DomainName" target="_blank">@Model.DomainName</a>
                    </div>
                </div>

                <div class="control-group">
                    <div class="control-label">@Html.LabelFor(Function(model) model.RegisterDate)</div>
                    <div class="controls">
                        @Html.DisplayFor(Function(model) model.RegisterDate)
                    </div>
                </div>

                <div class="control-group">
                    <div class="control-label">@Html.LabelFor(Function(model) model.RegisterBy)</div>
                    <div class="controls">
                        @Html.DisplayFor(Function(model) model.RegisterBy)
                    </div>
                </div>

                <div class="control-group">
                    <div class="control-label">@Html.LabelFor(Function(model) model.ActivateDate)</div>
                    <div class="controls">
                        @Html.DisplayFor(Function(model) model.ActivateDate)
                    </div>
                </div>

                <div class="control-group">
                    <div class="control-label">@Html.LabelFor(Function(model) model.ActivateBy)</div>
                    <div class="controls">
                        @Html.DisplayFor(Function(model) model.ActivateBy)
                    </div>
                </div>

                @If Not Model.SuspendDate Is Nothing Then
                    @<div class="control-group">
                        <div class="control-label">@Html.LabelFor(Function(model) model.SuspendDate)</div>
                        <div class="controls">
                            @Html.DisplayFor(Function(model) model.SuspendDate)
                        </div>
                    </div>
                End If

                @If Not String.IsNullOrEmpty(Model.SuspendBy) Then
                    @<div class="control-group">
                        <div class="control-label">@Html.LabelFor(Function(model) model.SuspendBy)</div>
                        <div class="controls">
                            @Html.DisplayFor(Function(model) model.SuspendBy)
                        </div>
                    </div>
                End If

                @If Not Model.DomainExpireDate Is Nothing Then
                    @<div class="control-group">
                        <div class="control-label">@Html.LabelFor(Function(model) model.DomainExpireDate)</div>
                        <div class="controls @IIf(Model.DomainExpireDate.Value <= Now.Date.AddDays(30), "text-error", "")">
                            @Html.DisplayFor(Function(model) model.DomainExpireDate)
                        </div>
                    </div>
                End If

                @If Not Model.ProductExpireDate Is Nothing Then
                    @<div class="control-group">
                        <div class="control-label">@Html.LabelFor(Function(model) model.ProductExpireDate)</div>
                        <div class="controls @IIf(Model.ProductExpireDate.Value <= Now.Date.AddDays(30), "text-error", "")">
                            @Html.DisplayFor(Function(model) model.ProductExpireDate)
                        </div>
                    </div>
                End If

                <div class="control-group">
                    <div class="control-label">@Html.LabelFor(Function(model) model.Status)</div>
                    <div class="controls">
                        <span class="@statusclass">@status</span>
                    </div>
                </div>
            </div>

            <div class="accordion" style="margin-top: 20px">
                <div class="accordion-group">
                    @For Each invoice In Model.Invoices.OrderBy(Function(o) o.CreateDate)
                        @<div class="accordion-heading @IIf(invoice.Equals(Model.Invoices.Last), "active", "")">
                            <a class="accordion-toggle" data-toggle="collapse" data-parent="#accordion2" href="#@invoice.InvoiceID">Invoice : #@invoice.InvoiceID</a>
                        </div>
                        @<div id="@invoice.InvoiceID" class="accordion-body collapse @IIf(invoice.Equals(Model.Invoices.Last), "in", "out")">
                            <div class="accordion-inner">
                                <div class="form-horizontal">
                                    <div class="control-group">
                                        <div class="control-label">
                                            @Html.LabelFor(Function(m) invoice.InvoiceID)
                                        </div>
                                        <div class="controls">
                                            @Html.DisplayFor(Function(m) invoice.InvoiceID)
                                            [ @Html.ActionLink("View Invoice", "Invoice", New With {.i = invoice.InvoiceID}) ]
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="control-label">
                                            @Html.LabelFor(Function(m) invoice.CreateDate)
                                        </div>
                                        <div class="controls">
                                            @Html.DisplayFor(Function(m) invoice.CreateDate)
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="control-label">
                                            Domain Term
                                        </div>
                                        <div class="controls">
                                            @If invoice.DomainRegTermNumber = 0 Then
                                                @("- none -")
                        Else
                                                @(invoice.DomainRegTermNumber & " " & invoice.DomainRegTermPeriod & IIf(invoice.DomainRegTermNumber > 1, "s", ""))
                        End If

                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="control-label">
                                            @Html.LabelFor(Function(m) invoice.Product.ProductName)
                                        </div>
                                        <div class="controls">
                                            <strong>@Html.DisplayFor(Function(m) invoice.Product.ProductName)</strong><br />
                                            @Html.DisplayFor(Function(m) invoice.Product.ProductDesc)
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="control-label">
                                            Product Term
                                        </div>
                                        <div class="controls">
                                            @Html.DisplayFor(Function(m) invoice.ProductTermNumber)
                                            @(invoice.ProductTermPeriod & IIf(invoice.ProductTermNumber > 1, "s", ""))
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <div class="control-label">
                                            Document attached
                                        </div>
                                        <div class="controls">
                                            @IIf(invoice.InvoiceAttachments.Count = 0, "-", "")
                                            @For Each File In invoice.InvoiceAttachments
                                                @<div><a class="@IIf(File.FileName.ToLower().Contains(".jpg"), "colorboximg", "") " href="@File.FileLink" target="_blank">@File.FileName</a></div>
                        Next
                                        </div>
                                    </div>

                                    @If invoice.DocumentIsComplete = True Then
                                        @<div class="control-group">
                                            <div class="control-label">
                                                @Html.LabelFor(Function(m) invoice.DocumentVerifiedBy)
                                            </div>
                                            <div class="controls">
                                                @Html.DisplayFor(Function(m) invoice.DocumentVerifiedBy)
                                                at
                                            @Html.DisplayFor(Function(m) invoice.DocumentVerifiedDate)
                                            </div>
                                        </div>
                        End If

                                    <div class="control-group">
                                        <div class="control-label">
                                            Total Price
                                        </div>
                                        <div class="controls">
                                            @Code
                        Dim totalPrice As Integer = invoice.DomainRegPrice - invoice.DomainRegDiscount + invoice.ProductPrice - invoice.ProductDiscount
                                            End Code
                                            @Format(totalPrice, "#,##0.##")
                                        </div>
                                    </div>

                                    @If Not String.IsNullOrEmpty(invoice.BankAccountNo) Then
                                        @<div class="control-group">
                                            <div class="control-label">
                                                Pay From
                                            </div>
                                            <div class="controls">
                                                @(invoice.Bank & " #" & invoice.BankAccountNo & " " & invoice.BankAccountName)
                                            </div>
                                        </div>

                                        @<div class="control-group">
                                            <div class="control-label">
                                                Pay To
                                            </div>
                                            <div class="controls">
                                                @(invoice.BankAccount.Bank & " #" & invoice.BankAccount.BankAccountNo & " " & invoice.BankAccount.BankAccountName)
                                            </div>
                                        </div>
                                        @<div class="control-group">
                                            <div class="control-label">
                                                Payment Method
                                            </div>
                                            <div class="controls">
                                                @invoice.PaymentMethod
                                            </div>
                                        </div>
                                        @<div class="control-group">
                                            <div class="control-label">
                                                Payment Date
                                            </div>
                                            <div class="controls">
                                                @Html.DisplayFor(Function(m) invoice.PaymentDate)
                                            </div>
                                        </div>
                                        @<div class="control-group">
                                            <div class="control-label">
                                                Payment Due Date
                                            </div>
                                            <div class="controls">
                                                @Html.DisplayFor(Function(m) invoice.PaymentDueDate)
                                            </div>
                                        </div>
                                        @<div class="control-group">
                                            <div class="control-label">
                                                Amount Paid
                                            </div>
                                            <div class="controls">
                                                @Html.DisplayFor(Function(m) invoice.PaymentAmount)
                                            </div>
                                        </div>
                                        @<div class="control-group">
                                            <div class="control-label">
                                                Validation No.
                                            </div>
                                            <div class="controls">
                                                @invoice.ValidationNo
                                            </div>
                                        </div>
                        End If


                                    @If invoice.PaymentIsComplete = True Then
                                        @<div class="control-group">
                                            <div class="control-label">
                                                @Html.LabelFor(Function(m) invoice.PaymentVerifiedBy)
                                            </div>
                                            <div class="controls">
                                                @Html.DisplayFor(Function(m) invoice.PaymentVerifiedBy)
                                                at
                                            @Html.DisplayFor(Function(m) invoice.PaymentVerifiedDate)
                                            </div>
                                        </div>
                        End If


                                    @If invoice.Equals(Model.Invoices.Last) Then
                            If String.IsNullOrEmpty(invoice.DocumentVerifiedBy) Or (String.IsNullOrEmpty(invoice.PaymentVerifiedBy) And Not String.IsNullOrEmpty(invoice.BankAccountNo)) Then
                                    
                                        @<div class="control-group">
                                            <div class="control-label">Action</div>
                                            <div class="controls">
                                                @If String.IsNullOrEmpty(invoice.DocumentVerifiedBy) Then
                                    Using Html.BeginForm("AcceptAndRequestPayment", "AdminDashboard", New With {.invoiceid = invoice.InvoiceID}, FormMethod.Post, Nothing)
                                                    @<div class="form-inline">
                                                        <button class="btn" type="button" onclick="rejectDocument('@invoice.InvoiceID')">Reject Request</button>
                                                        <button class="btn btn-success" type="submit" value="">Accept and Request Payment</button>
                                                    </div>
                                    End Using
                                End If
                                                @If String.IsNullOrEmpty(invoice.PaymentVerifiedBy) And Not String.IsNullOrEmpty(invoice.BankAccountNo) Then
                                                    @<a class="btn" onclick="rejectPaymentConfirmation('@invoice.InvoiceID')">Reject Payment Confirmation</a>
                                                    @<a class="btn btn-success" onclick="verify('@invoice.InvoiceID')">Accept and Activate</a>
                                End If
                                            </div>
                                        </div>
                            End If
                        End If
                                </div>
                            </div>
                        </div>
                    Next
                </div>
            </div>
            <div class="form-actions">
                <a class="btn" href="@Url.Action("Index", "AdminDashboard")">Dashboard</a>
            </div>
        </fieldset>
    </div>
</div>

<div id="myModal" class="modal hide fade">
    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h3></h3>
    </div>
    <div class="modal-body"></div>
    <div class="modal-footer"></div>
</div>

