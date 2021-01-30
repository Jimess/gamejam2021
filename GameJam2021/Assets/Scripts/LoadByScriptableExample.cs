using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LoadByScriptableExample : MonoBehaviour
{
    [SerializeField] ContentItemPanel trader1;
    [SerializeField] ContentItemPanel trader2;


    // Start is called before the first frame update
    void Start()
    {
        ContentItem testItem1 = GameContent.Instance.content[0];
        ContentItem testItem2 = GameContent.Instance.content[0];
        trader1.updateByItem(testItem1);
        trader2.updateByItem(testItem2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
