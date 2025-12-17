namespace Models
{
    public class User
    {
        public int Id { get; set; }   
        public string Username { get; set; }
        public string Password { get; set; }     // à hasher plus tard, mais en projet console on fait simple
        public string Role { get; set; }
    }
}
