using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialMessage : MonoBehaviour
{
    public string Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PopupText popUp = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopupText>();

            popUp.PopUp(Text);

            // Disable gameobject after a period of time
            Invoke("DisableObject", .5f);
        }
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
