using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace SpaceWarr
{
    public class Collect
    {
        Texture2D diamondtexture = Raylib.LoadTexture("diamond.png");
        public Vector2 position = new Vector2();
        public float collectradius = 10;
        public bool isCollected = false;


        public void CreateCollect()
        {
            Circle collecthitbox = new Circle(position, collectradius);
            DrawDiamond();
        }
        public void FindPositionofCollect(Vector2 Positiona)
        {
            Console.WriteLine("Found");
            position = Positiona;
        }
        void DrawDiamond()
        {
            float scale = 0.2f;
            Raylib.DrawTexturePro(
             diamondtexture,
              new Rectangle(0, 0, diamondtexture.Width, diamondtexture.Height),
            new Rectangle(position.X, position.Y, diamondtexture.Width * scale,
            diamondtexture.Height * scale),
            new Vector2(diamondtexture.Width * scale / 2.0f, diamondtexture.Height * scale / 2.0f),
            0,
            Color.White

          );

        }
    }
}
