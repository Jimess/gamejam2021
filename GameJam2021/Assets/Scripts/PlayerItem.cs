using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
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
}
