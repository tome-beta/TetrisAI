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

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        if (CurrentBlock.shape[(int)CurrentBlock.block_rot, y, x] != 0)
                        {
                            desRect.X = (CurrentPos.X  + x) * BlockInfo.BLOCK_WIDTH;
                            desRect.Y = (CurrentPos.Y + y) * BlockInfo.BLOCK_HEIGHT;
                            g.DrawImage(source_image, desRect, srcRect, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
        }

        //ブロックを取得
        public void SetCurrentBlock(BlockInfo.BlockType type)
        {
            CurrentBlock = this.blockInfo[(int)type];
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
        public Point CurrentPos = new Point(0,0);   //操作中のブロックの基準点

        BlockInfo[] blockInfo;
    }
}
