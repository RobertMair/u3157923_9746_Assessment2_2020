namespace ClassLibrary
{
    public abstract class NpcBase
    {

        public char NpcChar { get; set; }
        public int NpcX { get; set; }
        public int NpcY { get; set; }
        public int NpcPwrMax { get; set; }
        public int NpcPwrLvl { get; set; }
        public int NpcPwrWar { get; set; }
        public int FgColour { get; set; }
        public int BgColour { get; set; }
        public int NpcStartDelay { get; set; } // Used to set the timer for when the NPC appears in the level.
        public int NpcMoveSpeed { get; set; } // Used to set the current delay between each move of the NPC.
        public int NpcMaxMoveSpeed { get; set; } // The maximum move speed the NPC has.


        // What does the NPC do next?
        // Instead of marking method below 'abstract' I have marked it 'virtual' to demonstrate knowledge of 'polymorphism'
        // whereby derived classes have their own implementation of this method.
        public virtual void NpcMoveDecision(int endX, int endY)
        {
            // Do something.
        }

        // Move NPC using specfic derived type NPC movement method.
        // Method marked as abstract as Base class has no implementation, but derived classes must have an implementation.
        public abstract void NpcMove(int endX, int endY);

        // NPC has found the Player character, what does it do?
        public abstract string NpcFoundPlayer();

        // NPC has found the Player character and is attacking.
        public abstract string NpcAttackPlayer();

        public NpcBase(char npcChar, int npcX, int npcY, int npcPwrMax, int npcPwrLvl, int npcPwrWar, int fgColour, int bgColour, int npcStartDelay, int npcMoveSpeed, int npcMaxMoveSpeed)
        {
            NpcChar = npcChar;
            NpcX = npcX;
            NpcY = npcY;
            NpcPwrMax = npcPwrMax;
            NpcPwrLvl = npcPwrLvl;
            NpcPwrWar = npcPwrWar;
            FgColour = fgColour;
            BgColour = bgColour;
            NpcStartDelay = npcStartDelay;
            NpcMoveSpeed = npcMoveSpeed;
            NpcMaxMoveSpeed = npcMaxMoveSpeed;
        }

    }













}