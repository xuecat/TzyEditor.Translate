using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateArchivePlugin.Models.DB
{
    public partial class TbTranslateArchive
    {
        public int TId { get; set; }
        public string Trunk { get; set; }
        public int LogId { get; set; }
        public int FileId { get; set; }
        public int Status { get; set; }
    }
}
