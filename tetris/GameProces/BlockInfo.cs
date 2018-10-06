using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace tetris
{
    //テトリミノの個体としての情報
    [Serializable()]
    class BlockInfo
    {
        public const int BLOCK_WIDTH = 30;     //ブロック画像の幅
        public const int BLOCK_HEIGHT = 30;    //ブロック画像の高さ

        public const int BLOCK_CELL_WIDTH = 4;     //ブロック領域の幅
        public const int BLOCK_CELL_HEIGHT = 4;    //ブロック領域の高さ

        //SRS回転法則の数
        public const int SRS_ROT_NUM = 5;

        //テトリミノの種類
        public enum BlockType
        {
            MINO_NULL = 0,
            MINO_I,
            MINO_T,
            MINO_J,
            MINO_L,
            MINO_Z,
            MINO_S,
            MINO_O,
            MINO_FENCE,
            MINO_ATTACK,

            MINO_IN_FIELD = 10, //設置したブロックを表す
            MINO_VANISH = 100,  //消す予定のブロックを表す
        };

        public enum BlockRot
        {
            ROT_0,
            ROT_90,
            ROT_180,
            ROT_270,

            ROT_TYPE_NUM,//計算用
        };

        //ここにずらずらとミノの情報を取得する関数を書いていくのか
        //必要なの　ブロックの形＊４
        //ミノID
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

            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_Imino_dx;
            this.SRS_dy = this.SRS_Imino_dy;
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
            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_General_dx;
            this.SRS_dy = this.SRS_General_dy;
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
            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_General_dx;
            this.SRS_dy = this.SRS_General_dy;
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
            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_General_dx;
            this.SRS_dy = this.SRS_General_dy;
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
            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_General_dx;
            this.SRS_dy = this.SRS_General_dy;
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
            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_General_dx;
            this.SRS_dy = this.SRS_General_dy;
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
            //回転法則（SRS）を設定
            this.SRS_dx = this.SRS_General_dx;
            this.SRS_dy = this.SRS_General_dy;
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

        //持つのは形と回転法則だけっぽい。ミノを作るごとに回転法則を当てはめる
        public int[,,] SRS_dx = new int[2,4,5];//[回転方向（０＝右、１＝左][回転前の向き][回転ルール番号]
        public int[,,] SRS_dy = new int[2, 4, 5];//[回転方向（０＝右、１＝左][回転前の向き][回転ルール番号]

        public BlockType type;  //ミノの種類
        public int[,,] shape { get; set; }    //ブロックの形＊４


        //回転法則作成
        private readonly int[,,] SRS_General_dx = new int[2, 4, SRS_ROT_NUM]//[回転方向（０＝右、１＝左][回転前の向き][回転ルール番号]
        {
            //右回転
            {
               { 0,-1,-1,0,-1 },//ROT_0
               { 0, 1, 1,0, 1 },//ROT_90
               { 0, 1, 1,0, 1 },//ROT_180
               { 0,-1,-1,0,-1 },//ROT_270
            },
            //左回転
            {
               { 0, 1, 1,0, 1 },//ROT_0
               { 0, 1, 1,0, 1 },//ROT_90
               { 0,-1,-1,0,-1 },//ROT_180
               { 0,-1,-1,0,-1 },//ROT_270
            },
        };
        private readonly int[,,] SRS_General_dy = new int[2, 4, SRS_ROT_NUM]//[回転方向（０＝右、１＝左][回転前の向き][回転ルール番号]
        {
            //右回転
            {
               { 0, 0,-1, 2, 2 },//ROT_0
               { 0, 0, 1,-2,-2 },//ROT_90
               { 0, 0,-1, 2, 2 },//ROT_180
               { 0, 0, 1,-2,-2 },//ROT_270
            },
            //左回転
            {
               { 0, 0,-1, 2, 2 },//ROT_0
               { 0, 0, 1,-2,-2 },//ROT_90
               { 0, 0,-1, 2, 2 },//ROT_180
               { 0, 0, 1,-2,-2 },//ROT_270
            },
        };

        //Iミノ用の回転法則
        private readonly int[,,] SRS_Imino_dx = new int[2, 4, SRS_ROT_NUM]//[回転方向（０＝右、１＝左][回転前の向き][回転ルール番号]
        {
            //右回転
            {
               { 0,-2, 1,-2, 1 },//ROT_0
               { 0,-1, 2,-1, 2 },//ROT_90
               { 0, 2,-1, 2,-1 },//ROT_180
               { 0, 1,-2, 1,-2 },//ROT_270
            },
            //左回転
            {
               { 0,-1, 2,-1, 2 },//ROT_0
               { 0, 2,-1, 2,-1 },//ROT_90
               { 0, 1,-2, 1,-2 },//ROT_180
               { 0,-2, 1,-2, 1 },//ROT_270
            },
        };
        private readonly int[,,] SRS_Imino_dy = new int[2, 4, SRS_ROT_NUM]//[回転方向（０＝右、１＝左][回転前の向き][回転ルール番号]
        {
            //右回転
            {
               { 0, 0, 0, 1,-2 },//ROT_0
               { 0, 0, 0,-2, 1 },//ROT_90
               { 0, 0, 0,-1, 2 },//ROT_180
               { 0, 0, 0, 2,-1 },//ROT_270
            },
            //左回転
            {
               { 0, 0, 0,-2, 1 },//ROT_0
               { 0, 0, 0,-1, 2 },//ROT_90
               { 0, 0, 0, 2,-1 },//ROT_180
               { 0, 0, 0, 1,-2 },//ROT_270
            },
        };


    }
}
