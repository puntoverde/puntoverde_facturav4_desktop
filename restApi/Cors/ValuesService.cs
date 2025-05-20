using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;

namespace restApi
{
    public class ValuesService : IValues
    {
        public string GetValue(string id)
        {
            return string.Format("You have entered - {0}", id);
        }
        
        public string AddValue(string value)
        {
            return string.Format("POST response - {0}", value);
        }
    }
}
