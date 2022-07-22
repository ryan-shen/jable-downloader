using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace JableDownloader.Services
{
    public class HlsDownloader
    {
        private HttpClient _client;
        public byte[] Key { get; private set; }
        public byte[] Iv { get; private set; }
        public IEnumerable<string> FileNames { get; private set; }


        public HlsDownloader(string m3u8Url)
        {
            //取得 m3u8 檔案內容
            var m3u8Uri = new Uri(m3u8Url);
            //Query string will be a part of m3u8FileName as well
            var m3u8FileName = Regex.Match(m3u8Url, m3u8Uri.Segments[m3u8Uri.Segments.Length - 1] + ".*").Value;
            var baseUrl = m3u8Uri.AbsoluteUri.Replace(m3u8FileName, "");

            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            var m3u8Content = _client.GetAsync(m3u8FileName).Result.Content.ReadAsStringAsync().Result;

            //解析 m3u8 檔案內容
            string extXKey = Regex.Match(m3u8Content, @"#EXT\-X\-KEY.*").Value;
            string keyFileName = Regex.Match(extXKey, "(?<=URI=\").*(?=\")").Value;
            Key = string.IsNullOrEmpty(keyFileName) ? null : _client.GetAsync(keyFileName).Result.Content.ReadAsByteArrayAsync().Result;
            string ivByteString = Regex.Match(extXKey, "(?<=IV=0x).*").Value;
            Iv = string.IsNullOrEmpty(ivByteString) ? null : TypeConverter.ToByteArray(ivByteString);

            //抓出所有 .ts 檔名
            var matches = Regex.Matches(m3u8Content, @"(?<=#EXTINF:.+,\s?).+(?=\r?\n)");

            var fileNames = new List<string>();
            foreach (Match match in matches)
            {
                fileNames.Add(match.Value);
            }
            FileNames = fileNames;
        }

        /// <summary>
        /// 下載所有 .ts 檔
        /// </summary>
        /// <returns></returns>
        public async Task SequenceDownloadAsync(string downloadPath, Action<int, int> onSegmentDownloaded)
        {
            int fileCount = FileNames.Count();
            var index = 0;

            var plainBytes = new List<byte>();
            foreach (var fileName in FileNames)
            {
                HttpResponseMessage task = await DownloadFileAsync(fileName);

                onSegmentDownloaded(++index, fileCount);

                plainBytes.AddRange(Decrypt(await task.Content.ReadAsByteArrayAsync()));
            }

            File.WriteAllBytes(downloadPath, plainBytes.ToArray());
        }

        /// <summary>
        /// 下載所有 .ts 檔
        /// </summary>
        /// <returns></returns>
        public async Task DownloadAsync(string downloadPath, Action<int, int> onSegmentDownloaded)
        {
            int fileCount = FileNames.Count();
            int index = 0;

            var tasks = new List<Task<byte[]>>();
            foreach (var fileName in FileNames)
            {
                Task<byte[]> task = DownloadFileAsync(fileName).ContinueWith((result) =>
                {
                    onSegmentDownloaded(++index, fileCount);

                    return Decrypt(result.Result.Content.ReadAsByteArrayAsync().Result);
                });

                tasks.Add(task);
            }
            await Task.WhenAll(tasks);

            //解密
            byte[] plainBytes = tasks.SelectMany(x => x.Result).ToArray();

            File.WriteAllBytes(downloadPath, plainBytes);
        }

        //TODO: 不知為啥沒有建立新執行緒
        private async Task<HttpResponseMessage> DownloadFileAsync(string fileName)
        {
            HttpResponseMessage response;

            try
            {
                response = await _client.GetAsync(fileName);

                if (!response.IsSuccessStatusCode)
                {
                    //Download again in case of connection error
                    response = await _client.GetAsync(fileName);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"An error occurred when downloading {fileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                //Download again in case of faulted task
                response = await _client.GetAsync(fileName);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"An error occurred when downloading {fileName}", ex);
                }
            }

            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

            return response;
        }

        private byte[] Decrypt(byte[] bytes)
        {
            if (Key == null || !Key.Any())
            {
                return bytes;
            }

            var aes = Aes.Create();
            aes.BlockSize = 128;
            aes.Key = Key;
            aes.IV = Iv;

            var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
        }
    }
}
