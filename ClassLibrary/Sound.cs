using System.IO;
using System.Media;
using System.Speech.Synthesis;


namespace ClassLibrary
{
    public class Sound
    {

        // Set sound effect and initial music objects.

        public static string dataFilePath =
            Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\DataFiles\\";

        public static SoundServices playerStep =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_movement_footsteps1b.wav")),
                false);

        public static SoundServices lowEnergyOxygen =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_alarm_loop2.wav")),
                true);

        public static SoundServices airLockOpen =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_sound_depressurizing.wav")),
                false);

        public static SoundServices doorLocked =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_sounds_impact11.wav")),
                false);

        public static SoundServices goThroughAirLock =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_movement_portal1.wav")),
                false);

        public static SoundServices forceField =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_sounds_interaction13.wav")),
                false);

        public static SoundServices collectible =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_coin_single2.wav")),
                false);

        public static SoundServices infoTerminal =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "sfx_sound_poweron.wav")),
                false);

        public static SoundServices music =
            new SoundServices(new MemoryStream(File.ReadAllBytes(dataFilePath + "Underclocked (underunderclocked mix).wav")),
                true);


        // Speak and play music functions.
        // Initialize a new instance of the SoundPlayer.
        public static SoundPlayer SoundPlayer = new SoundPlayer();

        public static bool soundFxOn = true;

        // Initialize a new instance of the SpeechSynthesizer.
        public static SpeechSynthesizer SpeachSynth = new SpeechSynthesizer();

        public static void Speak(string text, int volume, bool sync)
        {
            SpeachSynth.SetOutputToDefaultAudioDevice();
            // Set the volume of the SpeechSynthesizer's output.  
            SpeachSynth.Volume = volume;
            // Create a prompt from a string.  
            Prompt intro = new Prompt(text);
            // Speak the contents of the prompt asynchronously.
            if (sync)
            {
                SpeachSynth.Speak(intro);
            }
            else
            {
                SpeachSynth.SpeakAsync(intro);
            }
        }

        public static void SoundFX(string file, int volume, bool sync)
        {
            string dataFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                                  "\\DataFiles\\" + file;
            SoundPlayer.SoundLocation = dataFilePath;
            SoundPlayer.Play();
        }

        public static void Music(string file, int volume, bool sync)
        {
            string dataFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName +
                                  "\\DataFiles\\" + file;
            SoundPlayer.SoundLocation = dataFilePath;
            SoundPlayer.PlayLooping();
        }

    }
}

