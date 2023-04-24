using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

/*
 * Page that user is sent to if they try to access a page they don't have access to
 */

namespace ECommerce.Pages.LoginRegister
{
    public class AccessDeniedModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
