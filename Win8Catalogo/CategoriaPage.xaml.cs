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
using Windows.UI.Xaml.Navigation;
using Win8Catalogo.Common;

// O Grupo Detalhe de modelo de item Página está documentado em http://go.microsoft.com/fwlink/?LinkId=234229

namespace Win8Catalogo
{
    /// <summary>
    /// Uma página que exibe uma visão geral de um único grupo, incluindo uma visualização dos itens dentro do grupo.
    /// </summary>
    public sealed partial class CategoriaPage : LayoutAwarePage
    {
        public static Popup settingsPopup = null;
        public CategoriaPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Preenche a página com o conteúdo passado durante a navegação. 
        /// Qualquer estado salvo também é fornecido ao recriar uma página de uma sessão anterior.
        /// </summary>
        /// <param name="navigationParameter">O valor do parâmetro enviado para 
        /// <see cref="Frame.Navigate(Type, Object)"/> Quando está página foi inicialmente requisitada.
        /// </param>
        /// <param name="pageState">Dicionário de estado preservado por esta página durante uma sessão anterior.
        /// Será null na primeira vez que a página é visitada.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Crie um modelo de dados apropriado para o seu problema, para trocar os dados de exmplo

            var categoria = Win8CatalogApplication.Instance.GetCategory(navigationParameter.ToString());
            this.DefaultViewModel["Group"] = categoria;
            this.DefaultViewModel["Items"] = categoria.Items;
        }

        /// <summary>
        /// Disparado quando um item é clicado.
        /// </summary>
        /// <param name="sender">O GridView (ou ListView quando a aplicação estiver no modo snapped)
        /// apresentando o item clicado.</param>
        /// <param name="e">Dados do evento que descreve o item clicado.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navegue para a página de destino apropriado, configurando a nova página, 
            //passando as informações necessárias como parâmetro de navegação
            var itemId = ((Item)e.ClickedItem).ID;

            this.Frame.Navigate(typeof(ItemPage), itemId);
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

        void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }

    }
}
