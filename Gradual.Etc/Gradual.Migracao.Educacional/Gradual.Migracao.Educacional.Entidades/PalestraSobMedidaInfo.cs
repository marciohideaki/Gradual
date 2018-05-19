using System;

namespace Gradual.Migracao.Educacional.Entidades
{
    public class PalestraSobMedidaInfo
    {
        public int IdCursoPalestraSobMedidaOracle { get; set; }

        public int IdCursoPalestraSobMedidaSql { get; set; }

        public int IdPalestrante { get; set; }

        public int IdTema { get; set; }

        public int IdEstado { get; set; }

        public string DsMunicipio { get; set; }

        public string DsEndereco { get; set; }

        public string DsCep { get; set; }

        public string DsLocal { get; set; }

        public string TpLocal { get; set; }

        public char TpSolicitante { get; set; }

        public DateTime DtCriacao { get; set; }

        public DateTime DtDataHoraInicio { get; set; }

        public DateTime DtDataHoraFim { get; set; }

        public string DsPublicoAlvo { get; set; }

        public int QtPessoas { get; set; }

        public char StSituacao { get; set; }
    }
}
