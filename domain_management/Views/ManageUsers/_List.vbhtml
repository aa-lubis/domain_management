@ModelType IEnumerable(Of domain_management.Entities.User)
@For Each item As domain_management.Entities.User In Model
    Dim trid As String = "tr_" & item.UserId
    @<tr id="@trid">
        <td>@Html.DisplayFor(Function(m) item.UserId)</td>
        <td>@Html.DisplayFor(Function(m) item.UserName)</td>
        <td class="hidden-phone">@Html.DisplayFor(Function(m) item.CreateDate)</td>
        <td class="hidden-phone">
            @For i As Integer = 0 To item.UserRoles.Count - 1
                @(item.UserRoles(i).Role.RoleName)
        If i <> item.UserRoles.Count - 1 Then
                @(", ")
        End If
    Next
        </td>
        <td style="text-align: center">
            <a class="btn btn-mini" title="Edit" onclick="openEditDialog('@item.UserId');"><i class="icon icon-pencil" style="cursor: pointer"></i></a>
            <a class="btn btn-mini" title="Delete" onclick="openDeleteDialog('@item.UserId');"><i class="icon icon-trash" style="cursor: pointer"></i></a>
        </td>
    </tr>
Next




