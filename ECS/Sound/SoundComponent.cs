using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace ECS.Sound
{
    public struct SoundComponent
    {
        public XAudio2 XAudio2;
        public MasteringVoice MasteringVoice;
        public string FilePath;

        public static async Task PlaySoundFileAsync(XAudio2 device, string fileName, bool loop = false)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var stream = new SoundStream(File.OpenRead(fileName)))
                    {
                        var waveFormat = stream.Format;
                        var buffer = new AudioBuffer
                        {
                            Stream = stream.ToDataStream(),
                            AudioBytes = (int)stream.Length,
                            Flags = BufferFlags.EndOfStream
                        };

                        var sourceVoice = new SourceVoice(device, waveFormat, true);
                        bool isPlaying = true;

                        sourceVoice.BufferEnd += (context) =>
                        {
                            if (loop && isPlaying)
                            {
                                buffer.Stream.Position = 0;
                                sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
                            }
                            else
                            {
                                isPlaying = false;
                            }
                        };

                        sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
                        sourceVoice.Start();

                        while (isPlaying && sourceVoice.State.BuffersQueued > 0)
                        {
                            Thread.Sleep(10);
                        }

                        isPlaying = false;
                        sourceVoice.Stop();
                        sourceVoice.DestroyVoice();
                        sourceVoice.Dispose();
                        buffer.Stream.Dispose();
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка воспроизведения: {ex.Message}");
            }
        }
    }
}
