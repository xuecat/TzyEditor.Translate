﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace TzyEditor.TranslateCore
{
    public class BaseException : System.ApplicationException
    {
        public int ErrorCode { get; set; }
        //构造
        //无错误码
        protected BaseException(string strMessage, System.Exception oInner, ILogger logger, int lerrcode) : base(strMessage, oInner)
        {
            ErrorCode = lerrcode;
            Dump(Format(strMessage, oInner), logger);
        }

        private void Dump(string str, ILogger logger)
        {
            if (logger != null)
            {
                logger.LogError(str);
            }


        }
        //有错误码

        public static string Format(string msg, System.Exception inner)
        {
            StringBuilder newmsg = new StringBuilder();
            string errorstack = null;
            //获取错误堆栈
            errorstack = BuildErrorStack(inner);

            newmsg.Append("RecException Summary \n")
                //.Append("---------------------------\n")
                //.Append(DateTime.Now.ToShortDateString())
                //.Append(":")
                //.Append(DateTime.Now.ToShortTimeString())
                .Append(" - ")
                .Append(msg)
                .Append("\n\n")
                .Append(errorstack);

            return newmsg.ToString();
        }

        private static string BuildErrorStack(System.Exception chain)
        {
            string error_stack = null;
            StringBuilder stack_builder = new StringBuilder();
            int num = 1;
            System.Exception inner = null;

            if (chain != null)
            {

                inner = chain;

                while (inner != null)
                {
                    stack_builder.Append(num).Append(") ").Append(inner.Message).Append("\n");
                    inner = inner.InnerException;
                    num++;
                }

                stack_builder.Append("Call Stack\n");
                stack_builder.Append(chain.StackTrace);
                error_stack = stack_builder.ToString();
            }
            else
            {
                //				error_stack = "exception was not chained";
                error_stack = "";
                //		error_stack+=Environment.StackTrace;
            }

            return error_stack;
        }
    }

    public class TzyEditorException : BaseException
    {

        private TzyEditorException(string strMessage, System.Exception oInner, ILogger logger, int lLogErrorCode) : base(strMessage, oInner, logger, lLogErrorCode)
        {
        }

        
       

        /// <summary>
        /// 通过自定义异常代码及上层异常抛出自定义异常
        /// </summary>
        /// <param name="lLogErrorCode">自定义异常代码</param>
        /// <param name="oInner">上层异常</param>
        /// <remarks>
        /// </remarks>
        public static void ThrowSelfNoParam(string custommessage, int lLogErrorCode, ILogger logger, System.Exception oInner)
        {
            string strMessage = custommessage + ':';
            strMessage += GlobalDictionary.Instance.GetMessageByCode(lLogErrorCode);
            BuildErrInfo(oInner, ref strMessage);
            TzyEditorException ex = new TzyEditorException(strMessage, oInner, logger, lLogErrorCode);
            throw ex;
        }

        public static void ThrowSelfOneParam(string custommessage, int lLogErrorCode, ILogger logger, object p, System.Exception oInner)
        {
            string strMessage = custommessage + ':';
            strMessage += GlobalDictionary.Instance.GetMessageByCode(lLogErrorCode);
            strMessage = string.Format(strMessage, p.ToString());
            BuildErrInfo(oInner, ref strMessage);
            TzyEditorException ex = new TzyEditorException(strMessage, oInner, logger, lLogErrorCode);
            throw ex;
        }

        public static void ThrowSelfTwoParam(string custommessage, int lLogErrorCode, ILogger logger, object p1, object p2, System.Exception oInner)
        {
            string strMessage = custommessage + ':';
            strMessage += GlobalDictionary.Instance.GetMessageByCode(lLogErrorCode);
            strMessage = string.Format(strMessage, p1.ToString(), p2.ToString());
            BuildErrInfo(oInner, ref strMessage);
            TzyEditorException ex = new TzyEditorException(strMessage, oInner, logger, lLogErrorCode);
            throw ex;
        }

        private static void BuildErrInfo(Exception oInner, ref string strMessage)
        {
            System.Exception walker = oInner;
            while (walker != null)
            {
                strMessage += ":";
                strMessage += walker.Message;
                walker = walker.InnerException;
            }
        }
    }
}
