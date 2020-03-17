using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class ShoppingCart
    {
        private Dictionary<string, CartItem> _cartItems;
        private double _totalPrice = 0;

        public ShoppingCart()
        {
            _cartItems = new Dictionary<string, CartItem>();
        }

        public double GetTotalPrice()
        {
            this._totalPrice = 0;
            foreach (var item in _cartItems.Values)
            {
                this._totalPrice += item.Price * item.Quantity;
            }
            return this._totalPrice;
        }

        public Dictionary<string, CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public void SetCartItems(Dictionary<string, CartItem> cartItems)
        {
            this._cartItems = cartItems;
        }

        /**
         * Thêm một sản phẩm vào giỏ hàng.
         * Trong trường hợp tồn tại sản phẩm trong giỏ hàng thì update số lượng.
         * Trong trường hợp không tồn tại thì thêm mới.
         */
        public void AddCart(Flower flower, int quantity)
        {
            if (_cartItems.ContainsKey(flower.Code))
            {
                var item = _cartItems[flower.Code];
                item.Quantity += quantity;
                _cartItems[flower.Code] = item;
                return;
            }
            var cartItem = new CartItem
            {
                FlowerCode = flower.Code,
                FlowerName = flower.Name,
                Price = Utility.NewPrice(flower.Price, flower.Discount),
                Quantity = quantity
            };
            // đưa cart item tương ứng với sản phẩm (ở trên) vào danh sách.
            _cartItems.Add(cartItem.FlowerCode, cartItem);
        }

        public void UpdateCart(int[] intQuantites)
        {
            //if (_cartItems.ContainsKey(product.Id))
            //{
            //    var item = _cartItems[product.Id];
            //    item.Quantity = quantity;
            //    _cartItems[product.Id] = item;
            //}
            var lstCartItems = GetCartItems();
            var count = 0;
            foreach (var item in lstCartItems)
            {
                item.Value.Quantity = intQuantites[count];
                count++;
            }

            SetCartItems(lstCartItems);
        }

        public void RemoveFromCart(string productCode)
        {
            _cartItems.Remove(productCode);
        }

        public void Clear()
        {
            _cartItems.Clear();
        }
    }
}