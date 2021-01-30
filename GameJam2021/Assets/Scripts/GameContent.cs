using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameContent : MonoBehaviour
{
    public static GameContent Instance;

    public List<ContentItem> rngNumbers;


    private void Awake() {
        Instance = this;
    }

    public List<ContentItem> getByTier(string tierName) {
        UnityEngine.Random.Range(0, 1);
        return rngNumbers.Where(x => x.name == tierName).OrderBy(x => Guid.NewGuid()).ToList();
    }
}
