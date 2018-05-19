using System.Web;
using System.Web.Mvc;

namespace Gradual.OMS.WsIntegracao.Arena
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}