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

        //ブロックを取得
        public BlockInfo GetBlock(BlockInfo.BlockType type)
        {
            return this.blockInfo[(int)type];
        }

        public Image BlockSourceImage { get; }
        public readonly int MINO_TYPE_MAX;

        BlockInfo[] blockInfo;
    }
}
