using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SportsStore.Infrastructure
{
    // to extend the HTTP, to store user's previous URL before they click the add to cart button
    public static class UrlExtensions
    {
        // be called inside the ProductSummary inside the form tag
        public static string PathAndQuery(this HttpRequest request) =>
            request.QueryString.HasValue
            ? $"{request.Path}{request.QueryString}" // Path: main path for the site; QueryString : the rest part of URL
            : request.Path.ToString(); // if it doesn't have value, just return the main path for the site


    }
}
