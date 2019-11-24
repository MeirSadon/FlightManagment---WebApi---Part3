using System.Web;
using System.Web.Mvc;

namespace FlightManagment___WebApi___Part_3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
