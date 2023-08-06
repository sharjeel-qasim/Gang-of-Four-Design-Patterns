namespace GoFDesignPatterns.Creational
{
    public class SingletonDesignPattern
    {
        // The private static instance variable to hold the single instance.
        private static SingletonDesignPattern instance;

        // Private constructor to prevent external instantiation.
        private SingletonDesignPattern() {}

        // Public static method to get the single instance of the class.
        public static SingletonDesignPattern GetInstance()
        {
            // If the instance is null, create a new instance.
            // Otherwise, return the existing instance.

            if (instance == null)
            {
                instance = new SingletonDesignPattern();
            }

            return instance;
        }
    }
}