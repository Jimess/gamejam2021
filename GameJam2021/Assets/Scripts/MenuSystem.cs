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
    [SerializeField] Transform logoGopnik;

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
    public ContentItemPanel randomTrashContentItem;
    [SerializeField] Transform randomTrashContentSpawnParent;

    [SerializeField] Transform trashTargetTf;
    [SerializeField] RectTransform trashTargetStartRTF;
    [SerializeField] RectTransform trashTargetEndRTF;

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
    public Image showArrow;

    private ContentItemPanel newSelectedGoalContentItem;
    [SerializeField] Transform goalTargetTf;
    [SerializeField] RectTransform goalTargetTfStartRTF;
    [SerializeField] RectTransform goalTargetTfEndRTF;

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
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
        seq.Join(logo2tm.DOColor(Color.red, 0.5f).From(Color.clear));
        seq.Join(logo2tm.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
        seq.Append(logo3tm.DOColor(Color.yellow, 0.5f).From(Color.clear));
        seq.Join(logo3tm.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
        seq.Append(logoGopnik.DOScale(100f, 1f).From(0).SetEase(Ease.InQuint));
        //seq.Join(logoGopnik.DOColor(Color.green, 0.5f).From(Color.clear));
        //seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
        seq.AppendCallback(() => {
            logoGopnik.GetComponent<Animator>().SetTrigger("dance");
        });

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
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
    }

    public void TrashButtonAnimationIN() {
        Sequence seq = DOTween.Sequence();
        // BUTTON TO SEARCH TRASH
        seq.AppendCallback(() => panelImage.gameObject.SetActive(true));
        seq.Append(trashButtontm.DOColor(Color.white, 0.5f).From(Color.clear).OnComplete(() => {
            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true;
        }));
        seq.Join(panelImage.DOColor(new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 1f), 0.5f).From(Color.clear));
        seq.Join(panelImage.transform.DOScale(1f, 0.5f).From(10f).SetEase(Ease.InQuint));
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
    }

    public void TrashPopupAnimationsIN() {
        Sequence seq = DOTween.Sequence();

        // trash popup dissapears without content item
        seq.AppendCallback(() => {
            if (randomTrashContentItem) {
                Debug.Log("DESTROY");
                Destroy(randomTrashContentItem.gameObject);
            }
                
            GameObject go = Instantiate(contentItemPanelPrefab, randomTrashContentSpawnParent);
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            ContentItemPanel panel = go.GetComponent<ContentItemPanel>();
            ContentItem startItem = GameContent.Instance.getTrash();
            GameManager.Instance.SetStartItem(startItem);
            panel.updateByItem(startItem);
            randomTrashContentItem = panel;
        });
        seq.Append(trashPopup.DOScale(1f, 0.75f));
    }

    public void TrashPopupAnimationsOUT() {
        Sequence seq = DOTween.Sequence();

        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
        //seq.Append(trashTargetTf.GetComponent<RectTransform>().DOAnchorPos(trashTargetStartRTF.anchoredPosition, 0.5f));
        //seq.AppendCallback(() => randomTrashContentItem.gameObject.SetActive((true)));
        //seq.Append(trashTargetTf.GetComponent<RectTransform>().DOAnchorPos(trashTargetEndRTF.anchoredPosition, 0.5f));
        seq.Append(trashTargetTf.DOScale(0, 0.7f));
        seq.Join(trashTargetTf.DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));
        seq.AppendCallback(() => {
            foreach(Transform tf in trashTargetTf) {
                Destroy(tf.gameObject);
            }
            Destroy(randomTrashContentItem.gameObject);
            GameObject go = Instantiate(contentItemPanelPrefab, trashTargetTf);
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            ContentItemPanel panel = go.GetComponent<ContentItemPanel>();
            panel.updateByItem(GameManager.Instance.GetStartItem());
            panel.gameObject.SetActive(false);
            randomTrashContentItem = panel;
        });
        seq.AppendCallback(() => randomTrashContentItem.gameObject.SetActive((true)));
        seq.Append(trashTargetTf.DOScale(1, 0.7f));
        seq.Join(trashTargetTf.DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));

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
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
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
        //set goal
        GameManager.Instance.SetGoalItem(clickItemPanel.item);

        Sequence seq = DOTween.Sequence();

        foreach (ContentItemPanel p in randomGoalContentItems) {
            p.transform.parent.GetComponent<Button>().interactable = false;
        }
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
        //seq.Append(goalTargetTf.GetComponent<RectTransform>().DOAnchorPos(goalTargetTfStartRTF.anchoredPosition, 0.5f));
        seq.Append(goalTargetTf.DOScale(0, 0.7f));
        seq.Join(goalTargetTf.DORotate(Vector3.forward * 360f, 0.7f, RotateMode.WorldAxisAdd));
        seq.AppendCallback(() => {
            foreach (Transform tf in goalTargetTf) {
                Destroy(tf.gameObject);
            }
            GameObject go = Instantiate(contentItemPanelPrefab, goalTargetTf);
            go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            ContentItemPanel panel = go.GetComponent<ContentItemPanel>();
            panel.updateByItem(clickItemPanel.item);
            panel.gameObject.SetActive(false);
            newSelectedGoalContentItem = panel;
        });
        seq.AppendCallback(() => {
            newSelectedGoalContentItem.gameObject.SetActive(true);
        });
        seq.Append(goalTargetTf.DOScale(1, 0.7f));
        seq.Join(goalTargetTf.DORotate(Vector3.back * 360f, 0.7f, RotateMode.WorldAxisAdd));
        seq.Append(showArrow.transform.DOScale(1f, 0.5f).From(0));
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));

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
        seq.Append(menuShakeTF.DOShakeRotation(0.2f, Vector3.forward * 10));
    }

    public Tween HideMenu() {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => {
            startTradePanelImage.gameObject.SetActive(false);
        });
        seq.Append(menuShakeTF.transform.DOScale(0, 0.5f));
        seq.Join(menuBG.DOScale(0, 0.5f));
        seq.AppendCallback(() => {
            menuCanvasGroup.blocksRaycasts = false;
            menuCanvasGroup.interactable = false;
        });

        return seq;
        
    }

    public void ShowMenu() {
        Sequence seq = DOTween.Sequence();
        seq.Append(menuShakeTF.transform.DOScale(1, 0.5f));
        seq.Join(menuBG.DOScale(1, 0.5f));
        seq.AppendCallback(() => {
            menuCanvasGroup.blocksRaycasts = true;
            menuCanvasGroup.interactable = true;
        });
        if (GameManager.Instance.GetStartItem() != null) {
            Debug.Log("SHOW MENU! " + GameManager.Instance.GetStartItem() != null);
            seq.AppendCallback(StartTradeButtonAnimationsIN);
        } else {
            Debug.Log("SHOW MENU FROM START " + GameManager.Instance.GetStartItem() != null);
            seq.AppendCallback(TrashButtonAnimationIN);
        }
    }

    public Transform getTrashTargetTF() {
        return trashTargetTf;
    }

    public Transform getGoalTF() {
        return goalTargetTf;
    }
}
