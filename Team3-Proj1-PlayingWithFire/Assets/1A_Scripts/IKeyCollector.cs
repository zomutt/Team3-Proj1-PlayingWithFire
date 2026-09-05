namespace _1A_Scripts
{
    // Lets Keys.cs hand a picked-up key off to whichever level's puzzle manager is actually in the scene.
    public interface IKeyCollector
    {
        void CollectKey(string color);
    }
}
