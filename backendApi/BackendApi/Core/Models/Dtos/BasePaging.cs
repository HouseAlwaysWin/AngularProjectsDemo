namespace BackendApi.Core.Models.Dtos
{
    public class BasePaging
    {
       public int PageIndex { get; set; } = 1;
       public int PageSize { get; set; } = 10;
       
    }
}