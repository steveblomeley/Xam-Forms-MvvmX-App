using System.Collections.Generic;
using MvvmCrossApp.Core.Models;

namespace MvvmCrossApp.Core.Services
{
    public interface IContactService
    {
        IEnumerable<Contact> GetContacts();
    }
}