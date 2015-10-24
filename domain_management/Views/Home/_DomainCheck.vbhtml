@ModelType IEnumerable(Of domain_management.ViewModels.CheckDomainViewModel)
<style>
    #tblSearchResult {
        width: 100%;
        font-size: 16px;
    }

        #tblSearchResult th {
            background-color: #ecf0f5;
            font-weight: normal;
            border-bottom: 2px solid #d2d9e2;
            padding: 5px;
            text-align: left;
        }

        #tblSearchResult td {
            border-bottom: 1px solid  #d2d9e2;
            padding: 3px 5px 3px 5px;
        }
</style>

<table id="tblSearchResult">
    <thead>
        <tr>
            <th>Domain</th>
            <th>Result</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        @For Each item In Model
            @<tr class="@IIf(item.Status = "available", "success text-success", "error text-error")">
                <td>@item.DomainName</td>
                <td>@item.Status</td>
                <td style="text-align: right">
                    @If item.Status = "available" Then
                        @Format(item.Price, "#,##0")
            End If
                </td>
            </tr>
        Next
    </tbody>
</table>
