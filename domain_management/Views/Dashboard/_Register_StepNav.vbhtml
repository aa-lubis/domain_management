<style>
    .flow {
        background-color: #DDD;
        padding: 10px;
        text-align: center;
        font-size: 1.1em;
        -webkit-border-radius: 5px;
        -moz-border-radius: 5px;
        border-radius: 5px;
        margin-bottom: 5px;
    }

        .flow.active {
            background-color: #e8e35f;
        }

        .flow.passed {
            background-color: #c7e59c;
        }

    #registernav a {
        color: black;
    }
</style>

<div style="padding-bottom: 30px" id="registernav">
    <div class="flow @IIf(ViewBag.Step > 0, IIf(ViewBag.Step = 1, "active", "passed"), "")">
        <strong>Step 1</strong><br />
        @If ViewBag.Step > 1 Then
            @Html.ActionLink("Choose Product", "Register", "Dashboard", New With {.s = 1}, Nothing)
        Else
            @:Choose Product
        End If
    </div>
    <div class="flow @IIf(ViewBag.Step > 1, IIf(ViewBag.Step = 2, "active", "passed"), "")">
        <strong>Step 2</strong><br />
        @If ViewBag.Step > 2 Then
            @Html.ActionLink("Domain Options", "Register", "Dashboard", New With {.s = 2}, Nothing)
        Else
            @:Domain Options
        End If
    </div>
    <div class="flow @IIf(ViewBag.Step > 2, IIf(ViewBag.Step = 3, "active", "passed"), "")">
        <strong>Step 3</strong><br />
        @If ViewBag.Step > 3 Then
            @Html.ActionLink("Configure", "Register", "Dashboard", New With {.s = 3}, Nothing)
        Else
            @:Configure
        End If
    </div>
    <div class="flow @IIf(ViewBag.Step > 3, IIf(ViewBag.Step = 4, "active", "passed"), "")">
        <strong>Step 4</strong><br />
        @If ViewBag.Step > 3 Then
            @Html.ActionLink("Upload Document & Confirm Order", "Register", "Dashboard", New With {.s = 4}, Nothing)
        Else
            @:Upload Document & Confirm Order
        End If
    </div>
</div>
