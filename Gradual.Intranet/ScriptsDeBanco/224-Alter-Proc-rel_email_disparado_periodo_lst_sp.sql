set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Bruno Varandas Ribeiro
-- Create date: 19/05/2010
-- Description:	Recupera uma lista de emails disparados em um certo período
-- =============================================

-- =============================================
-- Author:		Gustavo Malta Guimarães
-- Create date: 09/11/2010
-- Description:	Arrumando a performance
-- =============================================

-- =============================================
-- Author:		Bruno Varandas
-- Create date: 30/11/2010
-- Description:	Inclusão do filtro no campo TipoPessoa
-- =============================================

ALTER PROCEDURE [dbo].[rel_email_disparado_periodo_lst_sp]
	@DtDe datetime  ,
	@DtAte datetime ,
	@IdTipoEmail int,
	@DsEmailDestinatario varchar(200),
	@TipoPessoa varchar(1)
AS
BEGIN
	SELECT    [eml].[id_email]
    ,         [eml].[dt_envioemail]
    ,         [eml].[id_tipoemail]
    ,         [eml].[ds_corpoemail]
    ,         [eml].[ds_emailremetente]
    ,         [eml].[ds_emaildestinatario]
    ,         [eml].[dt_envioemail]
    ,         [eml].[ds_assuntoemail]
    ,         [cli].[ds_cpfcnpj]
    ,         [con].[cd_codigo]
	FROM      [dbo].[tb_emaildisparado] AS [eml]
--    LEFT JOIN [dbo].[tb_cliente] AS  [cli] 
--                                 ON ([cli].[id_cliente] IN 
--                                    (
--                                         SELECT     [id_cliente] FROM [tb_login]
--                                         INNER JOIN [dbo].[tb_cliente] ON [dbo].[tb_login].[id_login] = [dbo].[tb_cliente].[id_login]
--                                         WHERE      [ds_email] = [eml].[ds_emaildestinatario]
--                                    ))
	Left JOIN  tb_login on ([ds_emaildestinatario]=[ds_email])
	Left join tb_cliente AS cli on (tb_login.id_login = cli.id_login)
    LEFT JOIN [dbo].[tb_cliente_conta] AS [con] ON [con].[id_cliente] = [cli].[id_cliente] AND [con].[st_principal] = 1
	WHERE 
		[eml].[dt_envioemail] BETWEEN @DtDe AND @DtAte 
		AND ((@IdTipoEmail = 0) OR ([eml].[id_tipoemail] = @IdTipoEmail) )
		AND ((@DsEmailDestinatario = '') OR ([eml].[ds_emaildestinatario] LIKE '%' + @DsEmailDestinatario + '%'))
		AND [eml].[ds_emaildestinatario] <> 'a@a.a'
		AND ((@TipoPessoa = '') OR ([cli].[tp_pessoa] = @TipoPessoa))
	ORDER BY [eml].[dt_envioemail] DESC
END







