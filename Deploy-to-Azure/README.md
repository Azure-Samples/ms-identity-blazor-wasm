# Deploy your app to Azure

## Deployment to Azure App Services

There is one web project in this sample. To deploy it to **Azure App Services**, you'll need to:

- create an **Azure App Service**
- publish the projects to the **App Services**, and
- update its client(s) to call the web site instead of the local environment.

### Create Azure App Service and Publish the Project using Visual Studio

Follow the link to [Create Azure App Service and Publish Project with Visual Studio](https://docs.microsoft.com/visualstudio/deployment/quickstart-deploy-to-azure?view=vs-2019).

### Create Azure App Service and Publish the Project using Visual Studio Code

#### Create `WebApp-blazor-wasm` in Azure App Services

1. Sign in to the [Azure portal](https://portal.azure.com).
1. Select `Create a resource` in the top left-hand corner, select **Web** --> **Web App**, and give your web site a name, for example, `WebApp-blazor-wasm.azurewebsites.net`.
1. Next, select the `Subscription`, `Resource Group`, `App service plan and Location`. `OS` will be **Windows** and `Publish` will be **Code**.
1. Select `Create` and wait for the App Service to be created.
1. Once you get the `Deployment succeeded` notification, then select on `Go to resource` to navigate to the newly created App service.

#### Publish the project

1. Install the VS Code extension [Azure App Service](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice).
1. Sign-in to App Service using Azure AD Account.
1. Open the WebApp-blazor-wasm project folder.
1. Choose View > Terminal from the main menu.
1. The terminal opens in the WebApp-blazor-wasm folder.
1. Run the following command:

    ```console
    dotnet publish --configuration Release
    ```

1. Publish folder is created under path ``bin/Release/<Enter_Framework_FolderName>``.
1. Right Click on **Publish** folder and select **Deploy to Web App**.
1. Select **Create New Web App**, enter unique name for the app.
1. Select Windows as the OS. Press Enter.

### Update Azure App Services Configuration

1. Go to [Azure portal](https://portal.azure.com).
    - On the Settings tab, select Authentication / Authorization. Make sure `App Service Authentication` is Off. Select **Save**.
1. Browse your website. If you see the default web page of the project, the publication was successful.

### Update the Azure AD app registration for `WebApp-blazor-wasm`

1. Navigate back to to the [Azure portal](https://portal.azure.com).
In the left-hand navigation pane, select the **Azure Active Directory** service, and then select **App registrations (Preview)**.
1. In the resulting screen, select the `WebApp-blazor-wasm` application.
1. In the app's registration screen, select **Authentication** in the menu. 
   - Update the **Logout URL** section with the address of your service, for example [https://WebApp-blazor-wasm.azurewebsites.net/signout-oidc](https://WebApp-blazor-wasm.azurewebsites.net/signout-oidc)
   - In **Redirect URI** section, add the URL [https://WebApp-blazor-wasm.azurewebsites.net/authentication/login-callback](https://WebApp-blazor-wasm.azurewebsites.net/authentication/login-callback). If you have multiple redirect URIs, make sure that there is a new entry using the App service's URI for each redirect URI.
1. From the *Branding* menu, update the **Home page URL**, to the address of your service, for example [https://WebApp-blazor-wasm.azurewebsites.net](https://WebApp-blazor-wasm.azurewebsites.net). Save the configuration.

> :warning: If your app is using an *in-memory* storage, **Azure App Services** will spin down your web site if it is inactive, and any records that your app was keeping will emptied. In addition, if you increase the instance count of your web site, requests will be distributed among the instances. Your app's records, therefore, will not be the same on each instance.

## More information

- [App Service overview](https://docs.microsoft.com/azure/app-service/overview)
- [Deploy ASP.NET Core apps to Azure App Service](https://docs.microsoft.com/aspnet/core/host-and-deploy/azure-apps)

## Community Help and Support

Use [Stack Overflow](http://stackoverflow.com/questions/tagged/msal) to get support from the community.
Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before.
Make sure that your questions or comments are tagged with [`azure-active-directory`] [`msal`] [`dotnet`].

If you find a bug in the sample, raise the issue on [GitHub Issues](../../../../issues).

To provide feedback on or suggest features for Azure Active Directory, visit [User Voice page](https://feedback.azure.com/forums/169401-azure-active-directory).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.