
using AdApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdApp.Models;

namespace AdApp.Core.Helpers.User
{
    public class UserHelper:IUserHelper
    {
        public UserHelper()
        {
            // Define the folder path
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DATA");

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Combine the folder path with the file name
            string filePath = Path.Combine(folderPath, "users.json");

            // Create the file if it does not exist (initialize with an empty array)
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]");
            }
            _filePath = filePath;

        }
        private readonly JsonStorage _jsonStorage = new JsonStorage();
        private readonly string _filePath;
        public List<Models.User> GetAllUsers()
        {
            var users = _jsonStorage.Load<List<Models.User>>(_filePath);
            return users ?? new List<Models.User>();
        }
    }
}
