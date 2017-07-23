using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace tetris
{
    //テトリミノの個体としての情報
    class BlockInfo
    {
        //テトリミノの種類
        public enum BlockType
        {
            MINO_ATTACK = 0,
            MINO_I,
            MINO_T,
            MINO_J,
            MINO_L,
            MINO_Z,
            MINO_S,
            MINO_O,
        };

        private int[,] BlockShape = new int[,]
        {
            //ATTACK
            {
                1,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
            },
            //I
            {
                0,1,0,0,
                0,1,0,0,
                0,1,0,0,
                0,1,0,0,
            },
            //T
            {
                0,1,0,0,
                1,1,1,0,
                0,0,0,0,
                0,0,0,0,
            },
            //J
            {
                1,0,0,0,
                1,1,1,0,
                0,0,0,0,
                0,0,0,0,
            },
            //L
            {
                0,0,1,0,
                1,1,1,0,
                0,0,0,0,
                0,0,0,0,
            },
            //Z
            {
                1,1,0,0,
                0,1,1,0,
                0,0,0,0,
                0,0,0,0,
            },
            //S
            {
                0,1,1,0,
                1,1,0,0,
                0,0,0,0,
                0,0,0,0,
            },
            //O
            {
                0,1,1,0,
                0,1,1,0,
                0,0,0,0,
                0,0,0,0,
            },
        };


        public BlockInfo()
        {
            //使わない
        }

        public BlockInfo(BlockType type)
        {
            this.type = type;

            shape = new int[16];

            for(int i = 0; i < 16; i++)
            {
                shape[i] = BlockShape[(int)type, i];
            }

        }

        //ミノを描く
        public void Draw(Graphics g, Image source_image)
        {
            //ミノの種類により切り出す画像を選ぶ
            int y_pos = (int)(this.type) * BLOCK_HEIGHT;
            Rectangle srcRect = new Rectangle(0, y_pos, BLOCK_WIDTH, BLOCK_HEIGHT);
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

            for(int i = 0; i < 16;i++)
            {
                if( shape[i] != 0)
                {
                    int x = i % 4;
                    int y = i / 4;
                    desRect.X = x * BLOCK_WIDTH;
                    desRect.Y = y * BLOCK_HEIGHT;
                    g.DrawImage(source_image, desRect, srcRect, GraphicsUnit.Pixel);
                }
            }

        }

        public BlockType type;
        private int[] shape;

        const int BLOCK_WIDTH = 30;
        const int BLOCK_HEIGHT = 30;
    }
}
