﻿using DemoApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Data
{
    public class DemoAppRepository : IDemoAppRepository
    {
        private readonly DemoAppContext _ctx;
        private readonly ILogger<DemoAppRepository> _logger;

        public DemoAppRepository(DemoAppContext ctx, ILogger<DemoAppRepository> logger)
        {
           _ctx = ctx;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts() called");
                return _ctx.Products
                           .OrderBy(p => p.Title)
                           .ToList();
            }
            catch (Exception ex)
            {

                _logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products
                       .Where(p => p.Category == category)
                       .ToList();
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _ctx.Orders
                         .Include(o => o.Items)
                         .ThenInclude(p => p.Product)
                         .ToList();
            }
            else
            {
                return _ctx.Orders
                           .ToList();
            }
                        
        }

        public Order GetOrderById(int id)
        {
            return _ctx.Orders
                      .Include(o => o.Items)
                      .ThenInclude(p => p.Product)
                      .Where(o => o.Id == id)
                      .FirstOrDefault();
        }

        public IEnumerable<Message> GetMessages(string receiver)
        {
            return _ctx.Messages
                       .Where(m => m.Receiver == receiver)
                       .OrderByDescending(m => m.TimeSent)
                       .ToList();
        }

        public Message GetMessageById(int Id)
        {
            return _ctx.Messages
                       .FirstOrDefault(m => m.Id == Id);
        }


        public void AddEntity(object model)
        {
            _ctx.Add(model);
        } 

       

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        
    }
}
