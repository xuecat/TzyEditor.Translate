using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TranslateArchivePlugin.Dto.Request;
using TranslateArchivePlugin.Dto.Response;
using TranslateArchivePlugin.Models.DB;

namespace TranslateArchivePlugin
{
    public class TranslateProfile : Profile
    {
        public TranslateProfile()
        {
            CreateMap<TranslateBaseInfoRequest, TbTranslateArchive>().ReverseMap();

            CreateMap<TbTranslateArchive, TranslateInfoResponse>().ReverseMap();

            CreateMap<TbTranslateHistory, TranslateHistoryResponse>().ReverseMap();
            
        }
    }
}
