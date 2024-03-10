using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OpenAIController : MonoBehaviour
{
    // Chat history box
    public TMP_Text textField;

    // User input box
    public TMP_InputField inputField;

    // User input submit button
    public Button okButton;

    // API for chatgpt
    private OpenAIAPI api;

    // Stores list of messages sent and received from chatgpt, meant for memory usage
    private List<ChatMessage> messages;

    // This is the game object to be spawned
    private GameObject gunObject;

    // Shopkeeper
    public shopkeeper shopkeeper;

    // Shopui
    public shopUI wallet;

    // GPT chat UI
    public GameObject GPTChatUI;

    // Sound for sold item
    public AudioClip kaChingSound;

    // Start is called before the first frame update
    void Start()
    {
        // API KEY (Try not to get this out)
        api = new OpenAIAPI("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");

        // If user presses the click button, wait for response
        okButton.onClick.AddListener(() => GetResponse());
    }

    // Function to start conversation with user
    private void StartConversation()
    {
        // Store the very first prompt
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "For the rest of this chat you will be referred to as 'John'. You are a black market salesman selling a 'toy rifle' for 600 cash. Use the following rules as a guide:In the first reply introduce yourself. You keep your responses short and to the point. Describe what you are and use a tone where you are trying to sell something to the customer. Tell the user what items you are selling. Then prompt the user to manually write in a response. Try your best to sell the 'toy rifle' but do not sell it more than 600 Dollars. Once you have decided to sell the 'toy rifle' at a certain price. If the user agrees with the price, stop all conversation and only reply with: @Sold_For, 'toy rifle' and the price agreed upon")
        };

        // Set the input text field as empty
        inputField.text = "";

        // Writes what happens at the start of the conversation
        string startString = "You have just approached a suspicious person loitering around. He beckons you to come over.";
        textField.text = startString;
    }

    public void StartBargainConversation(GameObject gameObject, int price)
    {
        // Sets the gun object to be spawned
        gunObject = gameObject;

        // Get gun name and gun price
        string gunPrice = price.ToString();
        string gunName = gameObject.name;

        // Remove chat history

        string message = "For the rest of this chat you will be referred to as 'John'. You are a black market salesman selling a " + gunName + " for " + gunPrice + " cash. Use the following rules as a guide:In the first reply introduce yourself. You keep your responses short and to the point. Describe what you are and use a tone where you are trying to sell something to the customer. Tell the user what items you are selling. Then prompt the user to manually write in a response. Try your best to sell the " + gunName + " but do not sell it more than 600 Dollars. Once you have decided to sell the " + gunName + " at a certain price. If the user agrees with the price, stop all conversation and only reply with: @Sold, " + gunName + " and the price agreed upon";

        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, message)
        };


        // Writes what happens at the start of the conversation
        string startString = "You have just approached a suspicious person loitering around. He beckons you to come over.";
        textField.text = startString;
    }

    private async void GetResponse()
    {
        // If user tries to send a text with no words inside
        if (inputField.text.Length < 1)
        {
            // Do not do anything
            return;
        }

        // Disable submit button when waiting for response
        okButton.enabled = false;

        // Create user message 
        ChatMessage userMessage = new ChatMessage();

        // Set the message role as user
        userMessage.Role = ChatMessageRole.User;

        // Set the content of the message as the text written by user
        userMessage.Content = inputField.text;

        // Limits replies, for cost saving purposes (Im a broke person)
        if (userMessage.Content.Length > 400)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 400);
        }

        // Add sent message to history of messages
        messages.Add(userMessage);

        // Update the text field with what the user sent
        textField.text = string.Format("You: {0}", userMessage.Content);

        // Clears text input field
        inputField.text = "";

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.2,
            MaxTokens = 50,
            Messages = messages
        });

        // Get response message from chatGPT
        ChatMessage responseMessage = new ChatMessage();

        // Set the role as first role
        responseMessage.Role = chatResult.Choices[0].Message.Role;

        // Get the message content from chatGPT
        responseMessage.Content = chatResult.Choices[0].Message.Content;

        // @Sold, " + gunName + " and the price agreed upon"
        if (responseMessage.Content.Contains("@Sold,"))
        {
            // Get all occurrences of integers in the string
            MatchCollection matches = Regex.Matches(responseMessage.Content, @"\d+");

            // Iterate through all matches
            foreach (Match match in matches)
            {
                // Convert each match into
                int priceSold = Int32.Parse(match.Value);

                // If there is enough money in the wallet
                if (wallet.deductMoney(priceSold) == true)
                {

                    // Spawn gun
                    shopkeeper.spawnItem(gunObject);

                    // Play kaching sound
                    AudioSource.PlayClipAtPoint(kaChingSound, Camera.main.transform.position);
                }

                // Disable shop
                wallet.gameObject.SetActive(false);

                // Disable chat
                GPTChatUI.SetActive(false);

                // Resume movement
                Time.timeScale = 1f;

                break;
            }

        }

        // Add the response to the list of messages
        messages.Add(responseMessage);

        // Update the text field with the response
        textField.text = string.Format("You: {0}\n\nNot suspicious person: {1}", userMessage.Content, responseMessage.Content);

        // Enable the submit button for user to send
        okButton.enabled = true;
    }
}
