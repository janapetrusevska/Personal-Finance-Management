using AutoMapper;
using PersonalFinanceManagement.Database.Entities;
using PersonalFinanceManagement.Database.Repository;
using PersonalFinanceManagement.Models;
using PersonalFinanceManagement.Models.Category;
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

        public async Task<PagedSortedList<Category>> GetCategories(string parentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> ImportCategories(List<Category> categories)
        {
            List<CategoryEntity> categoryEntities = _mapper.Map<List<CategoryEntity>>(categories);

            await _repository.ImportCategories(categoryEntities);

            return categories;
        }
    }
}
