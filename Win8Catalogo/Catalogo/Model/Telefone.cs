using Win8Catalogo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win8Catalogo.Catalogo.Model
{
    public class Telefone : BindableBase
    {
        private string _Numero;
        public string Numero
        {
            get { return this._Numero; }
            set { this.SetProperty(ref this._Numero, value); }
        }

        private string _Tipo;
        public string Tipo
        {
            get { return this._Tipo; }
            set { this.SetProperty(ref this._Tipo, value); }
        }
    }
}
