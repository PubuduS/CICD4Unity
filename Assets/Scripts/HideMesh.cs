using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit.XRSDK.OpenXR;
using UnityEngine;

public class HideMesh : MonoBehaviour
{
    private bool m_IsMeshHidden = false;
    public void MeshHide()
    {
        // Get the first Mesh Observer available, generally we have only one registered
        var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
        var observer1 = CoreServices.GetSpatialAwarenessSystemDataProvider<OpenXRSpatialAwarenessMeshObserver>();

        if (!m_IsMeshHidden)
        {
            // Set to not visible
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.Occlusion;
            observer1.DisplayOption = SpatialAwarenessMeshDisplayOptions.Occlusion;
            m_IsMeshHidden = true;            
        }
        else
        {
            // Set to visible
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.Visible;
            observer1.DisplayOption = SpatialAwarenessMeshDisplayOptions.Visible;
            m_IsMeshHidden = false;
        }

    }
}
