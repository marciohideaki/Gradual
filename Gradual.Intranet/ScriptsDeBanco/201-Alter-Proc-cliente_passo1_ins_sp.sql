set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
--Descrição: Passo 1 do Portal.
--Autor: Gustavo Malta Guimarães
--Data de criação: 01/07/2010


-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 23/09/2010
-- Motivo                   : Ao Inserir um cliente cadastrar todas as pendências para ele.


-- Autor Ultima Alteração   : Gustavo Malta Guimarães
-- Data da ultima alteração : 15/10/2010
-- Motivo                   : Inserir apenas as pendencias automaticas.


ALTER PROCEDURE [dbo].[cliente_passo1_ins_sp]
	--Login
	@ds_email varchar(80),
	@Cd_Senha varchar(32),
	@Cd_AssinaturaEletronica varchar(32),     

	--Cliente
	@Id_Cliente numeric out,

	@Ds_CpfCnpj varchar(15) ,
	@Dt_NascimentoFundacao datetime,
	@Ds_Nome varchar(80) ,
	@Id_AssessorInicial numeric ,
	@Cd_Sexo varchar(1),
	@Tp_Pessoa varchar(1) ,
	@Tp_cliente numeric,
	--////public int StPasso { get { return 1; }}
	--////public DateTime DtPasso1 { get { return DateTime.Now; }}

	--Telefone
	@Id_Tipo_Telefone numeric,
	@Ds_Numero varchar(10),
	@Ds_Ddd varchar(7),
	@Ds_Ramal varchar(5)
	--////public int StPrincipal { get { return 1; }}
AS
	
	Begin Transaction trans;

	declare 
		@id_login int;

	insert into tb_login (
		ds_email ,
		Cd_Senha ,
		Cd_AssinaturaEletronica  ,
		nr_tentativaserradas ,
		dt_ultimaexpiracao ,
		tp_acesso
	)values(
		@ds_email ,
		@Cd_Senha ,
		@Cd_AssinaturaEletronica ,
		0,
		getdate(),
		0
	);

	SELECT @id_login=SCOPE_IDENTITY();
	
	insert into tb_cliente(
		Ds_CpfCnpj ,
		Dt_NascimentoFundacao,
		Ds_Nome ,
		Id_AssessorInicial ,
		Cd_Sexo ,
		Tp_Pessoa  ,
		Tp_cliente ,
		St_Passo ,
		Dt_Passo1 ,
		id_login,
		dt_ultimaatualizacao,
		st_ativo,
		ds_origemcadastro,
		st_cadastroportal
	)values(
		@Ds_CpfCnpj,
		@Dt_NascimentoFundacao,
		@Ds_Nome,
		@Id_AssessorInicial,
		@Cd_Sexo,
		@Tp_Pessoa,
		@Tp_cliente,
		1,
		getdate(),
		@id_login,
		getdate(),
		1,
		'Portal',
		1
	);
	 
	SELECT @id_cliente=SCOPE_IDENTITY();

	insert into tb_cliente_telefone(
		Id_Tipo_Telefone,
		Ds_Numero,
		Ds_Ddd,
		Ds_Ramal,
		St_Principal ,
		id_cliente
	)values(
		@Id_Tipo_Telefone,
		@Ds_Numero,
		@Ds_Ddd,
		@Ds_Ramal,
		1,
		@id_cliente
	);



--Procedimento para Inserir todas as pendências para o novo cliente
	--Declarando o Cursor
	DECLARE curPendencias
	Cursor For
	Select id_tipo_pendencia from tb_tipo_pendenciacadastral where st_automatica = 1
	--Declarando uma variavel para as linhas do cursor
	Declare @id_tipo_pendencia int;
	--Abrindo o cursor
	OPEN curPendencias
	--Pegando a primeira linha
	FETCH NEXT FROM curPendencias INTO @id_tipo_pendencia
	--Pegando cada linha
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--Efetuando o procedimento: Inserindo cada uma das pendências para o novo cliente
		INSERT INTO tb_cliente_pendenciacadastral
           (id_tipo_pendencia
           ,id_cliente
           ,ds_pendencia
           ,dt_cadastropendencia
           ,dt_resolucao
           ,ds_resolucao)
		VALUES
           (@id_tipo_pendencia
           ,@id_cliente
           ,'Pendencia cadastrada autometicamente para novo cliente'
           ,getdate()
           ,null
           ,null)
		--Próxima linha do cursor
		FETCH NEXT FROM curPendencias INTO @id_tipo_pendencia
	END
	--Fechando e desalocando cursor
	CLOSE curPendencias
	DEALLOCATE curPendencias


	if (@@error > 0)
	begin
		rollback tran	
	end
	else
	begin
		commit tran
	end

