using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerItem : MonoBehaviour
{

    private ContentItem item;
    public ContentItemPanel panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateByItem(ContentItem item)
    {
        this.item = item;
        updatePanel();
    }

    private void updatePanel()
    {
        panel.updateByItem(this.item);
    }

    public ContentItem returnItem()
    {
        return this.item;
    }

    public Tween AnimateOut() {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(Vector2.up * (Random.Range(0, 2) == 0 ? 90 : -90), GameManager.CARD_OPEN_TIME, RotateMode.LocalAxisAdd));

        return seq;
    }

    public Tween AnimateIn() {
        return transform.DORotate(Vector2.zero, GameManager.CARD_OPEN_TIME);
    }
}
