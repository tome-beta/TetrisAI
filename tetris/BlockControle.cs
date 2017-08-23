using System.Drawing;
using System.Diagnostics;
using System;

namespace tetris
{
    class BlockControle
    {
        //コンストラクタ   
        public BlockControle()
        {
            MINO_TYPE_MAX = 8;
            MINO_START_POS = new Point(3,0);

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
            CurrentBlock.type = type;

            //配列のコピー
            Array.Copy(this.blockInfo[(int)type].shape, CurrentBlock.shape, this.blockInfo[(int)type].shape.Length);

            CurrentBlock.SRS_dx = this.blockInfo[(int)type].SRS_dx;
            CurrentBlock.SRS_dy = this.blockInfo[(int)type].SRS_dy;

            //座標をスタート地点に
            this.CurrentPos = MINO_START_POS;
            this.CurrentRot = BlockInfo.BlockRot.ROT_0;
        }

        //操作しているブロックを回転させる
        public void RotateCurrentBlock(bool rot_r, int[,] field)
        {
            //回転した後の情報を仮でつくり、成立するかをチェックする
            BlockInfo.BlockRot tmp_rot = this.CurrentRot;
            BlockInfo.BlockType tmp_type = this.CurrentBlock.type;
            Point tmp_pos = new Point();
            Point delta_pos = new Point();
            tmp_pos = this.CurrentPos;

            if (rot_r)
            {
                //右回転
                tmp_rot++;
                if (tmp_rot > BlockInfo.BlockRot.ROT_270)
                {
                    tmp_rot = BlockInfo.BlockRot.ROT_0;
                }

                //TODO SRSの判定が追加される
                for (int i = 0; i < BlockInfo.SRS_ROT_NUM;i++)
                {
                    delta_pos = CheckSRS(i, rot_r, this.CurrentRot);

                    if (CheckPlaceBlock(tmp_pos.X + delta_pos.X, tmp_pos.Y + delta_pos.Y, tmp_rot, tmp_type, field))
                    {
                        //回転できる
                        this.CurrentRot = tmp_rot;
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
                for (int i = 0; i < BlockInfo.SRS_ROT_NUM; i++)
                {
                    delta_pos = CheckSRS(i, rot_r, this.CurrentRot);
                    if (CheckPlaceBlock(tmp_pos.X + delta_pos.X, tmp_pos.Y + delta_pos.Y, tmp_rot, tmp_type, field))
                    {
                        //回転できる
                        this.CurrentRot = tmp_rot;
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
            if (CheckPlaceBlock(CurrentPos.X + 1, CurrentPos.Y, this.CurrentRot, this.CurrentBlock.type, field))
            {
                CurrentPos.X++;
            }
        }
        //ブロックの左移動
        public void MoveCurrentBlockLeft(int[,] field)
        {
            if (CheckPlaceBlock(CurrentPos.X - 1, CurrentPos.Y, this.CurrentRot, this.CurrentBlock.type, field))
            {
                CurrentPos.X--;
            }
        }
        //ブロックの下移動
        public void MoveCurrentBlockDown(int[,] field)
        {
            if (CheckPlaceBlock(CurrentPos.X, CurrentPos.Y + 1, this.CurrentRot, this.CurrentBlock.type, field))
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
            while (CheckPlaceBlock(CurrentPos.X, CurrentPos.Y + y, this.CurrentRot, this.CurrentBlock.type, field))
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
                    if (CurrentBlock.shape[(int)CurrentRot, y, x] != 0 &&
                        field[base_y + y, base_x + x] == 0
                        )
                    {
                        field[base_y + y, base_x + x] = (int)CurrentBlock.type + (int)BlockInfo.BlockType.MINO_IN_FIELD;
                    }
                }
            }
        }

        /// <summary>
        //  HOLD機能
        /// </summary>
        /// <returns>既に保持しているブロックがあればtrue 無ければfalse</returns>
        public bool UpdateHold()
        {
            bool ret = false;

            //HOLDしているブロックがあれば交換
            if(BlockInfo.BlockType.MINO_I <= HoldBlock && HoldBlock <= BlockInfo.BlockType.MINO_O)
            {
                //CurrentBlockとHoldBlockを交換する
                BlockInfo.BlockType tmp_type = HoldBlock;
                HoldBlock = CurrentBlock.type;
                CurrentBlock.type = tmp_type;

                SetCurrentBlock((BlockInfo.BlockType)CurrentBlock.type);
                ret = true;
            }
            else
            {
                //操作しているブロックをHOLDするだけ
                HoldBlock = CurrentBlock.type;
            }

            //位置を初期化
            CurrentPos = MINO_START_POS;

            DoHold = true;

            return ret;
        }

        //ゲームオーバーかをチェックする
        public bool CheckGameOver(int[,] field)
        {
            if( CheckPlaceBlock(this.CurrentPos.X, this.CurrentPos.Y, this.CurrentRot, this.CurrentBlock.type,field) )
            {
                //おける
                return false;
            }
            else
            {
                //おけない
                return true;
            }
        }

        //=====================================================
        // private 
        //=====================================================
        /// <summary>
        /// SRSの回転法則をチェック
        /// </summary>
        /// <param name="SRS_rule">SRSの回転ルール</param>
        /// <param name="right_rot">右回転であるか</param>
        /// <param name="current_rot">現在の方向</param>
        /// <returns></returns>
        private Point CheckSRS(int SRS_rule,bool right_rot,BlockInfo.BlockRot current_rot)
        {
            Point delta_pos = new Point();

            int rot_num = 1;
            if(right_rot)
            {
                rot_num = 0;
            }

            delta_pos.X = this.CurrentBlock.SRS_dx[rot_num, (int)current_rot, SRS_rule];
            delta_pos.Y = this.CurrentBlock.SRS_dy[rot_num, (int)current_rot, SRS_rule];

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

        public readonly int MINO_TYPE_MAX;      //ミノとしての種類
        public readonly Point MINO_START_POS;   //ミノを出現させる位置


        //操作中のブロック
        public BlockInfo CurrentBlock = new BlockInfo(BlockInfo.BlockType.MINO_I);
        public BlockInfo.BlockType HoldBlock = BlockInfo.BlockType.MINO_VANISH;
        public Point CurrentPos = new Point(3,0);   //操作中のブロックの基準点
        public BlockInfo.BlockRot CurrentRot { get; set; }

        public bool DoHold = false;        //HOLDを実行したかどうか
        BlockInfo[] blockInfo;
    }
}
