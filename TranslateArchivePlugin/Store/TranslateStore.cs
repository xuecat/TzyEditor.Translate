using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslateArchivePlugin.Models;
using TranslateArchivePlugin.Models.DB;

namespace TranslateArchivePlugin.Store
{
    public class TranslateStore : ITranslateStore
    {
        public TranslateStore(ArchiveDBContext baseDataDbContext)
        {
            Context = baseDataDbContext;
        }
        
        protected ArchiveDBContext Context { get; }

        public async Task<int> AddFileLibraryAsync(TbFileLibrary tb, bool notrack = false)
        {
            try
            {
                await Context.TbFileLibrary.AddAsync(tb);
                if (!notrack)
                {
                    await Context.SaveChangesAsync();
                }
                return tb.FileId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> AddTranslateArchiveAsync(TbTranslateArchive tb, bool notrack = false)
        {
            try
            {
                await Context.TbTranslateArchive.AddAsync(tb);
                if (!notrack)
                {
                    await Context.SaveChangesAsync();
                }
                return tb.TId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> AddTranslateHistoryAsync(TbTranslateHistory tb, bool notrack = false)
        {
            try
            {
                await Context.TbTranslateHistory.AddAsync(tb);
                if (!notrack)
                {
                    await Context.SaveChangesAsync();
                }
                return tb.Id;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task<TResult> GetFileLibraryAsync<TResult>(Func<IQueryable<TbFileLibrary>, IQueryable<TResult>> query, bool notrack = false)
        {
            return GetFirstOrDefaultAsync(Context.TbFileLibrary, query, notrack);
        }

        public Task<List<TResult>> GetFileLibraryListAsync<TResult>(Func<IQueryable<TbFileLibrary>, IQueryable<TResult>> query, bool notrack = false)
        {
            return QueryModelListAsync(Context.TbFileLibrary, query, notrack);
        }

        public Task<TResult> GetTranslateArchiveAsync<TResult>(Func<IQueryable<TbTranslateArchive>, IQueryable<TResult>> query, bool notrack = false)
        {
            return GetFirstOrDefaultAsync(Context.TbTranslateArchive, query, notrack);
        }

        public Task<List<TResult>> GetTranslateArchiveListAsync<TResult>(Func<IQueryable<TbTranslateArchive>, IQueryable<TResult>> query, bool notrack = false)
        {
            return QueryModelListAsync(Context.TbTranslateArchive, query, notrack);
        }

        public Task<TResult> GetTranslateHistoryAsync<TResult>(Func<IQueryable<TbTranslateHistory>, IQueryable<TResult>> query, bool notrack = false)
        {
            return GetFirstOrDefaultAsync(Context.TbTranslateHistory, query, notrack);
        }

        public Task<List<TResult>> GetTranslateHistoryListAsync<TResult>(Func<IQueryable<TbTranslateHistory>, IQueryable<TResult>> query, bool notrack = false)
        {
            return QueryModelListAsync(Context.TbTranslateHistory, query, notrack);
        }

        public Task SaveChangeAsync()
        {
            try
            {
                return Context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw e;
            }
        }

        protected Task<TResult> GetFirstOrDefaultAsync<TDbp, TResult>(DbSet<TDbp> contextSet, Func<IQueryable<TDbp>, IQueryable<TResult>> query, bool notrack = false) where TDbp : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (notrack)
            {
                return query(contextSet.AsNoTracking()).FirstOrDefaultAsync();
            }
            return query(contextSet).FirstOrDefaultAsync();
        }
        protected Task<List<TResult>> QueryModelListAsync<TDbp, TResult>(DbSet<TDbp> contextSet, Func<IQueryable<TDbp>, IQueryable<TResult>> query, bool notrack = false) where TDbp : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (notrack)
            {
                return query(contextSet.AsNoTracking()).ToListAsync();
            }
            return query(contextSet).ToListAsync();
        }
    }
}
