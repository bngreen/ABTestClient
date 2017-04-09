using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABTestClient
{
    public class ActivationResponse
    {
        public string Variant { get; private set; }
        public UserState UserState { get; private set; }
        public ActivationResponse(string variant, UserState userState)
        {
            Variant = variant;
            UserState = userState;
        }
    }
}
