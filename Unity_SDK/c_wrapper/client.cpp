#include "client.h"

#include "emchatconfigs.h"
#include "emchatprivateconfigs.h"
#include "emclient.h"
#include "emchatmanager_interface.h"

using namespace easemob;

extern "C"
{
#define CLIENT static_cast<EMClient *>(client)
#define CALLBACK static_cast<Callback *>(callback)
}

static bool G_DEBUG_MODE = false;
static bool G_AUTO_LOGIN = true;

AGORA_API void Client_CreateAccount(void *client, FUNC_OnSuccess onSuccess, FUNC_OnError onError, const char *username, const char *password)
{
    LOG("Client_CreateAccount() called with username=%s password=%s", username, password);
    EMErrorPtr result = CLIENT->createAccount(username, password);
    
    if(EMError::isNoError(result)) {
        LOG("Account creation succeeds!");
        if(onSuccess) onSuccess();
    }else{
        LOG("Account creation failed!");
        if(onError) onError(result->mErrorCode, result->mDescription.c_str());
    }
}

EMChatConfigsPtr ConfigsFromOptions(Options *options) {
    const char *appKey = options->AppKey;
    LOG("Client_InitWithOptions() called with AppKey=%s", appKey);
    EMChatConfigsPtr configs = EMChatConfigsPtr(new EMChatConfigs("","",appKey,0));
    //TODO: non null-ptr assertion
    const char *dnsURL = options->DNSURL;
    const char *imServer = options->IMServer;
    const char *restServer = options->RestServer;
    int imPort = options->IMPort;
    bool enableDNSConfig = options->EnableDNSConfig;
    LOG("Options with DNSURL=%s, IMServer=%s, IMPort=%d, RestServer=%s, EnableDNSConfig=%s", dnsURL, imServer, imPort, restServer, enableDNSConfig ? "true" : "false");
    configs->setDnsURL(options->DNSURL);
    configs->privateConfigs().chatServer() = options->IMServer;
    configs->privateConfigs().restServer() = options->RestServer;
    configs->privateConfigs().chatPort() = options->IMPort;
    configs->privateConfigs().enableDns() = options->EnableDNSConfig;
    configs->setAutoAcceptFriend(options->AcceptInvitationAlways);
    configs->setAutoAcceptGroup(options->AutoAcceptGroupInvitation);
    configs->setRequireReadAck(options->RequireAck);
    configs->setRequireDeliveryAck(options->RequireDeliveryAck);
    configs->setDeleteMessageAsExitGroup(options->DeleteMessagesAsExitGroup);
    configs->setDeleteMessageAsExitChatRoom(options->DeleteMessagesAsExitRoom);
    configs->setIsChatroomOwnerLeaveAllowed(options->IsRoomOwnerLeaveAllowed);
    configs->setSortMessageByServerTime(options->SortMessageByServerTime);
    configs->setUsingHttps(options->UsingHttpsOnly);
    configs->setTransferAttachments(options->ServerTransfer);
    configs->setAutoDownloadThumbnail(options->IsAutoDownload);
    return configs;
}

EMClient *gClient = NULL;
ConnectionListener *gConnectionListener = NULL;

AGORA_API void* Client_InitWithOptions(Options *options, FUNC_OnConnected onConnected, FUNC_OnDisconnected onDisconnected, FUNC_OnPong onPong)
{
    // global switch
    G_DEBUG_MODE = options->DebugMode;
    G_AUTO_LOGIN = options->AutoLogin;
    // singleton client handle
    if(gClient == nullptr) {
        EMChatConfigsPtr configs = ConfigsFromOptions(options);
        gClient = EMClient::create(configs);
        gConnectionListener = new ConnectionListener(onConnected, onDisconnected, onPong);
        gClient->addConnectionListener(gConnectionListener);
    }
    
    return gClient;
}

AGORA_API void Client_Login(void *client, FUNC_OnSuccess onSuccess, FUNC_OnError onError, const char *username, const char *pwdOrToken, bool isToken)
{
    LOG("Client_Login() called with username=%s, pwdOrToken=%s, isToken=%d", username, pwdOrToken, isToken);
    EMErrorPtr result;
    result = isToken ? CLIENT->loginWithToken(username, pwdOrToken) : CLIENT->login(username, pwdOrToken);
    if(EMError::isNoError(result)) {
        LOG("Login succeeds.");
        if(onSuccess) onSuccess();
    }else{
        LOG("Login failed with code=%d desc=%s!", result->mErrorCode, result->mDescription.c_str());
        if(onError) onError(result->mErrorCode, result->mDescription.c_str());
    }
}

AGORA_API void Client_Logout(void *client, FUNC_OnSuccess onSuccess, bool unbindDeviceToken)
{
    CLIENT->logout();
    if(onSuccess) onSuccess();
}

AGORA_API void Client_StartLog(const char *logFilePath) {
    LogHelper::getInstance().startLogService(logFilePath);
}

AGORA_API void Client_StopLog() {
    return LogHelper::getInstance().stopLogService();
}

EMCallbackObserverHandle gCallbackObserverHandle;

AGORA_API void ChatManager_SendMessage(void *client, FUNC_OnSuccess onSuccess, FUNC_OnError onError, MessageTransferObject *mto) {
    EMMessagePtr messagePtr = mto->toEMMessage();
    EMCallbackPtr callbackPtr(new EMCallback(gCallbackObserverHandle,
                                             [onSuccess]()->bool {
                                                LOG("Message sent succeeds.");
                                                if(onSuccess) onSuccess();
                                                return true;
                                             },
                                             [onError](const easemob::EMErrorPtr error)->bool{
                                                LOG("Message sent failed with code=%d.", error->mErrorCode);
                                                if(onError) onError(error->mErrorCode,error->mDescription.c_str());
                                                return true;
                                             }));
    messagePtr->setCallback(callbackPtr);
    CLIENT->getChatManager().sendMessage(messagePtr);
}
