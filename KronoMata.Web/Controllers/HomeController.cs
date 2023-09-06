using KronoMata.Data;
using KronoMata.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace KronoMata.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtector _dataProtector;

        public HomeController(ILogger<HomeController> logger, IDataStoreProvider dataStoreProvider,
            IConfiguration configuration, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            DataStoreProvider = dataStoreProvider;
            Configuration = configuration;
            _dataProtector = dataProtectionProvider.CreateProtector("KronoMata.Web.v1");
        }

        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                ViewName = "Dashboard"
            };

            try
            {
                var now = DateTime.Now.Date;

                model.Plugins = DataStoreProvider.PluginMetaDataDataStore.GetAll();
                model.Hosts = DataStoreProvider.HostDataStore.GetAll();
                model.ScheduledJobs = DataStoreProvider.ScheduledJobDataStore.GetAll();
            }
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            var model = new LoginViewModel();

            try
            {
                var adminUser = DataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName("Login", "Username");
                var adminPassword = DataStoreProvider.GlobalConfigurationDataStore.GetByCategoryAndName("Login", "Password");

                try
                {
                    adminPassword.Value = _dataProtector.Unprotect(adminPassword.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Unable to decrypt Global Configuration Value for Category {adminPassword.Category}, Name {adminPassword.Name}. {ex.Message}"
                    , adminPassword.Category, adminPassword.Name, ex.Message);
                }

                if ((username == adminUser.Value) && (password == adminPassword.Value))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    LogMessage(model, NotificationMessageType.Error, "Invalid credentials provided.", "The credentials provided are invalid for this instance of KronoMata.");
                }
            } 
            catch (Exception ex)
            {
                LogException(model, ex);
                _logger.LogError(ex, "Error loading data for View {viewname}", model.ViewName);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");
        }
    }
}