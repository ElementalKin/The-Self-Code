using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


//[CustomPropertyDrawer(typeof(HexCoordinates))]
//public class HexCoordinatesDrawer : PropertyDrawer
//{

//    public override void OnGUI(
//        Rect position, SerializedProperty property, GUIContent label
//    )
//    {
//        HexCoordinates coordinates = new HexCoordinates(
//            property.FindPropertyRelative("x").intValue,
//            property.FindPropertyRelative("z").intValue
//        );

//        position = EditorGUI.PrefixLabel(position, label);
//        GUI.Label(position, coordinates.ToString());
//    }
//}

public enum HexDirection
{
    NE, E, SE, SW, W, NW
}

public static class HexDirectionExtensions
{

    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}



[System.Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int x, z;

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }
    }

    public int Y
    {
        get
        {
            return -X - Z;
        }
    }

    public HexCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x, z);
    }

    public override string ToString()
    {
        return "(" +
            X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }
}


public class TileValues : MonoBehaviour
{
    public int tileType;
    public int arrayPostion;
    public bool FOW = true;
    [SerializeField]
    GameObject[] neighbors;

    public GameObject GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(HexDirection direction, GameObject cell)
    {
        neighbors[(int)direction] = cell;
        cell.GetComponent<TileValues>().neighbors[(int)direction.Opposite()] = gameObject;
    }
    public HexCoordinates coordinates;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
