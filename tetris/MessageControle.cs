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

        public enum MESSAGE_TYPE
        {
            SINGLE,
            DOUBLE,
            TRIPLE,
            TETRIS,
            T_SPIN_MINI,
            T_SPIN,
            BACK_TO_BACK,
        };

        //コンストラクタ
        public MessageControle()
        {
            DispCouint = 0;
            eraseFlag = false;
        }

        /// <summary>
        /// メッセージを作成する
        /// </summary>
        public void MakeMessage()
        {
            string message = @"";

            foreach (MESSAGE_TYPE no in message_list)
            {
                switch (no)
                {
                    case MESSAGE_TYPE.SINGLE: message += "SINGLE "; break;
                    case MESSAGE_TYPE.DOUBLE: message += "DOUBLE "; break;
                    case MESSAGE_TYPE.TRIPLE: message += "TRIPLE "; break;
                    case MESSAGE_TYPE.TETRIS: message += "TETRiS "; break;
                    case MESSAGE_TYPE.T_SPIN_MINI: message += "T-SPIN MINI "; break;
                    case MESSAGE_TYPE.T_SPIN: message += "T-SPIN "; break;
                    case MESSAGE_TYPE.BACK_TO_BACK: message += "BACK to BACK "; break;
                }
            }
            message_list.Clear();

            this.eraseFlag = true;
            this.DispCouint = DISP_FRAME;
            this.Message1P.Text = message;
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
        public List<MESSAGE_TYPE> message_list = new List<MESSAGE_TYPE>();
    }
}
