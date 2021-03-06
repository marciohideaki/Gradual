set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[cliente_telefone_ins_sp]
                 @id_cliente       bigint
               , @id_tipo_telefone bigint
               , @st_principal     bit
               , @ds_ddd           numeric(7, 0)
               , @ds_ramal         numeric(5, 0)
               , @ds_numero        numeric(10, 0)
               , @id_telefone      bigint output
/*
 -- Criação  --
    Descrição      : Realiza a inclusão dos dados de telefone do cliente
    Autor          : Antônio Rodrigues
    Data de criação: 2010 04 28
 -- Alteração --
	Descrição      : Inclusão do parametro de output
    Autor          : Antônio Rodrigues
    Data           : 2010 05 13
*/
AS
     INSERT INTO [dbo].[tb_cliente_telefone]
            (    id_cliente
            ,    id_tipo_telefone
            ,    st_principal
            ,    ds_ddd
            ,    ds_ramal
            ,    ds_numero)
     VALUES (    @id_cliente
            ,    @id_tipo_telefone
            ,    @st_principal
            ,    @ds_ddd
            ,    @ds_ramal
            ,    @ds_numero)
SELECT @id_telefone = SCOPE_IDENTITY()

