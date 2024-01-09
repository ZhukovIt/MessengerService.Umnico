using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService.Umnico.Options
{
    [Serializable]
    public sealed class MessengerServiceOptionsDomainModel
    {
        private string m_Domain;
        private int m_Port;
        private string m_Guid;
        [NonSerialized]
        private bool m_IsInit;
        //----------------------------------------------------
        public string Domain
        {
            get
            {
                return m_Domain;
            }

            set
            {
                m_Domain = value;
                TryInit();
            }
        }
        //----------------------------------------------------
        public string Guid
        {
            get
            {
                return m_Guid;
            }

            set
            {
                m_Guid = value;
                TryInit();
            }
        }
        //----------------------------------------------------
        public int Port
        {
            get
            {
                return m_Port;
            }

            set
            {
                m_Port = value;
                TryInit();
            }
        }
        //----------------------------------------------------
        public bool IsInit
        {
            get
            {
                return m_IsInit;
            }
        }
        //----------------------------------------------------
        public MessengerServiceOptionsDomainModel()
        {
            TryInit();
        }
        //----------------------------------------------------
        private void TryInit()
        {
            bool _IsInit = true;

            if (string.IsNullOrWhiteSpace(m_Domain))
            {
                _IsInit = false;
            }
            if (string.IsNullOrWhiteSpace(m_Guid))
            {
                _IsInit = false;
            }
            if (m_Port <= 0 || m_Port > 65535)
            {
                _IsInit = false;
            }

            m_IsInit = _IsInit;
        }
        //----------------------------------------------------
    }
}
