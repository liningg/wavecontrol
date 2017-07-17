using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WaveControl
{
    public partial class UserControl1 : UserControl
    {

        private ControlDataCenter ctrlData;
        private ControlProperty ctrlProperty;
        int beginIndex = 0;
        int endIndex = 0;
        private List<ControlDataEntity> dataList;
        private List<Point> dataPointChildHeart1;
        private List<Point> dataPointChildHeart2;

        private List<Point> dataPointPalaceCompression;
        private List<Point> dataPointFetalMovement;

        int paddingLeft = 10;

        bool isOnScroll = false;
        int pix = 2;
        public bool isDraw = true;
        private bool isShowFetal1 = true;
        private bool isShowFetal2 = true;
        private int normalFetalUp = 160;
        private int normalFetalLow = 110;

        private List<FetalAlertStruct> fetalAlertList = new List<FetalAlertStruct>();

        public delegate void DoubleClickEventHander(string strNum);
        public event DoubleClickEventHander OnDoubleClick = null;
        public delegate void ClickEventHander(string strData);
        public event ClickEventHander OnClick = null;

        private string strDeviceNumber = string.Empty;
        private int textSize = 10;
        private string strConfigPath = string.Empty;
        private string strRootDir = string.Empty;
        private Point pMark;
        private Point pScreen;
        public UserControl1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            strRootDir = System.AppDomain.CurrentDomain.BaseDirectory;

        }
        public void StartDraw()
        {
            isDraw = false;
            Thread.Sleep(1000);
            ctrlData.ClearData();
            dataList.Clear();
            beginIndex = 0;
            endIndex = 0;
            isDraw = true;
            Invalidate();
        }
        public void SetSampleInternal(int pix)
        {
            this.pix = pix;
        }
        public void SetConfigPath(string str)
        {
            strConfigPath = str;
        }
        
        /// <summary>
        /// 控件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl1_Load(object sender, EventArgs e)
        {
            ctrlData = new ControlDataCenter();
            ctrlProperty = new ControlProperty();
            dataPointChildHeart1 = new List<Point>();
            dataPointChildHeart2 = new List<Point>();
            dataPointPalaceCompression = new List<Point>();
            dataPointFetalMovement = new List<Point>();
            dataList = new List<ControlDataEntity>();
        }

        /// <summary>
        /// 控件绘图
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
           
            if (isDraw)
            {
                // 绘制顶部区域
                DrawBackground(e, "top");
                DrawWave(e, "top");

                // 绘制底部区域
                DrawBackground(e, "bottom");
                DrawWave(e, "bottom");

                //绘制日期
                //DrawDateTime(e);

                int count = ctrlData.GetSize();

                // 滚动条设置
                if (!isOnScroll)
                {
                    int maxinum = (count * pix - Width) > 0 ? (count * pix - Width) : 0;
                    hScrollBar1.Maximum = maxinum;
                    hScrollBar1.Value = maxinum;
                }
            }
           
        }
        private void DrawFetalCoincide(PaintEventArgs e,PointF p)
        {
            //throw new NotImplementedException();
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            // Create image.
            Image newImage = Image.FromFile(strRootDir + "Image//FetalCoincide.png");
            g.DrawImage(newImage, p);
        }
        private void DrawMark(PaintEventArgs e, PointF p)
        {
            //throw new NotImplementedException();
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Image newImage = Image.FromFile(strRootDir + "Image//Mark.png");
            p.Y = p.Y - newImage.Height - 4;
            g.DrawImage(newImage, p);
        }
        private void DrawPcMark(PaintEventArgs e, PointF p,string str)
        {
            //throw new NotImplementedException();
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Image newImage = Image.FromFile(strRootDir + "Image//tip.png");
            p.Y = p.Y - newImage.Height - 4;
            g.DrawImage(newImage, p);
            Font font = new Font("微软雅黑", 10);
            g.DrawString(str, font, Brushes.Gray, p.X + newImage.Width, p.Y);
        }
        private void DrawManualFetalMove(PaintEventArgs e, PointF p)
        {
            //throw new NotImplementedException();
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Image newImage = Image.FromFile(strRootDir + "Image//ManualFetalMove.png");
            p.Y = p.Y - newImage.Height - 4;
            g.DrawImage(newImage, p);
        }
        private void DrawFetalAlert(PaintEventArgs e, PointF p)
        {
            //throw new NotImplementedException();
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            // Create image.
            Image newImage = Image.FromFile(strRootDir + "Image//FetalAlert.png");
            g.DrawImage(newImage, p);
        }

        private void DrawDateTime(PaintEventArgs e)
        {
            //throw new NotImplementedException();
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);
            Font font = new Font("Arial", 12);
            if (dataList.Count>0)
            {
                string[] time = dataList[0].entity.SampleTime.Split(' ');
                //string time = dataList[i].sampleTime.Substring(,)
                g.DrawString(time[0], font, Brushes.Gray,ctrlProperty.timeRect.Left,ctrlProperty.timeRect.Top);

            }

        }

        /// <summary>
        /// 设置控件大小
        /// </summary>
        /// <param name="s"></param>
        public void SetControlSize(Size s)
        {
            Size size = s;
            this.Size = size;
            ctrlProperty.interHeight = 10;
            int otherHight = ctrlProperty.interHeight + ctrlProperty.timeHeight + ctrlProperty.scrollHeight;
            size.Height = size.Height - otherHight ;
            ctrlProperty.topRect = new Rectangle { X = 0, Y = 10, Width = size.Width, Height = Convert.ToInt32(size.Height * 15/27) };
            ctrlProperty.interHeight = Convert.ToInt32(size.Height * 2 / 27);
            ctrlProperty.botRect = new Rectangle { X = 0, Y = ctrlProperty.topRect.Bottom + ctrlProperty.interHeight, Width = size.Width, Height = Convert.ToInt32(size.Height * 10/27) };
            ctrlProperty.timeRect = new Rectangle { X = 0, Y = ctrlProperty.botRect.Bottom, Width = ctrlProperty.botRect.Width,Height = ctrlProperty.timeHeight};
            ctrlProperty.scrollRect = new Rectangle { X = 0, Y = ctrlProperty.timeRect.Bottom, Width = ctrlProperty.timeRect.Width, Height = ctrlProperty.scrollHeight };
            // 横向滚动条
            hScrollBar1.Location = new Point(0, ctrlProperty.scrollRect.Top);
            hScrollBar1.Height = ctrlProperty.scrollHeight;
            hScrollBar1.Width = size.Width;
            hScrollBar1.Maximum = 0;

            int count = ctrlData.GetSize();
            if (!isOnScroll)
            {
                int maxinum = (count * pix - Width) > 0 ? (count * pix - Width) : 0;
                hScrollBar1.Maximum = maxinum;
                hScrollBar1.Value = maxinum;
            }
            CalcTextSize(size.Height);
        }

        /// <summary>
        /// 计算字体大小
        /// </summary>
        private void CalcTextSize(int height)
        {
            if(height>=400)
            {
                textSize = 12;
            }
            else 
            {
                textSize = 8;
            }

        }
        
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        public void SetChildHeart1Property(Color color, int width)
        {
            ctrlProperty.colorChildHeart1 = color;
            ctrlProperty.lineWidthChildHeart1 = width;
        }
        public void SetChildHeart2Property(Color color, int width)
        {
            ctrlProperty.colorChildHeart2 = color;
            ctrlProperty.lineWidthChildHeart2 = width;
        }
        public void SetFOCOExternal(Color color,int width)
        {
            ctrlProperty.colorFOCOExternal = color;
            ctrlProperty.lineFOCOExternal = width;
        }
        public void SetFetalMove(Color color,int width)
        {
            ctrlProperty.colorFetalMove = color;
            ctrlProperty.lineFetalMove = width;
        }
        public void SetPointPix(int pix)
        {
            this.pix = pix;
        }
        public void SetShowScroll(bool mark)
        {
            hScrollBar1.Visible = mark;
        }
        public void SetDeviceNumber(string num)
        {
            strDeviceNumber = num;
        }
        public void SetShowIndex(int bIndex,int eIndex)
        {
            beginIndex = bIndex;
            endIndex = eIndex;
            //首先从缓冲区拿到对应的数据
            dataList = ctrlData.GetDataSequnce(beginIndex, endIndex);
            //转换数据到点
            ConvertDataToPoint();
            //通知客户端进行绘图
            Invalidate();
        }
        public void SetShowFetal1(bool mark)
        {
            isShowFetal1 = mark;
            fetalAlertList.Clear();
        }
        public void SetShowFetal2(bool mark)
        {
            isShowFetal2 = mark;
            fetalAlertList.Clear();
        }
        public void SetNormalFetalRange(int up,int low)
        {
            normalFetalUp = up;
            normalFetalLow = low;
        }

        /// <summary>
        /// 实时更新数据
        /// </summary>
        /// <param name="data"></param>
        public void AddSampleData(string data)
        {
            //当有采样点的时候添加采样点到缓冲区
            ControlDataEntity entity = ControlDataEntity.ConvertEntity(data);
            if(entity == null)
            {
                return;
            }
            Monitor.Enter(this);//锁定，保持同步
            ctrlData.AddData(entity);
            if (!isOnScroll)
            {
                //确定begin和end索引点
                CalcDrawIndex();
                //首先从缓冲区拿到对应的数据
                dataList = ctrlData.GetDataSequnce(beginIndex, endIndex);
                //转换数据到点
                ConvertDataToPoint();
                //通知客户端进行绘图
                Invalidate();
                //Refresh();
            }
            Monitor.Exit(this);//锁定，保持同步
        }
        public void SetSampleData(int index,string data)
        {
           
            //当有采样点的时候添加采样点到缓冲区
            ControlDataEntity entity = ControlDataEntity.ConvertEntity(data);
            if (entity == null)
            {
                return;
            }
            Monitor.Enter(this);//锁定，保持同步
            ctrlData.SetData(index, entity);
            Monitor.Exit(this);//锁定，保持同步
        }
        /// <summary>
        /// 计算数据索引值
        /// </summary>
        private void CalcDrawIndex()
        {
            int cnt = (this.Size.Width) / pix;

            if (cnt <= ctrlData.GetSize())
            {
                endIndex = ctrlData.GetSize();
                beginIndex = ctrlData.GetSize() - cnt;
            }
            else
            {
                beginIndex = 0;
                endIndex = ctrlData.GetSize() <= 2 ? ctrlData.GetSize() : ctrlData.GetSize() - 1;
            }

        }

        /// <summary>
        /// 画横线
        /// </summary>
        /// <param name="e"></param>
        /// <param name="tag"></param>
        private void DrawBackHLine(PaintEventArgs e, string tag)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p1 = new Pen(Color.FromArgb(169, 169, 169), 2);
            Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);

            Rectangle rect;
            int col;
            if (tag == "top")
            {
                rect = ctrlProperty.topRect;
                col = 15;
            }
            else
            {
                rect = ctrlProperty.botRect;
                col = 10;
            }

            int hig = ctrlProperty.topRect.Height / 15;

            int i = 0;
            for (i = 0; i < col;)
            {
                g.DrawLine(p1, new Point(rect.Left + paddingLeft, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));
                i++;
                g.DrawLine(p2, new Point(rect.Left + paddingLeft, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));
                i++;
                if (tag == "top")
                {
                    g.DrawLine(p2, new Point(rect.Left + paddingLeft, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));
                    i++;
                }

            }
            g.DrawLine(p1, new Point(rect.Left + paddingLeft, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));

        }
        /*private void DrawBackVLine(PaintEventArgs e, string tag)
        {
            if (dataList.Count < 2)
            {
                return;
            }
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p1 = new Pen(Color.FromArgb(169, 169, 169), 2);
            Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);

            Rectangle rect;
            int[] value;
            int hig = ctrlProperty.topRect.Height / 15;
            if (tag == "top")
            {
                rect = ctrlProperty.topRect;
                value = ControlDataEntity.tValue;
                hig *= 3;
            }
            else
            {
                rect = ctrlProperty.botRect;
                value = ControlDataEntity.bValue;
                hig *= 2;
            }


            // 刻度文字居中
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;
            // 刻度文字
            Font font1 = new Font("Arial", 8, FontStyle.Bold);

            // 时间文字
            Font font = new Font("Arial", 9);

            //画竖线
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].isDrawVLine)
                {
                    g.DrawLine(p2, new Point(dataPointChildHeart1[i].X + paddingLeft, rect.Top), new Point(dataPointChildHeart1[i].X + paddingLeft, rect.Bottom));
                }

                if (dataList[i].isDrawTime)
                {
                    g.DrawLine(p1, new Point(dataPointChildHeart1[i].X + paddingLeft, rect.Top), new Point(dataPointChildHeart1[i].X + paddingLeft, rect.Bottom));

                    if (tag == "bottom")
                    {
                        g.DrawString(dataList[i].sampleTime, font, Brushes.Gray, dataPointChildHeart1[i].X + paddingLeft, rect.Bottom + 10);
                    }

                }

                if (dataList[i].isDrawNum)
                {
                    for (int j = 0; j < ControlDataEntity.tValue.Length; j++)
                    {
                        //g.FillRectangle(Brushes.White, dataPointChildHeart1[i].X, rect.Top + hig * j - paddingLeft/2, 20, 10);
                        g.DrawString(value[j].ToString(), font1, Brushes.Gray, dataPointChildHeart1[i].X + paddingLeft, rect.Top + hig * j, stringFormat);
                    }
                }
            }
        }*/
        /// <summary>
        /// 画竖线
        /// </summary>
        /// <param name="e"></param>
        /// <param name="tag"></param>
        private void DrawBackVLine(PaintEventArgs e, string tag)
        {
            List<ControlDataEntity> dataTemp = new List<ControlDataEntity>();
            
            if (dataList.Count >= 0 &&  dataList.Count< ctrlProperty.topRect.Width / pix)
            {
                ControlDataEntity[] dataArray = new ControlDataEntity[dataList.Count];
                dataList.CopyTo(dataArray, 0);
                dataTemp = dataArray.ToList<ControlDataEntity>();
                //dataList.CopyTo(dataTemp.ToArray(), 0);
                DateTime timei = DateTime.Now;
                if (dataList.Count > 0)
                {
                    timei = Convert.ToDateTime(dataList[dataList.Count - 1].entity.SampleTime);
                }
                
                for (int i = dataList.Count; i < ctrlProperty.topRect.Width / pix; i++)
                {
                    ControlDataEntity entity = new ControlDataEntity();
                    if(i != 0)
                    {
                        timei = timei.AddSeconds(1);
                    }
                    
                    entity.entity.SampleTime = timei.ToString();

                    DateTime time0 = DateTime.Now;
                    if (ctrlData.datalist.Count>0)
                    {
                        time0 = Convert.ToDateTime(ctrlData.datalist[0].entity.SampleTime);
                    }

                    Int64 seci = timei.Ticks;
                    Int64 sec0 = time0.Ticks;

                    TimeSpan ts1 = new TimeSpan(seci); //获取当前时间的刻度数
                                                       //执行某操作
                    TimeSpan ts2 = new TimeSpan(sec0);

                    TimeSpan ts = ts2.Subtract(ts1).Duration(); //时间差的绝对值
                    int sec = Convert.ToInt32(ts.TotalSeconds); //执行时间的总秒数
                    if (sec % 60 == 0)
                    {
                        entity.isDrawTime = true;
                        // 四分钟显示刻度
                        if (sec % 240 == 0)
                        {
                            entity.isDrawNum = true;
                        }
                        entity.isDrawVLine = false;

                    }
                    else if (sec % 15 == 0)
                    {
                        entity.isDrawTime = false;
                        entity.isDrawVLine = true;
                    }
                    dataTemp.Add(entity);

                }
            }
            else
            {
                dataTemp = dataList;
            }
            /*if(dataList.Count<2)
            {
                return;
            }*/
            //如果采集点未满一屏幕，那么给datalist 充满值
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p1 = new Pen(Color.FromArgb(169, 169, 169), 2);
            Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);

            Rectangle rect;
            int[] value;
            int hig = ctrlProperty.topRect.Height / 15;
            if (tag == "top")
            {
                rect = ctrlProperty.topRect;
                value = ControlDataEntity.tValue;
                hig *= 3;
            }
            else
            {
                rect = ctrlProperty.botRect;
                value = ControlDataEntity.bValue;
                hig *= 2;
            }


            // 刻度文字居中
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;
            stringFormat.Alignment = StringAlignment.Center;
            StringFormat stringFormat1 = new StringFormat();
            stringFormat1.LineAlignment = StringAlignment.Near;
            stringFormat1.Alignment = StringAlignment.Near;
            // 刻度文字
            Font font1 = new Font("Arial", textSize,FontStyle.Bold);

            // 时间文字
            Font font = new Font("Arial", textSize, FontStyle.Bold);

            //画竖线
            int mark = 0;
            for (int i = 0; i < dataTemp.Count; i++)
            {
                int hig1 = ctrlProperty.topRect.Height / 15;
                int top = 0;
                int bottom = 0;
                if (tag == "top")
                {
                    top = rect.Top;
                    bottom = top + 15 * hig1;
                }
                else
                {
                    top = rect.Top;
                    bottom = top + 10 * hig1;
                }
                if (dataTemp[i].isDrawVLine)
                {
                    g.DrawLine(p2, new Point(i* pix + paddingLeft,top), new Point(i* pix + paddingLeft, bottom));
                }

                if (dataTemp[i].isDrawTime)
                {
                    g.DrawLine(p1, new Point(i* pix + paddingLeft,top), new Point(i* pix + paddingLeft, bottom));
                    
                    if (tag == "bottom")
                    {
                        string strTime = dataTemp[i].entity.SampleTime;
                        if (mark != 0)
                        {
                            string[] time = dataTemp[i].entity.SampleTime.Split(' ');
                            strTime = time[1];
                        }
                        mark++;
                        //string time = dataList[i].sampleTime.Substring(,)
                        g.DrawString(strTime, font, Brushes.Gray, new RectangleF { X = i * pix + paddingLeft, Y = ctrlProperty.timeRect.Top, Width = 200, Height = ctrlProperty.timeRect.Height }, stringFormat1);
                    }
                }
                if (dataTemp[i].isDrawNum)
                {
                    for (int j = 0; j < ControlDataEntity.tValue.Length; j++)
                    {
                        //g.FillRectangle(Brushes.White, dataPointChildHeart1[i].X, rect.Top + hig * j - paddingLeft/2, 20, 10);
                        g.DrawString(value[j].ToString(), font1, Brushes.Gray, i*pix+ paddingLeft, rect.Top + hig * j, stringFormat);
                    }
                }
                if(tag == "top")
                {
                    if(isShowFetal1)
                    {
                        if(dataTemp[i].entity.bFetal1Alert)
                        {
                            if(i>0)
                            {
                                if(!dataTemp[ i-1].entity.bFetal1Alert)
                                {
                                    DrawFetalAlert(e, new PointF() { X = i * pix + paddingLeft, Y = top });
                                    fetalAlertList.Add(new FetalAlertStruct() { x = i * pix + paddingLeft, y = top, width = 20, heigth = 20, content = dataTemp[i].entity.strFetal1AlertContent });
                                }

                            }
                        }
                    }
                    if(isShowFetal2)
                    {
                        if (dataTemp[i].entity.bFetal2Alert)
                        {
                            if (i > 0)
                            {
                                if (!dataTemp[i - 1].entity.bFetal2Alert)
                                {
                                    DrawFetalAlert(e, new PointF() { X = i * pix + paddingLeft, Y = top });
                                    fetalAlertList.Add(new FetalAlertStruct() { x = i * pix + paddingLeft, y = top, width = 20, heigth = 20, content = dataTemp[i].entity.strFetal2AlertContent });
                                }
                            }
                        }
                    }
                    //画手动胎动
                    if(dataTemp[i].entity.ManualFetalMove == 1)
                    {
                        int h = ctrlProperty.topRect.Top + (210 - normalFetalLow) * (hig * 15) / (210 - 60);
                        DrawManualFetalMove(e, new PointF() { X = i * pix + paddingLeft, Y = h });
                    }
                    if(dataTemp[i].entity.FHRSame == 1)
                    {
                        DrawFetalCoincide(e, new PointF() { X = i * pix + paddingLeft, Y = top + 20 });
                    }
                    
                }
                if(tag == "bottom")
                {
                    if(dataTemp[i].entity.Mark == 1)
                    {
                        DrawMark(e, new PointF() { X = i * pix + paddingLeft, Y = top });
                    }
                }
                if (dataTemp[i].entity.bPcMark)
                {
                    DrawPcMark(e, new PointF() { X = i * pix + paddingLeft, Y = (float)(Height * dataTemp[i].entity.yPcMark) }, dataTemp[i].entity.PcMark);
                }
            }
            return;
        }
        
        /// <summary>
        /// 绘制区域背景
        /// </summary>
        /// <param name="e"></param>
        private void DrawBackground(PaintEventArgs e, string rect)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            // 填充背景
            Rectangle rectangle = new Rectangle();
            if (rect == "top")
            {
                rectangle = ctrlProperty.topRect;
            } else if (rect == "bottom")
            {
                rectangle = ctrlProperty.botRect;
            }
            //绘制胎心正常区域
            DrawBackNormalRect(e, rect);
            DrawBackHLine(e, rect);
            DrawBackVLine(e, rect);
        }

        /// <summary>
        /// 绘制正常胎心绿色区域
        /// </summary>
        /// <param name="e"></param>
        /// <param name="rect"></param>
        private void DrawBackNormalRect(PaintEventArgs e, string rect)
        {
            //throw new NotImplementedException();
            int hig = ctrlProperty.topRect.Height / 15;
            if (rect == "top")
            {
                int i = (210 - normalFetalUp) / 10;
                int normalTop = ctrlProperty.topRect.Top + hig * i;
                i = (210 - normalFetalLow) / 10;
                int normalBot = ctrlProperty.topRect.Top + hig * i;
                Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
                Pen p1 = new Pen(Color.FromArgb(169, 169, 169), 2);
                Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);
                Rectangle area = new Rectangle { X = ctrlProperty.topRect.Left +10, Y = normalTop, Width = ctrlProperty.topRect.Width, Height = normalBot - normalTop };
                Brush bs = new SolidBrush(Color.FromArgb(192, 255, 192));
                g.FillRectangle(bs, area);
            }
        }

        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="e"></param>
        private void DrawWave(PaintEventArgs e, string rect)
        {
            if (dataList.Count > 2)
            {
                Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
                Pen pen1 = null;
                Pen pen2 = null;
                List<Point> pointList1 = new List<Point>();
                List<Point> pointList2 = new List<Point>();

                if (rect == "top")
                {
                    pen1 = new Pen(ctrlProperty.colorChildHeart1, ctrlProperty.lineWidthChildHeart1);
                    pen2 = new Pen(ctrlProperty.colorChildHeart2, ctrlProperty.lineWidthChildHeart2);
                    pointList1 = dataPointChildHeart1;
                    pointList2 = dataPointChildHeart2;
                  
                }
                else
                {
                    pen1 = new Pen(ctrlProperty.colorFOCOExternal, ctrlProperty.lineFOCOExternal);
                    pen2 = new Pen(ctrlProperty.colorFetalMove, ctrlProperty.lineFetalMove);
                    pointList1 = dataPointPalaceCompression;
                    pointList2 = dataPointFetalMovement;
                }

                //绘图的时候可能需要抽取采样点，因为有重复，暂时不做考虑
                if (rect == "top")
                {
                    if (isShowFetal1)
                    {
                        Point[] p = paddingPoint(pointList1, "top").ToArray();
                        if(p.Length > 3)
                        {
                            g.DrawLines(pen1,p );
                        }
                        

                    }
                    if (isShowFetal2)
                    {
                        Point[] p = paddingPoint(pointList2, "top").ToArray();
                        if (p.Length > 3)
                        {
                            g.DrawLines(pen2, p);
                        }
                    }
                }
                else
                {
                    Point[] p = paddingPoint(pointList1, "bottom").ToArray();
                    if (p.Length > 3)
                    {
                        g.DrawLines(pen1, p);
                    }
                    p = paddingPoint(pointList2, "bottom").ToArray();
                    if (p.Length > 3)
                    {
                        g.DrawLines(pen2, p);
                    }
                }

                
                
                
               

            }
        }

        /// <summary>
        /// 给数据点加载padding
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        private List<Point> paddingPoint(List<Point> dataList,string rect)
        {
            int up = 0;
            int low = 0;
            int hig = ctrlProperty.topRect.Height / 15;
            if (rect == "top")
            {
                up = ctrlProperty.topRect.Top + (210 - 210) * (hig * 15) / (210 - 60);
                low = ctrlProperty.topRect.Top + (210 - 60) * (hig * 15) / (210 - 60);
            }
            else
            {
                up = ctrlProperty.botRect.Top + (100 - 100) * (hig * 15) / (100 - 0);
                low = ctrlProperty.botRect.Top + (100 - 0) * (hig * 15) / (100 - 0);
            }
            var tempList = new List<Point>();
            foreach(var item in dataList)
            {
                if(item.Y <=low && item.Y >= up)
                {
                    tempList.Add(new Point() { X = item.X + paddingLeft, Y = item.Y });
                }
                
            }
            return tempList;
        }
        
        /// <summary>
        /// 将数据转成坐标点
        /// </summary>
        public void ConvertDataToPoint()
        {
            if (dataList.Count < 2)
            {
                return;
            }
            //第一步：根据索引点获取需要绘制的数据
            //List<int> dataInt = ctrlData.GetChildHeart1Data(beginIndex, endIndex);
            //第三部：将采样数据转换成坐标点
            dataPointChildHeart1.Clear();
            dataPointChildHeart2.Clear();
            dataPointPalaceCompression.Clear();
            dataPointFetalMovement.Clear();

            int ipos = 0;
            int hig = ctrlProperty.topRect.Height / 15;
            for (int i = 0; i < dataList.Count; i++)
            {
                ControlDataEntity entity = dataList[i];
                //预处理采集点
                //PreHandleSampleData(ref entity);
                Point p = new Point();
                p.X = i*pix + ctrlProperty.topRect.Left;
                p.Y = ctrlProperty.topRect.Top + (210 - entity.entity.FetalHeart1) * (hig * 15) / (210 - 60);
                if(entity.entity.HR1_SensorDrop == 0)
                {
                    dataPointChildHeart1.Add(p);
                }
                
                p.Y = ctrlProperty.topRect.Top + (210 - entity.entity.FetalHeart2) * (hig * 15) / (210 - 60);
                if(entity.entity.HR2_SensorDrop == 0)
                {
                    dataPointChildHeart2.Add(p);
                }
                
                p.Y = ctrlProperty.botRect.Top + (100 - entity.entity.TOCOValue) * (hig * 10) / (100 - 0);
                if(entity.entity.TOCOSensorDrop == 0)
                {
                    dataPointPalaceCompression.Add(p);
                } 
                p.Y = ctrlProperty.botRect.Top + (100 - entity.entity.FetalMove) * (hig * 10) / (100 - 0);
                if(entity.entity.FetalMove != -1000)
                {
                    dataPointFetalMovement.Add(p);
                }
                else
                {
                    dataPointFetalMovement.Clear();
                }
                
                //ipos = ipos + pix;
            }
            for (int i = 0; i < (endIndex - beginIndex); i++)
            {
                DateTime timei = Convert.ToDateTime(dataList[i].entity.SampleTime);
                DateTime time0 = Convert.ToDateTime(ctrlData.datalist[0].entity.SampleTime);

                Int64 seci = timei.Ticks;
                Int64 sec0 = time0.Ticks;

                TimeSpan ts1 = new TimeSpan(seci); //获取当前时间的刻度数
                                                   //执行某操作
                TimeSpan ts2 = new TimeSpan(sec0);

                TimeSpan ts = ts2.Subtract(ts1).Duration(); //时间差的绝对值

                int sec = Convert.ToInt32(ts.TotalSeconds); //执行时间的总秒数
                if (sec % 60 == 0)
                {
                    dataList[i].isDrawTime = true;
                    // 四分钟显示刻度
                    if (sec % 240 == 0)
                    {
                        dataList[i].isDrawNum = true;
                    }
                    dataList[i].isDrawVLine = false;

                }
                else if (sec % 15 == 0)
                {
                    dataList[i].isDrawTime = false;
                    dataList[i].isDrawVLine = true;
                }

            }
            return;
        }

        /// <summary>
        /// 对数据进行预处理，因为有些数据超出了坐标
        /// </summary>
        /// <param name="controlDataEntity"></param>
        private void PreHandleSampleData(ref ControlDataEntity controlDataEntity)
        {
            //throw new NotImplementedException();
            if(controlDataEntity.entity.FetalHeart1 > 210)
            {
                controlDataEntity.entity.FetalHeart2 = 210;
            }
            if(controlDataEntity.entity.FetalHeart1 < 60)
            {
                controlDataEntity.entity.FetalHeart1 = 60;
            }
            if (controlDataEntity.entity.FetalHeart2 > 210)
            {
                controlDataEntity.entity.FetalHeart2 = 210;
            }
            if (controlDataEntity.entity.FetalHeart2 < 60)
            {
                controlDataEntity.entity.FetalHeart2 = 60;
            }
            if (controlDataEntity.entity.FetalMove > 100)
            {
                controlDataEntity.entity.FetalMove = 100;
            }
            if (controlDataEntity.entity.FetalMove < 0)
            {
                controlDataEntity.entity.FetalMove = 0;
            }
            if (controlDataEntity.entity.TOCOValue > 100)
            {
                controlDataEntity.entity.TOCOValue = 100;
            }
            if (controlDataEntity.entity.TOCOValue < 0)
            {
                controlDataEntity.entity.TOCOValue = 0;
            }
        }

        /// <summary>
        /// 滚动条事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.NewValue > 0)
            {
                isOnScroll = true;
                if ((hScrollBar1.Maximum - hScrollBar1.LargeChange + 1) == hScrollBar1.Value)
                {
                    isOnScroll = false;
                } else
                {
                    endIndex = (Width + e.NewValue) / pix;
                    beginIndex = endIndex - (Width / pix);

                    //首先从缓冲区拿到对应的数据
                    dataList = ctrlData.GetDataSequnce(beginIndex, endIndex);
                    //转换数据到点
                    ConvertDataToPoint();
                    //通知客户端进行绘图
                    Invalidate();
                }
            }
        }

        private void UserControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Point p = e.Location;
            foreach(var item in fetalAlertList)
            {
                if(p.X > item.x - 10 && p.X < item.x + 30 
                    && p.Y > item.y - 10 && p.Y < item.y + 30)
                {
                    MessageBox.Show(item.content);
                    return ;
                }
            }
            //通知外部程序该事件
            if (OnDoubleClick != null)
            {
                OnDoubleClick(strDeviceNumber);
            }
            
        }
        private void UserControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                pMark = e.Location;
                pScreen = this.PointToScreen(pMark);
                contextMenuStrip1.Show(pScreen);
            }
            else if(e.Button == MouseButtons.Left)
            {
                //通知外部应用程序
                if (OnClick != null)
                {
                    string str = "{\"type\":\"select\",\"devicenum\":" + "\"" +  strDeviceNumber + "\"}";
                    OnClick(str);
                }
            }
            
        }
        public int calcDataIndexByPoint(Point p)
        {
            int index = beginIndex + (p.X - paddingLeft) / pix;
            if(index < beginIndex || index > endIndex)
            {
                return -1;
            }
            return index;
        }

        private void 添加标注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = calcDataIndexByPoint(pMark);
            //通知外部应用程序
            if (OnClick != null)
            {
                string str = "{\"type\":\"mark\",\"devicenum\":" + "\"" + strDeviceNumber + "\"" + "," + 
                               "\"index\":" + "\"" + index + "\"" + "," + 
                               "\"screenx\":" + "\"" + pScreen.X.ToString() + "\"" + "," + 
                               "\"screeny\":" + "\"" + pScreen.Y.ToString() + "\"" + "," +
                               "\"clientx\":" + "\"" + pMark.X.ToString() + "\"" + "," +
                               "\"clienty\":" + "\"" + pMark.Y.ToString() + "\"" + "," +
                               "}";
                OnClick(str);
            }
        }

        private void 清除记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = "{\"type\":\"clear\",\"devicenum\":" + "\"" + strDeviceNumber + "\"" + "}";
            if(OnClick != null)
            {
                OnClick(str);
            }
            
        }
    }
    public class FetalAlertStruct
    {
        public int x;
        public int y;
        public int width;
        public int heigth;
        public string content;
    }
}
