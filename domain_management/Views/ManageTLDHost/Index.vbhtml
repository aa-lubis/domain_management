@Code
    ViewData("Title") = "Index"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType PagedList.IPagedList(Of domain_management.Entities.TLDHost)
@Section HeadScript
    <script type="text/javascript">
        $(document).ready(function () {
            $.ajaxSetup({ cache: false });
        });

        function jq(myid) {
            return myid.replace(/(:|\.|\[|\])/g, "\\$1");
        }

        function openCreateDialog() {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("New");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Create", "ManageTLDHost")",
                data: {},
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

        function openEditDialog(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Edit");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Edit", "ManageTLDHost")",
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
                url: "@Url.Action("Delete", "ManageTLDHost")",
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

<div class="row">
    <div class="span12">
        <fieldset>
            <legend>Top Level Domain Host</legend>
            <a style="margin-bottom: 20px" class="btn" onclick="openCreateDialog()"><i class="icon-plus"></i>&nbsp;Add TLD Host</a>
            @Using Html.BeginForm("Index", "ManageTLDHost", FormMethod.Get, New With {.class = "form-inline"})
                @<div class="input-append pull-right" style="margin-bottom: 10px">
                    @Html.TextBox("filter", Nothing, New With {.class = "span2"})
                    <button type="submit" class="btn"><i class="icon-search"></i></button>
                </div>
            End Using
            <table class="table" id="myTable">
                <thead>
                    <tr>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.TLD)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.Host)</th>
                        <th style="text-align: right">@Html.LabelFor(Function(m) Model.FirstOrDefault.Price)</th>
                        <th style="text-align: right">@Html.LabelFor(Function(m) Model.FirstOrDefault.Discount)</th>
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
                            <li class="@IIf(Model.PageNumber = 1, "disabled", "")"><a href="@IIf(Model.PageNumber > 1, Url.Action("Index", "ManageTLDHost"), "#")">&laquo;</a></li>
                            @For i As Integer = 1 To Model.PageCount
                    Dim span As Integer = 2
                    Dim shiftRight As Integer = 0
                    Dim shiftLeft As Integer = 0
                    If Model.PageNumber - span < 0 Then shiftRight = Math.Abs(Model.PageNumber - span)
                    If Model.PageNumber + span > Model.PageCount Then shiftLeft = -1 * Math.Abs(Model.PageNumber + span - 1 - Model.PageCount)
                    If i > Model.PageNumber - span + shiftLeft And i < Model.PageNumber + span + shiftRight Then
                                @<li class="@IIf(i = Model.PageNumber, "active", "")"><a href="@IIf(Model.PageNumber = i, "#", Url.Action("Index", "ManageTLDHost", New With {.p = i}))">@i</a></li>
                    End If
                Next
                            <li class="@IIf(Model.PageNumber = Model.PageCount, "disabled", "")"><a href="@IIf(Model.PageNumber = Model.PageCount, "#", Url.Action("Index", "ManageTLDHost", New With {.p = Model.PageCount}))">&raquo;</a></li>
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
