using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeSystem : MonoBehaviour
{

    public ContentItem currentPlayerItem;
    [SerializeField] PlayerItem playerItem;
    [SerializeField] Trader trader1;
    [SerializeField] Trader trader2;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        ContentItem testItem1 = GameContent.Instance.content[0];
        updateTrader(this.trader1, testItem1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateTrader(Trader trader, ContentItem item)
    {
        trader.updateByitem(item);
    }

    void updateCurrentItem(ContentItem item)
    {
        this.currentPlayerItem = item;
        this.playerItem.updateByItem(this.currentPlayerItem);
    }

}
