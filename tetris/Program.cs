using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    static class Program
    {
        //60FPSでの処理
        private const int waitTimes = (int)(1000.0 / 60.0);
        private const int FPS = 60;
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameField field = new GameField();
            field.Show();

            int frame = 0;
            int before = Environment.TickCount;


            int targetTimes = System.Environment.TickCount & int.MaxValue;
            targetTimes += waitTimes;

            while(field.Created)
            {
                int now = Environment.TickCount;
                int progress = now - before;
                int ideal = (int)(frame * (1000.0F / FPS));

                if (ideal > progress) System.Threading.Thread.Sleep(ideal - progress);

                frame++;
                if (progress >= 1000)
                {
                    //メインの処理
                    field.MainLoop(frame);
                    before = now;
                    frame = 0;
                }
                Application.DoEvents();             //windouwsメッセージ処理
            }
        }
    }
}
