using System.Collections.Generic;
using System.Web.Mvc;
using PaypalTestApp.Helpers;
using PayPal.Api;

namespace PaypalTestApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string token = CacheManager.Instance.Get<string>(Constants.TOKEN_KEY);
            if (!string.IsNullOrEmpty(token))
            {
                var apiContext = new APIContext(token);

                var invoice = new Invoice
                {
                    merchant_info = new MerchantInfo
                    {
                        email = "vladimir.shirmanov-facilitator@gmail.com",
                        first_name = "Vladimir",
                        last_name = "Shirmanov",
                        business_name = "Shirmanov and co",
                        phone = new Phone
                        {
                            country_code = "380",
                            national_number = "951341464"
                        },
                        address = new InvoiceAddress
                        {
                            line1 = "1234 Main St.",
                            city = "Kharkiv",
                            country_code = "UA"
                        }
                    },
                    billing_info = new List<BillingInfo>
                    {
                        new BillingInfo
                        {
                            email = "vladimir.shirmanov-buyer@gmail.com"
                        }
                    },
                    items = new List<InvoiceItem>
                    {
                        new InvoiceItem
                        {
                            name = "Firework",
                            quantity = 100,
                            unit_price = new Currency
                            {
                                currency = "USD",
                                value = "10"
                            }
                        }
                    }
                };

                var createdInvoice = invoice.Create(apiContext);
                createdInvoice.Send(apiContext);

                this.ViewBag.InvoiceId = createdInvoice.id;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}