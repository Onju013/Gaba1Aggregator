using System;
using System.Collections.Generic;
using System.Net;

namespace Gaba1Aggregator
{
    /// <summary>
    /// Snapshotで返ってくるjson
    /// </summary>
    [Serializable]
    public class Snapshot
    {
        public List<SnapData> Data { get; set; }
        public SnapshotMeta Meta { get; set; }

        public class SnapshotMeta
        {
            public string Id { get; set; }
            public int TotalCount { get; set; }
            public HttpStatusCode Status { get; set; }
        }
    }
}