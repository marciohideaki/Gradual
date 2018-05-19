using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Gradual.Generico.Dados;
using Gradual.Migracao.Educacional.Entidades;

namespace Gradual.Migracao.Educacional.Dados
{
    public class EducacionalOracleDbLib
    {
        #region | Atributos

        private const string gConexaoEducacionalOracle = "ConexaoEducacionalOracle";

        private const string gConexaoEducacionalSql = "ConexaoEducacionalSql";

        private Hashtable gListaAssessores = new Hashtable();

        #endregion

        #region | Construtores

        public EducacionalOracleDbLib()
        {
            this.RecuperarIdAssessor();
        }

        #endregion

        #region | Métodos

        public List<AvaliacaoInteresseInfo> BuscarAvaliacaoInteresseOracle()
        {
            var lRetorno = new List<AvaliacaoInteresseInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_avaliacao_interesse";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new AvaliacaoInteresseInfo()
                        {
                            DsAvaliacaoInteresse = lLinha["ds_avaliacaointeresse"].DBToString(),
                            IdAvaliacaoInteresseOracle = lLinha["id_avaliacaointeresse"].DBToInt32(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<AvaliacaoPalestraInfo> BuscarAvaliacaoPalestraOracle()
        {
            var lRetorno = new List<AvaliacaoPalestraInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_avaliacao_palestra";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new AvaliacaoPalestraInfo()
                        {
                            DsAvaliaPalestrante = lLinha["ds_avaliapalestrante"].DBToString(),
                            DsExpectativa = lLinha["ds_expectativa"].DBToString(),
                            DsInfraEstrutura = lLinha["ds_infraestrutura"].DBToString(),
                            DsMaterial = lLinha["ds_material"].DBToString(),
                            DtAvaliacao = lLinha["dt_avaliacao"].DBToDateTime(),
                            IdAvaliacaoPalestraOracle = lLinha["id_avaliacaopalestra"].DBToInt32(),
                            IdCursoPalestra = lLinha["id_cursopalestra"].DBToInt32()
                        });
                    }
            }

            return lRetorno;
        }

        public List<ClienteCursoPalestraInfo> BuscarClienteCursoPalestraOracle()
        {
            var lRetorno = new List<ClienteCursoPalestraInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = @"SELECT     pal.*, cli.CPF 
                              FROM       tb_cliente_curso_palestra pal
                              INNER JOIN cliente                   cli ON pal.ID_CLIENTE = cli.ID_CLIENTE
                              INNER JOIN tscclibol                 bol ON bol.cd_cliente = cli.codigobovespa
                              LEFT  JOIN tscclibmf                 bmf ON bmf.CODCLI     = cli.codigobovespa
                              WHERE      bol.in_situac = 'A'
                              OR         bmf.STATUS    = 'A'";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                var lIdClienteSql = default(int);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lIdClienteSql = this.RecuperarIdClienteNoSql(lLinha["cpf"].DBToString());

                        if (!0.Equals(lIdClienteSql)) lRetorno.Add(new ClienteCursoPalestraInfo()
                            {
                                DtCadastro = lLinha["dt_cadastro"].DBToDateTime(),
                                IdCliente = lIdClienteSql,
                                IdCursoPalestraOracle = lLinha["id_cursopalestra"].DBToInt32(),
                                StConfirmaInscricao = lLinha["st_confirmainscricao"].DbToChar(),
                                StListaEspera = lLinha["st_listaespera"].DbToChar(),
                                StPresenca = lLinha["st_presenca"].DbToChar(),
                            });
                    }
            }

            return lRetorno;
        }

        public List<CursoPalestraInfo> BuscarCursoPalestraOracle()
        {
            var lRetorno = new List<CursoPalestraInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_curso_palestra";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new CursoPalestraInfo()
                        {
                            DsCEP = lLinha["ds_cep"].DBToString(),
                            DsEndereco = lLinha["ds_endereco"].DBToString(),
                            DsMunicipio = lLinha["ds_municipio"].DBToString(),
                            DsTexto = lLinha["ds_texto"].DBToString(),
                            DtCriacao = lLinha["dt_criacao"].DBToDateTime(),
                            DtDataHoraCurso = lLinha["dt_datahoracurso"].DBToDateTime(),
                            DtDataHoraLimite = lLinha["dt_datahoralimite"].DBToDateTime(),
                            FlHome = lLinha["fl_home"].DbToChar(),
                            IdAssessor = this.gListaAssessores[lLinha["ID_FILIALASSESSOR"].DBToInt32()].DBToInt32(),
                            IdCursoPalestraOracle = lLinha["id_cursopalestra"].DBToInt32(),
                            IdLocalidade = lLinha["id_localidade"].DBToInt32(),
                            IdTema = lLinha["id_tema"].DBToInt32(),
                            NrVagaInscritos = lLinha["nr_vagainscritos"].DBToInt32(),
                            NrVagaLimite = lLinha["nr_vagalimite"].DBToInt32(),
                            StRealizado = lLinha["st_realizado"].DbToChar(),
                            StSituacao = lLinha["st_situacao"].DBToInt32(),
                            StTipoEvento = lLinha["st_tipoevento"].DbToChar(),
                            Valor = lLinha["VALOR"].DBToDecimal(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<CursoPalestraOnLineInfo> BuscarCursoPalestraOnLineOracle()
        {
            var lRetorno = new List<CursoPalestraOnLineInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_curso_palestra_online";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new CursoPalestraOnLineInfo()
                        {
                            DsCurso = lLinha["ds_curso"].DBToString(),
                            DsTexto = lLinha["ds_texto"].DBToString(),
                            DsUrl = lLinha["ds_url"].DBToString(),
                            IdCursoPalestraOnLineOracle = lLinha["id_cursopalestraonline"].DBToInt32(),
                            IdNivel = lLinha["id_nivel"].DBToInt32(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<EstadoInfo> BuscarEstadoOracle()
        {
            var lRetorno = new List<EstadoInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_estado";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new EstadoInfo()
                        {
                            DsEstado = lLinha["ds_estado"].DBToString(),
                            IdEstadoOracle = lLinha["id_estado"].DBToInt32()

                        });
                    }
            }

            return lRetorno;
        }

        public List<FichaPerfilInfo> BuscarFichaPerfilOracle()
        {
            var lRetorno = new List<FichaPerfilInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = @"SELECT     per.*, cli.cpf
                              FROM       tb_ficha_perfil per
                              INNER JOIN cliente         cli ON cli.id_cliente = per.id_cliente
                              INNER JOIN tscclibol       bol ON bol.cd_cliente = cli.codigobovespa
                              LEFT  JOIN tscclibmf       bmf ON bmf.CODCLI = cli.codigobovespa
                              WHERE      bol.in_situac = 'A'
                              OR         bmf.STATUS    = 'A'";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);
                var lIdClienteSql = default(int);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lIdClienteSql = this.RecuperarIdClienteNoSql(lLinha["cpf"].DBToString());

                        if (!0.Equals(lIdClienteSql)) lRetorno.Add(new FichaPerfilInfo()
                         {
                             DsConhecimento = lLinha["ds_conhecimento"].DBToString(),
                             DsFaixaEtaria = lLinha["ds_faixa_etaria"].DBToString(),
                             DsOcupacao = lLinha["ds_ocupacao"].DBToString(),
                             DsRendaFamiliar = lLinha["ds_renda_familiar"].DBToString(),
                             DtInclusao = lLinha["dt_inclusao"].DBToDateTime(),
                             IdCliente = lIdClienteSql,
                             IdFichaPerfilOracle = lLinha["id_ficha_perfil"].DBToInt32(),
                             TpInstituicao = lLinha["tp_instituicao"].DBToString(),
                             TpInvestidor = lLinha["tp_investidor"].DBToString(),
                             TpInvestimento = lLinha["tp_investimento"].DBToString(),
                         });
                    }
            }

            return lRetorno;
        }

        public List<LocalidadeInfo> BuscarLocalidadeOracle()
        {
            var lRetorno = new List<LocalidadeInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_localidade";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new LocalidadeInfo()
                        {
                            BitPortal = lLinha["bit_portal"].DBToInt32(),
                            DsLocalidade = lLinha["ds_localidade"].DBToString(),
                            IdLocalidadeOracle = lLinha["id_localidade"].DBToInt32(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<NivelInfo> BuscarNivelOracle()
        {
            var lRetorno = new List<NivelInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_nivel";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new NivelInfo()
                        {
                            DsNivel = lLinha["ds_nivel"].DBToString(),
                            IdNivelOracle = lLinha["id_nivel"].DBToInt32(),
                            NrOrder = lLinha["nr_order"].DBToInt32(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<PalestranteInfo> BuscarPalestranteOracle()
        {
            var lRetorno = new List<PalestranteInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_palestrante";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new PalestranteInfo()
                        {
                            DsCurriculo = lLinha["ds_curriculo"].DBToString(),
                            IdPalestranteOracle = lLinha["id_palestrante"].DBToInt32(),
                            NmPalestrante = lLinha["nm_palestrante"].DBToString(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<PalestraSobMedidaInfo> BuscarPalestraSobMedidaOracle()
        {
            var lRetorno = new List<PalestraSobMedidaInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_palestra_sob_medida";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new PalestraSobMedidaInfo()
                        {
                            DsCep = lLinha["ds_cep"].DBToString(),
                            DsEndereco = lLinha["ds_endereco"].DBToString(),
                            DsLocal = lLinha["ds_local"].DBToString(),
                            DsMunicipio = lLinha["ds_municipo"].DBToString(),
                            DsPublicoAlvo = lLinha["ds_publico_alvo"].DBToString(),
                            DtCriacao = lLinha["dt_criacao"].DBToDateTime(),
                            DtDataHoraFim = lLinha["dt_datahora_fim"].DBToDateTime(),
                            DtDataHoraInicio = lLinha["dt_datahora_inicio"].DBToDateTime(),
                            IdCursoPalestraSobMedidaOracle = lLinha["id_curso_palestra_sob_medida"].DBToInt32(),
                            IdEstado = lLinha["id_estado"].DBToInt32(),
                            IdPalestrante = lLinha["id_palestrante"].DBToInt32(),
                            IdTema = lLinha["id_tema"].DBToInt32(),
                            QtPessoas = lLinha["qt_pessoas"].DBToInt32(),
                            StSituacao = lLinha["st_situacao"].DbToChar(),
                            TpLocal = lLinha["tp_local"].DBToString(),
                            TpSolicitante = lLinha["tp_solicitante"].DbToChar(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<PerfilInfo> BuscarPerfilOracle()
        {
            var lRetorno = new List<PerfilInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_perfil";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new PerfilInfo()
                        {
                            DsPerfil = lLinha["ds_perfil"].DBToString(),
                            IdPerfilOracle = lLinha["id_perfil"].DBToInt32()
                        });
                    }
            }

            return lRetorno;
        }

        public List<TemaInfo> BuscarTemaOracle()
        {
            var lRetorno = new List<TemaInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_tema";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new TemaInfo()
                        {
                            DsChamada = lLinha["ds_chamada"].DBToString(),
                            DsTitulo = lLinha["ds_titulo"].DBToString(),
                            DtCriacao = lLinha["dt_criacao"].DBToDateTime(),
                            IdNivel = lLinha["id_nivel"].DBToInt32(),
                            IdTemaOracle = lLinha["id_tema"].DBToInt32(),
                            StSituacao = lLinha["st_situacao"].DbToChar(),
                        });
                    }
            }

            return lRetorno;
        }

        public List<UsuarioInfo> BuscarUsuarioOracle()
        {
            var lRetorno = new List<UsuarioInfo>();
            var lAcessaDados = new AcessaDados();

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            string lQuery = "SELECT * FROM tb_usuario";

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    foreach (DataRow lLinha in lDataTable.Rows)
                    {
                        lRetorno.Add(new UsuarioInfo()
                        {
                            DsEmail = lLinha["ds_email"].DBToString(),
                            DsNome = lLinha["ds_nome"].DBToString(),
                            DsSenha = lLinha["ds_senha"].DBToString(),
                            IdAssessor = this.gListaAssessores[lLinha["id_filialassessor"].DBToInt32()].DBToInt32(),
                            IdPerfil = lLinha["id_perfil"].DBToInt32(),
                            IdLocalidade = lLinha["id_localidade"].DBToInt32(),
                            IdUsuarioOracle = lLinha["id_usuario"].DBToInt32(),
                            StUsuario = lLinha["st_usuario"].DbToChar(),
                        });
                    }
            }

            return lRetorno;
        }

        private int RecuperarIdClienteNoSql(string pCpfCnpj)
        {
            var lRetorno = default(int);
            var lAcessaDados = new AcessaDados();

            var lQuery = "SELECT id_cliente FROM directtradecadastro.dbo.tb_cliente where ds_cpfcnpj like '%{0}'";

            lAcessaDados.ConnectionStringName = gConexaoEducacionalSql;


            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, string.Format(lQuery, pCpfCnpj)))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0)
                    lRetorno = lDataTable.Rows[0]["id_cliente"].DBToInt32();
                else
                    lRetorno = 0;
            }

            return lRetorno;
        }

        private void RecuperarIdAssessor()
        {
            var lAcessaDados = new AcessaDados();

            var lQuery = @"SELECT     ass.id_assessorsinacor
                           ,          fil.id_assessorfilial
                           FROM       assessor ass 
                           INNER JOIN assessorfilial fil ON ass.id_assessor = fil.id_assessor";

            lAcessaDados.ConnectionStringName = gConexaoEducacionalOracle;

            using (DbCommand lDbCommand = lAcessaDados.CreateCommand(CommandType.Text, lQuery))
            {
                var lDataTable = lAcessaDados.ExecuteDbDataTable(lDbCommand);

                if (null != lDataTable && lDataTable.Rows.Count > 0) foreach (DataRow lLinha in lDataTable.Rows)
                        gListaAssessores.Add(lLinha["id_assessorfilial"].DBToInt32(), lLinha["id_assessorsinacor"].DBToInt32());
            }
        }

        #endregion
    }
}
