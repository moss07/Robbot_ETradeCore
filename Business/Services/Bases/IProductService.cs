using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using AppCore.Business.Ordering;
using AppCore.Business.Services.Bases;
using Business.Models;
using Business.Models.Filters;
using Business.Models.Reports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services.Bases
{
    public interface IProductService : IService<ProductModel>
    {
        Result<ProductModel> GetProduct(int id); 
        Result<List<ProductReportModel>> GetReport(ProductFilterModel filter, PageModel page = null, OrderModel order = null);
    }
}
