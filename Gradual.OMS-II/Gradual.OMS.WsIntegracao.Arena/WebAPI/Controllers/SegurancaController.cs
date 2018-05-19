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
    public class SegurancaController : ApiController
    {
        #region propriedades
        private SegurancaServico gSericoSeguranca = new SegurancaServico();  
        #endregion
        // GET api/seguranca
        /*
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */
        // GET api/seguranca/5
        [HttpGet]
        public SegurancaCliente Get([FromBody]SegurancaCliente cliente)
        {
            SegurancaCliente lSeguranca = new SegurancaCliente();
            try
            {
                if (!string.IsNullOrEmpty(cliente.Usuario))
                {
                    lSeguranca = gSericoSeguranca.EfetuarLogin(cliente.Usuario, cliente.Email, cliente.IP, cliente.CodigoSistema, cliente.Senha);
                }
            }
            catch (Exception ex)
            {
            
            }

            return lSeguranca;
        }

        // POST api/seguranca
        public void Post([FromBody]string value)
        {
            
        }

        // PUT api/seguranca/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/seguranca/5
        public void Delete(int id)
        {
        }
    }
}
