using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageProcessing02
{
    internal class HiPerfTimer
    {
        //引用win32API中的QueryPerFormanceCounter()方法
        //该方法用来查询任意时刻高精度计数器的实际值
        [DllImport("Kernel32.dll")] //using System.Runtime.InteropServices
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        //引用win32API中的QueryPerFormanceCounter()方法
        //该方法用来查询任意时刻高精度计数器的实际值
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        public HiPerfTimer()
        {
            startTime = 0;
            stopTime = 0;

            if(QueryPerformanceFrequency(out freq) == false)
            {
                //不支持高性能计时器
                throw new Win32Exception(); //using System.ComponentModel;
            }
        }

        //开始计时
        public void Start()
        {
            //让等待线程工作
            Thread.Sleep(0); //using System.Threading;

            QueryPerformanceCounter(out startTime);
        }
        //结束计时
        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }
        //返回计时结果(ms)
        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime) * 1000 / (double)freq;
            }
        }
    }
}
