using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDemo.CostSaving
{
    /// <summary>
    /// The feature for calculating cost saving.
    /// </summary>
    class CostSaving : Feature
    {
        /// <summary>
        /// The config for the CostSaving feature.
        /// </summary>
        ConfigCostSaving Cfg = new ConfigCostSaving();

        /// <summary>
        /// The dictionary contains the definition of the cost savings.
        /// </summary>
        Dictionary<string, CostSavingLine> CostSavingDefDict = new Dictionary<string, CostSavingLine>();

        /// <summary>
        /// The output list for the detail.
        /// </summary>
        List<string> DetailList = new List<string>();

        /// <summary>
        /// The dictionary contains the work item counts.
        /// </summary>
        Dictionary<string, double> WorkItemCountDict = new Dictionary<string, double>();

        /// <summary>
        /// The output list for the overall result.
        /// </summary>
        List<string> OverallList = new List<string>();

        /// <summary>
        /// Implement the abstract function Load.
        /// </summary>
        /// <param name="arg">The argument need to be loaded</param>
        protected override void Load(Argument arg)
        {
            Cfg.Load(arg);
        }

        /// <summary>
        /// Implement the abstract function Run.
        /// </summary>
        protected override void Run()
        {
            Logger.WriteLog("Start to build the CostSaving definition dictionary.");
            BuildCostSavingDefDict();
            Logger.WriteLog("Start to merge the CostSaving info.");
            MergeCostSaving();
            Logger.WriteLog("Start to output the result.");
            OutputResult();
            Logger.WriteLog("Result output is done.");
        }

        /// <summary>
        /// Implement the abstract function SetStatusLine.
        /// </summary>
        protected override void SetStatusLine()
        {
            StatusLine.FeatureName = Cfg.FeatureName;
            // Cost saving is not a daily task, there is no need to run it multiple times.
            StatusLine.ItemCount = 0;
        }

        /// <summary>
        /// Output the result.
        /// </summary>
        private void OutputResult()
        {
            Directory.CreateDirectory(Cfg.OutputFolderPath);
            string detailPath = Path.Combine(Cfg.OutputFolderPath, "CostSavingDetail.txt");
            string overallPath = Path.Combine(Cfg.OutputFolderPath, "CostSavingOverall.txt");
            File.WriteAllLines(detailPath, DetailList);
            File.WriteAllLines(overallPath, OverallList);
        }

        /// <summary>
        /// Merge the CostSaving definition, and the CostSaving record.
        /// </summary>
        private void MergeCostSaving()
        {
            var r = CollectCostSavingRecords().ToArray();
            foreach(var record in CollectCostSavingRecords())
            {
                // For each of the CostSaving record, add its work item count to the item count dictionary.
                DetailList.Add(record.Output());
                if (WorkItemCountDict.ContainsKey(record.FeatureName))
                    WorkItemCountDict[record.FeatureName] += record.ItemCount;
                else
                    WorkItemCountDict[record.FeatureName] = record.ItemCount;
            }

            // This key is "CostSaving".
            // Cost saving work item count is always 1(by design).
            WorkItemCountDict[Cfg.FeatureName] = 1;

            foreach(var item in CostSavingDefDict)
            {
                // For each of the CostSaving definition, calculate how many work items are there.
                string featureName = item.Value.FeatureName;
                // If the WorkItemCountDict doesn't contain certain feature, then it means no work itme is ran by it.
                double workItemCount = WorkItemCountDict.ContainsKey(featureName) ? WorkItemCountDict[featureName] : 0;
                // This is the demo, so the CostSaving formula is pretty trival:
                //  TotalCostSaving = CostSavingPerWorkItem * WorkItemCount.
                // It can be more complicate.
                double totalCostSaving = workItemCount * item.Value.Rate;                             
                string reportString = string.Format(item.Value.ReportTemplate, item.Value.Rate, workItemCount);
                string description = item.Value.FeatureDescription;
                /*
                 * Five columns:
                 *  Column1 [FeatureName, th ename of the feature.]
                 *  Column2 [WorkItemCount, how many work item counts are there.]
                 *  Column3 [TotalCostSaving, how many cost savings are there.]
                 *  Column4 [ReportString, shows how exactly the cost saving is calculated.]
                 *  Column5 [Description, the description of the tool.]
                 * 
                 * This is not a fixed format.
                 */
                OverallList.Add(string.Join("\t", featureName, workItemCount, totalCostSaving, reportString, description));
            }
        }

        /// <summary>
        /// Collect all the CostSaving records in the certain time span.
        /// </summary>
        /// <returns>The collection of the CostSaving records.</returns>
        private IEnumerable<TaskStatusLine> CollectCostSavingRecords()
        {
            for(DateTime dt = Cfg.StartTime; dt <= Cfg.EndTime; dt = dt.AddDays(1))
            {
                string dateFolderPath = Path.Combine(FeatureConstants.WORK_STATUS_ARCHIVE_FOLDER, dt.Year.ToString("0000"), dt.Month.ToString("00"), dt.Day.ToString("00"));
                if (Directory.Exists(dateFolderPath))
                {
                    foreach(string archiveFilePath in Directory.EnumerateFiles(dateFolderPath))
                    {
                        TaskStatusLine l = null;
                        try
                        {
                            l = new TaskStatusLine(File.ReadAllText(archiveFilePath));
                        }
                        catch { }
                        if (l != null)
                            yield return l;
                    }
                }
            }
        }

        /// <summary>
        /// Build the CostSaving definition dictionary.
        /// </summary>
        private void BuildCostSavingDefDict()
        {
            var list = IO.ReadEmbed("InfrastructureDemo.Internal.CostSaving.txt", "InfrastructureDemo");
            foreach(string s in list)
            {
                if (s.Length > 0 && s[0] != '#')
                {
                    try
                    {
                        CostSavingDefDict[s.Split('\t')[0]] = new CostSavingLine(s);
                    }
                    catch(Exception e)
                    {
                        Logger.WriteError("This line failed to be added:");
                        Logger.WriteError(s);
                        Logger.WriteError(e.Message);
                    }
                }
            }
        }
    }
}
