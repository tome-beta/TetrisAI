using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    //プレー記録を残す
    class ScoreManage
    {
        public ScoreManage()
        {

        }

        //1試合の記録を消去
        public void ScoreClear()
        {
            for(int i = 0; i < 3;i++)
            {
                this.TspinEraseCount[i] = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                this.EraseCount[i] = 0;
            }
            TotalEraseLine = 0;
        }

        /// <summary>
        /// 消したラインを設定
        /// </summary>
        /// <param name="line_num"></param>
        /// <param name="tspin"></param>
        public void SetEraseLine(int line_num,bool tspin)
        {
            if(tspin)
            {
                TspinEraseCount[line_num-1]++;
            }
            else
            {
                EraseCount[line_num-1]++;
            }

            TotalEraseLine += line_num;
        }

        public int[] TspinEraseCount = new int[3];
        public int[] EraseCount = new int[4];
        public int TotalEraseLine = 0;
    }

}
