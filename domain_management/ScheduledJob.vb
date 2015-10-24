Imports System.Net
Imports System.Net.Mail
Imports System.Linq
Imports Quartz

Imports domain_management.Interfaces
Imports domain_management.Repositories
Imports domain_management.Entities
Imports domain_management.DataAccess

Public Class ScheduledJob
    Implements IJob

    Private userRepository As IUserRepository
    Private domainRepository As IDomainRepository
    Private userRoleRepository As IUserRoleRepository
    Private unitOfWork As IUnitOfWork

    Public Sub New(userRepository As IUserRepository,
                   domainRepository As IDomainRepository,
                   userRoleRepository As IUserRoleRepository,
                   unitOfWork As IUnitOfWork)
        Me.userRepository = userRepository
        Me.domainRepository = domainRepository
        Me.userRoleRepository = userRoleRepository
        Me.unitOfWork = unitOfWork
    End Sub

    Public Sub Execute(context As IJobExecutionContext) Implements IJob.Execute
        SendNotification()
    End Sub

    Public Sub CleanUpDomain()

        Dim CurrentDate As Date = Now.Date
        Dim ToBeSuspendedDate As Date = Now.Date.AddDays(-7)

        Dim expireddomains As List(Of Domain) = domainRepository.GetAll().Where(Function(w) w.DomainExpireDate < ToBeSuspendedDate And w.ProductExpireDate < CurrentDate And w.Status = "ACTIVE").ToList()
        For Each domain As Domain In expireddomains
            domain.Status = "SUSPENDED"
            domain.LastUpdateBy = "SYSTEM"
            domain.LastUpdateDate = Now
        Next
        unitOfWork.Commit()

    End Sub

    Public Sub SendNotification()

        Try

            CleanUpDomain()

            Dim CheckDate As Date = Now.AddMonths(1).Date

            Dim tobeexpireddomains As List(Of Domain) = domainRepository.GetAll().Where(Function(w) w.DomainExpireDate <= CheckDate And w.ProductExpireDate > CheckDate And w.Status = "ACTIVE").ToList()
            Dim tobeexpiredproducts As List(Of Domain) = domainRepository.GetAll().Where(Function(w) w.ProductExpireDate <= CheckDate And w.DomainExpireDate > CheckDate And w.Status = "ACTIVE").ToList
            Dim tobeexpireddomainandproducts As List(Of Domain) = domainRepository.GetAll().Where(Function(w) w.ProductExpireDate <= CheckDate And w.DomainExpireDate <= CheckDate And w.Status = "ACTIVE").ToList

            For Each domain As Domain In tobeexpireddomains

                Dim user As User = userRepository.GetById(domain.RegisterBy)
                Dim useremail As String = user.UserEmailAddress

                Dim mail As New MailMessage()
                mail.To.Add(useremail)
                mail.Subject = "Domain Management - Domain Expiration Notification"
                mail.Body = "Dear " & user.UserName & ",<br /><br />"
                If domain.DomainExpireDate > Now.Date Then
                    mail.Body &= "Your domain " & domain.DomainName & " will be expired on " & Format(domain.DomainExpireDate, "d MMMM yyyy") & "!"
                ElseIf domain.DomainExpireDate = Now.Date Then
                    mail.Body &= "Your domain expires today!"
                Else
                    mail.Body &= "Your domain " & domain.DomainName & " has expired since " & Format(domain.DomainExpireDate, "d MMMM yyyy") & "!"
                End If
                mail.Body &= "<br />Please extend it to continue this service or suspend it anyway"
                mail.Body &= "<br /><br />Thank you"
                mail.IsBodyHtml = True
                Send(mail)

                domain.LastNotificationDate = Now
                unitOfWork.Commit()

            Next

            For Each domain As Domain In tobeexpiredproducts

                Dim user As User = userRepository.GetById(domain.RegisterBy)
                Dim useremail As String = user.UserEmailAddress

                Dim mail As New MailMessage()
                mail.To.Add(useremail)
                mail.Subject = "Domain Management - Hosting Product Expiration Notification"
                mail.Body = "Dear " & user.UserName & ",<br /><br />"
                If domain.ProductExpireDate > Now.Date Then
                    mail.Body &= "Your hosting product for " & domain.DomainName & " will be expired on " & Format(domain.ProductExpireDate, "d MMMM yyyy") & "!"
                ElseIf domain.ProductExpireDate = Now.Date Then
                    mail.Body &= "Your hosting product expires today!"
                Else
                    mail.Body &= "Your hosting product for " & domain.DomainName & " has expired since " & Format(domain.DomainExpireDate, "d MMMM yyyy") & "!"
                End If
                mail.Body &= "<br />Please extend it for continue this service"
                mail.Body &= "<br /><br />Thank you"
                mail.IsBodyHtml = True
                Send(mail)

                domain.LastNotificationDate = Now
                unitOfWork.Commit()

            Next

            For Each domain As Domain In tobeexpireddomainandproducts

                Dim user As User = userRepository.GetById(domain.RegisterBy)
                Dim useremail As String = user.UserEmailAddress

                Dim mail As New MailMessage()
                mail.To.Add(useremail)
                mail.Subject = "Domain Management - Domain and Hosting Product Expiration Notification"
                mail.Body = "Dear " & user.UserName & ",<br /><br />"
                If domain.DomainExpireDate > Now.Date Then
                    mail.Body &= "Your domain " & domain.DomainName & " will be expired on " & Format(domain.DomainExpireDate, "d MMMM yyyy") & "<br />"
                ElseIf domain.DomainExpireDate = Now.Date Then
                    mail.Body &= "Your domain expires today" & "<br />"
                Else
                    mail.Body &= "Your domain " & domain.DomainName & " has expired since " & Format(domain.DomainExpireDate, "d MMMM yyyy") & "<br />"
                End If
                If domain.ProductExpireDate > Now.Date Then
                    mail.Body &= "and its hosting product will be expired on " & Format(domain.ProductExpireDate, "d MMMM yyyy")
                ElseIf domain.ProductExpireDate = Now.Date Then
                    mail.Body &= "and its hosting product expires today"
                Else
                    mail.Body &= "and its hosting product has expired since " & Format(domain.ProductExpireDate, "d MMMM yyyy")
                End If
                mail.Body &= "<br />Please extend it to continue this service or suspend it anyway"
                mail.Body &= "<br /><br />Thank you"
                mail.IsBodyHtml = True
                Send(mail)

                domain.LastNotificationDate = Now
                unitOfWork.Commit()
            Next

        Catch ex As Exception


            Dim mail As New MailMessage

            ' get admin roles
            Dim adminroles As List(Of UserRole) = userRoleRepository.GetAll().Where(Function(w) w.RoleId = "AD").Distinct().ToList()
            For Each UserRole In adminroles
                Dim adminuser As User = userRepository.GetById(UserRole.UserId)
                Dim emailaddress As String = adminuser.UserEmailAddress
                If Not String.IsNullOrEmpty(emailaddress) Then mail.To.Add(emailaddress)
            Next
            mail.Subject = System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " - Scheduler error"
            mail.Body = ex.ToString
            mail.IsBodyHtml = True

            Send(mail)

        End Try

      

    End Sub

    Public Sub Send(mail As MailMessage)

        mail.Body &= "<br /><br /><span style=""font-size: .8em; color: #888"">This notification has been sent to the email address associated with your " & _
            System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " account.<br />" & _
            "This email message was auto-generated. Please do not respond</span>"

        Dim smtp As New System.Net.Mail.SmtpClient
        smtp.Send(mail)

    End Sub


End Class
