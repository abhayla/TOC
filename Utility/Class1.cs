using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOC
{
    public class IndexData
    {
        public string indexName = "";
        public float open = 0.0f;
        public float high = 0.0f;
        public float low = 0.0f;
        public float ltp = 0.0f;
        public float ch = 0.0f;
        public float per = 0.0f;
        public float yCls = 0.0f;
        public float mCls = 0.0f;
        public float yHigh = 0.0f;
        public float yLow = 0.0f;
    }

    public class CompanyData
    {
        public string symbol = "";
        public string open = "";
        public string high = "";
        public string low = "";
        public string ltP = "";
        public string ptsC = "";
        public string per = "";
        public string trdVol = "";
        public string trdVolM = "";
        public string ntP = "";
        public string mVal = "";
        public string wkhi = "";
        public string wklo = "";
        public string wkhicm_adj = "";
        public string wklocm_adj = "";
        public string xDt = "";
        public string cAct = "";
        public string yPC = "";
        public string mPC = "";
    }

    public class LiveData
    {
        public float trdVolumesumMil = 0.0f;
        public DateTime time = DateTime.Now;
        public int declines = 0;
        public float trdValueSum = 0.0f;
        public float trdValueSumMil = 0.0f;
        public int unchanged = 0;
        public float trdVolumesum = 0.0f;
        public int advances = 0;
        public CompanyData[] data;
        public IndexData[] latestData;
    }
}