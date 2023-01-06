using System.Text;

namespace FirewallManager
{
    internal class ArgsProcessor
    {
        readonly string[] _commandLineArgs;
        internal ArgsProcessor(string[] commandLineArgs)
        {
            _commandLineArgs = commandLineArgs;
        }

        internal void HandleInput()
        {
            if (_commandLineArgs.Count() == 1)
            {
                Help();
                Exit();
            }

            string firstArg = _commandLineArgs[1].ToLower();
            var cfg = Config.GetConfig();
            string? path;

            switch (firstArg)
            {
                case "help":
                    Help();
                    break;

                case "nuke":
                    Console.WriteLine("Nuking generated rules and config- you may need to manually reconfigure allow rules on applications");
                    FirewallRuleManager.NukeGeneratedRules();
                    Config.SaveConfig(new Config());
                    break;

                case "unblock":
                    if (cfg.ApplicationPaths.Count > 0)
                    {
                        Console.WriteLine("Unblocking configured applications");
                        FirewallRuleManager.UnblockConfiguredApplications(cfg);
                    }
                    else
                    {
                        Console.WriteLine("No applications found to create unblocking rules for - add them using \"fwm add {path}\"");
                    }

                    break;

                case "block":
                    if (cfg.ApplicationPaths.Count > 0)
                    {
                        FirewallRuleManager.BlockConfiguredApplications(cfg);
                    }
                    else
                    {
                        Console.WriteLine("No applications found to create blocking rules for - add them using \"fwm add {path}\"");
                    }

                    break;

                case "list":
                    if (cfg.ApplicationPaths.Count > 0)
                    {
                        Console.WriteLine("No application rules currently setup");
                    }
                    else
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("Found rules for;");
                        foreach (var storedPath in cfg.ApplicationPaths)
                        {
                            builder.AppendLine(storedPath);
                        }

                        Console.WriteLine(builder.ToString());
                    }

                    break;

                case "remove":
                    path = _commandLineArgs.ElementAtOrDefault(2);

                    if (!string.IsNullOrEmpty(path))
                    {
                        if (cfg.ApplicationPaths.Contains(path))
                        {
                            Console.WriteLine($"Removed rule for {Path.GetFileName(path)}");
                            cfg.ApplicationPaths.Remove(path);
                        }
                        else
                        {
                            Console.WriteLine("Invalid rule to remove specified");
                        }

                        Config.SaveConfig(cfg);
                    }
                    else
                    {
                        Exit();
                    }
                    break;


                case "add":
                    path = _commandLineArgs.ElementAtOrDefault(2);

                    if (!File.Exists(path))
                    {
                        Console.WriteLine("Invalid path");
                        Exit();
                    }
                    else
                    {
                        if (cfg.ApplicationPaths.Contains(path.ToLower()))
                        {
                            Console.WriteLine($"A entry for {path} exists already");
                        }
                        else
                        {
                            Console.WriteLine($"Added {path}");
                            cfg.ApplicationPaths.Add(path.ToLower());
                            Config.SaveConfig(cfg);
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Invalid command, try wfm help to see a list of commands");
                    Exit();
                    break;
            }
        }


        private void Help()
        {
            StringBuilder builder = new StringBuilder();

            void A(string input) => builder.AppendLine(input);

            A("list - dumps all existing configured paths");
            A("block - creates a firewall block rule for all valid paths that were configured with the add command");
            A("unblock - creates allow rules for all valid configured with the add command, also detects any generated firewall rules and removes them");
            A("add {path} - creates a new entry for programs to create firewall rules for");
            A("remove {path} - removes an existing path from the configuration if it exists and deletes all existing configuration");
            A("nuke - detects and removes all generated firewall rules");

            Console.WriteLine(builder.ToString());
        }

        private void Exit() => Environment.Exit(0);
    }
}
