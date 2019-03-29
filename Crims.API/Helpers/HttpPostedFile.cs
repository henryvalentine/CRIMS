using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Crims.API.Helpers
{
    public class HttpFile : HttpPostedFileBase
    {
        private readonly byte[] _fileBytes;

        public HttpFile(byte[] fileBytes, string fileName = null)
        {
            _fileBytes = fileBytes;
            FileName = fileName;
            InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => _fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
        
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        {
            
        }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return name.Replace("\"", string.Empty);
        }
    }
}
