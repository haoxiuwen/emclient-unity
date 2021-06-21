using UnityEngine;
using System;

namespace ChatSDK
{
    class Client_Mac : IClient
    {
        private static ConnectionHub connectionHub;

        internal IntPtr client = IntPtr.Zero;
        private string currentUserName;
        private bool isLoggedIn;
        private bool isConnected;

        //events
        public event Action OnLoginSuccess;
        public event OnError OnLoginError;
        public event Action OnRegistrationSuccess;
        public event OnError OnRegistrationError;
        public event Action OnLogoutSuccess;

        public Client_Mac() {
            // start log service
            StartLog("/tmp/unmanaged_dll.log");
        }

        public override void CreateAccount(string username, string password, CallBack callback = null)
        {
            if (client != IntPtr.Zero)
            {
                OnRegistrationSuccess = () => callback?.Success();
                OnRegistrationError = (int code, string desc) => callback?.Error(code, desc);
                ChatAPINative.Client_CreateAccount(client, OnRegistrationSuccess, OnRegistrationError, username, password);
            }
            else
            {
                Debug.LogError("::InitWithOptions() not called yet.");
            }
        }

        public override void InitWithOptions(Options options, WeakDelegater<IConnectionDelegate> listeners = null)
        {

            ChatCallbackObject.GetInstance();
            if(connectionHub == null)
            {
                connectionHub = new ConnectionHub(listeners); //init only once
            }
            
            // keep only 1 client left
            if(client != IntPtr.Zero)
            {
                //stop log service
                StopLog();
                ChatAPINative.Client_Release(client);
            }
            StartLog("/tmp/unmanaged_dll.log");
            client = ChatAPINative.Client_InitWithOptions(options, connectionHub.OnConnected, connectionHub.OnDisconnected, connectionHub.OnPong);
        }

        public override void Login(string username, string pwdOrToken, bool isToken = false, CallBack callback = null)
        {
            if (client != IntPtr.Zero) {
                OnLoginSuccess = () =>
                {
                    currentUserName = username;
                    isLoggedIn = true;
                    callback?.Success();
                };
                OnLoginError = (int code, string desc) => callback?.Error(code, desc);

                ChatAPINative.Client_Login(client, OnLoginSuccess, OnLoginError, username, pwdOrToken, isToken);
            } else {
                Debug.LogError("::InitWithOptions() not called yet.");
            }
        }

        public override void Logout(bool unbindDeviceToken, CallBack callback = null)
        {
            if (client != IntPtr.Zero)
            {
                OnLogoutSuccess = () =>
                {
                    currentUserName = "";
                    isLoggedIn = false;
                    callback?.Success();
                };
                ChatAPINative.Client_Logout(client, OnLogoutSuccess, unbindDeviceToken);
            } else {
                Debug.LogError("::InitWithOptions() not called yet.");
            }
        }

        public override string CurrentUsername()
        {
            return currentUserName;
        }

        public override bool IsConnected()
        {
            return isConnected;
        }

        public override bool IsLoggedIn()
        {
            return isLoggedIn;
        }

        public override string AccessToken()
        {
            throw new System.NotImplementedException();
        }

        public override void StartLog(string logFilePath)
        {
            ChatAPINative.Client_StartLog(logFilePath);
        }

        public override void StopLog()
        {
            ChatAPINative.Client_StopLog();
        }
    }

}
