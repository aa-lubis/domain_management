Imports System.ComponentModel.DataAnnotations

Namespace Entities

    Public Class User

        <Key> _
        <Display(Name:="User ID")> _
        <MaxLength(20)> _
        <RegularExpression("^([a-z0-9](?(?!__|--)[a-z0-9_])+[a-z0-9])$", ErrorMessage:="User ID can only contains lower case characters, numbers and underscore")> _
        Public Property UserId As String

        <Required> _
        <Display(Name:="Full name")> _
        <MaxLength(50)> _
        Public Property UserName As String

        <MaxLength(200)> _
        Public Property Password As String

        <Required>
        <Display(Name:="Birth place")> _
        <MaxLength(50)> _
        Public Property BirthPlace As String

        <Required>
        <Display(Name:="Birth day")> _
        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}", ApplyFormatInEditMode:=True)> _
        Public Property BirthDay As Date

        <Required> _
        <Display(Name:="Email address")> _
        <RegularExpression("^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage:="Your email address is not in a valid format")> _
        <MaxLength(50)> _
        Public Property UserEmailAddress As String

        <Required> _
        <Display(Name:="Organization")> _
        <MaxLength(300)> _
        Public Property Organization As String

        <Required> _
        <Display(Name:="Address")> _
        <MaxLength(300)> _
        Public Property Address As String

        <Required> _
        <Display(Name:="City")> _
        <MaxLength(30)> _
        Public Property City As String

        <Required> _
        <Display(Name:="Province")> _
        <MaxLength(30)> _
        Public Property Province As String

        <Required> _
        <RegularExpression("[0-9]{5}$", ErrorMessage:="postal code pattern is not recognized")> _
        <Display(Name:="Postal Code")> _
        <MaxLength(6)> _
        Public Property PostalCode As String

        <Required> _
        <Display(Name:="Phone No.")> _
        <RegularExpression("^\+?[0-9]{4,20}$", ErrorMessage:="phone number pattern is not recognized")> _
        <MaxLength(30)> _
        Public Property PhoneNo As String

        <Required> _
        <MaxLength(2)> _
        <Display(Name:="Identity Type")> _
        Public Property IdentityTypeID As String

        <Required> _
        <MaxLength(40)> _
        <Display(Name:="Identity No.")> _
        <RegularExpression("^[0-9]+$", ErrorMessage:="identity number pattern is not recognized")> _
        Public Property IdentityNo As String

        <MaxLength(200)> _
        <Display(Name:="Scan of Identity")> _
        Public Property AttachmentFileName As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property CreateDate As Date?

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property LastUpdateDate As Date?

        Public Overridable Property UserRoles As ICollection(Of UserRole)
        Public Overridable Property IdentityType As IdentityType

    End Class

End Namespace

