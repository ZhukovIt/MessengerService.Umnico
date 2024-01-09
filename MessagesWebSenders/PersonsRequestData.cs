using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesWebSender
{
    [Serializable]
    public sealed class PersonsRequestData
    {
        public IEnumerable<PersonInfo> PersonsInfo { get; set; }

        public string Image { get; set; }

        public string Guid { get; set; }
    }
}
