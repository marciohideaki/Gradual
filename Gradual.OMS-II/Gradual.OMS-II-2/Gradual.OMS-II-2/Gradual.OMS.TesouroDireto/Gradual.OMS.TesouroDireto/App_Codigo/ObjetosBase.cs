using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Gradual.OMS.TesouroDireto.Lib.Dados;
using log4net;

namespace Gradual.OMS.TesouroDireto.App_Codigo
{
    public abstract class ObjetosBase
    {
        #region | Atributos

        protected String gInnerXML;
        protected Boolean gHasAlert;
        protected String gID_Sucesso;
        protected String gID_Sucesso_Descricao;
        protected List<AlertaInfo> gAlertas;

        private static readonly ILog gLogger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region | Propriedades

        public String InnerXML
        {
            get { return gInnerXML; }
            set { gInnerXML = value; }
        }

        public Boolean HasAlert
        {
            get { return gHasAlert; }
        }

        public String ID_Sucesso
        {
            get { return gID_Sucesso; }
        }

        public String ID_Sucesso_Descricao
        {
            get { return gID_Sucesso_Descricao; }
        }

        public List<AlertaInfo> Alertas
        {
            get { return gAlertas; }
        }

        #endregion

        #region | Construtores

        public ObjetosBase()
        {
            this.gAlertas = new List<AlertaInfo>();
        }

        #endregion

        #region | Métodos

        protected void AtribDefaultValues()
        {
            gID_Sucesso_Descricao = "";
            gID_Sucesso = "";
            gHasAlert = false;
            gAlertas = new List<AlertaInfo>();
        }

        /// <summary>
        /// Pega o Status da resposta, se contem erros ou não e retorna o elemento Root da resposta 
        /// em elementRoot passado por referência
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="elementRoot"></param>
        protected void GetStatus(string xml, out XElement elementRoot)
        {
            gInnerXML = xml;
            TextReader txtReader = new StringReader(xml);
            XDocument xDoc = XDocument.Load(txtReader);
            elementRoot = xDoc.Root;

            XAttribute atrib = elementRoot.FirstAttribute;
            gID_Sucesso = atrib.Value;
            gID_Sucesso_Descricao = atrib.NextAttribute.Value;

            XElement elemAlertas = elementRoot.Element("ALERTAS"); //Buscando todos os alertas do retorno

            foreach (XElement item in elemAlertas.Elements())
            {
                gHasAlert = true;
                gAlertas.Add(new AlertaInfo()
                {
                    Codigo = item.Element("CODIGO").Value,
                    Descricao = item.Element("DESCRICAO").Value
                });
            }

            //--> Verificação de erro
            if (elementRoot.Attribute("ID_SUCESSO") != null
            && (elementRoot.Attribute("ID_SUCESSO").Value == "0"))
            {
                string lMensagemErro = "¬Falha na realização da operação";

                if (gAlertas != null && gAlertas.Count > 0)
                    gAlertas.ForEach(lAlerta =>
                    {
                        lMensagemErro = string.Concat(lMensagemErro, "\n", lAlerta.Codigo, " - ", lAlerta.Descricao);
                    });

                lMensagemErro += "¬";

                gLogger.Error(lMensagemErro); //--> Log de erro na produção do XML.

                throw new Exception(lMensagemErro);
            }
        }

        #endregion
    }
}
