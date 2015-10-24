@ModelType IEnumerable(Of domain_management.Entities.Product)
@For Each item In Model
    @<div id="@item.ProductID" style="background-color: #eef3ef; padding: 10px 20px 30px 20px; margin-bottom: 10px; border-top: 1px solid #c8e5cd">

        <div class="pull-right">
            <a class="btn btn-mini" onclick="openEditDialog('@item.ProductID');" title="Edit"><i class="icon-pencil"></i></a>
            <a class="btn btn-mini" onclick="openDeleteDialog('@item.ProductID')" title="Delete"><i class="icon-trash"></i></a>
        </div>

        <h4>@item.ProductName</h4>
        <p>@item.ProductDesc</p>
        <table style="width: 100%">
            <thead>
                <tr>
                    <th style="width: 30%">Price</th>
                    <th style="width: 40%">Unit Price</th>
                    <th style="width: 30%">Discount</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Rp @Html.DisplayFor(Function(m) item.Price)</td>
                    <td>@Html.DisplayFor(Function(m) item.Counter)&nbsp;@Html.DisplayFor(Function(m) item.UnitPeriod)</td>
                    <td>Rp @Html.DisplayFor(Function(m) item.Discount)</td>
                </tr>
            </tbody>
        </table>
    </div>
Next
