﻿using Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Client.Services
{
    public interface ICartService
    {
        Dictionary<string, CartItemModel> Cart { get; set; }
        List<ProductModel> CartProducts { get; set; }

        void AddToCart(string sku, CartItemModel cartItemModel);
        bool CartContains(string sku);
        CartItemModel[] GetCartItems();
        void RemoveFromCart(string sku);
        void UpdateCart(string sku, int quantity);
    }

    public class CartService : ICartService
    {

        public List<ProductModel> CartProducts { get; set; } = new List<ProductModel>();
        public Dictionary<string, CartItemModel> Cart { get; set; } = new Dictionary<string, CartItemModel>();

        private readonly IEventService EventsService;

        public CartService(IEventService eventsService)
        {
            EventsService = eventsService;


        }

        public void UpdateCart(string sku, int quantity)
        {
            if (quantity > 0)
            {
                if (!CartContains(sku))
                {
                    AddToCart(sku, new CartItemModel() { ProductSku = sku, Quantity = quantity });
                }
                else
                {
                    Cart[sku].Quantity = quantity;
                }
            }
            else
            {
                if (CartContains(sku))
                    RemoveFromCart(sku);
            }
        }

        public void AddToCart(string sku, CartItemModel cartItemModel)
        {
            Cart.Add(sku, cartItemModel);
            EventsService.NotifyCartUpdated();
        }

        public void RemoveFromCart(string sku)
        {
            if (Cart.ContainsKey(sku))
            {
                Cart.Remove(sku);
                EventsService.NotifyCartUpdated();
            }
        }

        public CartItemModel[] GetCartItems()
        {
            return Cart.Values.ToArray();
        }

        public bool CartContains(string sku)
        {
            return Cart.ContainsKey(sku);
        }



    }
}