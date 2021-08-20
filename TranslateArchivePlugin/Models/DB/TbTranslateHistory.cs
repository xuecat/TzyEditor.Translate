using System;
using System.Collections.Generic;
using System.Text;

namespace TranslateArchivePlugin.Models.DB
{
    public partial class TbTranslateHistory
    {
        public int Id { get; set; }
        public int TId { get; set; }
        public int Type { get; set; }
        public DateTime Time { get; set; }
        public string User { get; set; }
    }
}
