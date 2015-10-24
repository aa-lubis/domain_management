@Code
    ViewBag.Step = 4
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType IEnumerable(Of domain_management.ViewModels.ProductPurchasedViewModel)
@Section HeadScript
    <style>
        #buttonContainer button {
            margin-left: 10px;
        }
    </style>
End Section

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
        <h3>Order Summary</h3>
        @If Not Html.ViewData.ModelState.IsValid Then
            @Html.ValidationSummary()
        End If
        Here is your order summary
        <table id="tblProduct" class="table">
            <thead>
                <tr>
                    <th>Description</th>
                    <th colspan="2">Price</th>
                </tr>
            </thead>
            <tbody>
                @Code
                    Dim Total As Integer = 0
                    Dim Discount As Integer = 0
                End Code
                @For Each item As domain_management.ViewModels.ProductPurchasedViewModel In Model
                    Total += item.Price
                    Discount += item.Discount
                    @<tr>
                        <td>@Html.Raw(item.ProductDesc)</td>
                        <td>Rp</td>
                        <td style="text-align: right">@Html.DisplayFor(Function(m) item.Price)</td>
                    </tr>
                Next
            </tbody>
            <tfoot>
                <tr>
                    <th>Total</th>
                    <th>Rp</th>
                    <th style="text-align: right">@Format(Total, "#,##0.##")</th>
                </tr>
                @If Discount > 0 Then
                    @<tr>
                        <th>Discount</th>
                        <th>Rp</th>
                        <th style="text-align: right">@Format(Discount, "#,##0.##")</th>
                    </tr>
                    @<tr>
                        <th>Total Purchase</th>
                        <th>Rp</th>
                        <th style="text-align: right">@Format(Total - Discount, "#,##0.##")</th>
                    </tr>
                End If
            </tfoot>
        </table>

        @Using Html.BeginForm("Register", "Dashboard", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data"})
          
            @<div>
                <div style="margin-bottom: 10px">
                    <div style="margin-bottom: 5px">Please upload required document as mentioned below :</div>
                    @IIf(Not ViewBag.Requirement Is Nothing, Html.Raw(ViewBag.Requirement), "- no requirement -")
                </div>
                <table>
                    <tr>
                        <td>Document 1 </td>
                        <td>
                            <input type="file" id="doc1" name="doc1" /></td>
                    </tr>
                    <tr>
                        <td>Document 2 </td>
                        <td>
                            <input type="file" id="doc2" name="doc2" /></td>
                    </tr>
                    <tr>
                        <td>Document 3 </td>
                        <td>
                            <input type="file" id="doc3" name="doc3" /></td>
                    </tr>
                    <tr>
                        <td>Document 4 </td>
                        <td>
                            <input type="file" id="doc4" name="doc4" /></td>
                    </tr>
                    <tr>
                        <td>Document 5 </td>
                        <td>
                            <input type="file" id="doc5" name="doc5" /></td>
                    </tr>
                </table>
            </div>
            
            @<div class="form-actions">
                <span class="pull-right">
                    <a href="@Url.Action("Index", "Dashboard")" class="btn">Cancel</a>
                    <button name="confirm" value="submit" class="btn btn-primary">Submit</button>
                </span>
            </div>
        End Using
    </div>
</div>

