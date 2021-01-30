using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContentItemPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameMesh;
    [SerializeField] Image spriteImage;
    [SerializeField] TextMeshProUGUI tierMesh;
    [SerializeField] Image spriteColor;

    public void updateByItem(ContentItem item)
    {
        nameMesh.text = item.itemName;
        spriteImage.sprite = item.sprite;
        tierMesh.text = item.itemTier.ToString();
        spriteColor.color = item.color;
    }

}
