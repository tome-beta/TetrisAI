﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    //ゲームフィールドを管理します。
    [Serializable]
    class FieldManage
    {
        public const int FIELD_HEIGHT = 20 + 1;     //ミノ領域＋床
        public const int FIELD_WIDTH = 10 + 2;      //ミノ領域 + 壁＊２

        public FieldManage()
        {
            this.BlockField = new int[FieldManage.FIELD_HEIGHT, FieldManage.FIELD_WIDTH];
            this.EraseLine = new List<int>();
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

        /// <summary>
        /// パーフェクトの確認
        /// </summary>
        /// <param name="erase_line_num">消したライン数</param>
        public bool CheckPerfect(int erase_line_num)
        {
            bool ok = false;

            //パーフェクトチェック
            //床から見ていく
            int perfect_count = 0;
            const int PERFECT_LINE_CHECK = 5;
            for (int h = FieldManage.FIELD_HEIGHT - 2; h > FieldManage.FIELD_HEIGHT - 2 - PERFECT_LINE_CHECK; h--)
            {
                bool line_check = true;
                //壁の所は見ない
                for (int w = 1; w < FieldManage.FIELD_WIDTH - 2; w++)
                {
                    //消す予定になっているor何もない
                    int block_data = BlockField[h, w];
                    if (block_data >= (int)BlockInfo.BlockType.MINO_VANISH ||
                        block_data == 0)
                    {

                    }
                    else
                    {
                        line_check = false;
                        break;
                    }
                }
                if (line_check)
                {
                    perfect_count++;
                }
            }

            //消したライン数と床から探索して消す予定ライン数が一致していたらパーフェクト
            if (perfect_count == PERFECT_LINE_CHECK)
            {
                ok = true;
            }

            return ok;
        }

        //消えるライン数をチェック
        public int CheckEraseLine()
        {
            int line_num = 0;
            //床から見ていく
            for (int h = FieldManage.FIELD_HEIGHT - 2; h >= 0; h--)
            {
                bool erase_line = true;
                //壁の所は見ない
                for (int w = 1; w < FieldManage.FIELD_WIDTH - 1; w++)
                {
                    //設置されていないか
                    if (BlockField[h, w] < (int)BlockInfo.BlockType.MINO_IN_FIELD)
                    {
                        //空きがあれば飛ばす
                        erase_line = false;
                        break;
                    }
                }

                //消すラインを予約する
                if (erase_line)
                {
                    //消す予定の情報を加える
                    for (int w = 1; w < FieldManage.FIELD_WIDTH - 1; w++)
                    {
                        BlockField[h, w] += (int)(BlockInfo.BlockType.MINO_VANISH);
                    }
                    //消す
                    line_num++;
                    this.EraseLine.Add(h);
                }
            }

            return line_num;

        }

        //消去するラインを調べる
        public void ExecEraseLine()
        {
            int vanish_line = 0;
            bool vanish = false;
            //ブロックを実際に消す処理
            //アニメーションをそのうちつける
            for (int h = 0; h < FieldManage.FIELD_HEIGHT; h++)
            {
                vanish = false;
                //壁の所は見ない
                for (int w = 1; w < FieldManage.FIELD_WIDTH - 1; w++)
                {
                    if (BlockField[h, w] >= (int)BlockInfo.BlockType.MINO_VANISH)
                    {
                        vanish = true;
                        BlockField[h, w] = 0;
                        for (int h2 = h; h2 > 0; h2--)
                        {
                            BlockField[h2, w] = BlockField[h2 - 1, w];
                        }
                    }
                }

                if(vanish)
                {
                    vanish_line++;
                }

            }


            //消したラインのぶんだけ空のラインを作る
            for(int i = 0; i < vanish_line; i++)
            {
                for (int w = 1; w < FieldManage.FIELD_WIDTH - 1; w++)
                {
                    BlockField[i, w] = 0;
                }
            }

            this.EraseLine.Clear();
        }

        /// <summary>
        /// フィールドをゲームオーバーの状態にする
        /// </summary>
        public void GameOverField()
        {
            //置いているブロックをすべて灰色にする
            //壁と設置されているブロックを描く
            for (int y = 0; y < FieldManage.FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < FieldManage.FIELD_WIDTH; x++)
                {
                    int field_block = BlockField[y, x] % (int)BlockInfo.BlockType.MINO_IN_FIELD;

                    if ((int)BlockInfo.BlockType.MINO_I <= field_block &&
                        field_block <= (int)BlockInfo.BlockType.MINO_O)
                    {
                        BlockField[y, x] = (int)BlockInfo.BlockType.MINO_ATTACK + (int)BlockInfo.BlockType.MINO_IN_FIELD;
                    }
                }
            }
        }


        public int[,] BlockField { get; set; }
        public List<int> EraseLine { get; set; }
    }
}
