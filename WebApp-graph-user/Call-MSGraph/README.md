---
page_type: sample
languages:
  - csharp
products:
  - aspnet-core
  - azure-active-directory
name: Enable your Blazor WebAssembly to sign-in users with the Microsoft identity platform
urlFragment: ms-identity-blazor-wasm
description: "This sample demonstrates how to enable your Blazor WebAssembly to sign-in users with the Microsoft identity platform"
---
# Enable your Blazor WebAssembly to authorize users for calling Microsoft Graph

 1. [Overview](#overview)
 1. [Scenario](#scenario)
 1. [How to run this sample](#how-to-run-this-sample)
 1. [Running the sample](#running-the-sample)
 1. [Explore the sample](#explore-the-sample)
 1. [Deployment](#deployment)
 1. [More information](#more-information)
 1. [Community Help and Support](#community-help-and-support)
 1. [Contributing](#contributing)

## Overview

In the second chapter, we extend our ASP.NET Core Blazor WebAssembly standalone application to call a downstream API (Microsoft Graph) to provide additional value.

This sample demonstrates an ASP.NET Core Blazor WebAssembly standalone application that authenticates users against [Azure Active Directory (Azure AD)](https://docs.microsoft.com/azure/active-directory/fundamentals/active-directory-whatis) using the [Microsoft Authentication Library](https://docs.microsoft.com/azure/active-directory/develop/msal-overview). It then acquires an Access Token for Microsoft Graph and calls the [Microsoft Graph API](https://docs.microsoft.com/graph/overview).

## Scenario

With respect to the previous chapter of the tutorial, this chapter adds the following steps:

1. The client application acquires an Access Token for Microsoft Graph.
1. The **Access Token** is used as a *bearer* token to authorize the user to call the [Microsoft Graph API](https://docs.microsoft.com/graph/overview)
1. **Microsoft Graph API** responds with the resource that the user has access to.

![Overview](./ReadmeFiles/topology.jpg)

## Prerequisites

- Either [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download).
- System should have .NET SDK v3.1.6 or above. You can install it from [Download .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- An **Azure AD** tenant. For more information see: [How to get an Azure AD tenant](https://docs.microsoft.com/azure/active-directory/develop/quickstart-create-new-tenant)
- A user account in your **Azure AD** tenant. This sample will not work with a **personal Microsoft account**. Therefore, if you signed in to the [Azure portal](https://portal.azure.com) with a personal account and have never created a user account in your directory before, you need to do that now.

## Setup

### Clone or download this repository

From your shell or command line:

```console
git clone https://github.com/Azure-Samples/ms-identity-blazor-wasm.git
cd ms-identity-blazor-wasm\WebApp-graph-user\Call-MSGraph
```

or download and extract the repository .zip file.

> :warning: To avoid path length limitations on Windows, we recommend cloning into a directory near the root of your drive.

### Register the sample application(s) with your Azure Active Directory tenant

There is one project in this sample. To register it, you can:

- follow the steps below for manually register your apps
- or use PowerShell scripts that:
  - **automatically** creates the Azure AD applications and related objects (passwords, permissions, dependencies) for you.
  - modify the projects' configuration files.

<details>
  <summary>Expand this section if you want to use this automation:</summary>

> :warning: If you have never used **Azure AD Powershell** before, we recommend you go through the [App Creation Scripts](./AppCreationScripts/AppCreationScripts.md) once to ensure that your environment is prepared correctly for this step.

1. On Windows, run PowerShell as **Administrator** and navigate to the root of the cloned directory
1. In PowerShell run:

   ```PowerShell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process -Force
   ```

1. Run the script to create your Azure AD application and configure the code of the sample application accordingly.
1. In PowerShell run:

   ```PowerShell
   cd .\AppCreationScripts\
   .\Configure.ps1
   ```

   > Other ways of running the scripts are described in [App Creation Scripts](./AppCreationScripts/AppCreationScripts.md)
   > The scripts also provide a guide to automated application registration, configuration and removal which can help in your CI/CD scenarios.

</details>

#### Choose the Azure AD tenant where you want to create your applications

As a first step you'll need to:

1. Sign in to the [Azure portal](https://portal.azure.com).
1. If your account is present in more than one Azure AD tenant, select your profile at the top right corner in the menu on top of the page, and then **switch directory** to change your portal session to the desired Azure AD tenant.

#### Register the web app (WebApp-blazor-wasm)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `WebApp-blazor-wasm`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
   - In the **Redirect URI** section, select **Web** in the combo-box and enter the following redirect URI: `https://localhost:44314/authentication/login-callback`.
     > Note that there are more than one redirect URIs used in this sample. You'll need to add them from the **Authentication** tab later after the app has been created successfully.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. In the app's registration screen, select **Authentication** in the menu.
   - In the **Logout URL** section, set it to `https://localhost:44314/signout-oidc`.
   - In **Implicit grant** section, select the check box for ID tokens and Access tokens.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **API permissions** blade in the left to open the page where we add access to the APIs that your application needs.
   - Select the **Add a permission** button and then,
   - Ensure that the **Microsoft APIs** tab is selected.
   - In the *Commonly used Microsoft APIs* section, select **Microsoft Graph**
   - In the **Delegated permissions** section, select the **User.Read** in the list. Use the search box if necessary.
   - Select the **Add permissions** button at the bottom.

#### Configure the web app (WebApp-blazor-wasm) to use your app registration

Open the project in your IDE (like Visual Studio or Visual Studio Code) to configure the code.

> In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `blazorwasm-calls-MS-graph\wwwroot\appsettings.json` file.
1. Find the key `ClientId` and replace the existing value with the application ID (clientId) of the `WebApp-blazor-wasm` application copied from the Azure portal.
1. Find the key `Authority` and concatenate the tenant id as shown here: 'https://login.microsoftonline.com/[enter_your_tenantId]'.

## Running the sample

You can run the sample by using either Visual Studio or command line interface as shown below:

### Run the sample using Visual Studio

Clean the solution, rebuild the solution, and run it.

### Run the sample using a command line interface such as VS Code integrated terminal

#### Step 1. Install .NET Core dependencies

```console
cd WebApp-graph-user\Call-MSGraph
cd blazorwasm-calls-MS-graph
dotnet restore
```

#### Step 2. Trust development certificates

```console
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

Learn more about [HTTPS in .NET Core](https://docs.microsoft.com/aspnet/core/security/enforcing-ssl).

#### Step 3. Run the applications

In the console window execute the below command:

```console
dotnet run
```

## Explore the sample

> If you are using incognito mode of browser to run this sample then allow third party cookies.

1. Open your browser and navigate to `https://localhost:44314`.
1. Select the **Sign in** button on the top right corner. You will see claims from the signed-in user's token.

    ![UserClaims](./ReadmeFiles/UserClaims.png)

1. Select **Profile** from navigation bar on the left. If user has signed-in then information fetched from Microsoft Graph is displayed, otherwise login screen will appear.

    ![UserProfile](./ReadmeFiles/UserProfile.png)

> :information_source: Did the sample not work for you as expected? Then please reach out to us using the [GitHub Issues](../../../../issues) page.

## We'd love your feedback!

Were we successful in addressing your learning objective? [Do consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUMEw0UFNBVVBEV1E3VFNBU1I0T05TNzhPViQlQCN0PWcu)

## About the code

1. In Program.cs, Main method registers **AddMsalAuthentication** as explained below:

   ```csharp
   builder.Services.AddMsalAuthentication(options =>
   {
      builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
   });
   ```

   **AddMsalAuthentication** is an extension method provided by the [Microsoft.Authentication.WebAssembly.Msal](https://www.nuget.org/packages/Microsoft.Authentication.WebAssembly.Msal) package and it provides support for authenticating users with the Microsoft Identity Platform.

1. **Index.razor** is the landing page when application starts. Index.razor contains child component called `UserClaims`. If user is authenticated successfully, `UserClaims` displays a few claims present in the ID Token issued by Azure AD.

1. In the `UserClaimsBase.cs` class, **GetClaimsPrincipalData** method retrieves signed-in user's claims using the **GetAuthenticationStateAsync()** method of the **AuthenticationStateProvider** class.

   ```csharp
    public class UserClaimsBase: ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set;
        }
        protected string _authMessage;
        protected IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();
        private string[] returnClaims = { "name", "preferred_username", "tid", "oid" };
        protected override async Task OnInitializedAsync()
        {
            await GetClaimsPrincipalData();
        }
        private async Task GetClaimsPrincipalData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                _authMessage = $"{user.Identity.Name} is authenticated.";
                _claims = user.Claims.Where(x => returnClaims.Contains(x.Type));
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }
    }
   ```

1. In Program.cs, Main method registers AddMicrosoftGraphClient as explained below:

    ```csharp
    builder.Services.AddMicrosoftGraphClient("https://graph.microsoft.com/User.Read");
    ```

    **AddMsalAuthentication** is an extension method provided by GraphClientExtensions.cs class.

1. **UserProfile.razor** component displays user information retrieved by **GetUserProfile** method of **UserProfileBase.cs**.

    **UserProfileBase.cs** calls Microsoft Graph `/me` endpoint to retrieve user information.

    ```csharp
    public class UserProfileBase : ComponentBase
    {
        [Inject]
        GraphServiceClient GraphClient { get; set; }
        protected User _user=new User();
        protected override async Task OnInitializedAsync()
        {
            await GetUserProfile();
        }
        private async Task GetUserProfile()
        {
            ...
                var request = GraphClient.Me.Request();
                _user = await request.GetAsync();
            ...
        }
    }
    ```

## Deployment

See [README.md](../../Deploy-to-Azure/README.md) to deploy this sample to Azure.

## More information

- [Microsoft Graph permissions reference](https://docs.microsoft.com/graph/permissions-reference)
- [Authentication and authorization basics for Microsoft Graph](https://docs.microsoft.com/graph/auth/auth-concepts)
- [Secure an ASP.NET Core Blazor WebAssembly standalone app with Azure Active Directory](https://docs.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-azure-active-directory)
- [ASP.NET Core Blazor WebAssembly additional security scenarios](https://docs.microsoft.com/aspnet/core/blazor/security/webassembly/additional-scenarios)

## Community Help and Support

Use [Stack Overflow](http://stackoverflow.com/questions/tagged/msal) to get support from the community.
Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before.
Make sure that your questions or comments are tagged with [`azure-active-directory` `ms-identity` `msal`].

If you find a bug in the sample, raise the issue on [GitHub Issues](../../../../issues).

To provide feedback on or suggest features for Azure Active Directory, visit [User Voice page](https://feedback.azure.com/forums/169401-azure-active-directory).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](../../CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.