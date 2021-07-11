using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Order
    {
        public int ID { get; set; }

        public int GoodId { get; set; }
        public Goods Good { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; }

        public int StateOrderId { get; set; }
        public StateOrder StateOrder { get; set; }


    }
}
