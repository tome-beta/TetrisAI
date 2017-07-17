using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    //テトリミノの情報
    class BlockInfo
    {
        //テトリミノの種類
        enum BlockType
        {
            MINO_ATTACK = 0,
            MINO_I,
            MINO_T,
            MINO_J,
            MINO_L,
            MINO_Z,
            MINO_S,
            MINO_O
        };

        const int BLOCK_WIDTH = 30;
        const int BLOCK_HEIGHT = 30;
        //メンバ変数
        BlockType type = BlockType.MINO_ATTACK;


    }
}
