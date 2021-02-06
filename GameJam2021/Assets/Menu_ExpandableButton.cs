using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GameConsts;

public class Menu_ExpandableButton : MonoBehaviour
{
    private Vector2 originalPosition;
    private Vector2 originalSize;
    private bool expanded = false;
    private Tween mainTween;

    private void Start() {
        originalPosition = transform.position;
        originalSize = transform.GetComponent<RectTransform>().sizeDelta;
    }

    public void OnClick_ToggleExpand() {
        if (expanded) {
            mainTween.Kill();
            mainTween = Shrink();
            expanded = false;
        } else {
            mainTween.Kill();
            mainTween = Expand();
            expanded = true;
        }
    }

    private Tween Expand() {
        Sequence seq = DOTween.Sequence();

        Vector2 expandedSize = new Vector2(Consts.SCREEN_WIDTH - (2 * Consts.MENU_ITEM_EXPANDED_MARGINS), Consts.SCREEN_HEIGHT - (2 * Consts.MENU_ITEM_EXPANDED_MARGINS));
        transform.GetComponent<Canvas>().sortingOrder = 1;

        seq.Join(transform.DOMove(Vector2.zero, Consts.MENU_ITEM_EXPANSION_SPEED));
        seq.Join(transform.GetComponent<RectTransform>().DOSizeDelta(expandedSize, Consts.MENU_ITEM_EXPANSION_SPEED));

        return seq;
    }

    private Tween Shrink() {
        Sequence seq = DOTween.Sequence();

        Vector2 srinkedSize = new Vector2(Consts.MENU_ITEM_WIDTH, Consts.MENU_ITEM_HEIGHT);

        seq.Join(transform.DOMove(originalPosition, Consts.MENU_ITEM_EXPANSION_SPEED));
        seq.Join(transform.GetComponent<RectTransform>().DOSizeDelta(srinkedSize, Consts.MENU_ITEM_EXPANSION_SPEED));
        seq.OnComplete(() => {
            transform.GetComponent<Canvas>().sortingOrder = 0;
        });

        return seq;
    }
}
