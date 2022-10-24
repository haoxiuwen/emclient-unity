﻿using System.Collections.Generic;
using AgoraChat.SimpleJSON;
namespace AgoraChat
{
    /**
     * \~chinese
     * 在线状态属性类，包含发布者的用户名、在线设备使用的平台、当前在线状态以及在线状态的扩展信息、更新时间和到期时间。
     *
     * \~english
     * The presence property class that contains presence properties, including the publisher's user ID and current presence state, and the platform used by the online device, as well as the presence's extension information, update time, and subscription expiration time.
     */
    public class Presence: BaseModel
    {

        /**
         * \~chinese
         * 获取在线状态发布者的用户 ID。
         *
         * @return 在线状态发布者的用户 ID。
         * \~english
         * Gets the user ID of the presence publisher.
         *
         * @return The user ID of the presence publisher.
         */

        public string Publisher { get; internal set; }

        /**
         * \~chinese
         * 获取当前在线状态的详情。当前在线状态详情由键值对组成，其中 key 表示在线状态发布者使用的设备平台，如“ios”、“android”、“linux”、“windows”或“webbim”，value 表示发布者的当前在线状态。
         *
         * @return 当前在线状态详情。
         *
         *\~english
         * Gets the details of the current presence state. The presence state details are a key-value structure, where the key can be "ios", "android", "linux", "windows", or "webim", and the value is the current presence state of the publisher.
         *
         * @return The details of the current presence state.
         */

        public List<PresenceDeviceStatus> StatusList { get; internal set; }


        /**
         * \~chinese
         * 获取状态描述信息。
         *
         * @return 状态描述信息。
         *
         * \~english
         * Gets the presence status description information.
         *
         * @return The presence status description information.
         */

        public string statusDescription { get; internal set; }

        /**
         *  \~chinese
         * 获取在线状态更新时间。
         *
         * @return 在线状态更新 Unix 时间戳，单位为秒。
         *
         *  \~english
         * Gets the presence update time.
         *
         *  @return The Unix timestamp when the presence state is updated. The unit is second.
         */
        public long LatestTime { get; internal set; }

        /**
         *  \~chinese
         * 获取在线状态订阅到期时间。
         *
         * @return  在线状态订阅到期的 Unix 时间戳，单位为秒。
         *
         *  \~english
         * Gets the expiration time of the presence subscription.
         *
         * @return  The Unix timestamp when the presence subscription expires. The unit is second.
         */

        public long ExpiryTime { get; internal set; }

        internal Presence() { }

        internal Presence(string jsonString):base(jsonString) { }

        internal Presence(JSONObject jsonObject):base(jsonObject) { }

        internal override void FromJsonObject(JSONObject jo)
        {
            if(null != jo)
            {
                Publisher = jo["publisher"].Value;
                statusDescription = jo["statusDescription"].Value;
                LatestTime = jo["lastTime"].AsInt;
                ExpiryTime = jo["expiryTime"].AsInt;
                StatusList = new List<PresenceDeviceStatus>();
                if (jo["statusDetails"].IsArray)
                {
                    JSONArray array = jo["statusDetails"].AsArray;
                    foreach (JSONObject it in array)
                    {
                        StatusList.Add(new PresenceDeviceStatus(it));
                    }
                }
            }
        }

        internal override JSONObject ToJsonObject()
        {
            return null;
        }
    }
}