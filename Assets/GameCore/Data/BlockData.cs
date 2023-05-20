using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Data
{
    public class BlockData
    {
        public List<Color> GateColors = new();
        public List<Vector3> PositionOffsets = new();
        public List<bool> WhichGatesShouldBeMoving = new();
        public Color CorrectColor;
        public float MixValue = 0.8f;
    }
}