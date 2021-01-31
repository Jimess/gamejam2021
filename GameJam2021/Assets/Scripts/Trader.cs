using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Trader : MonoBehaviour
{
    private ContentItem item;
    [SerializeField] ContentItemPanel panel;
    [SerializeField] Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateByitem(ContentItem item)
    {
        this.item = item;
        updatePanel();
    }

    public void selectItem()
    {
        TradeSystem.Instance.SelectItem(item);
    }
         
    private void updatePanel()
    {
        panel.updateByItem(item);
    }

    public ContentItem returnItem()
    {
        return this.item;
    }

    public void DisableButton() {
        button.interactable = false;
    }

    public void EnableButton() {
        button.interactable = true;
    }

    public Tween AnimateOut() {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(Vector2.up * (Random.Range(0, 1) == 0 ? 90 : -90), GameManager.CARD_OPEN_TIME, RotateMode.LocalAxisAdd));

        return seq;
    }

    public Tween AnimateIn() {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(Vector2.zero, GameManager.CARD_OPEN_TIME));
        return seq;
    }
}
