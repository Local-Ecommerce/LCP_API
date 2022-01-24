using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Constants;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(LoichDBContext context) : base(context) { }

        
        /// <summary>
        /// Get All Base Product
        /// </summary>
        /// <returns></returns>
        public async Task<List<Product>> GetAllBaseProduct()
        {
            List<Product> products = await _context.Products
                                            .Where(p => p.BelongTo == null && p.Status == (int)ProductStatus.VERIFIED_PRODUCT)
                                            .Include(p => p.InverseBelongToNavigation)
                                            .ToListAsync();

            return products;
        }


        /// <summary>
        /// Get Base Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product> GetBaseProductById(string productId)
        {
            Product product = await _context.Products
                                        .Where(p => p.ProductId.Equals(productId))
                                        .Include(p => p.InverseBelongToNavigation)
                                        .FirstOrDefaultAsync();

            return product;
        }

        /// <summary>
        /// Get Related Product By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product> GetRelatedProductById(string productId)
        {
            Product product = await _context.Products
                                                    .Where(p => p.ProductId.Equals(productId))
                                                    .Include(p => p.BelongToNavigation)
                                                    .FirstOrDefaultAsync();

            return product;
        }


        public async Task<List<Product>> GetPendingProducts()
        {
            List<Product>  products = await _context.Products
                                                              .Where(p => p.Status == (int)ProductStatus.UNVERIFIED_CREATE_PRODUCT 
                                                              || p.Status == (int)ProductStatus.UNVERIFIED_UPDATE_PRODUCT)
                                                              .Include(p => p.BelongToNavigation)
                                                              .OrderByDescending(p => p.CreatedDate)
                                                              .ToListAsync();

            return products;
        }
    }
}