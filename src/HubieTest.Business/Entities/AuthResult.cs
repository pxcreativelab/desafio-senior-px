namespace HubieTest.Business.Entities
{
    /// <summary>Authentication result returned by the login flow.</summary>
    public class AuthResult
    {
        /// <summary>"OK" when authenticated; otherwise an error code.</summary>
        public string STATUS { get; set; }

        /// <summary>JWT (Bearer) — also returned in the X-User-Token header.</summary>
        public string TOKEN { get; set; }

        public long USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_LOGIN { get; set; }
        public string USER_PROFILE { get; set; }
        public string USER_EMAIL { get; set; }
    }
}
