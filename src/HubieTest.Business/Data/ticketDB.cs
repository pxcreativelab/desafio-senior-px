using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HubieTest.Dal;

namespace HubieTest.Business.Data
{
    /// <summary>
    /// Data access for TICKET, INTERACTION and ATTACHMENT.
    ///
    /// ========================= CANDIDATE AREA =========================
    /// Implement the methods below following the pattern shown in
    /// categoryDB.cs (open a DbContext in a using, turn off proxy/lazy,
    /// use EntityState for create/update). Feel free to adjust signatures
    /// if you think it is better — just explain your choices in the PR.
    /// ==================================================================
    /// </summary>
    public class ticketDB
    {
        // ---------- TICKET ----------

        /// <summary>Inserts a new ticket and returns the generated Id (Identity).</summary>
        public long create(TICKET ticket)
        {
            // HINT (Hubie pattern / ticketDB.create):
            //   using (var db = new HubieContext()) {
            //       db.Entry(ticket).State = EntityState.Added;
            //       db.SaveChanges();
            //       return ticket.TICKET_ID; // populated after SaveChanges
            //   }
            throw new NotImplementedException("TODO: create the ticket via EF and return TICKET_ID.");
        }

        /// <summary>Returns a ticket by id (or null).</summary>
        public TICKET get(long ticketId)
        {
            throw new NotImplementedException("TODO: load the ticket by TICKET_ID.");
        }

        /// <summary>Lists the tickets opened by a requester (most recent first).</summary>
        public List<TICKET> listByRequester(long requesterId)
        {
            throw new NotImplementedException("TODO: filter by REQUESTER_ID, order by TICKET_CREATED_DT desc.");
        }

        /// <summary>
        /// Agent queue. If <paramref name="status"/> is null/empty, return every
        /// ticket that is not closed yet.
        /// </summary>
        public List<TICKET> listQueue(string status)
        {
            throw new NotImplementedException("TODO: list tickets for the agent (optional status filter).");
        }

        /// <summary>Updates an existing ticket (status, agent, dates, etc.).</summary>
        public void update(TICKET ticket)
        {
            // HINT (Hubie pattern / ticketDB.update):
            //   db.Entry(ticket).State = EntityState.Modified; db.SaveChanges();
            throw new NotImplementedException("TODO: update the ticket via EF.");
        }

        // ---------- INTERACTION ----------

        public INTERACTION addInteraction(INTERACTION interaction)
        {
            throw new NotImplementedException("TODO: insert the interaction and return it with the generated id.");
        }

        public List<INTERACTION> listInteractions(long ticketId)
        {
            throw new NotImplementedException("TODO: list the ticket interactions in chronological order.");
        }

        // ---------- ATTACHMENT ----------

        public ATTACHMENT addAttachment(ATTACHMENT attachment)
        {
            throw new NotImplementedException("TODO: insert the attachment record and return it with the generated id.");
        }

        public List<ATTACHMENT> listAttachments(long ticketId)
        {
            throw new NotImplementedException("TODO: list the ticket attachments.");
        }
    }
}
