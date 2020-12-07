using System.Windows.Forms;


namespace QServiceAgent
{
    public class MsgHelper
    {
        static MsgHelper _ = new MsgHelper();
        public static MsgHelper Instance { get; } = _;

        [System.Diagnostics.Conditional("DEBUG")]
        public void Warn(string msg)
        {
            MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
