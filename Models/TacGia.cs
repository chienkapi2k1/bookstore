using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class TacGia
    {
        public TacGia(string tacgia)
        {
            this.tacgia = tacgia;
        }
        public string tacgia { get; set; }
        
    }
}