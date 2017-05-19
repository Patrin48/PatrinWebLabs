using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PatrinWebLabs
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Request.RequestContext.RouteData.Values["controller"] == null)
                return;
            
            string Controller = (Request.RequestContext.RouteData.Values["controller"] != null ? Request.RequestContext.RouteData.Values["controller"].ToString() : "PanelSpecifications");
            string Action = (Request.RequestContext.RouteData.Values["action"] != null ? Request.RequestContext.RouteData.Values["action"].ToString() : "Index");
            string IP = Request.UserHostAddress;
            string UserName = User.Identity.Name;
            string GUID = Guid.NewGuid().ToString();
            Logger.Log.Info(" " + Controller + " " + Action + " " + IP + " " + UserName + " " + GUID);

            foreach (var item in Request.RequestContext.RouteData.Values)
            {
                if (item.Key.Trim().ToLower() != "controller"
                    && item.Key.Trim().ToLower() != "action")
                {
                    Logger.Log.Info(" " + Controller + " " + Action + " " + IP + " " + UserName + " " + GUID);

                }
            }
            
            foreach (string key in Request.QueryString.Keys)
            {
                string Value = Convert.ToString(Request.QueryString[key]);

                Logger.Log.Info(" " + Controller + " " + Action + " " + IP + " " + UserName + " " + GUID);

            }

            foreach (string key in Request.Form.Keys)
            {
                string Value = Convert.ToString(Request.Form[key]);
                Logger.Log.Info(" " + Controller + " " + Action + " " + IP + " " + UserName + " " + GUID);


            }
        }
    }
}
