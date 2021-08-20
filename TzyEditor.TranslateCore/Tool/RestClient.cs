
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TzyEditor.TranslateCore.Basic;
using TzyEditor.TranslateCore.Dto;

namespace TzyEditor.TranslateCore.Tool
{

    public class RestClient : IDisposable
    {
        private readonly ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

        private HttpClient _httpClient = null;

        //const string DEVICEAPI20 = "api/v2/device";

        private string WebServerUrl { get; set; }

        private bool _disposed;

        public RestClient(string url)
        {
            _disposed = false;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Connection.Clear();
            _httpClient.DefaultRequestHeaders.ConnectionClose = false;
            _httpClient.Timeout = TimeSpan.FromSeconds(15);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_httpClient.DefaultRequestHeaders.Add("http-system", "SERVER");
            WebServerUrl = url;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~RestClient()
        {
            //必须为false
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {

            }
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
            _disposed = true;
        }
        
        public async Task<TResponse> PostAsync<TResponse>(string url, object body, string method = "POST", NameValueCollection queryString = null, int timeout = 60)
            where TResponse : class, new()
        {
            TResponse response = null;
            try
            {
                string json = JsonHelper.ToJson(body);
                HttpClient client = _httpClient;
                if (queryString == null)
                {
                    queryString = new NameValueCollection();
                }
                if (String.IsNullOrEmpty(method))
                {
                    method = "POST";
                }
                url = CreateUrl(url, queryString);
                //Logger.Debug("请求：{0} {1}", method, url);
                byte[] strData = Encoding.UTF8.GetBytes(json);
                MemoryStream ms = new MemoryStream(strData);
                using (StreamContent sc = new StreamContent(ms))
                {
                    sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                    //foreach (var item in _httpClient.DefaultRequestHeaders)
                    //{
                    //    Logger.Error("header :  " + item.Key + ":" + item.Value.FirstOrDefault());
                    //    foreach(var test in item.Value)
                    //    {
                    //        Logger.Error("test :  " + test);
                    //    }
                    //}

                    var res = await client.PostAsync(url, sc).ConfigureAwait(true);
                    byte[] rData = await res.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                    string rJson = Encoding.UTF8.GetString(rData);
                    Logger.Info("url body response：\r\n{0} {1} {2}", url, json, rJson);
                    response = JsonHelper.ToObject<TResponse>(rJson);
                    return response;
                }

            }
            catch (System.Exception e)
            {
                TResponse r = new TResponse();
                Logger.Error("请求异常：\r\n{0} {1}", e.ToString(), url);
                throw;
            }
        }

        public async Task<string> PostAsync(string url, string body, string method = "POST", NameValueCollection queryString = null, int timeout = 60)
        {
            string response = null;
            try
            {
                string json = body;
                HttpClient client = _httpClient;
                if (queryString == null)
                {
                    queryString = new NameValueCollection();
                }
                if (String.IsNullOrEmpty(method))
                {
                    method = "POST";
                }
                url = CreateUrl(url, queryString);
                //Logger.Debug("请求：{0} {1}", method, url);
                byte[] strData = Encoding.UTF8.GetBytes(json);
                MemoryStream ms = new MemoryStream(strData);
                using (StreamContent sc = new StreamContent(ms))
                {
                    sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    var res = await client.PostAsync(url, sc).ConfigureAwait(true);
                    if (res.Content == null || res.Content.Headers.ContentLength == 0)
                    {
                        response = "";
                    }
                    else
                    {
                        byte[] rData = await res.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                        string rJson = Encoding.UTF8.GetString(rData);
                        //Logger.Debug("应答：\r\n{0}", rJson);
                        response = rJson;
                    }
                }

            }
            catch (System.Exception e)
            {
                response = "ERROR";
                Logger.Error("请求异常：\r\n{0} {1}", e.ToString(), url);
            }
            return response;
        }

        public async Task<TResponse> PutAsync<TResponse>(string url, object body, Dictionary<string, string> header, NameValueCollection queryString = null)
        {
            TResponse response = default(TResponse);
            try
            {
                string json = JsonHelper.ToJson(body);
                HttpClient client = _httpClient;
                if (queryString == null)
                {
                    queryString = new NameValueCollection();
                }

                url = CreateUrl(url, queryString);
                //Logger.Debug("请求：{0} {1}", method, url);
                byte[] strData = Encoding.UTF8.GetBytes(json);
                MemoryStream ms = new MemoryStream(strData);
                using (StreamContent sc = new StreamContent(ms))
                {
                    sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    if (header != null)
                    {
                        foreach (var item in header)
                        {
                            sc.Headers.Add(item.Key, item.Value);
                        }
                    }

                    var res = await client.PutAsync(url, sc).ConfigureAwait(true);
                    byte[] rData = await res.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                    string rJson = Encoding.UTF8.GetString(rData);
                    //Logger.Debug("应答：\r\n{0}", rJson);
                    response = JsonHelper.ToObject<TResponse>(rJson);
                    return response;
                }

            }
            catch (System.Exception e)
            {
                Logger.Error("请求异常：\r\n{0} {1}", e.ToString(), url);
                throw;
            }
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url, Dictionary<string, string> header, NameValueCollection queryString = null)
            where TResponse : class, new()
        {
            TResponse response = default(TResponse);
            try
            {
                HttpClient client = _httpClient;
                if (queryString == null)
                {
                    queryString = new NameValueCollection();
                }

                url = CreateUrl(url, queryString);
                //Logger.Debug("请求：{0} {1}", method, url);

                using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url))
                {
                    if (header != null)
                    {
                        foreach (var item in header)
                        {
                            requestMessage.Headers.Add(item.Key, item.Value);
                        }
                    }
                    var backinfo = await client.SendAsync(requestMessage).ConfigureAwait(true);
                    var rJson = await backinfo.Content.ReadAsStringAsync().ConfigureAwait(true);
                    Logger.Info("url response：\r\n{0} {1}", url, rJson);
                    response = JsonHelper.ToObject<TResponse>(rJson);
                }

            }
            catch (System.Exception e)
            {
                TResponse r = new TResponse();
                Logger.Error("请求异常：\r\n{0}", e.ToString(), url);
                return r;
            }
            return response;
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, NameValueCollection queryString, Dictionary<string, string> header)
                    where TResponse : class, new()
        {
            TResponse response = null;
            try
            {
                HttpClient client = _httpClient;
                if (queryString != null)
                {
                    url = CreateUrl(url, queryString);
                }

                //Logger.Debug("请求：{0} {1}", "GET", url);
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    if (header != null)
                    {
                        foreach (var item in header)
                        {
                            requestMessage.Headers.Add(item.Key, item.Value);
                        }
                    }
                    var backinfo = await client.SendAsync(requestMessage).ConfigureAwait(true);
                    var rJson = await backinfo.Content.ReadAsStringAsync().ConfigureAwait(true);
                    Logger.Info("url response：\r\n{0} {1}", url, rJson);
                    response = JsonHelper.ToObject<TResponse>(rJson);
                }

            }
            catch (System.Exception e)
            {
                TResponse r = new TResponse();
                Logger.Error("请求异常：\r\n{0} {1}", e.ToString(), url);
                return r;
            }
            return response;
        }


        public async Task<TResponse> PostAsync<TResponse>(string url, object body, Dictionary<string, string> header, string method = null, NameValueCollection queryString = null)
        {
            TResponse response = default(TResponse);
            try
            {
                string json = JsonHelper.ToJson(body);
                HttpClient client = _httpClient;
                if (queryString == null)
                {
                    queryString = new NameValueCollection();
                }

                url = CreateUrl(url, queryString);
                if (String.IsNullOrEmpty(method))
                {
                    method = "POST";
                }
                //Logger.Debug("请求：{0} {1}", method, url);
                byte[] strData = Encoding.UTF8.GetBytes(json);
                MemoryStream ms = new MemoryStream(strData);
                using (StreamContent sc = new StreamContent(ms))
                {
                    sc.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    if (header != null)
                    {
                        foreach (var item in header)
                        {
                            sc.Headers.Add(item.Key, item.Value);
                        }
                    }

                    var res = await client.PostAsync(url, sc).ConfigureAwait(true);
                    byte[] rData = await res.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
                    string rJson = Encoding.UTF8.GetString(rData);
                    //Logger.Debug("应答：\r\n{0}", rJson);
                    response = JsonHelper.ToObject<TResponse>(rJson);
                    return response;
                }

            }
            catch (System.Exception)
            {
                //Logger.Error("请求异常：\r\n{0}", e.ToString());
                throw;
            }
        }


        public async Task<TResult> SubmitFormAsync<TResult>(string url, Dictionary<string, string> formData, string method = "Post")
        {
            HttpMethod hm = new HttpMethod(method);
            using (var request = new HttpRequestMessage(hm, url))
            {
                request.Content = new FormUrlEncodedContent(formData);
                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(true);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new UnauthorizedAccessException("验证失败");
                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(str);
            }

        }

        public static string CreateUrl(string url, NameValueCollection qs)
        {
            if (qs != null && qs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                List<string> kl = qs.AllKeys.ToList();
                foreach (string k in kl)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }
                    sb.Append(k).Append("=");
                    if (!String.IsNullOrEmpty(qs[k]))
                    {

                        sb.Append(System.Net.WebUtility.UrlEncode(qs[k]));
                    }
                }

                if (url != null)
                {
                    if (url.Contains("?"))
                    {
                        url = url + "&" + sb.ToString();
                    }
                    else
                    {
                        url = url + "?" + sb.ToString();
                    }
                }

            }

            return url;

        }

        #region Configuration
        public async Task<WebConfig> GetWebConfig()
        {
            string group = "";
            string key = "";
            string host = "";

            var back = await AutoRetry.RunAsync<ResponseMessage<WebConfig>>(() =>
            {
                return GetAsync<ResponseMessage<WebConfig>>(
                    $"{WebServerUrl}/{group}/{key}/{host}"
                    , null, null);
            }).ConfigureAwait(true);

            if (back != null)
            {
                return back.Ext;
            }
            return null;
        }
        #endregion


    }
}
    
