using System;
using System.Drawing;
using System.Collections.Generic;

namespace tetris
{
    //  AIの動作を管理
    class AIManage
    {
        public const int LEARNING_TYPE_NUM = 4;
        //学習に使う設定
        public struct LearningSetting
        {
            public int ExecNum;            //繰り返し回数
            public int EvaluateScore;      //評価点
            public int AverageScore;       //平均点
            public int EndConditionsBlock; //終了ブロック数　これだけ落としたら止める。
        };


        /// <summary>
        //フィールドを評価して点数をつける関数
        /// </summary>
        /// <param name="nextBlock">NEXTブロック情報</param>
        /// <param name="block_ctrl">Nブロック操作</param>
        /// <param name="field_manage">フィールド操作</param>
        /// <returns></returns>
        public int EvaluateField(NextBlockManage next_manage,
                                BlockControle block_ctrl,
                                FieldManage field_manage)
        {
            int score = 0;
            int max_score = 0;
            //TOD 整頓必須

            //インスタンスをコピー
            AINextBlockManage = Common.DeepCopyHelper.DeepCopy<NextBlockManage>(next_manage);
            AIBlockControle = Common.DeepCopyHelper.DeepCopy<BlockControle>(block_ctrl);
            AIFieldManage = Common.DeepCopyHelper.DeepCopy<FieldManage>(field_manage);

            //擬似的に次のブロックを取り出す
            int type = AINextBlockManage.GetNextBlock();


            //４つの回転毎に左右に移動できる限界点を探す
            List<SearchPosInfo> searchList = MakeSerachPos(AIBlockControle, AIFieldManage.BlockField);



            //for文で全ての置く場所を検索
            foreach(var info in searchList)
            {
                //一回ごとにフィールドを元にもどす
                AIFieldManage = Common.DeepCopyHelper.DeepCopy<FieldManage>(field_manage);

                //置く場所を決める
                AIBlockControle.CurrentRot = (BlockInfo.BlockRot)info.rot;
                AIBlockControle.CurrentPos.X = info.x;
                AIBlockControle.CurrentPos.Y = info.y;

                //ハードドロップ
                AIBlockControle.HardDropCurrentBlock(AIFieldManage.BlockField);
                //ゲームオーバーの判定がいる
                if( AIBlockControle.CheckGameOver(AIFieldManage.BlockField))
                {
                    return 0;
                }
                 AIBlockControle.SetBlockInField(AIFieldManage.BlockField);

                //フィールドから特徴量を作る
                AIEvaluateManage.Exec(AIBlockControle, AIFieldManage,ref score);

                //計算した特徴量からフィールドのスコアを求める
                if( max_score < score)
                {
                    max_score = score;
                    block_ctrl.CurrentRot = (BlockInfo.BlockRot)info.rot;
                    block_ctrl.CurrentPos.X = info.x;
                    block_ctrl.CurrentPos.Y = info.y;
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
        private NextBlockManage AINextBlockManage = new NextBlockManage();
        private BlockControle AIBlockControle = new BlockControle();
        private FieldManage AIFieldManage = new FieldManage();

        private EvaluateManage AIEvaluateManage = new EvaluateManage();
    }
}
