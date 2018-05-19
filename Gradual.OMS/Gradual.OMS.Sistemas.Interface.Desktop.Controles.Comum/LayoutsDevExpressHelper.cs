using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;

namespace Gradual.OMS.Sistemas.Interface.Desktop.Controles.Comum
{
    /// <summary>
    /// Serve como repositorio de layouts dos controles
    /// DevExpress e auxilia na serialização, desserialização
    /// desses controles
    /// </summary>
    [Serializable]
    public class LayoutsDevExpressHelper
    {
        /// <summary>
        /// Lista de layouts, onde a chave é o nome do controle
        /// e o valor é a serialização do controle
        /// </summary>
        public Dictionary<string, LayoutDevExpressItemInfo> Layouts { get; set; }

        /// <summary>
        /// Faz a serialização do layout devexpress
        /// </summary>
        /// <param name="controle"></param>
        public void SalvarLayout(object controle)
        {
            // Inicializa
            LayoutDevExpressItemInfo layoutInfo = new LayoutDevExpressItemInfo();
            Type tipoControle = controle.GetType();

            // Recebe o nome do controle
            layoutInfo.NomeControle =
                (string)tipoControle.InvokeMember(
                    "Name", System.Reflection.BindingFlags.GetProperty, null, controle, (object[])null);

            // Traduz o tipo
            if (tipoControle == typeof(GridView))
            {
                layoutInfo.TipoControle = LayoutDevExpressTipoEnum.GridView;
                layoutInfo.NomeControlePai = ((GridView)controle).GridControl.Name;
            }
            else if (tipoControle == typeof(LayoutControl))
            {
                layoutInfo.TipoControle = LayoutDevExpressTipoEnum.LayoutManager;
            }

            // Serializa layout 
            MemoryStream ms = new MemoryStream();
            tipoControle.InvokeMember(
                "SaveLayoutToStream", System.Reflection.BindingFlags.InvokeMethod, 
                null, controle, new object[] { ms });
            ms.Position = 0;
            StreamReader reader = new StreamReader(ms);

            // Salva o layout
            layoutInfo.Layout = reader.ReadToEnd();

            // Salva layout 
            if (this.Layouts.ContainsKey(layoutInfo.NomeControle))
                this.Layouts[layoutInfo.NomeControle] = layoutInfo;
            else
                this.Layouts.Add(layoutInfo.NomeControle, layoutInfo);
        }

        /// <summary>
        /// Faz a recuperacao dos layouts dos controles devexpress
        /// </summary>
        /// <param name="controleHost">Controle, ou form, utilizado como host dos controles devexpress</param>
        public void RecuperarLayouts(Control controleHost)
        {
            // Varre todos os layouts
            foreach (KeyValuePair<string, LayoutDevExpressItemInfo> item in this.Layouts)
            {
                // Recupera o controle
                object controle = null;
                if (item.Value.TipoControle == LayoutDevExpressTipoEnum.GridView)
                {
                    GridControl controlePai = (GridControl)acharControle(controleHost, item.Value.NomeControlePai);
                    foreach (GridView view in controlePai.Views)
                    {
                        if (view.Name == item.Value.NomeControle)
                        {
                            controle = view;
                            break;
                        }
                    }
                }
                else
                {
                    controle = acharControle(controleHost, item.Value.NomeControle);
                }
                Type tipoControle = controle.GetType();

                // Pede a desserializacao
                MemoryStream ms = new MemoryStream();
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(item.Value.Layout);
                writer.Flush();
                ms.Position = 0;
                tipoControle.InvokeMember(
                    "RestoreLayoutFromStream", System.Reflection.BindingFlags.InvokeMethod, 
                    null, controle, new object[] { ms });
            }
        }

        private Control acharControle(Control controlePai, string nomeControle)
        {
            if (controlePai.Name == nomeControle)
                return controlePai;
            else if (controlePai.Controls.Count == 0)
                return null;
            else
                foreach (Control controle in controlePai.Controls)
                {
                    Control controleEncontrado = acharControle(controle, nomeControle);
                    if (controleEncontrado != null)
                        return controleEncontrado;
                }
            return null;
        }

        /// <summary>
        /// Construtor default
        /// </summary>
        public LayoutsDevExpressHelper()
        {
            this.Layouts = new Dictionary<string, LayoutDevExpressItemInfo>();
        }
    }
}
