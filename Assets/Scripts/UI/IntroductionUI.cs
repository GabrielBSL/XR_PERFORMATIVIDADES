using Main.Events;
using UnityEngine;

namespace Main.UI
{
    public class IntroductionUI : MonoBehaviour
    {
        private void Start()
        {
            MainEventsManager.onIntroUI?.Invoke(this);
        }
    }
}