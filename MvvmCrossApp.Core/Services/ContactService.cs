using System.Collections.Generic;
using System.Linq;
using MvvmCrossApp.Core.Models;

namespace MvvmCrossApp.Core.Services
{
    public class ContactService : IContactService
    {
        public IEnumerable<Contact> GetContacts()
        {
            return Enumerable
                .Range(1, 20)
                .Select(i => new Contact {Id = i, Name = $"Contact no. {i}"});
        }
    }
}