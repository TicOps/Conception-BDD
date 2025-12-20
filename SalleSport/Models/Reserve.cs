namespace Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int MemberId { get; set; }         // FK vers Member
        public int CourseId { get; set; }         // FK vers Course
        public DateTime DateReservation { get; set; }
    }
}
