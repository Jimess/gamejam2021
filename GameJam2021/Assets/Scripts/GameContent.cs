using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameContent : MonoBehaviour
{
    public static GameContent Instance;

    public List<ContentItem> content;

    public delegate void OnContentLoadDelegate();
    public static OnContentLoadDelegate OnContentLoad;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        LoadContent();
        OnContentLoad?.Invoke();
    }

    private void LoadContent() {
        content = Resources.LoadAll<ContentItem>("ContentItems").ToList();
    }

    public enum ITEM_TIER { TIER_1 = 0, TIER_2 = 1, TIER_3 = 2, TIER_4 = 3, TIER_5 = 4, TIER_6 = 5, TIER_7 = 6, TIER_8 = 7 }

    public ContentItem getTrash()
    {
        return getOneByTier(0);
    }

    public List<ContentItem> getGoals()
    {
        return getFromTierRange(1, 7, 4);
    }

    private ContentItem getOneByTier(int tier)
    {
        List<ContentItem> tierList = getByTier(tier);
        return tierList[UnityEngine.Random.Range(0, tierList.Count)];
    }

    private List<ContentItem> getByTier(int tier)
    {
        return content.FindAll(item => (int) item.itemTier == tier ).ToList();
    }

    private List<ContentItem> getFromTierRange(int startTier, int endTier, int resultCount)
    {
        List<ContentItem> result = new List<ContentItem>();
        List<ContentItem> tierList = new List<ContentItem>();
        for (int i = startTier; i <= endTier; i++)
        {
            tierList.AddRange(getByTier(i));
        }
        result.AddRange(getRandomItems(tierList, resultCount));
        return result;
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
        result.AddRange(getRandomItems(aroundTierList, resultCount));
        return result;
    }

    private List<ContentItem> getRandomItems(List<ContentItem> originalList, int resultCount)
    {
        List<ContentItem> randomResults = new List<ContentItem>();
        ContentItem randomResult;
        if (originalList.Count >= resultCount)
        {
            for (int i = 0; i < resultCount; i++)
            {
                randomResult = originalList[UnityEngine.Random.Range(0, originalList.Count)];
                randomResults.Add(randomResult);
                originalList.Remove(randomResult);
            }
        }
        return randomResults;
    }
}
