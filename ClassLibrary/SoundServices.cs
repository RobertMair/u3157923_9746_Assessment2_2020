using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Source code/repo from https://github.com/sharpdx/SharpDX-Samples/blob/master/Desktop/XAudio2/PlaySound/Program.cs
    public class SoundServices : IDisposable
    {
        /// <summary>
        /// Gets current sound file
        /// </summary>
        string SoundFile { get; }
        /// <summary>
        /// Gets current sound stream
        /// </summary>
        public Stream SoundStream { get; private set; }
        /// <summary>
        /// Gets/Sets looping option, sound will loop if set to true.
        /// </summary>
        public bool IsLooping { get; private set; }
        /// <summary>
        /// Holds the message of last error if any, check it if some feature fails
        /// </summary>
        public string LastErrorMsg { get; private set; }
        //Play control flags
        bool IsPlaying = false;
        bool IsUserStop = false;


        AutoResetEvent Playing;
        float CurrentVolume = 1.0f;
        bool IsInitialized = false;

        SoundStream stream;
        WaveFormat waveFormat;
        AudioBuffer buffer;
        SourceVoice sourceVoice;
        XAudio2 xaudio2;
        MasteringVoice masteringVoice;

        /// <summary>
        /// Initializes an instance by creating xAudio2 graph and preparing audio Buffers
        /// </summary>
        /// <param name="soundStream">WAV format sound stream</param>
        /// <param name="loop">Bool indicating to loop sound playing  or not</param>
        public SoundServices(Stream soundStream, bool loop)
        {
            try
            {
                if (soundStream == null)
                {
                    throw new ArgumentNullException("soundStream", "Null is not allowed, please specify a valid stream");
                }
                SoundStream = soundStream;
                SoundFile = null;
                IsLooping = loop;
                //Playing = new ManualResetEvent(false);
                Playing = new AutoResetEvent(false);
                BuildxAudio2Graph();
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        #region Sound Play API
        /// <summary>
        /// Plays the sound stream loaded during initialization
        /// </summary>
        /// <returns>Task of sound playing</returns>
        public Task PlaySound()
        {
            return Task.Factory.StartNew(() =>
            {
                PlayRepeatAsync();
            });
        }
        /// <summary>
        /// Task for starting play of sound buffers using xAudio2 graph
        /// </summary>
        void PlayRepeatAsync()
        {
            try
            {
                IsPlaying = true;
                if (buffer == null)
                {
                    //stream = new SoundStream(SoundStream);
                    //waveFormat = stream.Format;
                    buffer = new AudioBuffer
                    {
                        Stream = stream.ToDataStream(),
                        AudioBytes = (int)stream.Length,
                        Flags = SharpDX.XAudio2.BufferFlags.EndOfStream
                    };

                    if (IsLooping)
                        buffer.LoopCount = AudioBuffer.LoopInfinite;

                    sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
                    //Close the stream as it is now loaded in buffer already
                    //stream.Close();
                }

                sourceVoice.Start();
                Playing.WaitOne();
                sourceVoice.Stop();
                IsPlaying = false;
                sourceVoice.FlushSourceBuffers();
                //xAudio2 graph creation step (5) send the AudioBuffer to sourceVoice
                sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
            }
            catch (Exception e)
            {

                LastErrorMsg = "PlayRepeatAsync(): " + e.Message;
                IsPlaying = false;
            }

        }
        /// <summary>
        /// Initializes xAudio2 graph to be used for sound playing
        /// </summary>
        void BuildxAudio2Graph()
        {
            try
            {
                stream = new SoundStream(SoundStream);
                waveFormat = stream.Format;
                //xAudio2 graph creation step (1) Create XAudio2 device
                xaudio2 = new XAudio2();
                //xAudio2 graph creation step (2) Create MasteringVoice and connect it to device
                //*Note: You must use aMasteringVoice to connect to xAudioDevice
                masteringVoice = new MasteringVoice(xaudio2);
                //SetVolume(CurrentVolume);

                //xAudio2 graph creation step (3) Prepare sourceVoice
                sourceVoice = new SourceVoice(xaudio2, waveFormat, true);

                // Adds a  callback check buffer end and Looping option
                sourceVoice.BufferEnd += SourceVoice_BufferEnd;
                //xAudio2 graph creation step (5) send the AudioBuffer to sourceVoice

                IsInitialized = true;
            }
            catch (Exception e)
            {

                LastErrorMsg = "BuildxAudio2Graph(): " + e.Message;
                IsInitialized = false;
            }
        }
        /// <summary>
        /// Recreates source buffer allowing sound change or loop change
        /// </summary>
        void RecreateBuffer()
        {
            try
            {
                SoundStream.Seek(0, SeekOrigin.Begin);
                stream = new SoundStream(SoundStream);
                waveFormat = stream.Format;
                //if (buffer == null)
                {
                    //stream = new SoundStream(SoundStream);
                    //waveFormat = stream.Format;
                    buffer = new AudioBuffer
                    {
                        Stream = stream.ToDataStream(),
                        AudioBytes = (int)stream.Length,
                        Flags = SharpDX.XAudio2.BufferFlags.EndOfStream
                    };

                    if (IsLooping)
                        buffer.LoopCount = AudioBuffer.LoopInfinite;
                    sourceVoice.FlushSourceBuffers();
                    sourceVoice.SubmitSourceBuffer(buffer, stream.DecodedPacketsInfo);
                    //Close the stream as it is now loaded in buffer already
                    //stream.Close();
                }
            }
            catch (Exception e)
            {

                LastErrorMsg = "RecreateBuffer(): " + e.Message;
            }

        }

        /// <summary>
        /// Changes current audio to a new one
        /// </summary>
        /// <param name="soundStream">a stream from wav file</param>
        public void ChangeSoundTo(Stream soundStream, bool Loop)
        {
            try
            {
                IsLooping = Loop;
                SoundStream = soundStream;
                stream = new SoundStream(SoundStream);
                waveFormat = stream.Format;
                sourceVoice = new SourceVoice(xaudio2, waveFormat, true);
                sourceVoice.BufferEnd += SourceVoice_BufferEnd;
                RecreateBuffer();
            }
            catch (Exception e)
            {
                IsInitialized = false;
                LastErrorMsg = "ChangeSoundTo(): " + e.Message;
            }
        }

        /// <summary>
        /// Set loop ot no loop
        /// </summary>
        /// <param name="loop">True = Loop forever, false = play till end</param>
        public void SetLooping(bool loop)
        {
            if (IsPlaying)
                Stop();
            IsLooping = loop;
            RecreateBuffer();
        }
        #endregion

        /// <summary>
        /// Immediately Stops currently playing sound
        /// </summary>
        public void Stop()
        {
            try
            {
                if (IsPlaying)
                {
                    IsUserStop = true;
                    Playing.Set();
                }
            }
            catch (Exception e)
            {

                LastErrorMsg = "Stop(): " + e.Message;
            }
        }

        /// <summary>
        /// Gets Current Volume
        /// </summary>
        /// <returns>Current volume</returns>
        public float GetVolume()
        {
            float current = 0.0f;
            try
            {
                if (sourceVoice == null || sourceVoice.IsDisposed) return CurrentVolume;

                sourceVoice.GetVolume(out current);
            }
            catch (Exception e)
            {

                LastErrorMsg = "GetVolume(): " + e.Message;
            }
            return current;
        }

        /// <summary>
        /// Sets the current volume
        /// </summary>
        /// <param name="newVolume">returns back the current setting for confirmation</param>
        /// <returns>The current set volume</returns>
        public float SetVolume(float newVolume)
        {
            try
            {
                if (newVolume > 1 || newVolume < 0) return GetVolume();
                if (sourceVoice == null || sourceVoice.IsDisposed)
                {
                    CurrentVolume = newVolume;
                    return newVolume;
                }
                sourceVoice.SetVolume(newVolume, 0);

                return GetVolume();
            }
            catch (Exception e)
            {

                LastErrorMsg = "SetVolume(): " + e.Message;
                return 0.0f;
            }
        }


        /// <summary>
        /// End of buffer event handler
        /// </summary>
        /// <param name="obj"></param>
        private void SourceVoice_BufferEnd(IntPtr obj)
        {
            //Debug.WriteLine($"buffer end reached with looping {IsLooping}");
            if (!IsLooping)
            {
                if (IsPlaying && !IsUserStop)
                    Playing.Set();
                else if (IsUserStop)
                {
                    IsUserStop = false;
                }
            }

        }

        public void Dispose()
        {
            if (sourceVoice != null && !sourceVoice.IsDisposed)
            {
                sourceVoice.DestroyVoice();

                sourceVoice.Dispose();
            }
            if (buffer != null && buffer.Stream != null)
                buffer.Stream.Dispose();
            if (masteringVoice != null && !masteringVoice.IsDisposed)
                masteringVoice.Dispose();
            if (xaudio2 != null && !xaudio2.IsDisposed)
                xaudio2.Dispose();
        }

        ~SoundServices()
        {
            if (sourceVoice != null && !sourceVoice.IsDisposed)
            {
                sourceVoice.DestroyVoice();

                sourceVoice.Dispose();
            }
            if (buffer != null && buffer.Stream != null)
                buffer.Stream.Dispose();
            if (masteringVoice != null && !masteringVoice.IsDisposed)
                masteringVoice.Dispose();
            if (xaudio2 != null && !xaudio2.IsDisposed)
                xaudio2.Dispose();
        }
    }
}
