using System.Web;
using System.Web.Mvc;

namespace Proyecto1_Guaflix_1158116_1171316
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
