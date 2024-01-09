using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessagesWebSender
{
    public class UmnicoWebServerWorker : AbstractWebServerWorker
    {
        public override PersonsResponseData DefaultRoute(Uri _Uri)
        {
            PersonsResponseData _ResultResponse = new PersonsResponseData();

            bool _MutexRes = m_Mutex.WaitOne(m_MutexWaitMS);

            if (!_MutexRes)
            {
                _ResultResponse.Success = false;
                _ResultResponse.InfoMessage = "Не удалось получить доступ к объекту для отправки сообщений!";
                return _ResultResponse;
            }

            try
            {
                HttpWebRequest _Request = (HttpWebRequest)WebRequest.Create(_Uri);
                _Request.Method = "GET";

                using (HttpWebResponse _Response = (HttpWebResponse)_Request.GetResponse())
                {
                    _ResultResponse.Success = _Response.StatusCode == HttpStatusCode.OK;

                    using (StreamReader _Reader = new StreamReader(_Response.GetResponseStream()))
                    {
                        _ResultResponse.InfoMessage = _Reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                _ResultResponse.Success = false;
                _ResultResponse.InfoMessage = GetMessageByException(ex);
            }
            finally
            {
                if (_MutexRes)
                {
                    m_Mutex.ReleaseMutex();
                }
            }

            return _ResultResponse;
        }
        //-------------------------------------------------------------------------
        public override PersonsResponseData VerifyGuid(Uri _Uri, string _Guid)
        {
            PersonsResponseData _ResultResponse = new PersonsResponseData();

            bool _MutexRes = m_Mutex.WaitOne(m_MutexWaitMS);

            if (!_MutexRes)
            {
                _ResultResponse.Success = false;
                _ResultResponse.InfoMessage = "Не удалось получить доступ к объекту для отправки сообщений!";
                return _ResultResponse;
            }

            try
            {
                HttpWebRequest _Request = (HttpWebRequest)WebRequest.Create(_Uri);
                _Request.Method = "GET";
                _Request.ContentType = "application/json";
                _Request.Headers["Authorization"] = "GUID " + _Guid;

                using (HttpWebResponse _Response = (HttpWebResponse)_Request.GetResponse())
                {
                    using (StreamReader _Reader = new StreamReader(_Response.GetResponseStream()))
                    {
                        string _ResponseContent = _Reader.ReadToEnd();
                        _ResultResponse = JsonConvert.DeserializeObject<PersonsResponseData>(_ResponseContent);
                    }
                }
            }
            catch (WebException ex)
            {
                _ResultResponse.Success = false;

                if (ex.Response is HttpWebResponse _Response && _Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _ResultResponse.InfoMessage = "Ошибка авторизации - Guid неправильный!";
                }
                else
                {
                    _ResultResponse.InfoMessage = GetMessageByException(ex);
                }
            }
            finally
            {
                if (_MutexRes)
                {
                    m_Mutex.ReleaseMutex();
                }
            }

            return _ResultResponse;
        }
        //-------------------------------------------------------------------------
        public override PersonsResponseData SendMessages(Uri _Uri, string _Guid, PersonsRequestData _InputRequest)
        {
            PersonsResponseData _ResultResponse = new PersonsResponseData();

            bool _MutexRes = m_Mutex.WaitOne(m_MutexWaitMS);

            if (!_MutexRes)
            {
                _ResultResponse.Success = false;
                _ResultResponse.InfoMessage = "Не удалось получить доступ к объекту для отправки сообщений!";
                return _ResultResponse;
            }

            try
            {
                HttpWebRequest _Request = (HttpWebRequest)WebRequest.Create(_Uri);
                _Request.Method = "POST";
                _Request.ContentType = "application/json";
                _Request.Headers["Authorization"] = "GUID " + _Guid;

                string _StreamStringData = JsonConvert.SerializeObject(_InputRequest);
                byte[] _StreamData = Encoding.UTF8.GetBytes(_StreamStringData);
                _Request.ContentLength = _StreamData.Length;

                using (BinaryWriter _Writer = new BinaryWriter(_Request.GetRequestStream()))
                {
                    _Writer.Write(_StreamData);

                    using (HttpWebResponse _Response = (HttpWebResponse)_Request.GetResponse())
                    {
                        using (StreamReader _Reader = new StreamReader(_Response.GetResponseStream()))
                        {
                            string _ResponseContent = _Reader.ReadToEnd();
                            _ResultResponse = JsonConvert.DeserializeObject<PersonsResponseData>(_ResponseContent);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                _ResultResponse.Success = false;
                _ResultResponse.InfoMessage = GetMessageByException(ex);
            }
            finally
            {
                if (_MutexRes)
                {
                    m_Mutex.ReleaseMutex();
                }
            }

            return _ResultResponse;
        }
        //-------------------------------------------------------------------------
        private string GetMessageByException(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }

            return GetMessageByException(ex.InnerException);
        }
        //-------------------------------------------------------------------------
    }
}
