namespace IonFiltra.BagFilters.Core.Entities.Users.UserRoles
{
    public class ApplicationRoles
    {
        public int RoleId { get; set; }
        
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
