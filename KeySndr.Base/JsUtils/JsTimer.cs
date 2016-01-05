using System;
using System.Threading;
using Jint.Native;

namespace KeySndr.Base.JsUtils
{
    public class JsTimer
    {
        private Timer timer;
        private Action actions;

        public void Tick(Delegate d)
        {
            actions += () => d.DynamicInvoke(JsValue.Undefined, new[] { JsValue.Undefined });
        }
    
        public void Start(int delay, int milliseconds)
        {
            if (timer != null)
                return;

            timer = new Timer(s => actions());
            timer.Change(delay, milliseconds);
        }

        public void Stop()
        {
            if (timer == null)
                return;
            timer.Dispose();
            timer = null;
        }
    }
}
