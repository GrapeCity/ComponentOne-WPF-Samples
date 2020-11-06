using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MainTestApplication
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
                    var zip = new C1.C1Zip.C1ZipFile(asm.GetManifestResourceStream(resName));
                    using (var stream = zip.Entries["songs.xml"].OpenReader())
                    {
                        var xmls = new XmlSerializer(typeof(List<Song>));
                        return (List<Song>)xmls.Deserialize(stream);
                    }
                }
            }
            throw new Exception("Can't find 'data.zip' embedded resource.");
        }
    }
}
