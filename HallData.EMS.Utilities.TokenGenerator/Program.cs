using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HallData.EMS.Security.Tokenizer;
using System.Configuration;

namespace HallData.EMS.Utilities.TokenGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("User Name required");
                return;
            }
            string userName = args[0];
            Guid organizationId = new Guid(ConfigurationManager.AppSettings["DefaultOrganizationGuid"]);
            if(args.Length > 1)
            {
                if(!Guid.TryParse(args[1], out organizationId))
                {
                    Console.WriteLine("Organization Id supplied was in wrong format, must be a guid");
                    return;
                }
            }
            EmsTokenizer tokenizer = new EmsTokenizer();
            Console.WriteLine(tokenizer.TokenizeUserNameOrganizationId(userName, organizationId));
            
        }
    }
}
