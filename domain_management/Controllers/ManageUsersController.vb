Imports PagedList

Imports domain_management.Entities
Imports domain_management.ViewModels
Imports domain_management.Repositories
Imports domain_management.Interfaces
Imports domain_management.HtmlHelpers

Namespace domain_management

    <Authorize(Roles:="SUPERADMIN")> _
    Public Class ManageUsersController
        Inherits System.Web.Mvc.Controller

        Private unitOfWork As IUnitOfWork
        Private userRepository As IUserRepository
        Private roleRepository As IRoleRepository
        Private userRoleRepository As IUserRoleRepository

        Sub New(ByVal unitOfWork As IUnitOfWork,
                ByVal userRepository As IUserRepository,
                ByVal roleRepository As IRoleRepository,
                ByVal userRoleRepository As IUserRoleRepository)
            Me.unitOfWork = unitOfWork
            Me.userRepository = userRepository
            Me.roleRepository = roleRepository
            Me.userRoleRepository = userRoleRepository
        End Sub

        '
        ' GET: /ManageUsers

        Public Function Index(ByVal filter As String, Optional ByVal p As Integer = 1) As ActionResult
            Dim model = userRepository.GetAll(includeProperties:="UserRoles").ToList()
            If Not String.IsNullOrEmpty(filter) Then
                filter = filter.ToLower()
                model = model.Where(Function(w) w.UserId.ToLower().Contains(filter) Or w.UserName.ToLower.Contains(filter) Or w.UserEmailAddress.ToLower().Contains(filter)).ToList
            End If
            Return View(model.ToPagedList(p, 10))
        End Function

        Public Function Edit(id As String) As ActionResult
            Dim model = userRepository.GetAll(includeProperties:="UserRoles").Where(Function(w) w.UserId = id).FirstOrDefault
            ViewBag.UserRoles = roleRepository.GetAll
            Return PartialView("_Edit", model)
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken(Salt:="edituser")> _
        Public Function Edit(model As User, userroles As String) As JsonResult
            Dim ret As String = ""
            Try
                Dim editeduser = userRepository.GetAll(includeProperties:="UserRoles, UserRoles.Role").Where(Function(w) w.UserId = model.UserId).FirstOrDefault
                Dim tobedeleteduserroles As New List(Of UserRole)
                Dim newUserRoles As String() = userroles.Split(New Char() {","})
                For Each UserRole In editeduser.UserRoles
                    If Not newUserRoles.Contains(UserRole.RoleId) Then
                        tobedeleteduserroles.Add(UserRole)
                    End If
                Next
                For Each UserRole In tobedeleteduserroles
                    If (UserRole.RoleId = "AD" Or UserRole.RoleId = "SA") And userRoleRepository.GetAll().Where(Function(w) w.RoleId = UserRole.RoleId).Count = 1 Then
                        ret = "There's must be at least one user with " & UserRole.Role.RoleName & " priviledge!"
                        Exit Try
                    End If
                    userRoleRepository.Delete(UserRole)
                Next
                For Each newUserRole In newUserRoles
                    If userRoleRepository.GetAll().Any(Function(a) a.UserId = model.UserId And a.RoleId = newUserRole) = False And Not String.IsNullOrEmpty(newUserRole.Trim()) Then
                        userRoleRepository.Insert(New UserRole With {.UserId = model.UserId, .RoleId = newUserRole})
                    End If
                Next
                unitOfWork.Commit()
                editeduser = userRepository.GetAll(includeProperties:="UserRoles, UserRoles.Role").Where(Function(w) w.UserId = model.UserId).FirstOrDefault
                Return New JsonResult With {.Data = New With {.result = "success", .id = model.UserId, .html = RenderPartialViewToString("_List", New List(Of User) From {editeduser})}}
            Catch ex As Exception
                unitOfWork.RollBack()
                ret = ex.ToString
            End Try
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

        Public Function Delete(id As String) As ActionResult
            Dim model = userRepository.GetById(id)
            Return PartialView("_Delete", model)
        End Function

        <HttpPost()> _
        <ValidateAntiForgeryToken(Salt:="deleteuser")> _
        Public Function Delete(model As User) As JsonResult
            Dim ret As String = ""
            Try
                If userRoleRepository.GetAll().Any(Function(w) w.UserId = model.UserId And w.RoleId = "SA") Then
                    ret = "Cannot delete user with SUPERADMIN priviledge!"
                    Exit Try
                End If
                If userRoleRepository.GetAll().Any(Function(w) w.UserId = model.UserId And w.RoleId = "AD") Then
                    ret = "Cannot delete user with ADMIN priviledge!"
                    Exit Try
                End If
                userRepository.Delete(model.UserId)
                unitOfWork.Commit()
                Return New JsonResult With {.Data = New With {.result = "success", .id = model.UserId}}
            Catch ex As Exception
                ret = ex.ToString
            End Try
            Return New JsonResult With {.Data = New With {.result = ret}}
        End Function

    End Class
End Namespace
