using System.Media;
using System;
using System.IO;
using System.Threading;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace GameManager.Components
{
    /// <summary>
    /// Нет реализации
    /// </summary>
    public class AudioSource : Component
    {
        private XAudio2 _xaudio2;
        private MasteringVoice _masteringVoice;
        private string _fileName;

        public AudioSource() : base()
        {
            _xaudio2 = new XAudio2();
            _masteringVoice = new MasteringVoice(_xaudio2);
        }

        public void SetSound(string fileName)
        {
            _fileName = fileName;
        }

        public override void Initialize()
        {
            base.Initialize();
            _xaudio2 = new XAudio2();
            _masteringVoice = new MasteringVoice(_xaudio2);
        }

        /// <summary>
        /// Play wav, xwma, adcpm.wav
        /// </summary>
        public void Play()
        {
            if (_xaudio2 == null || _fileName == null)
            {
                Initialize();
                //return;
            }
            
            PLaySoundFile(_xaudio2, _fileName);
        }
        
        static void PLaySoundFile(XAudio2 device, string fileName)
        {
            var stream = new SoundStream(File.OpenRead(fileName));
            var waveFormat = stream.Format;
            var buffer = new AudioBuffer
            {
                Stream = stream.ToDataStream(),
                AudioBytes = (int) stream.Length,
                Flags = BufferFlags.EndOfStream
            };
            stream.Close();
            
            var sourceVoice = new SourceVoice(device, waveFormat, true);
            // Adds a sample callback to check that they are working on source voices
            sourceVoice.BufferEnd += (context) => Console.WriteLine(" => event received: end of buffer");
            sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
            sourceVoice.Start();

            int count = 0;
            while (sourceVoice.State.BuffersQueued > 0)
            {
                if (count == 50)
                {
                    Console.Out.Flush();
                    count = 0;
                }
                Thread.Sleep(10);
                count++;
            }

            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();
            buffer.Stream.Dispose();
        }

        public override void OnDestroy()
        {
            _masteringVoice.Dispose();
            _xaudio2.Dispose();
        }

        public override Component Clone()
        {
            var component = new AudioSource();
            component.SetSound(_fileName);
            return component;
        }
    }
}