using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;

namespace VueTodoApi.Data
{
    public class NotesDocument
    {
        public int UserId { get; set; }
        public string Text { get; set; }
        public Guid NoteId { get; set; }
    }

    //internal interface INotesRepository
    //{
    //    Task DisableNotes(string registrationNumber, string countryCode, DateTimeOffset disabledOn);
    //    Task EnableNotes(string registrationNumber, string countryCode, DateTimeOffset nextNotesDate, DateTimeOffset enabledOn);
    //    Task<List<NotesDocument>> GetNotes(DateTimeOffset begin, DateTimeOffset end, DateTimeOffset notSentSince);
    //    Task UpdateNotifiedOn(string registrationNumber, string countryCode, DateTimeOffset notifiedOn);
    //}

    public class NotesRepository //: INotesRepository
    {
        private readonly Func<IAsyncDocumentSession> _getSession;

        public NotesRepository(Func<IAsyncDocumentSession> getSession) => _getSession = getSession;

        public async Task<List<NotesDocument>> GetNotes(int userId)
        {
            using (var session = _getSession())
            {
                var result = await session.Query<NotesDocument>()
                    .Where(document => document.UserId == userId)
                    .ToListAsync();

                return result;
            }
        }

        //public async Task DeleteNote(int userId, int noteId)
        //{
        //    using (var session = _getSession())
        //    {
        //        var doc = await session.LoadAsync<NotesDocument>($"{countryCode}/{registrationNumber}");
        //        if (doc == null) return;
        //        doc.Disabled = true;
        //        doc.DisabledOn = disabledOn;
        //        await session.SaveChangesAsync();
        //    }
        //}

        public async Task CreateNote(int userId, string text)
        {
            using (var session = _getSession())
            {
                var doc = new NotesDocument
                {
                    Text = text,
                    UserId = userId,
                    NoteId = Guid.NewGuid()
                };
                await session.StoreAsync(doc);
                await session.SaveChangesAsync();
            }
        }

        //public async Task UpdateNotifiedOn(string registrationNumber, string countryCode, DateTimeOffset notifiedOn)
        //{
        //    using (var session = _getSession())
        //    {
        //        var doc = await session.LoadAsync<NotesDocument>($"{countryCode}/{registrationNumber}");
        //        doc.NotifiedOn.Add(notifiedOn);
        //        await session.SaveChangesAsync();
        //    }
        //}
    }
}
