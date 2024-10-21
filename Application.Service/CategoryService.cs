using Application.Domain.Interface;
using Application.Domain.Model;
using Application.Domain.Model.Enums;

namespace Application.Service
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<Category> AddAsync(string userId, Category category)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            return await _categoryRepository.AddAsync(userId, category);
        }

        public async Task<bool> DeleteAsync(string userId, string categoryId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentNullException(nameof(categoryId), "Category ID cannot be null or empty.");

            return await _categoryRepository.DeleteAsync(userId, categoryId);
        }

        public async Task<Category> UpdateAsync(string userId, Category category)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (category == null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            return await _categoryRepository.UpdateAsync(userId, category);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            return await _categoryRepository.GetAllAsync(userId);
        }

        public async Task<Category> GetByIdAsync(string userId, string categoryId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(categoryId))
                throw new ArgumentNullException(nameof(categoryId), "Category ID cannot be null or empty.");

            return await _categoryRepository.GetByIdAsync(userId, categoryId);
        }

        public async Task AddDefaultCategoryAsync(string userId)
        {
            var defaultCategories = new List<Category>
            {
                new Category
                {
                    Id = "category-2",
                    Name = "Alimentação",
                    Description = "Despesas relacionadas à alimentação",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-utensils",
                    Background = "#ED1B25"
                },
                new Category
                {
                    Id = "category-3",
                    Name = "Transporte",
                    Description = "Despesas com transporte",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-bus",
                    Background = "#A41A44"
                },
                new Category
                {
                    Id = "category-4",
                    Name = "Lazer",
                    Description = "Despesas com entretenimento",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-plane",
                    Background = "#7A2990"
                },
                new Category
                {
                    Id = "category-5",
                    Name = "Habitação",
                    Description = "Despesas com moradia",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-house",
                    Background = "#392B8A"
                },
                new Category
                {
                    Id = "category-6",
                    Name = "Saúde",
                    Description = "Despesas médicas e hospitalares",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-heartbeat",
                    Background = "#2F5AA8"
                },
                new Category
                {
                    Id = "category-7",
                    Name = "Educação",
                    Description = "Despesas relacionadas à educação",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-graduation-cap",
                    Background = "#018CCD"
                },
                new Category
                {
                    Id = "category-8",
                    Name = "Entretenimento",
                    Description = "Despesas com atividades de lazer",
                    Type = CategoryType.Expense,
                    Icon = "fa fa-film",
                    Background = "#60AF46"
                },
                new Category
                {
                    Id = "category-9",
                    Name = "Salário",
                    Description = "Renda proveniente do trabalho",
                    Type = CategoryType.Income,
                    Icon = "fa fa-money-bill-wave",
                    Background = "#CDDB26"
                },
                new Category
                {
                    Id = "category-10",
                    Name = "Investimentos",
                    Description = "Renda proveniente de investimentos",
                    Type = CategoryType.Income,
                    Icon = "fa fa-chart-line",
                    Background = "#F6ED24"
                },
                new Category
                {
                    Id = "category-11",
                    Name = "Presentes",
                    Description = "Renda proveniente de presentes",
                    Type = CategoryType.Income,
                    Icon = "fa fa-gift",
                    Background = "#F7BA12"
                },
                new Category
                {
                    Id = "category-12",
                    Name = "Rendimentos de Aluguel",
                    Description = "Renda proveniente de aluguel",
                    Type = CategoryType.Income,
                    Icon = "fa fa-home",
                    Background = "#F9971D"
                },
                new Category
                {
                    Id = "category-13",
                    Name = "Vendas",
                    Description = "Renda proveniente de vendas de produtos",
                    Type =  CategoryType.Income,
                    Icon = "fa fa-shopping-cart",
                    Background = "#F14B21"
                },
                new Category
                {
                    Id = "category-14",
                    Name = "Bonificações",
                    Description = "Renda proveniente de bonificações",
                    Type =  CategoryType.Income,
                    Icon = "fa fa-bell",
                    Background = "#F26A6A"
                }
            };


            foreach (var category in defaultCategories)
            {
                await AddAsync(userId, category);
            }
        }
    }
}
