using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using InspisWS;
using Newtonsoft.Json;

namespace InspisPipe.InspisWS
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class Events : BaseSvc
    {
        public string Ping()
        {            
            return this.GetApiPlainResult("Misc/Ping");

           
        }

        public string LoadLinkerSignature(string HashIsKey, int a01ID)
        {            
            return this.GetApiPlainResult($"Events/LoadLinkerSignature?HashIsKey={HashIsKey}&a01id={a01ID}");

            
        }
        public IEnumerable<a11EventForm> GetListEventForm(string HashIsKey, int f06ID, int a01ID, int a03ID)
        {            
            string s = this.GetApiPlainResult($"Events/GetListEventForm?HashIsKey={HashIsKey}&f06ID={f06ID}&a01ID={a01ID}&a03ID={a03ID}");
            return JsonConvert.DeserializeObject<IEnumerable<a11EventForm>>(s);

        }

        public IEnumerable<a10EventType> a10EventType()
        {            
            string s = this.GetApiPlainResult($"Lists/a10EventType");
            return JsonConvert.DeserializeObject<IEnumerable<a10EventType>>(s);

        }
    }
}
