using System.Collections;
using UnityEngine;

namespace CC.Core.Save
{
    public class SaveScreenCapture : MonoBehaviour
    {
        public void captureScreen(Component sender, object data)
        {
            if (data is int) StartCoroutine(CaptureScreen((int)data));
        }

        public IEnumerator CaptureScreen(int slot)
        {
            yield return null;
            Canvas canv = FindObjectOfType<Canvas>();
            canv.enabled = false;
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/GameData/SaveShot" + slot);
            canv.enabled = true;
        }
    }
}
