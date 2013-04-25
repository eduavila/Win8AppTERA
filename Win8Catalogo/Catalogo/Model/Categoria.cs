using Win8Catalogo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Win8Catalogo.Catalogo.Model
{
    public class Categoria : BindableBase
    {

        private string _Id;
        private string _Nome;
        private string _SubTitulo;
        private string _Descricao;
        private string _imageUrl;
        private ObservableCollection<Item> _Items = new ObservableCollection<Item>();

        public Categoria() { 
        }

        public Categoria(string ID, string Nome, string SubTitulo, string Descricao, string ImageUrl)
        {
            this.ID = ID;
            this.Nome = Nome;
            this.SubTitulo = SubTitulo;
            this.Descricao = Descricao;
            this.ImageUrl = ImageUrl;

            Items = new ObservableCollection<Item>();
        }
        
        public string ID
        {
            get { return this._Id; }
            set { this.SetProperty(ref this._Id, value); }
        }

        public string Nome
        {
            get { return this._Nome; }
            set { this.SetProperty(ref this._Nome, value); }
        }


        public string SubTitulo
        {
            get { return this._SubTitulo; }
            set { this.SetProperty(ref this._SubTitulo, value); }
        }

        
        public string Descricao
        {
            get { return this._Descricao; }
            set { this.SetProperty(ref this._Descricao, value); }
        }

        
        public string ImageUrl
        {
            get { return this._imageUrl; }
            set { this.SetProperty(ref this._imageUrl, value); }
        }

        
        public ObservableCollection<Item> Items
        {
            get { return this._Items; }
            set { this.SetProperty(ref this._Items, value); }
        }
    }
}
