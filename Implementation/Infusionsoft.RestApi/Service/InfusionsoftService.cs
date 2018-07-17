using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace com.ciclosoftware.infusionsoft.restapi.Service
{
    public enum ContentType
    {
        UrlEncoded,
        Json
    }

    public interface IInfusionsoftService
    {
        Task<string> Get(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded);

        Task<string> Post(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded, string altAuthHeader = null);
        Task<string> Put(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded, string altAuthHeader = null);

        Task<string> Delete(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded);
    }

    public class InfusionsoftService : IInfusionsoftService
    {
        private readonly EventLimiter _eventLimiter;

        public InfusionsoftService()
        {
            _eventLimiter = new EventLimiter(20, new TimeSpan(0, 0, 1, 0));
        }

        public async Task<string> Get(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded)
        {
            return await Send("GET", url, data, accessToken, contentType, null);
        }

        public async Task<string> Post(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded, string altAuthHeader = null)
        {
            return await Send("POST", url, data, accessToken, contentType, altAuthHeader);
        }

        public async Task<string> Put(string url, string data = null, string accessToken = null, ContentType contentType = ContentType.UrlEncoded,
            string altAuthHeader = null)
        {
            return await Send("PUT", url, data, accessToken, contentType, altAuthHeader);
        }

        public async Task<string> Delete(string url, string data = null, string accessToken = null,
            ContentType contentType = ContentType.UrlEncoded)
        {
            return await Send("DELETE", url, data, accessToken, contentType, null);
        }

        private async Task<string> Send(string method, string url, string data, string accessToken,
            ContentType contentType, string altAuthHeader)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.KeepAlive = true;
            request.ContentType = contentType == ContentType.UrlEncoded
                ? "application/x-www-form-urlencoded"
                : "application/json";
            if (!string.IsNullOrEmpty(data))
            {
                var dataBytes = Encoding.UTF8.GetBytes(data);
                using (var reqStream = request.GetRequestStream())
                {
                    reqStream.Write(dataBytes, 0, dataBytes.Length);
                }
            }
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers[HttpRequestHeader.Authorization] = $"Bearer {accessToken}";
            }
            else if (!string.IsNullOrEmpty(altAuthHeader))
            {
                request.Headers[HttpRequestHeader.Authorization] = altAuthHeader;
            }
            try
            {
                string resultJson;
                //make sure we don't call too often per second
                //from https://stackoverflow.com/questions/7728569/how-to-limit-method-usage-per-amount-of-time
                await _eventLimiter.EnqueueRequest();
                using (var response = await request.GetResponseAsync())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null)
                    {
                        throw new ApplicationException("ResponseStream is null");
                    }
                    var sr = new StreamReader(responseStream);
                    resultJson = response.ReadToEnd();
                    sr.Close();
                }
                return resultJson;
            }
            catch (WebException e)
            {
                //if ((e.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    throw new ApplicationException("Unauthorized", e);
                //}
                throw;
            }
        }
    }

    class EventLimiter
    {
        readonly Queue<DateTime> _requestTimes;
        readonly int _maxRequests;
        readonly TimeSpan _timeSpan;

        public EventLimiter(int maxRequests, TimeSpan timeSpan)
        {
            _maxRequests = maxRequests;
            _timeSpan = timeSpan;
            _requestTimes = new Queue<DateTime>(maxRequests);
        }

        private void SynchronizeQueue()
        {
            while (_requestTimes.Count > 0 &&
                   _requestTimes.Peek().Add(_timeSpan) < DateTime.UtcNow)
            {
                _requestTimes.Dequeue();
            }
        }

        public bool CanRequestNow()
        {
            SynchronizeQueue();
            return _requestTimes.Count < _maxRequests;
        }

        public async Task EnqueueRequest()
        {
            while (!CanRequestNow())
            {
                await Task.Delay(_requestTimes.Peek().Add(_timeSpan).Subtract(DateTime.UtcNow));
            }

            _requestTimes.Enqueue(DateTime.UtcNow);
        }
    }
}
