using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ContentItem", order = 1)]
public class ContentItem : ScriptableObject
{
    public string name;
    public Sprite sprite;
}
