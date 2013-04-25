using Win8Catalogo.Common;
using Win8Catalogo.Catalogo.Model;
using Win8Catalogo.Catalogo.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Search;
using Windows.UI.Xaml.Controls;

namespace Win8Catalogo.Catalogo.Logic
{
    public class Win8CatalogApplication : BindableBase
    {
        private static Win8CatalogApplication _Instance = new Win8CatalogApplication();

        public static Win8CatalogApplication Instance
        {
            get { return _Instance; }
        }

        private Empresa _Empresa;
        public Empresa Empresa
        {
            get { return _Empresa; }
            set { _Empresa = value; }
        }

        private ObservableCollection<Categoria> _categorias = new ObservableCollection<Categoria>();
        public ObservableCollection<Categoria> Categorias
        {
            get { return this._categorias; }
        }


        private ObservableCollection<Categoria> _resultadoPesquisa = new ObservableCollection<Categoria>();
        public ObservableCollection<Categoria> SearchResult
        {
            get { return this._resultadoPesquisa; }
            set { this.SetProperty(ref this._resultadoPesquisa, value); }
        }


        private Dictionary<string, Categoria> CategoriaDicionario = new Dictionary<string, Categoria>();
        private Dictionary<string, Item> ItemDicionario = new Dictionary<string, Item>();

        private Task LoadTask;

        public async Task LoadAsync()
        {
            if (LoadTask == null)
                LoadTask = InternalLoad();

            await LoadTask;
        }


        private async Task InternalLoad()
        {

            Categorias.Clear();
            CategoriaDicionario.Clear();
            ItemDicionario.Clear();


            var xmlEmpresaFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Catalogo\data\Empresa\Empresa.xml");

            try
            {
                var empresa = await ObjectPersister.LoadAsync<Empresa>(xmlEmpresaFile.Path, ObjectPersisterLocation.Path);
                Empresa = empresa;
            }
            catch { }


            var xmlCategoriaFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Catalogo\data\Categorias");
            var quizCategoriesQuery = xmlCategoriaFolder.CreateFileQueryWithOptions(new QueryOptions(CommonFileQuery.OrderByName, new string[] { ".xml" }));
            foreach (var files in await quizCategoriesQuery.GetFilesAsync())
            {
                try
                {
                    var category = await ObjectPersister.LoadAsync<Categoria>(files.Path, ObjectPersisterLocation.Path);
                    if (category.Items == null)
                    {
                        category.Items = new ObservableCollection<Item>();
                    }
                    else
                    {
                        foreach (Item item in category.Items)
                        {
                            item.IDCategoria = category.ID;
                            ItemDicionario[item.ID] = item;
                        }
                    }
                    CategoriaDicionario[category.ID] = category;
                    Categorias.Add(category);
                }
                catch { }
            }

        }



        public Categoria GetCategory(string uniqueId)
        {
            Categoria category = null;
            CategoriaDicionario.TryGetValue(uniqueId, out category);
            return category;
        }

        public Item GetItem(string uniqueId)
        {
            Item item = null;
            ItemDicionario.TryGetValue(uniqueId, out item);
            return item;
        }

        /// <summary>
        /// Retorna uma coleção de categoria, com a quantidade de itens especificada.
        /// </summary>
        /// <param name="Quantidade"></param>
        /// <returns></returns>
        public ObservableCollection<Categoria> GetTopItens(int Quantidade)
        {
            var CategoriasHome = new ObservableCollection<Categoria>();

            foreach (Categoria categoria in _categorias)
            {
                var categoria_busca = new Categoria();
                categoria_busca.ID = categoria.ID;
                categoria_busca.Nome = categoria.Nome;
                categoria_busca.SubTitulo = categoria.SubTitulo;
                categoria_busca.ImageUrl = categoria.ImageUrl;
                categoria_busca.Items =  new ObservableCollection<Item>(categoria.Items.Take(Quantidade));
                categoria_busca.Descricao = categoria.Descricao;

                CategoriasHome.Add(categoria_busca);
            }

            return CategoriasHome;
        }


        /// <summary>
        /// Realiza a busca em toda a estrutura carregada de quizes, para não alterar a instância utilizada na lógica do quiz irei duplicar os registros encontrados.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public static void ProcessQueryText(string search)
        {

            Win8CatalogApplication.Instance.SearchResult.Clear();

            ObservableCollection<Categoria> categories = Win8CatalogApplication.Instance.Categorias;


            foreach (Categoria categoria in categories)
            {
                Categoria searchCategory = null;
                bool founded;

                foreach (Item item in categoria.Items)
                {
                    founded = false;

                    if (item.Nome.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1
                        || item.Descricao.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1
                        || item.SubTitulo.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1
                        || item.Categoria.Descricao.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1
                        || item.SubTitulo.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1
                        || item.Categoria.Nome.IndexOf(search, StringComparison.CurrentCultureIgnoreCase) > -1
                    )
                    {
                        founded = true;
                    }

                    if (founded)
                    {
                        if (searchCategory == null)
                        {
                            searchCategory = new Categoria(item.Categoria.ID, item.Categoria.Nome, item.Categoria.SubTitulo, item.Categoria.Descricao, item.Categoria.ImageUrl);
                            searchCategory.Items = new ObservableCollection<Item>();
                        }

                        searchCategory.Items.Add(item);
                    }

                }

                if (searchCategory != null)
                {
                    Win8CatalogApplication.Instance.SearchResult.Add(searchCategory);
                }
            }
        }




    }
}
