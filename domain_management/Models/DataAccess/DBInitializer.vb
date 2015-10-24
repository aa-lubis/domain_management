Imports System.Data.Entity
Imports domain_management.Entities
Imports domain_management.Repositories

Namespace DataAccess

    ' IT IS ONLY REQUIRED FOR CREATING THE DATABASE AT THE FIRST TIME
    ' DON'T USE IT WITH EXISTING DATABASE BECAUSE IT WILL DROP ALL TABLES IN DATABASE

    Public Class DBInitializer
        Inherits DropCreateDatabaseAlways(Of DomainMgrContexts)

        Protected Overrides Sub Seed(context As DomainMgrContexts)
            MyBase.Seed(context)

            Dim identitytypes As List(Of IdentityType) = New List(Of IdentityType) From {
               New IdentityType With {.IdentityTypeID = "01", .IdentityTypeDesc = "Identity Card"},
                New IdentityType With {.IdentityTypeID = "02", .IdentityTypeDesc = "Driving License"}
            }

            identitytypes.ForEach(Function(x) context.IdentityTypes.Add(x))

            Dim users As List(Of User) = New List(Of User) From {
                New User With {.UserId = "sa", .UserName = "Administrator", .UserEmailAddress = "admin@foo.com", .Password = GetMd5Hash("123456"), .Address = "local", .City = "Jakarta", .Organization = "organization", .PhoneNo = "+62812000000", .PostalCode = "000000", .Province = "DKI Jakarta", .IdentityTypeID = "01", .IdentityNo = "123456", .AttachmentFileName = "xxx.jpg", .BirthPlace = "Bandung", .BirthDay = Date.Parse("12/11/1984")},
                New User With {.UserId = "euhq", .UserName = "Admin", .UserEmailAddress = "a_lube@ymail.com", .Password = GetMd5Hash("123456"), .Address = "alamat", .City = "Jakarta", .Organization = "CVX", .PhoneNo = "088", .PostalCode = "40294", .Province = "DKI Jakarta", .IdentityTypeID = "01", .IdentityNo = "123456", .AttachmentFileName = "xxx.jpg", .BirthPlace = "Bandung", .BirthDay = Date.Parse("12/11/1984")},
                New User With {.UserId = "agip", .UserName = "Anggi Prima N", .UserEmailAddress = "anggiprimanyl@gmail.com", .Password = GetMd5Hash("123456"), .Address = "alamat", .City = "Jakarta", .Organization = "CVX", .PhoneNo = "088", .PostalCode = "40294", .Province = "DKI Jakarta", .IdentityTypeID = "01", .IdentityNo = "123456", .AttachmentFileName = "xxx.jpg", .BirthPlace = "Bandung", .BirthDay = Date.Parse("12/11/1984")},
                New User With {.UserId = "wyul", .UserName = "Widyanto Yulius", .UserEmailAddress = "yulius.candra@gmail.com", .Password = GetMd5Hash("123456"), .Address = "alamat", .City = "Jakarta", .Organization = "CVX", .PhoneNo = "088", .PostalCode = "40294", .Province = "DKI Jakarta", .IdentityTypeID = "01", .IdentityNo = "123456", .AttachmentFileName = "xxx.jpg", .BirthPlace = "Bandung", .BirthDay = Date.Parse("12/11/1984")},
                New User With {.UserId = "nunu", .UserName = "Nurul Chotimah", .UserEmailAddress = "aa_lubis@gmail.com", .Password = GetMd5Hash("123456"), .Address = "alamat", .City = "Jakarta", .Organization = "CVX", .PhoneNo = "088", .PostalCode = "40294", .Province = "DKI Jakarta", .IdentityTypeID = "01", .IdentityNo = "123456", .AttachmentFileName = "xxx.jpg", .BirthPlace = "Jakarta", .BirthDay = Date.Parse("12/11/1984")}
            }

            users.ForEach(Function(x) context.Users.Add(x))

            Dim roles As List(Of Role) = New List(Of Role) From {
                New Role With {.RoleId = "SA", .RoleName = "SUPERADMIN", .RoleDescription = "Super Admin Priviledge"},
                New Role With {.RoleId = "AD", .RoleName = "ADMIN", .RoleDescription = "Admin Priviledge"},
                New Role With {.RoleId = "RV", .RoleName = "REPORTVIEWER", .RoleDescription = "Report Viewer Priviledge"}
            }

            roles.ForEach(Function(x) context.Roles.Add(x))

            Dim bankAccountRepository = New BankAccountRepository(context)
            Dim bankaccounts As List(Of BankAccount) = New List(Of BankAccount) From {
                New BankAccount With {.Bank = "Mandiri", .BankAccountName = "MyIndo Cyber Media", .BankAccountNo = "123456", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New BankAccount With {.Bank = "BNI", .BankAccountName = "MyIndo Cyber Media", .BankAccountNo = "123456", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New BankAccount With {.Bank = "BCA", .BankAccountName = "MyIndo Cyber Media", .BankAccountNo = "123456", .CreatedBy = "SYSTEM", .CreatedDate = Now}
            }

            For Each item As BankAccount In bankaccounts
                item.BankAccountID = bankAccountRepository.GetNewBankAccountID
                context.BankAccounts.Add(item)
                context.SaveChanges()
            Next

            Dim userroles As List(Of UserRole) = New List(Of UserRole) From {
                New UserRole With {.UserId = "sa", .RoleId = "SA"},
                New UserRole With {.UserId = "euhq", .RoleId = "AD"},
                New UserRole With {.UserId = "nunu", .RoleId = "RV"}
            }

            userroles.ForEach(Function(x) context.UserRoles.Add(x))

            Dim productcategories As List(Of ProductCategory) = New List(Of ProductCategory) From {
                New ProductCategory With {.ProductCategoryID = "PC01", .ProductCategoryName = "Personal Hosting Indonesia", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New ProductCategory With {.ProductCategoryID = "PC02", .ProductCategoryName = "Professional Hosting Indonesia", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New ProductCategory With {.ProductCategoryID = "PC03", .ProductCategoryName = "Enterprise Hosting Indonesia", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New ProductCategory With {.ProductCategoryID = "PC04", .ProductCategoryName = "No Hosting", .CreatedBy = "SYSTEM", .CreatedDate = Now}
            }

            productcategories.ForEach(Function(x) context.ProductCategories.Add(x))

            Dim products As List(Of Product) = New List(Of Product) From {
                New Product With {.ProductID = "P01", .ProductCategoryID = "PC01", .ProductName = "Personal 50", .ProductDesc = "Space 50MB, Bandwidth 5GB, Email Account unlimited, Unlimited FTP Account, Unlimited MySQL Account, cPanel", .Price = 5000, .Counter = 12, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P02", .ProductCategoryID = "PC01", .ProductName = "Personal 100", .ProductDesc = "Space 100MB, Bandwidth 10GB, Email Account unlimited, Unlimited FTP Account, Unlimited MySQL Account, cPanel", .Price = 10000, .Counter = 6, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P03", .ProductCategoryID = "PC01", .ProductName = "Personal 250", .ProductDesc = "Space 250MB, Bandwidth 25GB, Email Account unlimited, Unlimited FTP Account, Unlimited MySQL Account, cPanel, 10 Domain Parking, 1 Domain Addon", .Price = 20000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P04", .ProductCategoryID = "PC02", .ProductName = "Professional 500", .ProductDesc = "Space 500MB, Bandwidth unlimited, Email Account unlimited, FTP unlimited account, MySQL unlimited account, cPanel", .Price = 35000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P05", .ProductCategoryID = "PC02", .ProductName = "Professional 1000", .ProductDesc = "Space 1GB, Bandwidth unlimited, Email Account unlimited, FTP unlimited account, MySQL unlimited account, cPanel", .Price = 60000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P06", .ProductCategoryID = "PC02", .ProductName = "Professional 2000", .ProductDesc = "Space 2GB, Bandwidth unlimited, Email Account unlimited, FTP unlimited account, MySQL unlimited account, cPanel", .Price = 100000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P07", .ProductCategoryID = "PC03", .ProductName = "Enterprise 5GB", .ProductDesc = "Space 5GB, 250GB Data Transfer, Unlimited Email Account, Unlimited FTP account, Unlimited MySQL account, cPanel", .Price = 190000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P08", .ProductCategoryID = "PC03", .ProductName = "Enterprise 10GB", .ProductDesc = "Space 10GB, 500GB Data Transfer, Unlimited Email Account, Unlimited FTP account, Unlimited MySQL account, cPanel", .Price = 390000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P09", .ProductCategoryID = "PC03", .ProductName = "Enterprise 20GB", .ProductDesc = "Space 20GB, 1000GB Data Transfer, Unlimited Email Account, Unlimited FTP account, Unlimited MySQL account, cPanel", .Price = 600000, .Counter = 1, .UnitPeriod = "Month", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New Product With {.ProductID = "P10", .ProductCategoryID = "PC04", .ProductName = "Domain Only", .ProductDesc = "No hosting space will be provided", .Price = 0}
            }

            products.ForEach(Function(x) context.Products.Add(x))

            Dim tldhosts As List(Of TLDHost) = New List(Of TLDHost) From {
                New TLDHost With {.TLD = ".com", .Host = "whois.internic.net", .Price = 120000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".ac.id", .Host = "whois.pandi.or.id", .Price = 100000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".biz.id", .Host = "whois.pandi.or.id", .Price = 100000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".co.id", .Host = "whois.pandi.or.id", .Price = 130000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".go.id", .Host = "whois.pandi.or.id", .Price = 80000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".id", .Host = "whois.pandi.or.id", .Price = 100000, .Requirement = "%3Cul%3E%3Cli%3Escan%20of%20identity%3C%2Fli%3E%3Cli%3Escan%20of%20company%20profile%3C%2Fli%3E%3C%2Ful%3E%3Cp%3E%3Cbr%3E%3C%2Fp%3E", .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".mil.id", .Host = "whois.pandi.or.id", .Price = 120000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".my.id", .Host = "whois.pandi.or.id", .Price = 100000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".net.id", .Host = "whois.pandi.or.id", .Price = 100000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".or.id", .Host = "whois.pandi.or.id", .Price = 90000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".sch.id", .Host = "whois.pandi.or.id", .Price = 100000, .CreatedBy = "SYSTEM", .CreatedDate = Now},
                New TLDHost With {.TLD = ".web.id", .Host = "whois.pandi.or.id", .Price = 100000, .CreatedBy = "SYSTEM", .CreatedDate = Now}
            }

            tldhosts.ForEach(Function(x) context.TLDHosts.Add(x))

            context.SaveChanges()

        End Sub

    End Class

End Namespace

