using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Media.Imaging;
using System;

namespace VSEditorBackgroud
{
    /// <summary>
    /// Adornment class that draws a square box in the top right hand corner of the viewport
    /// </summary>
    public class EditorBackgroud
    {
        private Image _image;
        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;

        /// <summary>
        /// Creates a square image and attaches an event handler to the layout changed event that
        /// adds the the square in the upper right-hand corner of the TextView via the adornment layer
        /// </summary>
        /// <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
        public EditorBackgroud(IWpfTextView view)
        {
            _view = view;

            Config _Config = Config.CurrentConfig;

            //Grab a reference to the adornment layer that this adornment should be added to
            _adornmentLayer = view.GetAdornmentLayer("VSEditorBackgroud");

            _adornmentLayer.Opacity = _Config.LayerOpacity;
            ApplyImageConfig(_Config.BackgroundImage);

            _view.ViewportHeightChanged += delegate { this.onSizeChange(); };
            _view.ViewportWidthChanged += delegate { this.onSizeChange(); };
        }

        private void ApplyImageConfig(Config.ImageConfig config)
        {
            if (config != null)
            {
                try
                {
                    Image image = new Image();
                    string uri = config.Uri;
                    BitmapImage bgImage = new BitmapImage();
                    bgImage.BeginInit();
                    bgImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bgImage.CacheOption = BitmapCacheOption.OnLoad;
                    bgImage.UriSource = new Uri(uri);
                    bgImage.EndInit();
                    image.Stretch = Stretch.Fill;
                    image.Opacity = config.Opacity;
                    image.Source = bgImage;
                    this._image = image;
                }
                catch
                {
                }
            }
        }

        public void onSizeChange()
        {
            //clear the adornment layer of previous adornments
            _adornmentLayer.RemoveAllAdornments();

            if (this._image != null && !this._image.Width.Equals(this._view.ViewportWidth))
                this._image.Width = this._view.ViewportWidth;
            if (this._image != null && !this._image.Height.Equals(this._view.ViewportHeight))
                this._image.Height = this._view.ViewportHeight;

            //Place the image in the top right hand corner of the Viewport
            Canvas.SetLeft(_image, _view.ViewportLeft);
            Canvas.SetTop(_image, _view.ViewportTop);

            //add the image to the adornment layer and make it relative to the viewport
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _image, null);
        }
    }
}
