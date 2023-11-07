// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorkLogger.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public string ReturnUrl { get; private set; }
        public async Task<IActionResult> OnGetAsync(
            string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Clear the existing external cookie
            try
            {
                await HttpContext
                    .SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return LocalRedirect("/");
        }
    }
}
