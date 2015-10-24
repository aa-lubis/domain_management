@Code
    ViewData("Title") = "Index"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section HeadScript
    <link href="@Url.Content("~/Content/report.css")" type="text/css" rel="stylesheet" />
    <link href="@Url.Content("~/Content/bootstrap-datepicker.min.css")" type="text/css" rel="stylesheet" />
    <script src="@Url.Content("~/Scripts/bootstrap-datepicker.min.js")" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $(".datepicker").datepicker({ format: 'd-M-yyyy', autoclose: true });
            $(".datepicker").datepicker('update');
            $("form button[type=submit]").click(function () {
                $("form").removeAttr("target");
                if ($(this).prop("name") == "submit") {
                    $(this).closest("form").prop("target", "_blank");
                }
            });
        });
        
    </script>
    <style>
        .datepicker {
            text-align: center;
        }

        .reportfilter {
            border: 1px solid #DDD;
            padding: 10px 0 5px;
            background-color: #EEE;
        }
    </style>
End Section

@ModelType IEnumerable(Of domain_management.ViewModels.ReportInvoice)
<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Invoice Report</legend>
            @Using Html.BeginForm
                

                @<div class="reportfilter">
                    <div class="row">
                        <div class="span4">
                            <div class="form-horizontal">
                                <div class="control-group">
                                    <div class="control-label">
                                        From
                                    </div>
                                    <div class="controls">

                                        <div class="input-prepend">
                                            <span class="add-on"><i class="icon icon-calendar"></i></span>
                                            @Html.TextBox("FromDate", ViewData("FromDate"), New With {.class = "datepicker input-small"})
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="control-label">
                                        To
                                    </div>
                                    <div class="controls">
                                        <div class="input-prepend">
                                            <span class="add-on"><i class="icon icon-calendar"></i></span>
                                            @Html.TextBox("ToDate", ViewData("ToDate"), New With {.class = "datepicker input-small"})
                                        </div>
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="control-label">
                                        Payment
                                    </div>
                                    <div class="controls">
                                        @Html.DropDownList("payment", Nothing, New With {.class = "input-small"})
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="control-label">
                                        Type
                                    </div>
                                    <div class="controls">
                                        @Html.DropDownList("tld")
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="controls">
                                        <button type="submit" class="btn">Preview</button>
                                        @If ViewBag.ExportAllowed = True Then
                                            @<span>
                                                <button name="submit" type="submit" class="btn" value="exporttopdf" onclick="exportToPdf()">Export to Pdf</button>
                                            </span>
                End If
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="span5">
                            <div class="form-horizontal">

                                <div class="control-group">
                                    <div class="control-label">
                                        Product
                                    </div>
                                    <div class="controls">
                                        @Html.DropDownList("product")
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="control-label">
                                        Domain Expiry on
                                    </div>
                                    <div class="controls">
                                        @Html.DropDownList("domainexpirymonth", Nothing, New With {.class = "input-small"})
                                        @Html.DropDownList("domainexpiryyear", Nothing, New With {.class = "input-small"})
                                    </div>
                                </div>
                                <div class="control-group">
                                    <div class="control-label">
                                        Product Expiry on
                                    </div>
                                    <div class="controls">
                                        @Html.DropDownList("productexpirymonth", Nothing, New With {.class = "input-small"})
                                        @Html.DropDownList("productexpiryyear", Nothing, New With {.class = "input-small"})
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            End Using

            <div class="reportcontent">
                @Html.Partial("_ReportContent", Model)
            </div>
           
        </fieldset>
    </div>
</div>

