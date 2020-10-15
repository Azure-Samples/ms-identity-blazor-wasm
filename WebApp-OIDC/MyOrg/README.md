---
page_type: sample
languages:
  - csharp
products:
  - dotnet-core
  - azure-active-directory
name: Enable your Blazor Single-page Application (SPA) to sign-in users with the Microsoft identity platform
urlFragment: ms-identity-blazor-wasm
description: "This sample demonstrates how to enable your Blazor Single-page Application (SPA) to sign-in users with the Microsoft identity platform"
---
# Enable your Blazor Single-page Application (SPA) to sign-in users with the Microsoft identity platform

 1. [Overview](#overview)
 1. [Scenario](#scenario)
 1. [Contents](#contents)
 1. [Prerequisites](#prerequisites)
 1. [Setup](#setup)
 1. [Registration](#registration)
 1. [Running the sample](#running-the-sample)
 1. [Explore the sample](#explore-the-sample)
 1. [About the code](#about-the-code)
 1. [Deployment](#deployment)
 1. [More information](#more-information)
 1. [Community Help and Support](#community-help-and-support)
 1. [Contributing](#contributing)
 1. [Code of Conduct](#code-of-conduct)

![Build badge](https://identitydivision.visualstudio.com/_apis/public/build/definitions/a7934fdd-dcde-4492-a406-7fad6ac00e17/<BuildNumber>/badge)

## Overview

This sample demonstrates a ASP.NET Core Blazor WebAssembly standalone application that authenticates users against Azure AD.

## Scenario

1. The ASP.NET Core Blazor WebAssembly standalone app uses the [Microsoft Authentication Library (MSAL.js)](https://github.com/AzureAD/microsoft-authentication-library-for-js) to obtain an [ID Token](https://docs.microsoft.com/azure/active-directory/develop/id-tokens) from **Azure AD**:
1. The **ID Token** proves that the user has successfully authenticated against **Azure AD**.

![Overview](./ReadmeFiles/sign-in.png)

Application uses **Implicit flow** grant type provided by Microsoft identity platform.

![Overview](./ReadmeFiles/spa-app.svg)

## Prerequisites

- Either [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download) and [.NET Core SDK](https://www.microsoft.com/net/learn/get-started)
- System should have .Net SDK v3.1.6 or above. You can install it from [Download .NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- An **Azure AD** tenant. For more information see: [How to get an Azure AD tenant](https://azure.microsoft.com/documentation/articles/active-directory-howto-tenant/)
- A user account in your **Azure AD**. This sample will not work with a **personal Microsoft account**. Therefore, if you signed in to the [Azure portal](https://portal.azure.com) with a personal account and have never created a user account in your directory before, you need to do that now.

## Setup

### Clone or download this repository

From your shell or command line:

```console
    git clone https://github.com/Azure-Samples/ms-identity-blazor-wasm.git
```

or download and extract the repository .zip file.

> :warning: Given that the name of the sample is quite long, and so are the names of the referenced packages, you might want to clone it in a folder close to the root of your hard drive, to avoid maximum file path length limitations on Windows.

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
   - In the **Redirect URI (optional)** section, select **Web** in the combo-box and enter the following redirect URI: `https://localhost:44314/`.
     > Note that there are more than one redirect URIs used in this sample. You'll need to add them from the **Authentication** tab later after the app has been created successfully.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**. You use this value in your app's configuration file(s) later in your code.
1. In the app's registration screen, select **Authentication** in the menu.
   - If you don't have a platform added, select **Add a platform** and select the **Web** option.
   - In the **Redirect URIs** section, enter the following redirect URIs.
      - `https://localhost:44314/authentication/login-callback`
   - In the **Logout URL** section, set it to `https://localhost:44314/signout-oidc`.
   - In **Implicit grant** section, select the check box for ID tokens.
1. Select **Save** to save your changes.

#### Configure the web app (WebApp-blazor-wasm) to use your app registration

Open the project in your IDE (like Visual Studio or Visual Studio Code) to configure the code.

> In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the `blazorwasm-singleOrg\wwwroot\appsettings.json` file.
1. Find the key `ClientId` and replace the existing value with the application ID (clientId) of the `WebApp-blazor-wasm` application copied from the Azure portal.
1. Find the key `Authority` and concatenate the tenant id as shown here: 'https://login.microsoftonline.com/'+[enter_your_tenantId].
1. Find the key `ValidateAuthority` and replace the existing value with 'true'.

## Running the sample

You can run the sample by using either Visual Studio or command line interface as shown below:

### Run the sample using Visual Studio

Clean the solution, rebuild the solution, and run it.

### Run the sample using a command line interface such as VS Code integrated terminal

#### Step 1. Install .NET Core dependencies

```console
   cd blazorwasm-singleOrg
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

1. Open your browser and navigate to `https://localhost:44314`.
1. Click the **Log in** button on the top right corner. You will see claims from the signed-in user's token.

> :information_source: Did the sample not work for you as expected? Then please reach out to us using the [GitHub Issues](../../../../issues) page.

> [Consider taking a moment to share your experience with us.](https://forms.office.com/Pages/ResponsePage.aspx?id=v4j5cvGGr0GRqy180BHbR73pcsbpbxNJuZCMKN0lURpUMEw0UFNBVVBEV1E3VFNBU1I0T05TNzhPViQlQCN0PWcu)

## About the code

1. In Program.cs, Main method registers AddMsalAuthentication as below:

   ```csharp
      builder.Services.AddMsalAuthentication(options =>
      {
         builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
      });
   ```

   AddMsalAuthentication is an extension method provided by the [Microsoft.Authentication.WebAssembly.Msal](https://www.nuget.org/packages/Microsoft.Authentication.WebAssembly.Msal) package and it provides support for authenticating users.

1. Index.razor is the landing page when application starts. Index.razor contains child component called `UserClaims`. If user is authenticated, `UserClaims` displays claims present in ID Token.

   The AuthorizeView component selectively displays UI depending on whether the user is authorized to see it.

   ```csharp
   @inherits UserClaimsBase
   <AuthorizeView>
      <Authorized>
         <h3>Claims from the signed-in user's token</h3>
         @if (_claims.Count() > 0)
         {
               <table class="table">
                  <thead>
                     <tr>
                           <th>Claim Type</th>
                           <th>Value</th>
                     </tr>
                  </thead>
                  <tbody>
                     @foreach (var claim in _claims)
                     {
                           <tr>
                              <td>@claim.Type</td>
                              <td>@claim.Value</td>
                           </tr>
                     }
                  </tbody>
               </table>
         }
         else
         {
               <h3>_authMessage</h3>
         }
      </Authorized>
   </AuthorizeView>
   ```

1. In the `UserClaimsBase.cs` class, **GetClaimsPrincipalData** method retrieves signed-in user's claims using the **GetAuthenticationStateAsync()** method of the **AuthenticationStateProvider** class.

   ```csharp
    public class UserClaimsBase: ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set;
        }
        protected string _authMessage;
        protected IEnumerable<Claim> _claims = Enumerable.Empty<Claim>();

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
                _claims = user.Claims;
            }
            else
            {
                _authMessage = "The user is NOT authenticated.";
            }
        }
    }
   ```

## Next chapter of the tutorial: the Web APP calls Microsoft Graph

In the next chapter, we will enhance this Web APP to call downstream Web API (Microsoft Graph).

See [Call-MSGraph](../../WebApp-graph-user/Call-MSGraph/README-Incremental.md)

## More information

- [Microsoft identity platform (Azure Active Directory for developers)](https://docs.microsoft.com/azure/active-directory/develop/)
- [Overview of Microsoft Authentication Library (MSAL)](https://docs.microsoft.com/azure/active-directory/develop/msal-overview)
- [Quickstart: Register an application with the Microsoft identity platform (Preview)](https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app)
- [Understanding Azure AD application consent experiences](https://docs.microsoft.com/azure/active-directory/develop/application-consent-experience)
- [Application and service principal objects in Azure Active Directory](https://docs.microsoft.com/azure/active-directory/develop/app-objects-and-service-principals)
- [MSAL code samples](https://docs.microsoft.com/azure/active-directory/develop/sample-v2-code)

For more information about how OAuth 2.0 protocols work in this scenario and other scenarios, see [Authentication Scenarios for Azure AD](https://docs.microsoft.com/azure/active-directory/develop/authentication-flows-app-scenarios).

## Community Help and Support

Use [Stack Overflow](http://stackoverflow.com/questions/tagged/msal) to get support from the community.
Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before.
Make sure that your questions or comments are tagged with [`azure-active-directory` `azure-ad-b2c` `ms-identity` `adal` `msal`].

If you find a bug in the sample, please raise the issue on [GitHub Issues](../../issues).

To provide a recommendation, visit the following [User Voice page](https://feedback.azure.com/forums/169401-azure-active-directory).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](../../CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.