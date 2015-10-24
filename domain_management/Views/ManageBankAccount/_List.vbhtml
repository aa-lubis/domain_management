@ModelType IEnumerable(Of domain_management.Entities.BankAccount)
@For Each item In Model
    Dim trid = "tr_" & item.BankAccountID
    @<tr id="@trid">
        <td>@Html.DisplayFor(Function(m) item.BankAccountID)</td>
        <td>@Html.DisplayFor(Function(m) item.Bank)</td>
        <td>@Html.DisplayFor(Function(m) item.BankAccountName)</td>
        <td>@Html.DisplayFor(Function(m) item.BankAccountNo)</td>
        <td style="text-align: center">
                <a class="btn btn-mini" title="Edit" style="cursor:pointer" onclick="openEditDialog('@item.BankAccountID');"><i class="icon-pencil"></i></a>
                <a class="btn btn-mini" title="Delete" style="cursor: pointer" onclick="openDeleteDialog('@item.BankAccountID');"><i class="icon-trash"></i></a>
        </td>
    </tr>
Next