using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Collections.Concurrent;
using Gradual.OMS.WsIntegracao.Arena.Models;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace Gradual.OMS.WsIntegracao.Arena.Controllers
{
    //[EnableCors(origins: "http://localhost", headers: "*", methods:"*")]
    public class EventosController : BaseController
    {
        #region Properties
        private Thread _ThreadQueueSendMessage = null;
        private bool _KeepRunning;
        private object _Singleton = new object();
        private ConcurrentQueue<Evento> _QueueEventos = new ConcurrentQueue<Evento>();
        #endregion

        #region Eventos
        // GET api/listaeventos
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/listaeventos/5
        public string Get(Evento pEvento)
        {
            return pEvento.ShootEventPost().ToString();
            //return "value";
        }

        // POST api/listaeventos
        public Task<string> Post([FromBody]Evento value)
        {
            return Task.Run(() =>
            {
                return value.ShootEventPost();
            });
            //ShootEventQueued(value);
        }

        // PUT api/listaeventos/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/listaeventos/5
        public void Delete(int id)
        {
        }
        #endregion

        #region Construtores
        public EventosController()
        {
            try
            {
                /*
                _KeepRunning = true;

                _ThreadQueueSendMessage = new Thread(new ThreadStart(QueueEventoHandler));

                _ThreadQueueSendMessage.Start();
                 * */
            }
            catch (Exception ex)
            {
                gLogger.Error(ex);
            }
        }
        #endregion

        #region Metodos
        private void ShootEventQueued(Evento pEvento)
        {
            try
            {
                _QueueEventos.Enqueue(pEvento);
            }
            catch (Exception ex)
            {
                
            }
        }
        private void QueueEventoHandler()
        {
            gLogger.Info("Entrou na função de Enviar Messagens enfileiradas");

            while (_KeepRunning || !_QueueEventos.IsEmpty)
            {
                try
                {
                    Evento lEvento;

                    if (_QueueEventos.TryPeek(out lEvento))
                    {
                        lEvento.ShootEventPost();
                        //lMessage.Session.Send(lMessage.MessageByte, 0, lMessage.MessageByte.Length);
                        //lMessage.Session.Send(lMessage.MessageString);
                        while (!_QueueEventos.TryDequeue(out lEvento)) ;

                        gLogger.InfoFormat("Quantidade na fila: [{0}] ", _QueueEventos.Count);

                        continue;
                    }

                    //lock (_Singleton)
                    //{
                    //    Monitor.Wait(_Singleton, 150);
                    //}
                }
                catch (Exception ex)
                {
                    gLogger.Error("Erro encontrado no método QueueSendMessage -> ", ex);
                }
            }
        }
        #endregion


    }

    public class MethodOverrideHandler : DelegatingHandler
    {
        readonly string [] _methods = {"DELETE", "HEAD", "PUT"};

        const string _header = "X-HTTO-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post && request.Headers.Contains(_header))
            {
                var method = request.Headers.GetValues(_header).FirstOrDefault();

                if (_methods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    request.Method = new HttpMethod(method);
                }
            }

            var lAsync = base.SendAsync(request, cancellationToken);

            return lAsync;
        }
    }
}
