using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeSystem : MonoBehaviour
{
    public static TradeSystem Instance;
    
    [SerializeField] PlayerItem playerItem;
    [SerializeField] Trader trader1;
    [SerializeField] Trader trader2;
    [SerializeField] Slider timeCounter;
    [SerializeField] float timeLimit = 5.0f;

    private ContentItem currentPlayerItem;
    private float startTime;
    

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        updateCurrentItem(GameContent.Instance.content[0]);

        renewTradeItems();
        StartCoroutine(TradeStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            TogglePause();
        }
        timeCounter.value = Time.time - startTime;
    }

    void TogglePause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Debug.Log("PAUSED");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("UNPAUSED");
        }
    }

    IEnumerator TradeStart()
    {
        startTime = Time.time;
        timeCounter.maxValue = timeLimit;
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
