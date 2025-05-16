using C1.WPF.Maps;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MapsExplorer
{
    /// <summary>
    /// A <see cref="C1.WPF.Maps.MultiScaleTileSource"/> accessing OpenStreet static maps.
    /// </summary>
    public class OpenStreetTileSource : MultiScaleTileSource
    {
        private const string UriFormat = "http://tile.openstreetmap.org/{0}/{1}/{2}.png";
        private const int ZoomOffset = -8;

        /// <summary>
        /// Creates a new instance of the <see cref="OpenStreetTileSource"/> class.
        /// </summary>
        public OpenStreetTileSource() : base(0x8000000, 0x8000000, 256, 256, 0)
        {
        }

        /// <inheritdoc/>
        protected override void GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY, IList<object> tileImageLayerSources)
        {
            var uri = new Uri(string.Format(CultureInfo.InvariantCulture, UriFormat, tileLevel + ZoomOffset, tilePositionX, tilePositionY));
            tileImageLayerSources.Add(uri);
        }
    }
}