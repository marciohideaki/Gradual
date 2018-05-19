using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gradual.OMS.Seguranca.Lib;

namespace Gradual.Intranet.Www.App_Codigo
{
    public class ValidarSegurancaAttribute : Attribute
    {
        public ItemSegurancaInfo Seguranca { get; set; }

        public ValidarSegurancaAttribute(string permissoes, string grupos, string perfis)
        {
            this.Seguranca = new ItemSegurancaInfo();
            
            this.Seguranca.Grupos = new List<string>();

            this.Seguranca.Grupos.Add(grupos);
            
            this.Seguranca.Permissoes = new List<string>();

            this.Seguranca.Permissoes.Add(permissoes);
            
            this.Seguranca.Perfis = new List<string>();

            this.Seguranca.Perfis.Add(perfis);

            
            this.Seguranca =
                new ItemSegurancaInfo()
                {
                    Grupos = grupos.Split(';').ToList<string>(),
                    Perfis = perfis.Split(';').ToList<string>(),
                    Permissoes = permissoes.Split(';').ToList<string>(),
                    TipoAtivacao = ItemSegurancaAtivacaoTipoEnum.TodasAsCondicoes
                };
        }
    }
}
