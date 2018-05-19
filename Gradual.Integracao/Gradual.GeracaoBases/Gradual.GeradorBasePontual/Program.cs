using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Gradual.GeracaoBasesDB.Lib;

namespace Gradual.GeradorBasePontual
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            intra.gradual.financial.ValidateLogin validateLogin = new intra.gradual.financial.ValidateLogin();

            validateLogin.Username = "brocha";
            validateLogin.Password = "gradual12345*";

            intra.gradual.financial.PosicaoCotistaWS ws = new intra.gradual.financial.PosicaoCotistaWS();

            ws.ValidateLoginValue = validateLogin;

            intra.gradual.financial.PosicaoCotistaViewModel[] vm = ws.Exporta(null, null, null);

            //MessageBox.Show("Recebeu " + vm.Length + " registros");

            int len = vm.Length;

            DataSet dataSet = new DataSet();
            //DataTableReader xreader = dataSet.CreateDataReader();

            DataTable tabela = parseToDataTable<intra.gradual.financial.PosicaoCotistaViewModel>(vm);
            dataSet.Tables.Add(tabela);

            if (ExcelCreator.CreateExcel(dataSet, @"c:\temp\fundos.xlsx", "Fundos"))
            {
            }
        }

        private static DataTable parseToDataTable<T>(T[] arr)
        {
            DataTable deta = new DataTable(typeof(T).Name);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    deta.Columns.Add(info.Name, typeof(System.String) );
                    continue;
                }

                deta.Columns.Add(info.Name, info.PropertyType);
            }
            deta.AcceptChanges();


            foreach (T item in arr)
            {
                DataRow row = deta.NewRow();

                foreach (PropertyInfo property in item.GetType().GetProperties())
                {
                    //if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    //{
                    //    row[property.Name] = 
                    //    continue;
                    //}
                    row[property.Name] = property.GetValue(item, null);
                }
                deta.Rows.Add(row);
            }

            deta.AcceptChanges();
            return deta;
        }

    }
}
