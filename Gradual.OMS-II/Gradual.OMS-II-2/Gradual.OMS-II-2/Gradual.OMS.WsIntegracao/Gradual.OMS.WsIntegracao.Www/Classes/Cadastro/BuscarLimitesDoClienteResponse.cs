using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.Intranet.Contratos.Dados.Risco;

namespace Gradual.OMS.WsIntegracao
{
    public class BuscarLimitesDoClienteResposta : RespostaBase
    {
        #region Propriedades

        public decimal Limite_CompraAVista_Total       { get; set; }    // (origem idêntica ao valor do item "Limite">Opera a vista>LimiteR$ da intranet)
        public decimal Limite_CompraAVista_Utilizado   { get; set; }    // (origem idêntica ao valor do item "Limite">Opera a vista>Icone "I" azul>Valor Alocado da intranet.)+ um "if" saldo total em c/c < 0  somar o valor do saldo total em c/c
        public decimal Limite_CompraAVista_Disponivel  { get; set; }    // (origem idêntica ao valor do item "Limite">Opera a vista>Icone "I" azul>Valor disponível da intranet.)+ um "if" saldo total em c/c < 0  subtrair o valor do saldo total em c/c

        public decimal Limite_VendaAVista_Total        { get; set; }    // origem idêntica ao valor do item "Limite">Descoberto(Lado esquerdo>LimiteR$ da intranet.
        public decimal Limite_VendaAVista_Utilizado    { get; set; }    // origem idêntica ao valor do item "Limite">Descoberto(lado Esquerdo)>Icone "I" azul>Valor Alocado da intranet.
        public decimal Limite_VendaAVista_Disponivel   { get; set; }    // origem idêntica ao valor do item "Limite">Descoberto(lado Esquerdo)>Icone "I" azul>Valor disponível da intranet.

        public decimal Limite_CompraOpcoes_Total       { get; set; }    // origem idêntica ao valor do item "Limite">Opera Opção>LimiteR$ da intranet.
        public decimal Limite_CompraOpcoes_Utilizado   { get; set; }    // (origem idêntica ao valor do item "Limite">Opera opção>Icone "I" azul>Valor Alocado da intranet.)um "if" saldo total em c/c < 0  somar o valor do saldo total em c/c
        public decimal Limite_CompraOpcoes_Disponivel  { get; set; }    // (origem idêntica ao valor do item "Limite">Opera Opção>Icone "I" azul>Valor disponível da intranet.)+ um "if" saldo total em c/c < 0  subtrair o valor do saldo total em c/c

        public decimal Limite_VendaOpcoes_Total        { get; set; }    // origem idêntica ao valor do item "Limite">Descoberto(Lado Direito)>LimiteR$ da intranet.
        public decimal Limite_VendaOpcoes_Utilizado    { get; set; }    // origem idêntica ao valor do item "Limite">Descoberto(Lado Direito)>Icone "I" azul>Valor Alocado da intranet.
        public decimal Limite_VendaOpcoes_Disponivel   { get; set; }    // origem idêntica ao valor do item "Limite">Descoberto(Lado direito)>Icone "I" azul>Valor disponível da intranet.

        #endregion

        #region Métodos Públicos
        
        public void ReceberDadosDeLimite(List<RiscoLimiteAlocadoInfo> pListaDeLimites)
        {
            /*
             
                 // como está na intranet:
                 
                if (tr5.Parametro.Contains("compra"))
                {
                    if (tr5.Parametro.Contains("vista"))
                    {
                        this.txtCliente_Detalhes_AVista_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_AVista_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_AVista_Limite.Value = tr5.ValorLimite;
                    }
                    else
                    {
                        this.txtCliente_Detalhes_Opcoes_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_Opcoes_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_Opcoes_Limite.Value = tr5.ValorLimite;
                    }
                }
                else
                {
                    if (tr5.Parametro.Contains("vista"))
                    {
                        this.txtCliente_Detalhes_AVista_Descoberto_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_AVista_Descoberto_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_AVista_Descoberto_Limite.Value = tr5.ValorLimite;
                    }
                    else
                    {
                        this.txtCliente_Detalhes_Opcoes_Descoberto_Alocado.Value = tr5.ValorAlocado;
                        this.txtCliente_Detalhes_Opcoes_Descoberto_Disponivel.Value = tr5.ValorDisponivel;
                        this.txtCliente_Detalhes_Opcoes_Descoberto_Limite.Value = tr5.ValorLimite;
                    }
                }
             */

            string lDescParam;

            foreach (RiscoLimiteAlocadoInfo lLimite in pListaDeLimites)
            {
                if(!string.IsNullOrEmpty( lLimite.DsParametro))
                {
                    lDescParam = lLimite.DsParametro.ToLower();
                    
                    if (lDescParam.Contains("compra"))
                    {
                        if (lDescParam.Contains("vista"))
                        {
                            this.Limite_CompraAVista_Total      = lLimite.VlParametro;
                            this.Limite_CompraAVista_Utilizado  = lLimite.VlAlocado;
                            this.Limite_CompraAVista_Disponivel = lLimite.VlDisponivel;
                        }
                        else
                        {
                            this.Limite_CompraOpcoes_Total      = lLimite.VlParametro;
                            this.Limite_CompraOpcoes_Utilizado  = lLimite.VlAlocado;
                            this.Limite_CompraOpcoes_Disponivel = lLimite.VlDisponivel;
                        }
                    }
                    else
                    {
                        if (lDescParam.Contains("vista"))
                        {
                            this.Limite_VendaAVista_Total      = lLimite.VlParametro;
                            this.Limite_VendaAVista_Utilizado  = lLimite.VlAlocado;
                            this.Limite_VendaAVista_Disponivel = lLimite.VlDisponivel;
                        }
                        else
                        {
                            this.Limite_VendaOpcoes_Total      = lLimite.VlParametro;
                            this.Limite_VendaOpcoes_Utilizado  = lLimite.VlAlocado;
                            this.Limite_VendaOpcoes_Disponivel = lLimite.VlDisponivel;
                        }
                    }
                }
            }
        }

        #endregion
    }
}