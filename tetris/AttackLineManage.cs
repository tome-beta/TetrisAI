using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tetris
{
    //攻撃するライン数を管理する
    class AttackLineManage
    {
        //消去ライン情報
        public struct EraseLineResult
        {
            public int Line;
            public int Ren;
            public int Tspin;
            public bool BtoB;
            public bool perfect;
        };

        public AttackLineManage()
        {
            this.AttackLineNum = 0;
        }

        public void ClearAttackLine()
        {
            this.AttackLineNum = 0;
            this.RenCount = 0;
        }

        public void CalcAttackLine(int erase_line,
            int tspin_type,
            bool perfect,
            ref bool ref_BtoB,
            ref EraseLineResult out_result)
        {
            //REN数のチェック
            if (erase_line == 0)
            {
                this.RenCount = 0;
                return;
            }
            else
            {
                this.RenCount++;
            }

            int attack_line = 0; //攻撃ラインの数

            //Back to Back（バックトゥバック）とは、テトリス消しやT-Spinによるライン消しを連続で行うこと
            bool t_spin = false;
            int tmp_attack_line = 0;
            bool back_to_back = ref_BtoB;

            //T-SPINのメッセージ
            if (tspin_type == BlockControle.TSPIN)
            {
                //BtoBをつける
                ref_BtoB = true;
                t_spin = true;
            }
            else if (tspin_type == BlockControle.TSPIN_MINI)
            {
                //BtoBをつける
                ref_BtoB = true;
            }

            //出力結果を作成。
            out_result.Tspin = tspin_type;
            out_result.Line = erase_line;
            out_result.Ren = this.RenCount;

            //消したラインによって攻撃ラインの数を決める
            switch (erase_line)
            {
                case 1:
                    {
                        if (t_spin)
                        {
                            tmp_attack_line = 2;
                            if (back_to_back)
                            {
                                tmp_attack_line += 1;
                                out_result.BtoB = true;
                            }
                        }
                        else
                        {
                            ref_BtoB = false;
                        }
                    }
                    break;
                case 2:
                    {
                        tmp_attack_line = 1;
                        if (t_spin)
                        {
                            tmp_attack_line = 4;
                            if (back_to_back)
                            {
                                tmp_attack_line++;
                                out_result.BtoB = true;
                            }
                        }
                        else
                        {
                            ref_BtoB = false;
                        }
                    }
                    break;
                case 3:
                    {
                        tmp_attack_line = 2;
                        if (t_spin)
                        {
                            tmp_attack_line = 6;
                            if (back_to_back)
                            {
                                tmp_attack_line++;
                                out_result.BtoB = true;
                            }
                        }
                        else
                        {
                            ref_BtoB = false;
                        }
                    }
                    break;
                case 4:
                    {
                        //BtoBをつける
                        ref_BtoB = true;
                        tmp_attack_line = 4;
                        if (back_to_back)
                        {
                            tmp_attack_line += 1;
                            out_result.BtoB = true;
                        }
                    }
                    break;
            }

            //RENの判定
            int[] REN_ADD = { 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5 };
            int ren = this.RenCount >= REN_ADD.Length ? REN_ADD.Length : this.RenCount;
            tmp_attack_line += REN_ADD[ren];

            //パーフェクトのチェック
            if(perfect)
            {
                tmp_attack_line += 6;
                out_result.perfect = perfect;
            }
            //攻撃ライン数を確定
            attack_line = tmp_attack_line;

            //トータルの攻撃ライン数
            this.AttackLineNum += attack_line;
        }

        public int AttackLineNum = 0;
        public int RenCount = 0;
    }
}
