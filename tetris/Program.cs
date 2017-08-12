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

            // 次に処理するフレームの時刻（初回は即処理するので初期値は現在時刻をセット）
            double nextFrame = (double)System.Environment.TickCount;
            // フレームを処理する周期（1/60秒）
            float period = 1000f / 60f;

            double lastTime = (double)System.Environment.TickCount;
            int fps = 60;
            double fpsLimiter = 0.0f;
            //ゲームループ
            while (field.Created)
            {
                //開始時間を取得
                double startTime = (double)System.Environment.TickCount;

                //前回の呼び出しからの経過時間
                double deltaTime = startTime - lastTime;

///                mainForm.deltaTime = deltaTime;

                //フレームレートの計算
                fpsLimiter += deltaTime;

                if (fpsLimiter > 1000)
                {
                    fpsLimiter -= 1000;
                    field.fps = fps;
                    fps = 0;
                }

                // ここで描画以外の計算処理を行う。
                //メインの処理
                field.Exec();

                //描画処理
                field.DrawUpdate();

                fps++;

                lastTime = startTime;

                // Windowsメッセージを処理させる
                Application.DoEvents();

                //処理全体の時間が最大フレーム時間に満たない場合、スリープする
                double procTime = (double)System.Environment.TickCount - startTime;

                // 1ms以上の間があるか？
                if (period - procTime > 1)
                {
                    // Sleepする
                    System.Threading.Thread.Sleep((int)(period - procTime));
                }
            }
        }
    }
}
