using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TradeSystem : MonoBehaviour
{
    public static TradeSystem Instance;
    
    [SerializeField] PlayerItem playerItem;
    [SerializeField] Trader trader1;
    [SerializeField] Trader trader2;
    [SerializeField] Transform timeCounter;
    [SerializeField] float timeLimit = 5.0f;

    private ContentItem currentPlayerItem;
    private float startTime;
    

    private void Awake()
    {
        Instance = this;
        //GameContent.OnContentLoad += GO;

    }

    // Start is called before the first frame update
    void GO()
    {
        updateCurrentItem(GameContent.Instance.content[0]);

        renewTradeItems();
        StartCoroutine(TradeStart());
    }

    IEnumerator TradeStart()
    {
        startTime = Time.time;
        //timeCounter.maxValue = timeLimit;
        timeCounter.DOScaleX(1f, timeLimit).From(0).SetEase(Ease.OutBounce);
        while(Time.time - startTime < timeLimit)
        {
            yield return null;
        }
        Debug.Log("Trade ended");
        //cia viskas po trade end
    }

    public void SelectItem(ContentItem selectedItem)
    {
        updateCurrentItem(selectedItem);
        renewTradeItems();
    }

    private void renewTradeItems()
    {
        List<ContentItem> tradeItems = GameContent.Instance.getAroundTier((int)currentPlayerItem.itemTier, 2, currentPlayerItem);
        updateTrader(trader1, tradeItems[0]);
        updateTrader(trader2, tradeItems[1]);
    }

    void updateTrader(Trader trader, ContentItem item)
    {
        trader.updateByitem(item);
    }

    void updateCurrentItem(ContentItem item)
    {
        this.currentPlayerItem = item;
        this.playerItem.updateByItem(currentPlayerItem);
    }

}
