﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlycReSoft.TandemMSGlycopeptideGUI.ConfigMenus
{
    public class AlgorithmSettings : JsonConfig
    {
        public static double SchemaVersion = 0.23;
        public static double MS1ToleranceDefault = 1 / Math.Pow(10, 5);
        public static double MS2ToleranceDefault = 2 / Math.Pow(10, 5);
        public static int NumProcessesDefault = 6;
        public static int NumDecoysDefault = 5;
        public static bool OnlyRandomDecoysDefault = false;

        public double MS1MassErrorTolerance { get; set; }
        public double MS2MassErrorTolerance { get; set; }
        public int NumProcesses { get; set; }
        public int NumDecoys { get; set; }
        public bool OnlyRandomDecoys { get; set; }
        public double Version;

        public AlgorithmSettings()
        {
            Version = SchemaVersion;
            MS1MassErrorTolerance = MS1ToleranceDefault;
            MS2MassErrorTolerance = MS2ToleranceDefault;
            NumProcesses = NumProcessesDefault;
            NumDecoys = NumDecoysDefault;
            OnlyRandomDecoys = OnlyRandomDecoysDefault;
        }
    }
}
