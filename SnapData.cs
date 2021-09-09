using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Gaba1Aggregator.Attrs;

namespace Gaba1Aggregator
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    [Serializable]
    public class SnapData
    {
        /// <summary>
        /// コンテンツID
        /// sm～～～
        /// </summary>
        [SnapField]
        public string ContentId { get; set; }

        /// <summary>
        /// タイトル
        /// </summary>
        [SnapField]
        public string Title { get; set; }

        /// <summary>
        /// コンテンツ説明文
        /// </summary>
        [SnapField]
        public string Description { get; set; }

        /// <summary>
        /// 再生数
        /// </summary>
        [SnapField]
        public int ViewCounter { get; set; }

        /// <summary>
        /// マイリスト数
        /// </summary>
        [SnapField]
        public int MylistCounter { get; set; }

        /// <summary>
        /// 再生時間
        /// </summary>
        [SnapField]
        public int LengthSeconds { get; set; }

        /// <summary>
        /// サムネイルURL
        /// </summary>
        [SnapField]
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// 投稿日時
        /// </summary>
        [SnapField]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// コメント数
        /// </summary>
        [SnapField]
        public int CommentCounter { get; set; }

        /// <summary>
        /// カテゴリタグ
        /// </summary>
        [SnapField]
        public string CategoryTags { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        [SnapField]
        public string Tags { get; set; }

        /// <summary>
        /// ジャンル
        /// </summary>
        [SnapField]
        public string Genre { get; set; }

        /// <summary>
        /// fieldsに投げるための一覧取得
        /// </summary>
        /// <returns>,で区切ったフィールド一覧</returns>
        public static string GetFieldNames()
        {
            var fields = typeof(SnapData).GetProperties()
                .Select(prop => new { prop, attr = prop.GetCustomAttribute<SnapFieldAttribute>() })
                .Where(x => x.attr != null)
                .Select(x => x.attr.FieldName ?? (x.prop.Name[0].ToString().ToLower() + x.prop.Name.Substring(1, x.prop.Name.Length - 1)));

            return string.Join(",", string.Join(",", fields));
        }

        public static string GetFieldNames2()
        {
            var fields = typeof(SnapData).GetProperties()
                .Select(prop => new { prop, attr = prop.GetCustomAttribute<SnapFieldAttribute>() })
                .Where(x => x.attr != null)
                .Select(x => x.attr.FieldName ?? (x.prop.Name[0].ToString().ToLower() + x.prop.Name.Substring(1, x.prop.Name.Length - 1)));

            return string.Join(",", string.Join(",", fields));
        }

        public override string ToString()
        {
            return $"{ContentId} : {Title}";
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}