using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace FirewallManager
{
    static internal class PlatformUtils
    {
        internal static void Validate()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            {
                throw new ApplicationException("FirewallManager is only supported on Windows");
            }

#pragma warning disable CA1416 // Validate platform compatibility
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();

            WindowsPrincipal principal = new WindowsPrincipal(identity);

            if (principal.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                throw new UnauthorizedAccessException("Unable to proceed - administrator privileges required to modify firewall rules");
            }
#pragma warning restore CA1416 // Validate platform compatibility
        }

    
    }
}
