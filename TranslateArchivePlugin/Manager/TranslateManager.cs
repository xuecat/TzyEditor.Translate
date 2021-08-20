using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslateArchivePlugin.Dto;
using TranslateArchivePlugin.Dto.Request;
using TranslateArchivePlugin.Dto.Response;
using TranslateArchivePlugin.Models.DB;
using TranslateArchivePlugin.Store;
using TzyEditor.TranslateCore;

namespace TranslateArchivePlugin.Manager
{
    public class TranslateManager
    {
        public TranslateManager(ITranslateStore store, IMapper mapper, ILogger<TranslateManager> logger)
        {
            Store = store;
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;
        }

        private readonly ILogger<TranslateManager> _logger;
        protected ITranslateStore Store { get; }
        protected IMapper Mapper { get; }

        public async Task<int> CreateArchiveAsync(TranslateBaseInfoRequest request)
        {
            try
            {
                var f = await Store.AddTranslateArchiveAsync(Mapper.Map<TranslateBaseInfoRequest, TbTranslateArchive>(request,
                    opt => opt.AfterMap((src, dst) => 
                    {
                        dst.FileId = 0;
                        dst.Status = (int)TranslateStatus.EMPTY;
                    })), false);

                await Store.AddTranslateHistoryAsync(new TbTranslateHistory() { TId = f, Time = DateTime.Now, Type = (int)HistoryType.CREATE, User = request.User});
                return f;
            }
            catch (Exception e)
            {
                TzyEditorException.ThrowSelfNoParam(request.LogId.ToString(), GlobalDictionary.ADDRECORDFAILED, _logger, e);
            }
            return 0;
        }

        public async Task<TranslateInfoResponse> GetTranslateInfoAsync(int tid)
        {
            var backinfo = Mapper.Map<TranslateInfoResponse>(await Store.GetTranslateArchiveAsync(a => a.Where(b => b.TId == tid), true));
            backinfo.History = Mapper.Map<List<TranslateHistoryResponse>>(await Store.GetTranslateHistoryListAsync(a => a.Where(b => b.TId == tid), true));
            return backinfo;
        }

    }
}
