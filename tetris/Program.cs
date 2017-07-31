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
        private const float FPS = 60;
        private const int waitTime = (int)(1000.0 / FPS);

        
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameField field = new GameField();
            field.Show();

            double targetTime;
            targetTime = (double)System.Environment.TickCount;
            targetTime += waitTime;
            while (field.Created)
            {
                if ((double)System.Environment.TickCount >= targetTime)
                {
                    //メインの処理
                    field.MainLoop();
                    targetTime += waitTime;
                }
                System.Threading.Thread.Sleep(1);
                Application.DoEvents();
            }
        }
    }
}
