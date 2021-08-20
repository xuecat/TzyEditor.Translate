using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateArchivePlugin.Dto.Response
{
    public class TranslateBaseInfoResponse
    {
        /// <summary>翻译归档id</summary>
        /// <example>0</example>
        public int TId { get; set; }
        /// <summary>日志id</summary>
        /// <example>1</example>
        public int LogId { get; set; }
        /// <summary>分支信息</summary>
        /// <example>develop_truck</example>
        public string Trunk { get; set; }
        /// <summary>文件id</summary>
        /// <example>1</example>
        public int FileId { get; set; }
        /// <summary>翻译归档状态 EMPTY(0) RECORD(1) UPLOADED(2) EXPORT(3)</summary>
        /// <example>1</example>
        public TranslateStatus Status { get; set; }
    }

    public class TranslateInfoResponse : TranslateBaseInfoResponse
    {
        public List<TranslateHistoryResponse> History { get; set; }
    }
}
