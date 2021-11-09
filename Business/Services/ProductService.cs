using AppCore.Business.Models.Paging;
using AppCore.Business.Models.Results;
using AppCore.Business.Ordering;
using Business.Models;
using Business.Models.Filters;
using Business.Models.Reports;
using Business.Services.Bases;
using DataAccess.Repositories.Bases;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Business.Services
{
    public class ProductService : IProductService
    {

        private readonly ProductRepositoryBase _productRepository;
        private readonly CategoryRepositoryBase _categoryRepository;
        public ProductService(ProductRepositoryBase productRepository, CategoryRepositoryBase categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public Result Add(ProductModel model)
        {
            try
            {
                if (_productRepository.Query().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()))
                    return new ErrorResult("Product with the same exists!");

                double unitPrice;
                if (!double.TryParse(model.UnitPriceText.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out unitPrice))
                    return new ErrorResult("Unit Price must be a decimal number!");
                model.UnitPrice = unitPrice;

                model.ExpirationDate = null;
                if (!string.IsNullOrWhiteSpace(model.ExpirationDateText))
                    model.ExpirationDate = DateTime.Parse(model.ExpirationDateText, new CultureInfo("en"));

                var entity = new Product()
                {
                    CategoryId = model.CategoryId,
                    Description = model.Description?.Trim(),
                    ExpirationDate = model.ExpirationDate,
                    Name = model.Name.Trim(),
                    StockAmount = model.StockAmount,
                    UnitPrice = model.UnitPrice,
                    ImagePath = model.ImagePath
                };
                _productRepository.Add(entity);
                model.Id = entity.Id; 
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result Delete(int id)
        {
            try
            {
                _productRepository.Delete(id);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _productRepository?.Dispose();
        }

        public IQueryable<ProductModel> GetQuery(Expression<Func<ProductModel, bool>> predicate = null)
        {
            var query = _productRepository.Query("Category").Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                UnitPriceText = p.UnitPrice.ToString(new CultureInfo("en")),
                StockAmount = p.StockAmount,
                ExpirationDate = p.ExpirationDate,
                ExpirationDateText = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("yyyy/MM/dd", new CultureInfo("en")) : "",
                Description = p.Description,
                CategoryId = p.CategoryId,
                Category = new CategoryModel()
                {
                    Name = p.Category.Name,
                    Description = p.Category.Description
                },
                ImagePath = p.ImagePath
            });
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        public Result Update(ProductModel model)
        {
            try
            {
                if (_productRepository.Query().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id))
                    return new ErrorResult("Product with the same exists!");
                double unitPrice;
                if (!double.TryParse(model.UnitPriceText.Replace(",", "."), NumberStyles.Any, new CultureInfo("en"), out unitPrice))
                    return new ErrorResult("Unit Price must be a decimal number!");
                model.UnitPrice = unitPrice;

                model.ExpirationDate = null;
                if (!string.IsNullOrWhiteSpace(model.ExpirationDateText))
                    model.ExpirationDate = DateTime.Parse(model.ExpirationDateText, new CultureInfo("en"));

                var entity = new Product()
                {
                    Id = model.Id,
                    CategoryId = model.CategoryId,
                    Description = model.Description?.Trim(),
                    ExpirationDate = model.ExpirationDate,
                    Name = model.Name.Trim(),
                    StockAmount = model.StockAmount,
                    UnitPrice = model.UnitPrice,
                    ImagePath = model.ImagePath
                };
                _productRepository.Update(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public Result<ProductModel> GetProduct(int id)
        {
            try
            {
                var product = GetQuery().SingleOrDefault(p => p.Id == id);
                if (product == null)
                    return new ErrorResult<ProductModel>("Product not found");
                return new SuccessResult<ProductModel>(product);

            }
            catch (Exception exc)
            {
                return new ExceptionResult<ProductModel>(exc);
            }
        }

        public Result<List<ProductReportModel>> GetReport(ProductFilterModel filter, PageModel page=null, OrderModel order=null)
        {
            try
            {
                var productQuery = _productRepository.Query();
                var categoryQuery = _categoryRepository.Query();
                var query = from p in productQuery
                            join c in categoryQuery
                            on p.CategoryId equals c.Id
                            orderby c.Name, p.Name
                            select new ProductReportModel()
                            {
                                CategoryDescription = c.Description,
                                CategoryName = c.Name,
                                ExpirationDateText = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("MM/dd/yyyy", new CultureInfo("en")) : "",
                                ProductDescription = p.Description,
                                ProductName = p.Name,
                                StockAmount = p.StockAmount,
                                UnitPriceText = "$ " + p.UnitPrice.ToString(new CultureInfo("en")),
                                CategoryId=c.Id,
                                UnitPrice=p.UnitPrice,
                                ExpirationDate=p.ExpirationDate
                            };

                if (order != null && !string.IsNullOrWhiteSpace(order.Expression))
                {
                    switch (order.Expression)
                    {
                        case "Category":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.CategoryName)
                                : query.OrderByDescending(q => q.CategoryName);
                            break;
                        case "Product Name":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.ProductName)
                                : query.OrderByDescending(q => q.ProductName);
                            break;
                        case "Unit Price":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.UnitPrice)
                                : query.OrderByDescending(q => q.UnitPrice);
                            break;
                        case "Stock Amount":
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.StockAmount)
                                : query.OrderByDescending(q => q.StockAmount);
                            break;
                        default:
                            query = order.DirectionAscending
                                ? query.OrderBy(q => q.ExpirationDate)
                                : query.OrderByDescending(q => q.ExpirationDate);
                            break;
                    }
                }

                if (filter.CategoryId != null)
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                    query = query.Where(q => q.ProductName.ToUpper().Contains(filter.ProductName.Trim().ToUpper()));

                if (!string.IsNullOrWhiteSpace(filter.UnitPriceStartText))
                {
                    double unitPriceStart= Convert.ToDouble(filter.UnitPriceStartText.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice >= unitPriceStart);
                }
                if (!string.IsNullOrWhiteSpace(filter.UnitPriceEndText))
                {
                    double unitPriceEnd = Convert.ToDouble(filter.UnitPriceEndText.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
                    query = query.Where(q => q.UnitPrice <= unitPriceEnd);
                }
                
                if (filter.StockAmountStart.HasValue)
                {
                    query = query.Where(q => q.StockAmount >= filter.StockAmountStart.Value);
                }
                if (filter.StockAmountEnd.HasValue)
                {
                    query = query.Where(q => q.StockAmount <= filter.StockAmountEnd.Value);
                }

                if (!string.IsNullOrWhiteSpace(filter.ExpirationDateStartText))
                {
                    DateTime expirationDateStart = DateTime.Parse(filter.ExpirationDateStartText+"00:00:00", new CultureInfo("en"));
                    query = query.Where(q => q.ExpirationDate >= expirationDateStart);
                }
                if (!string.IsNullOrWhiteSpace(filter.ExpirationDateEndText))
                {
                    DateTime expirationDateEnd = DateTime.Parse(filter.ExpirationDateEndText + "23:59:59", new CultureInfo("en"));
                    query = query.Where(q => q.ExpirationDate <= expirationDateEnd);
                }


                if(page != null)
                {
                    page.RecordsCount = query.Count();
                    int skip=(page.PageNumber-1)*page.RecordsPerPageCount;
                    int take = page.RecordsPerPageCount;
                    query = query.Skip(skip).Take(take);
                }

                return new SuccessResult<List<ProductReportModel>>(query.ToList());
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<ProductReportModel>>(exc);
            }

        }
    }
}
