using Win8Catalogo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Win8Catalogo.SettingsFlyouts;
using Win8Catalogo.Catalogo.Logic;

// O modelo App Grid está documentado em http://go.microsoft.com/fwlink/?LinkId=234226

namespace Win8Catalogo
{
    /// <summary>
    ///Fornece aplicação comportamento específico para complementar a classe de aplicativos padrão.
    /// </summary>
    sealed partial class App : Application
    {

        double settingsWidth = 370;
        Popup settingsPopup;
        /// <summary>
        /// Inicializa o objeto Application singleton. Esta é a primeira linha de código criado executado, e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Chamado quando a aplicação é iniciada normalmente pelo usuário final. 
        /// Outros pontos de entrada será usado quando o aplicativo é lançado para abrir um arquivo específico, para apresentar os resultados da pesquisa, e assim por diante.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {

            await Win8CatalogApplication.Instance.LoadAsync();

            // Não repita a inicialização do aplicativo quando o aplicativo já estiver rodando, apenas verifique se a Janela já está ativa
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Carregar o estado da aplicação anteriormente suspensa
            }

            // Create a Frame to act navigation context and navigate to the first page
            var rootFrame = new Frame();
            if (!rootFrame.Navigate(typeof(HomePage), "AllGroups"))
            {
                throw new Exception("Falhou ao criar a página inicial");
            }

            //Coloque o frame na janela atual e garantir que ele está ativo 
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        /// <summary>
        ///Chamado quando a execução do aplicativo está sendo suspenso. 
        ///O estado do aplicativo é salvo sem saber se a aplicação será encerrada ou reiniciada com o conteúdo da memória ainda intactas.
        /// </summary>
        /// <param name="sender">Origem do pedido de suspender</param>
        /// <param name="e">Detalhes sobre o pedido de suspender</param>
        //private void OnSuspending(object sender, SuspendingEventArgs e)
        //{
        //    var deferral = e.SuspendingOperation.GetDeferral();
        //    //TODO: Salva o estado da aplicãção e para qualquer atividade realizada em background
        //    deferral.Complete();
        //}

        /// <summary>
        /// Metodo é executado quando a aplicação cria uma Janela
        /// </summary>
        /// <param name="args"></param>
        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            //Quando uma janela é criada, utilizamos o evento CommandsRequested para que nossa aplicação apresente conteúdo quando o usuário clicar na opção de settings na WindowsBar
            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
        }

        /// <summary>
        /// Implementação do Handle do evento CommandsRequested, nele iremos incluir os itens que serão apresentados no painel de settings no aplicativo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);

            SettingsCommand aboutCommand = new SettingsCommand("AU", "Sobre", handler);
            args.Request.ApplicationCommands.Add(aboutCommand);

            SettingsCommand contactCommand = new SettingsCommand("CU", "Contato", handler);
            args.Request.ApplicationCommands.Add(contactCommand);

            SettingsCommand privacyCommand = new SettingsCommand("PP", "Política de privacidade", handler);
            args.Request.ApplicationCommands.Add(privacyCommand);

        }

        private void onSettingsCommand(IUICommand command)
        {
            Rect windowBounds = Window.Current.Bounds;
            settingsPopup = new Popup();

            settingsPopup.Closed += settingsPopup_Closed;
            Window.Current.Activated += Current_Activated;
            settingsPopup.IsLightDismissEnabled = true;
            Page settingPage = null;

            switch (command.Id.ToString())
            {
                case "AU":
                    settingPage = new AboutUs();
                    break;
                case "CU":
                    settingPage = new ContactUs();
                    break;
                case "PP":
                    settingPage = new PrivacyPolicy();
                    break;
            }

            settingsPopup.Width = settingsWidth;
            settingsPopup.Height = windowBounds.Height;

            // No Windows 8, movimento é tudo! Vamos incluir uma animação que será executado ao abrir cada um dos itens selecionados.
            //Instância uma nova coleção de transições
            settingsPopup.ChildTransitions = new TransitionCollection();

            //Adiciona uma nova transição, já setando alguns parâmetros do objeto no momento da criação.
            settingsPopup.ChildTransitions.Add(new PaneThemeTransition()
            {
                
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                EdgeTransitionLocation.Right :
                EdgeTransitionLocation.Left
            });

            if (settingPage != null)
            {
                //Configura a largura do flyout se settings, com o valor definido na propriedade pública da classe App.
                settingPage.Width = settingsWidth;
                //Utiliza a altura utilizada na resolução da máquina do usuário
                settingPage.Height = windowBounds.Height;
            }

            // Place the SettingsFlyout inside our Popup window.
            settingsPopup.Child = settingPage;

            // Vamos definir a localicação do nosso PopUp.
            settingsPopup.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (windowBounds.Width - settingsWidth) : 0);
            settingsPopup.SetValue(Canvas.TopProperty, 0);
            settingsPopup.IsOpen = true;
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


        /// <summary>
        /// Método executado quando a aplicação está sendo suspensa. O estado da aplicação é salvo 
        /// sem saber se a aplicação será terminada ou suspensa com seus dados de memória preservados.
        /// </summary>
        /// <param name="sender">Origem do pedido se suspender.</param>
        /// <param name="e">Detalhes sobre o pedido de suspender</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Método executado quando a aplicação é ativada para apresentar os resultados da pesquisa.
        /// Nova funcionalidade do Windows 8 
        /// </summary>
        /// <param name="args">Detalhes sobre o pedido de ativação</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {
            // TODO: Registrar o evento Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().QuerySubmitted
            // no método OnWindowCreated para acelerar as buscas, uma vez que a aplicação estiver em modo de execução

            // Se a janela não estiver utilizando a navegação por Frame, inserir o nosso frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            // Se o aplicativo não possúi um frame no nível superior, é possível que este seja uma execução inicial do aplicativo.
            // Tipicamente este método ou OnLaunched podem executar o mesmo método
            if (frame == null)
            {
                // Crie um Frame para agir como um contexto de navegação e associe ele a uma chave do SuspensionManager
                // a SuspensionManager key
                frame = new Frame();
                SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restaurar o estado da sessão salva, somente quando apropriado
                    try
                    {
                        await SuspensionManager.RestoreAsync();
                    }
                    catch (SuspensionManagerException)
                    {
                        //Se acontecer algum problema durante o processo de restauração, 
                        //assuma que não existe estado e continue.
                    }
                }
            }


            await Win8CatalogApplication.Instance.LoadAsync();

            // Realiza a busca.
            Win8CatalogApplication.ProcessQueryText(args.QueryText);

            
            frame.Navigate(typeof(ResultadoBusca), args.QueryText);
            Window.Current.Content = frame;

            // Tenha certeza que o janela corrente esta ativa
            Window.Current.Activate();
        }
    }
}
