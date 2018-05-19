using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gradual.OMS.Contratos.Comum;
using Gradual.OMS.Contratos.Ordens;
using Gradual.OMS.Contratos.Ordens.Dados;
using Gradual.OMS.Library;

namespace Gradual.OMS.Sistemas.CanaisNegociacao
{
    public static class TradutorFix
    {
        public static DateTime TraduzirData(string valor)
        {
            return new DateTime(
                int.Parse(valor.Substring(0, 4)),
                int.Parse(valor.Substring(4, 2)),
                int.Parse(valor.Substring(6, 2)));
        }

        public static OrdemDirecaoEnum TraduzirOrdemDirecao(char valor)
        {
            // Decide
            OrdemDirecaoEnum retorno = OrdemDirecaoEnum.NaoInformado;
            switch (valor)
            {
                case '1':
                    retorno = OrdemDirecaoEnum.Compra;
                    break;
                case '2':
                    retorno = OrdemDirecaoEnum.Venda;
                    break;
                default:
                    Log.EfetuarLog("Gradual.OMS.Sistemas.CanaisNegociacao.TraduzirOrdemDirecao - Direção de ordem não implementado: " + valor.ToString(), LogTipoEnum.Erro, ModulosOMS.ModuloCanais);
                    retorno = OrdemDirecaoEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemMotivoRejeicaoEnum TraduzirOrdemMotivoRejeicao(int valor)
        {
            // Decide
            OrdemMotivoRejeicaoEnum retorno = OrdemMotivoRejeicaoEnum.NaoImplementado;
            switch (valor)
            {
                case 1:
                    retorno = OrdemMotivoRejeicaoEnum.SimboloDesconhecido;
                    break;
                case 2:
                    retorno = OrdemMotivoRejeicaoEnum.ForaHorarioRegular;
                    break;
                case 3:
                    retorno = OrdemMotivoRejeicaoEnum.OrdemExcedeLimite;
                    break;
                case 4:
                    retorno = OrdemMotivoRejeicaoEnum.TardeDemaisParaInserir;
                    break;
                case 5:
                    retorno = OrdemMotivoRejeicaoEnum.OrdemDesconhecida;
                    break;
                case 6:
                    retorno = OrdemMotivoRejeicaoEnum.OrdemDuplicada;
                    break;
                case 11:
                    retorno = OrdemMotivoRejeicaoEnum.CaracteristicaOrdemNaoSuportada;
                    break;
                case 13:
                    retorno = OrdemMotivoRejeicaoEnum.QuantidadeIncorreta;
                    break;
                case 15:
                    retorno = OrdemMotivoRejeicaoEnum.ContaDesconhecida;
                    break;
                case 99:
                    retorno = OrdemMotivoRejeicaoEnum.Outros;
                    break;
                default:
                    Log.EfetuarLog("Gradual.OMS.Sistemas.CanaisNegociacao.TraduzirOrdemMotivoRejeicao - Motivo de rejeição de ordem não implementado: " + valor.ToString(), LogTipoEnum.Erro, ModulosOMS.ModuloCanais);
                    retorno = OrdemMotivoRejeicaoEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemStatusEnum TraduzirOrdemStatus(char valor)
        {
            // Decide
            OrdemStatusEnum retorno = OrdemStatusEnum.NaoImplementado;
            switch (valor)
            {
                case 'A':
                    retorno = OrdemStatusEnum.NovaPendente;
                    break;
                case '0':
                    retorno = OrdemStatusEnum.Nova;
                    break;
                case '1':
                    retorno = OrdemStatusEnum.ParcialmenteExecutada;
                    break;
                case '2':
                    retorno = OrdemStatusEnum.Executada;
                    break;
                case '4':
                    retorno = OrdemStatusEnum.Cancelada;
                    break;
                case '5':
                    retorno = OrdemStatusEnum.Substituida;
                    break;
                case '6':
                    retorno = OrdemStatusEnum.CancelamentoPendente;
                    break;
                case '8':
                    retorno = OrdemStatusEnum.Rejeitada;
                    break;
                case '9':
                    retorno = OrdemStatusEnum.Suspenso;
                    break;
                default:
                    Log.EfetuarLog("Gradual.OMS.Sistemas.CanaisNegociacao.TraduzirOrdemStatus - Status de ordem não implementado: " + valor.ToString(), LogTipoEnum.Erro, ModulosOMS.ModuloCanais);
                    retorno = OrdemStatusEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemTipoEnum TraduzirOrdemTipo(char valor)
        {
            // Decide
            OrdemTipoEnum retorno = OrdemTipoEnum.NaoImplementado;
            switch (valor)
            {
                case '2':
                    retorno = OrdemTipoEnum.Limitada;
                    break;
                case '4':
                    retorno = OrdemTipoEnum.StopLimitada;
                    break;
                case 'K':
                    retorno = OrdemTipoEnum.MarketWithLeftOverLimit;
                    break;
                default:
                    Log.EfetuarLog("Gradual.OMS.Sistemas.CanaisNegociacao.TraduzirOrdemTipo - Tipo de ordem não implementado: " + valor.ToString(), LogTipoEnum.Erro, ModulosOMS.ModuloCanais);
                    retorno = OrdemTipoEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemValidadeEnum TraduzirOrdemValidade(char valor)
        {
            // Decide
            OrdemValidadeEnum retorno = OrdemValidadeEnum.NaoImplementado;
            switch (valor)
            {
                case '0':
                    retorno = OrdemValidadeEnum.ValidaParaODia;
                    break;
                case '3':
                    retorno = OrdemValidadeEnum.ExecutaIntegralParcialOuCancela;
                    break;
                case '4':
                    retorno = OrdemValidadeEnum.ExecutaIntegralOuCancela;
                    break;
                default:
                    Log.EfetuarLog("Gradual.OMS.Sistemas.CanaisNegociacao.TraduzirOrdemValidade - Validade de ordem não implementado: " + valor.ToString(), LogTipoEnum.Erro, ModulosOMS.ModuloCanais);
                    retorno = OrdemValidadeEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }

        public static OrdemTipoExecucaoEnum TraduzirOrdemTipoExecucao(char valor)
        {
            // Decide
            OrdemTipoExecucaoEnum retorno = OrdemTipoExecucaoEnum.NaoImplementado;
            switch (valor)
            {
                case '0':
                    retorno = OrdemTipoExecucaoEnum.Nova;
                    break;
                case '2':
                    retorno = OrdemTipoExecucaoEnum.Preenchimento;
                    break;
                case '4':
                    retorno = OrdemTipoExecucaoEnum.CancelamentoOferta;
                    break;
                case '5':
                    retorno = OrdemTipoExecucaoEnum.Substituicao;
                    break;
                case '6':
                    retorno = OrdemTipoExecucaoEnum.CancelamentoPendente;
                    break;
                case '8':
                    retorno = OrdemTipoExecucaoEnum.Rejeicao;
                    break;
                case '9':
                    retorno = OrdemTipoExecucaoEnum.Suspenso;
                    break;
                case 'F':
                    retorno = OrdemTipoExecucaoEnum.Negocio;
                    break;
                case 'H':
                    retorno = OrdemTipoExecucaoEnum.CancelamentoNegocio;
                    break;
                case 'C':
                    retorno = OrdemTipoExecucaoEnum.TerminoValidade;
                    break;
                case 'D':
                    retorno = OrdemTipoExecucaoEnum.Reconfirmacao;
                    break;
                case 'A':
                    retorno = OrdemTipoExecucaoEnum.NovaPendente;
                    break;
                default:
                    Log.EfetuarLog("Gradual.OMS.Sistemas.CanaisNegociacao.TraduzirOrdemTipoExecucao - Tipo de execução não implementado: " + valor.ToString(), LogTipoEnum.Erro, ModulosOMS.ModuloCanais);
                    retorno = OrdemTipoExecucaoEnum.NaoImplementado;
                    break;
            }

            // Retorna
            return retorno;
        }
    }
}
