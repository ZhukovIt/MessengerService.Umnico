using MessagesWebSender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MessengerService.Umnico.Options
{
    public partial class FormOptions : Form
    {
        private bool? m_AfterLoadNeedClick_btnOK;
        private ChatMessagesWebSender m_ChatMessagesWebSender;
        //------------------------------------------------------------------------------------------------
        public string URLDomain
        {
            get
            {
                return tBoxURLDomain.Text;
            }

            set
            {
                tBoxURLDomain.Text = value;
            }
        }
        //------------------------------------------------------------------------------------------------
        public string ClinicGuid
        {
            get
            {
                return tBoxClinicGuid.Text;
            }
            set
            {
                tBoxClinicGuid.Text = value;
            }
        }
        //------------------------------------------------------------------------------------------------
        public int URLPort
        {
            get
            {
                int tmp_i;

                if (!int.TryParse(tBoxURLPort.Text, out tmp_i))
                    return -1;

                return tmp_i;
            }
            set
            {
                tBoxURLPort.Text = value.ToString();
            }
        }
        //------------------------------------------------------------------------------------------------
        public FormOptions(MessengerServiceOptionsDomainModel _Options)
        {
            InitializeComponent();

            URLDomain = _Options.Domain;
            ClinicGuid = _Options.Guid;
            URLPort = _Options.Port;
        }
        //------------------------------------------------------------------------------------------------
        public FormOptions(MessengerServiceOptionsDomainModel _Options, bool _AfterLoadNeedClick_btnOK) : this(_Options)
        {
            m_AfterLoadNeedClick_btnOK = _AfterLoadNeedClick_btnOK;
        }
        //------------------------------------------------------------------------------------------------
        public FormOptions SetChatMessagesWebSender(ChatMessagesWebSender _ChatMessagesWebSender)
        {
            m_ChatMessagesWebSender = _ChatMessagesWebSender;
            return this;
        }
        //------------------------------------------------------------------------------------------------
        public void FormOptions_Load(object sender, EventArgs e)
        {
            if (m_AfterLoadNeedClick_btnOK.HasValue)
            {
                if (m_AfterLoadNeedClick_btnOK.Value)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
        }
        //------------------------------------------------------------------------------------------------
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!FormDataIsValidation())
                return;

            DialogResult = DialogResult.OK;
        }
        //------------------------------------------------------------------------------------------------
        private bool FormDataIsValidation()
        {
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(URLDomain))
            {
                errorProvider.SetError(tBoxURLDomain, "Необходимо ввести домен для URL-адреса сервера");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ClinicGuid))
            {
                errorProvider.SetError(tBoxClinicGuid, "Необходимо ввести Guid доступа к серверу");
                return false;
            }
            if (URLPort <= 0 || URLPort > 65535)
            {
                errorProvider.SetError(tBoxURLPort, "Значение порта для URL-адреса сервера должно быть в диапозоне от 1 до 65535");
                return false;
            }

            return true;
        }
        //------------------------------------------------------------------------------------------------
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        //------------------------------------------------------------------------------------------------
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            if (m_ChatMessagesWebSender == null)
                throw new ArgumentNullException("Для проверки соединения необходим объект ChatMessagesWebSender!");

            if (!FormDataIsValidation())
                return;

            ResetOptionsForChatMessagesWebSender();

            PersonsResponseData _Response = m_ChatMessagesWebSender.CheckConnection();

            MessageBox.Show(_Response.InfoMessage);
        }
        //------------------------------------------------------------------------------------------------
        private void btnVerifyGuid_Click(object sender, EventArgs e)
        {
            if (m_ChatMessagesWebSender == null)
                throw new ArgumentNullException("Для проверки Guid необходим объект ChatMessagesWebSender!");

            if (!FormDataIsValidation())
                return;

            ResetOptionsForChatMessagesWebSender();

            PersonsResponseData _Response = m_ChatMessagesWebSender.VerifyGuid();

            if (_Response.Success)
            {
                MessageBox.Show("Guid корректный!");
            }
            else
            {
                MessageBox.Show(_Response.InfoMessage);
            }
        }
        //------------------------------------------------------------------------------------------------
        private void ResetOptionsForChatMessagesWebSender()
        {
            MessagesWebSenderOptions _Options = new MessagesWebSenderOptions()
            {
                Domain = URLDomain,
                Port = URLPort,
                Guid = ClinicGuid
            };

            m_ChatMessagesWebSender.InitOptions(_Options);
        }
        //------------------------------------------------------------------------------------------------
    }
}
