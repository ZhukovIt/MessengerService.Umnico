using MessengerService.Umnico.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerServiceUmnicoTests.Tests
{
    public static class MotherObject
    {
        public static string CreateXMLSerializedString(MessengerServiceOptionsDomainModel _OptionsDomainModel)
        {
            MessengerServiceOptionsXMLParser _Parser = new MessengerServiceOptionsXMLParser(_OptionsDomainModel);

            return _Parser.Pack();
        }
        //------------------------------------------------------------------------------------------------
        public static string CreateUmnicoXMLSerializedString()
        {
            MessengerServiceOptionsDomainModel _OptionsDomainModel = CreateUmnicoOptionsDomainModel();

            return CreateXMLSerializedString(_OptionsDomainModel);
        }
        //------------------------------------------------------------------------------------------------
        public static MessengerServiceOptionsDomainModel CreateOptionsDomainModel(string _Domain, string _Guid, int _Port)
        {
            MessengerServiceOptionsDomainModel _OptionsDomainModel = new MessengerServiceOptionsDomainModel();

            _OptionsDomainModel.Domain = _Domain;
            _OptionsDomainModel.Guid = _Guid;
            _OptionsDomainModel.Port = _Port;

            return _OptionsDomainModel;
        }
        //------------------------------------------------------------------------------------------------
        public static MessengerServiceOptionsDomainModel CreateUmnicoOptionsDomainModel()
        {
            string _Domain = "192.168.0.19";
            int _Port = 12341;
            string _Guid = "7a93e9a3-ce8b-4b4c-937f-239e33c50b2d";

            return CreateOptionsDomainModel(_Domain, _Guid, _Port);
        }
        //------------------------------------------------------------------------------------------------
    }
}
