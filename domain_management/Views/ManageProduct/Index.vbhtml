@Code
    ViewData("Title") = "Index"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@ModelType domain_management.ViewModels.ProductViewModel
@Section HeadScript
    <style>
        table tr td, table tr th {
            border: 1px solid #CCC;
            padding: 3px 5px 3px 5px;
        }

        table tr td {
            background-color: white;
        }

        table tr th {
            background-color: #EEE;
        }
        ul#categorylist li a {
            padding-top: 10px;
            padding-bottom: 10px;
            border-bottom: 1px solid #CCC;
        }
        ul#categorylist li.nav-header {
            background-color: #EEE;
            border-bottom: 1px solid #CCC;
        }
        
    </style>
    <script type="text/javascript">

        function openCreateDialog(categoryid) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("New");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Create", "ManageProduct")",
                data: { cat : categoryid },
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

        function openEditDialog(productid) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Edit");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
               url: "@Url.Action("Edit", "ManageProduct")",
               data: { id: productid },
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

        function openDeleteDialog(productid) {
            $("#myModal .modal-body").html('<center><img src="@Url.Content("~/img/loader_large.gif")" alt="" style="padding: 20px"></img></center>');
            $("#myModal .modal-header h3").html("Delete");
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            $.ajax({
                url: "@Url.Action("Delete", "ManageProduct")",
                 data: { id: productid },
                 type: "get",
                 success: function (response) {
                     $("#myModal .modal-body").html(response);
                     $("#myModal .modal-footer").html('<button class="btn btn-primary">Yes</button><button class="btn" data-dismiss="modal">No</button>');
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
            <legend>Manage Product</legend>
        </fieldset>
    </div>
</div>
<div class="row">
    <div class="span2">
        <ul id="categorylist" class="nav nav-list">
            <li class="nav-header">Category</li>
            @For Each item In Model.ProductCategory
                @<li class="@IIf(ViewBag.SelectedCategory = item.ProductCategoryID, "active", "")"><a id="@item.ProductCategoryID" href="@Url.Action("Index", "ManageProduct", New With {.cat = item.ProductCategoryID})">@item.ProductCategoryName</a></li>
            Next
            @*<li><a><i class="icon-plus"></i>&nbsp;Add New Category</a></li>*@
        </ul>
    </div>
    <div class="span10">
        <div style="padding-bottom: 70px">
            <h3>@Model.Product.FirstOrDefault.ProductCategory.ProductCategoryName</h3>
            <a class="btn" style="margin-bottom: 20px" onclick="openCreateDialog('@Model.Product.FirstOrDefault.ProductCategoryID');"><i class="icon-plus"></i>&nbsp;Add Product</a>
            <div id="productlist">
            @Html.Partial("_List", Model.Product)
                </div>
        </div>
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
