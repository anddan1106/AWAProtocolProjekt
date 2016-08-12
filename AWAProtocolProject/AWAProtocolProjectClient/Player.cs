using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AWAProtocolProjectClient
{
    class Player
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Id { get; set; }
        public int Color { get; set; }
        public List<PictureBox> Images { get; set; }

        public Player(int id, int xPos, int yPos, int color = 2)
        {
            XPos = xPos;
            YPos = yPos;
            Id = id;
            Color = color;
            Images = new List<PictureBox>();
            for (int i = 0; i < 4; i++)
            {
                PictureBox p = new PictureBox();
                //string[] l = new string[] { "up", "right", "down", "left" };
                //p.Image = Image.FromFile($"Images/{color}/{l[i]}.png");
                //p.Visible = true; //ifall bilden inte syns :D
                p.Name = Id.ToString();
                p.Image = Image.FromFile($"Images\\{Color}\\{i}.png");
                p.Size = new System.Drawing.Size(32, 32);
                Images.Add(p);
            }

            //Läsa in bilderna till Images
        }
    }
}
