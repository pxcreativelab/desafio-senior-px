using System.Configuration;

namespace HubieTest.Dal.Tools
{
    /// <summary>
    /// Configuration helper. In the real Hubie this utility reads parameters
    /// from a configuration table in the database; here, to keep the test
    /// simple, we read from &lt;appSettings&gt; in Web.config / App.config.
    /// </summary>
    public static class ConfigReader
    {
        public static string GetByKey(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
