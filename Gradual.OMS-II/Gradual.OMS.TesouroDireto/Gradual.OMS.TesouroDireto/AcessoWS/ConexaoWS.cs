using System;
using System.Configuration;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using Gradual.OMS.TesouroDireto.WSDTCompra;
using Gradual.OMS.TesouroDireto.WSTDConsultas;
using Gradual.OMS.TesouroDireto.WSTDVenda;
using Gradual.OMS.TesouroDireto.WSTDInvestidor;

namespace Gradual.OMS.TesouroDireto.AcessoWS
{
    public static class ConexaoWS
    {
        public static consultas WsConsulta
        {
            get
            {
                return new consultas()
                {
                    hdSegurancaValue = ConexaoWS.GetSOAPHeader(EnumWS.Consultas) as WSTDConsultas.hdSeguranca
                };
            }
        }

        public static compra WsCompra
        {
            get
            {
                return new compra()
                {
                    hdSegurancaValue = ConexaoWS.GetSOAPHeader(EnumWS.Compra) as WSDTCompra.hdSeguranca
                };
            }
        }

        public static venda WsVenda
        {
            get
            {
                return new venda()
                {
                    hdSegurancaValue = ConexaoWS.GetSOAPHeader(EnumWS.Venda) as WSTDVenda.hdSeguranca
                };
            }
        }

        public static Investidor WsInvestidor
        {
            get
            {
                return new Investidor()
                {
                    hdSegurancaValue = ConexaoWS.GetSOAPHeader(EnumWS.Investidor) as WSTDInvestidor.hdSeguranca
                };
            }
        }

        private static Object GetSOAPHeader(EnumWS type)
        {
            switch (type)
            {
                case EnumWS.Consultas:
                    return new WSTDConsultas.hdSeguranca()
                    {
                        strContratoSenha = ConfigurationManager.AppSettings["TDHeader_ContratoSenha"],
                        strContratoHash = ConfigurationManager.AppSettings["TDHeader_ContratoHash"],
                        strLoginNome = ConfigurationManager.AppSettings["TDHeader_LoginNome"],
                        strLoginSenha = ConfigurationManager.AppSettings["TDHeader_LoginSenha"]
                    };

                case EnumWS.Compra:
                    return new WSDTCompra.hdSeguranca()
                    {
                        strContratoSenha = ConfigurationManager.AppSettings["TDHeader_ContratoSenha"],
                        strContratoHash = ConfigurationManager.AppSettings["TDHeader_ContratoHash"],
                        strLoginNome = ConfigurationManager.AppSettings["TDHeader_LoginNome"],
                        strLoginSenha = ConfigurationManager.AppSettings["TDHeader_LoginSenha"]
                    };

                case EnumWS.Venda:
                    return new WSTDVenda.hdSeguranca()
                    {
                        strContratoSenha = ConfigurationManager.AppSettings["TDHeader_ContratoSenha"],
                        strContratoHash = ConfigurationManager.AppSettings["TDHeader_ContratoHash"],
                        strLoginNome = ConfigurationManager.AppSettings["TDHeader_LoginNome"],
                        strLoginSenha = ConfigurationManager.AppSettings["TDHeader_LoginSenha"]
                    };

                case EnumWS.Investidor:
                    return new WSTDInvestidor.hdSeguranca()
                    {
                        strContratoSenha = ConfigurationManager.AppSettings["TDHeader_ContratoSenha"],
                        strContratoHash = ConfigurationManager.AppSettings["TDHeader_ContratoHash"],
                        strLoginNome = ConfigurationManager.AppSettings["TDHeader_LoginNome"],
                        strLoginSenha = ConfigurationManager.AppSettings["TDHeader_LoginSenha"]
                    };
                default:
                    return null;
            };
        }
    }
}
