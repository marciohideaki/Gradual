using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Gradual.StockMarket.Excel2003.AddIn
{
    public class ConversorDeImagens : System.Windows.Forms.AxHost
    {
        public ConversorDeImagens() : base(null) { }

        public static stdole.IPictureDisp Converter(System.Drawing.Image pImage)
        {
            return (stdole.IPictureDisp)System.Windows.Forms.AxHost.GetIPictureDispFromPicture(pImage);
        }
    }

    public static class CarregadorDeImagens
    {
        public static stdole.IPictureDisp Carregar(Bitmap pImagem)
        {
            ImageList lList = new ImageList();

            lList.Images.Add(pImagem);

            return ConversorDeImagens.Converter(lList.Images[0]);
        }
    }
}
