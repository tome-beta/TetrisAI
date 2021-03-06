﻿using System;
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
            REN,
            PERFECT,
        };

        //コンストラクタ
        public MessageControle()
        {
            DispCouint = 0;
            eraseFlag = false;
            message_list.Clear();
        }

        public void MakeEraseLineMessage(AttackLineManage.EraseLineResult result)
        {
            if (result.Line <= 0)
            {
                return;
            }

            if (result.perfect)
            {
                //パーフェクトは他にメッセージを出さない
                message_list.Add(MessageControle.MESSAGE_TYPE.PERFECT);
                MakeMessage();
                return;
            }

            if (result.BtoB)
            {
                message_list.Add(MessageControle.MESSAGE_TYPE.BACK_TO_BACK);
            }
            if (result.Tspin == BlockControle.TSPIN)
            {
                message_list.Add(MessageControle.MESSAGE_TYPE.T_SPIN);
            }
            else if (result.Tspin == BlockControle.TSPIN_MINI)
            {
                message_list.Add(MessageControle.MESSAGE_TYPE.T_SPIN_MINI);
            }

            switch (result.Line)
            {
                case 1: message_list.Add(MessageControle.MESSAGE_TYPE.SINGLE); break;
                case 2: message_list.Add(MessageControle.MESSAGE_TYPE.DOUBLE); break;
                case 3: message_list.Add(MessageControle.MESSAGE_TYPE.TRIPLE); break;
                case 4: message_list.Add(MessageControle.MESSAGE_TYPE.TETRIS); break;
                default: break;
            }

            //REN
            if (result.Ren >= 3)
            {
                ren_num = result.Ren;
                message_list.Add(MessageControle.MESSAGE_TYPE.REN);
            }

            MakeMessage();

        }


        /// <summary>
        /// メッセージを作成する
        /// </summary>
        private void MakeMessage()
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
                    case MESSAGE_TYPE.REN:
                        {
                            message += " REN " + this.ren_num; break;
                        }
                    case MESSAGE_TYPE.PERFECT:
                        {
                            message = @"PERFECT !!"; break;
                        }
                    default:
                        break;
                }
            }
            message_list.Clear();

            this.eraseFlag = true;
            this.DispCouint = DISP_FRAME;
            this.Message.Text = message;
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
                    this.Message.Text = @"";
                }
            }
        }
        public void ClearMessage()
        {
            this.eraseFlag = false;
            this.DispCouint = 0;
            this.Message.Text = @"";
        }

        public void SetMessage(string message,bool erase)
        {
            this.eraseFlag = erase;
            this.DispCouint = DISP_FRAME;
            this.Message.Text = message;
        }

        public Label Message { set; get; }

        private int DispCouint;     //表示フレーム
        private bool eraseFlag;     //時間で消すか
        public List<MESSAGE_TYPE> message_list = new List<MESSAGE_TYPE>();

        public int ren_num;         //REN数を表示するため
    }
}
