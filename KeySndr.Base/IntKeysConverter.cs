using System.Windows.Forms;

namespace KeySndr.Base
{
    public class IntKeysConverter : KeysConverter
    {
        public int ConvertToInt(Keys k)
        {
            return (int)k;
        }

        public Keys ConvertFromInt(int i)
        {
            return (Keys)i;
        }
    }
}
