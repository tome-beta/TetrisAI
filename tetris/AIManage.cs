using System;
using System.Drawing;
using System.Collections.Generic;

namespace tetris
{
    //  フィールドの評価を行うクラス
    class EvaluateManage
    {
        //最後に操作したブロック
        [Serializable]
        public struct LAST_BLOCK_INFO
        {
            public Point pos;
            public BlockInfo.BlockRot rot;
            public BlockInfo.BlockType type;
        };

        //フィールドの特徴量
        public struct FeatureData
        {
            public int last_block_height;      //１．直前に置いたミノの高さ
            public int eraseline_and_block;    //２．消えたラインの数×ミノの中で消えたブロックの数
            public int horizon_change;         //３．横方向にスキャンした時にセルの内容が変化する回数
            public int veritical_change;       //４．縦方向にスキャンした時にセルの内容が変化する回数
            public int hole;                   //５．穴の数
            public int well_total;             //６．井戸の高さの階和(例４の階和4+3+2+1)の和
            public int hole_on_block_total;    //７．穴の上のブロックの数の和
            public int hole_row;               //８．穴のある行数
            //９．各列の高さの平均値
            //１０．各列の高さの標準偏差
        };


        /// <summary>
        //フィールドを評価して点数をつける関数
        /// </summary>
        /// <param name="field">フィールド配列</param>
        /// <param name="nextBlock">NEXTブロック情報</param>
        /// <param name="block_ctrl">Nブロック操作</param>
        /// <param name="field_manage">フィールド操作</param>
        /// <returns></returns>
        public int EvaluateField(int[,] field, 
                                NextBlockManage next_manage,
                                BlockControle block_ctrl,
                                FieldManage field_manage)
        {
            int score = 0;

            //TOD 整頓必須

            //インスタンスをコピー
            AINextBlockManage = Common.DeepCopyHelper.DeepCopy<NextBlockManage>(next_manage);
            AIBlockControle = Common.DeepCopyHelper.DeepCopy<BlockControle>(block_ctrl);
            AIFieldManage = Common.DeepCopyHelper.DeepCopy<FieldManage>(field_manage);

            //フィールドをコピー
            AIField = (int[,])field.Clone();


            //擬似的に次のブロックを取り出す
            int type = AINextBlockManage.GetNextBlock();


            //４つの回転毎に左右に移動できる限界点を探す
            List<SearchPosInfo> searchList = MakeSerachPos(AIBlockControle,AIField);



            //for文で全ての置く場所を検索
            foreach(var info in searchList)
            {
                //一回ごとにフィールドを元にもどす
                AIField = (int[,])field.Clone();

                //置く場所を決める
                AIBlockControle.CurrentRot = (BlockInfo.BlockRot)info.rot;
                AIBlockControle.CurrentPos.X = info.x;
                AIBlockControle.CurrentPos.Y = info.y;

                //ハードドロップ
                AIBlockControle.HardDropCurrentBlock(AIField);
                //ゲームオーバーの判定がいる
                if( AIBlockControle.CheckGameOver(AIField))
                {
                    return 0;
                }
                 AIBlockControle.SetBlockInField(AIField);

                //フィールドから特徴量を作る
                CalcFeature(AIBlockControle, AIFieldManage,AIField);

                //計算した特徴量からフィールドのスコアを求める


                //TODO ランダムで場所をきめるため
                block_ctrl.CurrentRot = (BlockInfo.BlockRot)info.rot;
                block_ctrl.CurrentPos.X = info.x;
                block_ctrl.CurrentPos.Y = info.y;

                if (Common.MyRandom.Next(10) == 0)
                {
                    break;
                }

            }




            return score;
            
        }

        //AIが探索するブロックの移動範囲を探す
        private List<SearchPosInfo> MakeSerachPos(BlockControle ctrl , int[,] field)
        {
            List<SearchPosInfo> SearchList = new List<SearchPosInfo>();

            /*
              ROT_0,
              ROT_90,
              ROT_180,
              ROT_270,
            */
            for (int i = 0; i < (int)BlockInfo.BlockRot.ROT_TYPE_NUM; i++)
            {
                SearchPosInfo info = new SearchPosInfo();
                ctrl.CurrentRot = (BlockInfo.BlockRot)i;

                info.rot = i;
                info.y = 0;

                //左端を求める
                while(true)
                {
                    if(!ctrl.MoveCurrentBlockLeft(field) )
                    {
                        info.min_x = ctrl.CurrentPos.X;
                        break;
                    }
                }


                //右端を求める
                while (true)
                {
                    if (!ctrl.MoveCurrentBlockRight(field))
                    {
                        info.max_x = ctrl.CurrentPos.X;
                        break;
                    }
                }

                //左端から右端までの位置を作成
                for(int x = info.min_x + 1; x < info.max_x;x++ )
                {
                    info.x = x;
                    //Listに追加
                    SearchList.Add(info);

                }
            }





            return SearchList;

        }


        //特徴量を計算する
        private FeatureData CalcFeature(BlockControle ai_controle,FieldManage ai_field_manage,int[,] ai_field)
        {
            FeatureData feature_data = new FeatureData();

            //特徴量１
            feature_data.last_block_height =  CalcLastBlockHeight(ai_controle,ai_field);

            return feature_data;
        }

        //特徴量１：最後に置いたブロックの高さを計算
        private int CalcLastBlockHeight(BlockControle ai_controle, int[,] ai_field)
        {
            int height = 0;

            LAST_BLOCK_INFO last_block = ai_controle.LastBlockInfo;

            //位置を基準に4*4を検索して最初に空きじゃ無いところ
            for (int y = 0; y < BlockInfo.BLOCK_CELL_HEIGHT; y++)
            {
                for (int x = 0; x < BlockInfo.BLOCK_CELL_WIDTH; x++)
                {
                        //フィールドチェック
                        int chk_x = last_block.pos.X + x;
                    int chk_y = last_block.pos.Y + y;

                    int fence = (int)(BlockInfo.BlockType.MINO_FENCE | BlockInfo.BlockType.MINO_IN_FIELD);

                    if (ai_field[chk_y,chk_x] != 0 &&
                        ai_field[chk_y, chk_x] != fence)
                    {
                        //ブロック発見
                        height = chk_y;
                        break;
                    }

                }

                if(height != 0)
                {
                    break;
                }

            }


            return height;
        }

        /// <summary>
        /// 特徴量５　穴の数をカウント
        /// 穴・・四方を囲まれているセル
        /// </summary>
        /// <param name="data"></param>
        private void CalcHole(int[,] field,ref FeatureData data)
        {

        }

        //AIのブロック位置探索用
        private struct SearchPosInfo
        {
            public int rot;
            public int min_x;  //左端座標
            public int max_x;  //右端座標
            public int x;
            public int y;
        };

        BlockInfo block_info = new BlockInfo(); //先読み用

        //AI用に擬似的にフィールドを操作できるようにコピー先を用意する
        NextBlockManage AINextBlockManage = new NextBlockManage();
        BlockControle AIBlockControle = new BlockControle();
        FieldManage AIFieldManage = new FieldManage();
        int[,] AIField;
    }
}
