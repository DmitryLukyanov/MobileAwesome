using System;

namespace MobileAwesomeApp.Infrastructure
{
    public interface ISettingsProvider
    {
        string GetByKey(string key);
    }

    public class HardcodedSettingsProvider : ISettingsProvider
    {
        public string GetByKey(string key)
        {
            switch (key)
            {
                case "ApiKey": return "dx9F7DuYQ9AVN0EUIQTmvKyqUV2u45wpuBmJUhLzXjZkrK3lJsY65Z14E5nrbRf5";
                case "MongoRealmConnectionStringAuthArguments": return "authMechanism=PLAIN&authSource=%24external&ssl=true&appName=mobileawesomerealmapp-jiust:mongodb-atlas:api-key";
                default:
                    throw new ArgumentException($"Unspecified key {key}.");
            }
        }
    }
}