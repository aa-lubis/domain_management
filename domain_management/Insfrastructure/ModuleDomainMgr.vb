Imports domain_management.Entities
Imports domain_management.Repositories
Imports domain_management.Interfaces

Module ModuleDomainMgr

    Public Function GetMd5Hash(value As String) As String
        Dim md5Hasher = System.Security.Cryptography.MD5.Create()
        Dim data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value))
        Dim sBuilder = New StringBuilder()
        For i As Integer = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next
        Return sBuilder.ToString()
    End Function

    Public Function DomainAvailability(ByVal server As String, ByVal domain As String) As Integer
        Dim ret As String = 1 ' available
        Dim tcpWhois As System.Net.Sockets.TcpClient = Nothing
        Try

            Dim regex As New Regex("^(?!www\.)((?!-)[A-Za-z0-9-]{1,63}(?<!-)\.)+[A-Za-z]{2,6}$")
            Dim match As Match = regex.Match(domain)

            If match.Success = False Then
                ret = 3
            Else

                tcpWhois = New System.Net.Sockets.TcpClient(server, 43)

                Dim nsWhois As System.Net.Sockets.NetworkStream = tcpWhois.GetStream()
                Dim bfWhois As New System.IO.BufferedStream(nsWhois)
                Dim strmSend As New System.IO.StreamWriter(bfWhois)
                strmSend.WriteLine(domain)

                strmSend.Flush()

                Dim strmReceive As New System.IO.StreamReader(bfWhois)
                Dim response As String

                response = strmReceive.ReadToEnd()
                response = response.ToLower()

                If response.Length > 0 And Not response.Contains("no match") And Not response.Contains("not found") Then
                    ret = 0 ' taken
                End If

            End If

        Catch ex As Exception
            ret = 2
        Finally
            If Not tcpWhois Is Nothing Then tcpWhois.Close()
        End Try
        Return ret
    End Function

    Public Sub SendNotification(ByVal mail As System.Net.Mail.MailMessage, Optional id As String = Nothing)
        Dim status As String
        Try

            If id Is Nothing Then
                mail.Body &= "<br /><br /><span style=""font-size: .8em; color: #888"">This notification has been sent to the email address associated with your " & _
                    System.Configuration.ConfigurationManager.AppSettings("WebTitle") & " account.<br />" & _
                    "This email message was auto-generated. Please do not respond</span>"
            End If

            Dim smtp As New System.Net.Mail.SmtpClient
            smtp.Send(mail)
            status = "Sent"

        Catch ex As Exception
            status = ex.ToString
        End Try

        ' save notification log
        Dim unitOfWork As IUnitOfWork = DependencyResolver.Current.GetService(GetType(IUnitOfWork))
        Dim notificationLogRepository As INotificationLogRepository = DependencyResolver.Current.GetService(GetType(INotificationLogRepository))
        If String.IsNullOrEmpty(id) Then

            Dim notificationlogprefix As String = "L" & Format(Now, "yyMMdd")
            Dim newnotificationlogid As Object = notificationLogRepository.GetAll().Where(Function(w) w.LogID.StartsWith(notificationlogprefix)).Max(Function(m) m.LogID)
            If String.IsNullOrEmpty(newnotificationlogid) Then
                newnotificationlogid = notificationlogprefix & "000001"
            Else
                newnotificationlogid = notificationlogprefix + CStr(CInt(newnotificationlogid.Substring(newnotificationlogid.Length - 6)) + 1000001).Substring(1)
            End If

            Dim newnotificationlog = New NotificationLog With { _
                .LogID = newnotificationlogid,
                .Subject = mail.Subject,
                .SendTo = String.Join(";", mail.To),
                .SendCC = String.Join(";", mail.CC),
                .SendBCC = String.Join(";", mail.Bcc),
                .LogCreateTime = Now,
                .Body = mail.Body,
                .Status = status
            }
            notificationLogRepository.Insert(newnotificationlog)
        Else
            Dim notificationlog As NotificationLog = notificationLogRepository.GetById(id)
            If Not notificationlog Is Nothing Then
                notificationlog.Status = status
                notificationlog.ResendTime = Now
            End If
        End If
        unitOfWork.Commit()

    End Sub


End Module
