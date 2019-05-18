using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace InfiniteKnightsSaveEditor
{
    using RawData = Dictionary<string, Dictionary<string, object>>;

    public class SaveData
    {
        private RawData _data;

        private SaveData(RawData data)
        {
            _data = data;
        }

        /// <summary>钻石数量</summary>
        public int Gem
        {
            get { return (int)_data["playerData"]["gem"]; }
            set { _data["playerData"]["gem"] = value; }
        }

        /// <summary>免广告券数量</summary>
        public int AdsTicket
        {
            get { return (int)_data["playerData"]["AdsTicketNum"]; }
            set { _data["playerData"]["AdsTicketNum"] = value; }
        }

        /// <summary>次元灵石数量</summary>
        public BigInteger ElfStone
        {
            get { return BigInteger.Parse(_data["playerData"]["n_elfStone"] as string); }
            set { _data["playerData"]["n_elfStone"] = value.ToString(); }
        }

        /// <summary>金币数量</summary>
        public BigInteger Coin
        {
            get { return BigInteger.Parse(_data["playerData"]["n_coin"] as string); }
            set { _data["playerData"]["n_coin"] = value.ToString(); }
        }

        /// <summary>VIP订阅状态</summary>
        public bool SubscribeState
        {
            get { return Convert.ToBoolean(_data["settings"]["isSubscibe"]); }
            set { _data["settings"]["isSubscibe"] = Convert.ToInt32(value); }
        }

        /// <summary>VIP购买时间</summary>
        public DateTime SubscribeBuyTime
        {
            get
            {
                var str = _data["settings"]["SubscribeBuyTime"] as string;
                return DateTime.ParseExact(str, "yyyyMMdd", CultureInfo.CurrentCulture);
            }
            set {
                _data["settings"]["SubscribeBuyTime"] = value.ToString("yyyyMMdd");
            }
        }

        public void SetVip(bool status, DateTime? buyTime = null)
        {
            if (status == true)
            {
                SubscribeState = true;
                SubscribeBuyTime = buyTime ?? DateTime.UtcNow;
            }
            else
            {
                SubscribeState = false;
                SubscribeBuyTime = buyTime ?? DateTimeOffset.FromUnixTimeSeconds(0).DateTime;
            }
        }

        public void Save(string filePath)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                (new BinaryFormatter()).Serialize(memoryStream, _data);
                File.WriteAllBytes(filePath, memoryStream.ToArray());
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(_data, Formatting.Indented);
        }

        public static SaveData Load(string filePath)
        {
            var buffer = File.ReadAllBytes(filePath);
            using (var memoryStream = new MemoryStream(buffer))
            {
                var data = (new BinaryFormatter()).Deserialize(memoryStream) as RawData;
                return new SaveData(data);
            }
        }
    }
}
