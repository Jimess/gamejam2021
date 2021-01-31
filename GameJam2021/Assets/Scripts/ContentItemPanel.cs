using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContentItemPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameMesh;
    [SerializeField] Image spriteImage;
    public ContentItem item;

    public void updateByItem(ContentItem item)
    {
        this.item = item;
        nameMesh.text = item.itemName;
        spriteImage.sprite = item.sprite;
        spriteImage.color = item.color;
    }

    public void OnGoalSelected()
    {
        MenuSystem.Instance.GoalPopupAnimationsOUT(this);
    }

}
