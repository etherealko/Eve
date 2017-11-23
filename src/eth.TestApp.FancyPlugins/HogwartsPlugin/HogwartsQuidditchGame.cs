using System;
using System.Collections.Generic;

namespace eth.TestApp.FancyPlugins.HogwartsPlugin
{
    public class HogwartsQuidditchGame
    {
        public enum SnitchCatchingResult
        {
            Failed,
            Success,
            OnCooldown
        }

        private Random Rnd { get; set; }

        public DateTime EndDate { get; private set; }
        public List<HogwartsHouse> Teams { get; private set; }
        public bool IsEnded { get; set; }

        private Dictionary<HogwartsMember, DateTime> LastAttemptToCatch { get; set; }

        private int ChanceForSnitch = 15;
        public int CatchCooldownInMinutes = 30;
        private int MatchDurationInHours = 12;

        public HogwartsQuidditchGame()
        {
            Rnd = new Random();
            SetTeams();
            LastAttemptToCatch = new Dictionary<HogwartsMember, DateTime>();
            EndDate = DateTime.Now.AddHours(MatchDurationInHours);
        }

        public SnitchCatchingResult TryCatchSnitch(HogwartsMember member)
        {
            if (LastAttemptToCatch.ContainsKey(member) && LastAttemptToCatch[member].AddMinutes(CatchCooldownInMinutes) > DateTime.Now)
            {
                return SnitchCatchingResult.OnCooldown;
            }
            else
            {
                var rnd = Rnd.Next(100);
                LastAttemptToCatch[member] = DateTime.Now;
                if (rnd < ChanceForSnitch)
                {
                    EndGame();
                    return SnitchCatchingResult.Success;
                }
            }

            return SnitchCatchingResult.Failed;
        }

        private void SetTeams()
        {
            var firstTeam = RollHouse();
            var secondTeam = RollHouse();
            while (secondTeam == firstTeam)
            {
                secondTeam = RollHouse();
            }

            Teams = new List<HogwartsHouse>()
            {
                firstTeam, secondTeam
            };
        }

        private HogwartsHouse RollHouse()
        {
            return (HogwartsHouse)Rnd.Next(4);
        }

        private void EndGame()
        {

        }
    }
}
