using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.FrontEndFrameworks.Semantic
{
    public class SemanticStandardSearchResponse
    {
        public SemanticStandardSearchResponse()
        {
            success = false;
            results = new List<SemanticStandardSearchResponseResult>();
        }

        public bool success { get; set; }
        public List<SemanticStandardSearchResponseResult> results { get; set; }
    }

    public class SemanticStandardSearchResponseResult
    {
        public string title { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string price { get; set; }
        public string description { get; set; }
    }

    public class SemanticStandardDropdownSearchResponse
    {
        public SemanticStandardDropdownSearchResponse()
        {
            success = false;
            results = new List<SemanticStandardDropdownSearchResponseResult>();
        }

        public bool success { get; set; }
        public List<SemanticStandardDropdownSearchResponseResult> results { get; set; }
    }

    public class SemanticStandardDropdownSearchResponseResult
    {
        public SemanticStandardDropdownSearchResponseResult()
        {
            disabled = false;
        }

        public string name { get; set; }
        public string value { get; set; }
        public string text { get; set; }
        public bool disabled { get; set; }
    }
}
