using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gradual.Intranet.Contratos.Dados;
using Gradual.Intranet.Contratos.Mensagens;
//using Gradual.Intranet.Servicos.BancoDeDados.Persistencias;
using Gradual.Intranet.Servicos.BancoDeDados.Propriedades.Response;

namespace Gradual.Intranet.Servicos.BancoDeDados.Teste
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

           // ExportaTipo1();
            //ExportaTipo8();

        }

        //private void ExportaTipo1()
        //{
        //    SinacorExportarDbLib Exporta = new SinacorExportarDbLib();
        //    SalvarEntidadeResponse<SinacorExportarInfo> lRetorno = new SalvarEntidadeResponse<SinacorExportarInfo>();
        //    SalvarObjetoRequest<SinacorExportarInfo> lClienteExportacao = new SalvarObjetoRequest<SinacorExportarInfo>();
        //    lClienteExportacao.Objeto = new SinacorExportarInfo();
        //    lClienteExportacao.Objeto.Entrada = new SinacorExportacaoEntradaInfo();

        //    try
        //    {

             


        //        lClienteExportacao.Objeto.Entrada.PrimeiraExportacao = true;
        //        lClienteExportacao.Objeto.Entrada.IdCliente = 54053;
        //        lClienteExportacao.Objeto.Entrada.CdCodigo = null;

        //        try
        //        {
        //            lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);

        //        }
        //        catch (Exception ex) 
        //        {
        //            Console.Write(ex.Message);
        //            Console.ReadKey();
        //        }

        //        if (!lRetorno.Objeto.Retorno.DadosClienteOk)
        //        {
        //            //Fatal
        //            //Cadastro incorreto
        //            //Listar
        //            //lRetorno.Objeto.Retorno.DadosClienteMensagens;
        //        }
        //        if (!lRetorno.Objeto.Retorno.ExportacaoSinacorOk)
        //        {
        //            //Fatal
        //            //Erro no processo Sinacor
        //            //Listar
        //            //lRetorno.Objeto.Retorno.ExportacaoSinacorMensagens;
        //        }

        //        if (!lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk)
        //        {
        //            //Exportado
        //            //Não atualizou nosso sistema
        //            //Listar
        //            //lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroMensagens;
        //        }
        //        if (!lRetorno.Objeto.Retorno.ExportacaoComplementosOk)
        //        {
        //            //Exportado
        //            //Exportou, mas não colocou complementos, os dados precisarão ser alterado no sinacor manualmente
        //            //Listar
        //            //lRetorno.Objeto.Retorno.ExportacaoComplementosMensagens;
        //        }


               

        //        if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        {

        //        }
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 925; lClienteExportacao.Objeto.Entrada.CdCodigo = 91251;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        ////Falta endereco Comercial
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 926; lClienteExportacao.Objeto.Entrada.CdCodigo = 216; 
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{

        //        ////}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 927; lClienteExportacao.Objeto.Entrada.CdCodigo = 5228;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 928; lClienteExportacao.Objeto.Entrada.CdCodigo = 3007;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        ////Endereço Comercial
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 929; lClienteExportacao.Objeto.Entrada.CdCodigo = 3037; 
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////  
        //        ////}

        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 930; lClienteExportacao.Objeto.Entrada.CdCodigo = 1037;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 931; lClienteExportacao.Objeto.Entrada.CdCodigo = 7555;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 932; lClienteExportacao.Objeto.Entrada.CdCodigo = 886;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        ////sem conta bancária
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 933; lClienteExportacao.Objeto.Entrada.CdCodigo = 7885;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{

        //        ////}
        //        //////Endereço comercial
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 934; lClienteExportacao.Objeto.Entrada.CdCodigo = 7473;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{

        //        ////}

        //        ////Digito da conta bancária nulo
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 935; lClienteExportacao.Objeto.Entrada.CdCodigo = 7016;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}

        //        ////Endereço comercial e conta em Banco
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 936; lClienteExportacao.Objeto.Entrada.CdCodigo = 1321;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}

        //        ////Endereço comercial
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 937; lClienteExportacao.Objeto.Entrada.CdCodigo = 7002;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 938; lClienteExportacao.Objeto.Entrada.CdCodigo = 1395;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        ////Conta Bancária
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 939; lClienteExportacao.Objeto.Entrada.CdCodigo = 2933; 
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}

        //        ////Endereço comercial
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 940; lClienteExportacao.Objeto.Entrada.CdCodigo = 465;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}

        //        ////Endereço comercial
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 941; lClienteExportacao.Objeto.Entrada.CdCodigo = 6779; 
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}

        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 942; lClienteExportacao.Objeto.Entrada.CdCodigo = 6990;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 943; lClienteExportacao.Objeto.Entrada.CdCodigo = 219;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        //////Sem conta bancária
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 944; lClienteExportacao.Objeto.Entrada.CdCodigo = 309; 
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{

        //        ////}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 945; lClienteExportacao.Objeto.Entrada.CdCodigo = 2938;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 946; lClienteExportacao.Objeto.Entrada.CdCodigo = 6880;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        //////Endereço comercial e conta bancária
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 947; lClienteExportacao.Objeto.Entrada.CdCodigo = 7107; 
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{
        //        ////
        //        ////}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 948; lClienteExportacao.Objeto.Entrada.CdCodigo = 472;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 949; lClienteExportacao.Objeto.Entrada.CdCodigo = 6735;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 950; lClienteExportacao.Objeto.Entrada.CdCodigo = 266;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        //////Endereço Comercial e conta Bancária
        //        ////lClienteExportacao.Objeto.Entrada.IdCliente = 951; lClienteExportacao.Objeto.Entrada.CdCodigo = 2347;
        //        ////lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        ////if (! (lRetorno.Objeto.Retorno.DadosClienteOk  && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk  && lRetorno.Objeto.Retorno.ExportacaoComplementosOk  && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        ////{

        //        ////}


        //    }
        //    catch (Exception ex)
        //    {
        //        string x = ex.Message;
        //    }
        //}

        //private void ExportaTipo8()
        //{
        //    SinacorExportarDbLib Exporta = new SinacorExportarDbLib();
        //    SalvarEntidadeResponse<SinacorExportarInfo> lRetorno = new SalvarEntidadeResponse<SinacorExportarInfo>();
        //    SalvarObjetoRequest<SinacorExportarInfo> lClienteExportacao = new SalvarObjetoRequest<SinacorExportarInfo>();
        //    lClienteExportacao.Objeto = new SinacorExportarInfo();
        //    lClienteExportacao.Objeto.Entrada = new SinacorExportacaoEntradaInfo();

        //    try
        //    {


        //        ///TODO GUSTAVO testar exportação de de tipo 8 com emitente
        //        ///Limpar e inserir na tsccliemitordem


        //        lClienteExportacao.Objeto.Entrada.PrimeiraExportacao = false;

        //        lClienteExportacao.Objeto.Entrada.IdCliente = 47521;//        04437399000123  
        //        //Emitente - re-testar
        //        lClienteExportacao.Objeto.Entrada.CdCodigo = 21357;
        //        lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        {

        //        }

        //        lClienteExportacao.Objeto.Entrada.IdCliente = 47259;//        02158505000104  
        //        lClienteExportacao.Objeto.Entrada.CdCodigo = 7051;
        //        lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        {

        //        }



        //        ////Falta Conta Bancária
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 323;//       07660870000136  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 8525;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        ////Conta Banária
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 324;//        07841995000162  

        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 99142;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        ////Conta Bancária
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 326;//        04547305000179  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 1193;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 328;//        02421508000199  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 7080;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        ////Conta Bancária
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 329;//        05834228000109  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 92945;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}

        //        ////Conta Bancária
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 330;//        05834242000102  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 2951;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 331;//        68972140000165  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 700;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 332;//        06106337000164  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 93451;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 333;//        06192870000196  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 93457;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 334;//        71741581000160  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 2927;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 335;//        05788154000103  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 113085;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        ////Conta Bancária
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 336;//        05661072000101  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 2543;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 337;//        05946740000139  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 93085;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 338;//        06910073000105  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 4092;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 339;//        74162926000110  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 4185;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 340;//        67148817000182  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 4162;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 341;//        71735815000167  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 93774;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 342;//        05645005000195 
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 5045;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 343;//        04320872000198  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 4073;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 344;//        07190814000185  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 95415;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 345;//        07786157000133 
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 8851;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 346;//        08155398000147  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 99976;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 347;//        07944128000152 
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 9537;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 348;//        07916907000144  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 9456;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 349;//        07842450000170 
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 9141;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}
        //        //lClienteExportacao.Objeto.Entrada.IdCliente = 350;//        01627288000191  
        //        //lClienteExportacao.Objeto.Entrada.CdCodigo = 10193;
        //        //lRetorno = Exporta.SinacorExportarCliente(lClienteExportacao);
        //        //if (!(lRetorno.Objeto.Retorno.DadosClienteOk && lRetorno.Objeto.Retorno.ExportacaoAtualizarCadastroOk && lRetorno.Objeto.Retorno.ExportacaoComplementosOk && lRetorno.Objeto.Retorno.ExportacaoSinacorOk))
        //        //{

        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        string x = ex.Message;
        //    }
        //}

        //private void ExportaTipo18() { }

    }
}
