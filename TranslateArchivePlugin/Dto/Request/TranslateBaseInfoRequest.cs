using System;
using System.Collections.Generic;
using System.Text;
using TranslateArchivePlugin.Dto.Response;

namespace TranslateArchivePlugin.Dto.Request
{
    public class TranslateBaseInfoRequest : TranslateBaseInfoResponse
    {
        /// <summary>请求用户</summary>
        /// <example>0</example>
        public string User { get; set; }
    }
}
