﻿using System;
using System.Collections.Generic;

namespace Circuits
{
    class CircuitParameters
    {
        public string name { get; set; }
        public int numInputs { get; set; }
        public int numOutputs { get; set; }
        public int maxLevel { get; set; }
        public int numEdges { get; set; }
        //public int reconvergation { get; set; }
        public double reliability { get; set; }
        public double size { get; set; }
        public double area { get; set; }
        public int longest_path { get; set; }
        public int gates { get; set; }
        public double sensitivity_factor { get; set; }
        public double reliability_percent { get; set; }
        public double sensitive_area { get; set; }
        public double sensitive_area_percent { get; set; }
        //public int averageDistanceBetweenElements { get; set; }
        public string hashCode { get; set; }
        public Dictionary<string, int> numElementsOfEachType { get; set; }
        public Dictionary<Tuple<string, string>, int> numEdgesOfEachType { get; set; }
        public CircuitParameters()
        {

        }        
    }
}
