using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntoSport.Models;
using IntoSport.Helpers;
using IntoSport.ViewModels;
using System.Web.Security;
namespace IntoSport.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        public ActionResult AddProduct()
        {
            CartProduct product = new CartProduct();
            int tmp = 0 ;
            int.TryParse(Request.Form["id"], out tmp);
            product.ID = tmp;
            tmp = 0;
            int.TryParse(Request.Form["Quantity"], out tmp);
            product.Quantity = tmp;
            IntoSport.Helpers.ProductHelper productdetails = new IntoSport.Helpers.ProductHelper();
            List<string> keys = new List<string>();
            foreach(KeyValuePair<string,string>details in productdetails.getDetails(product.ID))
            {
                if (!keys.Contains(details.Key))
                {
                    keys.Add(details.Key);
                }
          
                
          
            }
            foreach(string key in keys)
            {
                if (Request.Form[key] != null)
                {
                    DetailWaarde detailwaarde = new DetailWaarde();
                    detailwaarde.waarde = Request.Form.Get(key).Split(',')[1];
                    product.DetailWaardeList.Add(detailwaarde);
                }
            }

           


            Cart cart = Session["cart"] as Cart;
            if (cart != null)
            {
                cart.productList.Add(product);
                Session["cart"] = cart;
            }
            else
            {
                Cart newCart = new Cart();
                newCart.productList.Add(product);
                Session["cart"] = newCart;

            }
            return RedirectToAction("Index");

           
        }
        public ActionResult Index()
        {
            var cart = Session["cart"] as Cart;
            if (cart != null)
            {
                List<CartProductView> CPVList = new List<CartProductView>();
                foreach (CartProduct cartP in cart.productList)
                {
                    CartProductView CPV = new CartProductView();
                    CPV.Product = new Product(cartP.ID);
                    CPV.Hoeveelheid = cartP.Quantity;
                    CPV.DetailWaardeList = cartP.DetailWaardeList;
                    CPVList.Add(CPV);
                }
                ViewData["CPVList"] = CPVList;
            }
            return View();

        }
        
        public ActionResult AddToCart(CartProduct cartP)
        {
            Cart cart = Session["cart"] as Cart;
            if (cart != null)
            {
                cart.productList.Add(cartP);
                Session["cart"] = cart;
            }
            else
            {
                Cart newCart = new Cart();
                newCart.productList.Add(cartP);
                Session["cart"] = newCart;

            }
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Remove()
        {
            string cart_id = Request.QueryString["cart_id"];
            Cart cart = Session["cart"] as Cart;
            cart.productList.RemoveAt(Convert.ToInt32(cart_id));
            Session["cart"] = cart;

            return RedirectToAction("Index");
        }

        public ActionResult Bestel()
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];//.ASPXAUTH
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                string id = authTicket.Name;
                OrderDBController ODBC = new OrderDBController();
                Cart cart = Session["cart"] as Cart;
                ODBC.MakeOrder(id, cart);
                new IntoSport.Helpers.MailerHelper("Bedankt voor uw bestelling bij intosport. Het wordt zo spoeding mogelijk verwerkt.", "Bestelling", IntoSport.Models.User.GetUser(id));
                Session["cart"] = null;
                return RedirectToAction("Success");
            }
            else
            {
                return Redirect("/login");
            }
        }
        public ActionResult Success()
        {
            return View();
        }
    }
}