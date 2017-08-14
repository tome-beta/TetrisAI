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
        private void DrawOneBlock(Graphics g,int x,int y,int type)
        {
            int y_pos = type * BlockInfo.BLOCK_HEIGHT;
            Rectangle srcRect = new Rectangle(0, y_pos, BlockInfo.BLOCK_WIDTH, BlockInfo.BLOCK_HEIGHT);
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

            desRect.X = x;
            desRect.Y = y;
            g.DrawImage(BlockSourceImage, desRect, srcRect, GraphicsUnit.Pixel);
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
        private void DrawCurrentBlock()
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
                        if (CurrnetInfo.shape[(int)CurrnetInfo.block_rot, y, x] != 0)
                        {
                            //ミノの種類により切り出す画像を選ぶ
                            DrawOneBlock(gFiled1P, 
                                (Pos.X + x) * BlockInfo.BLOCK_WIDTH, 
                                (Pos.Y + y) * BlockInfo.BLOCK_HEIGHT,
                                (int)(CurrnetInfo.type));
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
                        if (info.shape[(int)info.block_rot, y, x] != 0)
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

    }
}
