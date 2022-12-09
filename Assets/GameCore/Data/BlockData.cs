using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameCore.Data
{
    public class BlockData
    {
        public Guid ID = new Guid();
        public List<Color> GateColors;
        public Color CorrectColor;
        public float Score = 1f;
    }
}

