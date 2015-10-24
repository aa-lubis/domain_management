

@If User.Identity.IsAuthenticated Then
    @<p>Hi, <strong>@User.Identity.Name</strong> [ @Html.ActionLink("Log out", "LogOff", "Account") ]</p>
End If
