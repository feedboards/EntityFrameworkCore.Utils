namespace EntityFrameworkCore.Utils.DTOs.Response
{
    public class DeleteResponseDto
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class DeleteResponseDto<T>
    {
        public T? Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
