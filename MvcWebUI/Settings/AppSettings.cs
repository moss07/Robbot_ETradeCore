using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebUI.Settings
{
    public class AppSettings
    {
        public static string Title { get; set; }
        public static string AcceptedImageExtensions { get; set; }
        public static bool NewUserActive { get; set; }
        public static double AcceptedImageMaximumLength { get; set; }
        public static int RecordsPerPageCount { get; set; }
    }
}
