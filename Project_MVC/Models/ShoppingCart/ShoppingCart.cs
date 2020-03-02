using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class ShoppingCart
    {
        private Dictionary<string, CartItem> _cartItems = new Dictionary<string, CartItem>();
        private double _totalPrice = 0;

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
        public void AddCart(Product product, int quantity)
        {
            if (_cartItems.ContainsKey(product.Code))
            {
                var item = _cartItems[product.Code];
                item.Quantity += quantity;
                _cartItems[product.Code] = item;
                return;
            }
            var cartItem = new CartItem
            {
                ProductCode = product.Code,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            };
            // đưa cart item tương ứng với sản phẩm (ở trên) vào danh sách.
            _cartItems.Add(cartItem.ProductCode, cartItem);
        }

        public void UpdateCart(Product product, int quantity)
        {
            if (_cartItems.ContainsKey(product.Code))
            {
                var item = _cartItems[product.Code];
                item.Quantity = quantity;
                _cartItems[product.Code] = item;
            }
        }

        public void RemoveFromCart(string productCode)
        {
            _cartItems.Remove(productCode);
        }
    }
}