using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CelulaAtivacao;
using System.IO;
using System.Text;

namespace CelulaAtivacao
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    FormataDatas();
                    this.GetAssessor();
                }
                catch (Exception ex)
                {
                    Alert(ex.Message);
                }
            }
        }

        private void LoadGridView()
        {
            int? CodigoBovespa;
            if (txtCodigoBovespa.Text == string.Empty) { CodigoBovespa = null; } else { CodigoBovespa = Convert.ToInt32(txtCodigoBovespa.Text); }

            int? CodigoAssessor;
            if (DdlAssessor.SelectedItem.Text == string.Empty) { CodigoAssessor = null; } else { CodigoAssessor = Convert.ToInt32(DdlAssessor.SelectedItem.Value); }

            if (DDLMomento.SelectedItem.Value == "M1")
            {
                GvDados.DataSource = new BOCelulaAtivacao().GetDataM1(Convert.ToDateTime(txtDataInicial.Text), Convert.ToDateTime(txtDataFinal.Text), CodigoBovespa, CodigoAssessor);
                GvDados.DataBind();

            }
            else
            {
                if (DDLMomento.SelectedItem.Value == "M2")
                {
                    GvDados.DataSource = new BOCelulaAtivacao().GetDataM2(Convert.ToDateTime(txtDataInicial.Text), Convert.ToDateTime(txtDataFinal.Text), CodigoBovespa, CodigoAssessor);
                    GvDados.DataBind();
                }
                else
                {
                    GvDados.DataSource = new BOCelulaAtivacao().GetDataM3(Convert.ToDateTime(txtDataInicial.Text), Convert.ToDateTime(txtDataFinal.Text), CodigoBovespa, CodigoAssessor);
                    GvDados.DataBind();
                }

            }

            if (GvDados.Rows.Count > 0)
            {
                lnkExcel.Enabled = true;
            }
            else
            {
                lnkExcel.Enabled = false;
            }
        }

        protected void GvDados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GvDados.PageIndex = e.NewPageIndex;
                this.LoadGridView();
            }
            catch (Exception ex)
            {
                Alert(ex);
            }
        }

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaIsData(txtDataInicial.Text, "Data Inicial");
                ValidaIsData(txtDataFinal.Text, "Data Final");

                int? CodigoBovespa;
                if (txtCodigoBovespa.Text == string.Empty) { CodigoBovespa = null; } else { CodigoBovespa = Convert.ToInt32(txtCodigoBovespa.Text); }

                int? CodigoAssessor;
                if (DdlAssessor.SelectedItem.Text == string.Empty) { CodigoAssessor = null; } else { CodigoAssessor = Convert.ToInt32(DdlAssessor.SelectedItem.Value); }

                if (DDLMomento.SelectedItem.Value == "M1")
                {
                    GerarExcel(new BOCelulaAtivacao().GetDataM1(Convert.ToDateTime(txtDataInicial.Text).AddDays(-1), Convert.ToDateTime(txtDataFinal.Text).AddDays(1), CodigoBovespa, CodigoAssessor));

                }
                else
                {
                    if (DDLMomento.SelectedItem.Value == "M2")
                    {
                        GerarExcel(new BOCelulaAtivacao().GetDataM2(Convert.ToDateTime(txtDataInicial.Text).AddDays(-1), Convert.ToDateTime(txtDataFinal.Text).AddDays(1), CodigoBovespa, CodigoAssessor));

                    }
                    else
                    {
                        GerarExcel(new BOCelulaAtivacao().GetDataM3(Convert.ToDateTime(txtDataInicial.Text).AddDays(-1), Convert.ToDateTime(txtDataFinal.Text).AddDays(1), CodigoBovespa, CodigoAssessor));
                    }

                }
            }
            catch (Exception ex)
            {
                Alert(ex);
            }
        }

        private void GerarExcel(DataTable Data)
        {

            Response.Clear();

            Response.AddHeader("content-disposition", "attachment; filename=NomeCelulaAtivacao.xls");

            Response.Charset = "";

            Response.ContentType = "application/vnd.xls";

            System.IO.StringWriter stringWrite = new System.IO.StringWriter();

            System.Web.UI.HtmlTextWriter htmlWrite =
            new HtmlTextWriter(stringWrite);

            DataGrid dg = new DataGrid();

            dg.DataSource = Data;
            dg.DataBind();

            dg.RenderControl(htmlWrite);

            char[] Char = stringWrite.ToString().ToCharArray();
            byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(Char);
            string StringUTF8 = System.Text.Encoding.UTF8.GetString(utf8Bytes);

            Response.Write(stringWrite);
            Response.End();
        }

        protected void lnkConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidaIsData(txtDataInicial.Text,"Data Inicial");
                ValidaIsData(txtDataFinal.Text, "Data Final");
                this.LoadGridView();
            }
            catch (Exception ex)
            {
                Alert(ex);
            }
        }

        protected void DDLMomento_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtCodigoBovespa.Text = string.Empty;

                if (DDLMomento.SelectedItem.Value == "M3")
                {
                    txtCodigoBovespa.Enabled = true;
                }
                else
                {
                    txtCodigoBovespa.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Alert(ex);
            }

        }

        protected void GvDados_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (e.SortDirection == SortDirection.Ascending)
                {
                    e.SortDirection = SortDirection.Descending;
                }
                else
                {
                    e.SortDirection = SortDirection.Ascending;
                }

                LoadGridView();
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }

        }

        protected void GvDados_Sorted(object sender, EventArgs e)
        {

        }

        private void GetAssessor()
        {
            DdlAssessor.DataSource = new BOCelulaAtivacao().GetAssessor();
            DdlAssessor.DataTextField = "Value";//"NM_ASSESSOR";
            DdlAssessor.DataValueField = "Id";//"CD_ASSESSOR";
            DdlAssessor.DataBind();
            DdlAssessor.Items.Add(string.Empty);
            DdlAssessor.SelectedIndex = DdlAssessor.Items.IndexOf(DdlAssessor.Items.FindByValue(string.Empty));
        }

        public DateTime ValidaIsData(string Value, string campo)
        {
            DateTime retorno;

            if (DateTime.TryParse(Value, out retorno))
            {
                if (retorno > DateTime.Now.AddDays(1))
                {
                    throw new Exception(string.Format("A data para o campo {0} não pode ser maior que hoje", campo));
                }
                else
                {
                    if (retorno < new DateTime(1970, 1, 1))
                    {
                        throw new Exception(string.Format("O ano do campo {0} deve ser maior que 1970", campo));
                    }
                    else
                    {
                        return retorno;
                    }
                }
            }
            else
                throw new Exception(string.Format("Informe uma data válida para o campo {0}", campo));
        }

        public void Alert(Exception ex) {
            Alert(ex.Message);
        }
        
        public void Alert(string strMessage)
        {
            strMessage = strMessage.Replace(";", " ");
            strMessage = strMessage.Replace(Environment.NewLine, " ");
            strMessage = strMessage.Replace("\n", " ");
            StringBuilder scriptString = new StringBuilder("<script language=JavaScript>");
            scriptString.AppendFormat("window.alert('{0}');", strMessage.Replace("'", string.Empty));
            scriptString.Append("</script>");
            Page.RegisterStartupScript("alerta", scriptString.ToString());
        }

        public void FormataDatas()
        {
            txtDataInicial.Attributes.Add("onKeypress", "return FormataData(this,event);");
            txtDataFinal.Attributes.Add("onKeypress", "return FormataData(this,event);");
            txtDataInicial.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            txtDataFinal.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}


