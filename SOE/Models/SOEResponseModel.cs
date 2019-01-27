using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOE.Models
{
    public class SOEResponseModel
    {
        public string ErrorMessage { get; set; }
        public Dictionary<string, int> CountOccurrence { get; set; }
        public Dictionary<string, int> MetaTagOccurrences { get; set; }
        public int? ExternaLinkCount { get; set; }
        public string OutputText { get; set; }
    }
}