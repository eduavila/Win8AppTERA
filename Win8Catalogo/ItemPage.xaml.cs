using Win8Catalogo.Data;
using Win8Catalogo.Catalogo.Logic;
using Win8Catalogo.Catalogo.Model;
using Win8Catalogo.Catalogo.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Win8Catalogo.Common;

// Esta página está documentado em http://go.microsoft.com/fwlink/?LinkId=234232

namespace Win8Catalogo
{
    /// <summary>
    /// Uma página que exibe os detalhes para um único item dentro de um grupo, 
    /// permitindo gestos para percorrer outros itens pertencentes ao mesmo grupo.
    /// </summary>
    public sealed partial class ItemPage : LayoutAwarePage
    {
        private DataTransferManager dataTransferManager;
        private DataPackage requestData;
        private Item CurrentItem;
        private static Uri _baseUri = new Uri("ms-appx:///");

        public ItemPage()
        {
            this.InitializeComponent();
            flipView.SelectionChanged += flipView_SelectionChanged;

        }



        void flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = (Item)e.AddedItems[0];

            pageTitle.Text = item.Nome;
            pageSubtitle.Text = item.Categoria.Nome;
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
            // Permitir salvo estado de página para substituir o item inicial para exibir
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: Criar um modelo de dados apropriado para o seu domínio do problema para substituir os dados de exemplo
            //var item = SampleDataSource.GetItem((String)navigationParameter);
            CurrentItem = Win8CatalogApplication.Instance.GetItem(navigationParameter.ToString());

            this.DefaultViewModel["Item"] = CurrentItem;

            //this.DefaultViewModel["Group"] = item.Categoria;
            this.DefaultViewModel["Items"] = CurrentItem.Categoria.Items;
            this.flipView.SelectedItem = CurrentItem;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Register the current page as a share source.
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            this.dataTransferManager.TargetApplicationChosen += dataTransferManager_TargetApplicationChosen;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // Unregister the current page as a share source.
            this.dataTransferManager.DataRequested -= new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(this.OnDataRequested);
            this.dataTransferManager.TargetApplicationChosen -= dataTransferManager_TargetApplicationChosen;

        }

        // When share is invoked (by the user or programatically) the event handler we registered will be called to populate the datapackage with the
        // data to be shared.
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {

            if (GetShareContent(e.Request))
            {
                if (String.IsNullOrEmpty(e.Request.Data.Properties.Title))
                {
                    e.Request.FailWithDisplayText("Está faltando o título");
                }
            }
        }

        async void dataTransferManager_TargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            var item = (Item)this.flipView.SelectedItem;
            try
            {
                if (args.ApplicationName == "Email")
                {
                    await GetHtmlContent(requestData, Win8CatalogApplication.Instance.Empresa, item);
                }
                else if (args.ApplicationName == "People" || args.ApplicationName == "Pessoas")
                {
                    if (!String.IsNullOrEmpty(item.Uri))
                        requestData.SetUri(new Uri(item.Uri));
                    else
                        requestData.SetUri(new Uri(Win8CatalogApplication.Instance.Empresa.Website));
                }
                else {
                    if (!String.IsNullOrEmpty(item.Uri))
                        requestData.SetUri(new Uri(item.Uri));
                    else
                        requestData.SetUri(new Uri(Win8CatalogApplication.Instance.Empresa.Website));
                }
            }
            catch { }


        }

        private bool GetShareContent(DataRequest request)
        {
            bool succeeded = false;

            if (this.flipView.SelectedItem != null)
            {
                var item = (Item)this.flipView.SelectedItem;

                //                string customData = @"{
                //                    ""type"" : ""http://schema.org/Product"",
                //                    ""properties"" :
                //                    {
                //                    ""url"" : ""#URL#"",
                //                    ""description"" : ""#DESC#"",
                //                    ""name"" : ""#NAME#"",
                //                    ""model"" : ""#MODEL#"",
                //                    ""price"" : ""#PRICE#""
                //                    }
                //                }";

                //                customData = customData.Replace("#URL#", item.Uri).Replace("#DESC#", item.Descricao).Replace("#NAME#", item.Nome).Replace("#MODEL#", item.Categoria.Nome).Replace("#PRICE#", "R$ " + item.Valor);

                requestData = request.Data;
                //requestData.SetData("http://schema.org/Product", customData);
                requestData.Properties.Title = item.Nome == null ? "Imagem compartilhada" : item.Nome;
                requestData.Properties.Description = item.Descricao;

                RandomAccessStreamReference imageStreamRef = null;
                imageStreamRef = RandomAccessStreamReference.CreateFromUri(new Uri(_baseUri, item.ImageUrl));

                if (imageStreamRef != null)
                {

                    requestData.SetUri(new Uri(_baseUri, item.ImageUrl));
                    requestData.Properties.Title = string.Format("{0} - {1}", Win8CatalogApplication.Instance.Empresa.Nome, item.Nome);
                    requestData.Properties.Description = item.Descricao;
                    requestData.SetText(item.Descricao);

                    requestData.Properties.Thumbnail = imageStreamRef;
                    requestData.SetBitmap(imageStreamRef);

                    succeeded = true;
                }
            }
            else
            {
                request.FailWithDisplayText("Selecione a imagem que deseja compartilhar e tente novamente.");

            }
            return succeeded;
        }

        private async Task GetHtmlContent(DataPackage requestData, Empresa empresa, Item item)
        {
            var xml = new XmlDocument();
            var body = xml.CreateElement("DIV");

            xml.AppendChild(body);

            //Dados da empresa
            string Telefones = Utils.GetTelefoneNumeros(empresa.Telefones);

            string empresaShareText = empresa.ShareTexto.Replace("#TELEFONE#", Telefones).Replace("#URL#", empresa.Website);


            var empresaData = xml.CreateElement("P");
            empresaData.InnerText = empresaShareText;
            body.AppendChild(empresaData);

            body.AppendChild(xml.CreateElement("HR"));


            //Dados do Item compartilhado

            var Image = xml.CreateElement("IMG");
            Image.SetAttribute("SRC", new Uri(_baseUri, item.ImageUrl).ToString());
            body.AppendChild(Image);

            var Itemtipo = xml.CreateElement("H2");
            Itemtipo.InnerText = item.SubTitulo;

            var breakline = xml.CreateElement("BR");
            Itemtipo.AppendChild(breakline);

            var bold = xml.CreateElement("b");
            bold.InnerText = string.Format("R$ {0}", item.Valor);
            Itemtipo.AppendChild(bold);

            body.AppendChild(Itemtipo);

            var ImageDescription = xml.CreateElement("P");
            ImageDescription.InnerText = item.Descricao;
            body.AppendChild(ImageDescription);

            var localImage = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(item.ImageUrl.Replace("/", "\\"));

            requestData.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(xml.GetXml()));
            requestData.ResourceMap[new Uri(_baseUri, item.ImageUrl).ToString()] = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromFile(localImage);
        }

        /// <summary>
        /// Preserva estado associada a esta página caso o pedido de suspensão ou 
        /// a página é descartada a partir do cache de navegação.
        /// Os valores devem estar de acordo com os requisitos de serialização de <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">Um dicionário vazio a ser preenchido com serializável estado.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (Item)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.ID;
        }

        void btnHome_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }
    }
}
