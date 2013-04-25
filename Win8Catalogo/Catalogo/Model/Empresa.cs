using Win8Catalogo.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win8Catalogo.Catalogo.Model
{
    public class Empresa:BindableBase
    {
        private string _Nome;
        public string Nome
        {
            get { return this._Nome; }
            set { this.SetProperty(ref this._Nome, value); }
        }

        private string _RazaoSocial;
        public string RazaoSocial
        {
            get { return this._RazaoSocial; }
            set { this.SetProperty(ref this._RazaoSocial, value); }
        }

        private string _Endereco;
        public string Endereco
        {
            get { return this._Endereco; }
            set { this.SetProperty(ref this._Endereco, value); }
        }

        private string _Website;
        public string Website
        {
            get { return this._Website; }
            set { this.SetProperty(ref this._Website, value); }
        }

        private ObservableCollection<Telefone> _Telefones;
        public ObservableCollection<Telefone> Telefones
        {
            get { return this._Telefones; }
            set { this.SetProperty(ref this._Telefones, value); }
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get { return this._imageUrl; }
            set { this.SetProperty(ref this._imageUrl, value); }
        }

        private string _Sobre;
        public string Sobre
        {
            get { return this._Sobre; }
            set { this.SetProperty(ref this._Sobre, value); }
        }

        private string _SettingsSobre;
        public string SettingsSobre
        {
            get { return this._SettingsSobre; }
            set { this.SetProperty(ref this._SettingsSobre, value); }
        }

        private string _SettingsPolitica;
        public string SettingsPolitica
        {
            get { return this._SettingsPolitica; }
            set { this.SetProperty(ref this._SettingsPolitica, value); }
        }

        private string _SettingsContato;
        public string SettingsContato
        {
            get { return this._SettingsContato; }
            set { this.SetProperty(ref this._SettingsContato, value); }
        }

        private string _ShareTexto;
        public string ShareTexto
        {
            get { return this._ShareTexto; }
            set { this.SetProperty(ref this._ShareTexto, value); }
        }
        
        private string _LiveTileBig1;
        public string LiveTileBig1
        {
            get { return this._LiveTileBig1; }
            set { this.SetProperty(ref this._LiveTileBig1, value); }
        }

        private string _LiveTileBig2;
        public string LiveTileBig2
        {
            get { return this._LiveTileBig2; }
            set { this.SetProperty(ref this._LiveTileBig2, value); }
        }

        private string _LiveTileSmall1;
        public string LiveTileSmall1
        {
            get { return this._LiveTileSmall1; }
            set { this.SetProperty(ref this._LiveTileSmall1, value); }
        }

        private string _LiveTileSmall2;
        public string LiveTileSmall2
        {
            get { return this._LiveTileSmall2; }
            set { this.SetProperty(ref this._LiveTileSmall2, value); }
        }
    }
}
