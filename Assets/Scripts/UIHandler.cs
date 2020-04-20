using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour
{
    public GameObject minimap;
    private bool mapZoomed = false;

    public GameObject codePanel;
    private bool codePanelOpened = false;
    private float codePaneleWidth = 0; 

    private void Awake()
    {
        codePaneleWidth = Mathf.Abs(codePanel.transform.position.x);
    }

    public void MiniMapZoom()
    {
        StartCoroutine(MiniMapSizeChange());
    }

    public IEnumerator MiniMapSizeChange()
    {

        for (int i = 0; i < 10; i++)
        {
            if (!mapZoomed)
            {
                minimap.transform.localScale = minimap.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0f);
            }
            else
            {
                minimap.transform.localScale = minimap.transform.localScale - new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(0f);
            }
        }

        mapZoomed = !mapZoomed;

    }

    //CodePanel

    public void CodePanelOpen()
    {
        var clickedObject = EventSystem.current.currentSelectedGameObject;
        StartCoroutine(CodePanel(clickedObject));
    }

    public IEnumerator CodePanel(GameObject clickedObject)
    {
       

        for (int i = 0; i < 10; i++)
        {
            if (!codePanelOpened)
            {
                codePanel.transform.localPosition = codePanel.transform.localPosition + new Vector3(codePaneleWidth/10, 0, 0);
                clickedObject.transform.localPosition = clickedObject.transform.localPosition + new Vector3(codePaneleWidth/10, 0, 0);
                
                yield return new WaitForSeconds(0f);
            }
            else
            {
                codePanel.transform.localPosition = codePanel.transform.localPosition - new Vector3(codePaneleWidth/10, 0, 0);
                clickedObject.transform.localPosition = clickedObject.transform.localPosition - new Vector3(codePaneleWidth/10, 0, 0);
                yield return new WaitForSeconds(0f);
            }
        }
        clickedObject.transform.Rotate(new Vector3(0, 0, 180));
        codePanelOpened = !codePanelOpened;
        Debug.Log(codePanelOpened);
    }
}
