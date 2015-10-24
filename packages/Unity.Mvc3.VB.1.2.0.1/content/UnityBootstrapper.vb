Imports Microsoft.Practices.Unity
Imports System.Web.Mvc
Imports Unity.MVC3

''' <summary>
''' <para>
''' This is a VB.Net version of the Bootstrapper found in the Unity.Mvc3
''' package available from  NuGet.   It  depends  on  my  Unity.Mvc3.DLL
''' package which I created to make it easier for developers to use with
''' other .Net languages without polluting their projects with .cs files
''' that they end up having to delete.
''' </para>
''' <para>
''' Getting started with Unity.Mvc3<br />
''' <hr />
''' </para>
''' <para>
''' To get started, just add a call to Bootstrapper.Initialise() in  the
''' Application_Start method  of Global.asax.vb  and  the MVC  framework
''' will  then use  the  Unity.Mvc3  DependencyResolver to  resolve your
''' components.
''' </para>
''' <para>
''' There is code in the bootstrapper to initialise the Unity  container
''' and register  all  the  controllers  in  the  current assembly.  Any
''' components  that  you need  to  inject  should be registered in  the
''' BuildUnityContainer method of the Bootstrapper.  All components  that
''' implement the IDisposable interface should  be  registered  with  the
''' HierarchicalLifetimeManager to ensure that  they are  disposed of  at
''' the end of the request.
''' </para>
''' <para>
''' You can find out more about Unity.Mvc3 by visiting:<br />
''' <br />
''' http://is.gd/73xS3s
''' </para>
''' </summary>
''' <remarks>
''' <para>
''' Converted to VB.Net:  4/10/2012 at about six PM Central Time.<br />
''' By:  Bryan Johns (bjohns@greendragonweb.com)
''' </para>
''' </remarks>
Public NotInheritable Class UnityBootstrapper
	Private Sub New()
	End Sub
	Public Shared Sub Initialise()
		Dim container = BuildUnityContainer()

		DependencyResolver.SetResolver(New UnityDependencyResolver(container))
	End Sub

	Private Shared Function BuildUnityContainer() As IUnityContainer
		Dim container = New UnityContainer()

		' register all your components with the container here
		' e.g. container.RegisterType<ITestService, TestService>();            

		Return container
	End Function
End Class

