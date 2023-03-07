using Basic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SQLUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using H_Pannel_lib;
using System.Diagnostics;
namespace 智慧調劑台管理系統_WebApi
{
  

    public class MyTimer
    {
        private bool OnTick = false;
        public double TickTime = 0;
        static private Stopwatch stopwatch = new Stopwatch();
        public MyTimer()
        {
            stopwatch.Start();
        }
        public MyTimer(double TickTime)
        {
            stopwatch.Start();
            this.StartTickTime(TickTime);
        }

        private double CycleTime_start;
        public void StartTickTime(double TickTime)
        {
            this.TickTime = TickTime;
            if (!OnTick)
            {
                CycleTime_start = stopwatch.Elapsed.TotalMilliseconds;
                OnTick = true;
            }
        }

        public void StartTickTime()
        {
            if (!OnTick)
            {
                CycleTime_start = stopwatch.Elapsed.TotalMilliseconds;
                OnTick = true;
            }
        }
        public double GetTickTime()
        {
            return stopwatch.Elapsed.TotalMilliseconds - CycleTime_start;
        }
        public void TickStop()
        {
            this.OnTick = false;
        }
        public bool IsTimeOut()
        {
            //if (OnTick == false) return false;
            if ((stopwatch.Elapsed.TotalMilliseconds - CycleTime_start) >= TickTime)
            {
                OnTick = false;
                return true;
            }
            else return false;
        }



        public override string ToString()
        {
            return this.ToString(true);
        }
        public string ToString(bool retick)
        {
            string text = this.GetTickTime().ToString("0.000") + "ms";
            if (retick)
            {
                this.TickStop();
                this.StartTickTime(999999);
            }
            return text;

        }

    }
}
