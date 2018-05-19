using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;

namespace Gradual.OMS.WsIntegracao.Arena.Controllers
{
    public class BaseController : ApiController
    {
        #region Propriedades
        public static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Construtores
        public BaseController()
        {
            log4net.Config.XmlConfigurator.Configure();    
        }
        #endregion
        /*
        // GET api/base
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/base/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/base
        public void Post([FromBody]string value)
        {
        }

        // PUT api/base/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/base/5
        public void Delete(int id)
        {
        }
         * */
    }
}
