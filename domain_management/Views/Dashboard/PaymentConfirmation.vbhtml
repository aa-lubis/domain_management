@ModelType domain_management.Entities.Invoice
@Code
    ViewData("Title") = "PaymentConfirmation"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section headScript
    <link href="@Url.Content("~/Content/bootstrap-datepicker.min.css")" type="text/css" rel="stylesheet" />
    <script src="@Url.Content("~/Scripts/bootstrap-datepicker.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.min.js")" type="text/javascript"></script>
    <script src="@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            var currdate = new Date();
            $(".datepicker").datepicker({ format: 'd-M-yyyy', autoclose: true });
            $(".datepicker").datepicker('setDate', new Date());
        });
    </script>
    <style>
        .datepicker {
            text-align: center;
        }
    </style>
End Section

<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Payment Confirmation</legend>
            @Using (Html.BeginForm())
                @Html.HiddenFor(Function(model) model.InvoiceID)
                @<div class="form-horizontal">
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.PaymentMethod)
                        </div>
                        <div class="controls">
                            @Html.DropDownListFor(Function(model) model.PaymentMethod, ViewBag.PaymentMethod)
                            @Html.ValidationMessageFor(Function(model) model.PaymentMethod)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.Bank)
                        </div>
                        <div class="controls">
                            @Html.EditorFor(Function(model) model.Bank)
                            @Html.ValidationMessageFor(Function(model) model.Bank)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.BankAccountNo)
                        </div>
                        <div class="controls">
                            @Html.EditorFor(Function(model) model.BankAccountNo)
                            @Html.ValidationMessageFor(Function(model) model.BankAccountNo)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.BankAccountName)
                        </div>
                        <div class="controls">
                            @Html.EditorFor(Function(model) model.BankAccountName)
                            @Html.ValidationMessageFor(Function(model) model.BankAccountName)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.BankAccountID)
                        </div>
                        <div class="controls">
                            @Html.DropDownListFor(Function(model) model.BankAccountID, CType(ViewBag.BankAccounts, SelectList), New With {.class = "input-xlarge"})
                            @Html.ValidationMessageFor(Function(model) model.BankAccountID)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.PaymentDate)
                        </div>
                        <div class="controls">
                            <div class="input-prepend">
                                <span class="add-on"><i class="icon-calendar"></i></span>
                                @Html.TextBoxFor(Function(model) model.PaymentDate, New With {.class = "input-small datepicker"})
                            </div>
                            @Html.ValidationMessageFor(Function(model) model.PaymentDate)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.PaymentAmount)
                        </div>
                        <div class="controls">
                            @Html.EditorFor(Function(model) model.PaymentAmount)
                            @Html.ValidationMessageFor(Function(model) model.PaymentAmount)
                        </div>
                    </div>
                    <div class="control-group">
                        <div class="control-label">
                            @Html.LabelFor(Function(model) model.ValidationNo)
                        </div>
                        <div class="controls">
                            @Html.EditorFor(Function(model) model.ValidationNo)
                            @Html.ValidationMessageFor(Function(model) model.ValidationNo)
                        </div>
                    </div>
                    <div class="form-actions">
                        <a class="btn" href="@Url.Action("Details", "Dashboard", New With {.i = Model.DomainRegID})">Back</a>
                        <button type="submit" class="btn btn-primary">Submit</button>
                    </div>
                </div>
            End Using
        </fieldset>
    </div>
</div>
