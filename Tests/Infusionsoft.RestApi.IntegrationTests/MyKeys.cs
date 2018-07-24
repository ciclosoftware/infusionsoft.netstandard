namespace Infusionsoft.RestApi.IntegrationTests
{
    public static class MyKeys
    {
        //after adding your keys, you should run this command to not commit your secrets
        //git update-index --assume-unchanged Tests/Infusionsoft.RestApi.IntegrationTests/MyKeys.cs 
        public static string InfusionsoftClientId => "get it from developer.infusionsoft.com";
        public static string InfusionsoftClientSecret => "get it from developer.infusionsoft.com";
    }
}
