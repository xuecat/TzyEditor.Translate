using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranslateArchivePlugin.Dto.Request;
using TranslateArchivePlugin.Dto.Response;
using TranslateArchivePlugin.Manager;
using TzyEditor.TranslateCore;
using TzyEditor.TranslateCore.Basic;

namespace TranslateArchivePlugin.Controller.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TranslateController : ControllerBase
    {
        private readonly ILogger<TranslateController> _logger;
        private readonly TranslateManager _taskManage;
        private readonly IMapper _mapper;

        public TranslateController(TranslateManager task, IMapper mapper, ILogger<TranslateController> logger)
        {
            _logger = logger;
            _taskManage = task;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// 创建翻译节点
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="req">提交节点信息</param>
        /// <returns>创建之后的节点id</returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResponseMessage<int>> CreateTranslateArchive([FromBody, BindRequired]TranslateBaseInfoRequest req)
        {
            var Response = new ResponseMessage<int>();
            
            try
            {
                Response.Ext = await _taskManage.CreateArchiveAsync(req);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(TzyEditorException))//TzyEditorException会自动打印错误
                {
                    TzyEditorException se = e as TzyEditorException;
                    Response.Code = se.ErrorCode.ToString();
                    Response.Msg = se.Message;
                }
                else
                {
                    Response.Code = ResponseCodeDefines.ServiceError;
                    Response.Msg = "CreateTranslateArchive error info:" + e.Message;
                    _logger.LogError(Response.Msg);
                }
            }
            return Response;
        }


        /// <summary>
        /// 获取指定翻译节点
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="translateid">翻译节点id</param>
        /// <returns>翻译节点所有信息包括历史信息</returns>
        [HttpGet("{translateid}")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResponseMessage<TranslateInfoResponse>> GetTranslateInfo([FromRoute, BindRequired]int translateid)
        {
            _logger.LogInformation($"GetTranslateInfo translateid : {translateid}");

            var Response = new ResponseMessage<TranslateInfoResponse>();
            if (translateid < 1)
            {
                Response.Code = ResponseCodeDefines.ModelStateInvalid;
                Response.Msg = "request param error";
            }
            try
            {
                Response.Ext = await _taskManage.GetTranslateInfoAsync(translateid);
                if (Response.Ext == null)
                {
                    Response.Code = ResponseCodeDefines.NotFound;
                    Response.Msg = $"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName}:error info: not find data!";
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(TzyEditorException))//TzyEditorException会自动打印错误
                {
                    TzyEditorException se = e as TzyEditorException;
                    Response.Code = se.ErrorCode.ToString();
                    Response.Msg = se.Message;
                }
                else
                {
                    Response.Code = ResponseCodeDefines.ServiceError;
                    Response.Msg = "UpdateTaskMetaData error info:" + e.Message;
                    _logger.LogError(Response.Msg);
                }
            }
            return Response;
        }
    }
}
