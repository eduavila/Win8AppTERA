using Win8Catalogo.Catalogo.Logic;
using Win8Catalogo.Catalogo.Model;
using Win8Catalogo.Catalogo.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Win8Catalogo.Common;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Win8Catalogo.SettingsFlyouts
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ContactUs : LayoutAwarePage
    {
        public ContactUs()
        {
            this.InitializeComponent();

            if (!String.IsNullOrEmpty(Win8CatalogApplication.Instance.Empresa.SettingsContato))
            {

                string Telefones = Utils.GetTelefoneNumeros(Win8CatalogApplication.Instance.Empresa.Telefones);
                Win8CatalogApplication.Instance.Empresa.SettingsContato = Win8CatalogApplication.Instance.Empresa.SettingsContato.Replace("#TELEFONE#", Telefones);

                if (!String.IsNullOrEmpty(Win8CatalogApplication.Instance.Empresa.Website))
                {
                    Win8CatalogApplication.Instance.Empresa.SettingsContato = Win8CatalogApplication.Instance.Empresa.SettingsContato.Replace("#URL#", Win8CatalogApplication.Instance.Empresa.Website);
                }

                Win8CatalogApplication.Instance.Empresa.SettingsContato = Win8CatalogApplication.Instance.Empresa.SettingsContato.Replace("#URL#", string.Empty);

                txtContato.Text = Win8CatalogApplication.Instance.Empresa.SettingsContato;
            }

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
        }

        /// <summary>
        /// Preserva estado associada a esta página caso o pedido de suspensão ou 
        /// a página é descartada a partir do cache de navegação.
        /// Os valores devem estar de acordo com os requisitos de serialização de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">Um dicionário vazio a ser preenchido com serializável estado.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void CloseFlyout(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Popup)
                (this.Parent as Popup).IsOpen = false;

            SettingsPane.Show();
        }
    }
}
