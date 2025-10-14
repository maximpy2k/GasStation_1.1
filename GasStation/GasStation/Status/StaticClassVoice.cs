using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GasStation.Status
{
    public static class StaticClassVoice
    {
        public static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        private static bool IsStarted = false;
        public static void SpeakVoise()
        {
            try
            {
                IsStarted = true;
                while (queue.Count != 0)
                {
                    var message = "";
                    if (!queue.TryDequeue(out message))
                        continue;

                    new SpeechSynthesizer().Speak(message);
                }

                IsStarted = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void Speak(String message)
        {
            var cnt = queue.Count;
            Thread thread = new Thread(StaticClassVoice.SpeakVoise);
            queue.Enqueue(message);
            if (!IsStarted)
                thread.Start();

        }
    }
}
