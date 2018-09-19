using Microsoft.TeamFoundation.Framework.Server;
using System.Configuration;
using SysConfig = System.Configuration.Configuration;

namespace TfsJira.Plugin.Configuration
{
    public class Config
    {
        public static SysConfig config = null; 
        public static void MapAndLoadCustomConfig(IVssRequestContext vssRequestContext)
        {
            if (config == null)
            {
                var basePath = vssRequestContext != null ? vssRequestContext.RootContext.ServiceHost.PlugInDirectory + "\\TfsJira\\" : "";
                var configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = basePath + "TfsJira.Plugin.config"
                };
                config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            }
        }

        public static string JiraApiHostUrl => config.AppSettings.Settings["JiraApiHostUrl"].Value;

        public class Authorization
        {
            public static string Header => config.AppSettings.Settings["Authorization.Header"].Value;
        }

        public static string RemoteLinkTitle => config.AppSettings.Settings["RemoteLinkTitle"].Value;

        public static string RemoteLinkIconUrl => config.AppSettings.Settings["RemoteLinkIconUrl"].Value;

        public static string TfsChangesetBaseUrl => config.AppSettings.Settings["TfsChangesetBaseUrl"].Value;

        public static string AllowedJiraProjectsKey => config.AppSettings.Settings["AllowedJiraProjectsKey"].Value;

        public static char AllowedJiraProjectsKeySeparator => char.Parse(config.AppSettings.Settings["AllowedJiraProjectsKeySeparator"].Value);

    }
}
