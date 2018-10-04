# infusionsoft.netstandard
Implementation for the Infusionsoft REST Api on .NET Standard.

It includes IntegrationTests which also serve as examples for you. They can be run if you provide Authorization Code, Access Token and/or Refresh Token.

It will be available through NuGet anytime soon!

Examples

Infusionsoft.TagExample
.NET Core and Angular 6 WebApp that allows to generate a token (using an auth code from Infusionsoft's login page), to read all existing tags, and to tag a contact by email address and tag id. If the contact doesn't exist, it's created. You can try the app here:
https://infusionsofttagexample.azurewebsites.net/

Infusionsoft.OAuthExample
In work. Will show the authorization with the real OAuth flow popup window showing the Infusionsoft authorization page. From there, the WebApp generates the Token automatically. 

The functionality is growing as it is migrated from another project step-by-step, but it won't include YET a complete coverage of Infusionsoft's API. 
Functions and examples are also documented here:
https://www.ciclosoftware.com/2018/07/30/infusionsoft-netstandard/
https://www.ciclosoftware.com/2018/10/04/tag-infusionsoft-contacts-netcore/

Please let me know if you need anything that is still missing, and I can prioritize it! 

The implementation comes from a project that has ended, and that I can proudly publish as Open Source. May it help you to successfully implement your Infusionsoft integrations with .NET (or even Angular as well)! 

HAPPY PLANTING
