using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopkeeper : MonoBehaviour
{
    public TextMesh popupText;

    public AudioClip enterShopSound;

    private bool playerInShop;

    public GameObject shopMenu;
    public GameObject bargainMenu;

    // Item Spawn area
    public Transform spawnLocation;

    public OpenAIController openAIController;

    // Start is called before the first frame update
    void Start()
    {
        playerInShop = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If player is at shop and presses F
        if (Input.GetKeyDown(KeyCode.F) && playerInShop && !bargainMenu.activeSelf)
        {
            // Open / Close store menu page
            shopMenu.SetActive(!shopMenu.activeSelf);
        }

        if (!playerInShop)
        {
            // Close store menu page
            shopMenu.SetActive(false);

            // Close Bargain menu
            bargainMenu.SetActive(false);
        }

        // If player wants to quit 
        if (Input.GetKeyDown(KeyCode.Escape) && playerInShop)
        {
            // Close store menu page
            shopMenu.SetActive(false);

            // Close Bargain menu
            bargainMenu.SetActive(false);
        }
    }

    // Function which spawns weapons if bought
    public void spawnItem(GameObject item)
    {
        // Spawn item and rename it
        GameObject spawnedItem = Instantiate(item, spawnLocation.position, spawnLocation.rotation);

        spawnedItem.name = item.name;
    }

    public void Bargain(GameObject item, int price)
    {
        // Close Menu
        shopMenu.SetActive(false);

        // Open Bargain menu
        bargainMenu.SetActive(true);

        // Start chat with OpenAI
        openAIController.StartBargainConversation(item, price);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If player is within shop radius & presses 'E'
        if (collision.gameObject.CompareTag("Player"))
        {
            // Open Shop Menu
            playerInShop = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        AudioSource sound_source = gameObject.GetComponent<AudioSource>();

        // If player is within shop radius
        if (collision.gameObject.CompareTag("Player") && !sound_source.isPlaying)
        {
            // Display shop text
            popupText.text = "'F' - Talk to suspicious man";


            // Play enter shop sound
            sound_source.PlayOneShot(enterShopSound);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Clear text
            popupText.text = string.Empty;

            // Shows that player is not within radius of shop
            playerInShop = false;
        }
    }
}
