using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gradual.OMS.Alertas.Lib.Dados;
using log4net;

namespace Gradual.OMS.AlertasCliente
{
    public class MemoriaAlertas
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, DadosAlerta> alertas;
        private Dictionary<string, HashSet<string>> clienteVsAlertas;


        public MemoriaAlertas()
        {
            alertas = new Dictionary<string,DadosAlerta>();
            clienteVsAlertas = new Dictionary<string, HashSet<string>>();
        }

        public Dictionary<String, DadosAlerta> VerificarAlertas(String idCliente)
        {
            Dictionary<String, DadosAlerta> listaAlertas = new Dictionary<string, DadosAlerta>();

            if (clienteVsAlertas.ContainsKey(idCliente))
            {
                foreach (string idAlerta in clienteVsAlertas[idCliente])
                {
                    if (alertas.ContainsKey(idAlerta))
                    {
                        listaAlertas.Add(idAlerta, alertas[idAlerta]);
                    }
                }
            }

            return listaAlertas;
        }


        public void Limpar()
        {
            alertas.Clear();
        }


        public void AtualizarAlerta(string idAlerta, DadosAlerta alerta)
        {
            lock (alertas)
            {
                if (alertas.ContainsKey(idAlerta))
                {
                    alertas[idAlerta] = alerta;
                }
                else
                {
                    alertas.Add(idAlerta, alerta);
                }

                // Adiciona em clienteVsAlertas
                HashSet<string> alertasCliente;
                if (clienteVsAlertas.ContainsKey(alerta.IdCliente))
                    alertasCliente = clienteVsAlertas[alerta.IdCliente];
                else
                {
                    alertasCliente = new HashSet<string>();
                    clienteVsAlertas.Add(alerta.IdCliente, alertasCliente);
                }

                alertasCliente.Add(idAlerta);
            }
        }

        public void Excluir(string chave)
        {
            lock (alertas)
            {
                if (alertas.ContainsKey(chave))
                {
                    string cliente = alertas[chave].IdCliente;

                    if (clienteVsAlertas.ContainsKey(cliente))
                    {
                        HashSet<string> alertasDoCliente = clienteVsAlertas[cliente];
                        if (alertasDoCliente.Contains(chave))
                            alertasDoCliente.Remove(chave);
                    }

                    alertas.Remove(chave);
                }
                else
                {
                    logger.Error("Solicitada exclusão de IdAlerta=[" + chave + "] inexistente");
                }
            }
        }

    }
}
