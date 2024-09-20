using RegistrationTelegramBot.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationTelegramBot.DL.Services
{
    public class ClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        // Create
        public async Task<Client> AddClient(Client client)
        {
            client.CreatedOn = DateTime.UtcNow.AddHours(3);
            _context.Client.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        // Read (Single Client by Id)
        public Client GetClientById(int id)
        {
            return _context.Client.Find(id);
        }

        // Read (All Clients)
        public async Task<List<Client>> GetAllClients()
        {
            return _context.Client.ToList();
        }

        // Update
        public void UpdateClient(Client client)
        {
            _context.Client.Update(client);
            _context.SaveChanges();
        }

        // Delete
        public void DeleteClient(int id)
        {
            var client = _context.Client.Find(id);
            if (client != null)
            {
                _context.Client.Remove(client);
                _context.SaveChanges();
            }
        }

        public async Task<Client> GetClientByTgIdAsync(string tgId)
        {
            var users = await GetAllClients();
            return users != null ? users.Find(user => user.TgId == tgId) : null;
        }
    }
}
