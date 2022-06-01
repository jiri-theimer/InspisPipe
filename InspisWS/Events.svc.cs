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
    public class Events : BaseSvc, IEvents
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

        public a11EventForm GetEventForm(string HashIsKey, int a11ID)
        {
            return null;
        }
        public Result SaveCommentOnly(string HashIsKey, int a01ID, string comment)
        {
            return null;
        }
        public IEnumerable<a01Event> GetList(string HashIsKey, int b02ID, int a10ID, int a08ID, int a03ID, int j70ID, string a03REDIZO)
        {
            return null;
        }
        public Result Create(int a10ID, int a08ID, int[] f06IDs)
        {
            return null;
        }
        public Result CreateInline(string HashIsKey, int a10ID, int a08ID, string REDIZO, string IZO, int[] f06IDs)
        {
            return null;
        }
        public a01Event GetBySignature(string HashIsKey, string signature)
        {
            return null;
        }
        public a01Event Get(string HashIsKey, int a01ID)
        {
            return null;
        }
        public IEnumerable<b06WorkflowStep> GetAvailableWorkflowSteps(int b02ID)
        {
            return null;
        }
        public Result RunWorkflowStep(string HashIsKey, int a01ID, int b06ID, string comment)
        {
            return null;
        }
    }
}
