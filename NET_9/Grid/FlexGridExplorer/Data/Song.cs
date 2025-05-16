using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace FlexGridExplorer
{
    public class Song
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public long Duration { get; set; }  // in milliseconds
        public long Size { get; set; }      // in bytes
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
