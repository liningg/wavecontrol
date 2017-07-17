using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveControl
{
    public class ControlProperty
    {
        /// <summary>
        /// 整个绘图
        /// </summary>
        public int interHeight = 10;
        public int timeHeight = 20;
        public int scrollHeight = 20;

        public Rectangle allRect;
        /// <summary>
        /// 顶部Rect
        /// </summary>
        public Rectangle topRect;
        /// <summary>
        /// 底部Rect
        /// </summary>
        public Rectangle botRect;
        public Rectangle timeRect;
        public Rectangle scrollRect;
        /// <summary>
        /// 胎心1相关
        /// </summary>
        public Color colorChildHeart1 = Color.FromArgb(0, 0, 0);
        public int lineWidthChildHeart1 = 1;
        /// <summary>
        /// 胎心2相关
        /// </summary>
        public Color colorChildHeart2 = Color.FromArgb(255,0,0);
        public int lineWidthChildHeart2 = 1;

        public Color colorFOCOExternal = Color.FromArgb(122, 185, 0);
        public int lineFOCOExternal = 1;
        public Color colorFetalMove = Color.FromArgb(64, 64, 255);
        public int lineFetalMove = 1;
      
        

    }
}
