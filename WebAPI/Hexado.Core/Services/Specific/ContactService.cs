using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Functional.Maybe;
using Hexado.Db.Entities;
using Hexado.Db.Repositories.Specific;

namespace Hexado.Core.Services.Specific
{
    public interface IContactService : IBaseService<Contact>
    {
        Task AddContactAsync(string id, string userEmail);
        Task<Maybe<IEnumerable<Contact>>> GetUserContacts(string userEmail);
        Task DeleteContact(string id, string userEmail);
    }

    public class ContactService : BaseService<Contact>, IContactService
    {
        private readonly IHexadoUserRepository _hexadoUserRepository;
        private readonly IContactRepository _contactRepository;

        public ContactService(
            IHexadoUserRepository hexadoUserRepository,
            IContactRepository contactRepository)
            : base(contactRepository)
        {
            _hexadoUserRepository = hexadoUserRepository;
            _contactRepository = contactRepository;
        }

        public async Task AddContactAsync(string id, string userEmail)
        {
            var loggedUser = await _hexadoUserRepository.GetSingleOrMaybeAsync(hu =>
                hu.Email == userEmail);
            var contactUser = await _hexadoUserRepository.GetAsync(id);

            if (!loggedUser.HasValue || !contactUser.HasValue)
                return;

            loggedUser.Value.Contacts.Add(new Contact
            {
                HexadoUserId = loggedUser.Value.Id,
                HexadoUser = loggedUser.Value,
                ContactHexadoUserId = contactUser.Value.Id,
                ContactHexadoUser = contactUser.Value
            });

            await _hexadoUserRepository.UpdateAsync(loggedUser.Value);
        }

        public async Task<Maybe<IEnumerable<Contact>>> GetUserContacts(string userEmail)
        {
            var loggedUser = await _hexadoUserRepository.GetSingleOrMaybeAsync(hu =>
                    hu.Email == userEmail,
                $"{nameof(HexadoUser.Contacts)}.{nameof(Contact.ContactHexadoUser)}");

            return loggedUser.HasValue 
                ? loggedUser.Value.Contacts.AsEnumerable().ToMaybe() 
                : Maybe<IEnumerable<Contact>>.Nothing;
        }

        public async Task DeleteContact(string id, string userEmail)
        {
            var loggedUser = await _hexadoUserRepository.GetSingleOrMaybeAsync(hu =>
                hu.Email == userEmail,
                hu => hu.Contacts);

            if (!loggedUser.HasValue)
                return;

            var contact = loggedUser.Value.Contacts.FirstOrDefault(c => c.Id == id).ToMaybe();
            if (!contact.HasValue)
                return;

            await _contactRepository.DeleteAsync(contact.Value);
        }
    }
}