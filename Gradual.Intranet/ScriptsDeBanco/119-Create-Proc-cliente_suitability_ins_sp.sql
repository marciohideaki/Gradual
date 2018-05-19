

/****** Object:  StoredProcedure [dbo].[cliente_suitability_ins_sp]    Script Date: 06/07/2010 15:34:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Descrição: Insere registro na tabela tb_cliente_suitability.
--Autor: Luciano De Maria Leal
--Data de criação: 07/06/2010
CREATE PROCEDURE [dbo].[cliente_suitability_ins_sp]
	@id_cliente int,
	@ds_perfil varchar(50),
	@ds_status varchar(50),
	@dt_realizacao datetime,
	@st_preenchidopelocliente bit,
	@ds_loginrealizado varchar(200),
	@ds_fonte varchar(200),
	@ds_respostas varchar(500),

	@id_cliente_suitability int OUTPUT
AS
INSERT INTO [Cadastro].[dbo].[tb_cliente_suitability]
(
	 [id_cliente]
	,[ds_perfil]
	,[ds_status]
	,[dt_realizacao]
	,[st_preenchidopelocliente]
	,[ds_loginrealizado]
	,[ds_fonte]
	,[ds_respostas]
)
VALUES
(
	 @id_cliente
	,@ds_perfil
	,@ds_status
	,@dt_realizacao
	,@st_preenchidopelocliente
	,@ds_loginrealizado
	,@ds_fonte
	,@ds_respostas
)

-- Get the IDENTITY value for the row just inserted.
SELECT @id_cliente_suitability=SCOPE_IDENTITY()


