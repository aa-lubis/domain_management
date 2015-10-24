Imports System.Web.Security
Imports System.Security
Imports System.Security.Principal

Imports domain_management.Entities
Imports domain_management.Repositories
Imports domain_management.Interfaces

Namespace Infrastructure

    Public Class CustomRoleProvider
        Inherits RoleProvider

        Private ReadOnly Property userRepository As IUserRepository
            Get
                Return DependencyResolver.Current.GetService(GetType(IUserRepository))
            End Get
        End Property

        Private ReadOnly Property userRoleRepository As IUserRoleRepository
            Get
                Return DependencyResolver.Current.GetService(GetType(UserRoleRepository))
            End Get
        End Property

        Public Overrides Sub AddUsersToRoles(usernames() As String, roleNames() As String)
            Throw New NotImplementedException
        End Sub

        Public Overrides Property ApplicationName As String

        Public Overrides Sub CreateRole(roleName As String)
            Throw New NotImplementedException
        End Sub

        Public Overrides Function DeleteRole(roleName As String, throwOnPopulatedRole As Boolean) As Boolean
            Throw New NotImplementedException
        End Function

        Public Overrides Function FindUsersInRole(roleName As String, usernameToMatch As String) As String()
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetAllRoles() As String()
            'Using uow As New UnitOfWork
            'Return uow.RoleRepository.GetAll(Function(w) w.RoleName).ToArray()
            'End Using
            Throw New NotImplementedException
        End Function

        Public Overrides Function GetRolesForUser(username As String) As String()
            Dim user As User = userRepository.GetAll().Where(Function(w) w.UserId = username).FirstOrDefault
            If user Is Nothing Then Return New String() {}
            If user.UserRoles Is Nothing Then
                Return New String() {}
            Else
                Return user.UserRoles.Select(Function(u) u.Role).Select(Function(u) u.RoleName).ToArray()
            End If
        End Function

        Public Overrides Function GetUsersInRole(roleName As String) As String()
            Throw New NotImplementedException()
        End Function

        Public Overrides Function IsUserInRole(username As String, roleName As String) As Boolean

            Dim user As User = userRepository.GetAll().Where(Function(w) w.UserId = username).FirstOrDefault
            If user Is Nothing Then Return False
            Return Not user.UserRoles Is Nothing And user.UserRoles.Select(Function(u) u.Role).Any(Function(r) r.RoleName = roleName)

        End Function

        Public Overrides Sub RemoveUsersFromRoles(usernames() As String, roleNames() As String)
            Throw New NotImplementedException
        End Sub

        Public Overrides Function RoleExists(roleName As String) As Boolean
            Throw New NotImplementedException
        End Function
    End Class

End Namespace
