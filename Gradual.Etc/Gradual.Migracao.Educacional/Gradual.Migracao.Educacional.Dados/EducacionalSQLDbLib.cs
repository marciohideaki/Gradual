using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Migracao.Educacional.Entidades;
using System;

namespace Gradual.Migracao.Educacional.Dados
{
    public class EducacionalSQLDbLib
    {
        private const string gConexaoEducacionalSQL = "ConexaoEducacionalSql";
        Conexao gConexao = new Conexao();
        DbTransaction gDbTransaction;

        public EducacionalCompletoMensagemInfo EducacionalCompleto { get; set; }

        public EducacionalSQLDbLib(EducacionalCompletoMensagemInfo pParametro)
        {
            this.EducacionalCompleto = pParametro;
        }

        public void IniciarMigracao()
        {
            {   //--> Criando a transação.
                gConexao._ConnectionStringName = gConexaoEducacionalSQL;
                var lDbConnection = gConexao.CreateIConnection();
                lDbConnection.Open();
                gDbTransaction = lDbConnection.BeginTransaction();
            }

            try
            {
                this.LimparBase();
                this.MigrarEstado();
                this.MigrarPalestrante();
                this.MigrarNivel();
                this.MigrarCursoPalestraOnline();
                this.MigrarTema();
                this.MigrarPalestraSobMedida();
                this.MigrarPerfil();
                this.MigrarLocalidade();
                this.MigrarUsuario();
                this.MigrarCursoPalestra();
                this.MigrarAvaliacaoPalestra();
                this.MigrarClienteCursoPalestra();
                this.MigrarAvaliacaoInteresse();
                this.MigrarFichaPerfil();

                gDbTransaction.Commit();
            }
            catch (Exception ex)
            {
                gDbTransaction.Rollback();
                throw ex;
            }
            //finally
            //{
            //    lDbTransaction.s
            //}
        }

        private void LimparBase()
        {
            var lAcessaDados = new AcessaDados();
            var lQuery = @"DELETE FROM [dbo].[tb_cliente_curso_palestra];
                           DELETE FROM [dbo].[tb_usuario];
                           DELETE FROM [dbo].[tb_perfil];
                           DELETE FROM [dbo].[tb_curso_palestra_online];
                           DELETE FROM [dbo].[tb_estado];
                           DELETE FROM [dbo].[tb_palestra_sob_medida];
                           DELETE FROM [dbo].[tb_palestrante];
                           DELETE FROM [dbo].[tb_avaliacao_palestra];
                           DELETE FROM [dbo].[tb_curso_palestra];
                           DELETE FROM [dbo].[tb_localidade];
                           DELETE FROM [dbo].[tb_tema];
                           DELETE FROM [dbo].[tb_nivel];
                           DELETE FROM [dbo].[tb_ficha_perfil];
                           DELETE FROM [dbo].[tb_avaliacao_interesse];";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
            {
                lDbCommand.Transaction = gDbTransaction;
                lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection);
            }
        }

        private void MigrarFichaPerfil()
        {
            if (null != this.EducacionalCompleto.FichaPerfilInfo && this.EducacionalCompleto.FichaPerfilInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = @"INSERT INTO [dbo].[tb_ficha_perfil] 
                                      (    id_cliente
                                      ,    ds_faixa_etaria
                                      ,    ds_ocupacao
                                      ,    ds_conhecimento
                                      ,    tp_investidor
                                      ,    tp_investimento
                                      ,    tp_instituicao
                                      ,    ds_renda_familiar
                                      ,    dt_inclusao)
                               VALUES (    @id_cliente
                                      ,    @ds_faixa_etaria
                                      ,    @ds_ocupacao
                                      ,    @ds_conhecimento
                                      ,    @tp_investidor
                                      ,    @tp_investimento
                                      ,    @tp_instituicao
                                      ,    @ds_renda_familiar
                                      ,    @dt_inclusao);
                               SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.FichaPerfilInfo.ForEach(
                    fpe =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, fpe.IdCliente);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_faixa_etaria", DbType.String, fpe.DsFaixaEtaria);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_ocupacao", DbType.String, fpe.DsOcupacao);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_conhecimento", DbType.String, fpe.DsConhecimento);
                            lAcessaDados.AddInParameter(lDbCommand, "@tp_investidor", DbType.String, fpe.TpInvestidor);
                            lAcessaDados.AddInParameter(lDbCommand, "@tp_investimento", DbType.String, fpe.TpInvestimento);
                            lAcessaDados.AddInParameter(lDbCommand, "@tp_instituicao", DbType.String, fpe.TpInstituicao);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_renda_familiar", DbType.String, fpe.DsRendaFamiliar);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_inclusao", DbType.DateTime, fpe.DtInclusao);

                            lDbCommand.Transaction = gDbTransaction;
                            fpe.IdFichaPerfilSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarClienteCursoPalestra()
        {
            if (null != this.EducacionalCompleto.ClienteCursoPalestraInfo && this.EducacionalCompleto.ClienteCursoPalestraInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = @"INSERT INTO [dbo].[tb_cliente_curso_palestra] 
                                      (    id_cursopalestra 
                                      ,    id_cliente
                                      ,    dt_cadastro
                                      ,    st_presenca
                                      ,    st_confirmainscricao
                                      ,    st_listaespera)
                               VALUES (    @id_cursopalestra 
                                      ,    @id_cliente
                                      ,    @dt_cadastro
                                      ,    @st_presenca
                                      ,    @st_confirmainscricao
                                      ,    @st_listaespera);";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.ClienteCursoPalestraInfo.ForEach(
                    ccp =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            try
                            {
                                lAcessaDados.AddInParameter(lDbCommand, "@id_cursopalestra", DbType.Int32, this.EducacionalCompleto.CursoPalestraInfo.Find(cpa => { return cpa.IdCursoPalestraOracle == ccp.IdCursoPalestraOracle; }).IdCursoPalestraSql);
                                lAcessaDados.AddInParameter(lDbCommand, "@id_cliente", DbType.Int32, ccp.IdCliente);
                                lAcessaDados.AddInParameter(lDbCommand, "@dt_cadastro", DbType.DateTime, ccp.DtCadastro);
                                lAcessaDados.AddInParameter(lDbCommand, "@st_presenca", DbType.String, ccp.StPresenca);
                                lAcessaDados.AddInParameter(lDbCommand, "@st_confirmainscricao", DbType.String, ccp.StConfirmaInscricao);
                                lAcessaDados.AddInParameter(lDbCommand, "@st_listaespera", DbType.String, ccp.StListaEspera);

                                lDbCommand.Transaction = gDbTransaction;
                                lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection); //--> Esta tabla não possui PK
                            }
                            catch (Exception ex) { throw ex; }
                        }
                    });
            }
        }

        private void MigrarAvaliacaoInteresse()
        {
            if (null != this.EducacionalCompleto.AvaliacaoInteresseInfo && this.EducacionalCompleto.AvaliacaoInteresseInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_avaliacao_interesse] (ds_avaliacaointeresse) VALUES (@ds_avaliacaointeresse); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.AvaliacaoInteresseInfo.ForEach(
                    avi =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_avaliacaointeresse", DbType.String, avi.DsAvaliacaoInteresse);

                            lDbCommand.Transaction = gDbTransaction;
                            avi.IdAvaliacaoInteresseSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarAvaliacaoPalestra()
        {
            if (null != this.EducacionalCompleto.AvaliacaoPalestraInfo && this.EducacionalCompleto.AvaliacaoPalestraInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = @"INSERT INTO [dbo].[tb_avaliacao_palestra]
                                      (    id_cursopalestra
                                      ,    ds_avaliapalestrante
                                      ,    ds_material
                                      ,    ds_infraestrutura
                                      ,    ds_expectativa
                                      ,    dt_avaliacao)
                               VALUES (    @id_cursopalestra
                                      ,    @ds_avaliapalestrante
                                      ,    @ds_material
                                      ,    @ds_infraestrutura
                                      ,    @ds_expectativa
                                      ,    @dt_avaliacao);
                               SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.AvaliacaoPalestraInfo.ForEach(
                    pal =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_cursopalestra", DbType.Int32, this.EducacionalCompleto.CursoPalestraInfo.Find(cpa => { return cpa.IdCursoPalestraOracle == pal.IdCursoPalestra; }).IdCursoPalestraSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_avaliapalestrante", DbType.String, pal.DsAvaliaPalestrante);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_material", DbType.String, pal.DsMaterial);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_infraestrutura", DbType.String, pal.DsInfraEstrutura);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_expectativa", DbType.String, pal.DsExpectativa);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_avaliacao", DbType.DateTime, pal.DtAvaliacao);

                            lDbCommand.Transaction = gDbTransaction;
                            pal.IdAvaliacaoPalestraSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarCursoPalestra()
        {
            if (null != this.EducacionalCompleto.CursoPalestraInfo && this.EducacionalCompleto.CursoPalestraInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = @"INSERT INTO [dbo].[tb_curso_palestra]
                                      (    id_tema
                                      ,    id_localidade
                                      ,    id_assessor
                                      ,    ds_municipio
                                      ,    ds_endereco
                                      ,    ds_cep
                                      ,    ds_texto
                                      ,    dt_criacao
                                      ,    nr_vagalimite
                                      ,    nr_vagainscritos
                                      ,    st_situacao
                                      ,    st_realizado
                                      ,    st_tipoevento
                                      ,    valor
                                      ,    dt_datahoralimite
                                      ,    dt_datahoracurso
                                      ,    fl_home)
                               VALUES (    @id_tema
                                      ,    @id_localidade
                                      ,    @id_assessor
                                      ,    @ds_municipio
                                      ,    @ds_endereco
                                      ,    @ds_cep
                                      ,    @ds_texto
                                      ,    @dt_criacao
                                      ,    @nr_vagalimite
                                      ,    @nr_vagainscritos
                                      ,    @st_situacao
                                      ,    @st_realizado
                                      ,    @st_tipoevento
                                      ,    @valor
                                      ,    @dt_datahoralimite
                                      ,    @dt_datahoracurso
                                      ,    @fl_home);
                               SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.CursoPalestraInfo.ForEach(
                    cpa =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_tema", DbType.Int32, this.EducacionalCompleto.TemaInfo.Find(tem => { return tem.IdTemaOracle == cpa.IdTema; }).IdTemaSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_localidade", DbType.Int32, this.EducacionalCompleto.LocalidadeInfo.Find(loc => { return loc.IdLocalidadeOracle == cpa.IdLocalidade; }).IdLocalidadeSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, cpa.IdAssessor);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_municipio", DbType.String, cpa.DsMunicipio);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_endereco", DbType.String, cpa.DsEndereco);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_cep", DbType.String, cpa.DsCEP);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_texto", DbType.String, cpa.DsTexto);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_criacao", DbType.DateTime, cpa.DtCriacao);
                            lAcessaDados.AddInParameter(lDbCommand, "@nr_vagalimite", DbType.Int32, cpa.NrVagaLimite);
                            lAcessaDados.AddInParameter(lDbCommand, "@nr_vagainscritos", DbType.Int32, cpa.NrVagaInscritos);
                            lAcessaDados.AddInParameter(lDbCommand, "@st_situacao", DbType.Int32, cpa.StSituacao);
                            lAcessaDados.AddInParameter(lDbCommand, "@st_realizado", DbType.String, cpa.StRealizado);
                            lAcessaDados.AddInParameter(lDbCommand, "@st_tipoevento", DbType.String, cpa.StTipoEvento);
                            lAcessaDados.AddInParameter(lDbCommand, "@valor", DbType.Decimal, cpa.Valor);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_datahoralimite", DbType.DateTime, cpa.DtDataHoraLimite);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_datahoracurso", DbType.DateTime, cpa.DtDataHoraCurso);
                            lAcessaDados.AddInParameter(lDbCommand, "@fl_home", DbType.String, cpa.FlHome);

                            lDbCommand.Transaction = gDbTransaction;
                            cpa.IdCursoPalestraSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarUsuario()
        {
            if (null != this.EducacionalCompleto.UsuarioInfo && this.EducacionalCompleto.UsuarioInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = @"INSERT INTO [dbo].[tb_usuario]
                                      (    id_perfil
                                      ,    id_localidade
                                      ,    id_assessor
                                      ,    ds_nome
                                      ,    ds_email
                                      ,    ds_senha
                                      ,    st_usuario)
                               VALUES (    @id_perfil
                                      ,    @id_localidade
                                      ,    @id_assessor
                                      ,    @ds_nome
                                      ,    @ds_email
                                      ,    @ds_senha
                                      ,    @st_usuario);
                               SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.UsuarioInfo.ForEach(
                    usu =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_perfil", DbType.Int32, this.EducacionalCompleto.PerfilInfo.Find(per => { return per.IdPerfilOracle == usu.IdPerfil; }).IdPerfilSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_localidade", DbType.Int32, this.EducacionalCompleto.LocalidadeInfo.Find(loc => { return loc.IdLocalidadeOracle == usu.IdLocalidade; }).IdLocalidadeSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_assessor", DbType.Int32, usu.IdAssessor);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nome", DbType.String, usu.DsNome);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_email", DbType.String, usu.DsEmail);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_senha", DbType.String, usu.DsSenha);
                            lAcessaDados.AddInParameter(lDbCommand, "@st_usuario", DbType.String, usu.StUsuario);

                            lDbCommand.Transaction = gDbTransaction;
                            usu.IdUsuarioSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarLocalidade()
        {
            if (null != this.EducacionalCompleto.LocalidadeInfo && this.EducacionalCompleto.LocalidadeInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_localidade] (ds_localidade, bit_portal) values (@ds_localidade, @bit_portal); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.LocalidadeInfo.ForEach(
                    loc =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_localidade", DbType.String, loc.DsLocalidade);
                            lAcessaDados.AddInParameter(lDbCommand, "@bit_portal", DbType.Int32, loc.BitPortal);

                            lDbCommand.Transaction = gDbTransaction;
                            loc.IdLocalidadeSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarPerfil()
        {
            if (null != this.EducacionalCompleto.PerfilInfo && this.EducacionalCompleto.PerfilInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_perfil] (ds_perfil) values (@ds_perfil); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.PerfilInfo.ForEach(
                    per =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_perfil", DbType.String, per.DsPerfil);

                            lDbCommand.Transaction = gDbTransaction;
                            per.IdPerfilSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarPalestraSobMedida()
        {
            if (null != this.EducacionalCompleto.PalestraSobMedidaInfo && this.EducacionalCompleto.PalestraSobMedidaInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = @"INSERT INTO [dbo].[tb_palestra_sob_medida] 
                                      (    id_palestrante
                                      ,    id_tema
                                      ,    id_estado
                                      ,    ds_municipio
                                      ,    ds_endereco
                                      ,    ds_cep
                                      ,    ds_local
                                      ,    tp_solicitante
                                      ,    dt_criacao
                                      ,    dt_datahora_inicio
                                      ,    dt_datahora_fim
                                      ,    ds_publico_alvo
                                      ,    qt_pessoas
                                      ,    st_situacao)
                               VALUES (    @id_palestrante
                                      ,    @id_tema
                                      ,    @id_estado
                                      ,    @ds_municipio
                                      ,    @ds_endereco
                                      ,    @ds_cep
                                      ,    @ds_local
                                      ,    @tp_solicitante
                                      ,    @dt_criacao
                                      ,    @dt_datahora_inicio
                                      ,    @dt_datahora_fim
                                      ,    @ds_publico_alvo
                                      ,    @qt_pessoas
                                      ,    @st_situacao);
                               SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.PalestraSobMedidaInfo.ForEach(
                    psm =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_palestrante", DbType.Int32, this.EducacionalCompleto.PalestranteInfo.Find(pal => { return pal.IdPalestranteOracle == psm.IdPalestrante; }).IdPalestranteSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_tema", DbType.Int32, this.EducacionalCompleto.TemaInfo.Find(tem => { return tem.IdTemaOracle == psm.IdTema; }).IdTemaSql);
                            lAcessaDados.AddInParameter(lDbCommand, "@id_estado", DbType.Int32, this.EducacionalCompleto.EstadoInfo.Find(est => { return est.IdEstadoOracle == psm.IdEstado; }).IdEstadoSQL);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_municipio", DbType.String, psm.DsMunicipio);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_endereco", DbType.String, psm.DsEndereco);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_cep", DbType.String, psm.DsCep);
                            lAcessaDados.AddInParameter(lDbCommand, "@tp_solicitante", DbType.String, psm.TpSolicitante);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_criacao", DbType.DateTime, psm.DtCriacao);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_datahora_inicio", DbType.DateTime, psm.DtDataHoraInicio);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_datahora_fim", DbType.DateTime, psm.DtDataHoraFim);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_publico_alvo", DbType.String, psm.DsPublicoAlvo);
                            lAcessaDados.AddInParameter(lDbCommand, "@qt_pessoas", DbType.Decimal, psm.QtPessoas);
                            lAcessaDados.AddInParameter(lDbCommand, "@st_situacao", DbType.String, psm.StSituacao);

                            lDbCommand.Transaction = gDbTransaction;
                            psm.IdCursoPalestraSobMedidaSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarTema()
        {
            if (null != this.EducacionalCompleto.TemaInfo && this.EducacionalCompleto.TemaInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_tema] (id_nivel, ds_titulo, ds_chamada, st_situacao, dt_criacao) values (@id_nivel, @ds_titulo, @ds_chamada, @st_situacao, @dt_criacao); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.TemaInfo.ForEach(
                    tem =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_nivel", DbType.Int32, this.EducacionalCompleto.NivelInfo.Find(nvl => { return nvl.IdNivelOracle == tem.IdNivel; }).IdNivelSQL);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_titulo", DbType.String, tem.DsTitulo);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_chamada", DbType.String, tem.DsChamada);
                            lAcessaDados.AddInParameter(lDbCommand, "@st_situacao", DbType.String, tem.StSituacao);
                            lAcessaDados.AddInParameter(lDbCommand, "@dt_criacao", DbType.DateTime, tem.DtCriacao);

                            lDbCommand.Transaction = gDbTransaction;
                            tem.IdTemaSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarCursoPalestraOnline()
        {
            if (null != this.EducacionalCompleto.CursoPalestraOnLineInfo && this.EducacionalCompleto.CursoPalestraOnLineInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_curso_palestra_online] (id_nivel, ds_curso, ds_url, ds_texto) values (@id_nivel, @ds_curso, @ds_url, @ds_texto); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.CursoPalestraOnLineInfo.ForEach(
                    cco =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@id_nivel", DbType.Int32, this.EducacionalCompleto.NivelInfo.Find(nvl => { return nvl.IdNivelOracle == cco.IdNivel; }).IdNivelSQL);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_curso", DbType.String, cco.DsCurso);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_url", DbType.String, cco.DsUrl);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_texto", DbType.String, cco.DsTexto);

                            lDbCommand.Transaction = gDbTransaction;
                            cco.IdCursoPalestraOnLineSQL = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarNivel()
        {
            if (null != this.EducacionalCompleto.NivelInfo && this.EducacionalCompleto.NivelInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_nivel] (ds_nivel, nr_order) values (@ds_nivel, @nr_order); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.NivelInfo.ForEach(
                    lNivelInfo =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_nivel", DbType.String, lNivelInfo.DsNivel);
                            lAcessaDados.AddInParameter(lDbCommand, "@nr_order", DbType.String, lNivelInfo.NrOrder);

                            lDbCommand.Transaction = gDbTransaction;
                            lNivelInfo.IdNivelSQL = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarPalestrante()
        {
            if (null != this.EducacionalCompleto.PalestranteInfo && this.EducacionalCompleto.PalestranteInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_palestrante] (nm_palestrante, ds_curriculo) values (@nm_palestrante, @ds_curriculo); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.PalestranteInfo.ForEach(
                    lPalestranteInfo =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@nm_palestrante", DbType.String, lPalestranteInfo.NmPalestrante);
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_curriculo", DbType.String, lPalestranteInfo.DsCurriculo);

                            lDbCommand.Transaction = gDbTransaction;
                            lPalestranteInfo.IdPalestranteSql = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }

        private void MigrarEstado()
        {
            if (null != this.EducacionalCompleto.EstadoInfo && this.EducacionalCompleto.EstadoInfo.Count > 0)
            {
                var lAcessaDados = new AcessaDados();
                var lQuery = "INSERT INTO [dbo].[tb_estado] (ds_estado) values (@ds_estado); SELECT SCOPE_IDENTITY()";

                lAcessaDados.Conexao = gConexao;
                lAcessaDados.ConnectionStringName = gConexaoEducacionalSQL;

                this.EducacionalCompleto.EstadoInfo.ForEach(
                    lEstadoInfo =>
                    {
                        using (DbCommand lDbCommand = lAcessaDados.CreateCommand(gDbTransaction, CommandType.Text, lQuery))
                        {
                            lAcessaDados.AddInParameter(lDbCommand, "@ds_estado", DbType.String, lEstadoInfo.DsEstado);

                            lDbCommand.Transaction = gDbTransaction;

                            lEstadoInfo.IdEstadoSQL = lAcessaDados.ExecuteScalar(lDbCommand, lDbCommand.Connection).DBToInt32();
                        }
                    });
            }
        }
    }
}
