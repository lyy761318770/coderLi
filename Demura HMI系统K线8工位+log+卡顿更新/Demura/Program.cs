using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demura
{
    
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]

        

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmLogin frm = new FrmLogin();
            DialogResult dr = frm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Application.Run(new FrmMain());
            }
            //Application.Run(new FrmLogin());

        }
    }
}
