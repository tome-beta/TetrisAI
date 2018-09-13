using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace remakGA
{
/*
        世代数と遺伝子数の設定が必要
        使い方
        {
            Colony colon = new Colony();

            colon.Initialize(4 * 5);

            //初期の親を登録
            GenomManager manage = new GenomManager();

            manage.AddGenom(colon.RandomGetGenom());
            manage.AddGenom(colon.RandomGetGenom());

            const int GENALATION = 10;

            //世代数だけくりかえす
            for (int i = 0; i < GENALATION; i++)
            {
                //親をつくる
                manage.MakeParentGenom();

                //交叉で子をつくる
                manage.CrossExec();

                //突然変異
                manage.Mutation();

                //実行

                //結果による順位をつける
                int[] ranking = { 0, 1, 2, 3 };


                //判定にしたがってColnyにGenomを戻す
                manage.SelectExec(ranking);

            }

            //allGenomList の０番がいいヤツになってるはず
        }
*/

        //全探索空間　このクラスが必要かは要検討
        class Colony
        {
            //全探索空間に持っておく遺伝子の数
            readonly int GENOM_MAX = 100;

            public void Initialize(int DnaSize)
            {
                GenomList.Clear();

                for (int i = 0; i < GENOM_MAX; i++)
                {
                    Genom genom = new Genom(DnaSize);
                    GenomList.Add(genom);
                }

            }

            //ランダムに登録されている遺伝子を作り出す
            public Genom RandomGetGenom()
            {

                int num = rand.Next(GENOM_MAX);

                if (GenomList.Count <= num)
                {
                    //だめ
                    Debug.Assert(false);
                }

                return GenomList[num];
            }

            //遺伝子リストを持っとく

            private List<Genom> GenomList = new List<Genom>();
            Random rand = new Random();
        }

        //遺伝子
        class Genom
        {
            //デフォルトコンストラクタ
            public Genom()
            {
                this.DNA = new int[this.DNA_SIZE];
                SettingDefaultDNA();
            }

            //DNAのサイズを指定して作成
            public Genom(int DNA_size)
            {
                this.DNA = new int[DNA_size];
                SettingDefaultDNA();
            }

            //遺伝子配列を指定して作成。
            public Genom(int[] input_dna)
            {
                this.DNA = (int[])input_dna.Clone();
            }

            public int GetDNASize()
            {
                return DNA.Length;
            }

            //初期値をランダムで決める
            private void SettingDefaultDNA()
            {
                Random rand = new Random();

                for (int i = 0; i < DNA.Length; i++)
                {
                    this.DNA[i] = rand.Next(DNA_VALUE_MIN, DNA_VALUE_MAX);
                }
            }


            //持っているパラメータ
            public int[] DNA;

            public readonly int DNA_SIZE = 10;//デフォルト　まず使わない
            public readonly int DNA_VALUE_MAX = 100;
            public readonly int DNA_VALUE_MIN = -100;

        }

        //遺伝子操作クラス(局所空間)
        class GenomManager
        {
            //順位にしたがって残す遺伝子を決める
            public void SelectExec(int[] ranking)
            {
                //ranking 親１ 親２ 子１ 子２　の順番

                String result = @""; //上位２つを表す文字列

                for (int i = 0; i < ranking.Length; i++)
                {
                    if (ranking[i] == 0)
                    {
                        if (i >= 2)
                        {
                            result = result.Insert(0, "C");
                        }
                        else
                        {
                            result = result.Insert(0, "P");
                        }
                    }
                    if (ranking[i] == 1)
                    {
                        if (i >= 2)
                        {
                            result = result.Insert(result.Length, "C");
                        }
                        else
                        {
                            result = result.Insert(result.Length, "P");
                        }

                    }
                }

                //親の上位を決める
                int parent_up = -1;
                if (ranking[0] < ranking[1])
                {
                    parent_up = 0;
                }
                else
                {
                    parent_up = 1;
                }

                //子の上位を決める
                int child_up = -1;
                if (ranking[2] < ranking[3])
                {
                    child_up = 0;
                }
                else
                {
                    child_up = 1;
                }

                //局所集団にもどす遺伝子をきめる
                switch (result)
                {
                    case "CC": // type A 子供 2人と 親1人 コロニー ＋1
                        AddGenom(this.ChildList[0]);
                        AddGenom(this.ChildList[1]);
                        AddGenom(this.ParentList[parent_up]);
                        break;
                    case "PP": // type B 親1人 コロニー -1 人口 1人なら、異邦人要求
                        AddGenom(this.ParentList[parent_up]);
                        if (this.AllGenomList.Count < 2)
                        {
                            Genom n = new Genom(4 * 5);
                            AddGenom(n);
                        }
                        break;
                    case "PC": // type c 親1人 子供1人 コロニー 増減なし
                        AddGenom(this.ParentList[parent_up]);
                        AddGenom(this.ChildList[child_up]);
                        break;
                    case "CP": //type D 子供 1人 異邦人要求 コロニー 増減なし
                        AddGenom(this.ChildList[child_up]);
                        Genom n2 = new Genom(4 * 5);
                        AddGenom(n2);
                        break;
                }
            }


            //局所集団リストに加える
            public void AddGenom(Genom p)
            {
                this.AllGenomList.Add(p);
            }

            //局所集団リストから親をつくる
            public void MakeParentGenom()
            {
                this.ParentList.Clear();

                int num = rand.Next(this.AllGenomList.Count);
                this.ParentList.Add(this.AllGenomList[num]);
                this.AllGenomList.RemoveAt(num);

                num = rand.Next(this.AllGenomList.Count);
                this.ParentList.Add(this.AllGenomList[num]);
                this.AllGenomList.RemoveAt(num);

            }

            //親リストに加える
            public void AddParentGenom(Genom p)
            {
                if (ParentList.Count > 2)
                {
                    //だめ
                    Debug.Assert(false);
                }

                ParentList.Add(p);
            }

            //親２つで交叉を行う
            public void CrossExec()
            {
                int[] p1 = ParentList[0].DNA;
                int[] p2 = ParentList[1].DNA;

                //交叉点の作成
                int[] cutPoint = MakeCutPoint(rand.Next(1, p1.Length), 0, p1.Length);

                //分割点の最後に配列のサイズを入れる
                Array.Resize(ref cutPoint, cutPoint.Length + 1);
                cutPoint[cutPoint.Length - 1] = p1.Length;

                //交叉実行
                int[] ch1 = new int[p1.Length];
                int[] ch2 = new int[p1.Length];
                int IX = 0, st = 0;
                foreach (int pt in cutPoint)
                {
                    if (IX++ % 2 == 0)
                    {
                        Array.Copy(p1, st, ch1, st, pt - st);
                        Array.Copy(p2, st, ch2, st, pt - st);
                    }
                    else
                    {
                        Array.Copy(p1, st, ch2, st, pt - st);
                        Array.Copy(p2, st, ch1, st, pt - st);
                    }
                    st = pt;
                }

                //子をつくる
                Genom ch_genom1 = new Genom(ch1);
                Genom ch_genom2 = new Genom(ch2);

                ChildList.Add(ch_genom1);
                ChildList.Add(ch_genom2);
            }

            /// <summary>
            /// 突然変異
            /// </summary>
            public void Mutation()
            {
                //子のどちらかを突然変異させる
                int num = rand.Next(2);
                Genom ch = ChildList[num];

                HashSet<int> hsTable = new HashSet<int>();
                while (true)
                {
                    int point = rand.Next(ch.DNA_SIZE);
                    if (hsTable.Add(point) == false)
                    {
                        break;
                    }

                    ch.DNA[point] = rand.Next(ch.DNA_VALUE_MIN, ch.DNA_VALUE_MAX);
                }
            }

            /// <summary>
            /// 交叉するときに入れ替え点をつくる
            /// </summary>
            /// <param name="cut_num">交叉点の数</param>
            /// <param name="start"></param>
            /// <param name="enc"></param>
            /// <returns></returns>
            private int[] MakeCutPoint(int cut_num, int start, int end)
            {
                SortedSet<int> ssTable = new SortedSet<int>();
                while (ssTable.Count() < cut_num)
                {
                    ssTable.Add(rand.Next(start + 1, end));
                }
                int[] ans = new int[cut_num];
                ssTable.CopyTo(ans);
                return (ans);
            }

            //親１親２　子１子２のGenomを持つ
            List<Genom> ParentList = new List<Genom>();
            List<Genom> ChildList = new List<Genom>();

            //局所集団にあるGenom全て
            List<Genom> AllGenomList = new List<Genom>();   //この中から２つをランダムに選んで親とする



            Random rand = new Random();
        }

        //計算するところは外部になる


}


