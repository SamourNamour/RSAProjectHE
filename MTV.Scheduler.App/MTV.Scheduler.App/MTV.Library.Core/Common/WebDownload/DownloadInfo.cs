using System.IO;
using System.Net;

namespace MTV.Library.Core.Common.WebDownload
{
    /// <summary>
    /// Summary description for DownloadInfo.
    /// </summary>
    public class DownloadInfo
    {
        const int BufferSize = 1024;
        public byte[] BufferRead;

        public bool useFastBuffers;
        public byte[] dataBufferFast;
        public System.Collections.ArrayList dataBufferSlow;

        public int dataLength;
        public int bytesProcessed;

        public WebRequest Request;
        public Stream ResponseStream;

        public DownloadInfo()
        {
            BufferRead = new byte[BufferSize];
            Request = null;
            dataLength = -1;
            bytesProcessed = 0;
            useFastBuffers = true;
        }
    }
}
