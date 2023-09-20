using System.ComponentModel.Design;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //Platforms
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExist(int PlatformId);
        bool ExternalPlatformExists(int externalPlatformId);
        
        //Commands
        IEnumerable<Command> GetCommandsForPlatform (int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}