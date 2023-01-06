using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FirewallManager
{
    public class Config
    {
        static string GetConfigPath() => Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "Config.json");

        public List<string> ApplicationPaths = new List<string>();

        internal static Config GetConfig()
        {
            string cfgPath = GetConfigPath();

            if (File.Exists(cfgPath))
            { 
                var file = File.ReadAllText(cfgPath);

                if (!string.IsNullOrEmpty(file))
                {
                    var result = JsonConvert.DeserializeObject<Config>(file);

                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Configuration file corrupted !");
                    }
                }
            }

            return new Config();
        }


        internal static void SaveConfig(Config cfg)
        {
            using (StreamWriter file = File.CreateText(GetConfigPath()))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, cfg);
            }
        }
    }

}
