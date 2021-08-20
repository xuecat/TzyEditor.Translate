using System;
using System.Collections.Generic;
using System.Text;

namespace TzyEditor.TranslateCore.Basic
{
    public class ResponseMessage
    {
        public string Code { get; set; }
        public string Msg { get; set; }

        public ResponseMessage()
        {
            Code = ResponseCodeDefines.SuccessCode;
        }

        public bool IsSuccess()
        {
            if (Code == ResponseCodeDefines.SuccessCode)
            {
                return true;
            }
            return false;
        }
    }

    public class ResponseMessage<TEx> : ResponseMessage
    {
        public TEx Ext { get; set; }
    }
}
