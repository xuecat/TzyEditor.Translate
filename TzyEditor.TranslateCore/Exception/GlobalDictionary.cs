using System;
using System.Collections.Generic;
using System.Text;

namespace TzyEditor.TranslateCore
{
    public class GlobalDictionary
    {

        #region 内部成员

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        public GlobalDictionary()
        {
            InitDictionary();
        }

        private static Dictionary<int, string> _globalDict = new Dictionary<int, string>();

        private static object _globalDicLock = new object();

        private static GlobalDictionary _instance;

        /// <summary>
        /// 单件实体
        /// </summary>
        public static GlobalDictionary Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_globalDicLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalDictionary();
                        }
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 通过错误编码获得错误信息
        /// </summary>
        /// <param name="nCode">错误编码</param>
        /// <returns>错误信息</returns>
        public string GetMessageByCode(int nCode)
        {
            string strMessage = "";

            if (_globalDict.TryGetValue(nCode, out strMessage))
            {
                var f = strMessage.Split("@");
                if (f != null && f.Length > 2)
                {
                    return f[0];
                }
            }

            return strMessage;
        }

        #endregion

        #region 请参考已有样式，填入字典文件项目

        /// <summary>
        /// 初始化错误信息字典
        /// </summary>
        /// <remarks>
        /// 需要手动将错误代码和对应的错误信息添加到此处
        /// </remarks>
        private void InitDictionary()
        {
            // 请参考如下样式，添加自定义的异常信息
            try
            {
                _globalDict.Add(GLOBALDICT_CODE_UNKNOWN_ERROR, "一般性未知错误@Unknown error@一般性未知錯誤@");
                _globalDict.Add(ADDRECORDFAILED, "添加归档错误@Add Archive error@一般性未知錯誤@");
                // 调试日志
                //ApplicationLog.WriteDebugTraceLog0(string.Format("### GlobalDictionary having {0} count Dict record. ###", _globalDict.Count));
            }
            catch (Exception ae)
            {
                // 错误日志
                //ApplicationLog.WriteInfo(string.Format("In InitDictionary function, Add a duplicate key values! The exception message is: {0}", ae.Message));
            }
        }

        #endregion

        #region 自定义异常代码

        //////////////////////////////////////////////////////////////////////////
        /*           自定义的异常代码范围，从18001开始,至19999至                */
        /*                         请参考下表进行定义                           */
        //////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// 一般性未知错误: 19999
        /// </summary>
        public const int GLOBALDICT_CODE_UNKNOWN_ERROR = 19999;
        public const int ADDRECORDFAILED = 20000;


        #endregion

    }
}
