using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gradual.Spider.Utils.Email.Entities;
using Gradual.Spider.Utils.Email;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigMailInfo cfg = new ConfigMailInfo();
            MessageMailInfo msg = new MessageMailInfo();
            cfg.SmtpHost = "ironport.gradual.intra";
            msg.From = "janestreet_report@gradualinvestimentos.com.br";
            msg.To = "ffurukawa@cortexit.com.br";
            
            msg.Subject = "JaneStreet DC";
            msg.Body = "Teste de email do JaneStreet";
            msg.FileAttach = @"c:\temp\JaneStreetDP-2014-12-11-11-09-48.xlsx;c:\temp\aa.txt";
            bool ret = SpiderMail.SendEmail(cfg, msg);
            MessageBox.Show(ret.ToString());
            


        }
    }
}
