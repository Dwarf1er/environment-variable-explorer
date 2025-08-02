using EnvironmentVariableExplorer.Data;
using EnvironmentVariableExplorer.Helpers;
using EnvironmentVariableExplorer.Models;
using LiteDB;
using System.Globalization;
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

        public async Task StoreUserPreferenceAsync(bool isDarkMode, CultureInfo language)
        {
            await Task.Run(() =>
            {
                using (LiteDatabase db = new LiteDatabase(_dbPath))
                {
                    ILiteCollection<UserPreference> collection = db.GetCollection<UserPreference>(_collectionName);
                    UserPreference userPreference = collection.FindById(SystemUtils.currentUserName);

                    if (userPreference == null)
                    {
                        userPreference = new UserPreference
                        {
                            Id = SystemUtils.currentUserName,
                            IsDarkMode = isDarkMode,
                            Language = language.Name
                        };
                        collection.Insert(userPreference);
                    }
                    else
                    {
                        userPreference.IsDarkMode = isDarkMode;
                        userPreference.Language = language.Name;
                        collection.Update(userPreference);
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
                    ILiteCollection<UserPreference> collection = db.GetCollection<UserPreference>(_collectionName);
                    UserPreference userPreference = collection.FindById(SystemUtils.currentUserName);

                    return userPreference?.IsDarkMode ?? true;
                }
            });
        }

        public async Task<CultureInfo> GetUserLanguagePreferenceAsync()
        {
            return await Task.Run(() =>
            {
                using (LiteDatabase db = new LiteDatabase(_dbPath))
                {
                    ILiteCollection<UserPreference> collection = db.GetCollection<UserPreference>(_collectionName);
                    UserPreference userPreference = collection.FindById(SystemUtils.currentUserName);

                    string languageCode = userPreference?.Language ?? "en-US";
                    return new CultureInfo(languageCode);
                }
            });
        }
    }
}
