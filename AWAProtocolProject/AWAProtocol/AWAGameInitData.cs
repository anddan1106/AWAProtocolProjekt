using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocol
{
    public class AWAGameInitData : AWAData
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public AWAGameInitData(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
