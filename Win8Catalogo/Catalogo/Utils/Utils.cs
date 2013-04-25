using Win8Catalogo.Catalogo.Logic;
using Win8Catalogo.Catalogo.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win8Catalogo.Catalogo.Utils
{
    public static class Utils
    {
        public static string GetTelefoneNumeros(ObservableCollection<Telefone> telefones)
        {
            string strRetorno = string.Empty;
            if (telefones.Count > 0)
            {
                foreach (Telefone telefone in Win8CatalogApplication.Instance.Empresa.Telefones)
                {
                    strRetorno += string.Format("{0}: {1} \r\n", telefone.Tipo, telefone.Numero);
                }

            }
            return strRetorno;                
        }
    }
}
