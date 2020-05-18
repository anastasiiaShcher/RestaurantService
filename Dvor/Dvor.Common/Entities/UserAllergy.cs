namespace Dvor.Common.Entities
{
    public class UserAllergy
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public string AllergyId { get; set; }

        public Allergy Allergy { get; set; }
    }
}