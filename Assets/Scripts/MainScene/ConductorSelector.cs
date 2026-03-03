using System.Collections.Generic;
using UnityEngine;

public class ConductorSelector : MonoBehaviour
{
    int activeIndex = 0;
    [SerializeField] List<ConductorSO> conductorSOs;
    List<GameObject> panels = new List<GameObject>();
    [SerializeField] GameObject panelPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < conductorSOs.Count; i++)
        {
            GameObject panel = Instantiate(panelPrefab, transform);
            panel.GetComponent<ConductorPanel>().SetupPanel(conductorSOs[i]);
            panels.Add(panel);
        }

        UpdateActivePanelVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CycleRight()
    {
        activeIndex = (activeIndex + 1)%panels.Count;  
        UpdateActivePanelVisuals();
    }

    public void CycleLeft() 
    {
        activeIndex = activeIndex - 1;
        if(activeIndex < 0)
        {
            activeIndex = panels.Count - 1;
        }
        UpdateActivePanelVisuals();
    }

    public void UpdateActivePanelVisuals()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            if (i == activeIndex)
            {
                panels[i].SetActive(true);
            }
            else
            {
                panels[i].SetActive(false);
            }
        }
    }

    public ConductorSO GetActiveConductor()
    {
        return conductorSOs[activeIndex];
    }
}
