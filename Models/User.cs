using System.Collections.Concurrent;
using System.Net.Mail;

namespace UserManagementAPI.Models
{
    public class User
    {
        private static readonly ConcurrentDictionary<int, User> _users = new();

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        public static IEnumerable<User> GetAll() => _users.Values;

        public static User? Get(int id) => _users.GetValueOrDefault(id);

        public static User Add(User user)
        {
            if (user.Id == 0 || _users.ContainsKey(user.Id))
            {
                throw new ArgumentException("Invalid or duplicate Id");
            }
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("Name cannot be null or empty");
            }
            ValidateEmail(user.Email);
            _users[user.Id] = user;
            return user;
        }

        public static bool Update(int id, User user)
        {
            if (!_users.ContainsKey(id))
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new ArgumentException("Name cannot be null or empty");
            }
            ValidateEmail(user.Email);
            user.Id = id;
            _users[id] = user;
            return true;
        }

        public static bool Delete(int id) => _users.TryRemove(id, out _);

        private static void ValidateEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email format");
            }
        }
    }
}