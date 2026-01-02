using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab6
{
    internal class Note
    {
        public RectangleF Rect { get; set; }
        public string Name { get; set; }
        public bool IsSharp { get; set; }
        public Note()
        {

        }
        public bool IsClick(PointF point)
        {
            return Rect.Contains(point);
        }
        public RectangleF GetRect()
        {
            return Rect;
        }
    }
}
