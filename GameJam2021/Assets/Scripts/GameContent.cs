using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameContent : MonoBehaviour
{
    public static GameContent Instance;

    public List<ContentItem> content;


    private void Awake() {
        Instance = this;
    }

    public enum ITEM_TIER { TIER_1 = 0, TIER_2 = 1, TIER_3 = 2, TIER_4 = 3, TIER_5 = 4, TIER_6 = 5, TIER_7 = 6, TIER_8 = 7 }
}
