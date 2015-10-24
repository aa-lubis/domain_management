Imports System.Web.Security
Imports System.Security
Imports System.Security.Principal
Imports System.Security.Cryptography
Imports System.Web.Mvc
Imports Microsoft.Practices.Unity
Imports Unity.Mvc3

Imports domain_management.DataAccess
Imports domain_management.Entities
Imports domain_management.Interfaces
Imports domain_management.Repositories

Namespace Infrastructure

    Public Class CustomMembershipProvider
        Inherits MembershipProvider

        <Dependency> _
        Private ReadOnly Property userRepository As IUserRepository
            Get
                Return DependencyResolver.Current.GetService(GetType(IUserRepository))
            End Get
        End Property

        Public Overrides Property ApplicationName As String
            Get
                Throw New NotImplementedException
            End Get
            Set(value As String)
                Throw New NotImplementedException
            End Set
        End Property

        Public Overrides Function ChangePassword(username As String, oldPassword As String, newPassword As String) As Boolean
            Throw New NotImplementedException
        End Function

        Public Overrides Function ChangePasswordQuestionAndAnswer(username As String, password As String, newPasswordQuestion As String, newPasswordAnswer As String) As Boolean
            Throw New NotImplementedException
        End Function

        Public Overrides Function CreateUser(username As String, password As String, email As String, passwordQuestion As String, passwordAnswer As String, isApproved As Boolean, providerUserKey As Object, ByRef status As MembershipCreateStatus) As MembershipUser
            'Dim args = New ValidatePasswordEventArgs(username, password, True)
            'OnValidatingPassword(args)

            'If args.Cancel Then
            '    status = MembershipCreateStatus.InvalidPassword
            '    Return Nothing
            'End If

            'If RequiresUniqueEmail And GetUserNameByEmail(email) <> String.Empty Then
            '    status = MembershipCreateStatus.DuplicateEmail
            '    Return Nothing
            'End If

            'Dim user = GetUser(username, True)

            'If user Is Nothing Then
            '    Dim userObj As New User With {.UserName = username, .Password = GetMd5Hash(password), .UserEmailAddress = email}
            '    Using uow As New UnitOfWork
            '        uow.UserRepository.Insert(userObj)
            '        uow.Save()
            '    End Using
            '    status = MembershipCreateStatus.Success
            '    Return GetUser(username, True)
            'End If

            'status = MembershipCreateStatus.DuplicateUserName
            'Return Nothing
            Throw New NotImplementedException
        End Function

        Public Overrides Function DeleteUser(username As String, deleteAllRelatedData As Boolean) As Boolean
            Throw New NotImplementedException
        End Function

        Public Overrides ReadOnly Property EnablePasswordReset As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property EnablePasswordRetrieval As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides Function FindUsersByEmail(emailToMatch As String, pageIndex As Integer, pageSize As Integer, ByRef totalRecords As Integer) As MembershipUserCollection
            Throw New NotImplementedException
        End Function

        Public Overrides Function FindUsersByName(usernameToMatch As String, pageIndex As Integer, pageSize As Integer, ByRef totalRecords As Integer) As MembershipUserCollection
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetAllUsers(pageIndex As Integer, pageSize As Integer, ByRef totalRecords As Integer) As MembershipUserCollection
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetNumberOfUsersOnline() As Integer
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetPassword(username As String, answer As String) As String
            Throw New NotImplementedException
        End Function

        Public Overloads Overrides Function GetUser(providerUserKey As Object, userIsOnline As Boolean) As MembershipUser
            Throw New NotImplementedException
        End Function

        Public Overloads Overrides Function GetUser(username As String, userIsOnline As Boolean) As MembershipUser
            Dim users As List(Of User) = userRepository.GetAll().Where(Function(w) w.UserName = username).ToList()
            If Not users Is Nothing Then
                If users.Count > 0 Then
                    Dim user As User = users(0)
                    Dim memUser = New MembershipUser("CustomMembershipProvider", username, user.UserId, user.UserEmailAddress,
                                                     String.Empty, String.Empty,
                                                     True, False, DateTime.MinValue,
                                                     DateTime.MinValue,
                                                     DateTime.MinValue,
                                                     DateTime.Now, DateTime.Now)
                    Return memUser
                End If
            End If
            Return Nothing
        End Function

        Public Overrides Function GetUserNameByEmail(email As String) As String
            'Throw New NotImplementedException
            Dim userName As String = String.Empty

            Dim users As List(Of User) = userRepository.GetAll().Where(Function(w) w.UserEmailAddress = email).ToList()
            If users.Count > 0 Then userName = users(0).UserName.ToString()

            Return userName
        End Function

        Public Overrides ReadOnly Property MaxInvalidPasswordAttempts As Integer
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property MinRequiredNonAlphanumericCharacters As Integer
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property MinRequiredPasswordLength As Integer
            Get
                Return 6
            End Get
        End Property

        Public Overrides ReadOnly Property PasswordAttemptWindow As Integer
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property PasswordFormat As MembershipPasswordFormat
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property PasswordStrengthRegularExpression As String
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property RequiresQuestionAndAnswer As Boolean
            Get
                Throw New NotImplementedException
            End Get
        End Property

        Public Overrides ReadOnly Property RequiresUniqueEmail As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides Function ResetPassword(username As String, answer As String) As String
            Throw New NotImplementedException
        End Function

        Public Overrides Function UnlockUser(userName As String) As Boolean
            Throw New NotImplementedException
        End Function

        Public Overrides Sub UpdateUser(user As MembershipUser)
            Throw New NotImplementedException
        End Sub

        Public Overrides Function ValidateUser(userid As String, password As String) As Boolean
            Dim md5Hash = GetMd5Hash(password)

            Dim Md5Pass As String = GetMd5Hash(password)
            Return userRepository.GetAll().Any(Function(a) a.UserId = userid And a.Password = Md5Pass)

        End Function

    End Class

End Namespace
