using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportsStore.Infrastructure;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        [JsonIgnore] // Session property will be ignored when serializing
        public ISession Session { get; set; } // in order to add or remove product from the session, so we need to store the current session inside a variable.

        // one session will only have one cart
        public static Cart GetCart(IServiceProvider services)
        {
            // give us the access to the current session in order to get the cart info
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            SessionCart cart = session.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;

            return cart;
        }

        public override void AddItem(Product product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this); // save this sessioncart to the session
        }

        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this); // save this sessioncart to the session
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
