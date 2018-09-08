using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace tetris
{
    static class Common
    {
        public static class DeepCopyHelper
        {
            public static T DeepCopy<T>(T target)
            {

                T result;
                BinaryFormatter b = new BinaryFormatter();

                MemoryStream mem = new MemoryStream();

                try
                {
                    b.Serialize(mem, target);
                    mem.Position = 0;
                    result = (T)b.Deserialize(mem);
                }
                finally
                {
                    mem.Close();
                }

                return result;

            }
        }
        //データ配列
        public static System.Random MyRandom = new Random();
    }
}
