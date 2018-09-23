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
            this.sw.Dispose();
       }

        public void WriteLine(String str)
        {
            sw.WriteLine(str);
        }


        StreamWriter sw;
        String FileName = @"";
    }
}
