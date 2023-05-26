using UnityEngine;
[CreateAssetMenu(fileName ="ThemeData",menuName ="Themes/Create New Theme")]
public class ThemeData : ScriptableObject
{
    [Header("Theme Details")]
    public string _ThemeName;
    public int _ThemeCost;
    public Sprite _ThemeIcon;
    public WorldType _ThemeType;

    [Header("Theme Content")]
    public ThemePlace[] _Places;
    public GameObject _Collectable;

    [Header("Theme Content")]
    public Color _FogColor;
}
[System.Serializable]
public struct ThemePlace
{
    public int _Length;
    public GameObject[] _BuildingPrefabs;
}
