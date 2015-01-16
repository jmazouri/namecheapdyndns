using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace NamecheapDynDnsUpdater
{
    public class NetworkHelper
    {
        public static List<IPInfo> GetAddresses()
        {
            List<IPInfo> infolist = new List<IPInfo>();

            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkint in interfaces)
            {
                string ipaddress = "0.0.0.0";

                if (networkint.GetIPProperties().UnicastAddresses.Any())
                {
                    var foundaddresses = networkint.GetIPProperties().UnicastAddresses
                        .Where(d => d.Address.AddressFamily == 
                            (Config.Options.ShowIPv6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork) );
                    if (foundaddresses.Any())
                    {
                        ipaddress = foundaddresses.First().Address.ToString();
                    }
                        
                }

                if (networkint.OperationalStatus == OperationalStatus.Up || Config.Options.ShowDownedInterfaces)
                {
                    infolist.Add(new IPInfo()
                    {
                        AdapterName = networkint.Name,
                        IPAddress = ipaddress,
                        Status = networkint.OperationalStatus,
                        AdapterId = networkint.Id
                    });
                }
                    
                
            }

            return infolist;
        }
    }
}
