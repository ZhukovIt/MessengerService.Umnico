using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesWebSender
{
    [Serializable]
    public class PersonsResponseData
    {
        public bool Success { get; set; }

        public string InfoMessage { get; set; }

        public Dictionary<string, string> MesUmnGuids { get; set; }
    }
}
