using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broken
{
    public struct Level
    {
        public int[,] FloorMap;

        public int Size;

        public void Init()
        {
            FloorMap = new int[Size,Size];
        }
    }
}
