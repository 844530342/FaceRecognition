using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenApiSharedServiceCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OpenApiSharedServiceCore.Service
{
    public class FileOperationConnectionServer
    {
        private  HttpClient _httpClient;


        public FileOperationConnectionServer()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("请求域名地址");
        }
        public class requestparameter {
            public string fileName { get; set; }
            public byte[] streamByte { get; set; }
        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="url"></param>
        /// <param name="rp"></param>
        /// <returns></returns>
        public HttpResponseMessage InvokeMethod(string url, requestparameter rp)
        {          
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            SortedDictionary<string, string> sortDic=  GetParameter(rp);//
            var multipartFormDataContent = new MultipartFormDataContent();
            foreach (var item in sortDic)
            {
                multipartFormDataContent.Add(new StringContent(item.Value), item.Key);
            }
            if (rp.streamByte!=null&& rp.streamByte.Length > 0) {
                var contentByteContent = new ByteArrayContent(rp.streamByte);
                contentByteContent.Headers.Add("Content-Type", "application/octet-stream");
                contentByteContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
                contentByteContent.Headers.ContentDisposition.FileName = rp.fileName;
                contentByteContent.Headers.ContentDisposition.Name = "filekey";//封装POST请求头
                multipartFormDataContent.Add(contentByteContent, "image_content");
            }
            httpRequestMessage.Content = multipartFormDataContent;
            var response =  _httpClient.PostAsync("/uop/method", httpRequestMessage.Content).Result;
            return response;
        }

        private static SortedDictionary<string, string> GetParameter(requestparameter rp)
        {
            SortedDictionary<string, string> sortDic = new SortedDictionary<string, string>();
            System.Reflection.PropertyInfo[] propertys = rp.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo item in propertys)
            {
                if (item.GetType() == typeof(byte[]))
                {
                    continue;
                }
                else
                {
                    string Name = item.Name;
                    string value = item.GetValue(rp, null) == null ? "" : item.GetValue(rp, null).ToString();                   
                    if (!string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(value))
                    {
                        sortDic.Add(Name, value);
                    }
                }

            }
            return sortDic;
        }
    }
}
