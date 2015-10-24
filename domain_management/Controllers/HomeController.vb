Imports System.Globalization
Imports System.Security
Imports System.Security.Cryptography

Imports domain_management.Entities
Imports domain_management.Interfaces
Imports domain_management.Repositories
Imports domain_management.ViewModels

Namespace domain_management

    Public Class HomeController
        Inherits System.Web.Mvc.Controller

        Dim unitOfWork As IUnitOfWork
        Dim tldHostRepository As ITLDHostRepository
        Dim userRepository As IUserRepository
        Dim userRoleRepository As IUserRoleRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal userRepository As IUserRepository,
                ByVal tldHostRepository As ITLDHostRepository,
                ByVal userRoleRepository As IUserRoleRepository)
            Me.unitOfWork = unitOfWork
            Me.tldHostRepository = tldHostRepository
            Me.userRepository = userRepository
            Me.userRoleRepository = userRoleRepository
        End Sub

        '
        ' GET: /Home

        Function Index() As ActionResult

            If User.Identity.IsAuthenticated Then
                If User.IsInRole("ADMIN") Or User.IsInRole("SUPERADMIN") Then
                    Return RedirectToAction("Index", "AdminDashboard")
                Else
                    Return RedirectToAction("Index", "Dashboard")
                End If
            End If
            ViewBag.TLDs = tldHostRepository.GetAll().ToList()
            Return View()
        End Function

        <HttpPost> _
        Function Index(ByVal model As LogOnModel, ReturnUrl As String) As ActionResult
            If ModelState.IsValid Then
                If Membership.ValidateUser(model.UserID, model.Password) Then
                    SetupFormsAuthTicket(model.UserID, model.RememberMe)
                    Dim loggedinuser As User = userRepository.GetById(model.UserID)
                    Response.Cookies.Add(New HttpCookie("_userdisplayname", loggedinuser.UserName))

                    If Not String.IsNullOrEmpty(ReturnUrl) Then
                        Return Redirect(ReturnUrl)
                    Else

                        Dim test As Boolean = User.IsInRole("ADMIN")
                        If userRoleRepository.GetAll().Any(Function(a) a.UserId = model.UserID And (a.RoleId = "AD" Or a.RoleId = "SA")) Then
                            Return RedirectToAction("Index", "AdminDashboard")
                        Else
                            Return RedirectToAction("Index", "Dashboard")
                        End If
                    End If
                Else
                    ModelState.AddModelError("", "The user name or password provided is incorrect")
                End If
            End If
            ViewBag.TLDs = tldHostRepository.GetAll().ToList()
            Return View()
        End Function

        <HttpPost()> _
        Function CheckDomain(ByVal domain As String, ByVal selectedtld As String) As ActionResult
            Dim tlds As String() = selectedtld.Split(",")
            Dim model As New List(Of CheckDomainViewModel)
            For Each tld In tlds
                Dim tldHost As TLDHost = tldHostRepository.GetAll().Where(Function(w) w.TLD = tld).FirstOrDefault
                Dim fulldomain As String = domain & tld
                Dim status As String = ""
                Select Case ModuleDomainMgr.DomainAvailability(tldHost.Host, fulldomain)
                    Case 0
                        status = "taken"
                    Case 1
                        status = "available"
                    Case 2
                        model.Clear()
                        Exit For
                    Case 3
                        model.Clear()
                        Exit For
                End Select
                model.Add(New CheckDomainViewModel With {.DomainName = fulldomain, .Status = status, .Price = tldHost.Price, .UnitPrice = "Year"})
            Next
            Return PartialView("_DomainCheck", model)
        End Function

        Private Function SetupFormsAuthTicket(userId As String, persistanceFlag As Boolean)
            Dim user As User
            user = userRepository.GetById(userId)
            Dim userData = userId.ToString(CultureInfo.InvariantCulture)
            Dim authTicket = New FormsAuthenticationTicket(1, _
                                                           userId, _
                                                           DateTime.Now(), _
                                                           DateTime.Now().AddMinutes(30), _
                                                           persistanceFlag,
                                                           userData)
            Dim encTicket = FormsAuthentication.Encrypt(authTicket)
            Response.Cookies.Add(New HttpCookie(FormsAuthentication.FormsCookieName, encTicket))
            Return user
        End Function

        Public Function ValidateUser(userid As String, password As String) As Boolean
            Dim Md5Pass As String = GetMd5Hash(password)
            Return userRepository.GetAll().Any(Function(w) w.UserId = userid And w.Password = Md5Pass)
        End Function


    End Class
End Namespace
