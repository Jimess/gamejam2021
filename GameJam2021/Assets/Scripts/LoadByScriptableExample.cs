using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadByScriptableExample : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameMesh;
    [SerializeField] Image spriteImage;
    [SerializeField] TextMeshProUGUI tierMesh;
    [SerializeField] Image spriteColor;


    // Start is called before the first frame update
    void Start()
    {
        ContentItem testItem = GameContent.Instance.content[0];
        nameMesh.text = testItem.itemName;
        spriteImage.sprite = testItem.sprite;
        tierMesh.text = testItem.itemTier.ToString();
        spriteColor.color = testItem.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
