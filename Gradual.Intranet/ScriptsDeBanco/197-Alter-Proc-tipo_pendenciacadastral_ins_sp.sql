set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Insere registro na tabela tb_tipo_pendenciacadastral.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Adicionando informação se é automatica.
--Autor: Gustavo Malta Guimarães
--Data de criação: 14/10/2010

ALTER PROCEDURE [dbo].[tipo_pendenciacadastral_ins_sp]
	@ds_pendencia varchar(200),
	@st_automatica bit,
	@id_tipo_pendencia int OUTPUT
AS
INSERT tb_tipo_pendenciacadastral
(
	ds_pendencia,st_automatica
)
VALUES
(
	@ds_pendencia,@st_automatica
)

SELECT @id_tipo_pendencia=SCOPE_IDENTITY()

