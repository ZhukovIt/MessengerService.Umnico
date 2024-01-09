using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace MessengerService.Umnico.Options
{
    public sealed class MessengerServiceOptionsXMLParser : BaseOptions
    {
        private MessengerServiceOptionsDomainModel m_DomainModel;
        //------------------------------------------------------------------------------------------------
        public MessengerServiceOptionsDomainModel DomainModel
        {
            get
            {
                return m_DomainModel;
            }
        }
        //------------------------------------------------------------------------------------------------
        public MessengerServiceOptionsXMLParser(MessengerServiceOptionsDomainModel _DomainModel)
        {
            m_DomainModel = _DomainModel;
        }
        //------------------------------------------------------------------------------------------------
        public override string Pack()
        {
            MemoryStream stream = null;
            string _XMLResult = null;

            try
            {
                stream = new MemoryStream();
                stream.Position = 0;

                XmlSerializer _XMLSerializer = new XmlSerializer(typeof(MessengerServiceOptionsDomainModel));
                _XMLSerializer.Serialize(stream, m_DomainModel);
                _XMLResult = Encoding.UTF8.GetString(stream.GetBuffer());
            }
            finally
            {
                stream?.Dispose();
            }

            return _XMLResult;
        }
        //------------------------------------------------------------------------------------------------
        public override BaseOptions Unpack(string _Source)
        {
            MemoryStream stream = null;

            try
            {
                stream = new MemoryStream(Encoding.UTF8.GetBytes(_Source));

                XmlSerializer _XMLDeserializer = new XmlSerializer(typeof(MessengerServiceOptionsDomainModel));
                m_DomainModel = (MessengerServiceOptionsDomainModel)_XMLDeserializer.Deserialize(stream);
            }
            finally
            {
                stream?.Dispose();
            }

            return this;
        }
        //------------------------------------------------------------------------------------------------
    }
}
