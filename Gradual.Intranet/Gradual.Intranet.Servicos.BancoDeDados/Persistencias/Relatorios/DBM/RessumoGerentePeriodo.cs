using System.Data;
using System.Data.Common;
using Gradual.Intranet.Contratos.Dados.Relatorios.DBM;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Request;
using Gradual.OMS.Persistencia;

namespace Gradual.Intranet.Servicos.BancoDeDados.Persistencias.Relatorios.DBM
{
    public class RessumoGerentePeriodo
    {
        public ReceberObjetoResponse<ResumoGerenteinfo> ReceberDadosMesAnterior(ReceberEntidadeRequest<ResumoGerenteinfo> pParametro)
        {
            var lRetorno = new ReceberObjetoResponse<ResumoGerenteinfo>();
            var lAcessaDados = new ConexaoDbHelper();

            lAcessaDados.ConnectionStringName = ClienteDbLib.gNomeConexaoSinacorTrade;

            lRetorno = new ReceberObjetoResponse<ResumoGerenteinfo>();

            lRetorno.Objeto = new ResumoGerenteinfo();


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.StoredProcedure, "PRC_DBM_CORRETAGEM_PERIODO"))
            {

                lAcessaDados.AddInParameter(lDbCommand, "DATA_INICIO"   , DbType.Date, pParametro.Objeto.DataInicial);
                lAcessaDados.AddInParameter(lDbCommand, "DATA_FINAL"    , DbType.Date, pParametro.Objeto.DataFinal  );

                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BVSP_CORRETAGEM", DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BVSP_VOLUME"    , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BMF_CORRETAGEM" , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_PERIODO_BMF_VOLUME"     , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_BMF_BOVESPA_CORRETAGEM" , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_BMF_BOVESPA_VOLUME"     , DbType.Decimal, 0);
                lAcessaDados.AddOutParameter(lDbCommand, "MEDIA_CADASTRO"               , DbType.Decimal, 0);

                lAcessaDados.ExecuteNonQuery(lDbCommand);

                if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BVSP)
                {
                    lRetorno.Objeto.CadastradoIntervaloData = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BVSP_CORRETAGEM"   );
                    lRetorno.Objeto.VolumeIntervaloData     = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BVSP_VOLUME"       );
                    lRetorno.Objeto.CadastradoIntervaloData = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CADASTRO"                  );
                }
                else if (pParametro.Objeto.Mercado == ResumoGerenteinfo.TipoMercado.BMF)
                {
                    lRetorno.Objeto.CadastradoIntervaloData = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BMF_CORRETAGEM"    );
                    lRetorno.Objeto.VolumeIntervaloData     = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_PERIODO_BMF_VOLUME"        );
                    lRetorno.Objeto.CadastradoIntervaloData = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CADASTRO"                  ); //verificar
                }
                else //Bovespa e BMF
                {
                    lRetorno.Objeto.CadastradoIntervaloData = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_BMF_BOVESPA_CORRETAGEM"    );
                    lRetorno.Objeto.VolumeIntervaloData     = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_BMF_BOVESPA_VOLUME"        );
                    lRetorno.Objeto.CadastradoIntervaloData = (decimal)lAcessaDados.GetParameterValue(lDbCommand, "MEDIA_CADASTRO"                  ); //verificar
                }
            }

            return lRetorno;
        }
    }
}
