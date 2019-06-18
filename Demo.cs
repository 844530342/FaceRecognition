﻿using Newtonsoft.Json;
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

<<<<<<< HEAD
namespace 
=======
namespace
>>>>>>> 5d45f0586e8097b94f42bb09832c3c576e88450d
{
    public class ConnectionServer
    {
        public class requestparameter {
            public string fileName { get; set; }
            public byte[] streamByte { get; set; }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="rp"></param>
        /// <returns></returns>
        public HttpResponseMessage InvokeMethod(string url, requestparameter rp)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            SortedDictionary<string, string> sortDic=  GetParameter(rp);//GetParameter(rp)传入需要请求的参数返回键值对
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
            var response =  _httpClient.SendAsync(httpRequestMessage).Result;
            return response;
        }       
    }
}
