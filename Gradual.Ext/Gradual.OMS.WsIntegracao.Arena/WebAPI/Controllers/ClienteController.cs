using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gradual.OMS.WsIntegracao.Arena.Models;
using WebAPI_Test.Models;

namespace Gradual.OMS.WsIntegracao.Arena.Controllers
{
    public class ClienteController : ApiController
    {
        private readonly Cliente[] _Clientes = new Cliente[] 
        { 
            new Cliente { ID = 1, NomeCliente = "Eduardo Pires",   },
            new Cliente { ID = 2, NomeCliente = "Bill Gates",      },
            new Cliente { ID = 3, NomeCliente = "Aleister Crowley", }
        };


        public Cliente[] Get()
        {
            return _Clientes;
        }

        public Cliente Get(int id)
        {
            var clientes = _Clientes;

            return clientes.SingleOrDefault(cli => cli.ID == id);
        }
        /*
        // GET api/cliente
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/cliente/5
        public string Get(int id)
        {
            return "value";
        }
        */

        // POST api/cliente
        public void Post([FromBody]string value)
        {
        }

        // PUT api/cliente/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/cliente/5
        public void Delete(int id)
        {
        }
    }
}
