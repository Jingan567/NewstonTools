using NewstonTools.fw.Http;
using NUnit.Framework;
using System.Diagnostics;
using System.Net.Http;

namespace NewstonToolsfwTest
{
    public class HttpClientServiceTest1
    {
        IHttpService httpService;
        [SetUp]
        public void Setup()
        {
            httpService = new HttpClientService(new HttpClient());
        }

        [Test]
        public void GetTest1()
        {
            string Content = httpService.Get("https://www.baidu.com/");
            if (Content != null)
                Assert.Pass("³É¹¦");
            Debug.WriteLine(Content);
        }
    }
}