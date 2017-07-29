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
        public const int BLOCK_WIDTH = 30;     //ブロック画像の幅
        public const int BLOCK_HEIGHT = 30;    //ブロック画像の高さ

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

        public enum BlockRot
        {
            ROT_0,
            ROT_90,
            ROT_180,
            ROT_270,
        };

        //ここにずらずらとミノの情報を取得する関数を書いていくのか
        //必要なの　ブロックの形＊４
        //ミノID
        //今のミノの向き
        //回転法則

        private void SetMinoInfo_I()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 0,0,0,0, },
                    { 1,1,1,1, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,0,1,0, },
                    { 0,0,1,0, },
                    { 0,0,1,0, },
                    { 0,0,1,0, },
                },
                {
                    //ROT_180
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                    { 1,1,1,1, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 0,1,0,0, },
                    { 0,1,0,0, },
                    { 0,1,0,0, },
                    { 0,1,0,0, },
                },
            };

            this.type = BlockType.MINO_I;
            this.block_rot = BlockRot.ROT_0;

        }

        private void SetMinoInfo_T()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 0,1,0,0, },
                    { 1,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,1,0,0, },
                    { 0,1,1,0, },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_180
                    { 0,0,0,0, },
                    { 1,1,1,0, },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 0,1,0,0, },
                    { 1,1,0,0, },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
            };
            this.type = BlockType.MINO_T;
            this.block_rot = BlockRot.ROT_0;
        }

        private void SetMinoInfo_J()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 1,0,0,0, },
                    { 1,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,1,1,0, },
                    { 0,1,0,0, },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_180
                    { 1,1,1,0, },
                    { 0,0,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 0,1,0,0, },
                    { 0,1,0,0  },
                    { 1,1,0,0, },
                    { 0,0,0,0, },
                },
            };
            this.type = BlockType.MINO_J;
            this.block_rot = BlockRot.ROT_0;
        }

        private void SetMinoInfo_L()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 0,0,1,0, },
                    { 1,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,1,0,0, },
                    { 0,1,0,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_180
                    { 1,1,1,0, },
                    { 1,0,0,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 1,1,0,0, },
                    { 0,1,0,0  },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
            };
            this.type = BlockType.MINO_L;
            this.block_rot = BlockRot.ROT_0;
        }

        private void SetMinoInfo_Z()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 1,1,0,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,0,1,0, },
                    { 0,1,1,0, },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_180
                    { 0,0,0,0, },
                    { 1,1,0,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 0,1,0,0, },
                    { 1,1,0,0  },
                    { 1,0,0,0, },
                    { 0,0,0,0, },
                },
            };
            this.type = BlockType.MINO_Z;
            this.block_rot = BlockRot.ROT_0;
        }

        private void SetMinoInfo_S()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 0,1,1,0, },
                    { 1,1,0,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,1,0,0, },
                    { 0,1,1,0, },
                    { 0,0,1,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_180
                    { 0,0,0,0, },
                    { 0,1,1,0, },
                    { 1,1,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 1,0,0,0, },
                    { 1,1,0,0  },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
            };
            this.type = BlockType.MINO_S;
            this.block_rot = BlockRot.ROT_0;
        }

        private void SetMinoInfo_O()
        {
            //ミノの位置
            this.shape = new int[,,]
            {
                {
                    //ROT_0
                    { 0,1,1,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_90
                    { 0,1,1,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_180
                    { 0,1,1,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                {
                    //ROT_270
                    { 0,1,1,0, },
                    { 0,1,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
            };
            this.type = BlockType.MINO_O;
            this.block_rot = BlockRot.ROT_0;
        }

        public BlockInfo()
        {
            //使わない
        }

        public BlockInfo(BlockType type)
        {
            switch (type)
            {
                case BlockType.MINO_I:
                    {
                      SetMinoInfo_I();
                    }
                    break;
                case BlockType.MINO_T:
                    {
                        SetMinoInfo_T();
                    }
                    break;
                case BlockType.MINO_J:
                    {
                        SetMinoInfo_J();
                    }
                    break;
                case BlockType.MINO_L:
                    {
                        SetMinoInfo_L();
                    }
                    break;
                case BlockType.MINO_Z:
                    {
                        SetMinoInfo_Z();
                    }
                    break;
                case BlockType.MINO_S:
                    {
                        SetMinoInfo_S();
                    }
                    break;
                case BlockType.MINO_O:
                    {
                        SetMinoInfo_O();
                    }
                    break;

            }

        }



        public BlockType type;  //ミノの種類
        public int[,,] shape { get; set; }    //ブロックの形＊４
        public BlockRot block_rot { get; set; }
        //作成予定 回転法則

    }
}
