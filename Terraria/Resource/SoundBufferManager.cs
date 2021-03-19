using SFML.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resource
{
    public class SoundBufferManager
    {
        private string Folder                               { get; set; }
        private string Extension                            { get; set; }
        private Dictionary<string, SoundBuffer> Resources   { get; set; }

        public SoundBufferManager(string folder, string extension)
        {
            Folder = "res/" + folder + "/";
            Extension = "." + extension;

            Resources = new Dictionary<string, SoundBuffer>();
        }

        public SoundBuffer Get(string name)
        {
            if (!Exist(name))
            {
                Add(name);
            }

            return Resources[name];
        }
        public bool Exist(string name)
        {
            return Resources.ContainsKey(name);
        }
        public void Add(string name)
        {
            if (!File.Exists(GetFullFilename(name)))
            {
                Resources.Add(name, new SoundBuffer(GetFullFilename("_fail_")));
            }
            else
            {
                Resources.Add(name, new SoundBuffer(GetFullFilename(name)));
            }
        }
        private string GetFullFilename(string name)
        {
            return Folder + name + Extension;
        }
    }
}
