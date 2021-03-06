set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

--Descrição: Atualiza registro(s) na tabela tb_tipo_pendenciacadastral.
--Autor: Bruno Varandas Ribeiro
--Data de criação: 30/04/2010

--Descrição: Adicionando informação se é automatica.
--Autor: Gustavo Malta Guimarães
--Data de criação: 14/10/2010

ALTER PROCEDURE [dbo].[tipo_pendenciacadastral_upd_sp]
	@id_tipo_pendencia int,
	@st_automatica bit,
	@ds_pendencia varchar(200)
AS

UPDATE tb_tipo_pendenciacadastral
SET 
	[ds_pendencia] = @ds_pendencia, [st_automatica] = @st_automatica
WHERE
	[id_tipo_pendencia] = @id_tipo_pendencia

