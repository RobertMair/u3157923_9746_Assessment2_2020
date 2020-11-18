using System;

namespace ClassLibrary
{

    public class Homer : NpcBase
    {
        public Homer(char npcChar, int npcX, int npcY, int npcPwrMax, int npcPwrLvl, int npcPwrWar, int fgColour, int bgColour, int npcStartDelay, int npcMoveSpeed, int npcMaxMoveSpeed)
        : base(npcChar, npcX, npcY, npcPwrMax, npcPwrLvl, npcPwrWar, fgColour, bgColour, npcStartDelay, npcMoveSpeed, npcMaxMoveSpeed)
        {
        }

        private Random rand = new Random();

        // NPC AI move decisions based on 'weighted randoms', based on the attribute of how much power the NPC currently has.
        public override void NpcMoveDecision(int endX, int endY)
        {

            // Homer decision to move is based on current power level vs max power level.
            // The more power the Homer has, the more chance it will move to track the player.
            {
                int decision = rand.Next(0, NpcPwrMax);
                if (decision < NpcPwrLvl)
                {
                    if (NpcPwrLvl > NpcPwrWar * 1.5) // If Homer has enough power ( > 1.5 * power warning level) it will track the Player character at full speed.
                    {
                        NpcMoveSpeed = NpcMaxMoveSpeed;
                        NpcMove(endX, endY);
                    }
                    else if (NpcPwrLvl > NpcPwrWar) // If Homer has enough power ( > power warning level) it will track the Player character at half speed.
                    {
                        NpcMoveSpeed = NpcMaxMoveSpeed * 2;
                        NpcMove(endX, endY);
                    }
                    else if (NpcPwrLvl < NpcPwrWar) // If Homer is below half power warning level it will stop to recharge.
                    {
                        NpcPwrLvl = Math.Min(NpcPwrLvl + 75, NpcPwrMax);
                        GUI.CharWrite(Grid.GridXOrg + NpcX, Grid.GridYOrg + NpcY, NpcChar, FgColour,
                            BgColour);
                    }
                }
                NpcPwrLvl = Math.Min(NpcPwrLvl + 50, NpcPwrMax);
            }
        }

        public override void NpcMove(int endX, int endY)
        {
            if (!(NpcX == Player.playerX && NpcY == Player.playerY))
            {
                var (moveX, moveY) =
                    AStarSearch.AStar(NpcX, NpcY, endX,
                        endY); // Homer class uses the A* to find least cost path to the player character.
                Grid.RenderNode(Grid.GridXOrg, Grid.GridYOrg, NpcX,
                    NpcY); // Renders grid node content for where NPC is moving from.
                NpcX = moveX;
                NpcY = moveY;
                GUI.CharWrite(Grid.GridXOrg + NpcX, Grid.GridYOrg + NpcY, NpcChar, FgColour,
                    BgColour); // Renders NPC in new location.
                NpcPwrLvl = Math.Max(NpcPwrLvl - 15, 0);
            }
        }

        // What does the NPC do when it finds the player character? Here a basic hard coded decision tree is used to determine what to do.
        public override string NpcFoundPlayer()
        {

            // If NPC has more power than the player power level, then 100% attack the player.
            if (NpcPwrLvl > Player.playerPwrLvl)
            {
                string result = NpcAttackPlayer();
                return result;
            }

            // If NPC has more power than 50% of the player power level, and the player power level is < 50% of the maximum player power level,
            // then 75% chance of attacking the player.
            if (NpcPwrLvl > Player.playerPwrLvl / 2 && Player.playerPwrLvl < Player.playerPwrMax / 2)
            {
                int attack75 = rand.Next(100);
                if (attack75 > 25)
                {
                    string result = NpcAttackPlayer();
                    return result;

                }
            }

            // If NPC has more power than 50% of the player power level, then 50% chance of attacking the player.
            if (NpcPwrLvl > Player.playerPwrLvl / 2)
            {
                int attack50 = rand.Next(100);
                if (attack50 > 50)
                {
                    string result = NpcAttackPlayer();
                    return result;

                }
            }

            // For all other scenarios the NPC will only risk an attack 25% of the time.
            int attack25 = rand.Next(100);
            if (attack25 > 75)
            {
                string result = NpcAttackPlayer();
                return result;

            }

            return "NPC did not attack.";
        }

        // Even when Homer attacks the player character, the results are not pre-defined.
        public override string NpcAttackPlayer()
        {
            int attackResult = rand.Next(3);
            int attackDamage = rand.Next(50);
            switch (attackResult)
            {
                case 0: // Player defends, Homer is damaged.
                    NpcPwrLvl = Math.Max(NpcPwrLvl - attackDamage, 0);
                    return "Player defended, Homer loss of " + attackDamage + " power.";
                case 1: // Player defends, no damage to either player or Homer.
                    return "Player defended, no damage to player or Homer.";
                case 2: // Homer wins attack, player is damaged.
                    Player.playerPwrLvl = Math.Max(Player.playerPwrLvl - attackDamage, 0);
                    return "Player suit damaged, loss of " + attackDamage + " power.";

            }
            return "No attack outcome.";
        }
    }
}