using System;
using System.Collections.Generic;

namespace WEB.Models
{
    public class ProjectInfo
    {
        public string Title { get; set; }
        public string Team { get; set; }
        public string Description { get; set; }
        public List<string> PipelineSteps { get; set; }

        public ProjectInfo()
        {
            PipelineSteps = new List<string>();
        }
    }

    public class MissingValueItem
    {
        public string Feature { get; set; }
        public int MissingCount { get; set; }
        public double Percentage { get; set; }
        public string Handling { get; set; }
    }

    public class FeatureItem
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
    }

    public class LeaderboardItem
    {
        public int Rank { get; set; }
        public string Model { get; set; }
        public double TestF1 { get; set; }
        public double TestRecall { get; set; }
        public double TestPrecision { get; set; }
        public double TestAccuracy { get; set; }
        public double RocAuc { get; set; }
        public bool IsBest { get; set; }
    }

    public class TargetDistributionModel
    {
        public List<string> Labels { get; set; }
        public List<int> Counts { get; set; }
        public List<double> Percentages { get; set; }

        public TargetDistributionModel()
        {
            Labels = new List<string>();
            Counts = new List<int>();
            Percentages = new List<double>();
        }
    }

    public class ImagesModel
    {
        public string Correlation { get; set; }
        public string Roc { get; set; }
        public string ConfusionMatrix { get; set; }
        public string FeatureImportance { get; set; }
    }

    public class DatasetViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string TargetColumn { get; set; }
        public string TargetDescription { get; set; }
        public int TotalRows { get; set; }
        public int TotalColumns { get; set; }
        public string PreCleaningNote { get; set; }
        public TargetDistributionModel TargetDistribution { get; set; }
        public List<MissingValueItem> MissingValues { get; set; }
        public List<FeatureItem> Features { get; set; }
        public List<LeaderboardItem> Leaderboard { get; set; }
        public ImagesModel Images { get; set; }
        public string AnalysisSummary { get; set; }
        public List<string> Headers { get; set; }
        public List<string[]> SampleRows { get; set; }

        public DatasetViewModel()
        {
            TargetDistribution = new TargetDistributionModel();
            MissingValues = new List<MissingValueItem>();
            Features = new List<FeatureItem>();
            Leaderboard = new List<LeaderboardItem>();
            Images = new ImagesModel();
            Headers = new List<string>();
            SampleRows = new List<string[]>();
        }
    }

    public class ResearchSummary
    {
        public ProjectInfo ProjectInfo { get; set; }
        public DatasetViewModel Dataset1 { get; set; }
        public DatasetViewModel Dataset2 { get; set; }

        public ResearchSummary()
        {
            ProjectInfo = new ProjectInfo();
            Dataset1 = new DatasetViewModel();
            Dataset2 = new DatasetViewModel();
        }
    }

    public class DataTablesResponse
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<List<string>> data { get; set; }
        public List<string> columns { get; set; }

        public DataTablesResponse()
        {
            data = new List<List<string>>();
            columns = new List<string>();
        }
    }
}

