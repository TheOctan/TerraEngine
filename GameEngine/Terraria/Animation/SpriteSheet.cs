using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria
{
    class SpriteSheet
    {
        public SpriteSheet(int a, int b, int borderSize, int texW = 0, int texH = 0)
        {
            if (borderSize > 0)
            {
                this.borderSize = borderSize + 1;
            }
            else
            {
                this.borderSize = 0;
            }

            if (texW != 0 && texH != 0)
            {
                SubWidth = (int)Math.Ceiling((float)texW / a);
                SubHeigt = (int)Math.Ceiling((float)texH / b);
            }
            else
            {
                SubWidth = a;
                SubHeigt = b;
            }
        }

        public IntRect GetTextureRect(int i, int j)
        {
            int x = i * (SubWidth + borderSize);
            int y = j * (SubHeigt + borderSize);

            return new IntRect(x, y, SubWidth, SubHeigt);
        }

        public int SubWidth { get; private set; }
        public int SubHeigt { get; private set; }

        readonly int borderSize;


    }
}
