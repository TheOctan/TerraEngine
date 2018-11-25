using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resource
{
    public class TextureManager
    {
        private string Folder { get; set; }
        private string Extension { get; set; }
        private Dictionary<string, Texture> Resources { get; set; }

        public TextureManager(string folder, string extension)
        {
            Folder = "res/" + folder + "/";
            Extension = "." + extension;

            Resources = new Dictionary<string, Texture>();
        }

        public Texture Get(string name)
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
                Resources.Add(name, new Texture(GetFullFilename("_fail_")));
            }
            else
            {
                Resources.Add(name, new Texture(GetFullFilename(name)));
            }
        }
        private string GetFullFilename(string name)
        {
            return Folder + name + Extension;
        }
    }
}
