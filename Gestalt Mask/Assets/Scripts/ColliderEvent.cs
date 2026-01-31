using System.Collections.Generic;
using UnityEngine;
public class ColliderEvent : MonoBehaviour
{

    [SerializeField] bool disableAllAtStart;


    [Header("true = enable on Enter / false = enable on exit")]
    [SerializeField] bool enableAllScript_WhenEnter;

    [SerializeField] List<MonoBehaviour> allScript = new List<MonoBehaviour>();

    private void Start()
    {
        if (disableAllAtStart == false) return;

        foreach (var script in allScript)
        {
            script.enabled = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;

        if (enableAllScript_WhenEnter)
        {
            foreach (var script in allScript)
            {
                script.enabled = true;
             
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;

        if (enableAllScript_WhenEnter == false)
        {
            foreach (var script in allScript)
            {
                script.enabled = true;
            }
        }
    }
}
