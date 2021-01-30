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

    public List<ContentItem> getByTier(int tier)
    {
        return content.FindAll(item => (int) item.itemTier == tier ).ToList();
    }

    public List<ContentItem> getAroundTier(int tier, int resultCount, ContentItem itemToExclude)
    {
        List<ContentItem> result = new List<ContentItem>();
        List<ContentItem> aroundTierList = new List<ContentItem>();
        aroundTierList.AddRange(getByTier(tier));
        if (tier - 1 >= 0)
        {
            aroundTierList.AddRange(getByTier(tier - 1));
        }
        if (tier + 1 < Enum.GetNames(typeof(ITEM_TIER)).Length)
        {
            aroundTierList.AddRange(getByTier(tier + 1));
        }
        aroundTierList.Remove(itemToExclude);
        aroundTierList = aroundTierList.OrderBy(x => Guid.NewGuid()).ToList();
        ContentItem randomResult;
        if (aroundTierList.Count >= resultCount)
        {
            for (int i = 0; i < resultCount; i++)
            {
                randomResult = aroundTierList[UnityEngine.Random.Range(0, aroundTierList.Count)];
                result.Add(randomResult);
                aroundTierList.Remove(randomResult);
            }
        }
        Debug.Log(result);
        return result;




    }
}
