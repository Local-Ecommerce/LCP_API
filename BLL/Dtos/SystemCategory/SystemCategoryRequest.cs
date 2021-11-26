namespace BLL.Dtos.SystemCategory
{
    public class SystemCategoryRequest
    {
        public string SysCategoryName { get; set; }
    }

    public enum SystemCategoryStatus
    {
        ERROR = -1,
        SUCCESS = 0,
        SYSTEM_CATEGORY_NOT_FOUND = 1
    }
}
