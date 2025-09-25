using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MapsExplorer
{
    // Get your ApiKey for testing

    /// <summary>
    /// A <see cref="C1.WPF.Maps.MultiScaleTileSource"/> accessing GoogleMaps tiles or static maps.
    /// </summary>
    public class GoogleMapsTileSource : TokenizedMultiScaleTileSource
    {
        private const string UriFormat = "https://maps.googleapis.com/maps/api/staticmap?center={0},{1}&zoom={2}&size={3}x{4}&key={5}";
        private const string UriFormatWithSession = "https://tile.googleapis.com/tile/v1/viewport?session={0}&key={1}&zoom={2}&north={3}&south={4}&east={5}&west={6}";
        private const int ZoomOffset = -8;

        private static string _sessionToken = null;

        static async void GetSessionTokenAsync(string key)
        {
            using var httpClient = new HttpClient();

            var tokenGenerator = new GoogleMapsSessionTokenGenerator(httpClient);
            _sessionToken = await tokenGenerator.GetSessionTokenAsync(key);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="GoogleMapsTileSource"/> class.
        /// </summary>
        public GoogleMapsTileSource() : this(null)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="GoogleMapsTileSource"/> class.
        /// </summary>
        /// <param name="key">Your API key.</param>
        public GoogleMapsTileSource(string key) : base(key)
        {
            GetSessionTokenAsync(ApiKey);
        }

        /// <inheritdoc/>
        protected override string DialogTitle { get; } = "Requesting GoogleMaps API key";

        /// <inheritdoc/>
        protected override void GetTileLayers(int tileLevel, int tilePositionX, int tilePositionY, IList<object> tileImageLayerSources)
        {
            if (!IsAuthorized)
                return;

            if (_sessionToken is null)
            {
                int zoom = tileLevel + ZoomOffset;
                (double latitude, double longitude) = GetTileCenter(tilePositionX, tilePositionY, zoom);
                var uri = new Uri(string.Format(CultureInfo.InvariantCulture, UriFormat, latitude, longitude, zoom, TileWidth, TileHeight, ApiKey));
                tileImageLayerSources.Add(uri);
            }
            else
            {
                int zoom = tileLevel + ZoomOffset;
                (LatLon nw, LatLon ne, LatLon sw, LatLon se) = GetTileCorners(tilePositionX, tilePositionY, zoom);
                var uri = new Uri(string.Format(CultureInfo.InvariantCulture, UriFormatWithSession, _sessionToken, ApiKey, zoom, nw.Latitude, se.Latitude, se.Longitude, nw.Longitude));
                tileImageLayerSources.Add(uri);
            }
        }

        /// <summary>
        /// Converts the tile coordinates to the geographical coordinates of the tile center
        /// </summary>
        private (double latitude, double longitude) GetTileCenter(int x, int y, int zoom)
        {
            /*
            // Getting the coordinates of the tile corners            
            var nw = TileToLatLon(x, y, zoom);         // Northwest corner (upper left)
            var se = TileToLatLon(x + 1, y + 1, zoom); // South-east corner (lower right)

            // not working with mercator
            double centerLat = (nw.Latitude + se.Latitude) / 2;
            double centerLon = (nw.Longitude + se.Longitude) / 2;
            */

            // Instead find the Northwest corner of the desired tile in next zoom
            var center = TileToLatLon(1 + x * 2, 1 + y * 2, zoom + 1);

            double centerLat = center.Latitude;
            double centerLon = center.Longitude;

            return (centerLat, centerLon);
        }

        /// <summary>
        /// Returns the coordinates of all four corners of the tile in the format: North-west, North-east, South-west, South-east.
        /// </summary>
        private (LatLon nw, LatLon ne, LatLon sw, LatLon se) GetTileCorners(int x, int y, int zoom)
        {
            // Northwest corner (upper left)
            var nw = TileToLatLon(x, y, zoom);

            // Northeast corner (upper right)
            var ne = TileToLatLon(x + 1, y, zoom);

            // Southwest corner (lower left)
            var sw = TileToLatLon(x, y + 1, zoom);

            // South-east corner (lower right)
            var se = TileToLatLon(x + 1, y + 1, zoom);

            return (nw, ne, sw, se);
        }

        /// <summary>
        /// Converts tile coordinates to geographical coordinates (latitude and longitude)
        /// </summary>
        private LatLon TileToLatLon(double x, double y, int zoom)
        {
            // good resource: 
            // https://docs.maptiler.com/google-maps-coordinates-tile-bounds-projection/

            // mercator calculations:
            // https://wiki.openstreetmap.org/wiki/Slippy_map_tilenames
            double n = Math.Pow(2.0, zoom);
            double lonDeg = x / n * 360.0 - 180.0;
            double latRad = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * y / n)));
            double latDeg = latRad * 180.0 / Math.PI;

            return new LatLon(latDeg, lonDeg);
        }
    }

    /// <summary>
    /// Represents geographical coordinates
    /// </summary>
    internal struct LatLon
    {
        /// <summary>
        /// Latitude.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Longitude.
        /// </summary>
        public double Longitude { get; }

        public LatLon(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Lat: {Latitude:F6}, Lon: {Longitude:F6}";
        }
    }

    internal class GoogleMapsSessionTokenGenerator
    {
        private const string TokenApiUrl = "https://tile.googleapis.com/v1/createSession";
        private readonly HttpClient _httpClient;

        public GoogleMapsSessionTokenGenerator(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GetSessionTokenAsync(string apiKey)
        {
            try
            {
                var requestBody = new
                {
                    version = "1.0",
                    request = new
                    {
                        mapType = "streetview",
                        language = "en-US",
                        region = "US"
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var requestUrl = $"{TokenApiUrl}?key={apiKey}";

                var response = await _httpClient.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseContent);
                var token = doc.RootElement
                    .GetProperty("session")
                    .GetProperty("value")
                    .GetString();

                return token;
            }
            catch (HttpRequestException ex)
            {
                //throw new Exception("Error receiving the session token", ex);
                Debug.WriteLine("Error receiving the session token for GoogleMaps: " + ex.Message);
            }
            catch (JsonException ex)
            {
                //throw new Exception("Error processing the JSON response", ex);
                Debug.WriteLine("Error receiving the session token for GoogleMaps: " + ex.Message);
            }
            return null;
        }
    }
}