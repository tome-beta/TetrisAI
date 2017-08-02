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
            //ブロックの元画像を読み込んでおく
            BlockSourceImage = Image.FromFile(@"..\..\resource\mino.bmp");
            MINO_TYPE_MAX = 8;

            this.blockInfo = new BlockInfo[MINO_TYPE_MAX];
            //ミノ情報を作る
            for (int i = 0; i < MINO_TYPE_MAX; i++)
            {
                BlockInfo.BlockType type = (BlockInfo.BlockType)i;
                this.blockInfo[i] = new BlockInfo(type);
            }
        }

        //操作中のブロックの描画をここに
        public void DrawCurrentBlock(Graphics g, Image source_image)
        {
            if(CurrentBlock != null)
            {
                //ミノの種類により切り出す画像を選ぶ
                int y_pos = (int)(CurrentBlock.type) * BlockInfo.BLOCK_HEIGHT;
                Rectangle srcRect = new Rectangle(0, y_pos, BlockInfo.BLOCK_WIDTH, BlockInfo.BLOCK_HEIGHT);
                Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

                for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
                {
                    for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                    {
                        if (CurrentBlock.shape[(int)CurrentBlock.block_rot, y, x] != 0)
                        {
                            desRect.X = (CurrentPos.X  + x) * BlockInfo.BLOCK_WIDTH;
                            desRect.Y = (CurrentPos.Y + y) * BlockInfo.BLOCK_HEIGHT;
                            g.DrawImage(source_image, desRect, srcRect, GraphicsUnit.Pixel);
                        }
                    }
                }

                //デバッグ用にブロック領域の線を引いておく
                using (Pen pen = new Pen(Color.Pink))
                {
                    for(int x = 0; x < 5; x++)
                    {
                        g.DrawLine(pen, new Point((CurrentPos.X + x) * BlockInfo.BLOCK_WIDTH, (CurrentPos.Y + 0) * BlockInfo.BLOCK_HEIGHT),
                            new Point((CurrentPos.X + x) * BlockInfo.BLOCK_WIDTH, (CurrentPos.Y + 4) * BlockInfo.BLOCK_HEIGHT));
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        g.DrawLine(pen, new Point((CurrentPos.X + 0) * BlockInfo.BLOCK_WIDTH, (CurrentPos.Y + y) * BlockInfo.BLOCK_HEIGHT),
                            new Point((CurrentPos.X + 4) * BlockInfo.BLOCK_WIDTH, (CurrentPos.Y + y) * BlockInfo.BLOCK_HEIGHT));
                    }
                }

            }
        }

        //ブロックを取得
        public void SetCurrentBlock(BlockInfo.BlockType type)
        {
            CurrentBlock = this.blockInfo[(int)type];
        }

        public void RotateCurrentBlock(bool rot_r)
        {
            if(rot_r)
            {
                //右回転
                CurrentBlock.block_rot++;
                if(CurrentBlock.block_rot > BlockInfo.BlockRot.ROT_270)
                {
                    CurrentBlock.block_rot = BlockInfo.BlockRot.ROT_0;
                }
            }
            else
            {
                //左回転
                CurrentBlock.block_rot--;
                if (CurrentBlock.block_rot < BlockInfo.BlockRot.ROT_0)
                {
                    CurrentBlock.block_rot = BlockInfo.BlockRot.ROT_270;
                }
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

        //=====================================================
        // private 
        //=====================================================

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
            if ( 0 < x && x < GameField.FIELD_WIDTH)
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

        public Image BlockSourceImage { get; }
        public readonly int MINO_TYPE_MAX;

        //操作中のブロック
        private BlockInfo CurrentBlock = null;
        public Point CurrentPos = new Point(3,0);   //操作中のブロックの基準点

        BlockInfo[] blockInfo;
    }
}
