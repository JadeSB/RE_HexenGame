using UnityEngine;

namespace Utils
{
    class DontDestroyOnLoad: MonoBehaviour
    {
        //Dit dient om te checken of onze singelton al in onze scene zit.
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
