using System;
using System.Collections.Generic;
using HubieTest.Business.Data;
using HubieTest.Dal;

namespace HubieTest.Business
{
    /// <summary>
    /// Ticket business rules. Orchestrates ticketDB and applies the status
    /// transition rules, interaction authorship, etc.
    ///
    /// ========================= CANDIDATE AREA =========================
    /// Implement the rules below. The logged-in user (id/profile/name) is
    /// injected by the handler from the JWT — use it instead of trusting an
    /// id that comes from the request body.
    /// ==================================================================
    /// </summary>
    public class ticketBusiness
    {
        private readonly ticketDB _db = new ticketDB();

        // logged-in user context (set by the handler from the token)
        public long loggedUserId { get; set; }
        public string loggedUserName { get; set; }
        public string loggedUserProfile { get; set; }

        public bool hasError { get; set; }
        public string ErrorMessage { get; set; }

        // Valid ticket status values (suggestion).
        public const string STATUS_OPEN = "OPEN";
        public const string STATUS_IN_PROGRESS = "IN_PROGRESS";
        public const string STATUS_ANSWERED = "ANSWERED";
        public const string STATUS_CLOSED = "CLOSED";

        /// <summary>REQUESTER opens a new ticket. Returns the created ticket.</summary>
        public TICKET open(TICKET ticket)
        {
            // TODO:
            //  - validate title/description/category
            //  - fill REQUESTER_ID/NAME from the logged-in user
            //  - set TICKET_STATUS = STATUS_OPEN and TICKET_CREATED_DT
            //  - persist via _db.create(...) and return the object
            throw new NotImplementedException("TODO: open ticket.");
        }

        /// <summary>Lists the tickets of the logged-in requester.</summary>
        public List<TICKET> listMyTickets()
        {
            throw new NotImplementedException("TODO: list the logged-in requester's tickets.");
        }

        /// <summary>Service queue (AGENT view).</summary>
        public List<TICKET> listQueue(string status)
        {
            throw new NotImplementedException("TODO: list the agent queue.");
        }

        /// <summary>Ticket detail + interactions + attachments (to build the screen).</summary>
        public TICKET get(long ticketId)
        {
            throw new NotImplementedException("TODO: get the ticket (and related data, if you want).");
        }

        /// <summary>AGENT takes the ticket (status -> IN_PROGRESS).</summary>
        public void assign(long ticketId)
        {
            // TODO: validate AGENT profile, store AGENT_ID/NAME and change the status.
            throw new NotImplementedException("TODO: assign ticket.");
        }

        /// <summary>Changes the ticket status, respecting valid transitions.</summary>
        public void changeStatus(long ticketId, string newStatus)
        {
            throw new NotImplementedException("TODO: change status with transition validation.");
        }

        /// <summary>
        /// Adds a message to the ticket thread. Valid for BOTH profiles
        /// (requester and agent). Set authorship from the logged-in user.
        /// </summary>
        public INTERACTION addInteraction(long ticketId, string message)
        {
            throw new NotImplementedException("TODO: add interaction (authorship = logged-in user).");
        }

        public List<INTERACTION> listInteractions(long ticketId)
        {
            throw new NotImplementedException("TODO: list the ticket interactions.");
        }

        /// <summary>Registers an attachment already saved to disk by the upload handler.</summary>
        public ATTACHMENT registerAttachment(ATTACHMENT attachment)
        {
            throw new NotImplementedException("TODO: register the attachment metadata.");
        }

        public List<ATTACHMENT> listAttachments(long ticketId)
        {
            throw new NotImplementedException("TODO: list the ticket attachments.");
        }
    }
}
