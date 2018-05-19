create or replace
PROCEDURE prc_sel_cliente_custodia (
  Retorno              OUT GLOBALPKG.Retorno,
  IdCliente             IN INT )
AS
BEGIN
OPEN Retorno for
     select cod_cart
     ,      desc_cart
     ,      tipo_grup
     ,      tipo_merc
     ,      cod_cli
     ,      qtde_disp
     ,      qtde_aexe_cpa
     ,      qtde_aexe_vda
     ,      case tipo_merc
                 when 'FUT' then concat(cod_comm, cod_seri)
                 when 'OPD' then concat(cod_comm, cod_seri)
                 when 'OPF' then concat(cod_comm, cod_seri)
            else cod_neg
            end cod_neg
     ,      (qtde_disp - qtde_blqd + qtde_alqd + qtde_da1 + qtde_da2 + qtde_da3 + qtde_exec_cpa + qtde_exec_vda) qtde_atual
     ,      NOME_EMP_EMI
   from vcfposicao
   where cod_cli = IdCliente
   and (qtde_disp != 0
        or qtde_aexe_cpa != 0
        or qtde_aexe_vda != 0
        or (qtde_disp - qtde_blqd + qtde_alqd + qtde_da1 + qtde_da2 + qtde_da3 + qtde_exec_cpa + qtde_exec_vda) != 0);
END;
