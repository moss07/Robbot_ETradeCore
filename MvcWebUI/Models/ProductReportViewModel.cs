using Business.Models.Filters;
using Business.Models.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebUI.Models
{
    public class ProductReportViewModel
    {
        public ProductReportViewModel()
        {
            PageNumber = 1;
            OrderByDirectionAscending = true;
        }

        public IEnumerable<ProductReportModel> Products { get; set; }
        public ProductFilterModel Filter { get; set; }
        public SelectList Categories { get; set; }
        public int PageNumber { get; set; }
        public SelectList PageNumbers { get; set; }
        public string OrderByExpression { get; set; }
        public bool OrderByDirectionAscending { get; set; }
    }
}
