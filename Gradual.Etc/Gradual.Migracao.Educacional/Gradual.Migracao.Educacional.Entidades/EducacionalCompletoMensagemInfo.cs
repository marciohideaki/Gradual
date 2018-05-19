using System.Collections.Generic;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class EducacionalCompletoMensagemInfo
    {
        public List<AvaliacaoInteresseInfo> AvaliacaoInteresseInfo { get; set; }

        public List<AvaliacaoPalestraInfo> AvaliacaoPalestraInfo { get; set; }

        public List<ClienteCursoPalestraInfo> ClienteCursoPalestraInfo { get; set; }

        public List<CursoPalestraInfo> CursoPalestraInfo { get; set; }

        public List<CursoPalestraOnLineInfo> CursoPalestraOnLineInfo { get; set; }

        public List<EstadoInfo> EstadoInfo { get; set; }

        public List<FichaPerfilInfo> FichaPerfilInfo { get; set; }

        public List<LocalidadeInfo> LocalidadeInfo { get; set; }

        public List<NivelInfo> NivelInfo { get; set; }

        public List<PalestranteInfo> PalestranteInfo { get; set; }

        public List<PalestraSobMedidaInfo> PalestraSobMedidaInfo { get; set; }

        public List<PerfilInfo> PerfilInfo { get; set; }

        public List<TemaInfo> TemaInfo { get; set; }

        public List<UsuarioInfo> UsuarioInfo { get; set; }
    }
}
