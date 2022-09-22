using GeekShopping.CartAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Data.ValueObjects
{
    [Table("cart_detail")]
    public class CartDetailVO
    {
        public long Id { get; set; }
        public long CartHeaderId { get; set; }        
        public CartHeaderVO CartHeader { get; set; }
        public long ProductId { get; set; }     
        public ProductVO Product { get; set; }      
        public int Count { get; set; }

    }
}
