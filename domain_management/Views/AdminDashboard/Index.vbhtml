@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Section HeadScript
    <style>
        #myTable td a:hover {
            text-decoration: none;
        }

        #myTable {
            font-size: .9em;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            $.ajaxSetup({ cache: false });
            if ($("#myTable tr").length > 11) { $("#myTable").dataTable(); }
            $(".invoiceid").popover({ trigger: "hover", placement: "bottom", html: "true" });
        });

        

        function suspend(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Suspend");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Suspend", "Domain")",
                data: { id: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn btn-primary">Suspend</button><button class="btn" data-dismiss="modal">Cancel</button>');
                    $("#myModal .modal-footer button.btn-primary").click(function () {
                        var form = $("#myModal .modal-body form"); form.submit();
                    });
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

        function viewInvoice(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Invoice");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("ViewInvoice", "Domain")",
                type: "get",
                data: { id: id },
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn" data-dismiss="modal">Close</button>');
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

        function showInvoices(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Invoices");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("ShowInvoices", "Domain")",
                data: { id: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn" data-dismiss="modal">Close</button>');
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

        function viewUser(id) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("User Detail");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("ViewUser", "AdminDashboard")",
                data: { id: id },
                type: "get",
                success: function (response) {
                    $("#myModal .modal-body").html(response);
                    $("#myModal .modal-footer").html('<button class="btn" data-dismiss="modal">Close</button>');
                },
                error: function (e, xmlHttpStatus, textStatus) {
                    alert(textStatus);
                }
            });
        }

    </script>
End Section

@Imports PagedList.Mvc
@ModelType PagedList.IPagedList(Of domain_management.Entities.Domain)
<div class="container">
    <div class="row">
        <div class="span12">
            <fieldset>
                <legend>Domain List</legend>
                <div style="margin-top: 10px">
                    @Using Html.BeginForm("Index", "AdminDashboard", FormMethod.Get, New With {.class = "form-inline"})
                        @<div class="input-append pull-right" style="margin-bottom: 10px">
                            @Html.TextBox("filter", Nothing, New With {.class = "span2"})
                            <button type="submit" class="btn"><i class="icon-search"></i></button>
                        </div>
                    End Using
                    <table id="myTable" class="table">
                        <thead>
                            <tr>

                                <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.DomainName)</th>
                                <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.RegisterBy)</th>
                                <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.RegisterDate)</th>
                                <th class="hidden-phone">@Html.LabelFor(Function(m) Model.FirstOrDefault.ActivateDate)</th>
                                <th class="hidden-phone">@Html.LabelFor(Function(m) Model.FirstOrDefault.ActivateBy)</th>
                                <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.DomainExpireDate)</th>
                                <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.ProductExpireDate)</th>
                                <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.Status)</th>
                                <th></th>
                                <th style="width: 70px"></th>
                            </tr>
                        </thead>
                        <tbody>
                            @Html.Partial("_DomainList", Model)
                        </tbody>

                    </table>

                    @If Model.PageCount > 0 Then
                        ' pagination
                        @<div class="pull-right">
                            <div class="pagination" style="margin-top: 5px">
                                <ul>
                                    <li class="@IIf(Model.PageNumber = 1, "disabled", "")"><a href="@IIf(Model.PageNumber > 1, Url.Action("Index", "AdminDashboard"), "#")">&laquo;</a></li>
                                    @For i As Integer = 1 To Model.PageCount
                            Dim span As Integer = 2
                            Dim shiftRight As Integer = 0
                            Dim shiftLeft As Integer = 0
                            If Model.PageNumber - span < 0 Then shiftRight = Math.Abs(Model.PageNumber - span)
                            If Model.PageNumber + span > Model.PageCount Then shiftLeft = -1 * Math.Abs(Model.PageNumber + span - 1 - Model.PageCount)
                            If i > Model.PageNumber - span + shiftLeft And i < Model.PageNumber + span + shiftRight Then
                                        @<li class="@IIf(i = Model.PageNumber, "active", "")"><a href="@IIf(Model.PageNumber = i, "#", Url.Action("Index", "AdminDashboard", New With {.p = i}))">@i</a></li>
                            End If
                        Next
                                    <li class="@IIf(Model.PageNumber = Model.PageCount, "disabled", "")"><a href="@IIf(Model.PageNumber = Model.PageCount, "#", Url.Action("Index", "AdminDashboard", New With {.p = Model.PageCount}))">&raquo;</a></li>
                                </ul>
                            </div>
                        </div>
                    End If

                </div>
            </fieldset>
        </div>
    </div>
</div>

<div id="myModal" class="modal hide fade">
    <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
        <h3></h3>
    </div>
    <div class="modal-body">
    </div>
    <div class="modal-footer">
    </div>
</div>
