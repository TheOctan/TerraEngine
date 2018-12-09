using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Resource
{
    public static class ResourceHolder
    {
        public readonly static FontManager          Fonts           = new FontManager("fonts", "ttf");
        public readonly static TextureManager       Textures        = new TextureManager("txrs", "png");
        public readonly static SoundBufferManager   SoundBuffers    = new SoundBufferManager("sfx", "ogg");
    }
}
