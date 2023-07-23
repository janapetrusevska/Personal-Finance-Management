using AutoMapper;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceManagement.Service.Implementation
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetCategories(string parentId)
        {
            var categoryList = await _repository.GetCategories(parentId);
            return _mapper.Map<List<Category>>(categoryList);
        }

        public async Task<Category> GetCategoryByCode(string code)
        {
            var category = await _repository.GetCategoryByCode(code);
            return _mapper.Map<Category>(category);
        }

        public async Task<Boolean> ImportCategories(List<Category> categories)
        {
            List<CategoryEntity> categoryEntities = _mapper.Map<List<CategoryEntity>>(categories);

            var result = await _repository.ImportCategories(categoryEntities);

            return result;
        }
    }
}
