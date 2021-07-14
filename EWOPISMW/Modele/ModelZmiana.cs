using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWOPISMW.Modele
{
    class ModelZmiana
    {
        public string NazwaZmiany { get; set; }
        public int idZmiany;

        public string PobierzDoListy()
        {
            return "id=[" + idZmiany + "] " + NazwaZmiany;
        }

        public static int? pobierzIdDo(string selectedValuZComboboxa)
        {
            if(selectedValuZComboboxa == "")
            {
                return null;
            }
            string[] idzm = selectedValuZComboboxa.Split('[', ']');
            return Convert.ToInt32(idzm[1]);
        }
    }

   
}
