using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace james
{
    public class Item
    {
        public Color color;
        public int x, y, size;

        public Item(int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public Item(int _x, int _y, int _size, Color _color)
        {
            x = _x;
            y = _y;
            size = _size;
            color = _color;
        }

        public void Fall(int speed)
        {
            y += speed;
        }

        public void Move(string direction)
        {
           if (direction == "right")
            {
                x += 10;
            }
          
           if (direction == "left")
            {
                x -= 10;
            }
        }

    }
}
