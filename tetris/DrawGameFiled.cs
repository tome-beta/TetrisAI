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


        /// <summary>
        /// ミノを描く
        /// </summary>
        /// <param name="player">プレイヤー番号</param>
        /// <param name="g">描画対象</param>
        /// <param name="x">基準のXセル位置</param>
        /// <param name="y">基準のYセル位置</param>
        /// <param name="type">ミノの種類</param>
        /// <param name="alpha">alphaを掛けるか</param>
        private void DrawOneMino(int player, Graphics g,BlockInfo info,int base_x,int base_y, int type,bool alpha = false,bool game_over = false)
        {
            for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
            {
                for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                {
                    if (info.shape[(int)this.blockControle[player].CurrentRot, y, x] != 0)
                    {
                        if (game_over)
                        {
                            type = (int)BlockInfo.BlockType.MINO_ATTACK;
                        }
                        //ミノの種類により切り出す画像を選ぶ
                        DrawOneBlock(g,
                            (base_x + x) * BlockInfo.BLOCK_WIDTH,
                            (base_y + y) * BlockInfo.BLOCK_HEIGHT,
                            type,
                            alpha);
                    }
                }
            }


        }


        //フィールドに置かれたブロックを描く
        private void DrawGameField(int player)
        {
            //フィールドのクリア
            gFiled[player].Clear(Color.White);

#if DEBUG
            //デバッグ用にフィールドに線を引いておく
            using (Pen pen = new Pen(Color.Gray))
            {
                for (int x = 1; x < 11; x++)
                {
                    gFiled[player].DrawLine(pen, new Point(x * BlockInfo.BLOCK_WIDTH, 0 * BlockInfo.BLOCK_HEIGHT),
                        new Point(x * BlockInfo.BLOCK_WIDTH, 20 * BlockInfo.BLOCK_HEIGHT));
                }
                for (int y = 1; y < 21; y++)
                {
                    gFiled[player].DrawLine(pen, new Point(0 * BlockInfo.BLOCK_WIDTH, y * BlockInfo.BLOCK_HEIGHT),
                        new Point(20 * BlockInfo.BLOCK_WIDTH, y * BlockInfo.BLOCK_HEIGHT));
                }
            }
#endif
            if(fieldManage[0] != null)
            {
                int[,] field = fieldManage[player].BlockField;
                //壁と設置されているブロックを描く
                for (int y = 0; y < FieldManage.FIELD_HEIGHT; y++)
                {
                    for (int x = 0; x < FieldManage.FIELD_WIDTH; x++)
                    {
                        if (field[y, x] >= (int)BlockInfo.BlockType.MINO_IN_FIELD)
                        {
                            DrawOneBlock(gFiled[player], (x) * BlockInfo.BLOCK_WIDTH, (y) * BlockInfo.BLOCK_HEIGHT, (field[y, x] % (int)BlockInfo.BlockType.MINO_IN_FIELD));
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 操作中のブロックを描画 
        /// </summary>
        /// <param name="player">プレイヤーターン</param>
        /// <param name="game_over"></param>
        private void DrawCurrentBlock(int player,bool game_over)
        {
            if (this.blockControle[player].CurrentBlock != null)
            {
                //名前おきかえ
                Point Pos = this.blockControle[player].CurrentPos;
                BlockInfo CurrnetInfo = this.blockControle[player].CurrentBlock;

                //ミノの描画
                DrawOneMino(player,
                    gFiled[player],
                    CurrnetInfo,
                    Pos.X,
                    Pos.Y,
                    (int)(CurrnetInfo.type), 
                    false,
                    game_over
                    );

#if DEBUG
                //デバッグ用にブロック領域の線を引いておく
                using (Pen pen = new Pen(Color.Pink))
                {
                    for (int x = 0; x < 5; x++)
                    {
                        gFiled[player].DrawLine(pen, new Point((Pos.X + x) * BlockInfo.BLOCK_WIDTH, (Pos.Y + 0) * BlockInfo.BLOCK_HEIGHT),
                            new Point((Pos.X + x) * BlockInfo.BLOCK_WIDTH, (Pos.Y + 4) * BlockInfo.BLOCK_HEIGHT));
                    }
                    for (int y = 0; y < 5; y++)
                    {
                        gFiled[player].DrawLine(pen, new Point((Pos.X + 0) * BlockInfo.BLOCK_WIDTH, (Pos.Y + y) * BlockInfo.BLOCK_HEIGHT),
                            new Point((Pos.X + 4) * BlockInfo.BLOCK_WIDTH, (Pos.Y + y) * BlockInfo.BLOCK_HEIGHT));
                    }
                }
#endif
            }
        }

        //落下位置ガイドブロックを描画
        private void DrawGuideBlock(int player)
        {
            int[,] field = this.fieldManage[player].BlockField;

            if (this.blockControle[player].CurrentBlock != null)
            {
                int move_y = this.blockControle[player].CheckHardDropCurrentBlock(field);

                //名前おきかえ
                Point Pos = this.blockControle[player].CurrentPos;
                BlockInfo CurrnetInfo = this.blockControle[player].CurrentBlock;

                //ミノの描画
                DrawOneMino(player,
                    gFiled[player],
                    CurrnetInfo,
                    Pos.X,
                    Pos.Y + move_y,
                    (int)(CurrnetInfo.type), true);
            }
        }

        //NEXTブロックの描画
        private void DrawNextBlock(int player)
        {
            //表示位置のクリア
            gNextBlock[player].Clear(Color.White);

            List<int> next = this.nextManage[player].NextBlock;

            //ミノの種類により切り出す画像を選ぶ
            for (int next_num = 0; next_num < NextBlockManage.NEXT_BLOCK_DISP_NUM; next_num++)
            {
                BlockInfo info = new BlockInfo((BlockInfo.BlockType)(next[next_num]));

                DrawOneMino(player, 
                    gNextBlock[player],
                    info,
                    0,
                    next_num * 3, 
                    (int)(next[next_num]),
                    false);
            }
        }

        //HOLD中のブロックを描く
        private void DrawHoldBlock(int player)
        {
            //表示位置のクリア
            gHoldBlock[player].Clear(Color.White);

            if (BlockInfo.BlockType.MINO_I <= blockControle[player].HoldBlock && blockControle[player].HoldBlock <= BlockInfo.BlockType.MINO_O)
            {
                BlockInfo info = new BlockInfo((BlockInfo.BlockType)(blockControle[player].HoldBlock));

                DrawOneMino(player,
                    gHoldBlock[player],
                    info,
                    0,
                    0, 
                    (int)(blockControle[player].HoldBlock),
                    false);
            }
        }

        //スコアの表示
        private void DrawScore(int player)
        {
            const int p1 = (int)PLAYER_DEFINE.PLAYER_1;
            const int p2 = (int)PLAYER_DEFINE.PLAYER_2;

            if(player == (int)PLAYER_DEFINE.PLAYER_1)
            {
                this.label1Line1P.Text = @"1Line : " + this.scoreManage[p1].EraseCount[0];
                this.label2Line1P.Text = @"2Line : " + this.scoreManage[p1].EraseCount[1];
                this.label3Line1P.Text = @"3Line : " + this.scoreManage[p1].EraseCount[2];
                this.label4Line1P.Text = @"4Line : " + this.scoreManage[p1].EraseCount[3];

                this.labelT1Count1P.Text = @"Tspin1 : " + this.scoreManage[p1].TspinEraseCount[0];
                this.labelT2Count1P.Text = @"Tspin2 : " + this.scoreManage[p1].TspinEraseCount[1];
                this.labelT3Count1P.Text = @"Tspin3 : " + this.scoreManage[p1].TspinEraseCount[2];
            }
            else
            {
                this.label1Line2P.Text = @"1Line : " + this.scoreManage[p2].EraseCount[0];
                this.label2Line2P.Text = @"2Line : " + this.scoreManage[p2].EraseCount[1];
                this.label3Line2P.Text = @"3Line : " + this.scoreManage[p2].EraseCount[2];
                this.label4Line2P.Text = @"4Line : " + this.scoreManage[p2].EraseCount[3];

                this.labelT1Count2P.Text = @"Tspin1 : " + this.scoreManage[p2].TspinEraseCount[0];
                this.labelT2Count2P.Text = @"Tspin2 : " + this.scoreManage[p2].TspinEraseCount[1];
                this.labelT3Count2P.Text = @"Tspin3 : " + this.scoreManage[p2].TspinEraseCount[2];
            }

        }


        //攻撃ラインの表示
        private void DrawAttackLine(int player)
        {
            //実際は相手側の攻撃ライン数をチェックする
            int aite = player == (int)PLAYER_DEFINE.PLAYER_1 ? (int)PLAYER_DEFINE.PLAYER_2 : (int)PLAYER_DEFINE.PLAYER_1;

            int attack_line = this.attackLineManage[aite].AttackLineNum;

            gAttackLine[player].Clear(Color.White);

            int h = canvasAttackLine[player].Height - BlockInfo.BLOCK_HEIGHT * attack_line;
            gAttackLine[player].FillRectangle(Brushes.Red,
                0,
                h,
                canvasAttackLine[player].Width,
                canvasAttackLine[player].Height - h);
        }
    }
}
