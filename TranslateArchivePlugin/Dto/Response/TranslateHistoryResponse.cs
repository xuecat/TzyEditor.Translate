using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateArchivePlugin.Dto.Response
{
    public class TranslateHistoryResponse
    {
        /// <summary>翻译档id</summary>
        /// <example>1</example>
        public int TId { get; set; }
        /// <summary>记录类型 CREATE(0) MODIFY(1)</summary>
        /// <example>1</example>
        public int Type { get; set; }
        /// <summary>记录时间</summary>
        /// <example>yyyy-MM-dd HH:mm:ss</example>
        public DateTime Time { get; set; }
        /// <summary>记录用户</summary>
        /// <example>xxx</example>
        public string User { get; set; }
    }
}
