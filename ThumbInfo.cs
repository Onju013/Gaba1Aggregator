using System;
using System.Xml.Serialization;

namespace Gaba1Aggregator
{
    /// <summary>
    /// GetThumInfoで返ってくるxml
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("nicovideo_thumb_response")]
    public class ThumbInfo
    {
        [XmlAttribute]
        public string status { get; set; }

        public Thumb thumb { get; set; }

        [Serializable]
        [XmlType(AnonymousType = true)]
        public class Thumb
        {
            public string video_id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string thumbnail_url { get; set; }
            public DateTime first_retrieve { get; set; }
            public string length { get; set; }
            public string movie_type { get; set; }
            public int size_high { get; set; }
            public int size_low { get; set; }
            public int view_counter { get; set; }
            public int comment_num { get; set; }
            public int mylist_counter { get; set; }
            public string last_res_body { get; set; }
            public string watch_url { get; set; }
            public string thumb_type { get; set; }
            public bool embeddable { get; set; }
            public bool no_live_play { get; set; }
            public Tags tags { get; set; }
            public string genre { get; set; }
            public int user_id { get; set; }
            public string user_nickname { get; set; }
            public string user_icon_url { get; set; }

            [Serializable]
            [XmlType(AnonymousType = true)]
            public class Tags
            {
                [XmlElement]
                public Tag[] tag { get; set; }

                [XmlAttribute]
                public string domain { get; set; }

                [Serializable]
                [XmlType(AnonymousType = true)]
                public class Tag
                {
                    public byte @lock { get; set; }

                    [XmlIgnore]
                    public bool lockSpecified { get; set; }

                    [XmlText]
                    public string Value { get; set; }
                }
            }
        }
    }
}