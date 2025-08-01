using EnvironmentVariableExplorer.Data;
using EnvironmentVariableExplorer.Models;
using LiteDB;
using System.Threading.Tasks;

namespace EnvironmentVariableExplorer.Services
{
    public class UserPreferenceService
    {
        private readonly string _dbPath;
        private readonly string _collectionName = "preferences";

        public UserPreferenceService()
        {
            _dbPath = LiteDbContext.DbPath;
        }

        public async Task StoreUserPreferenceAsync(bool isDarkMode, string language)
        {
            await Task.Run(() =>
            {
                using (var db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<UserPreference>(_collectionName);
                    var userPref = collection.FindById("user1");

                    if (userPref == null)
                    {
                        userPref = new UserPreference
                        {
                            Id = "user1",
                            IsDarkMode = isDarkMode,
                            Language = language
                        };
                        collection.Insert(userPref);
                    }
                    else
                    {
                        userPref.IsDarkMode = isDarkMode;
                        userPref.Language = language;
                        collection.Update(userPref);
                    }
                }
            });
        }

        public async Task<bool> GetUserDarkModePreferenceAsync()
        {
            return await Task.Run(() =>
            {
                using (LiteDatabase db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<UserPreference>(_collectionName);
                    var userPref = collection.FindById("user1");

                    return userPref?.IsDarkMode ?? true;
                }
            });
        }

        public async Task<string> GetUserLanguagePreferenceAsync()
        {
            return await Task.Run(() =>
            {
                using (LiteDatabase db = new LiteDatabase(_dbPath))
                {
                    var collection = db.GetCollection<UserPreference>(_collectionName);
                    var userPref = collection.FindById("user1");

                    return userPref?.Language ?? "FR";
                }
            });
        }
    }
}
