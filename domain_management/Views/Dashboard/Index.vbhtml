@Code
    ViewData("Title") = ""
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType PagedList.IPagedList(Of domain_management.Entities.Domain)
<div class="row">
    <div class="span12" style="padding-bottom: 40px">
        <fieldset>
            <legend>Dashboard</legend>
            <div class="btn-group">
                <a href="@Url.Action("Register")" class="btn btn-success">Register new domain</a>
            </div>
            @Using Html.BeginForm("Index", "Dashboard", FormMethod.Get, New With {.class = "form-inline"})
                @<div class="input-append pull-right" style="margin-bottom: 10px">
                    @Html.TextBox("filter", Nothing, New With {.class = "span2"})
                    <button type="submit" class="btn"><i class="icon-search"></i></button>
                </div>
            End Using
            <table id="myTable" class="table">
                <thead>
                    <tr>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.DomainName)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.RegisterDate)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.DomainExpireDate)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.ProductExpireDate)</th>
                        <th>@Html.LabelFor(Function(m) Model.FirstOrDefault.Status)</th>
                        <th></th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    @Html.Partial("_MyDomainList", Model)
                    @If Model.Count = 0 and ViewBag.Filtered is nothing  Then
                        @<tr>
                            <td colspan="7" style="text-align: center">- You have no domain registered -</td>
                        </tr>
                    End If
                </tbody>
            </table>




            @If Model.PageCount > 0 Then
                ' pagination
                @<div class="pull-right">
                    <div class="pagination" style="margin-top: 5px">
                        <ul>
                            <li class="@IIf(Model.PageNumber = 1, "disabled", "")"><a href="@IIf(Model.PageNumber > 1, Url.Action("Index", "MyDomain"), "#")">&laquo;</a></li>
                            @For i As Integer = 1 To Model.PageCount
                    Dim span As Integer = 2
                    Dim shiftRight As Integer = 0
                    Dim shiftLeft As Integer = 0
                    If Model.PageNumber - span < 0 Then shiftRight = Math.Abs(Model.PageNumber - span)
                    If Model.PageNumber + span > Model.PageCount Then shiftLeft = -1 * Math.Abs(Model.PageNumber + span - 1 - Model.PageCount)
                    If i > Model.PageNumber - span + shiftLeft And i < Model.PageNumber + span + shiftRight Then
                                @<li class="@IIf(i = Model.PageNumber, "active", "")"><a href="@IIf(Model.PageNumber = i, "#", Url.Action("Index", "MyDomain", New With {.p = i}))">@i</a></li>
                    End If
                Next
                            <li class="@IIf(Model.PageNumber = Model.PageCount, "disabled", "")"><a href="@IIf(Model.PageNumber = Model.PageCount, "#", Url.Action("Index", "MyDomain", New With {.p = Model.PageCount}))">&raquo;</a></li>
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
    <div class="modal-body">
    </div>
    <div class="modal-footer">
    </div>
</div>




