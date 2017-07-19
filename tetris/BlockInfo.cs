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

        public BlockInfo()
        {

        }

        public BlockInfo(BlockType type)
        {
            this.type = type;
        }

        //ミノを描く
        public void Draw(Graphics g, Image source_image)
        {
            //ミノの種類により切り出す画像を選ぶ
            int y_pos = (int)(this.type) * BLOCK_HEIGHT;


            Rectangle srcRect = new Rectangle(0, y_pos, BLOCK_WIDTH, BLOCK_HEIGHT);
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            g.DrawImage(source_image, desRect, srcRect, GraphicsUnit.Pixel);

        }

        public BlockType type;

        const int BLOCK_WIDTH = 30;
        const int BLOCK_HEIGHT = 30;
    }
}
