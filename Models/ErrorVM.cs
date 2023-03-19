using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TechShop.Models
{
    public class ErrorVM
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public static ErrorVM Default()
        {
            return new () {RequestId = Activity.Current?.Id ?? new DefaultHttpContext().TraceIdentifier};
        }
    }
}