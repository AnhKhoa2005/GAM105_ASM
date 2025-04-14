// using System.Collections;
// using TMPro;
// using UnityEngine;

// public class Turorial : MonoBehaviour
// {
//     [SerializeField] TextMeshProUGUI Text;
//     string fullText = "WASD - Move\nJ - Attack\nK - Dash\n\nMission: Destroy enemies and collect enough diamonds to pass the level!";
//     Coroutine typingCoroutine;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     void OnTriggerEnter2D(Collider2D col)
//     {
//         if (col.CompareTag("Player"))
//         {
//             GameManager.instance.Tutorial.SetActive(true);
//             if (typingCoroutine != null)
//             {
//                 StopCoroutine(typingCoroutine);
//             }

//             typingCoroutine = StartCoroutine(TypeText());
//         }
//     }
//     void OnTriggerExit2D(Collider2D col)
//     {
//         if (col.CompareTag("Player"))
//         {
//             if (GameManager.instance.Tutorial != null)
//             {
//                 GameManager.instance.Tutorial.SetActive(false);
//                 if (typingCoroutine != null)
//                 {
//                     StopCoroutine(typingCoroutine);
//                     typingCoroutine = null;
//                 }

//                 Text.text = "";
//             }
//         }
//     }
//     IEnumerator TypeText()
//     {
//         Text.text = "";
//         foreach (char letter in fullText)
//         {
//             Text.text += letter;
//             yield return new WaitForSeconds(0.1f);
//         }
//     }

// }
