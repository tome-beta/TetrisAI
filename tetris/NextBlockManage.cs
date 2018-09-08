using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    [Serializable()]
    class NextBlockManage
    {
        public const int NEXT_BLOCK_MAX = 14;       //７種類＊２で表示は５個。
        public const int NEXT_BLOCK_DISP_NUM = 5;   //表示は５個。

        public NextBlockManage()
        {
            NextBlock = new List<int>();
        }

        //NEXTブロックを初期化する
        public void InitNextBlock()
        {
            this.NextBlock.Clear();
            UpdateNextBlock();

        }

        //NEXTブロックを取得
        public int GetNextBlock()
        {
            //一つ取り出す
            int type = NextBlock[0];
            NextBlock.RemoveAt(0);
            return type;
        }

        /// <summary>
        /// nextブロックの更新
        /// </summary>
        public void UpdateNextBlock()
        {
            //NEXTブロックの数をカウントする
            int count = this.NextBlock.Count();

            if (count <= NEXT_BLOCK_MAX)
            {
                //追加で７つのブロックを選び出す。
                //１から７の入った配列をランダムでシャッフルして追加する
                int[] array = { 1, 2, 3, 4, 5, 6, 7 };

                //Fisher–Yatesアルゴリズム
                for (int i = array.Length - 1; i > 0; i--)
                {
                    int a = i - 1;
                    int b = Common.MyRandom.Next(array.Length) % i;
                    var tmp = array[a];
                    array[a] = array[b];
                    array[b] = tmp;
                }

                foreach (int a in array)
                {
                    this.NextBlock.Add(a);
                }
            }
        }

        public List<int> NextBlock;
    }
}
