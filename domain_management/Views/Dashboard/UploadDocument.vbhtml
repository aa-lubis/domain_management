@ModelType domain_management.Entities.Invoice
@Code
    ViewData("Title") = "UploadDocument"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<fieldset>
    <legend>Upload Document</legend>
    @Using Html.BeginForm("UploadDocument", "Dashboard", Nothing, FormMethod.Post, New With {.enctype = "multipart/form-data"})
        @Html.HiddenFor(Function(m) Model.InvoiceID)
        @<div>
            @Html.ValidationSummary()
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
            <div class="form-actions">
                <a href="@Url.Action("Details", "Dashboard", New With {.i = Model.DomainRegID})" class="btn">Back</a>
                <button type="submit" class="btn btn-primary">Upload</button>
            </div>
        </div>
    End Using
</fieldset>


