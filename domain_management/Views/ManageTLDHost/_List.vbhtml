@ModelType IEnumerable(Of domain_management.Entities.TLDHost)
@For Each item In Model
    Dim trid = "tr_" & item.TLD
    @<tr id="@trid">
        <td>@Html.DisplayFor(Function(m) item.TLD)</td>
        <td>@Html.DisplayFor(Function(m) item.Host)</td>
        <td style="text-align: right">@Html.DisplayFor(Function(m) item.Price)</td>
        <td style="text-align: right">@Html.DisplayFor(Function(m) item.Discount)</td>
        <td style="text-align: center">
                <a class="btn btn-mini" title="Edit" style="cursor:pointer" onclick="openEditDialog('@item.TLD');"><i class="icon-pencil"></i></a>
                <a class="btn btn-mini" title="Delete" style="cursor: pointer" onclick="openDeleteDialog('@item.TLD');"><i class="icon-trash"></i></a>
        </td>
    </tr>
Next
