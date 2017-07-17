using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WaveControl.ControlDataEntity;

namespace TestPro
{
    public partial class form1 : Form
    {
        int num = 0;
        int num1 = 0;
        public form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            // 获取屏幕分辨率
            //int SH = Screen.PrimaryScreen.Bounds.Height;
            //int SW = Screen.PrimaryScreen.Bounds.Width;
            int width = this.Size.Width;
            int height = this.Size.Height;
            //height = 300;
            Size size = new Size(width, height);
            userControl11.Location = new Point(0, 0);

            userControl11.SetControlSize(size);
            //userControl11.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string time = ts.TotalMilliseconds.ToString();
            time = DateTime.Now.ToString();
            string[] data = new string[12];
            Random r = new Random();
            data[0] = "false";
            data[1] = "false";
            data[2] = "";
            data[3] = "false";
            data[4] = "";
            data[5] = "true";
            data[6] = Convert.ToInt32(Math.Sin(num++) * 20 + 150).ToString();
            data[7] = "true";
            data[8] = Convert.ToInt32(Math.Sin(num++) * 20 + 90).ToString();
            data[9] = Convert.ToInt32(Math.Sin(num++) * 20 + 80).ToString();
            data[10] = Convert.ToInt32(Math.Sin(num++) * 20 + 40).ToString();
            data[11] = time;
            num1++;
            if(num1 % 30 == 0)
            {
                data[1] = "true";
            }
            DataEntity entity = new DataEntity();
            entity.HR1_SensorDrop = 0;
            entity.HR2_SensorDrop = 0;
            entity.TOCOSensorDrop = 0;
            entity.bFetal1Alert = true;
            entity.bFetal2Alert = true;
            entity.FetalHeart1 = Convert.ToInt32(data[6]);
            entity.FetalHeart2 = Convert.ToInt32(data[8]);
            entity.FetalMove = Convert.ToInt32(data[9]);
            entity.TOCOValue = Convert.ToInt32(data[10]);
            entity.SampleTime = data[11];
            entity.Mark = 1;
            entity.FHRSame = 1;
            entity.ManualFetalMove = -1000;
            entity.bPcMark = false;
            entity.yPcMark = 0.3;
            entity.PcMark = "dadsa";
            string str = JsonConvert.SerializeObject(entity);
            userControl11.AddSampleData(str);
            //userControl11.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string time = ts.TotalMilliseconds.ToString();
            time = DateTime.Now.Ticks.ToString();
            string[] data = new string[5];
            Random r = new Random();
            data[0] = r.Next(80, 120).ToString();
            data[1] = time;

            //userControl11.AddSampleData(data);
            //userControl11.Invalidate();
        }

        private void form1_Resize(object sender, EventArgs e)
        {
            userControl11.SetControlSize(this.Size);
        }

        private void userControl11_MouseClick(object sender, MouseEventArgs e)
        {
            string[] data = new string[12];
            Random r = new Random();
            string time = DateTime.Now.ToString();
            data[0] = "false";
            data[1] = "false";
            data[2] = "";
            data[3] = "false";
            data[4] = "";
            data[5] = "true";
            data[6] = Convert.ToInt32(Math.Sin(num++) * 20 + 150).ToString();
            data[7] = "true";
            data[8] = Convert.ToInt32(Math.Sin(num++) * 20 + 90).ToString();
            data[9] = Convert.ToInt32(Math.Sin(num++) * 20 + 80).ToString();
            data[10] = Convert.ToInt32(Math.Sin(num++) * 20 + 40).ToString();
            data[11] = time;
            DataEntity entity = new DataEntity();
            entity.HR1_SensorDrop = 0;
            entity.HR2_SensorDrop = 0;
            entity.TOCOSensorDrop = 0;
            entity.bFetal1Alert = true;
            entity.bFetal2Alert = false;
            entity.FetalHeart1 = Convert.ToInt32(data[6]);
            entity.FetalHeart2 = Convert.ToInt32(data[8]);
            entity.FetalMove = Convert.ToInt32(data[9]);
            entity.TOCOValue = Convert.ToInt32(data[10]);
            entity.SampleTime = data[11];
            entity.Mark = 1;
            entity.FHRSame = 1;
            entity.ManualFetalMove = -1000;
            entity.bPcMark = true;
            entity.yPcMark = 0.343423432423434;
            entity.PcMark = "dadsa";
            string str = JsonConvert.SerializeObject(entity);
            userControl11.SetSampleData(10,str);
        }
    }
}
