using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Orbite.RV.Sistemas.Integracao.Bacen
{
    public class Utilitarios
    {
        public static DataTable Pivot(
                        DataTable tb,
                        DataColumn[] colunasPK,
                        DataColumn[] colunasPivot,
                        object[] valoresColunasPivot,
                        DataColumn[] colunasValores)
        {
            // Para facilitar, cria listas com as colecoes informadas
            List<DataColumn> listaColunasPK = new List<DataColumn>(colunasPK);
            List<DataColumn> listaColunasPivot = new List<DataColumn>(colunasPivot);
            List<DataColumn> listaColunasValores = new List<DataColumn>(colunasValores);
            List<DataColumn> listaColunasPKsemPivot = new List<DataColumn>();
            List<object> listaValoresColunasPivot = new List<object>();
            if (valoresColunasPivot != null)
                listaValoresColunasPivot.AddRange(valoresColunasPivot);

            // Cria tabela resultado
            DataTable tbResultado = new DataTable();

            // Monta os campos da PK da tabela resultado
            // (deixa para adicionar os campos de valores no loop, na medida em 
            // que estes campos possam ser identificados, pois dependem dos valores
            // da tabela)
            // A tabela resultado deverá ter:
            //   - As colunas da PK menos as colunas de Pivot e de Valores, 
            //   - Uma coluna indicando o tipo do valor, caso seja 
            //      pedido mais de uma coluna de valor
            foreach (DataColumn dc in listaColunasPK)
                if (!listaColunasPivot.Contains(dc))
                    listaColunasPKsemPivot.Add(tbResultado.Columns.Add(dc.ColumnName, dc.DataType));
            if (listaColunasValores.Count > 1)
                tbResultado.Columns.Add("_nomeValor", typeof(string));

            // Cria PK para poder fazer consultas
            DataColumn[] pkTemp = new DataColumn[listaColunasPKsemPivot.Count + (listaColunasValores.Count > 1 ? 1 : 0)];
            listaColunasPKsemPivot.CopyTo(pkTemp, 0);
            if (listaColunasValores.Count > 1)
                pkTemp[listaColunasPKsemPivot.Count] = tbResultado.Columns["_nomeValor"];
            tbResultado.PrimaryKey = pkTemp;

            // Varre a lista de valores preenchendo a tabela resultado
            foreach (DataRow dr in tb.Rows)
            {
                // Pega o valor do pivot
                string formatPivot = "";
                List<object> valoresPivot = new List<object>();
                for (int i = 0; i < listaColunasPivot.Count; i++)
                {
                    valoresPivot.Add(dr[listaColunasPivot[i]]);
                    formatPivot += "{" + i.ToString() + "}";
                }
                string valorPivot = string.Format(formatPivot, valoresPivot.ToArray());

                // Verifica se deve incluir no resultado
                if (listaValoresColunasPivot.Count == 0 || listaValoresColunasPivot.Contains(valorPivot))
                {
                    // Faz para cada coluna de valor
                    foreach (DataColumn dcValor in listaColunasValores)
                    {
                        // Cria array com valores da PK e aproveita para salvar valores 
                        // de uma possivel nova linha
                        DataRow drAdicionar = tbResultado.NewRow();
                        List<object> pk = new List<object>();
                        foreach (DataColumn dc in listaColunasPKsemPivot)
                            pk.Add(drAdicionar[dc] = dr[dc.ColumnName]);
                        if (listaColunasValores.Count > 1)
                            pk.Add(drAdicionar["_nomeValor"] = dcValor.ColumnName);

                        // Verifica se linha ja existe
                        DataRow drLinha = tbResultado.Rows.Find(pk.ToArray());
                        if (drLinha == null)
                            tbResultado.Rows.Add(drLinha = drAdicionar);

                        // Verifica se já existe coluna com o valor do pivot
                        if (!tbResultado.Columns.Contains(valorPivot))
                            tbResultado.Columns.Add(valorPivot, dcValor.DataType);

                        // Salva valor
                        drLinha[valorPivot] = dr[dcValor];
                    }
                }
            }

            // Retorna
            return tbResultado;

        }
    }
}
