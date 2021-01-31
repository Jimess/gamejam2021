using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TradeSystem : MonoBehaviour
{
    public static TradeSystem Instance;
    public delegate void OnTradeEndDelegate();
    public static OnTradeEndDelegate OnTradeEnd;

    [SerializeField] PlayerItem playerItem;
    [SerializeField] Trader trader1;
    [SerializeField] Trader trader2;
    [SerializeField] Transform timeCounter;
    [SerializeField] float timeLimit = 5.0f;

    private ContentItem goalItem;
    private ContentItem currentPlayerItem;
    private float startTime;

    [Header("Start Anim refs")]
    [SerializeField] CanvasGroup tradeCanvasGroup;


    Coroutine gameCoroutine;

    private void Awake()
    {
        Instance = this;
        GameManager.OnTradeLevelStart += StartTradeAnim;

    }

    private void StartTradeAnim() {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(InitStartAndGoal);
        seq.Append(DOTween.To(() => tradeCanvasGroup.alpha, x => tradeCanvasGroup.alpha = x, 1f, 1f));
        seq.AppendCallback(() => {
            tradeCanvasGroup.interactable = true;
            tradeCanvasGroup.blocksRaycasts = true;
        });
        seq.OnComplete(GO);
    }

    private void InitStartAndGoal() {
        updateCurrentItem(GameManager.Instance.GetStartItem());
        goalItem = GameManager.Instance.GetGoalItem();
        renewTradeItems();
    }

    // Start is called before the first frame update
    void GO()
    {
        gameCoroutine = StartCoroutine(TradeStart());
    }

    IEnumerator TradeStart()
    {
        startTime = Time.time;
        timeCounter.DOScaleX(1f, timeLimit).From(0).SetEase(Ease.OutBounce);
        while(Time.time - startTime < timeLimit)
        {
            yield return null;
        }
        EndTrade(false);
    }

    public void SelectItem(ContentItem selectedItem)
    {
        updateCurrentItem(selectedItem);
        if (currentPlayerItem == goalItem)
        {
            StopCoroutine(gameCoroutine);
            EndTrade(true);
        } else
        {
            renewTradeItems();
        }
    }

    private void EndTrade(bool win)
    {
        if (win)
        {
            GameManager.Instance.SetStartItem(null);
            GameManager.Instance.SetGoalItem(null);
        } else
        {
            GameManager.Instance.SetStartItem(currentPlayerItem);
        }

        Debug.Log("Theee eeend");
        OnTradeEnd?.Invoke();
        
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
