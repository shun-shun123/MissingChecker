using System;
using System.Collections.Generic;
using System.Linq;

namespace MissingChecker
{
    [Serializable]
    public class Report 
    {
        public string Title = "";

        public string Time = "";

        public List<CheckResult> Results = new List<CheckResult>();

        public Report()
        {
            Title = "";
            Time = LocalStorageUtility.TimeAsString;
            Results.Clear();
        }

        public Report Diff(Report other)
        {
            return Diff(this, other);
        }

        public static Report Diff(Report before, Report after)
        {
            var beforeResult = before.Results;
            List<CheckResult> diff = new List<CheckResult>(after.Results.Count);
            foreach (var r in after.Results)
            {
                if (beforeResult.Any(i => i.AssetPath == r.AssetPath))
                {
                    continue;
                }
                diff.Add(r);
            }
            return new Report()
            {
                Title = $"{before.Title}_{after.Title}",
                Time = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss"),
                Results = diff
            };
        }
    }
}
