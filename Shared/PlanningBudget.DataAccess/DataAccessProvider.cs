using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Windows.Storage;
using PlanningBudget.Models;
using Windows.UI;
using PlanningBudget.Core;
using System.Collections.ObjectModel;

namespace PlanningBudget.DataAccess
{
    public static class DataAccessProvider
    {
        public const string DbName = "budget.db";

        private static string CurrentProfileIdKey = "CurrentProfileId";

        public static Dictionary<Type, int> EventableTypes;

        static DataAccessProvider()
        {
            EventableTypes = new Dictionary<Type, int>()
            {
                {typeof(Profile), 0},
                {typeof(Account), 1},
                {typeof(Expense), 2},
                {typeof(Income), 3}
            };
        }


        public static async Task SetupDB()
        {
            if (!await IsDbExists())
            {
                await DataAccess.DataAccessProvider.CreateDB();
                await DataAccess.DataAccessProvider.FillDefaultCategories();
            }
        }

        public static async Task CreateDB()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            await conn.CreateTablesAsync(
                typeof(Profile),
                typeof(Account),
                typeof(Expense),
                typeof(Income),
                typeof(Transaction));
        }

        public static async Task<bool> IsDbExists()
        {
            bool result = true;
            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(DbName);
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static async Task FillDefaultCategories()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);

            string defaultAccount = "account1";

            await conn.InsertAsync(
                new Profile()
                {
                    Name = defaultAccount,
                    Color = 0xffff0000,
                    Icon = "A"
                });

            int defaultProfileId =
                (await conn.Table<Profile>().Where(i => i.Name == defaultAccount).ToListAsync()).Last().Id;

            CurrentProfileId = defaultProfileId;

            await conn.InsertAsync(
                new Account()
                {
                    Name = "Wallet",
                    ProfileId = defaultProfileId,
                    Balance = 100,
                    Color = 0xff49C5FF,
                    Icon = "W"
                });

            int defaultAccountId =
                (await conn.Table<Account>().Where(i => i.Name == "Wallet").ToListAsync()).Last().Id;

            await conn.InsertAsync(
                new Income()
                {
                    Name = "Salary",
                    ProfileId = defaultProfileId,
                    Icon = "\uE13F",
                    Color = ColorTools.ColorToArgb(Colors.LimeGreen)
                });

            await conn.InsertAllAsync(new List<Expense>()
            {
                new Expense()
                {
                    Name = "Home",
                    Budget = 20,
                    ProfileId = defaultProfileId,
                    Icon="\uE10F",
                    Color = ColorTools.ColorToArgb(Colors.DeepSkyBlue)
                },
                new Expense()
                {
                    Name = "Food",
                    Budget = 30,
                    ProfileId = defaultProfileId,
                    Icon="\uE168",
                    Color = ColorTools.ColorToArgb(Colors.MediumVioletRed)
                },
                new Expense()
                {
                    Name = "Clothes",
                    Budget = 15,
                    ProfileId = defaultProfileId,
                    Icon="\uE1CB",
                    Color = ColorTools.ColorToArgb(Colors.Gold)
                },
                new Expense()
                {
                    Name = "Phone",
                    Budget = 3,
                    ProfileId = defaultProfileId,
                    Icon="\uE13A",
                    Color = ColorTools.ColorToArgb(Colors.BlueViolet)
                }
            });
        }

        public static async Task<List<T>> GetAll<T>() where T : ItemWithNameColorAndIcon, new()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<T>();
            return await query.ToListAsync();
        }

        public static async Task<bool> AddCategory<T>(T category)
            where T : ItemWithNameColorAndIcon, new()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            if (await GetCategoryByName<T>(category.Name) == null)
            {
                await conn.InsertAsync(category);
                OnCategoryAdded(category);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<T> GetCategoryByName<T>(string name) where
            T : ItemWithNameColorAndIcon, new()
        {
            if (name == null)
            {
                return null;
            }

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<T>().Where(i => i.Name == name);
            var result = await query.ToListAsync();
            return result.Count > 0 ? (T)result[0] : null;
        }

        public static async Task<T> GetCategoryById<T>(int id) where
            T : ItemWithNameColorAndIcon, new()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<T>().Where(i => i.Id == id);
            return await query.FirstOrDefaultAsync();
        }

        public static async Task<bool> UpdateCategory<T>(T category) where
            T : ItemWithNameColorAndIcon, new()
        {
            //var similar = await GetCategoryByName<T>(category.Name);
            //if (similar != null && similar.Id == category.Id)
            //{
            //    return false;
            //}

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            await conn.UpdateAsync(category);
            OnCategoryChanged(category);

            return true;
        }

        public static async Task<List<Transaction>> GetAllTransactions()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<Transaction>();
            return await query.ToListAsync();
        }


        public static async void AddTransaction(Transaction tr)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            await conn.InsertAsync(tr);
            var account = await conn.Table<Account>().Where(i => i.Id == tr.AccountId).FirstOrDefaultAsync();
            account.Balance -= tr.Amount;
            await conn.UpdateAsync(account);
            OnAccountChanged(new CategoryAddedEventArgs<Account>(account));
            OnTransactionAdded(tr);
        }

        private static async Task<Transaction> GetTransactionByName(string name)
        {
            if (name == null)
            {
                return null;
            }

            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var query = conn.Table<Transaction>().Where(i => i.Name == name);
            var result = await query.ToListAsync();
            return result.Count > 0 ? (Transaction)result[0] : null;
        }

        private static void OnCategoryChanged(object category)
        {
            //      Profile 0
            //      Account 1
            //      Expense 2
            //      Income  3
            switch (EventableTypes[category.GetType()])
            {
                case 0:
                    OnProfileChanged(new CategoryAddedEventArgs<Profile>(category as Profile));
                    break;
                case 1:
                    OnAccountChanged(new CategoryAddedEventArgs<Account>(category as Account));
                    break;
                case 2:
                    OnExpenseChanged(new CategoryAddedEventArgs<Expense>(category as Expense));
                    break;
                //case 3:
                //    OnIncomeChanged(new CategoryAddedEventArgs<Income>(category as Income));
                //    break;
            }
        }

        private static void OnCategoryAdded(object category)
        {
            switch (EventableTypes[category.GetType()])
            {
                case 0:
                    OnProfileAdded(new CategoryAddedEventArgs<Profile>(category as Profile));
                    break;
                case 1:
                    OnAccountAdded(new CategoryAddedEventArgs<Account>(category as Account));
                    break;
                case 2:
                    OnExpenseAdded(new CategoryAddedEventArgs<Expense>(category as Expense));
                    break;
                //case 3:
                //    OnIncomeAdded(new CategoryAddedEventArgs<Income>(category as Income));
                //    break;
            }
        }

        private static void OnCategoryDeleted(object category)
        {
            switch (EventableTypes[category.GetType()])
            {
                case 0:
                    OnProfileDeleted(new CategoryAddedEventArgs<Profile>(category as Profile));
                    break;
                case 1:
                    OnAccountDeleted(new CategoryAddedEventArgs<Account>(category as Account));
                    break;
                case 2:
                    OnExpenseDeleted(new CategoryAddedEventArgs<Expense>(category as Expense));
                    break;
                //case 3:
                //    OnIncomeDeleted(new CategoryAddedEventArgs<Income>(category as Income));
                //    break;
            }
        }

        public static async Task<bool> DeleteCategoryByName<T>(string categoryName) where
            T : ItemWithNameColorAndIcon, new()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(DbName);
            var category = await GetCategoryByName<T>(categoryName);
            if (category != null)
            {
                await conn.DeleteAsync(category);
                OnCategoryDeleted(category);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static event EventHandler<CategoryAddedEventArgs<Profile>> ProfileAdded;

        public static event EventHandler<CategoryAddedEventArgs<Profile>> ProfileChanged;

        public static event EventHandler<CategoryAddedEventArgs<Profile>> ProfileDeleted;

        public static event EventHandler<CategoryAddedEventArgs<Account>> AccountAdded;

        public static event EventHandler<CategoryAddedEventArgs<Account>> AccountChanged;

        public static event EventHandler<CategoryAddedEventArgs<Account>> AccountDeleted;

        public static event EventHandler<CategoryAddedEventArgs<Expense>> ExpenseAdded;

        public static event EventHandler<CategoryAddedEventArgs<Expense>> ExpenseChanged;

        public static event EventHandler<CategoryAddedEventArgs<Expense>> ExpenseDeleted;

        public static event EventHandler<CategoryAddedEventArgs<Income>> IncomeAdded;

        public static event EventHandler<CategoryAddedEventArgs<Income>> IncomeChanged;

        public static event EventHandler<CategoryAddedEventArgs<Income>> IncomeDeleted;

        public static event EventHandler<Transaction> TransactionAdded;

        private static void OnProfileAdded(CategoryAddedEventArgs<Profile> e)
        {
            if (ProfileAdded != null)
                ProfileAdded(null, e);
        }

        private static void OnProfileChanged(CategoryAddedEventArgs<Profile> e)
        {
            if (ProfileChanged != null)
                ProfileChanged(null, e);
        }

        private static void OnProfileDeleted(CategoryAddedEventArgs<Profile> e)
        {
            if (ProfileDeleted != null)
                ProfileDeleted(null, e);
        }

        private static void OnAccountAdded(CategoryAddedEventArgs<Account> e)
        {
            if (AccountAdded != null)
                AccountAdded(null, e);
        }

        private static void OnAccountChanged(CategoryAddedEventArgs<Account> e)
        {
            if (AccountChanged != null)
                AccountChanged(null, e);
        }

        private static void OnAccountDeleted(CategoryAddedEventArgs<Account> e)
        {
            if (AccountDeleted != null)
                AccountDeleted(null, e);
        }

        private static void OnExpenseAdded(CategoryAddedEventArgs<Expense> e)
        {
            if (ExpenseAdded != null)
                ExpenseAdded(null, e);
        }

        private static void OnExpenseChanged(CategoryAddedEventArgs<Expense> e)
        {
            if (ExpenseChanged != null)
                ExpenseChanged(null, e);
        }

        private static void OnExpenseDeleted(CategoryAddedEventArgs<Expense> e)
        {
            if (ExpenseDeleted != null)
                ExpenseDeleted(null, e);
        }

        private static void OnTransactionAdded(Transaction e)
        {
            if (TransactionAdded != null)
                TransactionAdded(null, e);
        }

        public static int CurrentProfileId
        {
            get { return (Windows.Storage.ApplicationData.Current.LocalSettings.Values[CurrentProfileIdKey] as int?).Value; }
            set { Windows.Storage.ApplicationData.Current.LocalSettings.Values[CurrentProfileIdKey] = value; }
        }

        public static System.Collections.ObjectModel.ObservableCollection<string> GetIcons()
        {
            ObservableCollection<string> Icons = new ObservableCollection<string>();
            for (int i = 0xE000; i < 0xE300; i += 8)
            {
                Icons.Add(((char)i).ToString());
            }
            return Icons;

        }
    }
}
