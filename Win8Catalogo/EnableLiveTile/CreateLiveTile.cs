using Win8Catalogo.EnableLiveTile.NotificationsExtensions.TileContent;
using Win8Catalogo.EnableLiveTile.TileContent;
using Win8Catalogo.Catalogo.Logic;
using Windows.UI.Notifications;

namespace Win8Catalogo.EnableLiveTile
{
    public class CreateLiveTile
    {
        private static string _baseUri = "ms-appx:///";
        public static void ShowliveTile(bool IsLiveTile1, string name)
        {

            string wideTilesrc = string.Format("{0}{1}", _baseUri, Win8CatalogApplication.Instance.Empresa.LiveTileBig1);
            string smallTilesrc = string.Format("{0}{1}", _baseUri, Win8CatalogApplication.Instance.Empresa.LiveTileSmall1); 

            if (!IsLiveTile1)
            {
                wideTilesrc = string.Format("{0}{1}", _baseUri, Win8CatalogApplication.Instance.Empresa.LiveTileBig2);
                smallTilesrc = string.Format("{0}{1}", _baseUri, Win8CatalogApplication.Instance.Empresa.LiveTileSmall2);
            }
            // Note: This sample contains an additional project, NotificationsExtensions.
            // NotificationsExtensions exposes an object model for creating notifications, but you can also 
            // modify the strings directly. See UpdateTileWithImageWithStringManipulation_Click for an example

            // Create notification content based on a visual template.
            ITileWideImageAndText01 tileContent = TileContentFactory.CreateTileWideImageAndText01();
            tileContent.TextCaptionWrap.Text = name;
            tileContent.Image.Src = wideTilesrc;
            tileContent.Image.Alt = "Live tile";

            // Users can resize tiles to square or wide.
            // Apps can choose to include only square assets (meaning the app's tile can never be wide), or
            // include both wide and square assets (the user can resize the tile to square or wide).
            // Apps should not include only wide assets.

            // Apps that support being wide should include square tile notifications since users
            // determine the size of the tile.

            // create the square template and attach it to the wide template
            ITileSquareImage squareContent = TileContentFactory.CreateTileSquareImage();
            squareContent.Image.Src = smallTilesrc;
            squareContent.Image.Alt = "Live tile";
            tileContent.SquareContent = squareContent;

            // Send the notification to the app's application tile.
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileContent.CreateNotification());
        }

    }
}
