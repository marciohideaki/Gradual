using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using StackExchange.Redis;
using ProtoBuf;
using System.Configuration;

namespace Gradual.OMS.RedisUtilities
{
    public class RedisUtils
    {
        public static readonly log4net.ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static RedisUtils _me = null;
        private ConnectionMultiplexer redis = null;

        public static RedisUtils Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new RedisUtils();
                }

                return _me;
            }
        }

        public RedisUtils()
        {
            if ( ConfigurationManager.AppSettings["RedisServer"] == null )
                throw new ConfigurationErrorsException("Chave 'RedisServer' deve ser declarada em app.settings");

            redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisServer"].ToString());
        }


        public bool SetRedis<T>(string key, string attribute,  T objeto)
        {
            try
            {
                IDatabase redisDB = redis.GetDatabase();

                List<HashEntry> lstHashes = new List<HashEntry>();

                MemoryStream xxx = new MemoryStream();
                Serializer.Serialize<T>(xxx, objeto);

                byte[] arr = xxx.ToArray();

                lstHashes.Add(new HashEntry(attribute, arr));

                redisDB.HashSet(key, lstHashes.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("SetRedis: " + ex.Message, ex);
            }

            return false;
        }

        public bool SetRedisAsync<T>(string key, string attribute, T objeto)
        {
            try
            {
                IDatabase redisDB = redis.GetDatabase();

                List<HashEntry> lstHashes = new List<HashEntry>();

                MemoryStream xxx = new MemoryStream();
                Serializer.Serialize<T>(xxx, objeto);

                byte[] arr = xxx.ToArray();

                lstHashes.Add(new HashEntry(attribute, arr));

                redisDB.HashSetAsync(key, lstHashes.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("SetRedis: " + ex.Message, ex);
            }

            return false;
        }

        public T GetRedis<T>(string key, string attribute )
        {
            try
            {
                IDatabase redisDB = redis.GetDatabase();
                T objeto;

                if (redisDB.HashExists(key, attribute))
                {
                    byte[] arr = redisDB.HashGet(key, attribute);
                    
                    MemoryStream xxx = new MemoryStream(arr);

                    objeto = Serializer.Deserialize<T>(xxx);

                    return objeto;
                }

            }
            catch (Exception ex)
            {
                logger.Error("GetRedis: " + ex.Message, ex);
            }

            return default(T);
        }
    }
}
