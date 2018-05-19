using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gradual.OMS.WsIntegracao.Arena.Models;
using Gradual.OMS.WsIntegracao.Arena.Services;

namespace Gradual.OMS.WsIntegracao.Arena.Controllers
{
    public class ValidaAssinaturaEletronicaController : ApiController
    {
        #region Propriedades
        public SegurancaServico gServicoSeguranca;
        #endregion

        #region Construtores
        public ValidaAssinaturaEletronicaController()
        {
            gServicoSeguranca = new SegurancaServico();
        }
        #endregion

        // GET api/validaassinaturaeletronica
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/validaassinaturaeletronica/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/validaassinaturaeletronica
        public AssinaturaEletronica Post([FromBody]AssinaturaEletronica value)
        {
            return SegurancaServico.ValidarAssinaturaEletronica(value);
        }

        // PUT api/validaassinaturaeletronica/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/validaassinaturaeletronica/5
        public void Delete(int id)
        {
        }
    }
}
