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
    [SerializeField] GameObject randomTrashContentItem;
    [SerializeField] Transform trashTargetTf;

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

    public void TrashAnimationsIN() {
        Sequence seq = DOTween.Sequence();

        // trash popup dissapears without content item
        seq.AppendCallback(() => {
            //TODO SUGENERUOT RANDOMINI ITEMA
            //jei ka randomTrashContentItem <-----
        });
        seq.Append(trashPopup.DOScale(1f, 0.75f));

        //TODO trash popup places in trash item location

        //next stuff
    }

    public void TrashAnimationsOUT() {
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
    }
}
