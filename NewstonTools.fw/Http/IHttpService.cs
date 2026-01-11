using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace NewstonTools.fw.Http
{
    public interface IHttpService
    {
       // string Post(string url, string postData = null, string contentType = "application/json", int timeOut = 30);

        string Get(string url, string contentType = "application/json", Dictionary<string, string> headers = null);
    }
    /// <summary>
    /// <tt>HttpClientService复用同一个HttpClient,HttpClient支持不同的Uri,但是不同的Uri请求头要一致</tt>
    /// <strong>同一个HttpClient的请求头不是线程安全的，如果需要设置请求头，请重新创建一个HttpClientService</strong>
    /// </summary>
    public class HttpClientService : IHttpService, IDisposable
    {
        public readonly HttpClient h_httpClient;
        private ILogger h_Logger;
        private bool disposedValue;

        public HttpClientService() : this(null, new HttpClient()) { }
        public HttpClientService(HttpClient client) : this(null, client) { }
        public HttpClientService(ILogger logger) : this(logger, new HttpClient()) { }

        public HttpClientService(ILogger logger, HttpClient client)
        {
            this.h_Logger = logger;
            this.h_httpClient = client;
        }

        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public string Get(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            using (HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, url))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        req.Headers.Add(header.Key, header.Value);
                }
                if (contentType != null)
                    req.Headers.Add("Content-Type", contentType);
                req.RequestUri = new Uri(url);
                using (HttpResponseMessage response =h_httpClient.SendAsync(req).Result)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }


        ///// <summary>
        ///// 发起GET异步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="headers"></param>
        ///// <param name="contentType"></param>
        ///// <returns></returns>
        //public async Task<string> HttpGetAsync(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        //{
        //    if (contentType != null)
        //        h_httpClient.DefaultRequestHeaders.Add("ContentType", contentType);
        //    if (headers != null)
        //    {
        //        foreach (var header in headers)
        //            h_httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        //    }
        //    HttpResponseMessage response = await h_httpClient.GetAsync(url);
        //    return await response.Content.ReadAsStringAsync();
        //}


        ///// <summary>
        ///// 发起POST同步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        ///// <param name="headers">填充消息头</param>
        ///// <returns></returns>
        //public string HttpPost(string url, string postData = null, string contentType = "application/json", int timeOut = 30)
        //{
        //    postData = postData ?? "";
        //    using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
        //    {
        //        if (contentType != null)
        //            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        //        HttpResponseMessage response = h_httpClient.PostAsync(url, httpContent).Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }

        //}

        ///// <summary>
        ///// 发起POST异步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        ///// <param name="headers">填充消息头</param>
        ///// <returns></returns>
        //public async Task<string> HttpPostAsync(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        //{
        //    postData = postData ?? "";

        //    h_httpClient.Timeout = new TimeSpan(0, 0, timeOut);
        //    if (headers != null)
        //    {
        //        foreach (var header in headers)
        //            h_httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        //    }
        //    using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
        //    {
        //        if (contentType != null)
        //            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        //        HttpResponseMessage response = await h_httpClient.PostAsync(url, httpContent);
        //        return await response.Content.ReadAsStringAsync();
        //    }
        //}




        ///// <summary>
        ///// 发起POST同步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        ///// <param name="headers">填充消息头</param>
        ///// <returns></returns>
        //public T HttpPost<T>(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        //{
        //    return HttpPost(url, postData, contentType, timeOut, headers).ToEntity<T>();
        //}

        ///// <summary>
        ///// 发起POST异步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="postData"></param>
        ///// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        ///// <param name="headers">填充消息头</param>
        ///// <returns></returns>
        //public async Task<T> HttpPostAsync<T>(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        //{
        //    var res = await HttpPostAsync(url, postData, contentType, timeOut, headers);
        //    return res.ToEntity<T>();
        //}

        ///// <summary>
        ///// 发起GET同步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="headers"></param>
        ///// <param name="contentType"></param>
        ///// <returns></returns>
        //public T HttpGet<T>(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        //{
        //    return HttpGet(url, contentType, headers).ToEntity<T>();
        //}

        ///// <summary>
        ///// 发起GET异步请求
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="headers"></param>
        ///// <param name="contentType"></param>
        ///// <returns></returns>
        //public async Task<T> HttpGetAsync<T>(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        //{
        //    var res = await HttpGetAsync(url, contentType, headers);
        //    return res.ToEntity<T>();
        //}


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    h_httpClient.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~HttpClientService()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    //public static class HttpExtension
    //{
    //    /// <summary>
    //    /// 发起GET同步请求
    //    /// </summary>
    //    /// <param name="client"></param>
    //    /// <param name="url"></param>
    //    /// <param name="headers"></param>
    //    /// <param name="contentType"></param>
    //    /// <returns></returns>
    //    public static string HttpGet(this HttpClient client, string url, string contentType = "application/json",
    //                                 Dictionary<string, string> headers = null)
    //    {
    //        if (contentType != null)
    //            client.DefaultRequestHeaders.Add("ContentType", contentType);
    //        if (headers != null)
    //        {
    //            foreach (var header in headers)
    //                client.DefaultRequestHeaders.Add(header.Key, header.Value);
    //        }
    //        HttpResponseMessage response = h_httpClient.GetAsync(url).Result;
    //        return response.Content.ReadAsStringAsync().Result;
    //    }

    //    /// <summary>
    //    /// 发起GET异步请求
    //    /// </summary>
    //    /// <param name="client"></param>
    //    /// <param name="url"></param>
    //    /// <param name="headers"></param>
    //    /// <param name="contentType"></param>
    //    /// <returns></returns>
    //    public static async Task<string> HttpGetAsync(this HttpClient client, string url, string contentType = "application/json", Dictionary<string, string> headers = null)
    //    {
    //        if (contentType != null)
    //            client.DefaultRequestHeaders.Add("ContentType", contentType);
    //        if (headers != null)
    //        {
    //            foreach (var header in headers)
    //                client.DefaultRequestHeaders.Add(header.Key, header.Value);
    //        }
    //        HttpResponseMessage response = await h_httpClient.GetAsync(url);
    //        return await response.Content.ReadAsStringAsync();
    //    }

    //    /// <summary>
    //    /// 发起POST同步请求
    //    /// </summary>
    //    /// <param name="client"></param>
    //    /// <param name="url"></param>
    //    /// <param name="postData"></param>
    //    /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
    //    /// <param name="timeOut"></param>
    //    /// <param name="headers">填充消息头</param>
    //    /// <returns></returns>
    //    public static string HttpPost(this HttpClient client, string url, string postData = null,
    //                                  string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
    //    {
    //        postData = postData ?? "";
    //        if (headers != null)
    //        {
    //            foreach (var header in headers)
    //                client.DefaultRequestHeaders.Add(header.Key, header.Value);
    //        }
    //        using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
    //        {
    //            if (contentType != null)
    //                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

    //            HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
    //            return response.Content.ReadAsStringAsync().Result;
    //        }
    //    }

    //    /// <summary>
    //    /// 发起POST异步请求
    //    /// </summary>
    //    /// <param name="client"></param>
    //    /// <param name="url"></param>
    //    /// <param name="postData"></param>
    //    /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
    //    /// <param name="headers">填充消息头</param>
    //    /// <returns></returns>
    //    public static async Task<string> HttpPostAsync(this HttpClient client, string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
    //    {
    //        postData = postData ?? "";
    //        client.Timeout = new TimeSpan(0, 0, timeOut);
    //        if (headers != null)
    //        {
    //            foreach (var header in headers)
    //                client.DefaultRequestHeaders.Add(header.Key, header.Value);
    //        }
    //        using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
    //        {
    //            if (contentType != null)
    //                httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

    //            HttpResponseMessage response = await h_httpClient.PostAsync(url, httpContent);
    //            return await response.Content.ReadAsStringAsync();
    //        }
    //    }

    //    /// <summary>
    //    /// 发起POST同步请求
    //    /// </summary>
    //    /// <param name="url"></param>
    //    /// <param name="postData"></param>
    //    /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
    //    /// <param name="headers">填充消息头</param>
    //    /// <returns></returns>
    //    public static T HttpPost<T>(this HttpClient client, string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
    //    {
    //        return client.HttpPost(url, postData, contentType, timeOut, headers).ToEntity<T>();
    //    }

    //    /// <summary>
    //    /// 发起POST异步请求
    //    /// </summary>
    //    /// <param name="url"></param>
    //    /// <param name="postData"></param>
    //    /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
    //    /// <param name="headers">填充消息头</param>
    //    /// <returns></returns>
    //    public static async Task<T> HttpPostAsync<T>(this HttpClient client, string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
    //    {
    //        var res = await h_httpClient.HttpPostAsync(url, postData, contentType, timeOut, headers);
    //        return res.ToEntity<T>();
    //    }

    //    /// <summary>
    //    /// 发起GET同步请求
    //    /// </summary>
    //    /// <param name="url"></param>
    //    /// <param name="headers"></param>
    //    /// <param name="contentType"></param>
    //    /// <returns></returns>
    //    public static T HttpGet<T>(this HttpClient client, string url, string contentType = "application/json", Dictionary<string, string> headers = null)
    //    {
    //        return h_httpClient.HttpGet(url, contentType, headers).ToEntity<T>();
    //    }

    //    /// <summary>
    //    /// 发起GET异步请求
    //    /// </summary>
    //    /// <param name="url"></param>
    //    /// <param name="headers"></param>
    //    /// <param name="contentType"></param>
    //    /// <returns></returns>
    //    public static async Task<T> HttpGetAsync<T>(this HttpClient client, string url, string contentType = "application/json", Dictionary<string, string> headers = null)
    //    {
    //        var res = await h_httpClient.HttpGetAsync(url, contentType, headers);
    //        return res.ToEntity<T>();
    //    }
    //}

    /// <summary>
    /// Json扩展方法
    /// </summary>
    public static class JsonExtends
    {
        public static T ToEntity<T>(this string val)
        {
            return JsonSerializer.Deserialize<T>(val);
        }

        //public static List<T> ToEntityList<T>(this string val)
        //{
        //    return JsonConvert.DeserializeObject<List<T>>(val);
        //}
        public static string ToJson<T>(this T entity)
        {
            //JsonSerializerOptions options = new JsonSerializerOptions();
            //options.
            return JsonSerializer.Serialize(entity);
        }
    }

    public class HttpWebRequestService : IHttpService
    {
        public string Get(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }
    }
}
