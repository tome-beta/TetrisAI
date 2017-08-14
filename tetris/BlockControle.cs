using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace tetris
{
    class BlockControle
    {
        public BlockControle()
        {
            MINO_TYPE_MAX = 8;

            this.blockInfo = new BlockInfo[MINO_TYPE_MAX];
            //ミノ情報を作る
            for (int i = 0; i < MINO_TYPE_MAX; i++)
            {
                BlockInfo.BlockType type = (BlockInfo.BlockType)i;
                this.blockInfo[i] = new BlockInfo(type);
            }
        }

        //ブロックを取得
        public void SetCurrentBlock(BlockInfo.BlockType type)
        {
            CurrentBlock = this.blockInfo[(int)type];

            //座標をスタート地点に
            CurrentPos.X = 3;
            CurrentPos.Y = 0;
            CurrentBlock.block_rot = BlockInfo.BlockRot.ROT_0;
        }

        public void RotateCurrentBlock(bool rot_r, int[,] field)
        {
            //回転した後の情報を仮でつくり、成立するかをチェックする
            BlockInfo.BlockRot tmp_rot = this.CurrentBlock.block_rot;
            BlockInfo.BlockType tmp_type = this.CurrentBlock.type;
            Point tmp_pos = new Point();
            Point delta_pos = new Point();
            tmp_pos = this.CurrentPos;

            const int SRS_ROT_NUM = 5;

            if (rot_r)
            {
                //右回転
                tmp_rot++;
                if (tmp_rot > BlockInfo.BlockRot.ROT_270)
                {
                    tmp_rot = BlockInfo.BlockRot.ROT_0;
                }

                //TODO SRSの判定が追加される
                for (int i = 0; i < SRS_ROT_NUM;i++)
                {
                    if (this.CurrentBlock.type == BlockInfo.BlockType.MINO_I)
                    {
                        delta_pos = CheckSRS(i, rot_r, this.CurrentBlock.block_rot);
                    }
                    else
                    {
                        delta_pos = CheckSRS_Imino(i, rot_r, this.CurrentBlock.block_rot);
                    }


                    if (CheckPlaceBlock(tmp_pos.X + delta_pos.X, tmp_pos.Y + delta_pos.Y, tmp_rot, tmp_type, field))
                    {
                        //回転できる
                        this.CurrentBlock.block_rot = tmp_rot;
                        this.CurrentPos.X += delta_pos.X;
                        this.CurrentPos.Y += delta_pos.Y;
                        break;
                    }
                }
            }
            else
            {
                //左回転
                tmp_rot--;
                if (tmp_rot < BlockInfo.BlockRot.ROT_0)
                {
                    tmp_rot = BlockInfo.BlockRot.ROT_270;
                }
                //TODO SRSの判定が追加される
                for (int i = 0; i < SRS_ROT_NUM; i++)
                {
                    if (this.CurrentBlock.type == BlockInfo.BlockType.MINO_I)
                    {
                        delta_pos = CheckSRS(i, rot_r, this.CurrentBlock.block_rot);
                    }
                    else
                    {
                        delta_pos = CheckSRS_Imino(i, rot_r, this.CurrentBlock.block_rot);
                    }

                    if (CheckPlaceBlock(tmp_pos.X + delta_pos.X, tmp_pos.Y + delta_pos.Y, tmp_rot, tmp_type, field))
                    {
                        //回転できる
                        this.CurrentBlock.block_rot = tmp_rot;
                        this.CurrentPos.X += delta_pos.X;
                        this.CurrentPos.Y += delta_pos.Y;
                        break;
                    }
                }
            }
        }

        //ブロックの右移動
        public void MoveCurrentBlockRight(int[,] field)
        {
            if (CheckPlaceBlock(CurrentPos.X + 1, CurrentPos.Y, this.CurrentBlock.block_rot, this.CurrentBlock.type, field))
            {
                CurrentPos.X++;
            }
        }
        //ブロックの左移動
        public void MoveCurrentBlockLeft(int[,] field)
        {
            if (CheckPlaceBlock(CurrentPos.X - 1, CurrentPos.Y, this.CurrentBlock.block_rot, this.CurrentBlock.type, field))
            {
                CurrentPos.X--;
            }
        }
        //ブロックの下移動
        public void MoveCurrentBlockDown(int[,] field)
        {
            if (CheckPlaceBlock(CurrentPos.X, CurrentPos.Y + 1, this.CurrentBlock.block_rot, this.CurrentBlock.type, field))
            {
                CurrentPos.Y++;
            }
        }

        //ハードドロップさせる
        public void HardDropCurrentBlock(int[,] field)
        {
            //ハードドロップさせるとどこまで落とせるかの座標を探す
            int y = 0;
            //ブロック設置可能場所までYを増加する
            while (CheckPlaceBlock(CurrentPos.X, CurrentPos.Y + y, this.CurrentBlock.block_rot, this.CurrentBlock.type, field))
            {
                y++;
            }
            //設置できるY位置
            y -= 1;
            //移動させる
            CurrentPos.Y += y;
        }

        //フィールドの操作しているブロックを設置する
        public void SetBlockInField(int[,] field)
        {
            int base_x = this.CurrentPos.X;
            int base_y = this.CurrentPos.Y;

            for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
            {
                for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                {
                    //フィールドに何もなくて、操作しているブロックはある所
                    if (CurrentBlock.shape[(int)CurrentBlock.block_rot, y, x] != 0 &&
                        field[base_y + y, base_x + x] == 0
                        )
                    {
                        field[base_y + y, base_x + x] = (int)CurrentBlock.type + (int)BlockInfo.BlockType.MINO_IN_FIELD;
                    }
                }
            }
        }

        //=====================================================
        // private 
        //=====================================================
        private Point CheckSRS_Imino(int cnt, bool right_rot, BlockInfo.BlockRot current_rot)
        {
            Point delta_pos = new Point();
            //右回転
            if (right_rot)
            {
                if (current_rot == BlockInfo.BlockRot.ROT_0)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -2;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = -2;
                            delta_pos.Y = -1;
                            break;
                        case 4:
                            delta_pos.X = 1;
                            delta_pos.Y = 2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_90)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 2;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = -1;
                            delta_pos.Y = 2;
                            break;
                        case 4:
                            delta_pos.X = 2;
                            delta_pos.Y =-1;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_180)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 2;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = 2;
                            delta_pos.Y = 1;
                            break;
                        case 4:
                            delta_pos.X = -1;
                            delta_pos.Y = -2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_270)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -2;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = 1;
                            delta_pos.Y = -2;
                            break;
                        case 4:
                            delta_pos.X = -2;
                            delta_pos.Y = 1;
                            break;
                        default:
                            break;
                    }
                }
            }
            //左回転
            else
            {
                if (current_rot == BlockInfo.BlockRot.ROT_0)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 2;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = -1;
                            delta_pos.Y = 2;
                            break;
                        case 4:
                            delta_pos.X = 2;
                            delta_pos.Y = -1;
                            break;
                        default:
                            break;
                    }
                }
                else if (current_rot == BlockInfo.BlockRot.ROT_90)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 2;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = 2;
                            delta_pos.Y = 1;
                            break;
                        case 4:
                            delta_pos.X = -1;
                            delta_pos.Y = -2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_180)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -2;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = 1;
                            delta_pos.Y = -2;
                            break;
                        case 4:
                            delta_pos.X = -2;
                            delta_pos.Y = 1;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_270)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -2;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 3:
                            delta_pos.X = -2;
                            delta_pos.Y = -1;
                            break;
                        case 4:
                            delta_pos.X = 1;
                            delta_pos.Y = 2;
                            break;
                        default:
                            break;
                    }
                }
            }

            return delta_pos;
        }

        private Point CheckSRS(int cnt,bool right_rot,BlockInfo.BlockRot current_rot)
        {
            Point delta_pos = new Point();
            //右回転
            if (right_rot)
            {
                if (current_rot == BlockInfo.BlockRot.ROT_0)
                {
                    switch(cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -1;
                            delta_pos.Y = 1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = -2;
                            break;
                        case 4:
                            delta_pos.X = -1;
                            delta_pos.Y = -2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_90)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 1;
                            delta_pos.Y = -1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = 2;
                            break;
                        case 4:
                            delta_pos.X = 1;
                            delta_pos.Y = 2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_180)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 1;
                            delta_pos.Y = 1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = -2;
                            break;
                        case 4:
                            delta_pos.X = 1;
                            delta_pos.Y = -2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_270)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -1;
                            delta_pos.Y = -1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = 2;
                            break;
                        case 4:
                            delta_pos.X = -1;
                            delta_pos.Y = 2;
                            break;
                        default:
                            break;
                    }


                }
            }
            //左回転
            else
            {
                if (current_rot == BlockInfo.BlockRot.ROT_0)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 1;
                            delta_pos.Y = 1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = -2;
                            break;
                        case 4:
                            delta_pos.X = 1;
                            delta_pos.Y = -2;
                            break;
                        default:
                            break;
                    }
                }
                else if (current_rot == BlockInfo.BlockRot.ROT_90)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = 1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = 1;
                            delta_pos.Y = -1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = 2;
                            break;
                        case 4:
                            delta_pos.X = 1;
                            delta_pos.Y = 2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_180)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -1;
                            delta_pos.Y = 1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = -2;
                            break;
                        case 4:
                            delta_pos.X = -1;
                            delta_pos.Y = -2;
                            break;
                        default:
                            break;
                    }

                }
                else if (current_rot == BlockInfo.BlockRot.ROT_270)
                {
                    switch (cnt)
                    {
                        case 0:
                            delta_pos.X = 0;
                            delta_pos.Y = 0;
                            break;
                        case 1:
                            delta_pos.X = -1;
                            delta_pos.Y = 0;
                            break;
                        case 2:
                            delta_pos.X = -1;
                            delta_pos.Y = -1;
                            break;
                        case 3:
                            delta_pos.X = 0;
                            delta_pos.Y = 2;
                            break;
                        case 4:
                            delta_pos.X = -1;
                            delta_pos.Y = 2;
                            break;
                        default:
                            break;
                    }
                }
            }

            return delta_pos;
        }


        /// <summary>
        /// ブロックのその場所に置けるのかを判定
        /// </summary>
        /// <param name="base_x">基準位置X</param>
        /// <param name="base_y">基準位置Y</param>
        /// <param name="rot">ブロックの方向</param>
        /// <param name="type">ブロックの種類</param>
        /// <param name="field">ゲームフィールド</param>
        /// <returns></returns>
        private bool CheckPlaceBlock(int base_x, int base_y, BlockInfo.BlockRot rot, BlockInfo.BlockType type, int[,] field)
        {
            //４＊４のブロック位置を探索
            for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
            {
                for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                {
                    //その座標はフィールドの中？
                    if (ValidFieldPos(base_x + x, base_y + y))
                    {
                        //他にブロックがある？
                        int block_cell = field[base_y + y, base_x + x];

                        //操作しているブロックと設置されているブロックが重なるとだめ
                        if (block_cell != 0 && this.CurrentBlock.shape[(int)rot, y, x] != 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //TODO 画面上の見えないけれど回転はできる場所の事を考える
                        if (base_y + y < 0)
                        {

                        }
                        else
                        {
                            //                            return true;
                        }
                    }

                }
            }
            return true;
        }

        //ゲームフィールドの有効な場所であるか
        private bool ValidFieldPos(int x, int y)
        {
            //TODO 壁はとりあえず考えない
            if (0 <= x && x < GameField.FIELD_WIDTH)
            {
                if (0 <= y && y < GameField.FIELD_HEIGHT) //出現位置の分までいれたら
                {
                    return true;
                }
            }
            return false;
        }

        //ブロックを取得 (つかわないかも）
        public BlockInfo GetBlock(BlockInfo.BlockType type)
        {
            return this.blockInfo[(int)type];
        }

        public readonly int MINO_TYPE_MAX;

        //操作中のブロック
        public BlockInfo CurrentBlock = null;
        public Point CurrentPos = new Point(3,0);   //操作中のブロックの基準点

        BlockInfo[] blockInfo;
    }
}
