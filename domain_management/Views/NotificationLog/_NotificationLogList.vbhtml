@Modeltype IEnumerable(Of domain_management.Entities.NotificationLog)
@For Each item In Model
    @<tr id="@item.LogID">
        @*<td>@Html.DisplayFor(Function(m) item.LogID)</td>*@
        <td>@Html.DisplayFor(Function(m) item.SendTo)</td>
        <td>@Html.DisplayFor(Function(m) item.SendCC)</td>
        <td>@Html.DisplayFor(Function(m) item.SendBCC)</td>
        <td>@Html.DisplayFor(Function(m) item.Subject)</td>
        <td>
            <div style="overflow: hidden; max-height: 100px; max-width: 300px">@Html.DisplayFor(Function(m) item.Body)</div>
        </td>
        <td>@Html.DisplayFor(Function(m) item.LogCreateTime)</td>
        <td>
            <div class="@iif(item.Status.toLower = "sent", "text-success", "text-error")" style="overflow: hidden; max-height: 100px; max-width: 200px;">
                @If Not item.ResendTime Is Nothing And item.Status.toLower = "sent" Then
                    @:Sent at @Html.DisplayFor(Function(m) item.ResendTime)
                    Else
                    @Html.DisplayFor(Function(m) item.Status)            
    End If


            </div>
        </td>
        <td>
            @Code
    Dim buttonid As String = "btnResend_" & item.LogID
            End Code
            @If item.Status.ToLower <> "sent" Then
                @<button id="@buttonid" class="btn btn-small" onclick="resend('@item.LogID')">RESEND</button>
    End If
        </td>

    </tr>
Next
