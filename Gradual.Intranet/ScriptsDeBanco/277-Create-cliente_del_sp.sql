CREATE PROCEDURE cliente_del_sp
( @dt_nascimentofundacao DATETIME
, @ds_cpfcnpj            VARCHAR(15))
AS

DECLARE @id_login   INT
DECLARE @id_cliente INT

SELECT @id_login   = [id_login]
,      @id_cliente = [id_cliente]
FROM   [tb_cliente]
WHERE  [ds_cpfcnpj]            = @ds_cpfcnpj
AND    [dt_nascimentofundacao] = @dt_nascimentofundacao

DELETE FROM tb_alteracao                             WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_pendenciacadastral            WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_emitente                      WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_diretor                       WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_investidor_naoresidente       WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_procuradorrepresentante       WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_controladora                  WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_contrato                      WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_conta                         WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_banco                         WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_situacaofinanceirapatrimonial WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_telefone                      WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente_endereco                      WHERE id_cliente = @id_cliente
DELETE FROM tb_cliente                               WHERE id_cliente = @id_cliente
DELETE FROM tb_login                                 WHERE id_login   = @id_login
