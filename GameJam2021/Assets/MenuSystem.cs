using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MenuSystem : MonoBehaviour
{
    public static MenuSystem Instance;

    //menu canvas refs
    [SerializeField] Canvas menuCanvas;
    [SerializeField] CanvasGroup menuCanvasGroup;
    [SerializeField] Transform menuShakeTF;

    [Header("Logo refs")]
    [SerializeField] TextMeshProUGUI logo1tm;
    [SerializeField] TextMeshProUGUI logo2tm;
    [SerializeField] TextMeshProUGUI logo3tm;

    [Header("Story search junk refs")]
    [SerializeField] TextMeshProUGUI searchJunkStoryPT1;
    [SerializeField] TextMeshProUGUI searchJunkStoryPT2;
    [SerializeField] TextMeshProUGUI searchJunkStoryPT3;

    [Header("Button to search trash")]
    [SerializeField] TextMeshProUGUI trashButtontm;
    [SerializeField] Image panelImage;
    [SerializeField] Button button;

    [Header("Trash popup refs")]
    [SerializeField] Transform trashPopup;
    [SerializeField] ContentItemPanel randomTrashContentItem;
    [SerializeField] Transform trashTargetTf;

    [Header("Button to search goal")]
    [SerializeField] TextMeshProUGUI searchGoalButtontm;
    [SerializeField] Image searchPanelImage;
    [SerializeField] Button searchGoalButton;

    [Header("Goal popup refs")]
    [SerializeField] Transform goalPopup;
    [SerializeField] Transform goalContentItemParent;
    [SerializeField] GameObject goalContentItemPrefab;
    [SerializeField] GameObject contentItemPanelPrefab;
    [SerializeField] List<ContentItemPanel> randomGoalContentItems;
    [SerializeField] Transform goalTargetTf;

    [Header("Button to start Trade")]
    [SerializeField] TextMeshProUGUI startTradeGoalButtontm;
    [SerializeField] Image startTradePanelImage;
    [SerializeField] Button startTradeGoalButton;

    [SerializeField] Transform menuBG;



    private void Awake() {
        Instance = this;
        GameManager.OnGameStart += InitialInit;
    }

    private void InitialInit() {
        Sequence seq = DOTween.Sequence();

        //first animations (logo, buttons etc)        
        // 1 - LOGO (3PART LOGO)
        seq.Append(logo1tm.DOColor(Color.green, 0.5f).From(Color.clear));
        seq.Join(logo1tm.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.1f, 10f));
        seq.Join(logo2tm.DOColor(Color.red, 0.5f).From(Color.clear));
        seq.Join(logo2tm.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.1f, 10f));
        seq.Append(logo3tm.DOColor(Color.yellow, 0.5f).From(Color.clear));
        seq.Join(logo3tm.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.1f, 10f));

        // 2 - Story text
        seq.Append(searchJunkStoryPT1.DOColor(Color.white, 1f).From(Color.clear));
        seq.AppendInterval(1f);

        seq.Append(searchJunkStoryPT2.DOColor(Color.white, 1f).From(Color.clear));
        seq.AppendInterval(1f);

        seq.Append(searchJunkStoryPT3.DOColor(Color.white, 1f).From(Color.clear));
        seq.AppendInterval(1f);

        seq.Append(searchJunkStoryPT1.DOColor(Color.clear, 1f));
        seq.Join(searchJunkStoryPT2.DOColor(Color.clear, 1f));
        seq.Join(searchJunkStoryPT3.DOColor(Color.clear, 1f));

        // BUTTON TO SEARCH TRASH
        seq.Append(trashButtontm.DOColor(Color.white, 0.5f).From(Color.clear).OnComplete(() => {
            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true;
        }));
        seq.Join(panelImage.DOColor(new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 1f), 0.5f).From(Color.clear));
        seq.Join(panelImage.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.1f, 10f));
    }

    public void TrashPopupAnimationsIN() {
        Sequence seq = DOTween.Sequence();

        // trash popup dissapears without content item
        seq.AppendCallback(() => {
            ContentItem startItem = GameContent.Instance.getTrash();
            GameManager.Instance.SetStartItem(startItem);
            randomTrashContentItem.updateByItem(startItem);
        });
        seq.Append(trashPopup.DOScale(1f, 0.75f));
    }

    public void TrashPopupAnimationsOUT() {
        Sequence seq = DOTween.Sequence();

        // trash popup dissapears without content item
        seq.AppendCallback(() => {
            randomTrashContentItem.transform.SetParent(trashTargetTf);
        });
        seq.Append(menuShakeTF.DOShakeRotation(1f, 10f));
        seq.Append(randomTrashContentItem.GetComponent<RectTransform>().DOSizeDelta(Vector2.zero, 0.5f).SetEase(Ease.InQuint));
        seq.Join(randomTrashContentItem.transform.DOMove(trashTargetTf.position, 0.5f).SetEase(Ease.InQuint));
        seq.Append(trashPopup.DOScale(0f, 0.75f));

        //next stuff
        seq.AppendCallback(() => {
            GoalButtonAnimationsIN();
        });
    }

    public void GoalButtonAnimationsIN() {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => {
            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
            searchPanelImage.gameObject.SetActive(true);
        });
        seq.Append(searchGoalButtontm.DOColor(Color.white, 0.5f).From(Color.clear).OnComplete(() => {
            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true;
        }));
        seq.Join(searchPanelImage.DOColor(new Color(searchPanelImage.color.r, searchPanelImage.color.g, searchPanelImage.color.b, 1f), 0.5f).From(Color.clear));
        seq.Join(searchPanelImage.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.1f, 10f));
    }

    public void GoalPopupAnimationsIN() {
        Sequence seq = DOTween.Sequence();

        // trash popup dissapears without content item
        seq.AppendCallback(() => {
            Debug.Log("Append calback");
            //sugeneruot listuka random goalu ContentItem
            //sudet it randomGoalContentItems
            //painitializint 4 goalus ir priskirt parent goalContentItemParent
            randomGoalContentItems = new List<ContentItemPanel>();
            List<ContentItem> items = GameContent.Instance.getGoals();
            Debug.Log("content item count " + items.Count);
            foreach (ContentItem ct in GameContent.Instance.getGoals()) {
                Debug.Log("GOAL: " + ct.name);
                GameObject obj = Instantiate(goalContentItemPrefab, goalContentItemParent);
                ContentItemPanel cntPanel = obj.GetComponentInChildren<ContentItemPanel>();
                cntPanel.updateByItem(ct);
                randomGoalContentItems.Add(cntPanel);
            }
            
        });
        seq.Append(goalPopup.DOScale(1f, 0.75f));
    }

    public void GoalPopupAnimationsOUT(ContentItemPanel clickItemPanel) {
        GameObject item = Instantiate(contentItemPanelPrefab, clickItemPanel.transform.parent.position, Quaternion.identity, goalContentItemParent);
        ContentItemPanel panel = item.GetComponent<ContentItemPanel>();
        panel.updateByItem(clickItemPanel.item);
        //panel.GetComponent<RectTransform>().sizeDelta = clickItemPanel.GetComponent<RectTransform>().sizeDelta;
        panel.transform.SetParent(goalTargetTf);
        //item.SetActive(false);
        Sequence seq = DOTween.Sequence();

        foreach (ContentItemPanel p in randomGoalContentItems) {
            p.transform.parent.GetComponent<Button>().interactable = false;
        }
        seq.Append(menuShakeTF.DOShakeRotation(1f, 10f));
        seq.Append(item.gameObject.GetComponent<RectTransform>().DOSizeDelta(Vector2.zero, 0.5f).SetEase(Ease.InQuint));
        seq.Join(item.transform.DOMove(goalTargetTf.position, 0.5f).SetEase(Ease.InQuint));
        seq.Append(goalPopup.DOScale(0f, 0.75f).OnComplete(() => {
            foreach (Transform tf in goalContentItemParent) {
                Destroy(tf.gameObject);
            }
        }));
        //next stuff
        seq.OnComplete(StartTradeButtonAnimationsIN);
        
    }

    public void StartTradeButtonAnimationsIN() {
        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => {
            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
            startTradePanelImage.gameObject.SetActive(true);
        });
        seq.Append(startTradeGoalButtontm.DOColor(Color.white, 0.5f).From(Color.clear).OnComplete(() => {
            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true;
        }));
        seq.Join(startTradePanelImage.DOColor(new Color(startTradePanelImage.color.r, startTradePanelImage.color.g, startTradePanelImage.color.b, 1f), 0.5f).From(Color.clear));
        seq.Join(startTradePanelImage.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.1f, 10f));
    }

    public Tween HideMenu() {
        Sequence seq = DOTween.Sequence();
        seq.Append(menuShakeTF.transform.DOScale(0, 0.5f));
        seq.Join(menuBG.DOScale(0, 0.5f));
        seq.AppendCallback(() => {
            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
        });

        return seq;
        
    }

    public void ShowMenu() {

    }
}
