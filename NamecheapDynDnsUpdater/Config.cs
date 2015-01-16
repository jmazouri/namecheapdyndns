using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace NamecheapDynDnsUpdater
{
    public class Config
    {
        public string NetworkAdapterId { get; set; }
        public string Hostname { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }

        public bool ShowDownedInterfaces { get; set; }
        public bool ShowIPv6 { get; set; }
        public bool ShowAdapterIds { get; set; }

        private static Config _configCache = null;

        public static Config Options
        {
            get
            {
                if (_configCache == null)
                {
                    try
                    {
                        string filedata = File.ReadAllText("Config.json");
                        Options = JsonConvert.DeserializeObject<Config>(filedata);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Could not read config file Config.json.");
                    }
                }

                return _configCache;
            }
            set
            {
                _configCache = value;
            }
        }

        public static void SaveConfig()
        {
            string data = JsonConvert.SerializeObject(Options, Formatting.Indented);

            try
            {
                File.WriteAllText("Config.json", data);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not save to \"Config.json\". Does it exist?");
            }
        }
    }
}
