using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace tetris
{
    class LogManage
    {
       public void StartLogOutput(string file_name)
       {
            this.FileName = file_name;
            this.sw = new StreamWriter(file_name, false,Encoding.GetEncoding("shift_jis"));
       }

       public void EndLogOutput()
       {
            if( sw != null)
            {
                this.sw.Dispose();
            }
        }

        /// <summary>
        /// 結果ログに書き込み
        /// </summary>
        /// <param name="generation"></param>
        /// <param name="learn_type"></param>
        /// <param name="average_score"></param>
        public void GAResultWrite(int generation,int learn_type,double average_score,double [] weight_array)
        {
            String WeightStr = @"";
            for (int i = 0; i < weight_array.Length; i++)
            {
                WeightStr += weight_array[i].ToString();
                WeightStr += ",";
            }

            WriteLine(generation.ToString() + "," +
                                learn_type.ToString() + "," +
                                average_score.ToString("0.000") + "," +
                                WeightStr);

        }

        public void WriteLine(String str)
        {
            if (sw != null)
            {
                sw.WriteLine(str);
            }
        }


        StreamWriter sw;
        String FileName = @"";
    }
}
