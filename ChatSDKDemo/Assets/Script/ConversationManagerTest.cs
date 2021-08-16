﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ChatSDK;

public class ConversationManagerTest : MonoBehaviour
{
    private Text conversationText;
    private Toggle chatToggle;
    private Toggle groupToggle;
    private Toggle roomToggle;
    private Button backButton;
    private Button LastMessageBtn;
    private Button LastReceiveMessageBtn;
    private Button GetExtBtn;
    private Button SetExtBtn;
    private Button UnReadCountBtn;
    private Button MarkMessageAsReadBtn;
    private Button MarkAllMessageAsReadBtn;
    private Button InsertMessageBtn;
    private Button AppendMessageBtn;
    private Button UpdateMessageBtn;
    private Button DeleteMessageBtn;
    private Button DeleteAllMessageBtn;
    private Button LoadMessageBtn;
    private Button LoadMessagesBtn;
    private Button LoadMessagesWithKeywordBtn;
    private Button LoadMessagesWithTimeBtn;
    private Button LoadMessagesWithMsgTypeBtn;

    private string conversationId {
        get => conversationText.text;
    }

    private ConversationType convType {
        get {
            if (chatToggle.isOn)
            {
                return ConversationType.Chat;
            }
            else if (groupToggle.isOn)
            {
                return ConversationType.Group;
            }
            else {
                return ConversationType.Room;
            }
        }
    }

    private void Awake()
    {
        Debug.Log("conversation manager test script has load");

        conversationText = transform.Find("TextField/Text").GetComponent<Text>();

        ToggleGroup toggleGroup = transform.Find("ChatToggleGroup").GetComponent<ToggleGroup>();

        chatToggle = toggleGroup.transform.Find("Single").GetComponent<Toggle>();
        groupToggle = toggleGroup.transform.Find("Single").GetComponent<Toggle>();
        roomToggle = toggleGroup.transform.Find("Single").GetComponent<Toggle>();

        backButton = transform.Find("BackBtn").GetComponent<Button>();

        backButton.onClick.AddListener(backButtonAction);

        LastMessageBtn = transform.Find("Scroll View/Viewport/Content/LastMessageBtn").GetComponent<Button>();
        LastReceiveMessageBtn = transform.Find("Scroll View/Viewport/Content/LastReceiveMessageBtn").GetComponent<Button>();
        GetExtBtn = transform.Find("Scroll View/Viewport/Content/GetExtBtn").GetComponent<Button>();
        SetExtBtn = transform.Find("Scroll View/Viewport/Content/SetExtBtn").GetComponent<Button>();
        UnReadCountBtn = transform.Find("Scroll View/Viewport/Content/UnReadCountBtn").GetComponent<Button>();
        MarkMessageAsReadBtn = transform.Find("Scroll View/Viewport/Content/MarkMessageAsReadBtn").GetComponent<Button>();
        MarkAllMessageAsReadBtn = transform.Find("Scroll View/Viewport/Content/MarkAllMessageAsReadBtn").GetComponent<Button>();
        InsertMessageBtn = transform.Find("Scroll View/Viewport/Content/InsertMessageBtn").GetComponent<Button>();
        AppendMessageBtn = transform.Find("Scroll View/Viewport/Content/AppendMessageBtn").GetComponent<Button>();
        UpdateMessageBtn = transform.Find("Scroll View/Viewport/Content/UpdateMessageBtn").GetComponent<Button>();
        DeleteMessageBtn = transform.Find("Scroll View/Viewport/Content/DeleteMessageBtn").GetComponent<Button>();
        DeleteAllMessageBtn = transform.Find("Scroll View/Viewport/Content/DeleteAllMessageBtn").GetComponent<Button>();
        LoadMessageBtn = transform.Find("Scroll View/Viewport/Content/LoadMessageBtn").GetComponent<Button>();
        LoadMessagesBtn = transform.Find("Scroll View/Viewport/Content/LoadMessagesBtn").GetComponent<Button>();
        LoadMessagesWithKeywordBtn = transform.Find("Scroll View/Viewport/Content/LoadMessagesWithKeywordBtn").GetComponent<Button>();
        LoadMessagesWithTimeBtn = transform.Find("Scroll View/Viewport/Content/LoadMessagesWithTimeBtn").GetComponent<Button>();
        LoadMessagesWithMsgTypeBtn = transform.Find("Scroll View/Viewport/Content/LoadMessagesWithMsgTypeBtn").GetComponent<Button>();


        LastMessageBtn.onClick.AddListener(LastMessageBtnAction);
        LastReceiveMessageBtn.onClick.AddListener(LastReceiveMessageBtnAction);
        GetExtBtn.onClick.AddListener(GetExtBtnAction);
        SetExtBtn.onClick.AddListener(SetExtBtnAction);
        UnReadCountBtn.onClick.AddListener(UnReadCountBtnAction);
        MarkMessageAsReadBtn.onClick.AddListener(MarkMessageAsReadBtnAction);
        MarkAllMessageAsReadBtn.onClick.AddListener(MarkAllMessageAsReadBtnAction);
        InsertMessageBtn.onClick.AddListener(InsertMessageBtnAction);
        AppendMessageBtn.onClick.AddListener(AppendMessageBtnAction);
        UpdateMessageBtn.onClick.AddListener(UpdateMessageBtnAction);
        DeleteMessageBtn.onClick.AddListener(DeleteMessageBtnAction);
        DeleteAllMessageBtn.onClick.AddListener(DeleteAllMessageBtnAction);
        LoadMessageBtn.onClick.AddListener(LoadMessageBtnAction);
        LoadMessagesBtn.onClick.AddListener(LoadMessagesBtnAction);
        LoadMessagesWithKeywordBtn.onClick.AddListener(LoadMessagesWithKeywordBtnAction);
        LoadMessagesWithTimeBtn.onClick.AddListener(LoadMessagesWithTimeBtnAction);
        LoadMessagesWithMsgTypeBtn.onClick.AddListener(LoadMessagesWithMsgTypeBtnAction);
    }


    void backButtonAction()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    void LastMessageBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);

            if (conv.LastMessage != null)
            {
                UIManager.SuccessAlert(transform);
            }
            else
            {
                UIManager.DefaultAlert(transform, "未获取到最后一条消");
            }

        });

        UIManager.DefaultInputAlert(transform, config);
    }
    void LastReceiveMessageBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
          
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);

            if (conv.LastReceivedMessage != null)
            {
                UIManager.SuccessAlert(transform);
            }
            else
            {
                UIManager.DefaultAlert(transform, "未获取到最后一条消");
            }

        });

        UIManager.DefaultInputAlert(transform, config);
    }
    void GetExtBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
      
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);

            if (conv.Ext != null)
            {
                List<string> list = new List<string>();
                foreach (var kv in conv.Ext) {
                    list.Add($"{kv.Key}:{kv.Value}");
                }
                string str = string.Join(",", list.ToArray());
                UIManager.DefaultAlert(transform, str);
            }
            else
            {
                UIManager.DefaultAlert(transform, "未获取到Ext");
            }

        });

        UIManager.DefaultInputAlert(transform, config);
    }
    void SetExtBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
            string key = dict["key"];
            string value = dict["value"];
          
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);

            Dictionary<string, string> KV = new Dictionary<string, string>();
            KV.Add(key, value);
            conv.Ext = KV;
            UIManager.DefaultAlert(transform, "已设置");

        });

        config.AddField("key");
        config.AddField("value");

        UIManager.DefaultInputAlert(transform, config);

        Debug.Log("SetExtBtnAction");
    }
    void UnReadCountBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);
            UIManager.DefaultAlert(transform, $"未读数: {conv.UnReadCount.ToString()}");

        });
        
        UIManager.DefaultInputAlert(transform, config);
        Debug.Log("UnReadCountBtnAction");
    }
    void MarkMessageAsReadBtnAction()
    {

        InputAlertConfig config = new InputAlertConfig((dict) =>
        {

            string msgId = dict["MsgId"];
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);
            conv.MarkMessageAsRead(msgId);
            UIManager.DefaultAlert(transform, "已设置");

        });

        config.AddField("MsgId");

        UIManager.DefaultInputAlert(transform, config);

        Debug.Log("MarkMessageAsReadBtnAction");
    }
    void MarkAllMessageAsReadBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {

            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);
            conv.MarkAllMessageAsRead();
            UIManager.DefaultAlert(transform, "已设置");

        });

        UIManager.DefaultInputAlert(transform, config);

        Debug.Log("MarkAllMessageAsReadBtnAction");
    }
    void InsertMessageBtnAction()
    {
        UIManager.UnfinishedAlert(transform);
        Debug.Log("InsertMessageBtnAction");
    }
    void AppendMessageBtnAction()
    {
        UIManager.UnfinishedAlert(transform);
        Debug.Log("AppendMessageBtnAction");
    }
    void UpdateMessageBtnAction()
    {
        UIManager.UnfinishedAlert(transform);
        Debug.Log("UpdateMessageBtnAction");
    }
    void DeleteMessageBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
            string msgId = dict["MsgId"];
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);
            conv.DeleteMessage(msgId);
            UIManager.DefaultAlert(transform, "已删除");

        });

        config.AddField("MsgId");

        UIManager.DefaultInputAlert(transform, config);

        Debug.Log("DeleteMessageBtnAction");
    }
    void DeleteAllMessageBtnAction()
    {
        InputAlertConfig config = new InputAlertConfig((dict) =>
        {
            Conversation conv = SDKClient.Instance.ChatManager.GetConversation(conversationId, convType);
            conv.DeleteAllMessages();
            UIManager.DefaultAlert(transform, "已删除");

        });

        UIManager.DefaultInputAlert(transform, config);

        Debug.Log("DeleteAllMessageBtnAction");
    }
    void LoadMessageBtnAction()
    {
        Debug.Log("LoadMessageBtnAction");
    }
    void LoadMessagesBtnAction()
    {
        Debug.Log("LoadMessagesBtnAction");
    }
    void LoadMessagesWithKeywordBtnAction()
    {
        Debug.Log("LoadMessagesWithKeywordBtnAction");
    }
    void LoadMessagesWithTimeBtnAction()
    {
        Debug.Log("LoadMessagesWithTimeBtnAction");
    }
    void LoadMessagesWithMsgTypeBtnAction()
    {
        Debug.Log("LoadMessagesWithMsgTypeBtnAction");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
