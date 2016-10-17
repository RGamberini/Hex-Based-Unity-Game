using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum HexType {
    FIELD, FOREST, PASTURE, MOUNTAIN, HILLS, DESERT
}

public static class HexExtensions {
    public static Color color(this HexType hexType) {
        switch(hexType) {
            case HexType.FOREST:
                return new Color(56 / 255f, 142 / 255f, 60 / 255f, 1);
            case HexType.PASTURE:
                return new Color(139 / 255f, 195 / 255f, 74 / 255f, 1);
            case HexType.DESERT:
                return new Color(255 / 255f, 236 / 255f, 179 / 255f, 1);
            case HexType.FIELD:
                return new Color(255 / 255f, 235 / 255f, 59 / 255f, 1);
            case HexType.HILLS:
                return new Color(255 / 255f, 87 / 255f, 34 / 255f, 1);
            case HexType.MOUNTAIN:
                return new Color(97 / 255f, 97 / 255f, 97 / 255f, 1);
            default:
                return Color.black;
        }
    }
}