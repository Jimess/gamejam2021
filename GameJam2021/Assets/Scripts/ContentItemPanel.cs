using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContentItemPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameMesh;
    [SerializeField] Image spriteImage;

    public void updateByItem(ContentItem item)
    {
        nameMesh.text = item.itemName;
        spriteImage.sprite = item.sprite;
        spriteImage.color = item.color;
    }

}
