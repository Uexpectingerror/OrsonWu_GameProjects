using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ColorDataPack
{
    public Material mPaintMaterial;
    public LayerMask mLayer;
    public int mPlayerID;
    public ColorState mColorState;
    public Color mColor;
}

public enum ColorState
{
    Blue,
    Red,
    Clean,
    Contested
}


public class ColorManager : MonoBehaviour
{
    public ColorDataPack blueStatePack;
    public ColorDataPack redStatePack;
    public ColorDataPack TileDefaultStatePack;
    public ColorDataPack TowerDefaultStatePack;
    public ColorDataPack TowerContestedStatePack;

    public ColorDataPack GetColorDataPack (ColorState state, string tagName)
    {
        if (state == ColorState.Blue)
        {
            return blueStatePack;
        }
        else if (state == ColorState.Red)
        {
            return redStatePack;
        }
        else if (state == ColorState.Clean)
        {
            if(tagName == "Tile")
            {
                return TileDefaultStatePack;
            }
            else
            {
                return TowerDefaultStatePack;
            }
        }
        else if (state == ColorState.Contested)
        {
            return TowerContestedStatePack;
        }
        else
        {
            return TileDefaultStatePack;
        }

    }
    
}

