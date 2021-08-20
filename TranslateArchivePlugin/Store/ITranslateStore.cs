using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslateArchivePlugin.Models.DB;

namespace TranslateArchivePlugin.Store
{
    public interface ITranslateStore
    {
        Task<int> AddTranslateArchiveAsync(TbTranslateArchive tb, bool notrack = false);
        Task<int> AddTranslateHistoryAsync(TbTranslateHistory tb, bool notrack = false);
        Task<int> AddFileLibraryAsync(TbFileLibrary tb, bool notrack = false);

        Task<List<TResult>> GetTranslateArchiveListAsync<TResult>(Func<IQueryable<TbTranslateArchive>, IQueryable<TResult>> query, bool notrack = false);

        Task<TResult> GetTranslateArchiveAsync<TResult>(Func<IQueryable<TbTranslateArchive>, IQueryable<TResult>> query, bool notrack = false);


        Task<List<TResult>> GetTranslateHistoryListAsync<TResult>(Func<IQueryable<TbTranslateHistory>, IQueryable<TResult>> query, bool notrack = false);

        Task<TResult> GetTranslateHistoryAsync<TResult>(Func<IQueryable<TbTranslateHistory>, IQueryable<TResult>> query, bool notrack = false);

        Task<List<TResult>> GetFileLibraryListAsync<TResult>(Func<IQueryable<TbFileLibrary>, IQueryable<TResult>> query, bool notrack = false);

        Task<TResult> GetFileLibraryAsync<TResult>(Func<IQueryable<TbFileLibrary>, IQueryable<TResult>> query, bool notrack = false);

        Task SaveChangeAsync();
        //Task<TResult> GetTaskMetaDataAsync<TResult>(Func<IQueryable<DbpTaskMetadata>, IQueryable<TResult>> query, bool notrack = false);
        //Task<TResult> GetTaskCustomMetaDataAsync<TResult>(Func<IQueryable<DbpTaskCustommetadata>, IQueryable<TResult>> query, bool notrack = false);
    }
}
