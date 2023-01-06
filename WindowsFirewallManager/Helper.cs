using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirewallManager
{
    internal static class Helper
    {
        internal static IEnumerable<INetFwRule> WhereApplicationName(this IEnumerable<INetFwRule>? source, string comparison)
        {
            if(source != null)
            {
                var validRules = source.Where(x => !String.IsNullOrEmpty(x.ApplicationName));

                var matchingRules = validRules.Where(x => x.ApplicationName.RemoveWhitespace().ToLower() == comparison.RemoveWhitespace().ToLower());

                return matchingRules;
            }

            return Array.Empty<INetFwRule>();
        }

        internal static string RemoveWhitespace(this string input) => input.Replace(" ", string.Empty).ToLower();


      
    }
}
