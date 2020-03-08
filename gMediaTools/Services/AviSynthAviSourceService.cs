﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public class AviSynthAviSourceService : IAviSynthSourceService
    {
        public string GetAviSynthSource(string filename)
        {
            return $"AviSource(\"{filename}\"){Environment.NewLine}";
        }
    }
}
