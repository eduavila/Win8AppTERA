using Win8Catalogo.Common;
using Win8Catalogo.Catalogo.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Win8Catalogo.Catalogo.Model
{
    public class Item:BindableBase
    {

        public Item() { }

        public Item(string ID, string Nome, string Descricao, string valor, string ImageUrl, string SubTitulo, string Uri, string IDCategoria)
        {
            this.ID = ID;
            this.Nome = Nome;
            this.Valor = valor;
            this.ImageUrl = ImageUrl;
            this.SubTitulo = SubTitulo;
            this.Uri = Uri;
            this.IDCategoria = IDCategoria;
        }

        private string _Id;
        public string ID
        {
            get { return this._Id; }
            set { this.SetProperty(ref this._Id, value); }
        }

        private string _Nome;
        public string Nome
        {
            get { return this._Nome; }
            set { this.SetProperty(ref this._Nome, value); }
        }

        private string _Descricao;
        public string Descricao
        {
            get { return this._Descricao; }
            set { this.SetProperty(ref this._Descricao, value); }
        }

        private string _Valor;
        public string Valor
        {
            get { return this._Valor; }
            set { this.SetProperty(ref this._Valor, value); }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return this._imageUrl; }
            set { this.SetProperty(ref this._imageUrl, value); }
        }

        private string _SubTitulo;
        public string SubTitulo
        {
            get { return this._SubTitulo; }
            set { this.SetProperty(ref this._SubTitulo, value); }
        }

        private string _Uri;
        public string Uri
        {
            get { return this._Uri; }
            set { this.SetProperty(ref this._Uri, value); }
        }

        [IgnoreDataMember]
        public Categoria Categoria
        {
            get { return Win8CatalogApplication.Instance.GetCategory(IDCategoria); }
        }

        private string _IDCategoria;
        [IgnoreDataMember]
        public string IDCategoria
        {
            get { return this._IDCategoria; }
            set { this.SetProperty(ref this._IDCategoria, value); }
        }

    }
}
