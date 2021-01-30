using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    private ContentItem item;
    [SerializeField] ContentItemPanel panel;

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

    }
         
    private void updatePanel()
    {
        panel.updateByItem(this.item);
    }

    public ContentItem returnItem()
    {
        return this.item;
    }
}
