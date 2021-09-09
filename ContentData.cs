﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CsvHelper.Configuration.Attributes;
using Gaba1Aggregator.Attrs;

namespace Gaba1Aggregator
{
    /// <summary>
    /// コンテンツデータ
    /// </summary>
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public class ContentData
    {
        public string Status => _thumbInfo.status;

        #region Thumb

        [OutputProperty]
        public string VideoId => Thumb.video_id;

        [OutputProperty]
        public string Title => Thumb.title;

        //[OutputProperty]
        public string Description => Thumb.description;

        [OutputProperty]
        public string ThumbnailUrl => Thumb.thumbnail_url;

        [OutputProperty]
        public DateTime FirstRetrieve => Thumb.first_retrieve;

        [OutputProperty]
        public string Length => Thumb.length;

        [OutputProperty]
        public string MovieType => Thumb.movie_type;

        [OutputProperty]
        public bool SizeHigh => Thumb.size_high;

        [OutputProperty]
        public bool SizeLow => Thumb.size_low;

        [OutputProperty]
        public int ViewCounter => Thumb.view_counter;

        [OutputProperty]
        public int CommentNum => Thumb.comment_num;

        [OutputProperty]
        public int MylistCounter => Thumb.mylist_counter;

        [OutputProperty]
        public string LastResBody => Thumb.last_res_body;

        [OutputProperty]
        public string WatchUrl => Thumb.watch_url;

        [OutputProperty]
        public string ThumbType => Thumb.thumb_type;

        [OutputProperty]
        public bool Embeddable => Thumb.embeddable;

        [OutputProperty]
        public bool NoLivePlay => Thumb.no_live_play;

        [OutputProperty]
        public string Tags => string.Join(" ", Thumb.tags.tag.Select(t => t.Value));

        [OutputProperty]
        public string Genre => Thumb.genre;

        [OutputProperty]
        public int UserId => Thumb.user_id;

        [OutputProperty]
        public string UserNickname => Thumb.user_nickname;

        [OutputProperty]
        public string UserIconUrl => Thumb.user_icon_url;

        #endregion Thumb

        #region Snap

        [OutputProperty]
        public string SnapContentId => _snapData.ContentId;

        [OutputProperty]
        public string SnapTitle => _snapData.Title;

        [OutputProperty]
        public string SnapDescription => _snapData.Description;

        [OutputProperty]
        public int SnapViewCounter => _snapData.ViewCounter;

        [OutputProperty]
        public int SnapMylistCounter => _snapData.MylistCounter;

        [OutputProperty]
        public int SnapLengthSeconds => _snapData.LengthSeconds;

        [OutputProperty]
        public string SnapThumbnailUrl => _snapData.ThumbnailUrl;

        [OutputProperty]
        public DateTime SnapStartTime => _snapData.StartTime;

        [OutputProperty]
        public int SnapCommentCounter => _snapData.CommentCounter;

        [OutputProperty]
        public string SnapCategoryTags => _snapData.CategoryTags;

        [OutputProperty]
        public string SnapTags => _snapData.Tags;

        [OutputProperty]
        public string SnapGenre => _snapData.Genre;

        #endregion Snap

        [OutputProperty]
        public bool IsOryu => Tags.Contains("【おりゅ部門】");
        public bool HasRevenge => Tags.Contains("【リベンジ部門】");

        private ThumbInfo.Thumb Thumb => _thumbInfo.thumb;

        protected readonly SnapData _snapData;
        protected readonly ThumbInfo _thumbInfo;

        public ContentData(SnapData snapData, ThumbInfo thumbInfo)
        {
            _snapData = snapData;
            _thumbInfo = thumbInfo;
        }

        public override string ToString()
        {
            return $"{SnapContentId} : {Title} - {Status}";
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        public class ClassMap : CsvHelper.Configuration.ClassMap<ContentData>
        {
            public ClassMap()
            {
                foreach (var prop in typeof(ContentData).GetProperties())
                {
                    if (prop.GetCustomAttribute<OutputPropertyAttribute>() == null)
                    {
                        continue;
                    }

                    Map(prop.PropertyType, prop);
                }
            }
        }

        public class ClassMapSmall : CsvHelper.Configuration.ClassMap<ContentData>
        {
            public ClassMapSmall()
            {
                Map(x => x.VideoId);
                Map(x => x.UserNickname);
                Map(x => x.Title);
                Map(x => x.IsOryu);
                Map(x => x.HasRevenge);
                Map(x => x.WatchUrl);
                Map(x => x.UserIconUrl);
            }
        }
    }
}