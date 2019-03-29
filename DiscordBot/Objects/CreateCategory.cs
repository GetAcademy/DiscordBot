namespace DiscordBot.Objects
{
    /*
     * This Object is used to create a collection of channels with proper permissions under a given Category name
     */
    public class CreateCategory
    {
        private string _name;

        public CreateCategory(string inputName)
        {
            _name = inputName;
        }
    }
}