using ABTestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        private static string HOST = "http://localhost:9000";
        private static string EXPERIMENTNAME = "my_experiment";
        static void Main(string[] args)
        {
            new ClientExample().Run(EXPERIMENTNAME, HOST, 10000, 200);

        }

    }
}
