using MyShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        private IBasketService basketService;

        public BasketController(IBasketService BasketService)
        {
            this.basketService = BasketService;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(HttpContext);
            return View(model);
        }

        // GET: Basket/AddToBasket/1
        public ActionResult AddToBasket(string Id)
        {
            basketService.AddToBasket(HttpContext, Id);

            return RedirectToAction("Index");
        }

        // GET: Basket/RemoveFromBasket/1
        public ActionResult RemoveFromBasket(string Id)
        {
            basketService.RemoveFromBasket(HttpContext, Id);

            return RedirectToAction("Index");
        }

        // GET: Basket/BasketSummary
        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(HttpContext);

            return PartialView(basketSummary);
        }
    }
}