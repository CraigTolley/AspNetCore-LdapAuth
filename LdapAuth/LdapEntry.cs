namespace TestAuth2Mvc.Identity.Models
{
    public class LdapEntry : ILdapEntry
    {
        public string ObjectSid { get; set; }
        public string ObjectGuid { get; set; }
        public string ObjectCategory { get; set; }
        public string ObjectClass { get; set; }
        public string Name { get; set; }
        public string CommonName { get; set; }
        public string DistinguishedName { get; set; }
        public string SamAccountName { get; set; }
        public int SamAccountType { get; set; }
        public string[] MemberOf { get; set; }
    }
}
