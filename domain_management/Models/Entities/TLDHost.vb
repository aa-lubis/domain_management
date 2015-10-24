Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema


Namespace Entities

    Public Class TLDHost

        <Key> _
        <Required(ErrorMessage:="TLD field cannot be empty")> _
        <MaxLength(10)> _
        <Display(Name:="Top Level Domain")> _
        <RegularExpression("^.+([a-z0-9]{1,20})+([a-z0-9.]{1,20})", ErrorMessage:="Top level domain pattern is not recognized")> _
        Public Property TLD As String

        <Required(ErrorMessage:="Host field cannot be empty")> _
        <MaxLength(500)> _
        <Display(Name:="Host / Whois Server")> _
         <RegularExpression("([a-z0-9-]{3,20})+([a-z0-9.]{1,20})", ErrorMessage:="Host / Whois Server pattern is not recognized")> _
        Public Property Host As String

        <Required> _
        <DisplayFormat(DataFormatString:="{0:#,##0.##}", ApplyFormatInEditMode:=False)> _
        Public Property Price As Integer

        <DisplayFormat(DataFormatString:="{0:#,##0.##}", ApplyFormatInEditMode:=False)> _
        Public Property Discount As Integer

        <Column(TypeName:="text")> _
        Public Property Requirement As String

        <MaxLength(20)> _
        Public Property CreatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property CreatedDate As Date?

        <MaxLength(20)> _
        Public Property LastUpdatedBy As String

        <DisplayFormat(DataFormatString:="{0:dd-MMM-yyyy}")> _
        Public Property LastUpdatedDate As Date?

    End Class

End Namespace
