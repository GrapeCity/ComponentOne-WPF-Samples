using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace FlexGridExplorer
{
    public class Song
    {
        [Display(Name = nameof(AppResources.NameLabel), ResourceType = typeof(AppResources))]
        public string Name { get; set; }

        [Display(Name = nameof(AppResources.AlbumLabel), ResourceType = typeof(AppResources))]
        public string Album { get; set; }

        [Display(Name = nameof(AppResources.ArtistLabel), ResourceType = typeof(AppResources))]
        public string Artist { get; set; }

        [Display(Name = nameof(AppResources.DurationLabel), ResourceType = typeof(AppResources))]
        public long Duration { get; set; }  // in milliseconds

        [Display(Name = nameof(AppResources.SizeLabel), ResourceType = typeof(AppResources))]
        public long Size { get; set; }      // in bytes

        [Display(Name = nameof(AppResources.RatingLabel), ResourceType = typeof(AppResources))]
        public int Rating { get; set; }     // from 0 to 5
    }

    public class MediaLibrary
    {
        public static List<Song> Load()
        {
            var asm = Assembly.GetExecutingAssembly();
            foreach (var resName in asm.GetManifestResourceNames())
            {
                if (resName.EndsWith("data.zip"))
                {
                    var zip = new ZipArchive(asm.GetManifestResourceStream(resName));
                    using (var stream = zip.Entries.First(e => e.Name == "songs.json").Open())
                    {
                        return JsonSerializer.Deserialize<List<Song>>(stream);
                    }
                }
            }
            throw new Exception("Can't find 'data.zip' embedded resource.");
        }
    }
}
