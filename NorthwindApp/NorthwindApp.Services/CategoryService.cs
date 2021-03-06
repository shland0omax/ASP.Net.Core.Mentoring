﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindApp.Core.Interfaces;
using NorthwindApp.DAL.Interfaces;
using NorthwindApp.Models;
using NorthwindApp.Services.Interfaces;

namespace NorthwindApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfigurationProvider _configurationProvider;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IConfigurationProvider configurationProvider)
        {
            _categoryRepository = categoryRepository;
            _configurationProvider = configurationProvider;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<byte[]> GetCategoryImageAsync(int id)
        {
            var brokenImage = await _categoryRepository.GetCategoryImageAsync(id);

            return brokenImage?.Skip(_configurationProvider.CategoryImageGarbageSize).ToArray();
        }

        public async Task UploadCategoryImageAsync(int id, byte[] image)
        {
            var garbage = Enumerable.Range(0, _configurationProvider.CategoryImageGarbageSize)
                .Select(x => (byte)0);

            var brokenImage = new List<byte>(garbage);
            brokenImage.AddRange(image);

            await _categoryRepository.UploadCategoryImageAsync(id, brokenImage.ToArray());
        }
    }
}
