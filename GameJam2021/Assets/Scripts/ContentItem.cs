using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ContentItem", order = 1)]
public class ContentItem : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public Color color;
    public GameContent.ITEM_TIER itemTier;
    public AudioSource itemSound;
}
