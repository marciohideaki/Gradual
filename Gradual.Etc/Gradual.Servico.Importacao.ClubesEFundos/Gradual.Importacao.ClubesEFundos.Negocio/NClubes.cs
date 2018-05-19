using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gradual.MinhaConta.Dados;
using Gradual.MinhaConta.Entidade;

namespace Gradual.MinhaConta.Negocio
{
    public class NClubes
    {
        public List<EClubes> ConsultarClubes(string cpfCnpj)
        {
            return new DClubes().ConsultarClubes(cpfCnpj);
        }

        public void AtualizarClubes(string path)
        {
            if (!System.IO.File.Exists(path))
                return;
            List<EClubes> lstClubes = LoadClubs(path);
            if (null != lstClubes && lstClubes.Count > 0)
            {
                AtualizarClubes(lstClubes);
            }
        }

        private void AtualizarClubes(List<EClubes> lstClubes)
        {
            lstClubes = Filtra(lstClubes);
            //lstClubes = AtualizaCpf(lstClubes);
            new DClubes().AtualizarClubes(lstClubes);
        }

        private List<EClubes> LoadClubs(string path)
        {
            string[] strLinhas;
            string[] strTemp;

            using (var stream = new StreamReader(path))
            {
                strLinhas = stream.ReadToEnd().Split('\n');
            }

            System.IO.File.Delete(path.Replace(".txt", "_processado.txt"));
            System.IO.File.Move(path, path.Replace(".txt", "_processado.txt"));

            if (strLinhas.Count() < 1)
                return null;

            var lstClubes = new List<EClubes>();
            var eClubes = new EClubes();

            foreach (string str in strLinhas)
            {
                if (string.IsNullOrEmpty(str))
                    continue;

                strTemp = str.Split(';');

                eClubes = new EClubes();
                eClubes.Codigo_Bolsa = strTemp[00].DBToInt32();
                eClubes.Codigo_da_Empresa = strTemp[01].DBToInt32();
                eClubes.Nome_do_Clube = strTemp[02].DBToString();
                eClubes.Data = strTemp[03].DBToDateTime();
                eClubes.Cotacao = strTemp[04].DBToDecimal();
                eClubes.Codigo_Cliente = strTemp[05].DBToInt64();
                eClubes.Nome_do_Cliente = strTemp[06].DBToString();
                eClubes.Dcquantidade = strTemp[07].DBToInt32();
                eClubes.Dccotacao = strTemp[08].DBToInt32();
                eClubes.Data_Inicial = strTemp[09].DBToDateTime();
                eClubes.Saldo_Quantidade = strTemp[10].DBToDecimal();
                eClubes.Saldo_Bruto = strTemp[11].DBToDecimal();
                eClubes.IR = strTemp[12].DBToDecimal();
                eClubes.IOF = strTemp[13].DBToDecimal();
                eClubes.Rendimento = strTemp[14].DBToDecimal();
                eClubes.Performance = strTemp[15].DBToDecimal();
                eClubes.Saldo_Liquido = strTemp[16].DBToDecimal();
                eClubes.Saldo_Inicial = strTemp[17].DBToDecimal();
                eClubes.Codigo_do_Agente = strTemp[18].DBToInt32();
                eClubes.Nome_do_Agente = strTemp[19].ToString();

                lstClubes.Add(eClubes);
            }
            return lstClubes;
        }

        private List<EClubes> Filtra(List<EClubes> lstClubes)
        {
            var lListaFiltradaClubes = from ret in lstClubes orderby ret.Codigo_Cliente, ret.Nome_do_Clube, ret.Data select ret;

            var auxCodigo = long.MinValue;
            var auxClube = string.Empty;

            DateTime auxData;

            if (lListaFiltradaClubes.ToList().Count > 0)
            {
                EClubes Clube1 = (EClubes)lListaFiltradaClubes.ToList()[0];
                auxCodigo = Clube1.Codigo_Cliente.Value;
                auxClube = Clube1.Nome_do_Clube;
                auxData = Clube1.Data.Value;
            }

            List<EClubes> lstClubesReturn = new List<EClubes>();
            EClubes ClubeAnterior = (EClubes)lListaFiltradaClubes.ToList()[0];
            EClubes Clube;

            foreach (var ent in lListaFiltradaClubes)
            {
                Clube = (EClubes)ent;

                if (Clube.Codigo_Cliente.Value != auxCodigo || Clube.Nome_do_Clube != auxClube)
                    lstClubesReturn.Add(ClubeAnterior);

                auxCodigo = Clube.Codigo_Cliente.Value;
                auxClube = Clube.Nome_do_Clube;
                auxData = Clube.Data.Value;
                ClubeAnterior = (EClubes)ent;
            }

            return lstClubesReturn;
        }

        private List<EClubes> AtualizaCpf(List<EClubes> lstClubes)
        {
            var dClubes = new DClubes();

            foreach (EClubes item in lstClubes)
                item.CPF_CGC = dClubes.GetCpf(item.Codigo_Cliente.ToString());

            return lstClubes;
        }
    }
}
