using Microsoft.TeamFoundation.Framework.Server;
using TfsJira.Plugin.Configuration;

namespace TfsJira.Plugin.Extension
{
    public static class VssRequestContextExtension
    {
        public static void MapAndLoadCustomConfig(this IVssRequestContext vssRequestContext)
        {
            Config.MapAndLoadCustomConfig(vssRequestContext);
        }
    }
}
