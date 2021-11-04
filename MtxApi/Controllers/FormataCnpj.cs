using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MtxApi.Controllers
{
    public class FormataCnpj
    {
        public static string FormatarCNPJ(string CNPJ)
        {
            //verificar a qtd de digitos
            if (CNPJ.Length == 14)
            {
                return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
               
            }
            else
            {
                return CNPJ;
            }
            
        }
    }
}