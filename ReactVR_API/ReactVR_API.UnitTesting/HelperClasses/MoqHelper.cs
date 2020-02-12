using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReactVR_UnitTesting.HelperClasses
{
    public static class MoqHelper
    {
        public static Mock<HttpRequest> CreateMockRequest(object body)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(x => x.Body).Returns(ms);

            return mockRequest;
        }

        public static Mock<HttpRequest> CreateMockRequest(object body, string jwt)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);

            var json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            var mockRequest = new Mock<HttpRequest>();

            mockRequest.Setup(x => x.Body).Returns(ms);

            var headers = new HeaderDictionary();
            headers.Add("Authorization", jwt);

            mockRequest.Setup(x => x.Headers).Returns(headers);

            return mockRequest;
        }

        public static Mock<HttpRequest> CreateMockRequest(string jwt)
        {
            var mockRequest = new Mock<HttpRequest>();

            var headers = new HeaderDictionary();
            headers.Add("Authorization", jwt);

            mockRequest.Setup(x => x.Headers).Returns(headers);

            return mockRequest;
        }
    }
}
