@ModelType domain_management.ViewModels.InvoiceViewModel
<link href="@Url.Content("~/Content/invoice.css")" rel="stylesheet" type="text/css" />
<style>
    #myModal {
        width: 800px;
        margin-left: -400px !important;
    }
</style>

<div id="invoicepage">

    @If Model.Invoice Is Nothing Then
        @<div style="margin: auto; text-align: center; margin-top: 30px">
            <h3>Invoice not found</h3>
        </div>
    Else
        @<div>
            <table class="tablenarrow" style="width: 100%; margin-bottom: 30px;">
                <tr>
                    <td style="width: 50%; font-weight: bold; font-size: 1.5em; color: #4b778e">
                        <h2 id="header">
                            @*System.Configuration.ConfigurationManager.AppSettings("WebTitle").ToString()*@
                            <img src="@Url.Content("~/img/logo-plain.png")" alt="" />
                        </h2>
                    </td>
                    <td style="padding-left: 20px">
                        @If Model.Invoice.PaymentIsComplete = False Then
                            @<span style="font-weight: bold; font-size: 1.2em; color: red">UNPAID</span>                                        
        End If
                        <br />
                        @If Not Model.Invoice.BankAccountID Is Nothing Then
                            @<table>
                                <tr>
                                    <td>Bank</td>
                                    <td>&nbsp;:&nbsp;</td>
                                    <td>@Model.Invoice.Bank</td>
                                </tr>
                                <tr>
                                    <td>Account Name</td>
                                    <td>&nbsp;:&nbsp;</td>
                                    <td>@Model.Invoice.BankAccountName</td>
                                </tr>
                                <tr>
                                    <td>Account Number</td>
                                    <td>&nbsp;:&nbsp;</td>
                                    <td>@Model.Invoice.BankAccountNo</td>
                                </tr>
                            </table>
        Else
            Dim DefaultBankAccount As domain_management.Entities.BankAccount = ViewBag.DefaultBankAccount
                            @<table>
                                <tr>
                                    <td>Bank</td>
                                    <td>&nbsp;:&nbsp;</td>
                                    <td>@DefaultBankAccount.Bank</td>
                                </tr>
                                <tr>
                                    <td>Account Name</td>
                                    <td>&nbsp;:&nbsp;</td>
                                    <td>@DefaultBankAccount.BankAccountName</td>
                                </tr>
                                <tr>
                                    <td>Account Number</td>
                                    <td>&nbsp;:&nbsp;</td>
                                    <td>@DefaultBankAccount.BankAccountNo</td>
                                </tr>
                            </table>
        End If
                    </td>
                </tr>
            </table>
            <table id="address" style="width: 100%; border-collapse: collapse">
                <tr>
                    <td width="50%">Invoiced To :<br />
                        @Model.User.Organization<br />
                        @Model.User.UserName<br />
                        @Model.User.Address, @Model.User.PostalCode<br />
                        @Model.User.City - @Model.User.Province
                    </td>
                    <td>Pay To :<br />
                        PT. MyIndo Cyber Media<br />
                        Gedung Anakida Lt. 5 Suite 501<br />
                        Jl. Prof. DR. Soepomo, SH.<br />
                        No. 27,Tebet Jakarta Selatan 12810
                    </td>
                </tr>
            </table>

            <br />
            <div style="font-weight: bold; font-size: 1.3em">Invoice #@Model.Invoice.InvoiceID</div>
            Invoice Date : @Format(Model.Invoice.CreateDate, "dd MMM yyyy")

            @If Model.Invoice.PaymentIsComplete = False Then
                @:<br />Invoice Due Date : @Format(Model.Invoice.PaymentDueDate, "dd MMM yyyy")
            End If

            <br />
            <br />
            <table id="tblSummary" style="border-collapse: collapse; width: 100%">
                <thead>
                    <tr>
                        <th style="text-align: center">Description</th>
                        <th style="text-align: center">Amount</th>
                    </tr>
                </thead>
                <tbody>
                    @If Model.Invoice.ProductPrice > 0 Then
                        @<tr>
                            <td>@Model.Invoice.Product.ProductName - @Model.Invoice.ProductTermNumber @Model.Invoice.ProductTermPeriod@IIf(Model.Invoice.ProductTermNumber > 1, "s", "")</td>
                            <td style="text-align: right;">@Html.DisplayFor(Function(m) Model.Invoice.ProductPrice)</td>
                        </tr>
        End If
                    @If Model.Invoice.DomainRegPrice > 0 Then
                        @<tr>
                            <td>@Model.Invoice.Domain.DomainName - @Model.Invoice.DomainRegTermNumber @Model.Invoice.DomainRegTermPeriod@IIf(Model.Invoice.DomainRegTermNumber > 1, "s", "")</td>
                            <td style="text-align: right;">@Html.DisplayFor(Function(m) Model.Invoice.DomainRegPrice)</td>
                        </tr>
        End If


                </tbody>
                <tfoot>
                    <tr>
                        <th style="text-align: right">Total Rp</th>
                        <th style="text-align: right">@Format(Model.Invoice.ProductPrice + Model.Invoice.DomainRegPrice, "#,##0.##")</th>
                    </tr>
                    @If Not Model.Invoice.ProductDiscount Is Nothing Or Not Model.Invoice.DomainRegDiscount Is Nothing Then
            If Model.Invoice.ProductDiscount > 0 Or Model.Invoice.DomainRegDiscount > 0 Then
                        @<tr>
                            <th style="text-align: right">Discount</th>
                            <th style="text-align: right">@Format(Model.Invoice.ProductDiscount + Model.Invoice.DomainRegDiscount, "#,##0.##")</th>
                        </tr>
                        @<tr>
                            <th style="text-align: right">Total Purchase Rp</th>
                            <th style="text-align: right">@Format(Model.Invoice.ProductPrice + Model.Invoice.DomainRegPrice - Model.Invoice.ProductDiscount - Model.Invoice.DomainRegDiscount, "#,##0.##")</th>
                        </tr>
            End If
        End If

                </tfoot>
            </table>
            @If Model.Invoice.PaymentIsComplete = True Then
                @<div style="text-align: center; margin-top: 10px">- Thank you for your purchase -</div>
        End If
        </div>
    End If
</div>
