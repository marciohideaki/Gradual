using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AS.Dados;
using AS.Messages;


namespace AS.Negocio
{
    public class BAuthenticationServer : DAuthenticationServer
    {
         private enum EOperation{
            SignIn = 1,
            SignOut = 0
        };

         public BAuthenticationServer()
         {
           
        }

        public override object InsertAccess(SignIn _SignIn){
            return base.InsertAccess(_SignIn);
        }

        public override bool UpdateAccess(SignOut _SignOut){
            return base.UpdateAccess(_SignOut);
        }

    }
}
