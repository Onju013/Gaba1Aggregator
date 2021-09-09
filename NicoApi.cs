using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Gaba1Aggregator
{
    /// <summary>
    /// ニコニコAPIの簡易ラッパ
    /// </summary>
    public class NicoApi : IDisposable
    {
        private static readonly string thumbApiUrl = " http://ext.nicovideo.jp/api/getthumbinfo";
        private static readonly string snapApiUrl = "https://api.search.nicovideo.jp/api/v2/snapshot/video/contents/search";
        private static readonly string serviceName = "Gaba1Aggregator";
        private static readonly int limit = 100;

        private readonly HttpClient client;
        private bool disposedValue;

        public NicoApi()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", serviceName);
        }

        /// <summary>
        /// タグでスナップ検索＆サムネイル情報接合
        /// </summary>
        /// <param name="searchTags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ContentData>> SearchByTagsAsync(
            string searchTags,
            CancellationToken cancellationToken)
        {
            var contents = new List<ContentData>();
            var snap = await GetSnapshotByTagsAsync(searchTags, cancellationToken).ConfigureAwait(false);

            foreach (var snapData in snap.Data)
            {
                var thumbInfo = await GetThumbInfoAsync(snapData.ContentId, cancellationToken).ConfigureAwait(false);
                contents.Add(new ContentData(snapData, thumbInfo));
            }

            return contents;
        }

        /// <summary>
        /// サムネイル情報取得API
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>サムネイル情報</returns>
        public async Task<ThumbInfo> GetThumbInfoAsync(
            string contentId,
            CancellationToken cancellationToken)
        {
            var url = thumbApiUrl + "/" + contentId;

            using (var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false))
            {
                var serializer = new XmlSerializer(typeof(ThumbInfo));

                using (var stream = new StringReader(await response.Content.ReadAsStringAsync().ConfigureAwait(false)))
                {
                    return (ThumbInfo)serializer.Deserialize(stream);
                }
            }
        }

        /// <summary>
        /// タグでスナップショット検索
        /// 注意　最大100100件
        /// </summary>
        /// <param name="searchTags">検索タグ</param>
        /// <param name="cancellationToken"></param>
        /// <returns>スナップショット</returns>
        public async Task<Snapshot> GetSnapshotByTagsAsync(
            string searchTags,
            CancellationToken cancellationToken)
        {
            var snap = await GetSnapshotByTagsAsync(searchTags, 0, cancellationToken).ConfigureAwait(false);

            for (int offset = limit; offset < snap.Meta.TotalCount; offset += limit)
            {
                var temp = await GetSnapshotByTagsAsync(searchTags, offset, cancellationToken);

                snap.Data.AddRange(temp.Data);
                snap.Meta.TotalCount = temp.Meta.TotalCount;  //5時をまたいだ時用
            }

            return snap;
        }

        /// <summary>
        /// タグでスナップショット検索
        /// https://site.nicovideo.jp/search-api-docs/snapshot
        /// </summary>
        /// <param name="searchTags">検索タグ</param>
        /// <param name="offset">取得オフセット（max 100,000）</param>
        /// <param name="cancellationToken"></param>
        /// <returns>スナップショット</returns>
        public async Task<Snapshot> GetSnapshotByTagsAsync(
            string searchTags,
            int offset,
            CancellationToken cancellationToken)
        {
            var query = $"q={Uri.EscapeDataString(searchTags)}"
                + "&targets=tags"
                + $"&fields={SnapData.GetFieldNames()}"
                + $"&_sort={Uri.EscapeDataString("+startTime")}"
                + $"&_limit={limit}"
                + $"&_offset={offset}"
                + $"&_context={Uri.EscapeDataString(serviceName)}";
            var url = snapApiUrl + "?" + query;
            Debug.WriteLine($"url : ${url}");

            using (var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpStatusCodeNotOkException(response.StatusCode);
                }

                var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<Snapshot>(jsonString);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}