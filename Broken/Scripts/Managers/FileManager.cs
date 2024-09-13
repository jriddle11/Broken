using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using Broken.Scripts.Models;

namespace Broken
{
    /// <summary>
    /// File manager for the saving and retrieval of game data
    /// </summary>
    public static class FileManager
    {
        const string ROOM_CONFIGS_FILE_PATH = "C:/Users/Josh/source/repos/Broken/Broken/Data/Configurations/RoomConfigs.json";

        public static void AddToRoomConfigurations(RoomConfig room)
        {
            List<RoomConfig> roomConfigs;

            if (File.Exists(ROOM_CONFIGS_FILE_PATH))
            {
                string json = File.ReadAllText(ROOM_CONFIGS_FILE_PATH);
                roomConfigs = JsonSerializer.Deserialize<List<RoomConfig>>(json) ?? new List<RoomConfig>();
            }
            else
            {
                roomConfigs = new List<RoomConfig>();
            }

            var index = roomConfigs.FindIndex(r => r.ID == room.ID);
            if (index != -1)
            {
                roomConfigs[index] = room;
            }
            else
            {
                roomConfigs.Add(room);
            }

            string updatedJson = JsonSerializer.Serialize(roomConfigs, new JsonSerializerOptions());
            File.WriteAllText(ROOM_CONFIGS_FILE_PATH, updatedJson);
        }

        public static RoomConfig GetRoomConfigurationById(int id)
        {
            if (!File.Exists(ROOM_CONFIGS_FILE_PATH))
            {
                return null;
            }

            string json = File.ReadAllText(ROOM_CONFIGS_FILE_PATH);
            List<RoomConfig> roomConfigs = JsonSerializer.Deserialize<List<RoomConfig>>(json);

            if (roomConfigs == null)
            {
                return null;
            }

            RoomConfig roomConfig = roomConfigs.FirstOrDefault(rc => rc.ID == id);

            return roomConfig;
        }
    }
}
