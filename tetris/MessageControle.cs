using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetris
{
    /// <summary>
    /// メッセージ表示管理
    /// </summary>
    class MessageControle
    {
        //メッセージを表示するフレーム
        const int DISP_FRAME = 600;

        //コンストラクタ
        public MessageControle()
        {
            DispCouint = 0;
            eraseFlag = false;
        }

        //メッセージを表示する
        public void DrawUpdate()
        {
            if(this.eraseFlag)
            {
                if (this.DispCouint > 0)
                {
                    this.DispCouint--;
                }
                else
                {
                    this.Message1P.Text = @"";
                }
            }
        }
        public void ClearMessage()
        {
            this.eraseFlag = false;
            this.DispCouint = 0;
            this.Message1P.Text = @"";
        }

        public void SetMessage(string message,bool erase)
        {
            this.eraseFlag = erase;
            this.DispCouint = DISP_FRAME;
            this.Message1P.Text = message;
        }

        public Label Message1P { set; get; }

        private int DispCouint;     //表示フレーム
        private bool eraseFlag;     //時間で消すか
    }
}
