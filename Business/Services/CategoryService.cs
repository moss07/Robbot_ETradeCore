using AppCore.Business.Models.Results;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Repositories.Bases;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepositoryBase _categoryRepository;
        private readonly ProductRepositoryBase _productRepository;

        public CategoryService(CategoryRepositoryBase categoryRepository, ProductRepositoryBase productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }
        public IQueryable<CategoryModel> GetQuery(Expression<Func<CategoryModel, bool>> predicate = null)
        {
            var query= _categoryRepository.Query().OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Id = c.Id,
                Guid = c.Guid,
                Description = c.Description,
                Name = c.Name,
                ProductCount=c.Products.Count
            });
            if (predicate != null)
                query = query.Where(predicate);
            return query;
        }

        public Result Add(CategoryModel model)
        {
            try
            {
                if (_categoryRepository.Query().Any(c => c.Name.ToUpper() == model.Name.Trim()))
                    return new ErrorResult("Category with the same exists!");
                var entity = new Category()
                {
                    Name = model.Name.Trim(),
                    Description = model.Description?.Trim()
                };
                _categoryRepository.Add(entity);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }
        public Result Update(CategoryModel model)
        {
            try
            {
                if(_categoryRepository.Query().Any(c=>c.Name.ToUpper()==model.Name.ToUpper().Trim() && c.Id!=model.Id))
                    return new ErrorResult("Category with the same exists!");
                var entity = new Category()
                {
                    Id = model.Id,
                    Name = model.Name.Trim(),
                    Description = model.Description?.Trim()
                };
                _categoryRepository.Update(entity);
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
                var category = _categoryRepository.EntityQuery(c => c.Id == id, "Products").SingleOrDefault();
                if (category.Products !=null && category.Products.Count>0)
                {
                    return new ErrorResult("While Category has products can't be deleted!");
                }
                _categoryRepository.Delete(category);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }

        public async Task<Result<List<CategoryModel>>> GetCategoriesAsync()
        {
            try
            {
                List<Category> categoryEntities = await _categoryRepository.Query().OrderBy(c => c.Name).ToListAsync();
                List<CategoryModel> categories = categoryEntities.Select(c => new CategoryModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();
                return new SuccessResult<List<CategoryModel>>(categories);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<CategoryModel>>(exc);
            }
        }
    }
}
