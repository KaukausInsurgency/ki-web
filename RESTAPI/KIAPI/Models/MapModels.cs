﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KIAPI.Classes;

namespace KIAPI.Models
{
    public class MapLayerModel
    {
        public Resolution Resolution { get; set; }
        public string ImagePath { get; set; }
        public MapLayerModel(Resolution res, string path)
        {
            Resolution = res;
            ImagePath = path;
        }
    }

    public class GameMapModel
    {
        public bool MapExists { get; set; }
        public Resolution Resolution { get; set; }
        public string ImagePath { get; set; }
        public Position DCSOriginPosition { get; set; }
        public double Ratio;
        public List<MapLayerModel> Layers { get; set; }
    }
}