
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace WaveControl
{
    public class ControlDataEntity
    {
        public DataEntity entity = new DataEntity();
        /// <summary>
        /// 是否需要画竖线
        /// </summary>
        public bool isDrawVLine;
        /// <summary>
        /// 是否需要画时间
        /// </summary>
        public bool isDrawTime;
        /// <summary>
        /// 是否需要画刻度
        /// </summary>
        public bool isDrawNum;
        /// <summary>
        /// 胎心坐标系
        /// </summary>
        public static int[] tValue = { 210, 180, 150, 120, 90, 60 };
        /// <summary>
        /// 宫缩压坐标系
        /// </summary>
        public static int[] bValue = { 100, 80, 60, 40, 20, 0 };

        public static ControlDataEntity ConvertEntity(string data)
        {
            ControlDataEntity DataEntity = new ControlDataEntity();
            DataEntity entity = JsonConvert.DeserializeObject<DataEntity>(data);
            DataEntity.entity = entity;
            DataEntity.isDrawTime = false;
            DataEntity.isDrawNum = false;
            DataEntity.isDrawVLine = false;
            return DataEntity;
        }
        public class DataEntity
        {
            //版本
            public string version = "";
            //设备号
            public string device = "";
            //警告信息，包括各种导联脱落的警告
            public int FHRSame;
            public int HR1_SensorDrop;
            public int HR2_SensorDrop;
            public int MHR_SensorDrop;
            public int TOCOSensorDrop;
            //自动胎动
            public int FetalMove = -1000;
            //胎心率1
            public int FetalHeart1;
            //胎心率2
            public int FetalHeart2;
            //母亲心率
            public int MotherHeart;
            //母亲体温
            public int MotherTemperature;
            //母亲血氧
            public int MotherOxygen;
            //宫缩压类型
            public int TOCOType;
            //宫缩压值
            public int TOCOValue;
            //收缩压
            public int BPSystolic;
            //舒张压
            public int BPDiastolic;
            //平均压
            public int BPMean;
            //标注信息
            public int Mark = -1000;
            //母亲呼吸
            public int MotherBreathRate;
            public int FetalMoveFrom;//0 自动胎动。1 手动胎动
            //手动胎动
            public int ManualFetalMove;
            //PC标注
            public bool bPcMark;
            public string PcMark;
            public double yPcMark;
            //对于绘图添加的一些额外的信息
            public bool bFetal1Alert = false;
            public string strFetal1AlertContent = "";
            public bool bFetal2Alert = false;
            public string strFetal2AlertContent = "";
            //采样时间
            public string SampleTime = "";
        }

    }

    public class ControlDataCenter
    {
        public List<ControlDataEntity> datalist = new List<ControlDataEntity>();
        public void AddData(ControlDataEntity entity)
        {
            datalist.Add(entity);
        }
        public void DelData(int index)
        {
            datalist.RemoveAt(index);
        }
        public void ClearData()
        {
            datalist.Clear();
        }
        public ControlDataEntity GetData(int index)
        {
            if(index >= datalist.Count)
            {
                return null;
            }
            else
            {
                return datalist[index];
            }
            
        }
        public void SetData(int index, ControlDataEntity entity)
        {
            datalist[index] = entity;
        }
        public List<ControlDataEntity> GetDataSequnce(int begin, int end)
        {
            List<ControlDataEntity> data = new List<ControlDataEntity>();
            for (int i = begin, j = 0; i < end; i++, j++)
            {
                data.Add(datalist[i]);
            }
            return data;
        }
        public int GetSize()
        {
            return datalist.Count;
        }
        public List<int> GetChildHeart1Data(int begin, int end)
        {
            List<int> d = new List<int>();
            List<ControlDataEntity> data = GetDataSequnce(begin, end);
            for (int i = 0; i < end - begin; i++)
            {
                d.Add(data[i].entity.FetalHeart1);
            }
            return d;
        }
    }
}
