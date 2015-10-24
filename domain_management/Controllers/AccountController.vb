Imports System.Globalization
Imports System.Security
Imports System.Security.Cryptography
Imports System.Net.Mail
Imports System.IO

Imports domain_management.Entities
Imports domain_management.ViewModels
Imports domain_management.Repositories
Imports domain_management.Infrastructure
Imports domain_management.Interfaces


Namespace domain_management
    Public Class AccountController
        Inherits System.Web.Mvc.Controller

        '
        ' GET: /Account

        Private unitOfWork As IUnitOfWork
        Private userRepository As IUserRepository
        Private identityTypeRepository As IIdentityTypeRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal userRepository As IUserRepository,
                ByVal identityTypeRepository As IIdentityTypeRepository)
            Me.unitOfWork = unitOfWork
            Me.userRepository = userRepository
            Me.identityTypeRepository = identityTypeRepository
        End Sub

        Function LogOff() As ActionResult
            Response.Cookies.Clear()
            FormsAuthentication.SignOut()
            Return RedirectToAction("Index", "Home")
        End Function

        Function Register() As ActionResult
            ViewBag.IdentityType = New SelectList(identityTypeRepository.GetAll, "IdentityTypeID", "IdentityTypeDesc")
            Return View()
        End Function

        <HttpPost> _
        Function Register(model As RegisterModel, fileID As HttpPostedFileBase) As ActionResult
            If ModelState.IsValid Then

                Dim processed As Boolean = True
                If userRepository.GetAll().Any(Function(w) w.UserId = model.User.UserId.ToLower()) Then
                    ModelState.AddModelError("", "User ID requested has already taken")
                    processed = False
                End If
                If userRepository.GetAll().Any(Function(w) w.UserEmailAddress = model.User.UserEmailAddress) Then
                    ModelState.AddModelError("", "Email requested has already registered")
                    processed = False
                End If

                Dim maxContent As Integer = 2        ' 2 MB
                Dim allowedFileTypes As String() = {".jpg"}
                Dim fileName As String = String.Empty
                Dim fileExt As String = String.Empty

                If fileID Is Nothing Then
                    ModelState.AddModelError("", "Attachment required")
                    processed = False
                Else
                    fileExt = fileID.FileName.Substring(fileID.FileName.LastIndexOf(".")).ToLower
                    If fileID.ContentLength = 0 Then
                        ModelState.AddModelError("", "Attachment required")
                        processed = False
                    End If
                    If Not allowedFileTypes.Contains(fileExt) And processed = True Then
                        ModelState.AddModelError("", "Accepted file type : " & String.Join(",", allowedFileTypes))
                        processed = False
                    End If
                    If fileID.ContentLength > maxContent * 1024 * 1024 And processed = True Then
                        ModelState.AddModelError("", "Your file is too large, maximum allowed size is : " & maxContent & " MB")
                        processed = False
                    End If
                End If

                If processed = True Then
                    Try

                        ' check directory for upload storage
                        If Not System.IO.Directory.Exists(Server.MapPath("~/Attachment")) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Attachment"))
                        End If
                        If Not System.IO.Directory.Exists(Server.MapPath("~/Attachment/Users")) Then
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Attachment/Users"))
                        End If

                        ' save file attached
                        If Not fileID Is Nothing Then
                            fileName = ModuleDomainMgr.GetMd5Hash("id" & model.User.UserId) & fileExt
                            Dim savePath As String = Path.Combine(Server.MapPath("~/Attachment/Users/"), fileName)
                            fileID.SaveAs(savePath)
                        End If

                        Dim newuser As User = model.User
                        model.User.AttachmentFileName = fileName
                        newuser.Password = GetMd5Hash(model.Password)
                        newuser.CreateDate = Now
                        userRepository.Insert(newuser)
                        unitOfWork.Commit()

                        If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                            Dim mail As New MailMessage()
                            mail.To.Add(model.User.UserEmailAddress)
                            mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Account Registration"
                            mail.Body = "Dear " & model.User.UserName & ",<br /><br />"
                            mail.Body &= "Your account has been registered<br />"
                            mail.Body &= "User ID : " & model.User.UserId & "<br />"
                            mail.Body &= "Password : " & model.Password
                            mail.IsBodyHtml = True

                            ModuleDomainMgr.SendNotification(mail)

                        End If

                        ViewBag.RedirectLink = Url.Action("Index", "Home")
                        ViewBag.Message = "Your account has been created successfully!<br /> Click <a href=""" & Url.Action("Index", "Home") & """ title=""Login"">here</a> if the page doesn't redirect you"
                        Return View("Success")

                    Catch ex As Exception
                        ModelState.AddModelError("", ex.ToString())
                    End Try
                End If
            End If
            ViewBag.IdentityType = New SelectList(identityTypeRepository.GetAll, "IdentityTypeID", "IdentityTypeDesc", model.User.IdentityTypeID)
            Return View(model)
        End Function

        <Authorize> _
        Public Function Edit() As ActionResult
            Dim editedUser As User = userRepository.GetById(Me.User.Identity.Name)
            ViewBag.IdentityType = New SelectList(identityTypeRepository.GetAll, "IdentityTypeID", "IdentityTypeDesc")
            Return View(editedUser)
        End Function

        <Authorize> _
        <HttpPost> _
        Public Function Edit(model As User, fileID As HttpPostedFileBase) As ActionResult
            If ModelState.IsValid Then

                Dim processed As Boolean = True

                If userRepository.GetAll().Any(Function(a) a.UserEmailAddress = model.UserEmailAddress And a.UserId <> model.UserId) Then
                    ModelState.AddModelError("", "Email address has already registered to someone else")
                    processed = False
                End If

                Dim maxContent As Integer = 2        ' 2 MB
                Dim allowedFileTypes As String() = {".jpg"}
                Dim fileName As String = String.Empty
                Dim fileExt As String = String.Empty

                If processed = True Then
                    If Not fileID Is Nothing Then
                        fileExt = fileID.FileName.Substring(fileID.FileName.LastIndexOf(".")).ToLower
                        If fileID.ContentLength = 0 Then
                            ModelState.AddModelError("", "Attachment required")
                            processed = False
                        End If
                        If Not allowedFileTypes.Contains(fileExt) And processed = True Then
                            ModelState.AddModelError("", "Accepted file type : " & String.Join(",", allowedFileTypes))
                            processed = False
                        End If
                        If fileID.ContentLength > maxContent * 1024 * 1024 And processed = True Then
                            ModelState.AddModelError("", "Your file is too large, maximum allowed size is : " & maxContent & " MB")
                            processed = False
                        End If
                    End If
                End If

                If processed = True Then
                    Try

                        ' save file attached
                        If Not fileID Is Nothing Then
                            fileName = ModuleDomainMgr.GetMd5Hash("id" & model.UserId) & fileExt
                            Dim savePath As String = Path.Combine(Server.MapPath("~/Attachment/Users/"), fileName)
                            fileID.SaveAs(savePath)
                        End If

                        Dim editeduser As User = userRepository.GetById(model.UserId)
                        editeduser.UserName = model.UserName
                        editeduser.BirthPlace = model.BirthPlace
                        editeduser.BirthDay = model.BirthDay
                        editeduser.UserEmailAddress = model.UserEmailAddress
                        editeduser.Organization = model.Organization
                        editeduser.Address = model.Address
                        editeduser.City = model.City
                        editeduser.Province = model.Province
                        editeduser.PostalCode = model.PostalCode
                        editeduser.PhoneNo = model.PhoneNo
                        editeduser.IdentityTypeID = model.IdentityTypeID
                        editeduser.IdentityNo = model.IdentityNo
                        If Not fileID Is Nothing Then
                            editeduser.AttachmentFileName = fileName
                            model.AttachmentFileName = fileName
                        End If
                        editeduser.LastUpdateDate = Now
                        userRepository.Update(editeduser)
                        unitOfWork.Commit()
                        Request.Cookies("_userdisplayname").Value = editeduser.UserName
                        Response.Cookies("_userdisplayname").Value = editeduser.UserName
                        ViewBag.Success = True
                    Catch ex As Exception
                        ModelState.AddModelError("", ex.ToString)
                    End Try
                End If
            End If
            ViewBag.IdentityType = New SelectList(identityTypeRepository.GetAll, "IdentityTypeID", "IdentityTypeDesc", model.IdentityTypeID)
            Return View(model)
        End Function

        <Authorize()> _
        Function Details(ByVal userid As String)
            Return View(userRepository.GetAll(includeProperties:="IdentityType").Where(Function(w) w.UserId = userid).FirstOrDefault)
        End Function

        <Authorize()> _
        Public Function ChangePassword() As ActionResult
            Return View()
        End Function

        <Authorize()> _
        <HttpPost> _
        Public Function ChangePassword(model As ChangePasswordModel) As ActionResult
            If ModelState.IsValid Then
                Dim md5Pass = GetMd5Hash(model.OldPassword)
                Dim editedUser As User = userRepository.GetAll().SingleOrDefault(Function(w) w.UserId = Me.User.Identity.Name And w.Password = md5Pass)
                If editedUser Is Nothing Then
                    ModelState.AddModelError("", "Password supplied is incorrect")
                Else
                    Try
                        editedUser.Password = GetMd5Hash(model.NewPassword)
                        editedUser.LastUpdateDate = Now
                        unitOfWork.Commit()
                        ViewBag.Success = True
                        Return View()
                    Catch ex As Exception
                        ModelState.AddModelError("", ex.ToString)
                    End Try
                End If
            End If
            Return View(model)
        End Function

        Public Function ResetPassword() As ActionResult
            Return View()
        End Function

        <HttpPost> _
        Public Function ResetPassword(model As ResetPasswordModel) As ActionResult
            Dim user As User = userRepository.GetAll().SingleOrDefault(Function(m) m.UserId = model.UserID Or m.UserEmailAddress = model.UserID)
            If user Is Nothing Then
                ModelState.AddModelError("", "User ID or email provided is invalid")
            Else
                Try

                    Dim newPassword As String = RandomString()
                    user.Password = GetMd5Hash(newPassword)
                    user.LastUpdateDate = Now
                    unitOfWork.Commit()

                    If System.Configuration.ConfigurationManager.AppSettings("SendNotification") = "yes" Then

                        Dim mail As New MailMessage()
                        mail.To.Add(user.UserEmailAddress)
                        mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Password recovery"
                        mail.Body = "Dear " & user.UserName & ",<br /><br />"
                        mail.Body &= "You had reset your password<br />Your new password is " & newPassword
                        mail.IsBodyHtml = True

                        ModuleDomainMgr.SendNotification(mail)

                    End If

                    ViewBag.Success = True
                Catch ex As Exception
                    ModelState.AddModelError("", ex.ToString())
                End Try

            End If
            Return View(model)
        End Function

        Private Function RandomString()
            Dim r As New Random
            Dim s As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
            Dim sb As New StringBuilder
            Dim cnt As Integer = 8
            For i As Integer = 1 To cnt
                Dim idx As Integer = r.Next(0, s.Length)
                sb.Append(s.Substring(idx, 1))
            Next
            Return sb.ToString()
        End Function

    End Class
End Namespace
