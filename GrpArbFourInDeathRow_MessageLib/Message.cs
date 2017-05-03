using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpArbFourInDeathRow_MessageLib
{
    public class Message
    {
        public int Version { get; set; }

        public string Text { get; set; }
        public string MessageType { get; set; }
        public string PlayerName { get; set; }


        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }



            

    }
}
