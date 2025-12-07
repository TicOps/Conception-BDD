namespace Models
{
    public class User
    {
        public int Id { get; set; }              // sera dans la BDD plus tard
        public string Username { get; set; }
        public string Password { get; set; }     // à hasher plus tard, mais en projet console on fait simple
        public string Role { get; set; }         // "ADMIN" ou "MEMBER"
    }
}
