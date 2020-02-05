using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PerspectiveUI : Singleton<PerspectiveUI>
{
    private List<PerspectiveIndicator> perspectiveIndicators;

    void Awake()
    {
        // Get perspective indicators from children
        perspectiveIndicators = GetComponentsInChildren<PerspectiveIndicator>().ToList();
    }

    public void SelectPerspective(Perspective perspective)
    {
        // Select associated UI indicator
        PerspectiveIndicator selected = perspectiveIndicators.Find(p => p.cameraView.Equals(perspective.cameraView));
        selected.SetSelected(true);

        // Deselect all others
        foreach (PerspectiveIndicator perspectiveIndicator in perspectiveIndicators.Where(p => p.cameraView != perspective.cameraView))
        {
            perspectiveIndicator.SetSelected(false);
        }
    }
}
