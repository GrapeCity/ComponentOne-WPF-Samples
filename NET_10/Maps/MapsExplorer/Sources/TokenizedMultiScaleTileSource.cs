using C1.WPF.Maps;
using MapsExplorer.Sources;
using System.Windows;

namespace MapsExplorer
{
    /// <summary>
    /// Base class for <see cref="MultiScaleTileSource"/> implementations where API key is needed.
    /// </summary>
    public abstract class TokenizedMultiScaleTileSource : MultiScaleTileSource
    {
        private bool? _isAuthorized;

        /// <summary>
        /// Creates a new instance of the <see cref="GoogleMapsTileSource"/> class.
        /// </summary>
        /// <param name="key">Your API key.</param>
        public TokenizedMultiScaleTileSource(string key = null) : base(0x8000000, 0x8000000, 256, 256, 0)
        {
            ApiKey = key;
            if (!string.IsNullOrEmpty(key))
            {
                IsAuthorized = true;
            }
        }

        /// <summary>
        /// The received value indicates whether the account has been successfully authorized.
        /// </summary>
        protected bool IsAuthorized 
        { 
            get
            {
                if (_isAuthorized is null)
                {
                    ApiKey = GetMapsAPIKey();
                    _isAuthorized = string.IsNullOrEmpty(ApiKey) ? false : true;
                }
                return _isAuthorized.Value;
            }
            private set => _isAuthorized = value;
        }

        /// <summary>
        /// Gets API key. 
        /// </summary>
        protected string ApiKey { get; private set; }

        /// <summary>
        /// Gets title for <see cref="RequestKeyDialog"/>.
        /// </summary>
        protected virtual string DialogTitle { get; } = "Requesting maps API key";

        private string GetMapsAPIKey()
        {
            var dialog = new RequestKeyDialog(DialogTitle)
            {
                Owner = Application.Current.MainWindow
            };
            if (dialog.ShowDialog() ?? false)
            {
                return dialog.ApiKey;
            }
            return null;
        }
    }
}