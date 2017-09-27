using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    //ゲームフィールドを管理します。
    class FieldManage
    {
        public const int FIELD_HEIGHT = 20 + 1;     //ミノ領域＋床
        public const int FIELD_WIDTH = 10 + 2;      //ミノ領域 + 壁＊２

        public FieldManage()
        {
            this.BlockField = new int[FieldManage.FIELD_HEIGHT, FieldManage.FIELD_WIDTH];
            ClearField();
        }

        /// <summary>
        /// フィールドの初期化
        /// </summary>
        public void ClearField()
        {
            //フィールドを作る
            //フィールドは１０＊２０の両サイドに壁を表す９９を入れる。
            //ブロックのスタート位置のために上に３行加える。
            //床にも１行追加
            //全体としては１２＊２4
            //壁と床を設置
            for (int w = 0; w < FieldManage.FIELD_WIDTH; w++)
            {
                for (int h = 0; h < FieldManage.FIELD_HEIGHT; h++)
                {
                    if (w == 0 || w == FieldManage.FIELD_WIDTH - 1 ||
                        h == FieldManage.FIELD_HEIGHT - 1)
                    {
                        this.BlockField[h, w] = (int)BlockInfo.BlockType.MINO_FENCE + (int)BlockInfo.BlockType.MINO_IN_FIELD;
                    }
                    else
                    {
                        this.BlockField[h, w] = 0;
                    }
                }
            }

        }



        public int[,] BlockField { get; set; }
 

    }
}
