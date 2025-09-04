using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NewstonTools.HttpTool
{
    public class HttpTool : IHttpTool
    {
        private Uri uri;

        public Uri Uri
        {
            get { return uri; }
            set
            {
                uri = value;
            }
        }

        /// <summary>
        /// 失败重复次数，超过就会禁用。
        /// </summary>
        public int RetryTimes { get; set; }

        

        public bool Initialized { get; set; }
        public bool Initialize()
        {
            return Initialize(uri.AbsoluteUri);
        }

        public bool Initialize(string uri)
        {
            if (Initialized) throw new ArgumentNullException("已经初始化完成，不要重复初始化");
            if (uri == null) throw new ArgumentNullException("Uri不能为空");
            this.uri = new Uri(uri);
            httpClient = new HttpClient();
            httpClient.BaseAddress = this.uri;
            Initialized = true;
            return true;
        }

        private HttpClient httpClient { get; set; }

        private bool StatusCheck()
        {
            if (uri == null || !Initialized || httpClient ==null)
                return false;
            return true;
        }

        



        public bool SendMessage(string message)
        {
           throw new NotImplementedException();
        }
    }
}
