using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public GameObject uiPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiPanel.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("UIPanelTrigger"))
        {
            uiPanel.SetActive(true);
        }
    }
}
