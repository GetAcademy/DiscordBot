using System.Collections.Generic;
using Discord;

namespace DiscordBot.Objects
{
    /*
     * This Object is used to create a collection of channels with proper permissions under a given Category name
     */
    public class CreateCategory
    {
        private string _name;
        private ICategoryChannel _category;
        private List<IChannel> _channels;

        public CreateCategory(string inputName)
        {
            _name = inputName;
        }

        public void AddChannel(ChannelType type, ICategoryChannel defaultChannel = null)
        {
            if (defaultChannel != null)
            {
                /*
                 * If the channel object/id is given, add a new channel to this object.
                 */
            }
        }

        public void Initialize()
        {
            /*
             * Pushes the category to the cannel
             */
            var server = Program.GetServer;
            //server.CreateCategoryChannelAsync(_name, properties => )
        }

        
    }
}