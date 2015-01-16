using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace NamecheapDynDnsUpdater
{
    class Program
    {
        static void Main(string[] args)
        {

            List<IPInfo> infolist = NetworkHelper.GetAddresses();

            if (args.Contains("/c"))
            {
                Console.WriteLine("Choose an Adapter (* = current): ");
                for (int i = 0; i < infolist.Count; i++)
                {
                    IPInfo ip = infolist[i];

                    string prepend = (Config.Options.NetworkAdapterId == ip.AdapterId ? "* " : "");

                    if (Config.Options.ShowAdapterIds)
                    {
                        Console.WriteLine(prepend + "({0})\n{1}. {2} - {3} ({4})\n", i, ip.AdapterId, ip.AdapterName, ip.IPAddress, ip.Status);
                    }
                    else
                    {
                        Console.WriteLine(prepend + "{0}. {1} - {2} ({3})", i, ip.AdapterName, ip.IPAddress, ip.Status);
                    }
                }

                int chosen = 0;

                Int32.TryParse(Console.ReadLine(), out chosen);

                if (chosen > infolist.Count - 1)
                {
                    Console.WriteLine("That wasn't valid. Try again.");
                    Console.Read();
                }

                IPInfo chosenadapter = infolist[chosen];

                Config.Options.NetworkAdapterId = chosenadapter.AdapterId;

                Console.WriteLine("Successfully set adapter to \"{0}\"", chosenadapter.AdapterName);

                Console.Write("What's the hostname (subdomain)? ");
                string newhostname = Console.ReadLine();
                Config.Options.Hostname = newhostname;

                Console.Write("And the domain (ex. google.com)? ");
                string newdomain = Console.ReadLine();
                Config.Options.Domain = newdomain;

                Console.Write("Okay, the password (leave empty to load clipboard)? ");
                string newpassword = Console.ReadLine();

                if (newpassword.Trim() == String.Empty)
                {
                    newpassword = ClipboardHelper.GetClipboardText();
                }

                Config.Options.Password = newpassword;

                Config.SaveConfig();

                Console.WriteLine("That's it! Press any key to exit, and set up your chosen scheduler.");

                Console.ReadLine();
            }
            else
            {
                IPInfo current = infolist.FirstOrDefault(d => d.AdapterId == Config.Options.NetworkAdapterId);

                string apiurl = String.Format("https://dynamicdns.park-your-domain.com/update?host={0}&domain={1}&password={2}&ip={3}",
                    Config.Options.Hostname, Config.Options.Domain, Config.Options.Password, current.IPAddress);

                WebClient w = new WebClient();

                try
                {
                    w.DownloadData(apiurl);
                }
                catch (Exception)
                {
                    Console.WriteLine("Could not connect to Namecheap.");
                    if (args.Contains("/d"))
                    {
                        Console.ReadLine();
                    }
                }
            }
            
        }
    }
}
