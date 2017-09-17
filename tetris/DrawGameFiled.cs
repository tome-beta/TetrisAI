using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace tetris
{
    public partial class GameField : Form
    {
        //指定した座標にブロックを描く
        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="type"></param>
        /// <param name="alpha">true で半透明</param>
        private void DrawOneBlock(Graphics g,int x,int y,int type,bool alpha = false)
        {
            int y_pos = type * BlockInfo.BLOCK_HEIGHT;
            Rectangle srcRect = new Rectangle(0, y_pos, BlockInfo.BLOCK_WIDTH, BlockInfo.BLOCK_HEIGHT);
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

            desRect.X = x;
            desRect.Y = y;

            if (alpha)
            {
                //ColorMatrixオブジェクトの作成
                System.Drawing.Imaging.ColorMatrix cm =
                    new System.Drawing.Imaging.ColorMatrix();
                //ColorMatrixの行列の値を変更して、アルファ値が0.5に変更されるようにする
                cm.Matrix00 = 1;
                cm.Matrix11 = 1;
                cm.Matrix22 = 1;
                cm.Matrix33 = 0.5F;
                cm.Matrix44 = 1;

                //ImageAttributesオブジェクトの作成
                System.Drawing.Imaging.ImageAttributes ia =
                    new System.Drawing.Imaging.ImageAttributes();
                //ColorMatrixを設定する
                ia.SetColorMatrix(cm);

                g.DrawImage(BlockSourceImage, desRect, srcRect.X,srcRect.Y,srcRect.Width,srcRect.Height, GraphicsUnit.Pixel,ia);
            }
            else
            {
                g.DrawImage(BlockSourceImage, desRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        //フィールドに置かれたブロックを描く
        private void DrawGameField()
        {
            //フィールドのクリア
            gFiled1P.Clear(Color.White);

#if DEBUG
            //デバッグ用にフィールドに線を引いておく
            using (Pen pen = new Pen(Color.Gray))
            {
                for (int x = 1; x < 11; x++)
                {
                    gFiled1P.DrawLine(pen, new Point(x * BlockInfo.BLOCK_WIDTH, 0 * BlockInfo.BLOCK_HEIGHT),
                        new Point(x * BlockInfo.BLOCK_WIDTH, 20 * BlockInfo.BLOCK_HEIGHT));
                }
                for (int y = 1; y < 21; y++)
                {
                    gFiled1P.DrawLine(pen, new Point(0 * BlockInfo.BLOCK_WIDTH, y * BlockInfo.BLOCK_HEIGHT),
                        new Point(20 * BlockInfo.BLOCK_WIDTH, y * BlockInfo.BLOCK_HEIGHT));
                }
            }
#endif
            //壁と設置されているブロックを描く
            for (int y = 0; y < GameField.FIELD_HEIGHT; y++)
            {
                for (int x = 0; x < GameField.FIELD_WIDTH; x++)
                {
                    if (this.BlockField[y, x] >= (int)BlockInfo.BlockType.MINO_IN_FIELD)
                    {
                        DrawOneBlock(gFiled1P, (x) * BlockInfo.BLOCK_WIDTH, (y) * BlockInfo.BLOCK_HEIGHT, (this.BlockField[y, x] % (int)BlockInfo.BlockType.MINO_IN_FIELD));
                    }
                }
            }
        }

        //操作中のブロックを描画
        private void DrawCurrentBlock(bool game_over)
        {
            if (this.blockControle.CurrentBlock != null)
            {
                //名前おきかえ
                Point Pos = this.blockControle.CurrentPos;
                BlockInfo CurrnetInfo = this.blockControle.CurrentBlock;

                for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
                {
                    for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                    {
                        if (CurrnetInfo.shape[(int)this.blockControle.CurrentRot, y, x] != 0)
                        {
                            int draw_type = (int)(CurrnetInfo.type);
                            if( game_over)
                            {
                                draw_type = (int)BlockInfo.BlockType.MINO_ATTACK;
                            }


                            //ミノの種類により切り出す画像を選ぶ
                            DrawOneBlock(gFiled1P, 
                                (Pos.X + x) * BlockInfo.BLOCK_WIDTH, 
                                (Pos.Y + y) * BlockInfo.BLOCK_HEIGHT,
                                draw_type);
                        }
                    }
                }
#if DEBUG
                //デバッグ用にブロック領域の線を引いておく
                using (Pen pen = new Pen(Color.Pink))
                {
                    for (int x = 0; x < 5; x++)
                    {
                        gFiled1P.DrawLine(pen, new Point((Pos.X + x) * BlockInfo.BLOCK_WIDTH, (Pos.Y + 0) * BlockInfo.BLOCK_HEIGHT),
                            new Point((Pos.X + x) * BlockInfo.BLOCK_WIDTH, (Pos.Y + 4) * BlockInfo.BLOCK_HEIGHT));
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        gFiled1P.DrawLine(pen, new Point((Pos.X + 0) * BlockInfo.BLOCK_WIDTH, (Pos.Y + y) * BlockInfo.BLOCK_HEIGHT),
                            new Point((Pos.X + 4) * BlockInfo.BLOCK_WIDTH, (Pos.Y + y) * BlockInfo.BLOCK_HEIGHT));
                    }
                }
#endif
            }
        }

        //落下位置ガイドブロックを描画
        private void DrawGuideBlock()
        {
            if (this.blockControle.CurrentBlock != null)
            {
                int move_y = this.blockControle.HardDropCurrentBlock(this.BlockField);

                //名前おきかえ
                Point Pos = this.blockControle.CurrentPos;
                BlockInfo CurrnetInfo = this.blockControle.CurrentBlock;

                for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
                {
                    for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                    {
                        if (CurrnetInfo.shape[(int)this.blockControle.CurrentRot, y, x] != 0)
                        {
                            //ミノの種類により切り出す画像を選ぶ
                            DrawOneBlock(gFiled1P,
                                (Pos.X + x) * BlockInfo.BLOCK_WIDTH,
                                (Pos.Y + move_y + y) * BlockInfo.BLOCK_HEIGHT,
                                (int)(CurrnetInfo.type),
                                true);
                        }
                    }
                }
            }

            }

        //NEXTブロックの描画
        private void DrawNextBlock()
        {
            //表示位置のクリア
            gNextBlock1P.Clear(Color.White);

            //ミノの種類により切り出す画像を選ぶ
            for (int next_num = 0; next_num < NEXT_BLOCK_DISP_NUM; next_num++)
            {
                BlockInfo info = new BlockInfo((BlockInfo.BlockType)(NextBlock[next_num]));
                for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
                {
                    for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                    {
                        if (info.shape[(int)BlockInfo.BlockRot.ROT_0, y, x] != 0)
                        {
                            //ミノの種類により切り出す画像を選ぶ
                            DrawOneBlock(gNextBlock1P,
                                (x) * BlockInfo.BLOCK_WIDTH,
                                (next_num * 3 + y) * BlockInfo.BLOCK_HEIGHT,
                                (int)(NextBlock[next_num]));
                        }
                    }
                }
            }
        }

        //HOLD中のブロックを描く
        private void DrawHoldBlock()
        {
            //表示位置のクリア
            gHoldBlock1P.Clear(Color.White);

            if (BlockInfo.BlockType.MINO_I <= blockControle.HoldBlock && blockControle.HoldBlock <= BlockInfo.BlockType.MINO_O)
            {
                BlockInfo info = new BlockInfo((BlockInfo.BlockType)(blockControle.HoldBlock));
                for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
                {
                    for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                    {
                        if (info.shape[(int)BlockInfo.BlockRot.ROT_0, y, x] != 0)
                        {
                            //ミノの種類により切り出す画像を選ぶ
                            DrawOneBlock(gHoldBlock1P,
                                (x) * BlockInfo.BLOCK_WIDTH,
                                (y) * BlockInfo.BLOCK_HEIGHT,
                                (int)(blockControle.HoldBlock));
                        }
                    }
                }
            }
        }

        //スコアの表示
        private void DrawScore()
        {
            this.label1Line1P.Text = @"1Line : " + this.scoreManage.EraseCount[0];
            this.label2Line1P.Text = @"2Line : " + this.scoreManage.EraseCount[1];
            this.label3Line1P.Text = @"3Line : " + this.scoreManage.EraseCount[2];
            this.label4Line1P.Text = @"4Line : " + this.scoreManage.EraseCount[3];

            this.labelT1Count1P.Text = @"Tspin1 : " + this.scoreManage.TspinEraseCount[0];
            this.labelT2Count1P.Text = @"Tspin2 : " + this.scoreManage.TspinEraseCount[1];
            this.labelT3Count1P.Text = @"Tspin3 : " + this.scoreManage.TspinEraseCount[2];
        }


        //攻撃ラインの表示
        private void DrawAttackLine()
        {
            //TODO　実際は相手側の攻撃ライン数をチェックする
            int attack_line = this.attackLineManage.AttackLineNum;

            gAttackLine1P.Clear(Color.White);

            int h = canvasAttackLine1P.Height - BlockInfo.BLOCK_HEIGHT * attack_line;
            gAttackLine1P.FillRectangle(Brushes.Red,
                0,
                h,
                canvasAttackLine1P.Width,
                canvasAttackLine1P.Height - h);
        }
    }
}
