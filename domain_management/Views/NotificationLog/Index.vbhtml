@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType PagedList.IPagedList(Of domain_management.Entities.NotificationLog)
<script type="text/javascript">
    $(document).ready(function () {
        $.ajaxSetup({ cache: false });
        $("#cbFilter").change(function () {
            var target = $(this);
            target.prop('disabled', true);
            var filter = target.val();
            $.ajax({
                url: '@Url.Action("GetNotificationLogList", "NotificationLog")',
                type: "get",
                data: { filter: filter },
                success: function (response) {
                    $("#myTable tbody").html(response);
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            }).promise().done(function () {
                target.prop('disabled', false);
            });
        });
    });

    function resend(id) {
        var button = $("#btnResend_" + id);
        button.prepend('<img src="@Url.Content("~/img/loader_small.gif")" alt="" />&nbsp;');
        button.prop('disabled', true);
        $.ajax({
            url: '@Url.Action("ResendNotification", "NotificationLog")',
            type: "post",
            data: { id: id },
            success: function (response) {
                if (response.result != "success") {

                } else {
                    $("tr#" + id).replaceWith(response.html);
                }
            },
            error: function (e, xmlHttpStatus, textStatus) {
                alert(textStatus);
            }
        }).promise().done(function () {
            button.find('img').remove();
            button.prop('disabled', false);
        });
    }
</script>

<style>
    table {
        font-size: .9em;
        font-family: Arial, Tahoma;
    }
</style>

<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Notification Log</legend>
            Filter by
            <select id="cbFilter">
                <option value="">- All -</option>
                <option value="sent">Sent message</option>
                <option value="unsent">Unsent message</option>
            </select>
            <table class="table" id="myTable">
                <thead>
                    <tr>
                        @*<th>@Html.LabelFor(Function(m) Model.FirstOrDefault.LogID)</th>*@
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.SendTo)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.SendCC)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.SendBCC)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.Subject)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.Body)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.LogCreateTime)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.Status)</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @Html.Partial("_NotificationLogList", Model)
                </tbody>
            </table>

            @If Model.PageCount > 0 Then
                ' pagination
                @<div class="pull-right">
                    <div class="pagination" style="margin-top: 5px">
                        <ul>
                            <li class="@IIf(Model.PageNumber = 1, "disabled", "")"><a href="@IIf(Model.PageNumber > 1, Url.Action("Index", "NotificationLog"), "#")">&laquo;</a></li>
                            @For i As Integer = 1 To Model.PageCount
                    Dim span As Integer = 2
                    Dim shiftRight As Integer = 0
                    Dim shiftLeft As Integer = 0
                    If Model.PageNumber - span < 0 Then shiftRight = Math.Abs(Model.PageNumber - span)
                    If Model.PageNumber + span > Model.PageCount Then shiftLeft = -1 * Math.Abs(Model.PageNumber + span - 1 - Model.PageCount)
                    If i > Model.PageNumber - span + shiftLeft And i < Model.PageNumber + span + shiftRight Then
                                @<li class="@IIf(i = Model.PageNumber, "active", "")"><a href="@IIf(Model.PageNumber = i, "#", Url.Action("Index", "NotificationLog", New With {.p = i}))">@i</a></li>
                    End If
                Next
                            <li class="@IIf(Model.PageNumber = Model.PageCount, "disabled", "")"><a href="@IIf(Model.PageNumber = Model.PageCount, "#", Url.Action("Index", "NotificationLog", New With {.p = Model.PageCount}))">&raquo;</a></li>
                        </ul>
                    </div>
                </div>
            End If

        </fieldset>
    </div>
</div>

<div id="myModal" class="modal hide fade">
    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h3></h3>
    </div>
    <div class="modal-body"></div>
    <div class="modal-footer"></div>
</div>

