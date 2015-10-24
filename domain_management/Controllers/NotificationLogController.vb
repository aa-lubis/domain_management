Imports System.Net.Mail
Imports PagedList

Imports domain_management.Interfaces
Imports domain_management.Entities
Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize(Roles:="SUPERADMIN")> _
    Public Class NotificationLogController
        Inherits System.Web.Mvc.Controller

        Private unitOfWork As IUnitOfWork
        Private notificationLogRepository As INotificationLogRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal notificationlogRepository As INotificationLogRepository)
            Me.notificationLogRepository = notificationlogRepository
            Me.unitOfWork = unitOfWork
        End Sub

        Function Index(ByVal filter As String, Optional ByVal p As Integer = 1) As ActionResult
            Return View(Me.notificationLogRepository.GetAll().ToPagedList(p, 10))
        End Function

        Function GetNotificationLogList(filter As String) As ActionResult
            Dim model As IEnumerable(Of NotificationLog)
            If filter = "sent" Then
                model = notificationLogRepository.GetAll().Where(Function(w) w.Status.ToLower = "sent").ToList
            ElseIf filter = "unsent" Then
                model = notificationLogRepository.GetAll().Where(Function(w) w.Status.ToLower <> "sent").ToList
            Else
                model = notificationLogRepository.GetAll().ToList
            End If
            Return PartialView("_NotificationLogList", model)
        End Function

        <HttpPost()> _
        Function ResendNotification(id As String) As JsonResult
            Dim ret As String = ""
            Dim notificationlog As NotificationLog = notificationLogRepository.GetById(id)
            If Not notificationlog Is Nothing Then
                Try
                    ' resend notification
                    Dim mail As New MailMessage()

                    For Each sendto As String In notificationlog.SendTo.Split(New Char() {";"})
                        If Not String.IsNullOrEmpty(sendto) Then mail.To.Add(sendto)
                    Next
                    For Each ccto As String In notificationlog.SendCC.Split(New Char() {";"})
                        If Not String.IsNullOrEmpty(ccto) Then mail.CC.Add(ccto)
                    Next
                    For Each bccto As String In notificationlog.SendBCC.Split(New Char() {";"})
                        If Not String.IsNullOrEmpty(bccto) Then mail.Bcc.Add(bccto)
                    Next
                    mail.Subject = notificationlog.Subject
                    mail.Body = notificationlog.Body
                    mail.IsBodyHtml = True
                    ModuleDomainMgr.SendNotification(mail, id)

                    Return New JsonResult With {.Data = New With {.result = "success", .id = id, .html = RenderPartialViewToString("_NotificationLogList", New List(Of NotificationLog) From {notificationLogRepository.GetById(id)})}}
                Catch ex As Exception
                    ret &= ex.ToString
                End Try
            End If
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

    End Class
End Namespace
