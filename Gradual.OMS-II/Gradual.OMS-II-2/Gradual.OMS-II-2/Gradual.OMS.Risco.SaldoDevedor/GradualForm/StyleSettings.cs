using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GradualForm
{
    public static class StyleSettings
    {
        #region Globais

        public static int LarguraDaBorda_Form = 2;

        public static byte Padding_BarraDeTitulo = 4;

        public static int LarguraDaBorda_Botao = 1;

        public static int DeslocaImagem_Botao = 3;
        public static int DeslocaImagem_Label = 2;

        public static int LarguraDaBorda_TextBox = 1;

        public static Font Fonte_BarraDeTitulo = new Font("Calibri", 10.75F, FontStyle.Regular);
        public static Font Fonte_Grid = new Font("Calibri", 8.75F, FontStyle.Regular);
        public static Font Fonte_Gabriel = new Font("Verdana", 9, FontStyle.Regular);

        public static StringFormat FormatoDaFonte_BarraDeTitulo = new StringFormat() { LineAlignment = StringAlignment.Center };
        public static StringFormat FormatoDaFonte_Botao = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public static StringFormat FormatoDaFonte_TextBox = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

        /// <summary>
        ///     Cor de fundo do formulário
        /// </summary>
        public static Color Form_BackGround_Color = Color.Transparent;
        /// <summary>
        ///     Cor da borda do formulário
        /// </summary>
        public static Color Form_Border_Color = Color.Transparent;
        /// <summary>
        ///     Cor de fundo da barra de título do formulário
        /// </summary>
        public static Color Form_TitleBar_BackColor = Color.Transparent;
        /// <summary>
        ///     Cor de fundo da barra de título do formulário de boleta de Venda
        /// </summary>
        public static Color Form_TitleBar_BackColor_Boleta_Venda = Color.Transparent;
        /// <summary>
        ///     Cor de fundo da barra de título do formulário
        /// </summary>
        public static Color Form_TitleBar_BackColor_Boleta_Compra = Color.Transparent;
        /// <summary>
        ///     Cor do texto da barra de título do formulário
        /// </summary>
        public static Color Form_TitleBarText_ForeColor = Color.Transparent;
        /// <summary>
        ///     Cor do texto da barra de título do formulário
        /// </summary>
        public static Color Form_TitleBarText_ForeColor_Boleta = Color.Transparent;
        /// <summary>
        ///     Cor do botão minimizar quando não está selecionado
        /// </summary>
        public static Color Cor_BotaoMin_Fraca = Color.Transparent;
        /// <summary>
        ///     Cor do texto do botão
        /// </summary>
        public static Color Button_Text_Forecolor = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do botão
        /// </summary>
        public static Color Button_BackGround_Color_Start = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do botão
        /// </summary>
        public static Color Button_BackGround_Color_Finish = Color.Transparent;

        /// <summary>
        ///     Cor da borda do botão
        /// </summary>
        public static Color Button_Border_Color = Color.Transparent;
        /// <summary>
        ///     Cor da borda quando o botão está destacado (Hover)
        /// </summary>
        public static Color Button_HoverBorder_Color = Color.Transparent;
        /// <summary>
        ///     Cor do texto da caixa de texto
        /// </summary>
        public static Color TextBox_Text_ForeColor = Color.Transparent;
        /// <summary>
        ///     Cor do fundo da caixa de texto
        /// </summary>
        public static Color TextBox_BackGround_Color_Start = Color.Transparent;
        /// <summary>
        ///     Cor do fundo da caixa de texto
        /// </summary>
        public static Color TextBox_BackGround_Color_Finish = Color.Transparent;
        /// <summary>
        ///     Cor da borda da caixa de texto
        /// </summary>
        public static Color Cor_Borda_TextBox = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do painel
        /// </summary>
        public static Color Panel_BackGround_Color_Start = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do painel
        /// </summary>
        public static Color Panel_BackGround_Color_Finish = Color.Transparent;
        /// <summary>
        ///     Cor da borda interna do combo
        /// </summary>
        public static Color Cor_BordaInternaCombo = Color.Transparent;
        /// <summary>
        ///     Cor do texto do rótulo
        /// </summary>
        public static Color Label_Text_ForeColor = Color.Transparent;
        /// <summary>
        ///     Cor do texto do rótulo
        /// </summary>
        public static Color Label_BackGround_Color_Start = Color.Transparent;
        /// <summary>
        ///     Cor do texto do rótulo
        /// </summary>
        public static Color Label_BackGround_Color_Finish = Color.Transparent;

        /// <summary>
        ///     Cor do texto dos itens de menu
        /// </summary>
        public static Color Cor_Txt_MenuItems = Color.Transparent;
        /// <summary>
        ///     Cor do cabcalho da grid
        /// </summary>
        public static Color Cor_CabecalhoGrid_Div1 = Color.Transparent;
        /// <summary>
        ///     Cor do texto do cabecalho da grid
        /// </summary>
        public static Color Cor_Txt_CabecalhoGrid = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do cabecalho da grid
        /// </summary>
        public static Color Cor_Fd_CabecalhoGrid = Color.Transparent;
        /// <summary>
        ///     Cor de fundo da grid
        /// </summary>
        public static Color Cor_Fd_Grid = Color.Transparent;
        /// <summary>
        ///     Cor da linha da grid
        /// </summary>
        public static Color Cor_Txt_LinhaGrid = Color.Transparent;
        /// <summary>
        ///     Primeira cor de fundo da linha da grid (Zebra)
        /// </summary>
        public static Color Cor_Fd_LinhaGridA = Color.Transparent;
        /// <summary>
        ///     Segnda cor de fundo da linha da grid (Zebra)
        /// </summary>
        public static Color Cor_Fd_LinhaGridB = Color.Transparent;
        /// <summary>
        ///     Cor de fundo da linha selecionada na grid
        /// </summary>
        public static Color Cor_Fd_LinhaGrid_Selecionada = Color.Transparent;
        /// <summary>
        ///     Cor do texto da linha selecionada na grid
        /// </summary>
        public static Color Cor_Txt_LinhaGrid_Selecionada = Color.Transparent;
        /// <summary>
        ///     Cor inicial do fundo do painel (degrade)
        /// </summary>
        public static Color Grad_Fd_Panel_CorInicial = Color.Transparent;
        /// <summary>
        ///     Cor final do fundo do painel (degrade)
        /// </summary>
        public static Color Grad_Fd_Panel_CorFinal = Color.Transparent;
        /// <summary>
        ///     Cor da borda do botao da barra de ferramentas
        /// </summary>
        public static Color Button_Border_ColorToolBar = Color.Transparent;
        /// <summary>
        ///     Cor do fundo do botao na barra de ferramentas
        /// </summary>
        public static Color Button_BackGround_ColorToolBar = Color.Transparent;
        /// <summary>
        ///     Cor do fundo do item de menu quando destacado (hover)
        /// </summary>
        public static Color Cor_Fd_ItemMenu_Hover = Color.Transparent;
        /// <summary>
        ///     Cor do fundo do botao da barra de status
        /// </summary>
        public static Color Button_BackGround_ColorStatus = Color.Transparent;
        /// <summary>
        ///     Cor do texto do botao da barra de status
        /// </summary>
        public static Color Button_Text_ForecolorStatus = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do CheckBox
        /// </summary>
        public static Color CheckBox_BackGround_Color_Start = Color.Transparent;
        /// <summary>
        ///     Cor de fundo do CheckBox
        /// </summary>
        public static Color CheckBox_BackGround_Color_Finish = Color.Transparent;

        //private static Color Button_Border_Color   = Color.FromArgb(68, 81, 94);

        public static Brush Brush_Fd_Form = new SolidBrush(Color.Transparent);
        public static Brush Brush_Borda_Form = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_BarraDeTitulo = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_BarraDeTitulo_Boleta_Compra = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_BarraDeTitulo_Boleta_Venda = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_BarraDeTitulo_Boleta_CompraVenda = new SolidBrush(Color.Transparent);
        public static Brush Brush_Txt_BarraDeTitulo = new SolidBrush(Color.Transparent);
        public static Brush Brush_Txt_BarraDeTitulo_Boleta = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_Panel = new SolidBrush(Color.Transparent);
        public static Brush Brush_BotaoMin_Fraca = new SolidBrush(Color.Transparent);
        public static Brush Brush_Txt_Botao = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_Botao = new SolidBrush(Color.Transparent);
        public static Brush Brush_Borda_Botao = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_TextBox = new SolidBrush(Color.Transparent);
        public static Brush Brush_Borda_TextBox = new SolidBrush(Color.Transparent);
        public static Brush Brush_Txt_Label = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_CabecalhoGrid = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_CabecalhoGrid_Boleta_Compra = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_CabecalhoGrid_Boleta_Venda = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_BotaoToolBar = new SolidBrush(Color.Transparent);
        public static Brush Brush_Fd_ItemMenu_Hover = new SolidBrush(Color.Transparent);

        public static Pen Pen_Borda_Botao = new Pen(Color.Transparent);
        public static Pen Pen_Hover_Borda_Botao = new Pen(Color.Transparent);
        public static Pen Pen_Borda_TextBox = new Pen(Color.Transparent);
        public static Pen Pen_CabecalhoGrid_Div1 = new Pen(Color.Transparent);
        public static Pen Pen_BordaInternaCombo = new Pen(Color.Transparent);
        public static Pen Pen_Borda_BotaoToolBar = new Pen(Color.Transparent);

        private static string gSkinSelecionada = "";

        #endregion

        #region Propriedades

        public static string SkinSelecionada
        {
            get
            {
                return gSkinSelecionada;
            }
        }

        #endregion

        #region Métodos Private

        public static void CarregarSkin(string pNomeDaSkin)
        {
            if (pNomeDaSkin == "clara")
            {
                gSkinSelecionada = pNomeDaSkin;

                Form_BackGround_Color = Color.FromArgb(225, 222, 217);
                Form_Border_Color = Color.FromArgb(172, 172, 172);
                Form_TitleBar_BackColor = Color.FromArgb(172, 172, 172);
                Form_TitleBarText_ForeColor = Color.FromArgb(36, 36, 36);
                Form_TitleBarText_ForeColor_Boleta = Color.FromArgb(0, 0, 0);

                TextBox_Text_ForeColor = Color.FromArgb(36, 36, 36);
                TextBox_BackGround_Color_Start = Color.FromArgb(228, 228, 228);
                TextBox_BackGround_Color_Finish = Color.FromArgb(228, 228, 228);
                Cor_Borda_TextBox = Color.FromArgb(142, 142, 142);

                Panel_BackGround_Color_Start = Color.FromArgb(225, 222, 217);
                Panel_BackGround_Color_Finish = Color.FromArgb(225, 222, 217);
                Cor_BordaInternaCombo = Color.FromArgb(142, 142, 142);

                Label_Text_ForeColor = Color.FromArgb(36, 36, 36);
                Cor_Txt_MenuItems = Color.FromArgb(36, 36, 36);
                Cor_Txt_CabecalhoGrid = Color.FromArgb(36, 36, 36);

                Cor_Fd_CabecalhoGrid = Color.FromArgb(216, 216, 216);
                Cor_Fd_Grid = Color.FromArgb(255, 255, 255);

                Cor_Txt_LinhaGrid = Color.FromArgb(255, 255, 255);

                Cor_Fd_LinhaGridA = Color.FromArgb(255, 255, 255);
                Cor_Fd_LinhaGridB = Color.FromArgb(239, 239, 239);

                Cor_Fd_LinhaGrid_Selecionada = Color.FromArgb(190, 209, 247);
                Cor_Txt_LinhaGrid_Selecionada = Color.FromArgb(36, 36, 36);

                Button_Border_ColorToolBar = Color.FromArgb(172, 172, 172);
                Button_BackGround_ColorToolBar = Color.FromArgb(219, 219, 219);

                Cor_BotaoMin_Fraca = Color.FromArgb(172, 172, 172);

                Button_Text_Forecolor = Color.FromArgb(98, 98, 98);
                Button_BackGround_Color_Start = Color.FromArgb(66, 66, 66);
                Button_BackGround_Color_Finish = Color.FromArgb(66, 66, 66);

                Button_Border_Color = Color.FromArgb(66, 66, 66);

                Button_HoverBorder_Color = Color.FromArgb(98, 98, 98);

                Cor_CabecalhoGrid_Div1 = Color.FromArgb(164, 164, 164);

                Cor_Fd_ItemMenu_Hover = Color.FromArgb(190, 209, 247);

                Button_BackGround_ColorStatus = Color.FromArgb(219, 219, 219);
                Button_Text_ForecolorStatus = Color.FromArgb(172, 172, 172);


            }

            if (pNomeDaSkin == "GTI")
            {
                gSkinSelecionada = pNomeDaSkin;

                Form_BackGround_Color = Color.FromArgb(22, 22, 22);
                Form_Border_Color = Color.FromArgb(98, 98, 98);
                Form_TitleBar_BackColor = Color.FromArgb(28, 28, 28);
                Form_TitleBarText_ForeColor = Color.FromArgb(206, 211, 220);
                Form_TitleBarText_ForeColor_Boleta = Color.White;

                TextBox_Text_ForeColor = Color.FromArgb(255, 255, 255);
                TextBox_BackGround_Color_Start = Color.FromArgb(32, 32, 32);
                TextBox_BackGround_Color_Finish = Color.FromArgb(32, 32, 32);
                Cor_Borda_TextBox = Color.FromArgb(52, 52, 52);

                Panel_BackGround_Color_Start = Color.FromArgb(22, 22, 22);
                Panel_BackGround_Color_Finish = Color.FromArgb(22, 22, 22);
                Cor_BordaInternaCombo = Color.FromArgb(77, 77, 77);

                Label_Text_ForeColor = Color.FromArgb(203, 203, 203);
                Label_BackGround_Color_Start = Color.FromArgb(22,22,22);
                Label_BackGround_Color_Finish = Color.FromArgb(22, 22, 22);

                Cor_Txt_MenuItems = Color.FromArgb(255, 255, 255);
                Cor_Txt_CabecalhoGrid = Color.FromArgb(232, 232, 232);

                Cor_Fd_CabecalhoGrid = Color.FromArgb(0, 66, 156);
                Cor_Fd_Grid = Color.FromArgb(22, 22, 22);

                Cor_Txt_LinhaGrid = Color.FromArgb(255, 255, 255);

                Cor_Fd_LinhaGridA = Color.FromArgb(42, 42, 42);
                Cor_Fd_LinhaGridB = Color.FromArgb(62, 62, 62);

                Cor_Fd_LinhaGrid_Selecionada = Color.FromArgb(255, 35, 47, 57);
                Cor_Txt_LinhaGrid_Selecionada = Color.FromArgb(255, 255, 255, 255);

                Button_Border_ColorToolBar = Color.FromArgb(98, 98, 98);
                Button_BackGround_ColorToolBar = Color.FromArgb(66, 66, 66);

                Cor_BotaoMin_Fraca = Color.FromArgb(128, 128, 128);

                Button_Text_Forecolor = Color.FromArgb(255, 255, 255);
                Button_BackGround_Color_Start = Color.FromArgb(38, 54, 75);
                Button_BackGround_Color_Finish = Color.FromArgb(58, 74, 95);

                Button_Border_Color = Color.FromArgb(44, 50, 57);

                Button_HoverBorder_Color = Color.FromArgb(154, 154, 155);

                Cor_CabecalhoGrid_Div1 = Color.FromArgb(42, 42, 42);

                Cor_Fd_ItemMenu_Hover = Color.FromArgb(0, 66, 156);

                Button_BackGround_ColorStatus = Color.FromArgb(38, 54, 75);
                Button_Text_ForecolorStatus = Color.FromArgb(255, 255, 255);
            }

            if (pNomeDaSkin == "Gradual.Consulta")
            {
                gSkinSelecionada = pNomeDaSkin;

                Form_BackGround_Color = Color.FromArgb(62, 62, 62);
                Form_Border_Color = Color.FromArgb(98, 98, 98);
                Form_TitleBar_BackColor = Color.FromArgb(28, 28, 28);
                Form_TitleBarText_ForeColor = Color.FromArgb(206, 211, 220);
                Form_TitleBarText_ForeColor_Boleta = Color.White;

                TextBox_Text_ForeColor = Color.FromArgb(255, 255, 255);
                TextBox_BackGround_Color_Start = Color.FromArgb(62, 62, 62);
                TextBox_BackGround_Color_Finish = Color.FromArgb(62, 62, 62);
                Cor_Borda_TextBox = Color.FromArgb(52, 52, 52);

                Panel_BackGround_Color_Start = Color.FromArgb(62, 62, 62);
                Panel_BackGround_Color_Finish = Color.FromArgb(62, 62, 62);
                Cor_BordaInternaCombo = Color.FromArgb(77, 77, 77);

                Label_Text_ForeColor = Color.FromArgb(203, 203, 203);
                Label_BackGround_Color_Start = Color.FromArgb(62, 62, 62);
                Label_BackGround_Color_Finish = Color.FromArgb(62, 62, 62);

                Cor_Txt_MenuItems = Color.FromArgb(255, 255, 255);
                Cor_Txt_CabecalhoGrid = Color.FromArgb(232, 232, 232);

                Cor_Fd_CabecalhoGrid = Color.FromArgb(0, 66, 156);
                Cor_Fd_Grid = Color.FromArgb(22, 22, 22);

                Cor_Txt_LinhaGrid = Color.FromArgb(255, 255, 255);

                Cor_Fd_LinhaGridA = Color.FromArgb(42, 42, 42);
                Cor_Fd_LinhaGridB = Color.FromArgb(62, 62, 62);

                Cor_Fd_LinhaGrid_Selecionada = Color.FromArgb(255, 35, 47, 57);
                Cor_Txt_LinhaGrid_Selecionada = Color.FromArgb(255, 255, 255, 255);

                Button_Border_ColorToolBar = Color.FromArgb(98, 98, 98);
                Button_BackGround_ColorToolBar = Color.FromArgb(66, 66, 66);

                Cor_BotaoMin_Fraca = Color.FromArgb(128, 128, 128);

                Button_Text_Forecolor = Color.FromArgb(255, 255, 255);
                Button_BackGround_Color_Start = Color.FromArgb(50, 130, 170);
                Button_BackGround_Color_Finish = Color.FromArgb(90, 170, 210);

                Button_Border_Color = Color.FromArgb(44, 50, 57);

                Button_HoverBorder_Color = Color.FromArgb(154, 154, 155);

                Cor_CabecalhoGrid_Div1 = Color.FromArgb(42, 42, 42);

                Cor_Fd_ItemMenu_Hover = Color.FromArgb(0, 66, 156);

                Button_BackGround_ColorStatus = Color.FromArgb(38, 54, 75);
                Button_Text_ForecolorStatus = Color.FromArgb(255, 255, 255);

                CheckBox_BackGround_Color_Start = Color.FromArgb(152, 152, 152);
                CheckBox_BackGround_Color_Finish = Color.FromArgb(122, 122, 122);
            }

            if(String.IsNullOrEmpty(pNomeDaSkin))
            {
                gSkinSelecionada = "";

                Form_BackGround_Color = Color.FromArgb(0, 0, 0);
                Form_Border_Color = Color.FromArgb(98, 98, 98);
                Form_TitleBar_BackColor = Color.FromArgb(28, 28, 28);
                Form_TitleBarText_ForeColor = Color.FromArgb(206, 211, 220);
                Form_TitleBarText_ForeColor_Boleta = Color.White;

                TextBox_Text_ForeColor = Color.FromArgb(255, 255, 255);
                TextBox_BackGround_Color_Start = Color.FromArgb(228, 228, 228);
                TextBox_BackGround_Color_Finish = Color.FromArgb(172, 172, 172);
                Cor_Borda_TextBox = Color.FromArgb(54, 54, 55);

                Panel_BackGround_Color_Start = Color.FromArgb(0, 0, 0);
                Panel_BackGround_Color_Finish = Color.FromArgb(0, 0, 0);
                Cor_BordaInternaCombo = Color.FromArgb(77, 77, 77);

                Label_Text_ForeColor = Color.FromArgb(203, 203, 203);
                Cor_Txt_MenuItems = Color.FromArgb(255, 255, 255);
                Cor_Txt_CabecalhoGrid = Color.FromArgb(232, 232, 232);

                Cor_Fd_CabecalhoGrid = Color.FromArgb(0, 66, 156);
                Cor_Fd_Grid = Color.FromArgb(22, 22, 22);

                Cor_Txt_LinhaGrid = Color.FromArgb(255, 255, 255);

                Cor_Fd_LinhaGridA = Color.FromArgb(31, 31, 31);
                Cor_Fd_LinhaGridB = Color.FromArgb(22, 22, 22);

                Cor_Fd_LinhaGrid_Selecionada = Color.FromArgb(255, 35, 47, 57);
                Cor_Txt_LinhaGrid_Selecionada = Color.FromArgb(255, 255, 255, 255);

                Button_Border_ColorToolBar = Color.FromArgb(98, 98, 98);
                Button_BackGround_ColorToolBar = Color.FromArgb(66, 66, 66);

                Cor_BotaoMin_Fraca = Color.FromArgb(128, 128, 128);

                Button_Text_Forecolor = Color.FromArgb(255, 255, 255);
                Button_BackGround_Color_Start = Color.FromArgb(38, 54, 75);
                Button_BackGround_Color_Finish = Color.FromArgb(38, 54, 75);

                Button_Border_Color = Color.FromArgb(44, 50, 57);

                Button_HoverBorder_Color = Color.FromArgb(154, 154, 155);

                Cor_CabecalhoGrid_Div1 = Color.FromArgb(42, 42, 42);

                Cor_Fd_ItemMenu_Hover = Color.FromArgb(0, 66, 156);

                Button_BackGround_ColorStatus = Color.FromArgb(38, 54, 75);
                Button_Text_ForecolorStatus = Color.FromArgb(255, 255, 255);
            }

            Brush_Fd_Form = new SolidBrush(Form_BackGround_Color);
            Brush_Borda_Form = new SolidBrush(Form_Border_Color);

            Brush_Fd_BarraDeTitulo = new SolidBrush(Form_TitleBar_BackColor);
            Brush_Txt_BarraDeTitulo = new SolidBrush(Form_TitleBarText_ForeColor);
            Brush_Txt_BarraDeTitulo_Boleta = new SolidBrush(Form_TitleBarText_ForeColor_Boleta);

            Brush_BotaoMin_Fraca = new SolidBrush(Cor_BotaoMin_Fraca);


            Brush_Txt_Botao = new SolidBrush(Button_Text_Forecolor);
            Brush_Fd_Botao = new SolidBrush(Button_BackGround_Color_Start);

            Brush_Borda_Botao = new SolidBrush(Button_Border_Color);
            Pen_Borda_Botao = new Pen(Button_Border_Color, (float)LarguraDaBorda_Botao);
            Pen_Hover_Borda_Botao = new Pen(Button_HoverBorder_Color, (float)LarguraDaBorda_Botao);

            Brush_Fd_TextBox = new SolidBrush(TextBox_BackGround_Color_Start);

            Brush_Borda_TextBox = new SolidBrush(Cor_Borda_TextBox);
            Pen_Borda_TextBox = new Pen(Cor_Borda_TextBox, (float)LarguraDaBorda_TextBox);

            Brush_Fd_Panel = new SolidBrush(Panel_BackGround_Color_Start
                
                );

            Pen_BordaInternaCombo = new Pen(Cor_BordaInternaCombo);

            Brush_Txt_Label = new SolidBrush(Label_Text_ForeColor);

            Pen_CabecalhoGrid_Div1 = new Pen(Cor_CabecalhoGrid_Div1, 1);

            Brush_Fd_CabecalhoGrid = new SolidBrush(Cor_Fd_CabecalhoGrid);

            Pen_Borda_BotaoToolBar = new Pen(Button_Border_ColorToolBar);

            Brush_Fd_BotaoToolBar = new SolidBrush(Button_BackGround_ColorToolBar);

            Brush_Fd_ItemMenu_Hover = new SolidBrush(Cor_Fd_ItemMenu_Hover);
        }

        #endregion

    }
}