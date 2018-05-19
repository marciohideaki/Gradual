SELECT 
	   CONVERT(VARCHAR(10),a.CD_CLIENTE)  AS Cliente,
	   a.nm_titulo	 AS Titulo,
	   CONVERT(VARCHAR(10),dt_movimento,103)  AS Aplicacao,
	   CONVERT(VARCHAR(10),dt_vencimento,103) AS Vencimento,
	   CONVERT(VARCHAR(50), tx_emissao)    AS Taxa ,
	   CONVERT(VARCHAR(50),sd_final)      AS Quantidade,
	   CONVERT(VARCHAR(50),tp_indice)	 AS Indice,
	   CONVERT(VARCHAR(50),vr_resgate)    AS SaldoBruto,
	   CONVERT(VARCHAR(50),vr_aplicacao)  AS ValorOriginal,
	   CONVERT(VARCHAR(50),VR_IRF)		 AS IRRF ,
	   CONVERT(VARCHAR(50),VR_IOF)		 AS IOF  ,
	   CONVERT(VARCHAR(50),VR_LIQUIDO)	 AS SaldoLiquido,
	   CONVERT(VARCHAR(10), a.dt_atual,103) AS DataAtualizacao
FROM  
	rf342_posicao_cliente AS a
INNER JOIN 
(
	SELECT b.cd_cliente,
		  cd_titulo,
		  nm_titulo,
		  max(b.dt_atual) as Maxdate 
	FROM rf342_posicao_cliente b /*where b.cd_cliente = @cd_cliente */ 
	GROUP BY b.cd_cliente, cd_titulo, nm_titulo
)

tb_cs ON a.cd_cliente = tb_cs.cd_cliente AND a.dt_atual = tb_cs.MaxDate AND a.cd_titulo = tb_cs.cd_titulo

WHERE CONVERT(VARCHAR(10), a.dt_atual,103) = CONVERT(VARCHAR(10), tb_cs.MaxDate, 103) AND a.dt_atual > GETDATE()-2
ORDER BY a.CD_CLIENTE,a.dt_movimento ASC

