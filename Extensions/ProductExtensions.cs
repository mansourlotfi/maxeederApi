﻿using ecommerceApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ecommerceApi.Extensions
{
    public static class ProductExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);
            query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }

        public static IQueryable<Product> Search(this IQueryable<Product> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;

            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string? brands, string? category,string? subCategory, string? size,string? usage,bool? isActive,bool? showPrice)
        {
            var brandList = new List<string>();
            var categoryList = new List<string>();
            var subCategoryList = new List<string>();
            var sizeList = new List<string>();
            var usageList = new List<string>();
    

            if (!string.IsNullOrEmpty(brands))
                brandList.AddRange(brands.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(category))
                categoryList.AddRange(category.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(subCategory))
                subCategoryList.AddRange(subCategory.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(size))
                sizeList.AddRange(size.ToLower().Split(",").ToList());

            if (!string.IsNullOrEmpty(usage))
                usageList.AddRange(usage.ToLower().Split(",").ToList());



            query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()));
            query = query.Where(p => categoryList.Count == 0 || categoryList.Contains(p.SubCategory.Category.Name.ToLower()));
            query = query.Where(p => subCategoryList.Count == 0 || subCategoryList.Contains(p.SubCategory.Name.ToLower()));
            query = query.Where(p => sizeList.Count == 0 || sizeList.Contains(p.Size.ToLower()));
            query = query.Where(p => usageList.Count == 0 || usageList.Contains(p.Usage.ToLower()));
            query = query.Where(p => isActive == null || p.IsActive == isActive);
            query = query.Where(p => showPrice == null || p.ShowPrice == showPrice);


            return query;

        }
    }
}