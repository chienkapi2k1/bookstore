using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class ThongTinController : Controller
    {
        // GET: ThongTin
        BookStoreEntities db = new BookStoreEntities();
        public ActionResult VeChungToi()
        {
            ThongTinHeThong ttht = db.ThongTinHeThong.OrderBy(n => n.MaTT).SingleOrDefault();
            return View(ttht);
        }
    }
}