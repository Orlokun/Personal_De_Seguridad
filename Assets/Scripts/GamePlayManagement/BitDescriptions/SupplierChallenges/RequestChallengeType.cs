namespace GamePlayManagement.BitDescriptions.SupplierChallenges
{
    public enum RequestChallengeType
    {
        UseGuard =1,
        UseCamera,
        UseGuardItem,
        UseTrap,
        NotUseGuard,
        NotUseCamera,
        NotUserGuardItem,
        NotUseTrap
    }

    public enum RequestChallengeLogicOperator
    {
        BiggerThan,
        LesserThan,
        EqualTo,
        DifferentThan
    }

    public enum CharacterRaces
    {
        Human = 1,
        Elf = 2,
        Dwarf = 4,
        Orc = 8,
        Goblin = 16,
        Robot = 32,
        Alien = 64,
        Demon = 128,
        Angel = 256,
        Dragon = 512,
        Werewolf = 1024,
        Vampire = 2048,
        Ghost = 4096,
        Zombie = 8192,
    }

    //For now this enum encapsulates all types of parameters that might be considered for a challenge.
    //For example, race of a guard, or damage of an item. Be careful not to use a Guards parameter with a Cameras paramenter, for example. 
    public enum ConsideredParameter
    {
        //Common Parameters
        Price,

        //GuardsParameters
        Race,
        Intelligence,
        Kindness,
        Proactivity,
        Aggressive,
        Strength,
        Agility,
        Persuasion,
        Speed,
        
    }
}