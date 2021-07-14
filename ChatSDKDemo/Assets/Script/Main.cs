﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

using ChatSDK;

public class Main : MonoBehaviour
{
    // 接收消息id
    public InputField RecvIdField;
    // 输入内容
    public InputField TextField;
    // 发送按钮
    public Button SendBtn;
    // 群组id
    public InputField GroupField;
    // 加入群组按钮
    public Button JoinGroupBtn;
    // 获取群详情按钮
    public Button GroupInfoBtn;
    // 退出群组按钮
    public Button LeaveGroupBtn;
    // 聊天室id
    public InputField RoomField;
    // 加入聊天室按钮
    public Button JoinRoomBtn;
    // 获取聊天室按钮
    public Button RoomInfoBtn;
    // 退出聊天室按钮
    public Button LeaveRoomBtn;


    //public ScrollView scrollView;
    public ScrollRect scrollRect;

    IEnumerable<Toggle> ToggleGroup;

    GroupInfo currGroup;
    Conversation conversation;


    // Start is called before the first frame update
    void Start()
    {
        SendBtn.onClick.AddListener(SendMessageAction);

        JoinGroupBtn.onClick.AddListener(JoinGroupAction);
        GroupInfoBtn.onClick.AddListener(GetGroupInfoAction);
        LeaveGroupBtn.onClick.AddListener(LeaveGroupAction);

        JoinRoomBtn.onClick.AddListener(JoinRoomAction);
        RoomInfoBtn.onClick.AddListener(GetRoomInfoAction);
        LeaveRoomBtn.onClick.AddListener(LeaveRoomAction);

        ToggleGroup = GameObject.Find("ToggleGroup").GetComponent<ToggleGroup>().ActiveToggles();
        foreach (Toggle tog in ToggleGroup) {
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        Debug.Log("Quit and release resources...");
        SDKClient.Instance.Logout(false);
    }

    void SendMessageAction()
    {
        SendTextMessage();
    }

    void JoinGroupAction()
    {
        AppendConversationMessage();
    }

    void GetGroupInfoAction()
    {
        DeleteAllMessages();
    }

    void LeaveGroupAction()
    {
        UpdateConversationMessage();
    }

    void JoinRoomAction()
    {
        LoadConversationMessagesWithType();
    }

    void GetRoomInfoAction()
    {
        GetConversationExt();
    }

    void LeaveRoomAction()
    {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        List<Message> list = conv.LoadMessages(null);
        foreach (Message msg in list) {
            ChatSDK.MessageBody.TextBody textBody = (ChatSDK.MessageBody.TextBody)msg.Body;
            Debug.Log("message message: " + textBody.Text);
        }
    }


    // 发送文字消息
    void SendTextMessage() {

        Message msg = Message.CreateTextSendMessage("du003", "我是文字消息");

        CallBack callBack = new CallBack(

            onSuccess:()=> {
                Debug.Log("发送成功");
            },

            onError:(code, desc)=> {
                Debug.LogError("发送失败: " + code + "desc: " + desc);
            }

            );

        SDKClient.Instance.ChatManager.SendMessage(msg, callBack);
    }

    // 获取与xxx的会话
    void GetConversation() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Debug.Log("conversation id: " + conv.Id);
    }

    void GetUnreadCount() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Debug.Log("unread count --- " + conv.UnReadCount);
    }

    void GetLatestReceiveMessage() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Message msg = conv.LastReceivedMessage;
        if (msg != null)
        {
            if (msg.Body.Type == MessageBodyType.TXT)
            {
                ChatSDK.MessageBody.TextBody textBody = (ChatSDK.MessageBody.TextBody)msg.Body;
                Debug.Log("lataestReceive message: " + textBody.Text);
            }
        }
    }

    void GetLatestMessage() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Message msg = conv.LastMessage;
        if (msg != null)
        {
            if (msg.Body.Type == MessageBodyType.TXT)
            {
                ChatSDK.MessageBody.TextBody textBody = (ChatSDK.MessageBody.TextBody)msg.Body;
                Debug.Log("latest message: " + textBody.Text);
            }
        }
    }

    void MarkAllMessageAsRead() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        conv.MarkAllMessageAsRead();
    }

    void DeleteLastMessage() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Message msg = conv.LastMessage;
        conv.DeleteMessage(msg.MsgId);
    }

    void DeleteAllMessages() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        conv.DeleteAllMessages();
    }

    void LoadMessages() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        List<Message> list = conv.LoadMessages(null);
        Debug.Log("load messsage count --- " + list.Count);
    }

    void MakeAllConversationAsRead() {
        SDKClient.Instance.ChatManager.MarkAllConversationsAsRead();
    }

    void GetAllMessageUnReadCount() {
        int count = SDKClient.Instance.ChatManager.GetUnreadMessageCount();
        Debug.Log("all unread count --- " + count);
    }

    void InsertConversationMessage() {
        Message msg = Message.CreateTextSendMessage("du003", "我是会话插入的消息");
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        conv.InsertMessage(msg);
    }

    void UpdateChatMangerMessage(Message msg) {
        ChatSDK.MessageBody.TextBody textBody = new ChatSDK.MessageBody.TextBody("我是更新的消息");
        msg.Body = textBody;

        CallBack callback = new CallBack(
                onSuccess: () => {
                    Debug.Log("插入成功");
                },
                onError:(code, desc) => {
                    Debug.LogError("插入失败 --- " + code + " " + desc);
                }
            );

        SDKClient.Instance.ChatManager.UpdateMessage(msg, callback);
    }

    void AppendConversationMessage()
    {
        Message msg = Message.CreateTextSendMessage("du003", "我是会话Append的消息");
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        bool ret = conv.AppendMessage(msg);
        Debug.Log("插入 " + (ret ? "成功" : "失败") );

    }

    void UpdateConversationMessage() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Message msg = conv.LastMessage;
        ChatSDK.MessageBody.TextBody body = new ChatSDK.MessageBody.TextBody("我是Conversation更新的消息");
        msg.Body = body;

        conv.UpdateMessage(msg);
    }

    void InsertChatManagerMessage() {
        Message msg = Message.CreateTextSendMessage("du003", "我是Chat插入的消息");
        List<Message> list = new List<Message>();
        list.Add(msg);
        SDKClient.Instance.ChatManager.ImportMessages(list);
    }

    void RecallMessage(Message msg) {
        SDKClient.Instance.ChatManager.RecallMessage(msg.MsgId);
    }

    void SetConversationExt() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add("keyaaa", "valuebbb");
        conv.Ext = dict;
    }

    void GetConversationExt() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        foreach (string key in conv.Ext.Keys) {
            Debug.Log("ext --- " + conv.Ext[key]);
        }   
    }

    void LoadConversationMessagesWithKeyword(string str) {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        List<Message> msgs = conv.LoadMessagesWithKeyword(str, null);
        foreach (Message msg in msgs) {
            if (msg.Body.Type == MessageBodyType.TXT)
            {
                ChatSDK.MessageBody.TextBody textBody = (ChatSDK.MessageBody.TextBody)msg.Body;
                Debug.Log("search message: " + textBody.Text);
            }
        }
    }

    void LoadConversationMessagesWithType() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
        List<Message> msgs = conv.LoadMessagesWithMsgType(MessageBodyType.TXT, null);
        foreach (Message msg in msgs)
        {
            if (msg.Body.Type == MessageBodyType.TXT)
            {
                ChatSDK.MessageBody.TextBody textBody = (ChatSDK.MessageBody.TextBody)msg.Body;
                Debug.Log("type message: " + textBody.Text);
            }
        }
    }

    void LoadConversationMessageWithType() {
        Conversation conv = SDKClient.Instance.ChatManager.GetConversation("du003");
    }
}
