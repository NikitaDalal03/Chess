using UnityEngine;

public class ChessboardViewport : MonoBehaviour
{
    public Camera _camera;

    public void Start()
    {
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        float targetAspect = 1080f / 2400f; // Target portrait aspect (example: 1080x2400 for phones)
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleWidth = targetAspect / windowAspect;

        _camera.orthographicSize = scaleWidth > 1 ? 6f * scaleWidth : 6f;

    }
}
