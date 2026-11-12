using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WEB.Models
{
    public static class WaterDataService
    {
        private static ResearchSummary _cachedSummary;
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, (List<string> Headers, List<string[]> Rows)> _cachedCsvData 
            = new Dictionary<string, (List<string> Headers, List<string[]> Rows)>();

        public static ResearchSummary GetSummary()
        {
            if (_cachedSummary != null)
            {
                return _cachedSummary;
            }

            lock (_lock)
            {
                if (_cachedSummary != null) return _cachedSummary;

                string appDataPath = HttpContext.Current != null 
                    ? HttpContext.Current.Server.MapPath("~/App_Data")
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

                string jsonPath = Path.Combine(appDataPath, "research_summary.json");
                if (File.Exists(jsonPath))
                {
                    string json = File.ReadAllText(jsonPath, System.Text.Encoding.UTF8);
                    _cachedSummary = JsonConvert.DeserializeObject<ResearchSummary>(json);
                }
                else
                {
                    _cachedSummary = new ResearchSummary();
                }

                return _cachedSummary;
            }
        }

        public static DatasetViewModel GetDatasetWithSamples(string datasetId, int maxSamples = 200)
        {
            var summary = GetSummary();
            var ds = datasetId.ToLower() == "dataset2" ? summary.Dataset2 : summary.Dataset1;

            var (headers, rows) = GetCsvData(datasetId);
            ds.Headers = headers;
            ds.SampleRows = rows.Take(maxSamples).ToList();
            return ds;
        }

        public static (List<string> Headers, List<string[]> Rows) GetCsvData(string datasetId)
        {
            lock (_lock)
            {
                if (_cachedCsvData.ContainsKey(datasetId))
                {
                    return _cachedCsvData[datasetId];
                }

                string appDataPath = HttpContext.Current != null 
                    ? HttpContext.Current.Server.MapPath("~/App_Data")
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");

                string filename = datasetId.ToLower() == "dataset2" 
                    ? "waterQuality1.csv" 
                    : "water_potability.csv";

                string filePath = Path.Combine(appDataPath, filename);
                var headers = new List<string>();
                var rows = new List<string[]>();

                if (File.Exists(filePath))
                {
                    using (var reader = new StreamReader(filePath, System.Text.Encoding.UTF8))
                    {
                        string headerLine = reader.ReadLine();
                        if (!string.IsNullOrEmpty(headerLine))
                        {
                            headers = headerLine.Split(',').Select(h => h.Trim()).ToList();
                        }

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                var parts = line.Split(',').Select(p => p.Trim()).ToArray();
                                rows.Add(parts);
                            }
                        }
                    }
                }

                _cachedCsvData[datasetId] = (headers, rows);
                return _cachedCsvData[datasetId];
            }
        }

        public static DataTablesResponse QueryCsvData(string datasetId, int draw, int start, int length, string searchValue)
        {
            var (headers, allRows) = GetCsvData(datasetId);

            var filtered = allRows.AsEnumerable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchLower = searchValue.ToLower();
                filtered = filtered.Where(r => r.Any(col => col.ToLower().Contains(searchLower)));
            }

            int recordsFiltered = filtered.Count();
            int recordsTotal = allRows.Count;

            var pageData = filtered.Skip(start).Take(length > 0 ? length : 10).Select(r => r.ToList()).ToList();

            return new DataTablesResponse
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                columns = headers,
                data = pageData
            };
        }
    }
}

