using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resource
{
    public class FontManager
    {
        private string Folder                       { get; set; }
        private string Extension                    { get; set; }
        private Dictionary<string, Font> Resources  { get; set; }

        public FontManager(string folder, string extension)
        {
            Folder = "res/" + folder + "/";
            Extension = "." + extension;

            Resources = new Dictionary<string, Font>();
        }

        public Font Get(string name)
        {
            if(!Exist(name))
            {
                Add(name);
            }
            Resources.TryGetValue(name, out var font);

            return font;
        }
        public bool Exist(string name)
        {
            return Resources.ContainsKey(name);
        }
        public void Add(string name)
        {
            if(!File.Exists(GetFullFilename(name)))
            {
                Resources.Add(name, new Font(GetFullFilename("_fail_")));
            }
            else
            {
                Resources.Add(name, new Font(GetFullFilename(name)));
            }
        }
        private string GetFullFilename(string name)
        {
            return Folder + name + Extension;
        }
    }
}
