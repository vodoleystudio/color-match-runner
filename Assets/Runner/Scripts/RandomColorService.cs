using System;
using System.Collections.Generic;
using UnityEngine;
using static ColorComponentsEnum;
using Random = UnityEngine.Random;
   
public class RandomColorService : IRandomColorService
{
    private Dictionary<ColorComponents,float> _colorComponents = new Dictionary<ColorComponents, float>
    {
        { ColorComponents.R, 0f },
        { ColorComponents.G, 0f },
        { ColorComponents.B, 0f },  
    };

    public Color GetRandomColor()
    {
        foreach (var item in (ColorComponents[])Enum.GetValues(typeof(ColorComponents)))
        {
            _colorComponents[item] = Random.Range(0, 1f);
        }
        
        return new Color(_colorComponents[ColorComponents.R], _colorComponents[ColorComponents.G], _colorComponents[ColorComponents.B]);
    }
}
