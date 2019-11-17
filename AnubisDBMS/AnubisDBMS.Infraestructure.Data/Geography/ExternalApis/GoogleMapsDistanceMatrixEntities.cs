using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnubisDBMS.Infraestructure.Data.Geography.ExternalApis
{
    public class GoogleMapsDistanceMatrixClientConfiguration
    {
        public GoogleMapsDistanceMatrixClientConfiguration(string apiKey)
        {
            OutputFormat = OutputFormat.json;
            UnitType = UnitType.metric;
            ApiKey = apiKey;
            Language = "es";
        }

        public string ApiKey { get; private set; }

        public string Language { get; set; }

        public OutputFormat OutputFormat { get; set; }

        public UnitType UnitType { get; set; }
    }

    public enum UnitType
    {
        imperial,
        metric
    }

    public enum OutputFormat
    {
        json,
        xml
    }

    public class DirectionsResponse
    {
        public string status { get; set; }
        public string[] origin_addresses { get; set; }
        public string[] destination_addresses { get; set; }
        public List<DirectionsResponseRow> rows { get; set; }
    }

    public class DirectionsResponseRow
    {
        public List<DirectionsResponseRowElement> elements { get; set; }
    }

    public class DirectionsResponseRowElement
    {
        public string status { get; set; }
        public DirectionsResponseRowElementDuration duration { get; set; }
        public DirectionsResponseRowElementDistance distance { get; set; }
    }

    public class DirectionsResponseRowElementDuration
    {
        public int value { get; set; }
        public string text { get; set; }
    }

    public class DirectionsResponseRowElementDistance
    {
        public int value { get; set; }
        public string text { get; set; }
    }

    public class DirectionsResponseRowElementFare
    {
        public string currency { get; set; }
        public int value { get; set; }
        public string text { get; set; }
    }
}
