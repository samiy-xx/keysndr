using System;

namespace KeySndr.Win
{
    public class AssembyInfo
    {
        public static Version GetAssemblyVersion(Type a)
        {
            var reference = a.Assembly;
            return reference.GetName().Version;
        }
    }
}
