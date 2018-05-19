using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gradual.OMS.Contratos.Ordens.Dados
{
    /// <summary>
    /// Motivos de rejeicao de ordem
    /// </summary>
    public enum OrdemMotivoRejeicaoEnum
    {
        [Description("Não Informado")]
        NaoInformado,

        [Description("Não Implementado")]
        NaoImplementado,

        [Description("Característica de Ordem não suportada")]
        CaracteristicaOrdemNaoSuportada,

        [Description("Conta Desconhecida")]
        ContaDesconhecida,

        [Description("Fora do Horário Regular")]
        ForaHorarioRegular,

        [Description("Ordem Desconhecida")]
        OrdemDesconhecida,

        [Description("Ordem Duplicada")]
        OrdemDuplicada,

        [Description("Ordem Excede Limite")]
        OrdemExcedeLimite,

        [Description("Outros")]
        Outros,
        
        [Description("Quantidade Incorreta")]
        QuantidadeIncorreta,

        [Description("Símbolo Desconhecido")]
        SimboloDesconhecido,

        [Description("Tarde Demais para Inserir")]
        TardeDemaisParaInserir
    }
}
