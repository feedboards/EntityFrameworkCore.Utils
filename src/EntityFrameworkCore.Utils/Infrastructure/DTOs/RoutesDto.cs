namespace EntityFrameworkCore.Utils.Infrastructure.DTOs
{
    public class RoutesDto
    {
        public string? Root { get; set; }

        public string? SQL { get; set; }

        public string? Id { get; set; }
        public string? IdSoftDelete { get; set; }

        public string? Items { get; set; }
        public string? ItemsSoftDelete { get; set; }
    }
}
