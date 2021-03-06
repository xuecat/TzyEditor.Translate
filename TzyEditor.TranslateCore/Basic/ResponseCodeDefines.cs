using System;
using System.Collections.Generic;
using System.Text;

namespace TzyEditor.TranslateCore.Basic
{
    public class ResponseCodeDefines
    {
        public static readonly string SuccessCode = "0";
        public static readonly string ModelStateInvalid = "100";
        public static readonly string ArgumentNullError = "101";
        public static readonly string ObjectAlreadyExists = "102";
        public static readonly string PartialFailure = "103";

        public static readonly string NotFound = "404";
        public static readonly string NotAllow = "403";
        public static readonly string ServiceError = "500";
    }
}
