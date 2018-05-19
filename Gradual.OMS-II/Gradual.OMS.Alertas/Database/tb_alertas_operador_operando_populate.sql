--
-- TB_ALERTAS_OPERADOR_OPERANDO_POPULATE.SQL
--
-- Script para inicializa��o do conte�do das tabelas 
-- TB_ALERTAS_OPERADOR e TB_ALERTAS_OPERANDO
--

insert into tb_alertas_operador ("Tipo", "Descricao") values (0, 'MaiorIgual')
insert into tb_alertas_operador ("Tipo", "Descricao") values (1, 'MenorIgual')
insert into tb_alertas_operador ("Tipo", "Descricao") values (2, 'Atingido')

insert into tb_alertas_operando ("Tipo", "Descricao") values (0, 'Preco')
insert into tb_alertas_operando ("Tipo", "Descricao") values (1, 'Oscilacao')
insert into tb_alertas_operando ("Tipo", "Descricao") values (2, 'Maximo')
insert into tb_alertas_operando ("Tipo", "Descricao") values (3, 'Minimo')
