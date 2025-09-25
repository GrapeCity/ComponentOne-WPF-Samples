using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Windows.Input;

namespace MapsExplorer
{
    /// <summary>
    /// A <see cref="C1.WPF.Maps.MultiScaleTileSource"/> accessing AzureMaps tiles.
    /// </summary>
    public class AzureMapsTileSource : TokenizedMultiScaleTileSource
    {
        private const string UriFormat = "https://atlas.microsoft.com/map/tile?api-version=2022-08-01&tilesetId=microsoft.base.road&zoom={0}&x={1}&y={2}&tileSize={3}&{4}";
        private const int ZoomOffset = -8;

        private AzureMapsConfiguration _config;

        /// <summary>
        /// Creates a new instance of the <see cref="AzureMapsTileSource"/> class.
        /// </summary>
        public AzureMapsTileSource() : this(null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AzureMapsTileSource"/> class.
        /// </summary>
        /// <param name="key">Your API key.</param>
        public AzureMapsTileSource(string key) : base(key)
        {
        }

        protected override string DialogTitle => "Requesting Azure Maps auth config";

        protected override void GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY, IList<object> tileImageLayerSources)
        {
            if (!IsAuthorized)
                return;

             _config = new AzureMapsConfiguration
            {
                AuthType = "subscriptionKey",
                SubscriptionKey = ApiKey
            };

            var tokenPart = GetAuthQueryParam(_config);
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, UriFormat, tileLevel + ZoomOffset, tilePositionX, tilePositionY, TileWidth, tokenPart));
            tileImageLayerSources.Add(uri);
        }

        private static string GetAuthQueryParam(AzureMapsConfiguration config)
        {
            return config.AuthType switch
            {
                "subscriptionKey" => $"subscription-key={config.SubscriptionKey}",
                "sas" => $"sasToken={Uri.EscapeDataString(config.SasToken)}",               
                _ => throw new NotSupportedException($"Auth type '{config.AuthType}' is not supported in this tile source."),
            };
        }


    }
    /// <summary>
    /// <see cref="AzureMapsConfiguration"/> is a model class used to define the 
    /// authentication and configuration settings for connecting to Azure Maps services.
    /// </summary>
    public class AzureMapsConfiguration
    {
        public string AuthType { get; set; } = "subscriptionKey";
        public string SubscriptionKey { get; set; }
        public string SasToken { get; set; }
        public string AadAppId { get; set; }
        public string AadTenant { get; set; }
        public string AadInstance { get; set; }
        public string ClientId { get; set; }
        public string Domain { get; set; }
        public bool? DisableTelemetry { get; set; }
        public bool? EnableAccessibility { get; set; }
        public bool? EnableAccessibilityLocationFallback { get; set; }
        public string SessionId { get; set; }
    }

}