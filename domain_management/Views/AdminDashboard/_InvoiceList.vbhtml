@ModelType IEnumerable(Of domain_management.Entities.Invoice)
<style>
    #tblInvoices {
        width: 100%;
    }

        #tblInvoices th, #tblInvoices td {
            padding: 5px;
        }

        #tblInvoices th {
            background-color: #585a80;
            color: white;
            text-align: left;
        }

        #tblInvoices td {
            border-bottom: 1px solid #d4d7dd;
            background-color: #f2f6f6;
        }

        #tblInvoices tr:hover td {
            background-color: #e7f1f3;
            cursor: pointer;
        }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        $("#tblInvoices tr td").click(function () {
            var target = $(this).parent().prop('id');
            viewInvoice(target);
        });
    });
</script>
<table id="tblInvoices">
    <thead>
        <tr>
            <th>Invoice No.</th>
            <th>Date</th>
            <th>Status</th>
        </tr>
    </thead>
    <tbody>

        @For Each item In Model
            @<tr id="@item.InvoiceID">
                <td>@item.InvoiceID</td>
                <td>@Html.DisplayFor(Function(m) item.CreateDate)</td>
                <td>
                    @If item.VerifyDate Is Nothing Then
                        @<span class="text-error">UNPAID</span>
            Else
                        @<span class="text-success">VERIFIED</span>
            End If
                </td>
            </tr>
        Next
    </tbody>
</table>
