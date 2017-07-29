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
            this.shape2 = new int[,,]
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
                //ROT_180
                {
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                    { 1,1,1,1, },
                    { 0,0,0,0, },
                },
                //ROT_270
                {
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
            this.shape2 = new int[,,]
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
                //ROT_180
                {
                    { 0,0,0,0, },
                    { 1,1,1,0, },
                    { 0,1,0,0, },
                    { 0,0,0,0, },
                },
                //ROT_270
                {
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
            this.shape2 = new int[,,]
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
                //ROT_180
                {
                    { 1,1,1,0, },
                    { 0,0,1,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                //ROT_270
                {
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
            this.shape2 = new int[,,]
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
                //ROT_180
                {
                    { 1,1,1,0, },
                    { 1,0,0,0, },
                    { 0,0,0,0, },
                    { 0,0,0,0, },
                },
                //ROT_270
                {
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
            this.shape2 = new int[,,]
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
            this.shape2 = new int[,,]
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
            this.shape2 = new int[,,]
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
            //            this.shape2 = new int[4, 4, 4]; //回転パターン、x座標、ｙ座標

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

        //ミノを描く
        public void Draw(Graphics g, Image source_image)
        {
            //ミノの種類により切り出す画像を選ぶ
            int y_pos = (int)(this.type) * BLOCK_HEIGHT;
            Rectangle srcRect = new Rectangle(0, y_pos, BLOCK_WIDTH, BLOCK_HEIGHT);
            Rectangle desRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);

            for(int y = 0; y < 4;y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (shape2[(int)this.block_rot,y,x] != 0)
                    {
                        desRect.X = x * BLOCK_WIDTH;
                        desRect.Y = y * BLOCK_HEIGHT;
                        g.DrawImage(source_image, desRect, srcRect, GraphicsUnit.Pixel);
                    }
                }
            }

        }

        public BlockType type;  //ミノの種類
        private int[,,] shape2;    //ブロックの形＊４
        private BlockRot block_rot;
        //作成予定 回転法則

        const int BLOCK_WIDTH = 30;     //ブロック画像の幅
        const int BLOCK_HEIGHT = 30;    //ブロック画像の高さ
    }
}
