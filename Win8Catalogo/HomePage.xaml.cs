using Win8Catalogo.Data;
using Win8Catalogo.Catalogo.Logic;
using Win8Catalogo.Catalogo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Win8Catalogo.Common;

// O modelo de item itens agrupados página está documentado em http://go.microsoft.com/fwlink/?LinkId=234231
namespace Win8Catalogo
{
    /// <summary>
    ///Uma página que exibe uma coleção de itens agrupados.
    /// </summary>
    public sealed partial class HomePage : LayoutAwarePage
    {
        public static Popup settingsPopup = null;

        public HomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Preenche a página com o conteúdo passado durante a navegação. 
        /// Qualquer estado salvo também é fornecido ao recriar uma página de uma sessão anterior.
        /// </summary>
        /// <param name="navigationParameter">O valor do parâmetro passado
        /// <see cref="Frame.Navigate(Type, Object)"/> Quando está página foi inicialmente requisitada.
        /// </param>
        /// <param name="pageState">Um dicionário de estado preservado por esta página durante uma sessão anterior. 
        /// Será nulo primeira vez que uma página é visitada.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Criar um modelo de dados apropriado para o seu domínio do problema para substituir os dados de exemplo
            //var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            
            //this.DefaultViewModel["Groups"] = Win8CatalogApplication.Instance.Categorias;
            this.DefaultViewModel["Groups"] = Win8CatalogApplication.Instance.GetTopItens(4);

            EnableLiveTile.CreateLiveTile.ShowliveTile(true, Win8CatalogApplication.Instance.Empresa.Nome);

            EmpresaNome.Text = Win8CatalogApplication.Instance.Empresa.Nome;
            EmpresaDescricao.Text = Win8CatalogApplication.Instance.Empresa.Sobre;
            EmpresaRazao.Text = Win8CatalogApplication.Instance.Empresa.RazaoSocial;
            ImgEmpresa.Source = new BitmapImage(new Uri(new Uri("ms-appx:///"), Win8CatalogApplication.Instance.Empresa.ImageUrl));

        }

        /// <summary>
        /// Chamado quando um cabeçalho de grupo é clicado.
        /// </summary>
        /// <param name="sender">O botão usado como um cabeçalho de grupo para o grupo selecionado.</param>
        /// <param name="e">Os dados de eventos que descreve como o clique foi iniciado.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determinar o que o grupo representa instância de botão
            var group = (sender as FrameworkElement).DataContext;

            // Navegue até a página de destino apropriado, configurando a nova página, passando as informações necessárias como parâmetro de navegação
            //this.Frame.Navigate(typeof(WomenGroupedItemsPage), "AllGroups");
            this.Frame.Navigate(typeof(CategoriaPage), ((Categoria)group).ID);
        }


        void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }
                
        /// <summary>
        /// Chamado quando um item dentro de um grupo é clicado.
        /// </summary>
        /// <param name="sender">O GridView (ou ListView quando a aplicação estiver no modo snapped)
        /// apresentando o item clicado.</param>
        /// <param name="e">Dados do evento que descreve o item clicado.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navegue para a página de destino apropriado, configurando a nova página, 
            //passando as informações necessárias como parâmetro de navegação
           
            this.Frame.Navigate(typeof(ItemPage), ((Item)e.ClickedItem).ID);
        }

       
        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                settingsPopup.IsOpen = false;
            }
        }

        void settingsPopup_Closed(object sender, object e)
        {
            Window.Current.Activated -= Current_Activated;
        }
    }
}
