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
    [SerializeField] GameObject contentItemPrefab;


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

    private Tween EndTradeAnim() {
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => tradeCanvasGroup.alpha, x => tradeCanvasGroup.alpha = x, 0f, 1f));
        seq.AppendCallback(() => {
            tradeCanvasGroup.interactable = false;
            tradeCanvasGroup.blocksRaycasts = false;
        });

        if (GameManager.Instance.GetStartItem() != null) {
            seq.Append(MenuSystem.Instance.getTrashTargetTF().DOScale(0, 0.7f));
            seq.Join(MenuSystem.Instance.getTrashTargetTF().DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));
            seq.AppendCallback(() => {
                foreach (Transform tf in MenuSystem.Instance.getTrashTargetTF()) {
                    Destroy(tf.gameObject);
                }
                if (GameManager.Instance.GetStartItem() != null) {
                    GameObject go = Instantiate(contentItemPrefab, MenuSystem.Instance.getTrashTargetTF());
                    go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                    ContentItemPanel panel = go.GetComponent<ContentItemPanel>();
                    panel.updateByItem(GameManager.Instance.GetStartItem());
                    panel.gameObject.SetActive(false);
                    MenuSystem.Instance.randomTrashContentItem = panel;
                }
            });
            seq.AppendCallback(() => MenuSystem.Instance.randomTrashContentItem.gameObject.SetActive((true)));
            seq.Append(MenuSystem.Instance.getTrashTargetTF().DOScale(1, 0.7f));
            seq.Join(MenuSystem.Instance.getTrashTargetTF().DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));
        } else {

            seq.Append(MenuSystem.Instance.getTrashTargetTF().DOScale(0, 0.7f));
            seq.Join(MenuSystem.Instance.getTrashTargetTF().DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));
            seq.Join(MenuSystem.Instance.getGoalTF().DOScale(0, 0.7f));
            seq.Join(MenuSystem.Instance.getGoalTF().DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));

            seq.AppendCallback(() => {
                foreach (Transform tf in MenuSystem.Instance.getTrashTargetTF()) {
                    Destroy(tf.gameObject);
                }
                foreach (Transform tf in MenuSystem.Instance.getGoalTF()) {
                    Destroy(tf.gameObject);
                }
            });
        }

        return seq;
    }

    private void InitStartAndGoal() {
        this.currentPlayerItem = GameManager.Instance.GetStartItem();
        updateCurrentItem(GameManager.Instance.GetStartItem());
        goalItem = GameManager.Instance.GetGoalItem();
        renewTradeItems();
        trader1.AnimateIn();
        trader2.AnimateIn();
        playerItem.AnimateIn();
        trader1.EnableButton();
        trader2.EnableButton();
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
        Sequence seq = DOTween.Sequence();
        // DISABLE ALL BUTTONS
        DisableAllButton();
        this.currentPlayerItem = selectedItem;

        //ANIMS
        if (trader1.returnItem().itemName == selectedItem.name) {
            seq.Append(trader1.AnimateOut());
            seq.Join(playerItem.AnimateOut());
            seq.Join(trader2.AnimateOut());
            seq.AppendInterval(GameManager.CARD_OPEN_TIME);
            seq.AppendCallback(() => updateCurrentItem(selectedItem));
            seq.Append(playerItem.AnimateIn());
            
        } else {
            seq.Append(trader2.AnimateOut());
            seq.Join(playerItem.AnimateOut());
            seq.Join(trader1.AnimateOut());
            seq.AppendInterval(GameManager.CARD_OPEN_TIME);
            seq.AppendCallback(() => updateCurrentItem(selectedItem));
            seq.Append(playerItem.AnimateIn());
            
        }
        
        if (currentPlayerItem == goalItem)
        {
            seq.AppendCallback(() => {
                StopCoroutine(gameCoroutine);
                EndTrade(true);
            });
        } else {
            seq.AppendCallback(() => renewTradeItems());
            seq.Append(trader1.AnimateIn());
            seq.Join(trader2.AnimateIn());
            seq.AppendCallback(() => {
                trader1.EnableButton();
                trader2.EnableButton();
            });
            
        }
    }

    void DisableAllButton() {
        trader1.DisableButton();
        trader2.DisableButton();
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
        EndTradeAnim().OnComplete(() => {
            OnTradeEnd?.Invoke();
        });
        
        
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
        this.playerItem.updateByItem(currentPlayerItem);
    }

}
