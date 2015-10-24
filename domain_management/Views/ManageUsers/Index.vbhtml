@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType PagedList.IPagedList(Of domain_management.Entities.User)
@Section HeadScript
    <script type="text/javascript">

        var table;
        var clickeditem;
        var userid;

        $(document).ready(function () {
            $.ajaxSetup({ cache: false });
        });

        function openEditDialog(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Edit");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Edit", "ManageUsers")",
                data: { id: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn btn-primary">Save</button><button class="btn" data-dismiss="modal">Cancel</button>');
                    $("#myModal .modal-footer button.btn-primary").click(function () {
                        var form = $("#myModal .modal-body form"); form.submit();
                    });
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

        function openDeleteDialog(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Delete");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Delete", "ManageUsers")",
                    data: { id: id },
                    type: "get",
                    success: function (response) {
                        $("#myModal .modal-body").html(response);
                        $("#myModal .modal-footer").html('<button class="btn btn-primary">Yes</button><button data-dismiss="modal" class="btn">No</button>');
                        $("#myModal .modal-footer button.btn-primary").click(function () {
                            var form = $("#myModal .modal-body form"); form.submit();
                        });
                    },
                    error: function (e, xmlHttpStatus, textStatus) {
                        alert(textStatus);
                    }
                });
            }

    </script>

End Section

<fieldset>
    <legend>Manage Users</legend>
    <div id="userlist">
        @Using Html.BeginForm("Index", "ManageUsers", FormMethod.Get, New With {.class = "form-inline"})
            @<div class="input-append pull-right" style="margin-bottom: 10px">
                @Html.TextBox("filter", Nothing, New With {.class = "span2"})
                <button type="submit" class="btn"><i class="icon-search"></i></button>
            </div>
        End Using
        <table class="table" id="myTable">
            <thead>
                <tr>
                    <th>User ID</th>
                    <th>User Name</th>
                    <th class="hidden-phone">Register Date</th>
                    <th class="hidden-phone">Roles</th>
                    <th style="width: 70px"></th>
                </tr>
            </thead>
            <tbody>
                @Html.Partial("_List", Model)
            </tbody>
        </table>


        @If Model.PageCount > 0 Then
            ' pagination
            @<div class="pull-right">
                <div class="pagination" style="margin-top: 5px">
                    <ul>
                        <li class="@IIf(Model.PageNumber = 1, "disabled", "")"><a href="@IIf(Model.PageNumber > 1, Url.Action("Index", "ManageUsers"), "#")">&laquo;</a></li>
                        @For i As Integer = 1 To Model.PageCount
                Dim span As Integer = 2
                Dim shiftRight As Integer = 0
                Dim shiftLeft As Integer = 0
                If Model.PageNumber - span < 0 Then shiftRight = Math.Abs(Model.PageNumber - span)
                If Model.PageNumber + span > Model.PageCount Then shiftLeft = -1 * Math.Abs(Model.PageNumber + span - 1 - Model.PageCount)
                If i > Model.PageNumber - span + shiftLeft And i < Model.PageNumber + span + shiftRight Then
                            @<li class="@IIf(i = Model.PageNumber, "active", "")"><a href="@IIf(Model.PageNumber = i, "#", Url.Action("Index", "ManageUsers", New With {.p = i}))">@i</a></li>
                End If
            Next
                        <li class="@IIf(Model.PageNumber = Model.PageCount, "disabled", "")"><a href="@IIf(Model.PageNumber = Model.PageCount, "#", Url.Action("Index", "ManageUsers", New With {.p = Model.PageCount}))">&raquo;</a></li>
                    </ul>
                </div>
            </div>
        End If


    </div>
</fieldset>

<div id="myModal" class="modal hide fade">
    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h3></h3>
    </div>
    <div class="modal-body"></div>
    <div class="modal-footer"></div>
</div>





