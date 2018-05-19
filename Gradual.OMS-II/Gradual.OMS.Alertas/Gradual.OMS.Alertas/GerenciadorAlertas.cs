using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

using Newtonsoft.Json;

using Gradual.OMS.Alertas.Lib.Dados;

namespace Gradual.OMS.Alertas.Lib
{
    class GerenciadorAlertas
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<string, DadosAlerta> alertas;
        private Dictionary<string, HashSet<string>> clienteVsAlertas;
        private Dictionary<string, HashSet<string>> instrumentoVsAlertas;

        public GerenciadorAlertas()
        {
            alertas = new Dictionary<string,DadosAlerta>();
            clienteVsAlertas = new Dictionary<string, HashSet<string>>();
            instrumentoVsAlertas = new Dictionary<string, HashSet<string>>();
        }

        public DadosAlerta Cadastrar(
            string Chave,
            string IdCliente,
            string Instrumento,
            Operando TipoOperando,
            Operador TipoOperador,
            decimal Valor,
            DateTime dataHoraCadastro)
        {
            DadosAlerta Dados = new DadosAlerta();

            Dados.IdAlerta = Chave;
            Dados.IdCliente = IdCliente;
            Dados.Instrumento = Instrumento;
            Dados.TipoOperando = TipoOperando;
            Dados.TipoOperador = TipoOperador;
            Dados.Valor = Valor;
            Dados.DataCadastro = dataHoraCadastro;

            alertas.Add(Chave, Dados);

            // Adiciona em clienteVsAlertas
            HashSet<string> alertasCliente;
            if (clienteVsAlertas.ContainsKey(IdCliente))
            {
                alertasCliente = clienteVsAlertas[IdCliente];
                alertasCliente.Add(Chave);
            }
            else
            {
                alertasCliente = new HashSet<string>();
                alertasCliente.Add(Chave);
                clienteVsAlertas.Add(IdCliente, alertasCliente);
            }

            // Adiciona em instrumentosVsAlertas
            HashSet<string> alertasInstrumento;
            if (instrumentoVsAlertas.ContainsKey(Instrumento))
            {
                alertasInstrumento = instrumentoVsAlertas[Instrumento];
                alertasInstrumento.Add(Chave);
            }
            else
            {
                alertasInstrumento = new HashSet<string>();
                alertasInstrumento.Add(Chave);
                instrumentoVsAlertas.Add(Instrumento, alertasInstrumento);
            }

            return(Dados);
        }

        public List<DadosAlerta> Checar(string mensagem, Dictionary<String, DadosInstrumento> instrumentosMonitorados)
        {
            List<DadosAlerta> idAlertasAtingidos = new List<DadosAlerta>();

            if (mensagem.Length != TamanhoMensagemAR)
            {
                logger.Error("Recebida mensagem AR com tamanho inválido: [" + mensagem + "]");
                return idAlertasAtingidos;
            }

            String instrumento = mensagem.Substring(InstrumentoPos, InstrumentoTam).Trim();
            Decimal preco = Convert.ToDecimal(mensagem.Substring(PrecoPos, PrecoTam));
            Decimal maximo = Convert.ToDecimal(mensagem.Substring(MaximaPos, MaximaTam));
            Decimal minimo = Convert.ToDecimal(mensagem.Substring(MinimaPos, MinimaTam));
            Decimal oscilacao = Math.Round(Convert.ToDecimal(mensagem.Substring(VariacaoPos, VariacaoTam)), 2);

            bool maximoAtingido = false;
            bool minimoAtingido = false;

            if (instrumentosMonitorados.ContainsKey(instrumento))
            {
                // Atualiza Valores de máximo e mínimo
                if (maximo > instrumentosMonitorados[instrumento].maximo)
                {
                    if (!instrumentosMonitorados[instrumento].maximo.Equals(Decimal.MinValue))
                        maximoAtingido = true;
                    else
                    {
                        if (preco.Equals(maximo))
                            maximoAtingido = true;
                    }

                    instrumentosMonitorados[instrumento].maximo = maximo;
                }
            }
            else
                logger.Error("Dictionary de instrumentos monitorados não contém instrumento [" + instrumento + "]");

            if (instrumentosMonitorados.ContainsKey(instrumento))
            {
                if (minimo < instrumentosMonitorados[instrumento].minimo)
                {
                    if (!instrumentosMonitorados[instrumento].minimo.Equals(Decimal.MaxValue))
                        minimoAtingido = true;
                    else
                    {
                        if (preco.Equals(minimo))
                            minimoAtingido = true;
                    }

                    instrumentosMonitorados[instrumento].minimo = minimo;
                }
            }
            else
                logger.Error("Dictionary de instrumentos monitorados não contém instrumento [" + instrumento + "]");

            if (instrumentoVsAlertas.ContainsKey(instrumento))
            {
                foreach (string idAlerta in instrumentoVsAlertas[instrumento])
                {
                    DadosAlerta dadosAlerta = alertas[idAlerta];

                    if (dadosAlerta.Atingido)
                        continue;

                    if (dadosAlerta.TipoOperando == Operando.Preco)
                    {
                        if (ExecutarOperacao(preco, dadosAlerta.Valor, dadosAlerta.TipoOperador))
                        {
                            dadosAlerta.Atingido = true;
                            dadosAlerta.DataAtingimento = System.DateTime.Now;
                            dadosAlerta.Cotacao = preco;

                            idAlertasAtingidos.Add(dadosAlerta);
                        }
                    }
                    else if (dadosAlerta.TipoOperando == Operando.Maximo)
                    {
                        if (maximoAtingido)
                        {
                            dadosAlerta.Atingido = true;
                            dadosAlerta.DataAtingimento = System.DateTime.Now;
                            dadosAlerta.Cotacao = preco;

                            idAlertasAtingidos.Add(dadosAlerta);
                        }
                    }
                    else if (dadosAlerta.TipoOperando == Operando.Minimo)
                    {
                        if (minimoAtingido)
                        {
                            dadosAlerta.Atingido = true;
                            dadosAlerta.DataAtingimento = System.DateTime.Now;
                            dadosAlerta.Cotacao = preco;

                            idAlertasAtingidos.Add(dadosAlerta);
                        }
                    }
                    else if (dadosAlerta.TipoOperando == Operando.Oscilacao)
                    {
                        if (ExecutarOperacao(oscilacao, dadosAlerta.Valor, dadosAlerta.TipoOperador))
                        {
                            dadosAlerta.Atingido = true;
                            dadosAlerta.DataAtingimento = System.DateTime.Now;
                            dadosAlerta.Cotacao = oscilacao;

                            idAlertasAtingidos.Add(dadosAlerta);
                        }
                    }
                }
            }

            return idAlertasAtingidos;
        }

        public Dictionary<String, DadosAlerta> VerificarAlertas(String idCliente)
        {
            Dictionary<String, DadosAlerta> listaAlertas = new Dictionary<string, DadosAlerta>();

            if (clienteVsAlertas.ContainsKey(idCliente))
            {
                foreach (string idAlerta in clienteVsAlertas[idCliente])
                    listaAlertas.Add(idAlerta, alertas[idAlerta]);
            }

            return listaAlertas;
        }

        public DadosAlerta Excluir(string chave)
        {
            if (alertas.ContainsKey(chave))
            {
                string instrumento = alertas[chave].Instrumento;
                if (instrumentoVsAlertas.ContainsKey(instrumento))
                {
                    HashSet<string> alertasDoInstrumento = instrumentoVsAlertas[instrumento];
                    if (alertasDoInstrumento.Contains(chave))
                        alertasDoInstrumento.Remove(chave);
                }

                string cliente = alertas[chave].IdCliente;
                if (clienteVsAlertas.ContainsKey(cliente))
                {
                    HashSet<string> alertasDoCliente = clienteVsAlertas[cliente];
                    if (alertasDoCliente.Contains(chave))
                        alertasDoCliente.Remove(chave);
                }

                DadosAlerta alertaExcluido = alertas[chave];

                alertas.Remove(chave);

                return alertaExcluido;
            }
            else
            {
                logger.Error("Solicitada exclusão de IdAlerta=[" + chave + "] inexistente");
                return null;
            }
        }

        public void Carregar(Dictionary<String, DadosAlerta> dadosDbAlerta)
        {
            foreach (KeyValuePair<String, DadosAlerta> kvAlerta in dadosDbAlerta)
            {
                if (alertas.ContainsKey(kvAlerta.Key))
                    alertas[kvAlerta.Key] = kvAlerta.Value;
                else
                    alertas.Add(kvAlerta.Key, kvAlerta.Value);

                // Adiciona em clienteVsAlertas
                HashSet<string> alertasCliente;
                if (clienteVsAlertas.ContainsKey(kvAlerta.Value.IdCliente))
                {
                    alertasCliente = clienteVsAlertas[kvAlerta.Value.IdCliente];
                    alertasCliente.Add(kvAlerta.Key);
                }
                else
                {
                    alertasCliente = new HashSet<string>();
                    alertasCliente.Add(kvAlerta.Key);
                    clienteVsAlertas.Add(kvAlerta.Value.IdCliente, alertasCliente);
                }

                // Adiciona em instrumentosVsAlertas
                HashSet<string> alertasInstrumento;
                if (instrumentoVsAlertas.ContainsKey(kvAlerta.Value.Instrumento))
                {
                    alertasInstrumento = instrumentoVsAlertas[kvAlerta.Value.Instrumento];
                    alertasInstrumento.Add(kvAlerta.Key);
                }
                else
                {
                    alertasInstrumento = new HashSet<string>();
                    alertasInstrumento.Add(kvAlerta.Key);
                    instrumentoVsAlertas.Add(kvAlerta.Value.Instrumento, alertasInstrumento);
                }

            }

            return;
        }

        public HashSet<String> ObterInstrumentos()
        {
            HashSet<String> instrumentos = new HashSet<String>();

            foreach (KeyValuePair<String, DadosAlerta> kvAlerta in alertas)
            {
                instrumentos.Add(kvAlerta.Value.Instrumento);
            }

            return instrumentos;
        }

        public DadosAlerta MarcarComoExibido(String idAlerta)
        {
            DadosAlerta alertaMarcado = null;

            if (alertas.ContainsKey(idAlerta))
            {
                alertas[idAlerta].Exibido = true;
                alertaMarcado = alertas[idAlerta];    
            }
            else
            {
                logger.Error("Solicitado MarcarComoExibido de IdAlerta=[" + idAlerta + "] inexistente");
            }

            return alertaMarcado;
        }

        public DateTime ObterDataAtingimento(String idAlerta)
        {
            DateTime dataAtingimento;
            if (alertas.Keys.Contains(idAlerta))
                dataAtingimento = alertas[idAlerta].DataAtingimento;
            else
                dataAtingimento = DateTime.MinValue;

            return dataAtingimento;
        }

        public Decimal ObterCotacao(String idAlerta)
        {
            Decimal dataAtingimento;
            if (alertas.Keys.Contains(idAlerta))
                dataAtingimento = alertas[idAlerta].Cotacao;
            else
                dataAtingimento = Decimal.MinusOne;

            return dataAtingimento;
        }

        public void Limpar()
        {
            alertas.Clear();
        }

        private bool ExecutarOperacao(Decimal valorCotacao, Decimal valorCadastro, Operador operador)
        {
            bool retorno = false;

            if (operador == Operador.MaiorIgual)
                retorno = (valorCotacao >= valorCadastro);
            else if (operador == Operador.MenorIgual)
                retorno = (valorCotacao <= valorCadastro);

            return retorno;
        }

        public string GerarSnapshot()
        {
            string alertasSerializado = JsonConvert.SerializeObject(alertas);

            return alertasSerializado;
        }

        private const int InstrumentoPos = 21;
        private const int InstrumentoTam = 20;
        private const int PrecoPos = 42;
        private const int PrecoTam = 23;
        private const int MaximaPos = 65;
        private const int MaximaTam = 23;
        private const int MinimaPos = 88;
        private const int MinimaTam = 23;
        private const int VariacaoPos = 111;
        private const int VariacaoTam = 23;
        private const int TamanhoMensagemAR = 134;


/*
    Resposta de Acompanhamento para Alerta (AR)

    Cabeçalho
    Nome                        Tipo    Tamanho Observação
    A - Tipo de Mensagem        A       2       "AR"
    B - Tipo de Bolsa           A       2       "  ", "BV" (Bovespa), "BF" (BM&F)
    C - Data                    N       8       Formato AAAAMMDD
    D - Hora                    N       9       Formato HHMMSSmmm (mmm = milisegundos)
    E - Código do Instrumento   A       20  
    
    Corpo   
    Nome                        Tipo    Tamanho Observação
    F - TipoResposta            N       1	    0 - Requisição OK
                                                1 - Preço
                                                2 - Oscilação
                                                9 - Erro na requisição
    G - Preço                   N       23
    H - Máxima                  N       23
    I - Mínima                  N       23
    J - Variação                N       23

                                                                                                    1         1         1         1         1
          1         2         3         4         5         6         7         8         9         0         1         2         3         4
012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
AABBCCCCCCCCDDDDDDDDDEEEEEEEEEEEEEEEEEEEEFGGGGGGGGGGGGGGGGGGGGGGGHHHHHHHHHHHHHHHHHHHHHHHIIIIIIIIIIIIIIIIIIIIIIIJJJJJJJJJJJJJJJJJJJJJJJ
*/

    }
}
