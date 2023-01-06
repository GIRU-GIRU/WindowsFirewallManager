using NetFwTypeLib;

namespace FirewallManager
{
    static internal class FirewallRuleManager
    {
        internal static void NukeGeneratedRules()
        {
            INetFwPolicy2 firewallPolicy = WindowsFirewallHelper.CreatePolicy();

            firewallPolicy.RemoveExistingGeneratedRules();
        }
        internal static void BlockConfiguredApplications(Config config)
        {
            INetFwPolicy2 firewallPolicy = WindowsFirewallHelper.CreatePolicy();

            if (firewallPolicy == null)
            {
                throw new SystemException("Unable to communicate with the Windows Firewall Api");
            }

            foreach (string applicationPath in config.ApplicationPaths)
            {
                firewallPolicy.CheckToRemoveExistingRules(applicationPath);

                firewallPolicy.CreateBlockingRules(applicationPath);
            }
        }

        internal static void UnblockConfiguredApplications(Config config)
        {
            INetFwPolicy2 firewallPolicy = WindowsFirewallHelper.CreatePolicy();

            if (firewallPolicy == null)
            {
                throw new SystemException("Unable to communicate with the Windows Firewall Api");
            }

            firewallPolicy.RemoveExistingGeneratedRules();

       
            foreach (string applicationPath in config.ApplicationPaths)
            {
                firewallPolicy.CheckToRemoveExistingRules(applicationPath);
                firewallPolicy.CreateAllowingRules(applicationPath);
            }
        }
    }
}
