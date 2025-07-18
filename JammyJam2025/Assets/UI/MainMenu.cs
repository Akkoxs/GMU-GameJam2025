    using UnityEngine;
    using System.Collections;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    public class MainMenu : MonoBehaviour
    {

        [SerializeField] private RawImage BlackScreen;
        [SerializeField] private Transform playerSprite;
        [SerializeField] private Transform CenterPoint;

        static Color BlFade;

        private float blackScreenTime = 1.3f;
        private float moveIncrement = 0.00001f;
        private bool isFalling = false; 
        private Vector3 p_pos;
        private Vector3 cp;

        public void Start()
        {
            BlFade = BlackScreen.color;
            BlFade.a = 1f;
            BlackScreen.color = BlFade;
            StartCoroutine(FadeScreen(BlFade, 1f, 0f, 1f, blackScreenTime, 0f, false, false));
        }

        public void Update(){
            playerSprite.position = p_pos;
            if (!isFalling){
                StartCoroutine(FallSway());
                isFalling = true;
            }
        }

        //while true, sway
        //maybe have a lerp movment to a new Y
        //new Y is calculated after place is reached as a RAND between 2 limits
        private IEnumerator FallSway(){
            //elapsed = 0;
            while(p_pos.y > cp.y){
                p_pos.y -= moveIncrement;
                if(p_pos.y > cp.y){
                    p_pos.y = cp.y;
                }
                yield return null;
            }
            while(true){
                //p_pos.y = playerSprite.position.y;
                yield return null; 
            }
        }

    public IEnumerator FadeScreen(Color UI, float start, float end, float duration, float delay, float endDelay, bool isActive, bool endScene)
    { //copy and pasted code because im stupid
        yield return new WaitForSecondsRealtime(delay);
        float elapsed = 0;
        while (duration > elapsed)
        {
            UI.a = Mathf.Lerp(start, end, elapsed / duration);
            BlackScreen.color = UI;
            elapsed += Time.deltaTime;
            yield return null;
        }
        UI.a = end;
        BlackScreen.color = UI;
        BlackScreen.gameObject.SetActive(isActive);
        yield return new WaitForSecondsRealtime(endDelay);
            if (endScene){
                SceneManager.LoadSceneAsync(1); //this is horrible but idc anymore 
                yield break;
            }
        }

        public void PlayGame(){
            BlackScreen.gameObject.SetActive(true);
            BlFade = BlackScreen.color;
            BlFade.a = 0f;
            BlackScreen.color = BlFade;
            StartCoroutine(FadeScreen(BlFade, 0f, 1f, 1.5f, 0f, blackScreenTime, true, true));
        }

        public void QuitGame(){
            Application.Quit();
        }

    }
